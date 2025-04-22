using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_Interact_Gem : UI_InGame_Interact_Base
{
	public TextMeshProUGUI nameText;

	public TextMeshProUGUI shortText;

	public ActionDisplay equipObject;

	public ActionDisplay combineObject;

	public ActionDisplay dismantleObject;

	public Image dismantleProgressFill;

	public GameObject isLockedObject;

	private float _fillCv;

	public override Type GetSupportedType()
	{
		return typeof(Gem);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		Gem gem = (Gem)base.interactable;
		string template = DewLocalization.GetUIValue("InGame_Tooltip_GemName_" + gem.qualityType);
		string gemName = DewLocalization.GetGemName(DewLocalization.GetGemKey(gem.GetType()));
		nameText.text = $"({gem.quality}%) {string.Format(template, gemName)}";
		nameText.color = Dew.GetRarityColor(gem.rarity);
		shortText.text = DewLocalization.GetGemShortDescription(DewLocalization.GetGemKey(gem.GetType()));
		UpdateActions();
		dismantleProgressFill.fillAmount = 0f;
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		UpdateActions();
	}

	private void UpdateActions()
	{
		Gem gem = (Gem)base.interactable;
		GemLocation loc;
		Gem gem2;
		bool hasDuplicate = DewPlayer.local.hero.Skill.TryGetEquippedGemOfSameType(gem.GetType(), out loc, out gem2);
		equipObject.gameObject.SetActive(!hasDuplicate);
		combineObject.gameObject.SetActive(hasDuplicate);
		bool canDismantle = !ManagerBase<ControlManager>.instance.isDismantleDisabled && (ManagerBase<ControlManager>.instance.dismantleConstraint == null || ManagerBase<ControlManager>.instance.dismantleConstraint(gem));
		dismantleObject.gameObject.SetActive(canDismantle);
		if (dismantleObject.gameObject.activeSelf)
		{
			dismantleObject.GetComponentInChildren<CostDisplay>().SetupDreamDust(gem.GetDismantleAmount(DewPlayer.local), showCantAfford: false, showPlusSign: true);
		}
		equipObject.isDisabled = gem.isLocked;
		combineObject.isDisabled = gem.isLocked;
		dismantleObject.isDisabled = gem.isLocked;
		isLockedObject.SetActive(gem.isLocked);
		if (dismantleProgressFill != null)
		{
			dismantleProgressFill.fillAmount = Mathf.SmoothDamp(dismantleProgressFill.fillAmount, gem.dismantleProgress / 0.8f, ref _fillCv, 0.1f, float.PositiveInfinity, 1f / 30f);
		}
	}
}
