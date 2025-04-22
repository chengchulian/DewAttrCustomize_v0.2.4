using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[LogicUpdatePriority(1520)]
public class UI_InGame_VoteDisplay : LogicBehaviour
{
	[Header("Vote Types")]
	public GameObject nextNodeObject;

	public GameObject nextZoneObject;

	public GameObject sideTrackObject;

	public TextMeshProUGUI sideTrackText;

	public TextMeshProUGUI sideTrackSubtitleText;

	public Image sideTrackBackdrop;

	[Header("Extras")]
	public CanvasGroup partyDisplayCg;

	public TextMeshProUGUI startedVoteText;

	public TextMeshProUGUI movingInSecondsText;

	public UI_InGame_VoteDisplay_PlayerItem playerItemPrefab;

	public Transform playerItemParent;

	public RectTransform worldContainer;

	public Transform actionDisplay;

	private int _lastRemainingSeconds;

	private Color _subtitleColor;

	private CanvasGroup _cg;

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
		_subtitleColor = movingInSecondsText.color;
	}

	private void Start()
	{
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnVoteStarted += new Action<DewPlayer>(ClientEventOnVoteStarted);
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnVoteCanceled += new Action<DewPlayer>(ClientEventOnVoteCanceled);
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnVoteCompleted += new Action(ClientEventOnVoteCompleted);
		Hide();
		GameManager.CallOnReady(delegate
		{
			foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
			{
				humanPlayer.ClientEvent_OnIsReadyChanged += new Action<bool>(ClientEventOnIsReadyChanged);
			}
		});
	}

	private void Update()
	{
		if (nextNodeObject.activeSelf)
		{
			UpdateWorldContainerPos();
		}
	}

	private void UpdateWorldContainerPos()
	{
		List<RectTransform> list = InGameUIManager.instance.miniWorldMapNodeItems;
		int currIndex = NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex;
		int nextIndex = NetworkedManagerBase<ZoneManager>.instance.voteData;
		if (currIndex >= 0 && currIndex < list.Count && nextIndex >= 0 && nextIndex < list.Count)
		{
			RectTransform curr = list[currIndex];
			RectTransform next = list[nextIndex];
			if (!(curr == null) && !(next == null))
			{
				Vector2 pos = (curr.anchorMin + next.anchorMin) * 0.5f;
				worldContainer.pivot = pos;
				worldContainer.anchoredPosition = Vector2.zero;
			}
		}
	}

	private void OnDestroy()
	{
		foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
		{
			humanPlayer.ClientEvent_OnIsReadyChanged -= new Action<bool>(ClientEventOnIsReadyChanged);
		}
	}

	private void ClientEventOnIsReadyChanged(bool obj)
	{
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Vote_PlaceVote");
	}

	private void ClientEventOnVoteCompleted()
	{
		Hide();
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Vote_Complete");
	}

	private void ClientEventOnVoteCanceled(DewPlayer obj)
	{
		Hide();
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Vote_Canceled");
	}

	private void Hide()
	{
		base.transform.DOKill(complete: true);
		base.gameObject.SetActive(value: false);
		partyDisplayCg.DOFade(1f, 0.5f);
		SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
		for (int i = playerItemParent.childCount - 1; i >= 0; i--)
		{
			global::UnityEngine.Object.Destroy(playerItemParent.GetChild(i).gameObject);
		}
	}

	private void ClientEventOnVoteStarted(DewPlayer obj)
	{
		nextNodeObject.SetActive(NetworkedManagerBase<ZoneManager>.instance.voteType == VoteType.NextNode);
		nextZoneObject.SetActive(NetworkedManagerBase<ZoneManager>.instance.voteType == VoteType.NextZone);
		sideTrackObject.SetActive(NetworkedManagerBase<ZoneManager>.instance.voteType == VoteType.Sidetrack);
		if (sideTrackObject.activeSelf)
		{
			Rift_Sidetrack rift = NetworkedManagerBase<ZoneManager>.instance.GetVoteSidetrackRift();
			sideTrackText.text = UI_InGame_Interact_Rift.GetRiftNameText(rift);
			float h = 0f;
			float s = 0f;
			float v = 0f;
			if (NetworkedManagerBase<ZoneManager>.instance.isSidetracking)
			{
				Color.RGBToHSV(NetworkedManagerBase<ZoneManager>.instance.currentZone.mainColor, out h, out s, out v);
				sideTrackSubtitleText.text = DewLocalization.GetUIValue("Generic_TheRapids");
			}
			else
			{
				if (rift != null)
				{
					Color.RGBToHSV(rift.mainColor, out h, out s, out v);
				}
				sideTrackSubtitleText.text = DewLocalization.GetUIValue("Generic_Otherworld");
			}
			sideTrackText.color = Color.HSVToRGB(h, s * 0.3f, v * 0.3f + 0.7f);
			sideTrackBackdrop.color = Color.HSVToRGB(h, 0.35f + s * 0.65f, v);
		}
		base.gameObject.SetActive(value: true);
		_cg.alpha = 0f;
		partyDisplayCg.DOFade(0f, 0.5f);
		Transform obj2 = base.transform;
		obj2.DOKill(complete: true);
		Vector3 op = obj2.localPosition;
		obj2.localPosition = op + Vector3.up * 200f;
		obj2.DOLocalMove(op, 1f);
		string template = DewLocalization.GetUIValue("InGame_Vote_StartedTravelVote");
		startedVoteText.text = string.Format(template, ChatManager.GetColoredDescribedPlayerName(obj));
		UpdateStatus();
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Vote_Start");
		SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
		List<DewPlayer> list = new List<DewPlayer>(DewPlayer.humanPlayers);
		list.Sort(delegate(DewPlayer x, DewPlayer y)
		{
			int num = (x.isLocalPlayer ? (-100) : ((int)x.netId));
			int value = (y.isLocalPlayer ? (-100) : ((int)y.netId));
			return num.CompareTo(value);
		});
		foreach (DewPlayer h2 in list)
		{
			if (!h2.hero.IsNullInactiveDeadOrKnockedOut())
			{
				global::UnityEngine.Object.Instantiate(playerItemPrefab, playerItemParent).player = h2;
			}
		}
		UpdateWorldContainerPos();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		UpdateStatus();
		bool shouldShow = true;
		_cg.alpha = Mathf.MoveTowards(_cg.alpha, shouldShow ? 1 : 0, 8f * dt);
	}

	private void UpdateStatus()
	{
		if (_lastRemainingSeconds == NetworkedManagerBase<ZoneManager>.instance.voteRemainingSeconds || !base.isActiveAndEnabled)
		{
			return;
		}
		_lastRemainingSeconds = NetworkedManagerBase<ZoneManager>.instance.voteRemainingSeconds;
		string template = DewLocalization.GetUIValue("InGame_Vote_TravelingInSeconds");
		movingInSecondsText.text = string.Format(template, _lastRemainingSeconds);
		if (_lastRemainingSeconds <= 5)
		{
			if (_lastRemainingSeconds != 0)
			{
				ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Vote_TimeTicking");
			}
			movingInSecondsText.DOKill(complete: true);
			movingInSecondsText.color = Color.white;
			movingInSecondsText.DOColor(_subtitleColor, 0.5f);
		}
		else
		{
			movingInSecondsText.color = _subtitleColor;
		}
	}
}
