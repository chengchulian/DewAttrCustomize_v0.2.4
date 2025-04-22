using TMPro;
using UnityEngine;

public class UI_Collectables_ObjectItemProvider_Skill : UI_Collectables_ObjectItemProvider
{
	public GameObject activationKeyObject;

	public TextMeshProUGUI activationKeyText;

	public override void OnSetup(Object obj, UnlockStatus status, int index)
	{
		base.OnSetup(obj, status, index);
		SkillTrigger skill = (SkillTrigger)obj;
		if (skill.rarity == Rarity.Character)
		{
			activationKeyObject.SetActive(skill.rarity == Rarity.Character);
			string skillName = skill.GetType().Name;
			if (skillName.Contains("_QR_"))
			{
				activationKeyText.text = DewInput.GetReadableTextForCurrentMode(DewSave.profile.controls.GetSkillBinding(HeroSkillLocation.Q)) + "<size=60%> / </size>" + DewInput.GetReadableTextForCurrentMode(DewSave.profile.controls.GetSkillBinding(HeroSkillLocation.R));
				return;
			}
			HeroSkillLocation type = HeroSkillLocation.Q;
			if (skillName.Contains("_Q_"))
			{
				type = HeroSkillLocation.Q;
			}
			else if (skillName.Contains("_R_"))
			{
				type = HeroSkillLocation.R;
			}
			else if (skillName.Contains("_D_"))
			{
				type = HeroSkillLocation.Identity;
			}
			activationKeyText.text = DewInput.GetReadableTextForCurrentMode(DewSave.profile.controls.GetSkillBinding(type));
		}
		else
		{
			activationKeyObject.SetActive(value: false);
		}
	}

	public override DewProfile.UnlockData GetUnlockData()
	{
		return DewSave.profile.skills[targetObj.GetType().Name];
	}

	public override Color GetRarityColor()
	{
		return Dew.GetRarityColor(((SkillTrigger)targetObj).rarity);
	}

	public override Sprite GetIcon()
	{
		return ((SkillTrigger)targetObj).configs[0].triggerIcon;
	}
}
