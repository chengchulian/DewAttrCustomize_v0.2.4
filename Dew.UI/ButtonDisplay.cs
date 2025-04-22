using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonDisplay : MonoBehaviour, ISettingsChangedCallback
{
	[Serializable]
	public struct MouseButtonEntry
	{
		public MouseButton button;

		public GameObject obj;
	}

	[Serializable]
	public struct GamepadIconEntry
	{
		public GamepadButtonForDisplay button;

		public GameObject obj;
	}

	public enum GamepadButtonForDisplay
	{
		None = -1,
		DpadUp = 0,
		DpadDown = 1,
		DpadLeft = 2,
		DpadRight = 3,
		North = 4,
		Triangle = 4,
		Y = 4,
		B = 5,
		Circle = 5,
		East = 5,
		A = 6,
		Cross = 6,
		South = 6,
		Square = 7,
		West = 7,
		X = 7,
		LeftStick = 8,
		RightStick = 9,
		LeftShoulder = 10,
		RightShoulder = 11,
		Start = 12,
		Select = 13,
		LeftTrigger = 32,
		RightTrigger = 33,
		Dpad = 255,
		LeftStickAxis = 256,
		RightStickAxis = 257,
		LeftStickUp = 1000,
		LeftStickDown = 1001,
		LeftStickLeft = 1002,
		LeftStickRight = 1003,
		RightStickUp = 1010,
		RightStickDown = 1011,
		RightStickLeft = 1012,
		RightStickRight = 1013
	}

	private static DewBinding _tempBinding = new DewBinding
	{
		canAssignGamepad = true,
		canAssignKeyboard = true,
		canAssignMouse = true
	};

	[SerializeField]
	private bool _isDisabled;

	public bool showWithSettingsKey = true;

	public string settingsKey;

	public MouseButton mouseButton;

	public GamepadButtonForDisplay gamepadButton = GamepadButtonForDisplay.None;

	public Key key;

	public bool allowMultipleIcons;

	[NonSerialized]
	public DewBinding binding;

	private bool _isInvalid;

	[Space(20f)]
	public ButtonDisplay_IconData icons;

	public float disabledAlpha;

	public GameObject mockObject;

	private CanvasGroup _cg;

	private List<GameObject> _instantiated = new List<GameObject>();

	public bool isDisabled
	{
		get
		{
			return _isDisabled;
		}
		set
		{
			_isDisabled = value;
			UpdateDisabledStatus();
		}
	}

	private void OnValidate()
	{
		if (!showWithSettingsKey)
		{
			_isInvalid = false;
			return;
		}
		string[] array = settingsKey.Split(",");
		_isInvalid = false;
		string[] array2 = array;
		foreach (string k in array2)
		{
			FieldInfo field = typeof(DewControlSettings).GetField(k);
			if (field == null || field.FieldType != typeof(DewBinding))
			{
				_isInvalid = true;
				break;
			}
		}
	}

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
		UpdateDisabledStatus();
		if (mockObject != null)
		{
			global::UnityEngine.Object.Destroy(mockObject);
		}
	}

	private void OnEnable()
	{
		UpdateButtonDisplay();
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Combine(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	private void OnCurrentModeChanged(InputMode arg1, InputMode arg2)
	{
		UpdateButtonDisplay();
	}

	private void OnDisable()
	{
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Remove(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	private void UpdateDisabledStatus()
	{
		if (_cg == null)
		{
			_cg = GetComponent<CanvasGroup>();
		}
		_cg.alpha = (isDisabled ? disabledAlpha : 1f);
	}

	public void OnSettingsChanged()
	{
		UpdateButtonDisplay();
	}

	public void UpdateButtonDisplay()
	{
		foreach (GameObject g in _instantiated)
		{
			if (g != null)
			{
				global::UnityEngine.Object.Destroy(g);
			}
		}
		_instantiated.Clear();
		if (!showWithSettingsKey)
		{
			DewBinding val = _tempBinding;
			val.pcBinds.Clear();
			val.gamepadBinds.Clear();
			InputMode mode;
			if (binding != null)
			{
				mode = DewInput.currentMode;
				val = binding;
			}
			else if (mouseButton != 0)
			{
				mode = InputMode.KeyboardAndMouse;
				val.pcBinds.Add(mouseButton);
			}
			else if (key != 0)
			{
				mode = InputMode.KeyboardAndMouse;
				val.pcBinds.Add(key);
			}
			else if (gamepadButton != GamepadButtonForDisplay.None)
			{
				mode = InputMode.Gamepad;
				val.gamepadBinds.Add((GamepadButtonEx)gamepadButton);
			}
			else
			{
				mode = InputMode.KeyboardAndMouse;
				val = DewBinding.MockBinding;
			}
			TryInstantiateForBinding(val, mode);
		}
		else
		{
			string[] array = settingsKey.Split(",");
			InputMode mode = DewInput.currentMode;
			string[] array2 = array;
			foreach (string k in array2)
			{
				DewBinding val = (DewBinding)DewSave.profile.controls.GetSettingsValue(k);
				TryInstantiateForBinding(val, mode);
				if (_instantiated.Count > 0 && !allowMultipleIcons)
				{
					break;
				}
			}
		}
		if (_instantiated.Count == 0)
		{
			InstantiateIcon(icons.none);
		}
	}

	private void TryInstantiateForBinding(DewBinding val, InputMode mode)
	{
		if (val == null)
		{
			return;
		}
		if (mode == InputMode.KeyboardAndMouse)
		{
			foreach (PCBind pcBind in val.pcBinds)
			{
				PCBind b = pcBind;
				if (_instantiated.Count > 0)
				{
					InstantiateIcon(icons.or);
				}
				if (b.key != 0)
				{
					InstantiateModifiers();
					InstantiateIcon(icons.keyboardGeneric).GetComponentInChildren<TextMeshProUGUI>().text = b.key.GetReadableText();
				}
				else if (b.mouse != 0)
				{
					GameObject obj = null;
					for (int j = 0; j < icons.mouse.Length; j++)
					{
						if (icons.mouse[j].button == b.mouse)
						{
							obj = icons.mouse[j].obj;
							break;
						}
					}
					if (obj != null)
					{
						InstantiateModifiers();
						InstantiateIcon(obj);
					}
				}
				if (!allowMultipleIcons)
				{
					break;
				}
				void InstantiateModifiers()
				{
					foreach (Key m in b.modifiers)
					{
						InstantiateIcon(icons.keyboardGeneric).GetComponentInChildren<TextMeshProUGUI>().text = m.GetReadableText();
						InstantiateIcon(icons.plus);
					}
				}
			}
			return;
		}
		foreach (GamepadButtonEx b2 in val.gamepadBinds)
		{
			if (_instantiated.Count > 0)
			{
				InstantiateIcon(icons.or);
			}
			GameObject obj2 = null;
			for (int i = 0; i < icons.gamepad.Length; i++)
			{
				if (icons.gamepad[i].button == (GamepadButtonForDisplay)b2)
				{
					obj2 = icons.gamepad[i].obj;
					break;
				}
			}
			if (obj2 != null)
			{
				InstantiateIcon(obj2);
			}
			else
			{
				InstantiateIcon(icons.gamepadGeneric).GetComponentInChildren<TextMeshProUGUI>().text = b2.GetReadableText();
			}
			if (!allowMultipleIcons)
			{
				break;
			}
		}
	}

	private GameObject InstantiateIcon(GameObject icon)
	{
		GameObject obj = global::UnityEngine.Object.Instantiate(icon, base.transform);
		_instantiated.Add(obj);
		return obj;
	}
}
