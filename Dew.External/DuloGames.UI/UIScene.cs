using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DuloGames.UI;

[DisallowMultipleComponent]
[ExecuteInEditMode]
[RequireComponent(typeof(CanvasGroup))]
[AddComponentMenu("UI/UI Scene/Scene")]
public class UIScene : MonoBehaviour
{
	public enum Type
	{
		Preloaded,
		Prefab,
		Resource
	}

	public enum Transition
	{
		None,
		Animation,
		CrossFade,
		SlideFromRight,
		SlideFromLeft,
		SlideFromTop,
		SlideFromBottom
	}

	[Serializable]
	public class OnActivateEvent : UnityEvent<UIScene>
	{
	}

	[Serializable]
	public class OnDeactivateEvent : UnityEvent<UIScene>
	{
	}

	private UISceneRegistry m_SceneManager;

	private bool m_AnimationState;

	[SerializeField]
	private int m_Id;

	[SerializeField]
	private bool m_IsActivated = true;

	[SerializeField]
	private Type m_Type;

	[SerializeField]
	private Transform m_Content;

	[SerializeField]
	private GameObject m_Prefab;

	[SerializeField]
	[ResourcePath]
	private string m_Resource;

	[SerializeField]
	private Transition m_Transition;

	[SerializeField]
	private float m_TransitionDuration = 0.2f;

	[SerializeField]
	private TweenEasing m_TransitionEasing = TweenEasing.InOutQuint;

	[SerializeField]
	private string m_AnimateInTrigger = "AnimateIn";

	[SerializeField]
	private string m_AnimateOutTrigger = "AnimateOut";

	[SerializeField]
	private GameObject m_FirstSelected;

	public OnActivateEvent onActivate = new OnActivateEvent();

	public OnDeactivateEvent onDeactivate = new OnDeactivateEvent();

	private CanvasGroup m_CanvasGroup;

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

	public int id => m_Id;

	public bool isActivated
	{
		get
		{
			return m_IsActivated;
		}
		set
		{
			if (value)
			{
				Activate();
			}
			else
			{
				Deactivate();
			}
		}
	}

	public Type type => m_Type;

	public Transform content
	{
		get
		{
			return m_Content;
		}
		set
		{
			m_Content = value;
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

	public string animateInTrigger
	{
		get
		{
			return m_AnimateInTrigger;
		}
		set
		{
			m_AnimateInTrigger = value;
		}
	}

	public string animateOutTrigger
	{
		get
		{
			return m_AnimateOutTrigger;
		}
		set
		{
			m_AnimateOutTrigger = value;
		}
	}

	public RectTransform rectTransform => base.transform as RectTransform;

	public Animator animator => base.gameObject.GetComponent<Animator>();

	protected UIScene()
	{
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_FloatTweenRunner.Init(this);
	}

	protected virtual void Awake()
	{
		m_SceneManager = UISceneRegistry.instance;
		if (m_SceneManager == null)
		{
			Debug.LogWarning("Scene registry does not exist.");
			base.enabled = false;
			return;
		}
		m_AnimationState = m_IsActivated;
		m_CanvasGroup = base.gameObject.GetComponent<CanvasGroup>();
		if (Application.isPlaying && isActivated && base.isActiveAndEnabled && m_FirstSelected != null)
		{
			EventSystem.current.SetSelectedGameObject(m_FirstSelected);
		}
	}

	protected virtual void Start()
	{
	}

	protected virtual void OnEnable()
	{
		if (m_SceneManager != null && base.gameObject.activeInHierarchy)
		{
			m_SceneManager.RegisterScene(this);
		}
		if (isActivated && onActivate != null)
		{
			onActivate.Invoke(this);
		}
	}

	protected virtual void OnDisable()
	{
		if (m_SceneManager != null)
		{
			m_SceneManager.UnregisterScene(this);
		}
	}

	protected void Update()
	{
		if (!(animator != null) || string.IsNullOrEmpty(m_AnimateInTrigger) || string.IsNullOrEmpty(m_AnimateOutTrigger))
		{
			return;
		}
		AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
		if (state.IsName(m_AnimateInTrigger) && !m_AnimationState)
		{
			if (state.normalizedTime >= state.length)
			{
				m_AnimationState = true;
				OnTransitionIn();
			}
		}
		else if (state.IsName(m_AnimateOutTrigger) && m_AnimationState && state.normalizedTime >= state.length)
		{
			m_AnimationState = false;
			OnTransitionOut();
		}
	}

	public void Activate()
	{
		if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (m_Type == Type.Prefab || m_Type == Type.Resource)
		{
			GameObject prefab = null;
			if (m_Type == Type.Prefab)
			{
				if (m_Prefab == null)
				{
					Debug.LogWarning("You are activating a prefab scene and no prefab is specified.");
					return;
				}
				prefab = m_Prefab;
			}
			if (m_Type == Type.Resource)
			{
				if (string.IsNullOrEmpty(m_Resource))
				{
					Debug.LogWarning("You are activating a resource scene and no resource path is specified.");
					return;
				}
				prefab = Resources.Load<GameObject>(m_Resource);
			}
			if (prefab != null)
			{
				GameObject obj = global::UnityEngine.Object.Instantiate(prefab);
				m_Content = obj.transform;
				m_Content.SetParent(base.transform);
				if (m_Content is RectTransform)
				{
					RectTransform rectTransform = m_Content as RectTransform;
					rectTransform.localScale = Vector3.one;
					rectTransform.localPosition = Vector3.zero;
					rectTransform.anchorMin = new Vector2(0f, 0f);
					rectTransform.anchorMax = new Vector2(1f, 1f);
					rectTransform.pivot = new Vector2(0.5f, 0.5f);
					Canvas canvas = UIUtility.FindInParents<Canvas>(base.gameObject);
					if (canvas == null)
					{
						canvas = base.gameObject.GetComponentInChildren<Canvas>();
					}
					if (canvas != null)
					{
						RectTransform crt = canvas.transform as RectTransform;
						rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, crt.sizeDelta.x);
						rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, crt.sizeDelta.y);
					}
					rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
				}
			}
		}
		if (m_Content != null)
		{
			m_Content.gameObject.SetActive(value: true);
		}
		if (base.isActiveAndEnabled && m_FirstSelected != null)
		{
			EventSystem.current.SetSelectedGameObject(m_FirstSelected);
		}
		m_IsActivated = true;
		if (onActivate != null)
		{
			onActivate.Invoke(this);
		}
	}

