using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI;

[DisallowMultipleComponent]
[AddComponentMenu("UI/Tooltip", 58)]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(VerticalLayoutGroup))]
[RequireComponent(typeof(ContentSizeFitter))]
public class UITooltip : MonoBehaviour
{
	public enum Transition
	{
		None,
		Fade
	}

	public enum VisualState
	{
		Shown,
		Hidden
	}

	public enum Corner
	{
		BottomLeft,
		TopLeft,
		TopRight,
		BottomRight
	}

	public enum Anchoring
	{
		Corners,
		LeftOrRight,
		TopOrBottom
	}

	public enum Anchor
	{
		BottomLeft,
		BottomRight,
		TopLeft,
		TopRight,
		Left,
		Right,
		Top,
		Bottom
	}

	[Serializable]
	public class AnchorEvent : UnityEvent<Anchor>
	{
	}

	private static UITooltip mInstance;

	public const ContentSizeFitter.FitMode DefaultHorizontalFitMode = ContentSizeFitter.FitMode.Unconstrained;

	[SerializeField]
	[Tooltip("Used when no width is specified for the current tooltip display.")]
	private float m_DefaultWidth = 257f;

	[SerializeField]
	[Tooltip("Should the tooltip follow the mouse movement or anchor to the position where it was called.")]
	private bool m_followMouse;

	[SerializeField]
	[Tooltip("Tooltip offset from the pointer when not anchored to a rect.")]
	private Vector2 m_Offset = Vector2.zero;

	[SerializeField]
	private Anchoring m_Anchoring;

	[SerializeField]
	[Tooltip("Tooltip offset when anchored to a rect.")]
	private Vector2 m_AnchoredOffset = Vector2.zero;

	[SerializeField]
	private Graphic m_AnchorGraphic;

	[SerializeField]
	private Vector2 m_AnchorGraphicOffset = Vector2.zero;

	[SerializeField]
	private Transition m_Transition;

	[SerializeField]
	private TweenEasing m_TransitionEasing;

	[SerializeField]
	private float m_TransitionDuration = 0.1f;

	public AnchorEvent onAnchorEvent = new AnchorEvent();

	private RectTransform m_Rect;

	private CanvasGroup m_CanvasGroup;

	private ContentSizeFitter m_SizeFitter;

	private Canvas m_Canvas;

	private VisualState m_VisualState;

	private RectTransform m_AnchorToTarget;

	private Anchor m_CurrentAnchor;

	private UITooltipLines m_LinesTemplate;

	private Vector2 m_OriginalOffset = Vector2.zero;

	private Vector2 m_OriginalAnchoredOffset = Vector2.zero;

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

	public static UITooltip Instance => mInstance;

	public float defaultWidth
	{
		get
		{
			return m_DefaultWidth;
		}
		set
		{
			m_DefaultWidth = value;
		}
	}

	public bool followMouse
	{
		get
		{
			return m_followMouse;
		}
		set
		{
			m_followMouse = value;
		}
	}

	public Vector2 offset
	{
		get
		{
			return m_Offset;
		}
		set
		{
			m_Offset = value;
			m_OriginalOffset = value;
		}
	}

	public Anchoring anchoring
	{
		get
		{
			return m_Anchoring;
		}
		set
		{
			m_Anchoring = value;
		}
	}

	public Vector2 anchoredOffset
	{
		get
		{
			return m_AnchoredOffset;
		}
		set
		{
			m_AnchoredOffset = value;
			m_OriginalAnchoredOffset = value;
		}
	}

	public float alpha
	{
		get
		{
			if (!(m_CanvasGroup != null))
			{
				return 1f;
			}
			return m_CanvasGroup.alpha;
		}
	}

	public VisualState visualState => m_VisualState;

