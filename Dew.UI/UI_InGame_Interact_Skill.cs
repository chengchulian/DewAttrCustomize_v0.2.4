using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_Interact_Skill : UI_InGame_Interact_Base
{
	public GameObject ultimateIndicator;

	public GameObject identityIndicator;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI shortText;

	public ActionDisplay dismantleObject;

	public ActionDisplay equipObject;

	public ActionDisplay embraceObject;

	public Image dismantleProgressFill;

	public GameObject isLockedObject;

	private float _fillCv;

	public override Type GetSupportedType()
	{
		return typeof(SkillTrigger);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		SkillTrigger skill = (SkillTrigger)base.interactable;
		string text = string.Format(DewLocalization.GetSkillLevelTemplate(skill.level, null), DewLocalization.GetSkillName(DewLocalization.GetSkillKey(skill.GetType()), 0));
		nameText.text = text;
		nameText.color = Dew.GetRarityColor(skill.rarity);
		shortText.text = DewLocalization.GetSkillShortDesc(DewLocalization.GetSkillKey(skill.GetType()), 0);
		ultimateIndicator.SetActive(skill.type == SkillType.Ultimate);
		identityIndicator.SetActive(skill.rarity == Rarity.Identity);
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
		SkillTrigger skill = (SkillTrigger)base.interactable;
		equipObject.gameObject.SetActive(skill.rarity != Rarity.Identity);
		embraceObject.gameObject.SetActive(skill.rarity == Rarity.Identity);
		bool canDismantle = !ManagerBase<ControlManager>.instance.isDismantleDisabled && (ManagerBase<ControlManager>.instance.dismantleConstraint == null || ManagerBase<ControlManager>.instance.dismantleConstraint(skill));
		dismantleObject.gameObject.SetActive(canDismantle);
		if (dismantleObject.gameObject.activeSelf)
		{
			dismantleObject.GetComponentInChildren<CostDisplay>().SetupDreamDust(skill.GetDismantleAmount(DewPlayer.local), showCantAfford: false, showPlusSign: true);
		}
		equipObject.isDisabled = skill.isLocked;
		embraceObject.isDisabled = skill.isLocked;
		dismantleObject.isDisabled = skill.isLocked;
		isLockedObject.SetActive(skill.isLocked);
		if (dismantleProgressFill != null)
		{
			dismantleProgressFill.fillAmount = Mathf.SmoothDamp(dismantleProgressFill.fillAmount, skill.dismantleProgress / 0.8f, ref _fillCv, 0.1f, float.PositiveInfinity, 1f / 30f);
		}
	}
}
