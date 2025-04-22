using UnityEngine;
using UnityEngine.Events;
using viperTools;

namespace viperOSK;

public class OSK_GamepadHelper : MonoBehaviour
{
	public OSK_Keyboard keyboard;

	public OSK_Receiver receiver;

	public GameObject selectionMarker;

	public int gamepadNum;

	private Vector2 joy;

	public bool allowRepeatButton;

	public bool invertY;

	protected OSK_Key selectedKey;

	public float inputReactiveness;

	private float t;

	private float tBtn;

	private bool aBtnPressed;

	private bool bBtnPressed;

	private bool active = true;

	private bool connected;

	public UnityEvent onActivate;

	public UnityEvent onDeactivate;

	public void GamepadPrep()
	{
		if (keyboard == null)
		{
			keyboard = Object.FindObjectOfType<OSK_Keyboard>();
			if (keyboard == null)
			{
				Debug.LogError("keyboard needs to be assigned an OSK_Keyboard, you can do this in the inspector");
			}
			else
			{
				keyboard.bypassDefaultInput = true;
			}
		}
		else
		{
			keyboard.bypassDefaultInput = true;
		}
		if (receiver == null)
		{
			receiver = Object.FindObjectOfType<OSK_Receiver>();
			if (receiver == null)
			{
				Debug.Log("OSK_Receiver needs to be assigned if you want to use separate input fields than the one attached to the keyboard");
			}
		}
		connected = true;
		if (gamepadNum < 0 || viperInput.NumControllers() < gamepadNum)
		{
			Debug.LogError("Note enough controllers connected. This controller #" + gamepadNum + " is not in the " + viperInput.NumControllers() + "  gamepad/joysticks connected");
			active = false;
			connected = false;
		}
		if (selectedKey == null)
		{
			selectedKey = keyboard.gameObject.GetComponentInChildren<OSK_Key>();
		}
	}

	public OSK_Key GetSelectedKey()
	{
		return selectedKey;
	}

	public void SetSelectedKey(OSK_Key k)
	{
		selectedKey = k;
	}

	public void SetSelectedKey(string k)
	{
		selectedKey = keyboard.GetOSKKey(k);
	}

	public void Activate()
	{
		active = true;
		onActivate.Invoke();
	}

	public void DeActivate()
	{
		active = false;
		onDeactivate.Invoke();
	}

	private void Start()
	{
		GamepadPrep();
	}

	private Vector2 JoystickInput()
	{
		return viperInput.GetPlayerJoystickInput(gamepadNum);
	}

	private bool JoystickButtonA()
	{
		return viperInput.GetPlayerAButton(gamepadNum);
	}

	private bool JoystickButtonB()
	{
		return viperInput.GetPlayerBButton(gamepadNum);
	}

	private void FixedUpdate()
	{
		if (!active || !connected)
		{
			return;
		}
		t += Time.fixedDeltaTime;
		if (!(t > inputReactiveness))
		{
			return;
		}
		t = 0f;
		joy = JoystickInput();
		if (Mathf.Abs(joy.x) > 0.15f || Mathf.Abs(joy.y) > 0.15f)
		{
			if (invertY)
			{
				joy.y = 0f - joy.y;
			}
			selectedKey = keyboard.SelectedKeyMove(joy, selectedKey.GetLayoutLocation());
		}
		if (selectionMarker != null)
		{
			selectionMarker.transform.position = selectedKey.GetKeyTransform().position;
		}
		else
		{
			keyboard.SetSelectedKey(selectedKey.key);
		}
	}

	private void Update()
	{
		if (!active || !connected)
		{
			return;
		}
		if (allowRepeatButton)
		{
			tBtn += Time.deltaTime;
			if (tBtn > inputReactiveness * 2f)
			{
				tBtn = 0f;
				aBtnPressed = false;
				bBtnPressed = false;
			}
		}
		if (JoystickButtonA())
		{
			if (!aBtnPressed)
			{
				aBtnPressed = true;
				if (selectedKey != null)
				{
					selectedKey.Click(receiver);
				}
			}
		}
		else
		{
			aBtnPressed = false;
		}
		if (JoystickButtonB())
		{
			if (!bBtnPressed)
			{
				bBtnPressed = true;
				OSK_Key back = keyboard.GetOSKKey("Backspace");
				if (back != null)
				{
					back.Click(receiver);
				}
			}
		}
		else
		{
			bBtnPressed = false;
		}
	}
}