	public void Deactivate()
	{
		if (m_Content != null)
		{
			m_Content.gameObject.SetActive(value: false);
		}
		if (m_Type == Type.Prefab || m_Type == Type.Resource)
		{
			global::UnityEngine.Object.Destroy(m_Content.gameObject);
		}
		Resources.UnloadUnusedAssets();
		m_IsActivated = false;
		if (onDeactivate != null)
		{
			onDeactivate.Invoke(this);
		}
	}

	public void TransitionTo()
	{
		if (base.isActiveAndEnabled && base.gameObject.activeInHierarchy && m_SceneManager != null)
		{
			m_SceneManager.TransitionToScene(this);
		}
	}

	public void TransitionIn()
	{
		TransitionIn(m_Transition, m_TransitionDuration, m_TransitionEasing);
	}

	public void TransitionIn(Transition transition, float duration, TweenEasing easing)
	{
		if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy || m_CanvasGroup == null)
		{
			return;
		}
		switch (transition)
		{
		case Transition.None:
			Activate();
			return;
		case Transition.Animation:
			Activate();
			TriggerAnimation(m_AnimateInTrigger);
			return;
		}
		Vector2 rectSize = rectTransform.rect.size;
		if (transition == Transition.SlideFromLeft || transition == Transition.SlideFromRight || transition == Transition.SlideFromTop || transition == Transition.SlideFromBottom)
		{
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.sizeDelta = rectSize;
		}
		FloatTween floatTween = default(FloatTween);
		floatTween.duration = duration;
		switch (transition)
		{
		case Transition.CrossFade:
			m_CanvasGroup.alpha = 0f;
			floatTween.startFloat = 0f;
			floatTween.targetFloat = 1f;
			floatTween.AddOnChangedCallback(SetCanvasAlpha);
			break;
		case Transition.SlideFromRight:
			rectTransform.anchoredPosition = new Vector2(rectSize.x, 0f);
			floatTween.startFloat = rectSize.x;
			floatTween.targetFloat = 0f;
			floatTween.AddOnChangedCallback(SetPositionX);
			break;
		case Transition.SlideFromLeft:
			rectTransform.anchoredPosition = new Vector2(rectSize.x * -1f, 0f);
			floatTween.startFloat = rectSize.x * -1f;
			floatTween.targetFloat = 0f;
			floatTween.AddOnChangedCallback(SetPositionX);
			break;
		case Transition.SlideFromBottom:
			rectTransform.anchoredPosition = new Vector2(0f, rectSize.y * -1f);
			floatTween.startFloat = rectSize.y * -1f;
			floatTween.targetFloat = 0f;
			floatTween.AddOnChangedCallback(SetPositionY);
			break;
		case Transition.SlideFromTop:
			rectTransform.anchoredPosition = new Vector2(0f, rectSize.y);
			floatTween.startFloat = rectSize.y;
			floatTween.targetFloat = 0f;
			floatTween.AddOnChangedCallback(SetPositionY);
			break;
		}
		Activate();
		floatTween.AddOnFinishCallback(OnTransitionIn);
		floatTween.ignoreTimeScale = true;
		floatTween.easing = easing;
		m_FloatTweenRunner.StartTween(floatTween);
	}

	public void TransitionOut()
	{
		TransitionOut(m_Transition, m_TransitionDuration, m_TransitionEasing);
	}

	public void TransitionOut(Transition transition, float duration, TweenEasing easing)
	{
		if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy || m_CanvasGroup == null)
		{
			return;
		}
		switch (transition)
		{
		case Transition.None:
			Deactivate();
			return;
		case Transition.Animation:
			TriggerAnimation(m_AnimateOutTrigger);
			return;
		}
		Vector2 rectSize = rectTransform.rect.size;
		if (transition == Transition.SlideFromLeft || transition == Transition.SlideFromRight || transition == Transition.SlideFromTop || transition == Transition.SlideFromBottom)
		{
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.sizeDelta = rectSize;
			rectTransform.anchoredPosition = new Vector2(0f, 0f);
		}
		FloatTween floatTween = default(FloatTween);
		floatTween.duration = duration;
		switch (transition)
		{
		case Transition.CrossFade:
			m_CanvasGroup.alpha = 1f;
			floatTween.startFloat = m_CanvasGroup.alpha;
			floatTween.targetFloat = 0f;
			floatTween.AddOnChangedCallback(SetCanvasAlpha);
			break;
		case Transition.SlideFromRight:
			floatTween.startFloat = 0f;
			floatTween.targetFloat = rectSize.x * -1f;
			floatTween.AddOnChangedCallback(SetPositionX);
			break;
		case Transition.SlideFromLeft:
			floatTween.startFloat = 0f;
			floatTween.targetFloat = rectSize.x;
			floatTween.AddOnChangedCallback(SetPositionX);
			break;
		case Transition.SlideFromBottom:
			floatTween.startFloat = 0f;
			floatTween.targetFloat = rectSize.y;
			floatTween.AddOnChangedCallback(SetPositionY);
			break;
		case Transition.SlideFromTop:
			floatTween.startFloat = 0f;
			floatTween.targetFloat = rectSize.y * -1f;
			floatTween.AddOnChangedCallback(SetPositionY);
			break;
		}
		floatTween.AddOnFinishCallback(OnTransitionOut);
		floatTween.ignoreTimeScale = true;
		floatTween.easing = easing;
		m_FloatTweenRunner.StartTween(floatTween);
	}

	public void StartAlphaTween(float targetAlpha, float duration, TweenEasing easing, bool ignoreTimeScale, UnityAction callback)
	{
		if (!(m_CanvasGroup == null))
		{
			FloatTween floatTween = default(FloatTween);
			floatTween.duration = duration;
			floatTween.startFloat = m_CanvasGroup.alpha;
			floatTween.targetFloat = targetAlpha;
			FloatTween floatTween2 = floatTween;
			floatTween2.AddOnChangedCallback(SetCanvasAlpha);
			floatTween2.AddOnFinishCallback(callback);
			floatTween2.ignoreTimeScale = ignoreTimeScale;
			floatTween2.easing = easing;
			m_FloatTweenRunner.StartTween(floatTween2);
		}
	}

	public void SetCanvasAlpha(float alpha)
	{
		if (!(m_CanvasGroup == null))
		{
			m_CanvasGroup.alpha = alpha;
		}
	}

	public void SetPositionX(float x)
	{
		rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
	}

	public void SetPositionY(float y)
	{
		rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
	}

	private void TriggerAnimation(string triggername)
	{
		Animator animator = base.gameObject.GetComponent<Animator>();
		if (!(animator == null) && animator.enabled && animator.isActiveAndEnabled && !(animator.runtimeAnimatorController == null) && animator.hasBoundPlayables && !string.IsNullOrEmpty(triggername))
		{
			animator.ResetTrigger(m_AnimateInTrigger);
			animator.ResetTrigger(m_AnimateOutTrigger);
			animator.SetTrigger(triggername);
		}
	}

	protected virtual void OnTransitionIn()
	{
		_ = m_CanvasGroup != null;
	}

	protected virtual void OnTransitionOut()
	{
		Deactivate();
		_ = m_CanvasGroup != null;
		SetCanvasAlpha(1f);
		SetPositionX(0f);
	}
}
