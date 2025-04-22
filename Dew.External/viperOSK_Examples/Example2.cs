using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using viperOSK;

namespace viperOSK_Examples;

public class Example2 : MonoBehaviour
{
	public SpriteRenderer mySpeechBubble;

	public TextMeshPro mySpeech;

	public TextMeshPro NPCSpeech;

	public OSK_Keyboard keyboard;

	private bool contextShift;

	private Vector3 current;

	private Vector3 target;

	private float t;

	private void Start()
	{
		if (Camera.main.aspect < 1.6777778f)
		{
			Camera.main.orthographicSize = 5.5f;
		}
	}

	public void Submit(string text)
	{
		if (text.Equals("switch", StringComparison.InvariantCultureIgnoreCase))
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			Debug.Log("current scene = " + currentSceneIndex + " / " + SceneManager.sceneCountInBuildSettings);
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
		Debug.Log(text);
		mySpeech.text = text + "<-";
		mySpeechBubble.color = Color.cyan;
		if (text.ToLowerInvariant().Contains("hello") || text.ToLower().Contains("hi"))
		{
			NPCSpeech.text = "Hi friend";
		}
		else
		{
			NPCSpeech.text = "Aha";
		}
		keyboard.gameObject.SetActive(value: false);
		Invoke("Clear", 5f);
		current = Camera.main.gameObject.transform.position;
		target = current + new Vector3(0f, 3.5f, 0f);
		contextShift = true;
		t = 0f;
	}

	private void Clear()
	{
		mySpeech.text = "";
		mySpeechBubble.color = Color.white;
		NPCSpeech.text = "Hey there!";
		keyboard.gameObject.SetActive(value: true);
		current = Camera.main.gameObject.transform.position;
		target = current + new Vector3(0f, -3.5f, 0f);
		contextShift = true;
		t = 0f;
	}

	private void Update()
	{
		if (contextShift)
		{
			t += Time.deltaTime * 3f;
			Camera.main.gameObject.transform.position = Vector3.Lerp(current, target, t);
			if (t >= 1f)
			{
				Camera.main.gameObject.transform.position = target;
				contextShift = false;
			}
		}
	}
}
