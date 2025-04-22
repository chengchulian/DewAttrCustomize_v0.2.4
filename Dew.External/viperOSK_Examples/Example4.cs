using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using viperOSK;

namespace viperOSK_Examples;

public class Example4 : MonoBehaviour
{
	public List<OSK_Receiver> inputFields;

	private int currentField;

	public OSK_Keyboard keyboard;

	private string username;

	private string password;

	public TextMeshPro usernameTMP;

	public TextMeshPro pwdTMP;

	private void Start()
	{
		username = "";
		password = "";
	}

	public void TabKey()
	{
		if (currentField == 0 && username.Length == 0)
		{
			username = inputFields[currentField].Text();
		}
		currentField++;
		currentField %= inputFields.Count;
		keyboard.SetOutput(inputFields[currentField]);
	}

	public void UsernameSubmit(string txt)
	{
		if (txt == null || txt.Length == 0)
		{
			return;
		}
		username = txt;
		if (txt.Equals("switch", StringComparison.InvariantCultureIgnoreCase))
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
			{
				SceneManager.LoadScene(currentSceneIndex + 1);
			}
			else
			{
				SceneManager.LoadScene(0);
			}
		}
		else
		{
			TabKey();
		}
	}

	public void PasswordSubmit(string txt)
	{
		if (txt != null && txt.Length != 0)
		{
			password = txt;
			if (inputFields[0].Text().Length == 0)
			{
				usernameTMP.text = "[missing username]";
				return;
			}
			username = inputFields[0].Text();
			usernameTMP.text = username;
			pwdTMP.text = password;
			inputFields[0].ClearText();
			inputFields[1].ClearText();
			TabKey();
			Invoke("Cleanup", 10f);
		}
	}

	private void Cleanup()
	{
		usernameTMP.text = "";
		pwdTMP.text = "";
	}

	private void Update()
	{
	}
}
