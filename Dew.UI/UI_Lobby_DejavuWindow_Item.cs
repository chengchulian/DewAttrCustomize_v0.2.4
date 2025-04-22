using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Lobby_DejavuWindow_Item : MonoBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public UI_Achievement_Icon icon;

	public GameObject lockedObject;

	public CostDisplay costDisplay;

	public GameObject[] onStars;

	public GameObject[] offStars;

	public GameObject[] perfectStars;

	public GameObject freeObject;

	[NonSerialized]
	public global::UnityEngine.Object target;

	private UI_Toggle _toggle;

	private void Awake()
	{
		_toggle = GetComponentInChildren<UI_Toggle>();
		_toggle.doNotToggleOnClick = true;
		_toggle.onClick.AddListener(OnClick);
		Dew.CallOnReady(this, () => DewPlayer.local != null, delegate
		{
			DewPlayer.local.ClientEvent_OnSelectedDejavuItemChanged += new Action<string>(OnLocalClientEventOnSelectedDejavuItemChanged);
		});
	}

	private void OnLocalClientEventOnSelectedDejavuItemChanged(string s)
	{
		_toggle.isChecked = s == target.GetType().Name;
	}

	private void OnDestroy()
	{
		if (DewPlayer.local != null)
		{
			DewPlayer.local.ClientEvent_OnSelectedDejavuItemChanged -= new Action<string>(OnLocalClientEventOnSelectedDejavuItemChanged);
		}
	}

	private void OnClick()
	{
		if (lockedObject.activeSelf)
		{
			return;
		}
		if (DewPlayer.local.selectedDejavuItem == target.GetType().Name)
		{
			NetworkedManagerBase<GameSettingsManager>.instance.localPlayerDejavuCost = 0;
			DewPlayer.local.CmdSetDejavuItem(null);
			return;
		}
		if (Dew.IsDejavuFree(target))
		{
			NetworkedManagerBase<GameSettingsManager>.instance.localPlayerDejavuCost = 0;
		}
		else
		{
			NetworkedManagerBase<GameSettingsManager>.instance.localPlayerDejavuCost = Dew.GetDejavuCost(target);
		}
		DewPlayer.local.CmdSetDejavuItem(target.GetType().Name);
	}

	public void Setup(global::UnityEngine.Object item, int index)
	{
		target = item;
		icon.SetupByItem(target);
		if (target is SkillTrigger)
		{
			icon.transform.localScale *= 0.9f;
		}
		if (target is Gem)
		{
			icon.transform.localScale *= 0.8f;
		}
		costDisplay.Setup(new Cost
		{
			stardust = Dew.GetDejavuCost(item)
		});
		_toggle.index = index;
		if (DewSave.profile.itemStatistics.TryGetValue(item.GetType().Name, out var data))
		{
			lockedObject.SetActive(data.wins <= 0);
			costDisplay.gameObject.SetActive(value: false);
			int maxWins = Dew.GetDejavuMaxWins(item);
			if (data.wins >= maxWins)
			{
				onStars.SetActiveAll(value: false);
				for (int i = 0; i < perfectStars.Length; i++)
				{
					perfectStars[i].SetActive(maxWins > i);
				}
			}
			else
			{
				perfectStars.SetActiveAll(value: false);
				for (int j = 0; j < onStars.Length; j++)
				{
					onStars[j].SetActive(data.wins > j);
				}
			}
			for (int k = 0; k < offStars.Length; k++)
			{
				offStars[k].SetActive(k < maxWins - data.wins);
			}
		}
		else
		{
			lockedObject.SetActive(value: true);
		}
		freeObject.SetActive(Dew.IsDejavuFree(target));
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (target is SkillTrigger st)
		{
			tooltip.ShowSkillTooltipForDejavu((Func<Vector2>)(() => base.transform.position), st);
		}
		else if (target is Gem g)
		{
			tooltip.ShowGemTooltipForDejavu((Func<Vector2>)(() => base.transform.position), g);
		}
	}
}
