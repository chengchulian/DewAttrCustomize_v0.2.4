using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[LogicUpdatePriority(1050)]
public class UI_Lobby_HeroList_Item : LogicBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Transform tooltipPivot;

	public Hero hero;

	public UI_HeroIcon icon;

	public GameObject selectedObjects;

	public GameObject unselectedObjects;

	public GameObject lockedObjects;

	public GameObject newObjects;

	private CanvasGroup _cg;

	private bool _isLocked;

	private string _heroName;

	private void Start()
	{
		_cg = icon.GetComponent<CanvasGroup>();
		_heroName = hero.GetType().Name;
		icon.Setup(_heroName);
		DewProfile.UnlockData h = DewSave.profile.heroes[_heroName];
		_isLocked = !h.isAvailableInGame;
		newObjects.SetActive(h.isNewHeroOrHeroSkill);
		lockedObjects.SetActive(_isLocked);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (!(DewPlayer.local == null))
		{
			bool isSelected = DewPlayer.local.selectedHeroType == _heroName;
			if (isSelected != selectedObjects.activeSelf || isSelected != !unselectedObjects.activeSelf)
			{
				UpdateSelectedStatus();
			}
			if (newObjects.activeSelf && isSelected)
			{
				newObjects.SetActive(value: false);
				DewSave.profile.heroes[_heroName].isNewHeroOrHeroSkill = false;
			}
		}
	}

	private void UpdateSelectedStatus()
	{
		bool isSelected = DewPlayer.local.selectedHeroType == _heroName;
		selectedObjects.SetActive(isSelected);
		unselectedObjects.SetActive(!isSelected);
		_cg.alpha = (isSelected ? 1f : 0.25f);
		icon.transform.DOKill();
		icon.transform.DOScale(isSelected ? Vector3.one : (Vector3.one * 0.8f), 0.2f);
	}

	public void Click()
	{
		if (!_isLocked)
		{
			SingletonBehaviour<UI_Constellations>.instance.DoActionWithUndoConfirm(delegate
			{
				DewPlayer.local.CmdSetHeroType(_heroName);
				newObjects.SetActive(value: false);
				DewSave.profile.heroes[_heroName].isNewHeroOrHeroSkill = false;
			});
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		tooltip.ShowCollectableTooltip(new TooltipSettings
		{
			mode = TooltipPositionMode.RawValue,
			position = tooltipPivot.position,
			pivot = new Vector2(0.5f, 1f)
		}, hero.GetType());
	}
}
