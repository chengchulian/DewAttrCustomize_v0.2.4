using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InGame_QuestDisplay_Item : MonoBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public static UI_InGame_QuestDisplay_Item animatingItem;

	public RectTransform animationTarget;

	public RectTransform tooltipPivot;

	public GameObject goalObject;

	public GameObject curseObject;

	public GameObject tutorialObject;

	public GameObject questObject;

	public Image headerBackdrop;

	public GameObject progressObject;

	public TextMeshProUGUI progressText;

	public GameObject progressKillsObject;

	public GameObject progressTravelObject;

	public GameObject fxComplete;

	public GameObject fxFailed;

	public Color goalTitleColor;

	public Color curseTitleColor;

	public Color tutorialTitleColor;

	public Color questTitleColor;

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI descText;

	public TextMeshProUGUI questFailReasonText;

	public CanvasGroup updateFlash;

	public Image updateFlashImage;

	public Color updateFlashTint;

	public float updateFlashDecayTime;

	public Image initialFlashesImage;

	public Color initialFlashesTint;

	private bool _isAnimating;

	public DewQuest quest { get; private set; }

	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
		animatingItem = null;
	}

	public void Setup(DewQuest item)
	{
		if (quest != null)
		{
			throw new InvalidOperationException("QuestItem already setup");
		}
		quest = item;
		NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnQuestUpdated += new Action<DewQuest>(ClientEventOnQuestUpdated);
		NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnQuestFailed += new Action<DewQuest, QuestFailReason>(ClientEventOnQuestFailed);
		NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnQuestCompleted += new Action<DewQuest>(ClientEventOnQuestCompleted);
		NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnQuestRemoved += new Action<DewQuest>(ClientEventOnQuestRemoved);
		UpdateContent();
		Vector3 originalScale = animationTarget.localScale;
		animationTarget.localScale = Vector3.zero;
		GameManager.CallOnReady(delegate
		{
			StartCoroutine(Routine());
		});
		IEnumerator Routine()
		{
			_isAnimating = true;
			yield return new WaitForSeconds(0.5f);
			yield return new WaitUntil(CanStartAnimating);
			animatingItem = this;
			ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Game_QuestAdded");
			animationTarget.localScale = originalScale;
			Vector3 toLocalPos = animationTarget.localPosition;
			Quaternion toLocalRot = animationTarget.localRotation;
			animationTarget.DOPunchScale(Vector3.one * 0.1f, 0.5f);
			animationTarget.rotation = Quaternion.identity;
			animationTarget.position = new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.7f, 0f);
			yield return new WaitForSeconds(1.25f);
			animationTarget.DOLocalMove(toLocalPos, 0.5f);
			animationTarget.DOLocalRotateQuaternion(toLocalRot, 0.5f);
			if (animatingItem == this)
			{
				animatingItem = null;
			}
			_isAnimating = false;
		}
	}

	private void ClientEventOnQuestRemoved(DewQuest obj)
	{
		if (!(quest != obj))
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void ClientEventOnQuestUpdated(DewQuest obj)
	{
		if (!(quest != obj))
		{
			UpdateContent();
		}
	}

	private void ClientEventOnQuestCompleted(DewQuest obj)
	{
		if (!(quest != obj))
		{
			GameManager.CallOnReady(delegate
			{
				DoDestroyRoutine(isComplete: true);
			});
		}
	}

	private void ClientEventOnQuestFailed(DewQuest obj, QuestFailReason reason)
	{
		if (!(quest != obj))
		{
			questFailReasonText.text = DewLocalization.GetUIValue("InGame_Quest_Message_QuestFailed_Reason_" + reason);
			GameManager.CallOnReady(delegate
			{
				DoDestroyRoutine(isComplete: false);
			});
		}
	}

	private bool CanStartAnimating()
	{
		if (SingletonBehaviour<UI_InGame_RoomModDisplay>.instance == null || !SingletonBehaviour<UI_InGame_RoomModDisplay>.instance.isAnnouncing)
		{
			return animatingItem == null;
		}
		return false;
	}

	private void DoDestroyRoutine(bool isComplete)
	{
		if (animatingItem == this)
		{
			animatingItem = null;
		}
		StopAllCoroutines();
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			_isAnimating = true;
			yield return new WaitForSeconds(0.5f);
			while (!CanStartAnimating())
			{
				yield return null;
			}
			animatingItem = this;
			progressObject.SetActive(value: false);
			if (base.transform.parent != null)
			{
				((RectTransform)base.transform).SetParent(base.transform.parent.parent, worldPositionStays: true);
			}
			animationTarget.DORotateQuaternion(Quaternion.identity, 0.5f);
			animationTarget.DOMove(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.7f, 0f), 0.5f);
			yield return new WaitForSeconds(0.8f);
			DewEffect.Play(isComplete ? fxComplete : fxFailed);
			updateFlash.alpha = 1f;
			updateFlash.DOFade(0f, updateFlashDecayTime);
			animationTarget.DOPunchScale(Vector3.one * 0.1f, 0.5f);
			yield return new WaitForSeconds(2f);
			animationTarget.DOScale(Vector3.zero, 0.3f);
			yield return new WaitForSeconds(0.3f);
			_isAnimating = false;
			if (animatingItem == this)
			{
				animatingItem = null;
			}
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (!(NetworkedManagerBase<QuestManager>.instance == null))
		{
			NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnQuestUpdated -= new Action<DewQuest>(ClientEventOnQuestUpdated);
			NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnQuestFailed -= new Action<DewQuest, QuestFailReason>(ClientEventOnQuestFailed);
			NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnQuestCompleted -= new Action<DewQuest>(ClientEventOnQuestCompleted);
			NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnQuestRemoved -= new Action<DewQuest>(ClientEventOnQuestRemoved);
		}
	}

	private void UpdateContent()
	{
		switch (quest.type)
		{
		case QuestType.Goal:
			titleText.color = goalTitleColor;
			break;
		case QuestType.Curse:
			titleText.color = curseTitleColor;
			break;
		case QuestType.Tutorial:
			titleText.color = tutorialTitleColor;
			break;
		case QuestType.Quest:
			titleText.color = questTitleColor;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		Color c = titleText.color;
		headerBackdrop.color = c.WithS(0.4f).WithV(c.GetV() * 0.75f);
		updateFlashImage.color = titleText.color * updateFlashTint;
		initialFlashesImage.color = titleText.color * initialFlashesTint;
		goalObject.SetActive(quest.type == QuestType.Goal);
		curseObject.SetActive(quest.type == QuestType.Curse);
		tutorialObject.SetActive(quest.type == QuestType.Tutorial);
		questObject.SetActive(quest.type == QuestType.Quest);
		titleText.text = quest.questTitleRaw;
		descText.text = quest.questShortDescriptionRaw;
		bool hasProgress = quest.progressType != QuestProgressType.Hidden;
		progressObject.SetActive(hasProgress);
		if (hasProgress)
		{
			progressKillsObject.SetActive(quest.progressType == QuestProgressType.Kills);
			progressTravelObject.SetActive(quest.progressType == QuestProgressType.Travel);
			progressText.text = quest.currentProgress;
		}
		updateFlash.alpha = 1f;
		updateFlash.DOFade(0f, updateFlashDecayTime);
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (!_isAnimating)
		{
			tooltip.ShowQuestTooltip((Func<Vector2>)(() => tooltipPivot.position), quest);
		}
	}
}
