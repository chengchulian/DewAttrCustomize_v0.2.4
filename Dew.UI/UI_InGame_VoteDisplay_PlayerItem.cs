using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[LogicUpdatePriority(1520)]
public class UI_InGame_VoteDisplay_PlayerItem : LogicBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public GameObject localObject;

	public GameObject notReadyObject;

	public GameObject readyObject;

	[NonSerialized]
	public DewPlayer player;

	private void Start()
	{
		if (!(player == null))
		{
			if (localObject != null)
			{
				localObject.SetActive(player.isLocalPlayer);
			}
			if (player.isLocalPlayer)
			{
				base.transform.localScale *= 1.35f;
			}
			UpdateStatus();
		}
	}

	private void Update()
	{
		UpdateStatus();
	}

	private void UpdateStatus()
	{
		if (player == null || !player.isActiveAndEnabled || player.hero.IsNullInactiveDeadOrKnockedOut())
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		notReadyObject.SetActive(!player.isReady);
		if (!readyObject.activeSelf && player.isReady)
		{
			readyObject.SetActive(value: true);
			readyObject.transform.DOKill(complete: true);
			readyObject.transform.DOPunchScale(Vector3.one * 0.4f, 0.3f);
		}
		else if (readyObject.activeSelf && !player.isReady)
		{
			readyObject.SetActive(value: false);
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		tooltip.ShowRawTextTooltip(null, ChatManager.GetColoredDescribedPlayerName(player));
	}
}
