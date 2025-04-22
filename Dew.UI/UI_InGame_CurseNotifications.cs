using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[LogicUpdatePriority(400)]
public class UI_InGame_CurseNotifications : LogicBehaviour
{
	public GameObject windowObject;

	public Image curseIcon;

	public TextMeshProUGUI curseNameText;

	public TextMeshProUGUI curseDescriptionText;

	public TextMeshProUGUI curseLiftConditionText;

	public RectTransform hideOnCursorArea;

	public float sustainTime;

	public float decayTime;

	public float mouseOverAlpha = 0.4f;

	public float defaultAlpha = 1f;

	public CanvasGroup windowCg;

	public float showAlphaSpeed = 4f;

	private float _curseStartTime = float.NegativeInfinity;

	private UI_InGame_SkillNotifications _skillNotifications;

	private void Start()
	{
		GameManager.CallOnReady(delegate
		{
			DewPlayer.local.hero.ClientEntityEvent_OnStatusEffectAdded += new Action<EventInfoStatusEffect>(ClientEntityEventOnStatusEffectAdded);
		});
		windowObject.SetActive(value: false);
		_skillNotifications = global::UnityEngine.Object.FindObjectOfType<UI_InGame_SkillNotifications>();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (DewPlayer.local == null || DewPlayer.local.hero == null)
		{
			return;
		}
		if (DewPlayer.local.hero.isKnockedOut)
		{
			_curseStartTime = float.NegativeInfinity;
			windowObject.SetActive(value: false);
			return;
		}
		float elapsed = Time.time - _curseStartTime;
		if (_skillNotifications.currentAlpha > 0.1f)
		{
			if (Time.time - _curseStartTime > sustainTime * 0.4f)
			{
				_curseStartTime = Time.time - sustainTime * 0.4f;
			}
			_curseStartTime += Time.deltaTime;
			windowCg.alpha = Mathf.MoveTowards(windowCg.alpha, 0f, showAlphaSpeed * Time.deltaTime);
		}
		else if (elapsed < sustainTime)
		{
			bool shouldHide = hideOnCursorArea.GetScreenSpaceRect().Contains(Input.mousePosition) && DewPlayer.local != null && !DewPlayer.local.hero.IsNullInactiveDeadOrKnockedOut() && DewPlayer.local.hero.isInCombat;
			windowCg.alpha = Mathf.MoveTowards(windowCg.alpha, shouldHide ? mouseOverAlpha : defaultAlpha, Time.deltaTime * showAlphaSpeed);
		}
		else if (elapsed < sustainTime + decayTime)
		{
			windowCg.alpha = Mathf.MoveTowards(windowCg.alpha, 0f, 1f / decayTime * Time.deltaTime);
		}
		else
		{
			windowObject.SetActive(value: false);
		}
	}

	private void ClientEntityEventOnStatusEffectAdded(EventInfoStatusEffect obj)
	{
		if (obj.effect is CurseStatusEffect se)
		{
			string key = DewLocalization.GetCurseKey(se.GetType().Name);
			curseNameText.text = se.GetName();
			curseDescriptionText.text = DewLocalization.GetCurseDescription(key).ToText(se);
			curseLiftConditionText.text = string.Format(DewLocalization.GetUIValue("InGame_Curse_LiftCondition_" + se.progressType), se.requiredAmount);
			_curseStartTime = Time.time;
			windowObject.SetActive(value: true);
			windowCg.alpha = 0f;
			ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Game_LocalHeroCursed");
		}
	}
}
