using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class UI_Settings_BindingWindow : SingletonBehaviour<UI_Settings_BindingWindow>
{
	public TextMeshProUGUI targetText;

	public TextMeshProUGUI keyText;

	private DewBinding _binding;

	private Action _onFinish;

	private Action _onCancel;

	private BindingType _allowedTypes;

	public void Show(string targetName, DewBinding newBinding, BindingType allowedTypes, Action onFinish, Action onCancel)
	{
		allowedTypes &= newBinding.GetBindingTypes();
		base.gameObject.SetActive(value: false);
		targetText.text = targetName;
		_onFinish = onFinish;
		_onCancel = onCancel;
		_binding = newBinding;
		_allowedTypes = allowedTypes;
		DewInput.ListenAnyButtonRaw(this, delegate(InputControl ctrl)
		{
			if (ctrl.noisy)
			{
				return false;
			}
			if (allowedTypes.HasFlag(BindingType.Mouse) && ctrl is DeltaControl deltaControl)
			{
				if (_binding.pcBinds.Count != 1)
				{
					_binding.pcBinds.Clear();
					_binding.pcBinds.Add(new PCBind());
				}
				if (DewInput.ModifierKeys.Contains(_binding.pcBinds[0].key) && !_binding.pcBinds[0].modifiers.Contains(_binding.pcBinds[0].key))
				{
					_binding.pcBinds[0].modifiers.Add(_binding.pcBinds[0].key);
					_binding.pcBinds[0].key = Key.None;
				}
				else
				{
					_binding.pcBinds[0].modifiers.Clear();
					_binding.pcBinds[0].key = Key.None;
				}
				_binding.pcBinds[0].mouse = ((deltaControl.value.y > 0f) ? MouseButton.ScrollUp : MouseButton.ScrollDown);
				UpdateText();
				return false;
			}
			if (!(ctrl is ButtonControl buttonControl))
			{
				return false;
			}
			if (allowedTypes.HasFlag(BindingType.Keyboard) && ctrl is KeyControl keyControl)
			{
				if (keyControl.isPressed)
				{
					return false;
				}
				Key keyCode = keyControl.keyCode;
				if (keyCode == Key.LeftMeta || keyCode == Key.RightMeta)
				{
					return false;
				}
				if (_binding.pcBinds.Count != 1)
				{
					_binding.pcBinds.Clear();
					_binding.pcBinds.Add(new PCBind());
				}
				_binding.pcBinds[0].mouse = MouseButton.None;
				if (_binding.pcBinds[0].key == Key.None || _binding.pcBinds[0].key == keyControl.keyCode || _binding.pcBinds[0].modifiers.Contains(keyControl.keyCode))
				{
					_binding.pcBinds[0].modifiers.Clear();
				}
				else if (DewInput.ModifierKeys.Contains(_binding.pcBinds[0].key) && !_binding.pcBinds[0].modifiers.Contains(_binding.pcBinds[0].key))
				{
					_binding.pcBinds[0].modifiers.Add(_binding.pcBinds[0].key);
				}
				else
				{
					_binding.pcBinds[0].modifiers.Clear();
				}
				_binding.pcBinds[0].key = keyControl.keyCode;
			}
			if (allowedTypes.HasFlag(BindingType.Mouse) && ctrl.device is Mouse && DewInput.NameToMouseButton.TryGetValue(buttonControl.name, out var value))
			{
				if (value == MouseButton.Left)
				{
					return false;
				}
				if (buttonControl.isPressed)
				{
					return false;
				}
				if (_binding.pcBinds.Count != 1)
				{
					_binding.pcBinds.Clear();
					_binding.pcBinds.Add(new PCBind());
				}
				if (DewInput.ModifierKeys.Contains(_binding.pcBinds[0].key) && !_binding.pcBinds[0].modifiers.Contains(_binding.pcBinds[0].key))
				{
					_binding.pcBinds[0].modifiers.Add(_binding.pcBinds[0].key);
					_binding.pcBinds[0].key = Key.None;
				}
				else
				{
					_binding.pcBinds[0].modifiers.Clear();
					_binding.pcBinds[0].key = Key.None;
				}
				_binding.pcBinds[0].mouse = value;
			}
			if (allowedTypes.HasFlag(BindingType.Gamepad) && ctrl.device is Gamepad)
			{
				foreach (GamepadButton gamepadButton in Enum.GetValues(typeof(GamepadButton)))
				{
					if (ctrl == Gamepad.current[gamepadButton])
					{
						_binding.gamepadBinds.Clear();
						_binding.gamepadBinds.Add((GamepadButtonEx)gamepadButton);
						break;
					}
				}
			}
			UpdateText();
			return false;
		});
		base.gameObject.SetActive(value: true);
		UpdateText();
	}

	private void UpdateText()
	{
		keyText.text = DewInput.GetReadableTextOfPC(_binding);
	}

	public void DoLeftClick()
	{
		if (_binding.pcBinds.Count != 1)
		{
			_binding.pcBinds.Clear();
			_binding.pcBinds.Add(new PCBind());
		}
		_binding.pcBinds[0].mouse = MouseButton.Left;
		if (DewInput.ModifierKeys.Contains(_binding.pcBinds[0].key))
		{
			_binding.pcBinds[0].modifiers.Add(_binding.pcBinds[0].key);
			_binding.pcBinds[0].key = Key.None;
		}
		else
		{
			_binding.pcBinds[0].modifiers.Clear();
			_binding.pcBinds[0].key = Key.None;
		}
		UpdateText();
	}

	public void Clear()
	{
		if (_allowedTypes.HasFlag(BindingType.Gamepad))
		{
			_binding.gamepadBinds.Clear();
		}
		if (_allowedTypes.HasFlag(BindingType.Keyboard) || _allowedTypes.HasFlag(BindingType.Mouse))
		{
			_binding.pcBinds.Clear();
		}
		UpdateText();
	}

	public void Confirm()
	{
		try
		{
			_onFinish?.Invoke();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		base.gameObject.SetActive(value: false);
	}

	public void Cancel()
	{
		base.gameObject.SetActive(value: false);
		try
		{
			_onCancel?.Invoke();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	private void OnEnable()
	{
		ManagerBase<GlobalUIManager>.instance.isBackDisabled = true;
	}

	private void OnDisable()
	{
		ManagerBase<GlobalUIManager>.instance.isBackDisabled = false;
		DewInput.StopListenAnyButton();
	}
}
