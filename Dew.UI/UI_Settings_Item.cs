using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Settings_Item : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ILangaugeChangedCallback, IGamepadFocusable, IGamepadFocusListener, IGamepadFocusableOverrideInput
{
	public enum CategoryType
	{
		Gameplay,
		Controls,
		Graphics,
		AudioUser,
		AudioPlatform,
		GameplayPlatform
	}

	public CategoryType type;

	public string key;

	public DewLocalizedText nameText;

	public int maxPrecision = 2;

	public float sliderGamepadStepSize = 0.1f;

	public bool dontOverrideInput;

	protected FieldInfo _field;

	protected List<object> _dropdownValues;

	public UI_Settings_View parent { get; private set; }

	private void Awake()
	{
		parent = GetComponentInParent<UI_Settings_View>();
	}

	public void Init()
	{
		PopulateFieldInfoIfDidnt();
		OnSetup();
		nameText.key = $"Settings_{type}_{key}";
		nameText.UpdateText();
	}

	private void OnEnable()
	{
		ManagerBase<GlobalUIManager>.instance.AddGamepadFocusable(this);
	}

	private void OnDisable()
	{
		if (ManagerBase<GlobalUIManager>.instance != null)
		{
			ManagerBase<GlobalUIManager>.instance.RemoveGamepadFocusable(this);
		}
	}

	private void PopulateFieldInfoIfDidnt()
	{
		switch (type)
		{
		case CategoryType.Gameplay:
			_field = typeof(DewGameplaySettings_User).GetField(key);
			break;
		case CategoryType.Controls:
			_field = typeof(DewControlSettings).GetField(key);
			break;
		case CategoryType.Graphics:
			_field = typeof(DewGraphicsSettings).GetField(key);
			break;
		case CategoryType.AudioUser:
			_field = typeof(DewAudioSettings_User).GetField(key);
			break;
		case CategoryType.AudioPlatform:
			_field = typeof(DewAudioSettings_Platform).GetField(key);
			break;
		case CategoryType.GameplayPlatform:
			_field = typeof(DewGameplaySettings_Platform).GetField(key);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	protected virtual void OnSetup()
	{
		Slider slider = GetComponentInChildren<Slider>();
		if (slider != null)
		{
			slider.onValueChanged.AddListener(delegate(float val)
			{
				SetValue(val);
			});
		}
		UI_Toggle toggle = GetComponentInChildren<UI_Toggle>();
		if (toggle != null)
		{
			toggle.onIsCheckedChanged.AddListener(delegate(bool b)
			{
				SetValue(b);
			});
		}
		TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();
		if (dropdown != null)
		{
			dropdown.onValueChanged.AddListener(delegate(int b)
			{
				SetValue(_dropdownValues[b]);
			});
			OnLanguageChanged();
		}
	}

	public virtual void OnLoad()
	{
		if (GetSettingsObject() == null || _field == null)
		{
			return;
		}
		Slider slider = GetComponentInChildren<Slider>();
		if (slider != null)
		{
			if (GetValue() is int i)
			{
				slider.value = i;
			}
			else
			{
				slider.value = (float)GetValue();
			}
		}
		UI_Toggle toggle = GetComponentInChildren<UI_Toggle>();
		if (toggle != null)
		{
			toggle.isChecked = (bool)GetValue();
		}
		TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();
		if (!(dropdown != null) || !_field.FieldType.IsEnum)
		{
			return;
		}
		object val = GetValue();
		for (int j = 0; j < _dropdownValues.Count; j++)
		{
			if (_dropdownValues[j].Equals(val))
			{
				dropdown.value = j;
				break;
			}
		}
	}

	protected object GetSettingsObject()
	{
		return type switch
		{
			CategoryType.Gameplay => parent.gameplayUser, 
			CategoryType.Controls => parent.controls, 
			CategoryType.Graphics => parent.graphics, 
			CategoryType.AudioUser => parent.audioUser, 
			CategoryType.AudioPlatform => parent.audioPlatform, 
			CategoryType.GameplayPlatform => parent.gameplayPlatform, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	protected object GetValue()
	{
		return _field.GetValue(GetSettingsObject());
	}

	protected virtual void SetValue(object value)
	{
		if (!value.Equals(GetValue()))
		{
			if (value is float f)
			{
				float powered = Mathf.Pow(10f, maxPrecision);
				value = Mathf.Round(f * powered) / powered;
			}
			if (_field.FieldType == typeof(int))
			{
				_field.SetValue(GetSettingsObject(), Mathf.RoundToInt(float.Parse(value.ToString())));
			}
			else
			{
				_field.SetValue(GetSettingsObject(), value);
			}
			parent.MarkAsDirty();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (_field != null && _field.FieldType.IsEnum)
		{
			parent.ShowTooltip(type, key, _field.FieldType.Name);
		}
		else
		{
			parent.ShowTooltip(type, key);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		parent.HideTooltip();
	}

	public virtual void OnLanguageChanged()
	{
		TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();
		if (dropdown == null || _field == null)
		{
			return;
		}
		_dropdownValues = new List<object>();
		int val = dropdown.value;
		dropdown.ClearOptions();
		if (_field.FieldType.IsEnum)
		{
			Array values = Enum.GetValues(_field.FieldType);
			List<string> list = new List<string>();
			foreach (object v in values)
			{
				list.Add(DewLocalization.GetUIValue("Settings_" + _field.FieldType.Name + "_" + Enum.GetName(_field.FieldType, v)));
				_dropdownValues.Add(v);
			}
			dropdown.AddOptions(list);
		}
		dropdown.value = val;
	}

	public virtual void LoadDefaults()
	{
		object defaultObject = Activator.CreateInstance(GetSettingsObject().GetType());
		try
		{
			if (defaultObject is IInitializableSettings s)
			{
				s.Initialize();
			}
			if (defaultObject is IValidatableSettings ss)
			{
				ss.Validate();
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		SetValue(_field.GetValue(defaultObject));
		OnLoad();
	}

	public void OnFocusStateChanged(bool state)
	{
		if (state)
		{
			Dew.CallDelayed(delegate
			{
				OnPointerEnter(null);
			});
		}
	}

	public bool OnGamepadDpadLeft()
	{
		if (dontOverrideInput)
		{
			return false;
		}
		Slider slider = GetComponentInChildren<Slider>();
		if (slider != null)
		{
			slider.value -= sliderGamepadStepSize;
		}
		return true;
	}

	public bool OnGamepadDpadRight()
	{
		if (dontOverrideInput)
		{
			return false;
		}
		Slider slider = GetComponentInChildren<Slider>();
		if (slider != null)
		{
			slider.value += sliderGamepadStepSize;
		}
		return true;
	}

	public bool OnGamepadConfirm()
	{
		UI_Toggle toggle = GetComponentInChildren<UI_Toggle>();
		if (toggle != null)
		{
			toggle.isChecked = !toggle.isChecked;
		}
		TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();
		if (dropdown != null)
		{
			dropdown.Show();
		}
		return true;
	}
}
