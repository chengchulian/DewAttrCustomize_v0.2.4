using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Lobby_Loadout_SkillSlot : UI_Lobby_Loadout_ComponentBase, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public HeroSkillLocation type;

	public Transform tooltipPivot;

	public Transform availableSkillPivot;

	public Image skillIcon;

	public GameObject activationKeyObject;

	public TextMeshProUGUI activationKey;

	public bool placeMenuOnPivot;

	public GameObject selectionOpenObject;

	public GameObject selectionNotOpenObject;

	public TextMeshProUGUI selectionCountText;

	public GameObject selectionGroupObject;

	public GameObject newObject;

	private UI_Lobby_Loadout_AvailableSkills _availableSkills;

	private CanvasGroup _cg;

	private SkillTrigger[] _skills;

	public SkillTrigger skill { get; private set; }

	protected override void Start()
	{
		base.Start();
		_cg = GetComponent<CanvasGroup>();
		_availableSkills = global::UnityEngine.Object.FindObjectOfType<UI_Lobby_Loadout_AvailableSkills>(includeInactive: true);
		activationKey.text = DewInput.GetReadableTextForCurrentMode(DewSave.profile.controls.GetSkillBinding(type));
		selectionNotOpenObject.SetActive(value: true);
		selectionOpenObject.SetActive(value: false);
		_availableSkills.onClose += new Action(UpdateHasNewStatus);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (_availableSkills != null)
		{
			_availableSkills.onClose -= new Action(UpdateHasNewStatus);
		}
	}

	private void UpdateHasNewStatus()
	{
		if (type == HeroSkillLocation.Movement || base.hero == null)
		{
			newObject.SetActive(value: false);
			return;
		}
		SkillTrigger[] loadoutSkills = base.hero.GetComponent<HeroSkill>().GetLoadoutSkills(type);
		bool hasNew = false;
		SkillTrigger[] array = loadoutSkills;
		foreach (SkillTrigger s in array)
		{
			if (DewSave.profile.skills[s.GetType().Name].isNewHeroOrHeroSkill && !(skill == s))
			{
				hasNew = true;
				break;
			}
		}
		newObject.SetActive(hasNew);
	}

	protected override void OnLoadoutChanged()
	{
		base.OnLoadoutChanged();
		_skills = (from s in base.hero.GetComponent<HeroSkill>().GetLoadoutSkills(type)
			where Dew.IsSkillIncludedInGame(s.GetType().Name)
			select s).ToArray();
		int index = base.loadout.GetSkill(type);
		index = Mathf.Clamp(index, 0, _skills.Length);
		skill = _skills[index];
		skillIcon.sprite = skill.configs[0].triggerIcon;
		activationKeyObject.SetActive(type != HeroSkillLocation.Movement && skill.configs[0].isActive);
		selectionCountText.text = $"{index + 1}/{_skills.Length}";
		selectionGroupObject.SetActive(_skills.Length > 1);
		UpdateHasNewStatus();
	}

	public void Click()
	{
		if (!selectionOpenObject.activeSelf && _skills.Length > 1)
		{
			_availableSkills.gameObject.SetActive(value: false);
			_availableSkills.type = type;
			_availableSkills.skills = _skills;
			_availableSkills.currentSkill = base.loadout.GetSkill(type);
			_availableSkills.gameObject.SetActive(value: true);
			if (placeMenuOnPivot)
			{
				_availableSkills.transform.position = availableSkillPivot.position;
			}
			LogicUpdate(0f);
			newObject.SetActive(value: false);
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (_skills != null)
		{
			selectionOpenObject.SetActive(_availableSkills.isActiveAndEnabled && _availableSkills.type == type);
			selectionNotOpenObject.SetActive(_skills.Length > 1 && !selectionOpenObject.activeSelf);
			_cg.interactable = !selectionOpenObject.activeSelf;
			_cg.blocksRaycasts = !selectionOpenObject.activeSelf;
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		tooltip.ShowSkillTooltip(tooltipPivot.position, skill);
	}
}