	public Camera uiCamera
	{
		get
		{
			if (m_Canvas == null)
			{
				return null;
			}
			if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay || (m_Canvas.renderMode == RenderMode.ScreenSpaceCamera && m_Canvas.worldCamera == null))
			{
				return null;
			}
			if (m_Canvas.worldCamera != null)
			{
				return m_Canvas.worldCamera;
			}
			return Camera.main;
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

	protected UITooltip()
	{
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_FloatTweenRunner.Init(this);
	}

	protected virtual void Awake()
	{
		mInstance = this;
		m_Rect = base.gameObject.GetComponent<RectTransform>();
		m_CanvasGroup = base.gameObject.GetComponent<CanvasGroup>();
		m_CanvasGroup.blocksRaycasts = false;
		m_CanvasGroup.interactable = false;
		m_SizeFitter = base.gameObject.GetComponent<ContentSizeFitter>();
		m_SizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		VerticalLayoutGroup component = base.gameObject.GetComponent<VerticalLayoutGroup>();
		component.childControlHeight = true;
		component.childControlWidth = true;
		m_OriginalOffset = m_Offset;
		m_OriginalAnchoredOffset = m_AnchoredOffset;
		if (base.gameObject.GetComponent<UIAlwaysOnTop>() == null)
		{
			base.gameObject.AddComponent<UIAlwaysOnTop>().order = 99999;
		}
		SetAlpha(0f);
		m_VisualState = VisualState.Hidden;
		InternalOnHide();
	}

	protected virtual void Start()
	{
		m_Rect.anchorMin = new Vector2(0.5f, 0.5f);
		m_Rect.anchorMax = new Vector2(0.5f, 0.5f);
	}

	protected virtual void OnDestroy()
	{
		mInstance = null;
	}

	protected virtual void OnCanvasGroupChanged()
	{
		m_Canvas = UIUtility.FindInParents<Canvas>(base.gameObject);
	}

	public virtual bool IsActive()
	{
		if (base.enabled)
		{
			return base.gameObject.activeInHierarchy;
		}
		return false;
	}

	protected virtual void Update()
	{
		if (m_followMouse && base.enabled && IsActive() && alpha > 0f)
		{
			UpdatePositionAndPivot();
		}
	}

	public virtual void UpdatePositionAndPivot()
	{
		if (m_Canvas == null)
		{
			return;
		}
		UpdatePivot();
		if (m_AnchorToTarget == null)
		{
			Vector2 pivotBasedOffset = new Vector2((m_Rect.pivot.x == 1f) ? (m_Offset.x * -1f) : m_Offset.x, (m_Rect.pivot.y == 1f) ? (m_Offset.y * -1f) : m_Offset.y);
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.transform as RectTransform, Input.mousePosition, uiCamera, out var localPoint))
			{
				m_Rect.anchoredPosition = pivotBasedOffset + localPoint;
			}
		}
		if (m_AnchorToTarget != null)
		{
			if (m_Anchoring == Anchoring.Corners)
			{
				Vector3[] targetWorldCorners = new Vector3[4];
				m_AnchorToTarget.GetWorldCorners(targetWorldCorners);
				Corner oppositeCorner = GetOppositeCorner(VectorPivotToCorner(m_Rect.pivot));
				Vector2 pivotBasedOffset2 = new Vector2((m_Rect.pivot.x == 1f) ? (m_AnchoredOffset.x * -1f) : m_AnchoredOffset.x, (m_Rect.pivot.y == 1f) ? (m_AnchoredOffset.y * -1f) : m_AnchoredOffset.y);
				Vector2 anchorPoint = m_Canvas.transform.InverseTransformPoint(targetWorldCorners[(int)oppositeCorner]);
				m_Rect.anchoredPosition = pivotBasedOffset2 + anchorPoint;
			}
			else if (m_Anchoring == Anchoring.LeftOrRight || m_Anchoring == Anchoring.TopOrBottom)
			{
				Vector3[] targetWorldCorners2 = new Vector3[4];
				m_AnchorToTarget.GetWorldCorners(targetWorldCorners2);
				Vector2 topleft = m_Canvas.transform.InverseTransformPoint(targetWorldCorners2[1]);
				if (m_Anchoring == Anchoring.LeftOrRight)
				{
					Vector2 pivotBasedOffset3 = new Vector2((m_Rect.pivot.x == 1f) ? (m_AnchoredOffset.x * -1f) : m_AnchoredOffset.x, m_AnchoredOffset.y);
					if (m_Rect.pivot.x == 0f)
					{
						m_Rect.anchoredPosition = topleft + pivotBasedOffset3 + new Vector2(m_AnchorToTarget.rect.width, m_AnchorToTarget.rect.height / 2f * -1f);
					}
					else
					{
						m_Rect.anchoredPosition = topleft + pivotBasedOffset3 + new Vector2(0f, m_AnchorToTarget.rect.height / 2f * -1f);
					}
				}
				else if (m_Anchoring == Anchoring.TopOrBottom)
				{
					Vector2 pivotBasedOffset4 = new Vector2(m_AnchoredOffset.x, (m_Rect.pivot.y == 1f) ? (m_AnchoredOffset.y * -1f) : m_AnchoredOffset.y);
					if (m_Rect.pivot.y == 0f)
					{
						m_Rect.anchoredPosition = topleft + pivotBasedOffset4 + new Vector2(m_AnchorToTarget.rect.width / 2f, 0f);
					}
					else
					{
						m_Rect.anchoredPosition = topleft + pivotBasedOffset4 + new Vector2(m_AnchorToTarget.rect.width / 2f, m_AnchorToTarget.rect.height * -1f);
					}
				}
			}
		}
		m_Rect.anchoredPosition = new Vector2(Mathf.Round(m_Rect.anchoredPosition.x), Mathf.Round(m_Rect.anchoredPosition.y));
		m_Rect.anchoredPosition = new Vector2(m_Rect.anchoredPosition.x + m_Rect.anchoredPosition.x % 2f, m_Rect.anchoredPosition.y + m_Rect.anchoredPosition.y % 2f);
	}

	public void UpdatePivot()
	{
		Vector3 targetPosition = Input.mousePosition;
		if (m_Anchoring == Anchoring.Corners)
		{
			Vector2 corner = new Vector2((targetPosition.x > (float)Screen.width / 2f) ? 1f : 0f, (targetPosition.y > (float)Screen.height / 2f) ? 1f : 0f);
			SetPivot(VectorPivotToCorner(corner));
		}
		else if (m_Anchoring == Anchoring.LeftOrRight)
		{
			Vector2 pivot = new Vector2((targetPosition.x > (float)Screen.width / 2f) ? 1f : 0f, 0.5f);
			SetPivot(pivot);
		}
		else if (m_Anchoring == Anchoring.TopOrBottom)
		{
			Vector2 pivot2 = new Vector2(0.5f, (targetPosition.y > (float)Screen.height / 2f) ? 1f : 0f);
			SetPivot(pivot2);
		}
	}

	protected void SetPivot(Vector2 pivot)
	{
		m_Rect.pivot = pivot;
		m_CurrentAnchor = VectorPivotToAnchor(pivot);
		UpdateAnchorGraphicPosition();
	}

	protected void SetPivot(Corner point)
	{
		switch (point)
		{
		case Corner.BottomLeft:
			m_Rect.pivot = new Vector2(0f, 0f);
			break;
		case Corner.BottomRight:
			m_Rect.pivot = new Vector2(1f, 0f);
			break;
		case Corner.TopLeft:
			m_Rect.pivot = new Vector2(0f, 1f);
			break;
		case Corner.TopRight:
			m_Rect.pivot = new Vector2(1f, 1f);
			break;
		}
		m_CurrentAnchor = VectorPivotToAnchor(m_Rect.pivot);
		UpdateAnchorGraphicPosition();
	}

	protected void UpdateAnchorGraphicPosition()
	{
		if (m_AnchorGraphic == null)
		{
			return;
		}
		RectTransform rt = m_AnchorGraphic.transform as RectTransform;
		if (m_Anchoring == Anchoring.Corners)
		{
			rt.pivot = Vector2.zero;
			rt.anchorMax = m_Rect.pivot;
			rt.anchorMin = m_Rect.pivot;
			rt.anchoredPosition = new Vector2((m_Rect.pivot.x == 1f) ? (m_AnchorGraphicOffset.x * -1f) : m_AnchorGraphicOffset.x, (m_Rect.pivot.y == 1f) ? (m_AnchorGraphicOffset.y * -1f) : m_AnchorGraphicOffset.y);
			rt.localScale = new Vector3((m_Rect.pivot.x == 0f) ? 1f : (-1f), (m_Rect.pivot.y == 0f) ? 1f : (-1f), rt.localScale.z);
		}
		else if (m_Anchoring == Anchoring.LeftOrRight || m_Anchoring == Anchoring.TopOrBottom)
		{
			switch (m_CurrentAnchor)
			{
			case Anchor.Left:
				rt.pivot = new Vector2(0f, 0.5f);
				rt.anchorMax = new Vector2(0f, 0.5f);
				rt.anchorMin = new Vector2(0f, 0.5f);
				rt.anchoredPosition3D = new Vector3(m_AnchorGraphicOffset.x, m_AnchorGraphicOffset.y, rt.localPosition.z);
				rt.localScale = new Vector3(1f, 1f, rt.localScale.z);
				break;
			case Anchor.Right:
				rt.pivot = new Vector2(1f, 0.5f);
				rt.anchorMax = new Vector2(1f, 0.5f);
				rt.anchorMin = new Vector2(1f, 0.5f);
				rt.anchoredPosition3D = new Vector3(m_AnchorGraphicOffset.x * -1f - rt.rect.width, m_AnchorGraphicOffset.y, rt.localPosition.z);
				rt.localScale = new Vector3(-1f, 1f, rt.localScale.z);
				break;
			case Anchor.Bottom:
				rt.pivot = new Vector2(0.5f, 0f);
				rt.anchorMax = new Vector2(0.5f, 0f);
				rt.anchorMin = new Vector2(0.5f, 0f);
				rt.anchoredPosition3D = new Vector3(m_AnchorGraphicOffset.x, m_AnchorGraphicOffset.y, rt.localPosition.z);
				rt.localScale = new Vector3(1f, 1f, rt.localScale.z);
				break;
			case Anchor.Top:
				rt.pivot = new Vector2(0.5f, 1f);
				rt.anchorMax = new Vector2(0.5f, 1f);
				rt.anchorMin = new Vector2(0.5f, 1f);
				rt.anchoredPosition3D = new Vector3(m_AnchorGraphicOffset.x, m_AnchorGraphicOffset.y * -1f - rt.rect.height, rt.localPosition.z);
				rt.localScale = new Vector3(1f, -1f, rt.localScale.z);
				break;
			}
		}
		if (onAnchorEvent != null)
		{
			onAnchorEvent.Invoke(m_CurrentAnchor);
		}
	}

	protected virtual void Internal_Show()
	{
		EvaluateAndCreateTooltipLines();
		UpdatePositionAndPivot();
		UIUtility.BringToFront(base.gameObject);
		EvaluateAndTransitionToState(state: true, instant: false);
	}

	protected virtual void Internal_Hide()
	{
		EvaluateAndTransitionToState(state: false, instant: false);
	}

	protected virtual void Internal_AnchorToRect(RectTransform targetRect)
	{
		m_AnchorToTarget = targetRect;
	}

	protected void Internal_SetWidth(float width)
	{
		m_Rect.sizeDelta = new Vector2(width, m_Rect.sizeDelta.y);
	}

	protected void Internal_SetHorizontalFitMode(ContentSizeFitter.FitMode mode)
	{
		m_SizeFitter.horizontalFit = mode;
	}

	protected void Internal_OverrideOffset(Vector2 offset)
	{
		m_Offset = offset;
	}

	protected void Internal_OverrideAnchoredOffset(Vector2 offset)
	{
		m_AnchoredOffset = offset;
	}

	private void EvaluateAndTransitionToState(bool state, bool instant)
	{
		Transition transition = m_Transition;
		if (transition != 0 && transition == Transition.Fade)
		{
			StartAlphaTween(state ? 1f : 0f, instant ? 0f : m_TransitionDuration);
		}
		else
		{
			SetAlpha(state ? 1f : 0f);
			m_VisualState = ((!state) ? VisualState.Hidden : VisualState.Shown);
		}
		if (m_Transition == Transition.None && !state)
		{
			InternalOnHide();
		}
	}

	public void SetAlpha(float alpha)
	{
		m_CanvasGroup.alpha = alpha;
	}

	public void StartAlphaTween(float targetAlpha, float duration)
	{
		FloatTween floatTween = default(FloatTween);
		floatTween.duration = duration;
		floatTween.startFloat = m_CanvasGroup.alpha;
		floatTween.targetFloat = targetAlpha;
		FloatTween floatTween2 = floatTween;
		floatTween2.AddOnChangedCallback(SetAlpha);
		floatTween2.AddOnFinishCallback(OnTweenFinished);
		floatTween2.ignoreTimeScale = true;
		floatTween2.easing = m_TransitionEasing;
		m_FloatTweenRunner.StartTween(floatTween2);
	}

	protected virtual void OnTweenFinished()
	{
		if (alpha == 0f)
		{
			m_VisualState = VisualState.Hidden;
			InternalOnHide();
		}
		else
		{
			m_VisualState = VisualState.Shown;
		}
	}

	private void InternalOnHide()
	{
		CleanupLines();
		m_AnchorToTarget = null;
		m_SizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
		m_Rect.sizeDelta = new Vector2(m_DefaultWidth, m_Rect.sizeDelta.y);
		m_Offset = m_OriginalOffset;
		m_AnchoredOffset = m_OriginalAnchoredOffset;
	}

	private void EvaluateAndCreateTooltipLines()
	{
		if (m_LinesTemplate == null || m_LinesTemplate.lineList.Count == 0)
		{
			return;
		}
		foreach (UITooltipLines.Line line in m_LinesTemplate.lineList)
		{
			GameObject lineObject = CreateLine(line.padding);
			for (int i = 0; i < 2; i++)
			{
				string column = ((i == 0) ? line.left : line.right);
				if (!string.IsNullOrEmpty(column))
				{
					CreateLineColumn(lineObject.transform, column, i == 0, line.style, line.customStyle);
				}
			}
		}
	}

	private GameObject CreateLine(RectOffset padding)
	{
		GameObject obj = new GameObject("Line", typeof(RectTransform));
		(obj.transform as RectTransform).pivot = new Vector2(0f, 1f);
		obj.transform.SetParent(base.transform);
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		obj.transform.localPosition = Vector3.zero;
		obj.layer = base.gameObject.layer;
		HorizontalLayoutGroup horizontalLayoutGroup = obj.AddComponent<HorizontalLayoutGroup>();
		horizontalLayoutGroup.padding = padding;
		horizontalLayoutGroup.childControlHeight = true;
		horizontalLayoutGroup.childControlWidth = true;
		return obj;
	}

	private void CreateLineColumn(Transform parent, string content, bool isLeft, UITooltipLines.LineStyle style, string customStyle)
	{
		GameObject obj = new GameObject("Column", typeof(RectTransform), typeof(CanvasRenderer));
		obj.layer = base.gameObject.layer;
		obj.transform.SetParent(parent);
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		obj.transform.localPosition = Vector3.zero;
		(obj.transform as RectTransform).pivot = new Vector2(0f, 1f);
		Text text = obj.AddComponent<Text>();
		text.text = content;
		text.supportRichText = true;
		text.raycastTarget = false;
		UITooltipLineStyle lineStyle = UITooltipManager.Instance.defaultLineStyle;
		switch (style)
		{
		case UITooltipLines.LineStyle.Title:
			lineStyle = UITooltipManager.Instance.titleLineStyle;
			break;
		case UITooltipLines.LineStyle.Description:
			lineStyle = UITooltipManager.Instance.descriptionLineStyle;
			break;
		case UITooltipLines.LineStyle.Custom:
			lineStyle = UITooltipManager.Instance.GetCustomStyle(customStyle);
			break;
		}
		text.font = lineStyle.TextFont;
		text.fontStyle = lineStyle.TextFontStyle;
		text.fontSize = lineStyle.TextFontSize;
		text.lineSpacing = lineStyle.TextFontLineSpacing;
		text.color = lineStyle.TextFontColor;
		if (lineStyle.OverrideTextAlignment == OverrideTextAlignment.No)
		{
			text.alignment = (isLeft ? TextAnchor.LowerLeft : TextAnchor.LowerRight);
		}
		else
		{
			switch (lineStyle.OverrideTextAlignment)
			{
			case OverrideTextAlignment.Left:
				text.alignment = TextAnchor.LowerLeft;
				break;
			case OverrideTextAlignment.Center:
				text.alignment = TextAnchor.LowerCenter;
				break;
			case OverrideTextAlignment.Right:
				text.alignment = TextAnchor.LowerRight;
				break;
			}
		}
		if (lineStyle.TextEffects.Length == 0)
		{
			return;
		}
		UITooltipTextEffect[] textEffects = lineStyle.TextEffects;
		foreach (UITooltipTextEffect tte in textEffects)
		{
			if (tte.Effect == UITooltipTextEffectType.Shadow)
			{
				Shadow shadow = obj.AddComponent<Shadow>();
				shadow.effectColor = tte.EffectColor;
				shadow.effectDistance = tte.EffectDistance;
				shadow.useGraphicAlpha = tte.UseGraphicAlpha;
			}
			else if (tte.Effect == UITooltipTextEffectType.Outline)
			{
				Outline outline = obj.AddComponent<Outline>();
				outline.effectColor = tte.EffectColor;
				outline.effectDistance = tte.EffectDistance;
				outline.useGraphicAlpha = tte.UseGraphicAlpha;
			}
		}
	}

	protected virtual void CleanupLines()
	{
		foreach (Transform t in base.transform)
		{
			if (!(t.gameObject.GetComponent<LayoutElement>() != null) || !t.gameObject.GetComponent<LayoutElement>().ignoreLayout)
			{
				global::UnityEngine.Object.Destroy(t.gameObject);
			}
		}
		m_LinesTemplate = null;
	}

	protected void Internal_SetLines(UITooltipLines lines)
	{
		m_LinesTemplate = lines;
	}

	protected void Internal_AddLine(string a, RectOffset padding)
	{
		if (m_LinesTemplate == null)
		{
			m_LinesTemplate = new UITooltipLines();
		}
		m_LinesTemplate.AddLine(a, padding);
	}

	protected void Internal_AddLine(string a, UITooltipLines.LineStyle style)
	{
		if (m_LinesTemplate == null)
		{
			m_LinesTemplate = new UITooltipLines();
		}
		m_LinesTemplate.AddLine(a, new RectOffset(), style);
	}

	protected void Internal_AddLine(string a, string customStyle)
	{
		if (m_LinesTemplate == null)
		{
			m_LinesTemplate = new UITooltipLines();
		}
		m_LinesTemplate.AddLine(a, new RectOffset(), customStyle);
	}

	protected void Internal_AddLine(string a, RectOffset padding, UITooltipLines.LineStyle style)
	{
		if (m_LinesTemplate == null)
		{
			m_LinesTemplate = new UITooltipLines();
		}
		m_LinesTemplate.AddLine(a, padding, style);
	}

	protected void Internal_AddLine(string a, RectOffset padding, string customStyle)
	{
		if (m_LinesTemplate == null)
		{
			m_LinesTemplate = new UITooltipLines();
		}
		m_LinesTemplate.AddLine(a, padding, customStyle);
	}

	protected void Internal_AddLineColumn(string a)
	{
		if (m_LinesTemplate == null)
		{
			m_LinesTemplate = new UITooltipLines();
		}
		m_LinesTemplate.AddColumn(a);
	}

	protected void Internal_AddLineColumn(string a, UITooltipLines.LineStyle style)
	{
		if (m_LinesTemplate == null)
		{
			m_LinesTemplate = new UITooltipLines();
		}
		m_LinesTemplate.AddColumn(a, style);
	}

	protected void Internal_AddLineColumn(string a, string customStyle)
	{
		if (m_LinesTemplate == null)
		{
			m_LinesTemplate = new UITooltipLines();
		}
		m_LinesTemplate.AddColumn(a, customStyle);
	}

	protected virtual void Internal_AddTitle(string title)
	{
		if (m_LinesTemplate == null)
		{
			m_LinesTemplate = new UITooltipLines();
		}
		m_LinesTemplate.AddLine(title, new RectOffset(0, 0, 0, 0), UITooltipLines.LineStyle.Title);
	}

	protected virtual void Internal_AddDescription(string description)
	{
		if (m_LinesTemplate == null)
		{
			m_LinesTemplate = new UITooltipLines();
		}
		m_LinesTemplate.AddLine(description, new RectOffset(0, 0, 0, 0), UITooltipLines.LineStyle.Description);
	}

	protected virtual void Internal_AddSpacer()
	{
		if (m_LinesTemplate == null)
		{
			m_LinesTemplate = new UITooltipLines();
		}
		m_LinesTemplate.AddLine("", new RectOffset(0, 0, UITooltipManager.Instance.spacerHeight, 0));
	}

	public static void SetLines(UITooltipLines lines)
	{
		if (mInstance != null)
		{
			mInstance.Internal_SetLines(lines);
		}
	}

	public static void AddLine(string content)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddLine(content, new RectOffset());
		}
	}

	public static void AddLine(string content, UITooltipLines.LineStyle style)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddLine(content, new RectOffset(), style);
		}
	}

	public static void AddLine(string content, string customStyle)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddLine(content, new RectOffset(), customStyle);
		}
	}

	public static void AddLine(string content, RectOffset padding)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddLine(content, padding);
		}
	}

	public static void AddLine(string content, RectOffset padding, UITooltipLines.LineStyle style)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddLine(content, padding, style);
		}
	}

	public static void AddLine(string content, RectOffset padding, string customStyle)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddLine(content, padding, customStyle);
		}
	}

	public static void AddLineColumn(string content)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddLineColumn(content);
		}
	}

	public static void AddLineColumn(string content, UITooltipLines.LineStyle style)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddLineColumn(content, style);
		}
	}

	public static void AddLineColumn(string content, string customStyle)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddLineColumn(content, customStyle);
		}
	}

	public static void AddSpacer()
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddSpacer();
		}
	}

	public static void InstantiateIfNecessary(GameObject rel)
	{
		if (UITooltipManager.Instance == null || UITooltipManager.Instance.prefab == null)
		{
			return;
		}
		Canvas canvas = UIUtility.FindInParents<Canvas>(rel);
		if (canvas == null)
		{
			return;
		}
		if (mInstance != null)
		{
			Canvas prevTooltipCanvas = UIUtility.FindInParents<Canvas>(mInstance.gameObject);
			if (prevTooltipCanvas != null && prevTooltipCanvas.Equals(canvas))
			{
				return;
			}
			global::UnityEngine.Object.Destroy(mInstance.gameObject);
		}
		global::UnityEngine.Object.Instantiate(UITooltipManager.Instance.prefab, canvas.transform, worldPositionStays: false);
	}

	public static void AddTitle(string title)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddTitle(title);
		}
	}

	public static void AddDescription(string description)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AddDescription(description);
		}
	}

	public static void Show()
	{
		if (mInstance != null && mInstance.IsActive())
		{
			mInstance.Internal_Show();
		}
	}

	public static void Hide()
	{
		if (mInstance != null)
		{
			mInstance.Internal_Hide();
		}
	}

	public static void AnchorToRect(RectTransform targetRect)
	{
		if (mInstance != null)
		{
			mInstance.Internal_AnchorToRect(targetRect);
		}
	}

	public static void SetHorizontalFitMode(ContentSizeFitter.FitMode mode)
	{
		if (mInstance != null)
		{
			mInstance.Internal_SetHorizontalFitMode(mode);
		}
	}

	public static void SetWidth(float width)
	{
		if (mInstance != null)
		{
			mInstance.Internal_SetWidth(width);
		}
	}

	public static void OverrideOffset(Vector2 offset)
	{
		if (mInstance != null)
		{
			mInstance.Internal_OverrideOffset(offset);
		}
	}

	public static void OverrideAnchoredOffset(Vector2 offset)
	{
		if (mInstance != null)
		{
			mInstance.Internal_OverrideAnchoredOffset(offset);
		}
	}

	public static Corner VectorPivotToCorner(Vector2 pivot)
	{
		if (pivot.x == 0f && pivot.y == 0f)
		{
			return Corner.BottomLeft;
		}
		if (pivot.x == 0f && pivot.y == 1f)
		{
			return Corner.TopLeft;
		}
		if (pivot.x == 1f && pivot.y == 0f)
		{
			return Corner.BottomRight;
		}
		return Corner.TopRight;
	}

	public static Anchor VectorPivotToAnchor(Vector2 pivot)
	{
		if (pivot.x == 0f && pivot.y == 0f)
		{
			return Anchor.BottomLeft;
		}
		if (pivot.x == 0f && pivot.y == 1f)
		{
			return Anchor.TopLeft;
		}
		if (pivot.x == 1f && pivot.y == 0f)
		{
			return Anchor.BottomRight;
		}
		if (pivot.x == 0.5f && pivot.y == 0f)
		{
			return Anchor.Bottom;
		}
		if (pivot.x == 0.5f && pivot.y == 1f)
		{
			return Anchor.Top;
		}
		if (pivot.x == 0f && pivot.y == 0.5f)
		{
			return Anchor.Left;
		}
		if (pivot.x == 1f && pivot.y == 0.5f)
		{
			return Anchor.Right;
		}
		return Anchor.TopRight;
	}

	public static Corner GetOppositeCorner(Corner corner)
	{
		return corner switch
		{
			Corner.BottomLeft => Corner.TopRight, 
			Corner.BottomRight => Corner.TopLeft, 
			Corner.TopLeft => Corner.BottomRight, 
			Corner.TopRight => Corner.BottomLeft, 
			_ => Corner.BottomLeft, 
		};
	}

	public static Anchor GetOppositeAnchor(Anchor anchor)
	{
		return anchor switch
		{
			Anchor.BottomLeft => Anchor.TopRight, 
			Anchor.BottomRight => Anchor.TopLeft, 
			Anchor.TopLeft => Anchor.BottomRight, 
			Anchor.TopRight => Anchor.BottomLeft, 
			Anchor.Top => Anchor.Bottom, 
			Anchor.Bottom => Anchor.Top, 
			Anchor.Left => Anchor.Right, 
			Anchor.Right => Anchor.Left, 
			_ => Anchor.BottomLeft, 
		};
	}
}
