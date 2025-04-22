using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace viperTools;

public class viperInput : MonoBehaviour
{
	public void Start()
	{
	}

	public static void RegisterKeyStrokeCallback(Action<char> action, bool enable)
	{
		if (enable && Keyboard.current != null && action != null)
		{
			Keyboard.current.onTextInput -= action;
			Keyboard.current.onTextInput += action;
		}
		else
		{
			Keyboard.current.onTextInput -= action;
		}
	}

	public static Key ConvertKeyCodeToKey(KeyCode k)
	{
		return k switch
		{
			KeyCode.Return => Key.Enter, 
			KeyCode.KeypadEnter => Key.NumpadEnter, 
			KeyCode.CapsLock => Key.CapsLock, 
			KeyCode.LeftShift => Key.LeftShift, 
			KeyCode.RightShift => Key.RightShift, 
			_ => (Key)Enum.Parse(typeof(Key), k.ToString()), 
		};
	}

	public static bool KeyDown(KeyCode k)
	{
		Key c = ConvertKeyCodeToKey(k);
		return Keyboard.current[c].wasPressedThisFrame;
	}

	public static bool KeyUp(KeyCode k)
	{
		Key c = ConvertKeyCodeToKey(k);
		return Keyboard.current[c].wasPressedThisFrame;
	}

	public static bool KeyPress(KeyCode k)
	{
		Key c = ConvertKeyCodeToKey(k);
		return Keyboard.current[c].IsPressed();
	}

	public static bool PointerDown(int mouseBtn = 0)
	{
		return Pointer.current.press.isPressed;
	}

	public static bool PointerUp(int mouseBtn = 0)
	{
		return Pointer.current.press.wasPressedThisFrame;
	}

	public static bool Fire1()
	{
		return Gamepad.current.buttonSouth.wasPressedThisFrame;
	}

	public static bool AnyPhysicalKey()
	{
		return Keyboard.current.anyKey.wasPressedThisFrame;
	}

	public static string GetPhysicalKey()
	{
		return Input.inputString;
	}

	public static string ConvertToLegacyAxis(AXIS_INPUT axis)
	{
		return axis switch
		{
			AXIS_INPUT.DPAD_X => "Horizontal", 
			AXIS_INPUT.DPAD_Y => "Vertical", 
			AXIS_INPUT.LEFTSTICK_X => "Horizontal", 
			AXIS_INPUT.LEFTSTICK_Y => "Vertical", 
			AXIS_INPUT.RIGHTSTICK_X => "Horizontal", 
			AXIS_INPUT.RIGHTSTICK_Y => "Vertical", 
			_ => "Horizontal", 
		};
	}

	public static string[] GetControllerNames()
	{
		string[] s = new string[Gamepad.all.Count];
		for (int i = 0; i < Gamepad.all.Count; i++)
		{
			s[i] = Gamepad.all[i].displayName;
		}
		return s;
	}

	public static float GetAllAxis()
	{
		return Gamepad.current.leftStick.ReadValue().sqrMagnitude + Gamepad.current.dpad.ReadValue().sqrMagnitude;
	}

	public static float GetAxis(AXIS_INPUT axis)
	{
		switch (axis)
		{
		case AXIS_INPUT.DPAD_X:
			return (Gamepad.current.dpad.right.isPressed ? 1f : 0f) + (Gamepad.current.dpad.left.isPressed ? (-1f) : 0f);
		case AXIS_INPUT.DPAD_Y:
			return (Gamepad.current.dpad.up.isPressed ? 1f : 0f) + (Gamepad.current.dpad.down.isPressed ? (-1f) : 0f);
		case AXIS_INPUT.LEFTSTICK_X:
			if (!(Mathf.Abs(Gamepad.current.leftStick.ReadValue().x) >= 0.9f))
			{
				return 0f;
			}
			return Mathf.Sign(Gamepad.current.leftStick.ReadValue().x);
		case AXIS_INPUT.LEFTSTICK_Y:
			if (!(Mathf.Abs(Gamepad.current.leftStick.ReadValue().y) >= 0.9f))
			{
				return 0f;
			}
			return Mathf.Sign(Gamepad.current.leftStick.ReadValue().y);
		case AXIS_INPUT.RIGHTSTICK_X:
			return Gamepad.current.rightStick.ReadValue().x;
		case AXIS_INPUT.RIGHTSTICK_Y:
			return Gamepad.current.rightStick.ReadValue().y;
		default:
			return (Gamepad.current.dpad.right.isPressed ? 1f : 0f) + (Gamepad.current.dpad.left.isPressed ? (-1f) : 0f);
		}
	}

	public static Vector2 GetPlayerJoystickInput(int p)
	{
		p = Mathf.Clamp(p - 1, 0, Gamepad.all.Count);
		Vector2 v = default(Vector2);
		v.x = (Gamepad.all[p].dpad.right.isPressed ? 1f : 0f) + (Gamepad.all[p].dpad.left.isPressed ? (-1f) : 0f);
		v.y = (Gamepad.all[p].dpad.up.isPressed ? 1f : 0f) + (Gamepad.all[p].dpad.down.isPressed ? (-1f) : 0f);
		v.x += Gamepad.all[p].leftStick.ReadValue().x;
		v.y += Gamepad.all[p].leftStick.ReadValue().y;
		return v;
	}

	public static bool GetPlayerAButton(int p)
	{
		p = Mathf.Clamp(p - 1, 0, Gamepad.all.Count);
		if (Gamepad.all[p] == null)
		{
			return Gamepad.current.buttonSouth.wasPressedThisFrame;
		}
		return Gamepad.all[p].buttonSouth.wasPressedThisFrame;
	}

	public static bool GetPlayerBButton(int p)
	{
		p = Mathf.Clamp(p - 1, 0, Gamepad.all.Count);
		if (Gamepad.all[p] == null)
		{
			return Gamepad.current.buttonEast.wasPressedThisFrame;
		}
		return Gamepad.all[p].buttonEast.wasPressedThisFrame;
	}

	public static int NumControllers()
	{
		return Gamepad.all.Count;
	}

	public static void ResetAllAxis()
	{
		Input.ResetInputAxes();
	}

	public static Vector2 GetPointerPos()
	{
		Vector2 pos = Vector2.zero;
		if (Pointer.current != null)
		{
			pos = Pointer.current.position.ReadValue();
		}
		return pos;
	}
}
