using UnityEngine;
using UnityEngine.UI;

[LogicUpdatePriority(3000)]
public class UI_InGame_Scoreboard_PlayerItem_Skill_Gem : LogicBehaviour, IPingableUIElement
{
	public int index;

	public Image icon;

	private UI_InGame_Scoreboard_PlayerItem_Skill _skill;

	private UI_InGame_ScoreboardView _view;

	private UI_InGame_Scoreboard_PlayerItem _item;

	Object IPingableUIElement.pingTarget
	{
		get
		{
			Hero h = _item.hero;
			if (h == null || !h.isActive)
			{
				return null;
			}
			return h.Skill.GetGem(new GemLocation(_skill.type, index));
		}
	}

	private void Start()
	{
		_view.onShow.AddListener(UpdateInfo);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_skill = GetComponentInParent<UI_InGame_Scoreboard_PlayerItem_Skill>();
		_item = GetComponentInParent<UI_InGame_Scoreboard_PlayerItem>();
		_view = GetComponentInParent<UI_InGame_ScoreboardView>();
		UpdateInfo();
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
			Gem g = h.Skill.GetGem(new GemLocation(_skill.type, index));
			icon.gameObject.SetActive(g != null);
			if (g != null)
			{
				icon.sprite = g.icon;
			}
		}
	}
}
