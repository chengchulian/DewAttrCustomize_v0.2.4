using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Common_SkipButton : MonoBehaviour
{
	public UnityEvent onSkip;

	public float holdDuration = 0.75f;

	public float showAnimationTime = 0.25f;

	public float keepShownDuration = 1f;

	public float hideAnimationTime = 1f;

	public Image fill;

	private DewInputTrigger it_skip;

	private CanvasGroup _cg;

	private float _lastTouchTime = float.NegativeInfinity;

	private float _holdAmount;

	private float _cv;

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
		it_skip = new DewInputTrigger
		{
			owner = this,
			priority = -100,
			binding = () => DewSave.profile.controls.skip,
			isValidCheck = () => base.gameObject.activeInHierarchy
		};
	}

	private void OnEnable()
	{
		fill.fillAmount = 0f;
		_holdAmount = 0f;
		_lastTouchTime = float.NegativeInfinity;
		_cg.alpha = 0f;
	}

	private void Update()
	{
		fill.fillAmount = Mathf.SmoothDamp(fill.fillAmount, _holdAmount, ref _cv, 0.05f, float.PositiveInfinity, Time.unscaledDeltaTime);
		float elapsedTime = Time.unscaledTime - _lastTouchTime;
		if (elapsedTime < showAnimationTime + keepShownDuration)
		{
			_cg.alpha = Mathf.MoveTowards(_cg.alpha, 1f, 1f / showAnimationTime * Time.unscaledDeltaTime);
		}
		else if (elapsedTime > showAnimationTime + keepShownDuration + hideAnimationTime)
		{
			_cg.alpha = Mathf.MoveTowards(_cg.alpha, 0f, 1f / hideAnimationTime * Time.unscaledDeltaTime);
		}
		if (_holdAmount >= 1f)
		{
			return;
		}
		if (DewInput.GetButtonDownAnyKey() || DewInput.GetButtonDownAnyGamepad() || DewInput.GetButtonDownAnyMouse() || (bool)it_skip)
		{
			_lastTouchTime = Time.unscaledTime;
		}
		if ((bool)it_skip)
		{
			_holdAmount += 1f / holdDuration * Time.unscaledDeltaTime;
			if (_holdAmount >= 1f)
			{
				try
				{
					onSkip?.Invoke();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}
		else
		{
			_holdAmount = 0f;
		}
	}
}
