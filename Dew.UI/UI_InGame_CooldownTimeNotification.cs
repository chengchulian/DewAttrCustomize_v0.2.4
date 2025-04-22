using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_InGame_CooldownTimeNotification : LogicBehaviour
{
	public TextMeshProUGUI text;

	public float disappearSpeed = 1.25f;

	public float showCooldownTime = 0.75f;

	private AbilityTrigger _target;

	private CanvasGroup _cg;

	private float _lastShowTime;

	private bool _isOnHero;

	private void Start()
	{
		ControlManager instance = ManagerBase<ControlManager>.instance;
		instance.onCastFailed = (Action<AbilityTrigger>)Delegate.Combine(instance.onCastFailed, new Action<AbilityTrigger>(OnCastFailed));
		_cg = GetComponent<CanvasGroup>();
		_cg.alpha = 0f;
	}

	private void OnCastFailed(AbilityTrigger trigger)
	{
		if (trigger.currentConfigCurrentCharge > 0 || trigger.currentConfigCooldownTime < 0.01f || Time.unscaledTime - _lastShowTime < showCooldownTime)
		{
			return;
		}
		_lastShowTime = Time.unscaledTime;
		_isOnHero = false;
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			if (ManagerBase<ControlManager>.instance.aimPoint.HasValue)
			{
				base.transform.position = ManagerBase<DewCamera>.instance.mainCamera.WorldToScreenPoint(ManagerBase<ControlManager>.instance.aimPoint.Value);
			}
			else
			{
				_isOnHero = true;
				base.transform.position = Vector3.left * 1000f;
			}
		}
		else
		{
			base.transform.position = Input.mousePosition;
		}
		_target = trigger;
		if (_target is SkillTrigger { type: SkillType.Ultimate })
		{
			float val = 1f - _target.currentConfigCooldownTime / _target.currentConfigMaxCooldownTime;
			text.text = $"{Mathf.FloorToInt(val * 100f)}%";
		}
		else
		{
			text.text = GetReadableRepresentationOfNumber(_target.currentConfigCooldownTime);
		}
		_cg.alpha = 1f;
		text.DOKill(complete: true);
		text.color = new Color(1f, 0.35f, 0.35f);
		text.DOColor(Color.white, 0.65f);
		base.transform.DOKill(complete: true);
		base.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f);
		if (_cg.alpha > _target.currentConfigCooldownTime)
		{
			_cg.alpha = _target.currentConfigCooldownTime;
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		_cg.alpha = Mathf.MoveTowards(_cg.alpha, 0f, Time.deltaTime * disappearSpeed);
		if (_isOnHero && ManagerBase<ControlManager>.instance.controllingEntity != null)
		{
			base.transform.position = ManagerBase<DewCamera>.instance.mainCamera.WorldToScreenPoint(ManagerBase<ControlManager>.instance.controllingEntity.agentPosition + Vector3.down * 1.5f);
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (_target == null)
		{
			_cg.alpha = 0f;
			return;
		}
		if (_target.currentConfigCooldownTime < 0.01f)
		{
			text.text = "";
			_cg.alpha = 0f;
		}
		else if (_target is SkillTrigger { type: SkillType.Ultimate })
		{
			float val = 1f - _target.currentConfigCooldownTime / _target.currentConfigMaxCooldownTime;
			text.text = $"{Mathf.FloorToInt(val * 100f)}%";
		}
		else
		{
			text.text = GetReadableRepresentationOfNumber(_target.currentConfigCooldownTime);
		}
		if (_cg.alpha > _target.currentConfigCooldownTime)
		{
			_cg.alpha = _target.currentConfigCooldownTime;
		}
	}

	private string GetReadableRepresentationOfNumber(float num)
	{
		if (num > 10f)
		{
			return ((int)num).ToString();
		}
		return num.ToString("0.0");
	}
}
