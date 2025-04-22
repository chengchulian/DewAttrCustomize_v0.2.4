using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using viperOSK;

namespace viperOSK_Examples;

public class Example1 : MonoBehaviour
{
	public TextMeshPro outputtext;

	public List<OSK_Keyboard> keyboards = new List<OSK_Keyboard>();

	public viperOSK_SimpleButton upBtn;

	public viperOSK_SimpleButton downBtn;

	private int currentKeyboard;

	private float t;

	private Vector3 start;

	private Vector3 end;

	private void Start()
	{
		t = -1f;
		start = Camera.main.transform.position;
		if (upBtn != null)
		{
			upBtn.Enable(enabled: false);
		}
		else
		{
			Debug.Log("Please set the Up Button in inspector");
		}
		if (downBtn == null)
		{
			Debug.Log("Please set the Down Button in inspector");
		}
		for (int i = 1; i < keyboards.Count; i++)
		{
			keyboards[i].HasFocus(isFocus: false);
		}
	}

	public void MoveUp()
	{
		keyboards[currentKeyboard].HasFocus(isFocus: false);
		currentKeyboard--;
		currentKeyboard = Mathf.Clamp(currentKeyboard, 0, keyboards.Count - 1);
		keyboards[currentKeyboard].HasFocus(isFocus: true);
		SetMoveCoord();
	}

	public void MoveDown()
	{
		keyboards[currentKeyboard].HasFocus(isFocus: false);
		currentKeyboard++;
		currentKeyboard = Mathf.Clamp(currentKeyboard, 0, keyboards.Count - 1);
		keyboards[currentKeyboard].HasFocus(isFocus: true);
		SetMoveCoord();
	}

	public void SetMoveCoord()
	{
		if (currentKeyboard == 0)
		{
			upBtn.Enable(enabled: false);
		}
		else
		{
			upBtn.Enable(enabled: true);
		}
		if (currentKeyboard == keyboards.Count - 1)
		{
			downBtn.Enable(enabled: false);
		}
		else
		{
			downBtn.Enable(enabled: true);
		}
		t = 0f;
		end = keyboards[currentKeyboard].transform.position;
		end.y += 2f;
		end.z = start.z;
	}

	public void LoadEmailLayout()
	{
		string layout = "1 2 3 4 5 6 7 8 9 0 \n Q W E R T Y U I O P \n A S D F G H J K L \" Skip.2 Backspace \n CapsLock Z X C V B N M & \n LeftShift # ' - _ @ . Skip.2 Return";
		if (keyboards.Count >= 2)
		{
			keyboards[1].LoadLayout(layout);
		}
	}

	public void SubmitText(string s)
	{
		if (s.Equals("switch", StringComparison.InvariantCultureIgnoreCase))
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
			Debug.Log("TEXT SUBMITTED: " + s);
			outputtext.text = s;
		}
	}

	private void Update()
	{
		if (t >= 0f && keyboards.Count > 0)
		{
			t += Time.deltaTime;
			Camera.main.transform.position = Vector3.Slerp(start, end, t);
			if (t >= 1f)
			{
				t = -1f;
				start = Camera.main.transform.position;
			}
		}
	}
}
