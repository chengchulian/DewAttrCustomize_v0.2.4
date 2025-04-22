using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Settings_Item_DewBinding_BindItem : LogicBehaviour, ILangaugeChangedCallback, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private DewBinding _value;

	public BindingType allowedTypes;

	public GameObject hoverObject;

	public GameObject notHoverObject;

	public Action<object> onValueChanged;

	public TextMeshProUGUI keyDisplayText;

	public DewBinding value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
			UpdateKeyDisplayText();
			onValueChanged?.Invoke(_value);
		}
	}

	public void StartListening()
	{
		DewBinding newBinding = (DewBinding)value.Clone();
		string targetName = DewLocalization.GetUIValue(GetComponentInParent<UI_Settings_Item>().nameText.key);
		SingletonBehaviour<UI_Settings_BindingWindow>.instance.Show(targetName, newBinding, allowedTypes & value.GetBindingTypes(), delegate
		{
			value = newBinding;
		}, null);
	}

	public void OnLanguageChanged()
	{
		UpdateKeyDisplayText();
	}

	private void UpdateKeyDisplayText()
	{
		if (_value != null)
		{
			keyDisplayText.text = (allowedTypes.HasFlag(BindingType.Gamepad) ? DewInput.GetReadableTextOfGamepad(_value) : DewInput.GetReadableTextOfPC(_value));
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (hoverObject != null)
		{
			hoverObject.SetActive(value: true);
		}
		if (notHoverObject != null)
		{
			notHoverObject.SetActive(value: false);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (hoverObject != null)
		{
			hoverObject.SetActive(value: false);
		}
		if (notHoverObject != null)
		{
			notHoverObject.SetActive(value: true);
		}
	}

	public void UpdateValueDontNotify(DewBinding newReference)
	{
		_value = newReference;
		UpdateKeyDisplayText();
	}
}
