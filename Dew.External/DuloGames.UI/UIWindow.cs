using System;
using System.Collections.Generic;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DuloGames.UI;

[DisallowMultipleComponent]
[ExecuteInEditMode]
[AddComponentMenu("UI/Window", 58)]
[RequireComponent(typeof(CanvasGroup))]
public class UIWindow : MonoBehaviour, IEventSystemHandler, ISelectHandler, IPointerDownHandler
{
	public enum Transition
	{
		Instant,
		Fade
	}

	public enum VisualState
	{
		Shown,
		Hidden
	}

	public enum EscapeKeyAction
	{
		None,
		Hide,
		HideIfFocused,
		Toggle
	}

	[Serializable]
	public class TransitionBeginEvent : UnityEvent<UIWindow, VisualState, bool>
	{
	}

	[Serializable]
	public class TransitionCompleteEvent : UnityEvent<UIWindow, VisualState>
	{
	}

	protected static UIWindow m_FucusedWindow;

	[SerializeField]
	private UIWindowID m_WindowId;

	[SerializeField]
	private int m_CustomWindowId;

	[SerializeField]
	private VisualState m_StartingState = VisualState.Hidden;

	[SerializeField]
	private EscapeKeyAction m_EscapeKeyAction = EscapeKeyAction.Hide;

	[SerializeField]
	private bool m_UseBlackOverlay;

	[SerializeField]
	private bool m_FocusAllowReparent = true;

	[SerializeField]
	private Transition m_Transition;

	[SerializeField]
	private TweenEasing m_TransitionEasing = TweenEasing.InOutQuint;

	[SerializeField]
	private float m_TransitionDuration = 0.1f;

	protected bool m_IsFocused;

	private VisualState m_CurrentVisualState = VisualState.Hidden;

	private CanvasGroup m_CanvasGroup;

	public TransitionBeginEvent onTransitionBegin = new TransitionBeginEvent();

	public TransitionCompleteEvent onTransitionComplete = new TransitionCompleteEvent();

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

	public static UIWindow FocusedWindow
	{
		get
		{
			return m_FucusedWindow;
		}
		private set
		{
			m_FucusedWindow = value;
		}
	}

	public UIWindowID ID
	{
		get
		{
			return m_WindowId;
		}
		set
		{
			m_WindowId = value;
		}
	}

	public int CustomID
	{
		get
		{
			return m_CustomWindowId;
		}
		set
		{
			m_CustomWindowId = value;
		}
	}

	public EscapeKeyAction escapeKeyAction
	{
		get
		{
			return m_EscapeKeyAction;
		}
		set
		{
			m_EscapeKeyAction = value;
		}
	}

	public bool useBlackOverlay
	{
		get
		{
			return m_UseBlackOverlay;
		}
		set
		{
			m_UseBlackOverlay = value;
			if (!Application.isPlaying || !m_UseBlackOverlay || !base.isActiveAndEnabled)
			{
				return;
			}
			UIBlackOverlay overlay = UIBlackOverlay.GetOverlay(base.gameObject);
			if (overlay != null)
			{
				if (value)
				{
					onTransitionBegin.AddListener(overlay.OnTransitionBegin);
				}
				else
				{
					onTransitionBegin.RemoveListener(overlay.OnTransitionBegin);
				}
			}
		}
	}

	public bool focusAllowReparent
	{
		get
		{
			return m_FocusAllowReparent;
		}
		set
		{
			m_FocusAllowReparent = value;
		}
	}

	public Transition transition
	{
		get
		{
			return m_Transition;
		}
		set
		{
			m_Transition = value;
		}
	}

	public TweenEasing transitionEasing
	{
		get
		{
			return m_TransitionEasing;
		}
		set
		{
			m_TransitionEasing = value;
		}
	}

	public float transitionDuration
	{
		get
		{
			return m_TransitionDuration;
		}
		set
		{
			m_TransitionDuration = value;
		}
	}

	public bool IsVisible
	{
		get
		{
			if (!(m_CanvasGroup != null) || !(m_CanvasGroup.alpha > 0f))
			{
				return false;
			}
			return true;
		}
	}

	public bool IsOpen => m_CurrentVisualState == VisualState.Shown;

	public bool IsFocused => m_IsFocused;

	public static int NextUnusedCustomID
	{
		get
		{
			List<UIWindow> windows = GetWindows();
			if (GetWindows().Count > 0)
			{
				windows.Sort(SortByCustomWindowID);
				return windows[windows.Count - 1].CustomID + 1;
			}
			return 0;
		}
	}

	protected UIWindow()
	{
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_FloatTweenRunner.Init(this);
	}

	protected virtual void Awake()
	{
		m_CanvasGroup = base.gameObject.GetComponent<CanvasGroup>();
		if (Application.isPlaying)
		{
			ApplyVisualState(m_StartingState);
		}
	}

	protected virtual void Start()
	{
		if (CustomID == 0)
		{
			CustomID = NextUnusedCustomID;
		}
		if (m_EscapeKeyAction != 0 && global::UnityEngine.Object.FindObjectOfType<UIWindowManager>() == null)
		{
			GameObject obj = new GameObject("Window Manager");
			obj.AddComponent<UIWindowManager>();
			obj.transform.SetAsFirstSibling();
		}
	}

	protected virtual void OnEnable()
	{
		if (Application.isPlaying && m_UseBlackOverlay)
		{
			UIBlackOverlay overlay = UIBlackOverlay.GetOverlay(base.gameObject);
			if (overlay != null)
			{
				onTransitionBegin.AddListener(overlay.OnTransitionBegin);
			}
		}
	}

