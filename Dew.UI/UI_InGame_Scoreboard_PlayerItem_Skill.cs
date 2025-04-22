using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[LogicUpdatePriority(3000)]
public class UI_InGame_Scoreboard_PlayerItem_Skill : LogicBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPingableUIElement
{
	public HeroSkillLocation type;

	public GameObject[] gemObjects234;

	public Image skillIcon;

	public GameObject hasSkillObject;

	public GameObject noSkillObject;

	public GameObject multipleChargesObject;

	public TextMeshProUGUI chargeCountText;

	public TextMeshProUGUI activationKeyText;

	public Transform tooltipPivot;

	private UI_InGame_ScoreboardView _view;

	private UI_InGame_Scoreboard_PlayerItem _item;

	public Hero hero => _item.hero;

	Object IPingableUIElement.pingTarget
	{
		get
		{
			Hero h = _item.hero;
			if (h == null || !h.isActive)
			{
				return null;
			}
			return h.Skill.GetSkill(type);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_item = GetComponentInParent<UI_InGame_Scoreboard_PlayerItem>();
		_view = GetComponentInParent<UI_InGame_ScoreboardView>();
		UpdateInfo();
	}

	private void Start()
	{
		_view.onShow.AddListener(UpdateInfo);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (_view.isShowing)
		{
			UpdateInfo();
		}
	}

	private void UpdateInfo()
	{
		Hero h = _item.hero;
		if (!(h == null) && h.isActive)
		{
			for (int i = 0; i < gemObjects234.Length; i++)
			{
				gemObjects234[i].SetActive(h.Skill.GetMaxGemCount(type) == i + 2);
			}
			SkillTrigger skill = h.Skill.GetSkill(type);
			hasSkillObject.SetActive(skill != null);
			noSkillObject.SetActive(skill == null);
			if (skill != null)
			{
				skillIcon.sprite = skill.configs[0].triggerIcon;
				multipleChargesObject.SetActive(skill.configs[0].maxCharges > 1);
				chargeCountText.text = skill.configs[0].maxCharges.ToString();
			}
			activationKeyText.text = DewInput.GetReadableTextForCurrentMode(ManagerBase<ControlManager>.instance.GetSkillBinding(type));
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		SkillTrigger skill = hero.Skill.GetSkill(type);
		tooltip.ShowSkillTooltip(tooltipPivot.position, skill, hero);
	}
}
