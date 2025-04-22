using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar_Simple : LogicBehaviour
{
	private enum State
	{
		Hide,
		Show,
		Destroy
	}

	public Color healthColorOwn;

	public Color healthColorAlly;

	public Color healthColorEnemy;

	public Color healthColorNeutral;

	public Image healthFill;

	public Image shieldFill;

	public Image[] deltas;

	public TextMeshProUGUI levelText;

	public RectTransform hpBgTransform;

	public bool showAlways;

	public float deltaAnimateSpeed = 1f;

	public float deltaMoveUpSpeed = 10f;

	public float appearSpeed = 0.5f;

	public float disappearSpeed = 0.5f;

	public AnimationCurve shakeCurve;

	public float shakeMagnitude = 10f;

	public float shakeDecreaseSpeed = 3f;

	public float shakeInterval = 0.05f;

	private float _shakeValue;

	private Vector2 _shakeOffset;

	private float _lastShakeTime;

	private CanvasGroup _canvasGroup;

	private float[] _deltaValues;

	private int _deltaCursor;

	private float _lastValue;

	private State _state;

	[HideInInspector]
	public Entity target;

	private Vector3 _lastWorldPosition;

	private void Start()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		_canvasGroup.alpha = 0f;
		for (int i = 0; i < deltas.Length; i++)
		{
			deltas[i].color = new Color(1f, 1f, 1f, 0f);
		}
		_deltaValues = new float[deltas.Length];
		if (showAlways)
		{
			_state = State.Show;
		}
		Refresh();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		RefreshValue();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		for (int i = 0; i < deltas.Length; i++)
		{
			if (!(_deltaValues[i] <= 0f))
			{
				if (_deltaValues[i] < 0.99f)
				{
					Vector2 newPos = deltas[i].rectTransform.anchoredPosition;
					newPos.y += deltaMoveUpSpeed * Time.deltaTime;
					deltas[i].rectTransform.anchoredPosition = newPos;
				}
				_deltaValues[i] = Mathf.MoveTowards(_deltaValues[i], 0f, deltaAnimateSpeed * Time.deltaTime);
				deltas[i].color = new Color(1f, 1f, 1f, _deltaValues[i]);
			}
		}
		if (_state == State.Show)
		{
			_canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 1f, appearSpeed * Time.deltaTime);
			return;
		}
		_canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0f, disappearSpeed * Time.deltaTime);
		if (_state == State.Destroy && _canvasGroup.alpha <= 0f)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void LateUpdate()
	{
		RefreshPosition();
	}

	private void Refresh()
	{
		if (target == null || !target.isActive)
		{
			if (_state != State.Destroy)
			{
				_canvasGroup.alpha = 1f;
				_state = State.Destroy;
			}
		}
		else
		{
			RefreshPosition();
			RefreshValue();
		}
	}

	private void RefreshPosition()
	{
		if (target == null || !target.isActive)
		{
			if (_state != State.Destroy)
			{
				_canvasGroup.alpha = 1f;
				_state = State.Destroy;
			}
		}
		else
		{
			_lastWorldPosition = ((target.Visual.healthBarPosition != null) ? target.Visual.healthBarPosition.position : target.Visual.GetAbovePosition());
		}
		base.transform.position = Dew.mainCamera.WorldToScreenPoint(_lastWorldPosition) + (Vector3)_shakeOffset;
		if (Time.time - _lastShakeTime > shakeInterval)
		{
			_lastShakeTime = Time.time;
			_shakeOffset = global::UnityEngine.Random.insideUnitCircle * shakeMagnitude * _shakeValue;
		}
		_shakeValue = Mathf.MoveTowards(_shakeValue, 0f, shakeDecreaseSpeed * Time.deltaTime);
	}

	private void RefreshValue()
	{
		if (target == null || !target.isActive)
		{
			if (_state != State.Destroy)
			{
				_canvasGroup.alpha = 1f;
				_state = State.Destroy;
			}
			healthFill.fillAmount = 0f;
			shieldFill.fillAmount = 0f;
		}
		else
		{
			switch (DewPlayer.local.GetTeamRelation(target))
			{
			case TeamRelation.Own:
				healthFill.color = healthColorOwn;
				break;
			case TeamRelation.Neutral:
				healthFill.color = healthColorNeutral;
				break;
			case TeamRelation.Enemy:
				healthFill.color = healthColorEnemy;
				break;
			case TeamRelation.Ally:
				healthFill.color = healthColorAlly;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			float maxAmount = Mathf.Max(target.currentHealth + target.Status.currentShield, target.maxHealth);
			healthFill.fillAmount = target.currentHealth / maxAmount;
			shieldFill.fillAmount = (target.currentHealth + target.Status.currentShield) / maxAmount;
			levelText.text = target.level.ToString();
		}
		if (_lastValue > healthFill.fillAmount)
		{
			if (_state == State.Hide)
			{
				_state = State.Show;
			}
			float diff = _lastValue - healthFill.fillAmount;
			RectTransform rectTransform = deltas[_deltaCursor].rectTransform;
			rectTransform.sizeDelta = new Vector2(hpBgTransform.sizeDelta.x * diff, hpBgTransform.sizeDelta.y);
			rectTransform.anchoredPosition = new Vector2(hpBgTransform.anchoredPosition.x - hpBgTransform.sizeDelta.x / 2f + hpBgTransform.sizeDelta.x * (_lastValue + healthFill.fillAmount) / 2f, hpBgTransform.anchoredPosition.y);
			deltas[_deltaCursor].color = Color.white;
			_deltaValues[_deltaCursor] = 1f;
			_deltaCursor++;
			if (_deltaCursor >= deltas.Length)
			{
				_deltaCursor = 0;
			}
			_shakeValue += shakeCurve.Evaluate(diff);
		}
		_lastValue = healthFill.fillAmount;
	}
}