	protected virtual void OnDisable()
	{
		if (Application.isPlaying && m_UseBlackOverlay)
		{
			UIBlackOverlay overlay = UIBlackOverlay.GetOverlay(base.gameObject);
			if (overlay != null)
			{
				onTransitionBegin.RemoveListener(overlay.OnTransitionBegin);
			}
		}
	}

	protected virtual bool IsActive()
	{
		if (base.enabled)
		{
			return base.gameObject.activeInHierarchy;
		}
		return false;
	}

	public virtual void OnSelect(BaseEventData eventData)
	{
		Focus();
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		Focus();
	}

	public virtual void Focus()
	{
		if (!m_IsFocused)
		{
			m_IsFocused = true;
			OnBeforeFocusWindow(this);
			BringToFront();
		}
	}

	public void BringToFront()
	{
		UIUtility.BringToFront(base.gameObject, m_FocusAllowReparent);
	}

	public virtual void Toggle()
	{
		if (m_CurrentVisualState == VisualState.Shown)
		{
			Hide();
		}
		else
		{
			Show();
		}
	}

	public virtual void Show()
	{
		Show(instant: false);
	}

	public virtual void Show(bool instant)
	{
		if (IsActive())
		{
			Focus();
			if (m_CurrentVisualState != 0)
			{
				EvaluateAndTransitionToVisualState(VisualState.Shown, instant);
			}
		}
	}

	public virtual void Hide()
	{
		Hide(instant: false);
	}

	public virtual void Hide(bool instant)
	{
		if (IsActive() && m_CurrentVisualState != VisualState.Hidden)
		{
			EvaluateAndTransitionToVisualState(VisualState.Hidden, instant);
		}
	}

	protected virtual void EvaluateAndTransitionToVisualState(VisualState state, bool instant)
	{
		float targetAlpha = ((state == VisualState.Shown) ? 1f : 0f);
		if (onTransitionBegin != null)
		{
			onTransitionBegin.Invoke(this, state, instant || m_Transition == Transition.Instant);
		}
		if (m_Transition == Transition.Fade)
		{
			float duration = (instant ? 0f : m_TransitionDuration);
			StartAlphaTween(targetAlpha, duration, ignoreTimeScale: true);
		}
		else
		{
			SetCanvasAlpha(targetAlpha);
			if (onTransitionComplete != null)
			{
				onTransitionComplete.Invoke(this, state);
			}
		}
		m_CurrentVisualState = state;
		if (state == VisualState.Shown)
		{
			m_CanvasGroup.blocksRaycasts = true;
		}
	}

	public virtual void ApplyVisualState(VisualState state)
	{
		float targetAlpha = ((state == VisualState.Shown) ? 1f : 0f);
		SetCanvasAlpha(targetAlpha);
		m_CurrentVisualState = state;
		if (state == VisualState.Shown)
		{
			m_CanvasGroup.blocksRaycasts = true;
		}
		else
		{
			m_CanvasGroup.blocksRaycasts = false;
		}
	}

	public void StartAlphaTween(float targetAlpha, float duration, bool ignoreTimeScale)
	{
		if (!(m_CanvasGroup == null))
		{
			FloatTween floatTween = default(FloatTween);
			floatTween.duration = duration;
			floatTween.startFloat = m_CanvasGroup.alpha;
			floatTween.targetFloat = targetAlpha;
			FloatTween floatTween2 = floatTween;
			floatTween2.AddOnChangedCallback(SetCanvasAlpha);
			floatTween2.AddOnFinishCallback(OnTweenFinished);
			floatTween2.ignoreTimeScale = ignoreTimeScale;
			floatTween2.easing = m_TransitionEasing;
			m_FloatTweenRunner.StartTween(floatTween2);
		}
	}

	public void SetCanvasAlpha(float alpha)
	{
		if (!(m_CanvasGroup == null))
		{
			m_CanvasGroup.alpha = alpha;
			if (alpha == 0f)
			{
				m_CanvasGroup.blocksRaycasts = false;
			}
		}
	}

	protected virtual void OnTweenFinished()
	{
		if (onTransitionComplete != null)
		{
			onTransitionComplete.Invoke(this, m_CurrentVisualState);
		}
	}

	public static List<UIWindow> GetWindows()
	{
		List<UIWindow> windows = new List<UIWindow>();
		UIWindow[] array = Resources.FindObjectsOfTypeAll<UIWindow>();
		foreach (UIWindow w in array)
		{
			if (w.gameObject.activeInHierarchy)
			{
				windows.Add(w);
			}
		}
		return windows;
	}

	public static int SortByCustomWindowID(UIWindow w1, UIWindow w2)
	{
		return w1.CustomID.CompareTo(w2.CustomID);
	}

	public static UIWindow GetWindow(UIWindowID id)
	{
		foreach (UIWindow window in GetWindows())
		{
			if (window.ID == id)
			{
				return window;
			}
		}
		return null;
	}

	public static UIWindow GetWindowByCustomID(int customId)
	{
		foreach (UIWindow window in GetWindows())
		{
			if (window.CustomID == customId)
			{
				return window;
			}
		}
		return null;
	}

	public static void FocusWindow(UIWindowID id)
	{
		if (GetWindow(id) != null)
		{
			GetWindow(id).Focus();
		}
	}

	protected static void OnBeforeFocusWindow(UIWindow window)
	{
		if (m_FucusedWindow != null)
		{
			m_FucusedWindow.m_IsFocused = false;
		}
		m_FucusedWindow = window;
	}
}
