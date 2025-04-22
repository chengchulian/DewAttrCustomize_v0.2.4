using System;
using UnityEngine;

public class UI_PlayTutorial_TutorialCanvas : MonoBehaviour
{
	private Canvas _canvas;

	private void Start()
	{
		_canvas = GetComponent<Canvas>();
		InGameUIManager instance = InGameUIManager.instance;
		instance.onStateChanged = (Action<string, string>)Delegate.Combine(instance.onStateChanged, new Action<string, string>(OnStateChanged));
	}

	private void OnStateChanged(string arg1, string arg2)
	{
		_canvas.enabled = arg2 == "Playing";
	}
}
