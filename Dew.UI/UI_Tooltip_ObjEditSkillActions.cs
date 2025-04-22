using UnityEngine;

public class UI_Tooltip_ObjEditSkillActions : UI_Tooltip_BaseObj
{
	public GameObject confirmObject;

	public GameObject startMovingOrUnequipObject;

	public GameObject cleanseObject;

	public GameObject upgradeObject;

	public GameObject sellObject;

	public GameObject buyObject;

	public GameObject seeDetailsObject;

	public override void OnSetup()
	{
		base.OnSetup();
		if (ManagerBase<EditSkillManager>.instance == null)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		EditSkillManager.ModeType mode = ManagerBase<EditSkillManager>.instance.mode;
		if (mode == EditSkillManager.ModeType.None)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		seeDetailsObject.SetActive(base.currentObject != null);
		if (currentObjects.Count == 3 && currentObjects[2] is string info && info == "RAW")
		{
			confirmObject.SetActive(value: false);
			startMovingOrUnequipObject.SetActive(value: false);
			cleanseObject.SetActive(value: false);
			upgradeObject.SetActive(value: false);
			sellObject.SetActive(value: false);
			buyObject.SetActive(value: false);
			return;
		}
		if (ManagerBase<EditSkillManager>.instance.draggingObject != null)
		{
			confirmObject.SetActive(value: true);
			startMovingOrUnequipObject.SetActive(value: false);
			cleanseObject.SetActive(value: false);
			upgradeObject.SetActive(value: false);
			sellObject.SetActive(value: false);
			buyObject.SetActive(value: false);
		}
		else if ((DewInput.currentMode == InputMode.Gamepad && ManagerBase<GlobalUIManager>.instance.focused != null && ManagerBase<GlobalUIManager>.instance.focused.GetTransform().GetComponent<UI_InGame_FloatingWindow_Shop_Item>() != null) || (DewInput.currentMode == InputMode.KeyboardAndMouse && SingletonBehaviour<UI_TooltipManager>.instance.lastTarget is UI_InGame_FloatingWindow_Shop_Item))
		{
			confirmObject.SetActive(value: false);
			startMovingOrUnequipObject.SetActive(value: false);
			cleanseObject.SetActive(value: false);
			upgradeObject.SetActive(value: false);
			sellObject.SetActive(value: false);
			buyObject.SetActive(value: true);
		}
		else
		{
			switch (mode)
			{
			case EditSkillManager.ModeType.Regular:
			{
				confirmObject.SetActive(value: false);
				startMovingOrUnequipObject.SetActive(base.currentObject != null && (!(base.currentObject is SkillTrigger s) || !DewPlayer.local.hero.Skill.TryGetSkillLocation(s, out var loc) || DewPlayer.local.hero.Skill.CanReplaceSkill(loc)));
				cleanseObject.SetActive(value: false);
				upgradeObject.SetActive(value: false);
				sellObject.SetActive(value: false);
				buyObject.SetActive(value: false);
				break;
			}
			case EditSkillManager.ModeType.EquipGem:
				confirmObject.SetActive(value: true);
				startMovingOrUnequipObject.SetActive(value: false);
				cleanseObject.SetActive(value: false);
				upgradeObject.SetActive(value: false);
				sellObject.SetActive(value: false);
				buyObject.SetActive(value: false);
				break;
			case EditSkillManager.ModeType.EquipSkill:
				confirmObject.SetActive(value: true);
				startMovingOrUnequipObject.SetActive(value: false);
				cleanseObject.SetActive(value: false);
				upgradeObject.SetActive(value: false);
				sellObject.SetActive(value: false);
				buyObject.SetActive(value: false);
				break;
			case EditSkillManager.ModeType.UpgradeGem:
				confirmObject.SetActive(value: false);
				startMovingOrUnequipObject.SetActive(value: false);
				cleanseObject.SetActive(value: false);
				upgradeObject.SetActive(base.currentObject is Gem);
				sellObject.SetActive(value: false);
				buyObject.SetActive(value: false);
				break;
			case EditSkillManager.ModeType.UpgradeSkill:
				confirmObject.SetActive(value: false);
				startMovingOrUnequipObject.SetActive(value: false);
				cleanseObject.SetActive(value: false);
				upgradeObject.SetActive(base.currentObject is SkillTrigger);
				sellObject.SetActive(value: false);
				buyObject.SetActive(value: false);
				break;
			case EditSkillManager.ModeType.Upgrade:
			{
				confirmObject.SetActive(value: false);
				startMovingOrUnequipObject.SetActive(value: false);
				cleanseObject.SetActive(value: false);
				GameObject obj3 = upgradeObject;
				object obj2 = base.currentObject;
				obj3.SetActive(obj2 is SkillTrigger || obj2 is Gem);
				sellObject.SetActive(value: false);
				buyObject.SetActive(value: false);
				break;
			}
			case EditSkillManager.ModeType.Sell:
			{
				confirmObject.SetActive(value: false);
				startMovingOrUnequipObject.SetActive(value: false);
				cleanseObject.SetActive(value: false);
				upgradeObject.SetActive(value: false);
				sellObject.SetActive(base.currentObject is Gem || (base.currentObject is SkillTrigger st && DewPlayer.local.hero.Skill.TryGetSkillLocation(st, out var sloc) && DewPlayer.local.hero.Skill.CanReplaceSkill(sloc)));
				buyObject.SetActive(value: false);
				break;
			}
			case EditSkillManager.ModeType.Cleanse:
			{
				confirmObject.SetActive(value: false);
				startMovingOrUnequipObject.SetActive(value: false);
				GameObject obj = cleanseObject;
				object obj2 = base.currentObject;
				obj.SetActive(obj2 is SkillTrigger || obj2 is Gem);
				upgradeObject.SetActive(value: false);
				sellObject.SetActive(value: false);
				buyObject.SetActive(value: false);
				break;
			}
			default:
				confirmObject.SetActive(value: false);
				startMovingOrUnequipObject.SetActive(value: false);
				cleanseObject.SetActive(value: false);
				upgradeObject.SetActive(value: false);
				sellObject.SetActive(value: false);
				buyObject.SetActive(value: false);
				break;
			}
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeSelf)
			{
				return;
			}
		}
		base.gameObject.SetActive(value: false);
	}
}
