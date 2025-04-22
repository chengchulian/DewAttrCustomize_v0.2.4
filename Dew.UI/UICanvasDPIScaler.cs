using System;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasDPIScaler : MonoBehaviour
{
	public float scaleFactor = 1f;

	private CanvasScaler _scaler;

	private void Awake()
	{
		_scaler = GetComponent<CanvasScaler>();
		if (_scaler == null)
		{
			base.enabled = false;
			throw new Exception("Cannot find CanvasScaler in '" + base.name + "'");
		}
		CheckDPI();
	}

	private void FixedUpdate()
	{
		CheckDPI();
	}

	private void CheckDPI()
	{
		float newFactor = Screen.dpi / 96f * scaleFactor;
		if (Math.Abs(_scaler.scaleFactor - newFactor) > 0.001f)
		{
			_scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
			_scaler.scaleFactor = newFactor;
		}
	}
}
