using System;
using DuloGames.UI.Tweens;
using UnityEngine;

namespace DuloGames.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasGroup))]
public class UIBlackOverlay : MonoBehaviour
{
	private CanvasGroup m_CanvasGroup;

	private int m_WindowsCount;

	private bool m_Transitioning;

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

	protected UIBlackOverlay()
	{
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_FloatTweenRunner.Init(this);
	}

	protected void Awake()
	{
		m_CanvasGroup = base.gameObject.GetComponent<CanvasGroup>();
		m_CanvasGroup.interactable = false;
		m_CanvasGroup.blocksRaycasts = false;
	}

	protected void Start()
	{
		m_CanvasGroup.interactable = false;
		Hide();
	}

	protected void OnEnable()
	{
		if (!Application.isPlaying)
		{
			Hide();
		}
	}

	public void Show()
	{
		SetAlpha(1f);
		m_CanvasGroup.blocksRaycasts = true;
	}

	public void Hide()
	{
		SetAlpha(0f);
		m_CanvasGroup.blocksRaycasts = false;
	}

	public bool IsActive()
	{
		if (base.enabled)
		{
			return base.gameObject.activeInHierarchy;
		}
		return false;
	}

	public bool IsVisible()
	{
		return m_CanvasGroup.alpha > 0f;
	}

	public void OnTransitionBegin(UIWindow window, UIWindow.VisualState state, bool instant)
	{
		if (!IsActive() || window == null || (state == UIWindow.VisualState.Hidden && !IsVisible()))
		{
			return;
		}
		float duration = (instant ? 0f : window.transitionDuration);
		TweenEasing easing = window.transitionEasing;
		if (state == UIWindow.VisualState.Shown)
		{
			m_WindowsCount++;
			if (IsVisible() && !m_Transitioning)
			{
				UIUtility.BringToFront(window.gameObject);
				return;
			}
			UIUtility.BringToFront(base.gameObject);
			UIUtility.BringToFront(window.gameObject);
			StartAlphaTween(1f, duration, easing);
			m_CanvasGroup.blocksRaycasts = true;
			return;
		}
		m_WindowsCount--;
		if (m_WindowsCount < 0)
		{
			m_WindowsCount = 0;
		}
		if (m_WindowsCount <= 0)
		{
			StartAlphaTween(0f, duration, easing);
			m_CanvasGroup.blocksRaycasts = false;
		}
	}

	private void StartAlphaTween(float targetAlpha, float duration, TweenEasing easing)
	{
		if (!(m_CanvasGroup == null))
		{
			if (m_Transitioning)
			{
				m_FloatTweenRunner.StopTween();
			}
			if (duration == 0f || !Application.isPlaying)
			{
				SetAlpha(targetAlpha);
				return;
			}
			m_Transitioning = true;
			FloatTween floatTween = default(FloatTween);
			floatTween.duration = duration;
			floatTween.startFloat = m_CanvasGroup.alpha;
			floatTween.targetFloat = targetAlpha;
			FloatTween floatTween2 = floatTween;
			floatTween2.AddOnChangedCallback(SetAlpha);
			floatTween2.ignoreTimeScale = true;
			floatTween2.easing = easing;
			floatTween2.AddOnFinishCallback(OnTweenFinished);
			m_FloatTweenRunner.StartTween(floatTween2);
		}
	}

	public void SetAlpha(float alpha)
	{
		if (m_CanvasGroup != null)
		{
			m_CanvasGroup.alpha = alpha;
		}
	}

	protected void OnTweenFinished()
	{
		m_Transitioning = false;
	}

	public static UIBlackOverlay GetOverlay(GameObject relativeGameObject)
	{
		Canvas canvas = UIUtility.FindInParents<Canvas>(relativeGameObject);
		if (canvas != null)
		{
			UIBlackOverlay overlay = canvas.gameObject.GetComponentInChildren<UIBlackOverlay>();
			if (overlay != null)
			{
				return overlay;
			}
			if (UIBlackOverlayManager.Instance != null && UIBlackOverlayManager.Instance.prefab != null)
			{
				return UIBlackOverlayManager.Instance.Create(canvas.transform);
			}
		}
		return null;
	}
}
