using TMPro;
using UnityEngine;
using UnityEngine.Events;
using viperOSK;

namespace viperOSK_Examples;

public class Example6_B_Gamepad : MonoBehaviour
{
	public OSK_Keyboard keyboard;

	public OSK_Receiver receiver1;

	public OSK_Receiver receiver2;

	public ParticleSystem particle;

	public TMP_Text message;

	private bool isWarning;

	public AudioClip successSound;

	public AudioClip invalidSound;

	public UnityEvent<int> onSuccessEntry;

	public UnityEvent<int> onFailEntry;

	public void PlayerOneSubmit()
	{
		if (receiver1.Text().Length > 0)
		{
			particle.transform.position = receiver1.transform.position;
			particle.Emit(10);
			onSuccessEntry.Invoke(1);
		}
		else
		{
			particle.transform.position = receiver1.transform.position;
			particle.Emit(1);
			onFailEntry.Invoke(1);
		}
	}

	public void PlayerTwoSubmit()
	{
		if (receiver2.Text().Length > 0)
		{
			particle.transform.position = receiver2.transform.position;
			particle.Emit(10);
			onSuccessEntry.Invoke(2);
		}
		else
		{
			particle.transform.position = receiver2.transform.position;
			particle.Emit(1);
			onFailEntry.Invoke(2);
		}
	}

	public void SuccessEntry(int playernum)
	{
		if (playernum == 1)
		{
			receiver1.OnFocusLost();
		}
		if (playernum == 2)
		{
			receiver2.OnFocusLost();
		}
		if (!receiver1.isFocused() && !receiver2.isFocused())
		{
			receiver1.ClearText();
			receiver2.ClearText();
			receiver1.OnFocus();
			receiver2.OnFocus();
		}
	}

	public void FailDing()
	{
		GetComponent<AudioSource>().PlayOneShot(invalidSound);
	}

	public void SuccessDing()
	{
		GetComponent<AudioSource>().PlayOneShot(successSound);
	}

	public void TwoControllerWarning()
	{
		if (isWarning)
		{
			message.fontStyle = FontStyles.Normal;
			message.color = Color.white;
			isWarning = false;
		}
		else
		{
			isWarning = true;
			message.fontStyle = FontStyles.Bold;
			message.color = Color.red;
			Invoke("TwoControllerWarning", 3f);
		}
		Debug.Log("twocontroler");
	}

	private void Start()
	{
		if (keyboard == null)
		{
			Debug.LogError("keyboard needs to be assigned an OSK_Keyboard, you can do this in the inspector");
		}
		string[] joysticks = Input.GetJoystickNames();
		Debug.Log("num of joysticks:" + joysticks.Length);
		string[] array = joysticks;
		foreach (string n in array)
		{
			Debug.Log("joy:" + n);
		}
		if (joysticks.Length < 2)
		{
			Invoke("TwoControllerWarning", 1f);
		}
		Invoke("InputFieldsFocus", 1f);
	}

	private void InputFieldsFocus()
	{
		receiver1.OnFocus();
		receiver2.OnFocus();
	}
}
