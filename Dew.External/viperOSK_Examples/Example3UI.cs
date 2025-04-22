using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using viperOSK;

namespace viperOSK_Examples;

public class Example3UI : MonoBehaviour
{
	private Vector2 pointerPos;

	public GameObject cube;

	private Vector3 cubeRotation;

	public OSK_UI_Keyboard keyboard;

	public OSK_Receiver field;

	public TMP_Text output1;

	public TMP_Text output2;

	public TMP_Text output3;

	public TMP_InputField infield;

	public void OnSubmit(string text)
	{
		if (text.Equals("switch", StringComparison.InvariantCultureIgnoreCase))
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
			return;
		}
		if (keyboard.output.name.Equals("UI_Input_Text"))
		{
			output1.text = text;
		}
		if (keyboard.output.name.Equals("OSK_InputField (TMP)"))
		{
			output2.text = text;
		}
		if (keyboard.output.name.Equals("OSK_InputField"))
		{
			output3.text = text;
		}
		RandomizeCubeRotation();
	}

	private void Awake()
	{
	}

	private void Start()
	{
		RandomizeCubeRotation();
	}

	public void Hello()
	{
		field.SetText("HELLO123");
		Debug.Log("HELLO");
	}

	private void RandomizeCubeRotation()
	{
		Vector3 rot1 = new Vector3(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
		cubeRotation = rot1;
		Time.timeScale = Mathf.Abs(1f - Time.timeScale);
		Debug.Log("timescale=" + Time.timeScale);
	}

	private void Update()
	{
		if (cube != null)
		{
			cube.transform.Rotate(cubeRotation * 15f * Time.deltaTime, Space.World);
		}
	}
}
