using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Lobby_Constellations_Skills_Slot : LogicBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public HeroSkillLocation type;

	public Image skillIcon;

	public TextMeshProUGUI activationKeyText;

	public GameObject activationKeyObject;

	public TextMeshProUGUI selectionCountText;

	public GameObject selectionGroupObject;

	public Transform tooltipPivot;

	public Transform menuPivot;

	public GameObject menuOpenObject;

	public GameObject menuNotOpenObject;

	public GameObject newObject;

	private UI_Lobby_Constellations_Skills_ContextMenu _menu;

	private SkillTrigger[] _skills;

	private SkillTrigger _skill;

	private Hero _hero;

	private CanvasGroup _cg;

	private void Start()
	{
		_menu = global::UnityEngine.Object.FindObjectOfType<UI_Lobby_Constellations_Skills_ContextMenu>(includeInactive: true);
		SingletonBehaviour<UI_Lobby_Constellations>.instance.onLoadoutChanged += new Action(Refresh);
		_cg = GetComponent<CanvasGroup>();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		Refresh();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (_skills != null)
		{
			menuOpenObject.SetActive(_menu.isActiveAndEnabled && _menu.type == type);
			menuNotOpenObject.SetActive(_skills.Length > 1 && !menuOpenObject.activeSelf);
			_cg.interactable = !menuOpenObject.activeSelf;
			_cg.blocksRaycasts = !menuOpenObject.activeSelf;
		}
	}

	private void Refresh()
	{
		if (SingletonBehaviour<UI_Lobby_Constellations>.instance == null || SingletonBehaviour<UI_Lobby_Constellations>.instance.loadout == null)
		{
			return;
		}
		HeroLoadoutData loadout = SingletonBehaviour<UI_Lobby_Constellations>.instance.loadout;
		if (_hero == null || _hero.GetType().Name != DewPlayer.local.selectedHeroType)
		{
			_hero = DewResources.GetByShortTypeName<Hero>(DewPlayer.local.selectedHeroType);
			_skills = (from s in _hero.GetComponent<HeroSkill>().GetLoadoutSkills(type)
				where Dew.IsSkillIncludedInGame(s.GetType().Name)
				select s).ToArray();
		}
		activationKeyText.text = DewInput.GetReadableTextForCurrentMode(DewSave.profile.controls.GetSkillBinding(type));
		int index = loadout.GetSkill(type);
		index = Mathf.Clamp(index, 0, _skills.Length);
		_skill = _skills[index];
		skillIcon.sprite = _skill.configs[0].triggerIcon;
		activationKeyObject.SetActive(type != HeroSkillLocation.Movement && _skill.configs[0].isActive);
		selectionCountText.text = $"{index + 1}/{_skills.Length}";
		selectionGroupObject.SetActive(_skills.Length > 1);
		UpdateHasNewStatus();
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		tooltip.ShowSkillTooltip(new TooltipSettings
		{
			position = tooltipPivot.position,
			pivot = new Vector2(0f, 0.5f)
		}, _skill);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!menuOpenObject.activeSelf && _skills.Length > 1)
		{
			_menu.gameObject.SetActive(value: false);
			_menu.type = type;
			_menu.skills = _skills;
			_menu.currentSkill = SingletonBehaviour<UI_Lobby_Constellations>.instance.loadout.GetSkill(type);
			_menu.gameObject.SetActive(value: true);
			_menu.transform.position = menuPivot.position;
			LogicUpdate(0f);
			newObject.SetActive(value: false);
		}
	}

	private void UpdateHasNewStatus()
	{
		if (type == HeroSkillLocation.Movement || _hero == null)
		{
			newObject.SetActive(value: false);
			return;
		}
		SkillTrigger[] loadoutSkills = _hero.GetComponent<HeroSkill>().GetLoadoutSkills(type);
		bool hasNew = false;
		SkillTrigger[] array = loadoutSkills;
		foreach (SkillTrigger s in array)
		{
			if (DewSave.profile.skills[s.GetType().Name].isNewHeroOrHeroSkill && !(_skill == s))
			{
				hasNew = true;
				break;
			}
		}
		newObject.SetActive(hasNew);
	}
}
