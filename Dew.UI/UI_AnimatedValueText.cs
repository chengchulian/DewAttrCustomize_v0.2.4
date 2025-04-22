using System;
using TMPro;
using UnityEngine;

public abstract class UI_AnimatedValueText : LogicBehaviour
{
	private TextMeshProUGUI _text;

	public bool flashOnChange;

	public Color flashColor;

	public float colorSmoothTime = 0.5f;

	public float valueSmoothTime = 0.3f;

	public float valueLinearSpeed = 1f;

	public string valueFormat = "###,0";

	private Color _originalColor;

	private Vector4 _colorVel;

	private float _lastRealValue;

	private float _currentDisplayedValue;

	private float _currentValueVelocity;

	protected virtual void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
		_originalColor = _text.color;
	}

	private void Start()
	{
		_lastRealValue = GetValue();
		_currentDisplayedValue = GetValue();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_lastRealValue = GetValue();
		_currentDisplayedValue = GetValue();
	}

	protected abstract float GetValue();

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		float currentRealValue = GetValue();
		if (Math.Abs(_lastRealValue - currentRealValue) > 0.01f)
		{
			_lastRealValue = currentRealValue;
			if (flashOnChange)
			{
				_text.color = flashColor;
				_colorVel = Vector4.zero;
			}
		}
		_currentDisplayedValue = Mathf.MoveTowards(_currentDisplayedValue, currentRealValue, valueLinearSpeed * (1f / 30f));
		_currentDisplayedValue = Mathf.SmoothDamp(_currentDisplayedValue, currentRealValue, ref _currentValueVelocity, valueSmoothTime, float.PositiveInfinity, 1f / 30f);
		_text.text = _currentDisplayedValue.ToString(valueFormat);
		_text.color = ColorSmoothDamp.SmoothDamp(_text.color, _originalColor, ref _colorVel, colorSmoothTime, float.PositiveInfinity, 1f / 30f);
	}
}
