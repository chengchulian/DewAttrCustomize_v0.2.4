using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Lobby_Loadout_AvailableSkills_Item : UI_GamepadFocusable, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IGamepadFocusableOverrideInput
{
	public Image icon;

	public GameObject selectedObject;

	public GameObject lockedObject;

	public GameObject newObject;

	public Transform tooltipPivot;

	[NonSerialized]
	public int index;

	private SkillTrigger _skill;

	public void Setup(int i, SkillTrigger skill, bool isSelected)
	{
		string sName = skill.GetType().Name;
		if (isSelected)
		{
			DewSave.profile.skills[sName].isNewHeroOrHeroSkill = false;
		}
		_skill = skill;
		index = i;
		icon.sprite = skill.configs[0].triggerIcon;
		selectedObject.SetActive(isSelected);
		lockedObject.SetActive(!DewSave.profile.skills[sName].isAvailableInGame);
		newObject.SetActive(DewSave.profile.skills[sName].isNewHeroOrHeroSkill);
		GetComponent<Button>().interactable = !lockedObject.activeSelf;
	}

	public void Click()
	{
		if (!lockedObject.activeSelf)
		{
			DewSave.profile.skills[_skill.GetType().Name].isNewHeroOrHeroSkill = false;
			newObject.SetActive(value: false);
			GetComponentInParent<UI_Lobby_Loadout_AvailableSkills>().ClickOnItem(index);
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (lockedObject.activeSelf)
		{
			tooltip.ShowCollectableTooltip(tooltipPivot.position, _skill.GetType());
		}
		else
		{
			tooltip.ShowSkillTooltip(tooltipPivot.position, _skill);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		SingletonBehaviour<UI_TooltipManager>.instance.Hide();
	}

	public bool OnGamepadDpadUp()
	{
		ManagerBase<GlobalUIManager>.instance.MoveFocus(Vector3.up, 89f, 1f, float.PositiveInfinity, (IGamepadFocusable f) => f.GetTransform().IsSelfOrDescendantOf(base.transform.parent));
		return true;
	}

	public bool OnGamepadDpadLeft()
	{
		return true;
	}

	public bool OnGamepadDpadDown()
	{
		ManagerBase<GlobalUIManager>.instance.MoveFocus(Vector3.down, 89f, 1f, float.PositiveInfinity, (IGamepadFocusable f) => f.GetTransform().IsSelfOrDescendantOf(base.transform.parent));
		return true;
	}

	public bool OnGamepadDpadRight()
	{
		return true;
	}
}
