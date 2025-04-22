using TMPro;
using UnityEngine;
using viperOSK;
using viperTools;

namespace viperOSK_Examples;

public class Example6_A_Gamepad : MonoBehaviour
{
	public OSK_Keyboard keyboard;

	public OSK_GamepadHelper player1;

	public OSK_GamepadHelper player2;

	public ParticleSystem particle;

	public TMP_Text message;

	private bool isWarning;

	public int currentPlayer = 1;

	public float inputReactiveness;

	private float t;

	private Vector2 joy;

	public void NextPlayer()
	{
		if ((currentPlayer == 1 && player1.receiver.Text().Length > 0) || (currentPlayer == 2 && player2.receiver.Text().Length > 0))
		{
			currentPlayer = ((currentPlayer != 1) ? 1 : 2);
			if (currentPlayer == 1)
			{
				PlayerOne();
			}
			else
			{
				PlayerTwo();
			}
		}
		else
		{
			if (currentPlayer == 1)
			{
				particle.transform.position = player1.receiver.transform.position;
			}
			else
			{
				particle.transform.position = player2.receiver.transform.position;
			}
			particle.Emit(1);
		}
	}

	public void PlayerOne()
	{
		keyboard.highlighterColor = new Color(240f, 0f, 0f);
		player1.Activate();
		player2.DeActivate();
		particle.transform.position = player1.receiver.transform.position;
		particle.Emit(10);
	}

	public void PlayerTwo()
	{
		keyboard.highlighterColor = new Color(0f, 240f, 0f);
		player1.DeActivate();
		player2.Activate();
		particle.transform.position = player2.receiver.transform.position;
		particle.Emit(10);
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
	}

	private void Start()
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
		joy = default(Vector2);
		string[] joysticks = viperInput.GetControllerNames();
		Debug.Log("num of joysticks:" + joysticks.Length);
		string[] array = joysticks;
		foreach (string n in array)
		{
			Debug.Log("joy:" + n);
		}
		if (joysticks.Length < 2)
		{
			player2.DeActivate();
			Invoke("TwoControllerWarning", 1f);
		}
		PlayerOne();
	}

	private void FixedUpdate()
	{
	}

	private void Update()
	{
	}
}
