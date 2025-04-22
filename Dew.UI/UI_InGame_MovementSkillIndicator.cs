using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_MovementSkillIndicator : MonoBehaviour
{
	private const float ShowAnimationDuration = 0.25f;

	private const float HideAnimationDuration = 1.5f;

	public Image fillTemplate;

	public Transform fillsRoot;

	public Color readyFillColor;

	public Color notReadyFillColor;

	private readonly List<Image> _fills = new List<Image>();

	private CanvasGroup _canvasGroup;

	private void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		_canvasGroup.alpha = 0f;
	}

	private void Start()
	{
		ControlManager instance = ManagerBase<ControlManager>.instance;
		instance.onCastFailed = (Action<AbilityTrigger>)Delegate.Combine(instance.onCastFailed, new Action<AbilityTrigger>(OnCastFailed));
	}

	private void OnCastFailed(AbilityTrigger obj)
	{
		if (DewPlayer.local.hero.Skill.Movement == obj)
		{
			base.transform.DOKill(complete: true);
			base.transform.DOPunchScale(Vector3.one * 0.65f, 0.6f);
		}
	}

	private void LateUpdate()
	{
		if (!(DewPlayer.local == null) && !(DewPlayer.local.hero == null) && !(DewPlayer.local.hero.Skill.Movement == null))
		{
			base.transform.position = Dew.mainCamera.WorldToScreenPoint(DewPlayer.local.hero.position).Quantitized();
		}
	}

	private void Update()
	{
		if (DewPlayer.local == null || DewPlayer.local.hero.IsNullInactiveDeadOrKnockedOut() || DewPlayer.local.hero.Skill.Movement == null || NetworkedManagerBase<ZoneManager>.instance.isInRoomTransition || InGameUIManager.instance.isWorldDisplayed != 0)
		{
			_canvasGroup.alpha = 0f;
			return;
		}
		SkillTrigger mov = DewPlayer.local.hero.Skill.Movement;
		int maxCharges = mov.currentConfig.maxCharges;
		int currentCharges = mov.currentConfigCurrentCharge;
		if (currentCharges < maxCharges && _canvasGroup.alpha < 1f)
		{
			_canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 1f, Time.deltaTime / 0.25f);
		}
		else if (currentCharges >= maxCharges && _canvasGroup.alpha > 0f)
		{
			_canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0f, Time.deltaTime / 1.5f);
		}
		if (!(_canvasGroup.alpha > 0f))
		{
			return;
		}
		if (_fills.Count > maxCharges)
		{
			for (int i = _fills.Count - 1; i >= maxCharges; i--)
			{
				global::UnityEngine.Object.Destroy(_fills[i].gameObject);
				_fills.RemoveAt(i);
			}
		}
		else if (_fills.Count < maxCharges)
		{
			for (int j = 0; j < maxCharges - _fills.Count; j++)
			{
				Image newFill = global::UnityEngine.Object.Instantiate(fillTemplate, fillsRoot);
				newFill.gameObject.SetActive(value: true);
				_fills.Add(newFill);
			}
		}
		for (int k = 0; k < _fills.Count; k++)
		{
			if (k < currentCharges)
			{
				_fills[k].fillAmount = 1f;
				_fills[k].color = readyFillColor;
			}
			else if (k == currentCharges)
			{
				_fills[k].fillAmount = 1f - mov.currentConfigCooldownTime / mov.currentConfigMaxCooldownTime;
				_fills[k].color = notReadyFillColor;
			}
			else
			{
				_fills[k].fillAmount = 0f;
			}
		}
	}
}
