using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

[LogicUpdatePriority(400)]
public class UI_InGame_AchievementNotifications : LogicBehaviour
{
	public TextMeshProUGUI title;

	public TextMeshProUGUI description;

	public TextMeshProUGUI addedStarDust;

	public UI_Achievement_Icon icon;

	public UI_Achievement_AddedToCollectablesText addedToCollectablesText;

	public Vector2 startAnchoredPos;

	public float startTime;

	public float sustainTime;

	public float decayTime;

	public float alphaTime;

	private Animator[] _animators;

	private Vector2 _originalAnchoredPos;

	private CanvasGroup _cg;

	private bool _isBusy;

	private Queue<Type> _queue = new Queue<Type>();

	private void Awake()
	{
		_originalAnchoredPos = ((RectTransform)base.transform).anchoredPosition;
		_cg = GetComponent<CanvasGroup>();
		_cg.alpha = 0f;
		_animators = GetComponentsInChildren<Animator>();
	}

	private void Start()
	{
		ManagerBase<AchievementManager>.instance.LocalClientEvent_OnAchievementComplete += (Action<Type>)delegate(Type obj)
		{
			_queue.Enqueue(obj);
			ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_AchievementComplete");
		};
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (_queue.Count > 0 && !_isBusy)
		{
			DoNotification(_queue.Dequeue());
		}
	}

	private void DoNotification(Type obj)
	{
		_isBusy = true;
		try
		{
			icon.Setup(obj);
			List<Type> unlocked = Dew.GetUnlockedTargetsOfAchievement(obj);
			if (unlocked.Count > 0)
			{
				global::UnityEngine.Object target = DewResources.GetByType(unlocked[0]);
				addedToCollectablesText.Setup(target);
			}
			title.color = Dew.GetAchievementColor(obj);
			title.text = DewLocalization.GetAchievementName(obj.Name);
			description.text = DewLocalization.GetAchievementDescription(obj.Name);
			int gainedStarDust = ((DewAchievementItem)Activator.CreateInstance(obj)).grantedStardust;
			addedStarDust.text = string.Format("+{0:#,##0} {1}", gainedStarDust, DewLocalization.GetUIValue("InGame_Currency_Stardust"));
			DOTween.Kill(this);
			_cg.DOKill();
			RectTransform rt = (RectTransform)base.transform;
			rt.anchoredPosition = startAnchoredPos;
			_cg.alpha = 0f;
			_cg.DOFade(1f, alphaTime);
			Animator[] animators = _animators;
			for (int i = 0; i < animators.Length; i++)
			{
				animators[i].enabled = true;
			}
			DOTween.Sequence().SetId(this).Append(rt.DOAnchorPos(_originalAnchoredPos, startTime))
				.AppendInterval(sustainTime)
				.AppendCallback(delegate
				{
					_cg.DOFade(0f, alphaTime);
				})
				.Append(rt.DOAnchorPos(startAnchoredPos, decayTime))
				.AppendCallback(delegate
				{
					_isBusy = false;
				})
				.AppendCallback(delegate
				{
					Animator[] animators2 = _animators;
					for (int j = 0; j < animators2.Length; j++)
					{
						animators2[j].enabled = false;
					}
				});
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
			_isBusy = false;
		}
	}
}
