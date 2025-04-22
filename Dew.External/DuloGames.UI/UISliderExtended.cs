using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Slider Extended", 58)]
public class UISliderExtended : Slider
{
	public enum TextEffectType
	{
		None,
		Shadow,
		Outline
	}

	public enum OptionTransition
	{
		None,
		ColorTint
	}

	public enum TransitionTarget
	{
		Image,
		Text
	}

	[SerializeField]
	private List<string> m_Options = new List<string>();

	[SerializeField]
	private List<GameObject> m_OptionGameObjects = new List<GameObject>();

	[SerializeField]
	private GameObject m_OptionsContGameObject;

	[SerializeField]
	private RectTransform m_OptionsContRect;

	[SerializeField]
	private GridLayoutGroup m_OptionsContGrid;

	[SerializeField]
	private RectOffset m_OptionsPadding = new RectOffset();

	[SerializeField]
	private Sprite m_OptionSprite;

	[SerializeField]
	private Font m_OptionTextFont;

	[SerializeField]
	private FontStyle m_OptionTextStyle = FontData.defaultFontData.fontStyle;

	[SerializeField]
	private int m_OptionTextSize = FontData.defaultFontData.fontSize;

	[SerializeField]
	private Color m_OptionTextColor = Color.white;

	[SerializeField]
	private TextEffectType m_OptionTextEffect;

	[SerializeField]
	private Color m_OptionTextEffectColor = new Color(0f, 0f, 0f, 128f);

	[SerializeField]
	private Vector2 m_OptionTextEffectDistance = new Vector2(1f, -1f);

	[SerializeField]
	private bool m_OptionTextEffectUseGraphicAlpha = true;

	[SerializeField]
	private Vector2 m_OptionTextOffset = Vector2.zero;

	[SerializeField]
	private OptionTransition m_OptionTransition;

	[SerializeField]
	private TransitionTarget m_OptionTransitionTarget = TransitionTarget.Text;

	[SerializeField]
	private Color m_OptionTransitionColorNormal = ColorBlock.defaultColorBlock.normalColor;

	[SerializeField]
	private Color m_OptionTransitionColorActive = ColorBlock.defaultColorBlock.highlightedColor;

	[SerializeField]
	[Range(1f, 6f)]
	private float m_OptionTransitionMultiplier = 1f;

	[SerializeField]
	private float m_OptionTransitionDuration = 0.1f;

	private GameObject m_CurrentOptionGameObject;

	public List<string> options
	{
		get
		{
			return m_Options;
		}
		set
		{
			m_Options = value;
			RebuildOptions();
			ValidateOptions();
		}
	}

	public GameObject selectedOptionGameObject => m_CurrentOptionGameObject;

	public int selectedOptionIndex
	{
		get
		{
			int optionIndex = Mathf.RoundToInt(value);
			if (optionIndex < 0 || optionIndex >= m_Options.Count)
			{
				return 0;
			}
			return optionIndex;
		}
	}

	public RectOffset optionsPadding
	{
		get
		{
			return m_OptionsPadding;
		}
		set
		{
			m_OptionsPadding = value;
		}
	}

	public bool HasOptions()
	{
		if (m_Options != null)
		{
			return m_Options.Count > 0;
		}
		return false;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		ValidateOptions();
		base.onValueChanged.AddListener(OnValueChanged);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		base.onValueChanged.RemoveListener(OnValueChanged);
	}

	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		if (IsActive())
		{
			UpdateGridProperties();
		}
	}

	public void OnValueChanged(float value)
	{
		if (IsActive() && HasOptions() && m_OptionTransition == OptionTransition.ColorTint)
		{
			Graphic currentTarget = ((m_OptionTransitionTarget == TransitionTarget.Text) ? ((Graphic)m_CurrentOptionGameObject.GetComponentInChildren<Text>()) : ((Graphic)m_CurrentOptionGameObject.GetComponent<Image>()));
			StartColorTween(currentTarget, m_OptionTransitionColorNormal * m_OptionTransitionMultiplier, m_OptionTransitionDuration);
			int newOptionIndex = Mathf.RoundToInt(value);
			if (newOptionIndex < 0 || newOptionIndex >= m_Options.Count)
			{
				newOptionIndex = 0;
			}
			GameObject newOptionGameObject = m_OptionGameObjects[newOptionIndex];
			if (newOptionGameObject != null)
			{
				Graphic newTarget = ((m_OptionTransitionTarget == TransitionTarget.Text) ? ((Graphic)newOptionGameObject.GetComponentInChildren<Text>()) : ((Graphic)newOptionGameObject.GetComponent<Image>()));
				StartColorTween(newTarget, m_OptionTransitionColorActive * m_OptionTransitionMultiplier, m_OptionTransitionDuration);
			}
			m_CurrentOptionGameObject = newOptionGameObject;
		}
	}

	private void StartColorTween(Graphic target, Color targetColor, float duration)
	{
		if (!(target == null))
		{
			if (!Application.isPlaying || duration == 0f)
			{
				target.canvasRenderer.SetColor(targetColor);
			}
			else
			{
				target.CrossFadeColor(targetColor, duration, ignoreTimeScale: true, useAlpha: true);
			}
		}
	}

	protected void ValidateOptions()
	{
		if (!IsActive())
		{
			return;
		}
		if (!HasOptions())
		{
			if (m_OptionsContGameObject != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(m_OptionsContGameObject);
				}
				else
				{
					Object.DestroyImmediate(m_OptionsContGameObject);
				}
			}
			return;
		}
		if (m_OptionsContGameObject == null)
		{
			CreateOptionsContainer();
		}
		if (!base.wholeNumbers)
		{
			base.wholeNumbers = true;
		}
		base.minValue = 0f;
		base.maxValue = (float)m_Options.Count - 1f;
		UpdateGridProperties();
		UpdateOptionsProperties();
	}

	public void UpdateGridProperties()
	{
		if (!(m_OptionsContGrid == null))
		{
			if (!m_OptionsContGrid.padding.Equals(m_OptionsPadding))
			{
				m_OptionsContGrid.padding = m_OptionsPadding;
			}
			Vector2 cellSize = ((m_OptionSprite != null) ? new Vector2(m_OptionSprite.rect.width, m_OptionSprite.rect.height) : Vector2.zero);
			if (!m_OptionsContGrid.cellSize.Equals(cellSize))
			{
				m_OptionsContGrid.cellSize = cellSize;
			}
			float spacingX = (m_OptionsContRect.rect.width - ((float)m_OptionsPadding.left + (float)m_OptionsPadding.right) - (float)m_Options.Count * cellSize.x) / ((float)m_Options.Count - 1f);
			if (m_OptionsContGrid.spacing.x != spacingX)
			{
				m_OptionsContGrid.spacing = new Vector2(spacingX, 0f);
			}
		}
	}

	public void UpdateOptionsProperties()
	{
		if (!HasOptions())
		{
			return;
		}
		int i = 0;
		foreach (GameObject optionObject in m_OptionGameObjects)
		{
			bool selected = Mathf.RoundToInt(value) == i;
			if (selected)
			{
				m_CurrentOptionGameObject = optionObject;
			}
			Image image = optionObject.GetComponent<Image>();
			if (image != null)
			{
				image.sprite = m_OptionSprite;
				image.rectTransform.pivot = new Vector2(0f, 1f);
				if (m_OptionTransition == OptionTransition.ColorTint && m_OptionTransitionTarget == TransitionTarget.Image)
				{
					image.canvasRenderer.SetColor(selected ? m_OptionTransitionColorActive : m_OptionTransitionColorNormal);
				}
				else
				{
					image.canvasRenderer.SetColor(Color.white);
				}
			}
			Text text = optionObject.GetComponentInChildren<Text>();
			if (text != null)
			{
				text.font = m_OptionTextFont;
				text.fontStyle = m_OptionTextStyle;
				text.fontSize = m_OptionTextSize;
				text.color = m_OptionTextColor;
				if (m_OptionTransition == OptionTransition.ColorTint && m_OptionTransitionTarget == TransitionTarget.Text)
				{
					text.canvasRenderer.SetColor(selected ? m_OptionTransitionColorActive : m_OptionTransitionColorNormal);
				}
				else
				{
					text.canvasRenderer.SetColor(Color.white);
				}
				(text.transform as RectTransform).anchoredPosition = m_OptionTextOffset;
				UpdateTextEffect(text.gameObject);
			}
			i++;
		}
	}

	protected void RebuildOptions()
	{
		if (!HasOptions())
		{
			return;
		}
		if (m_OptionsContGameObject == null)
		{
			CreateOptionsContainer();
		}
		DestroyOptions();
		int i = 0;
		foreach (string option in m_Options)
		{
			GameObject optionObject = new GameObject("Option " + i, typeof(RectTransform), typeof(Image));
			optionObject.layer = base.gameObject.layer;
			optionObject.transform.SetParent(m_OptionsContGameObject.transform, worldPositionStays: false);
			optionObject.transform.localScale = Vector3.one;
			optionObject.transform.localPosition = Vector3.zero;
			GameObject obj = new GameObject("Text", typeof(RectTransform));
			obj.layer = base.gameObject.layer;
			obj.transform.SetParent(optionObject.transform, worldPositionStays: false);
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			obj.AddComponent<Text>().text = option;
			ContentSizeFitter contentSizeFitter = obj.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			m_OptionGameObjects.Add(optionObject);
			i++;
		}
		UpdateOptionsProperties();
	}

	private void AddTextEffect(GameObject gObject)
	{
		if (m_OptionTextEffect == TextEffectType.Shadow)
		{
			Shadow shadow = gObject.AddComponent<Shadow>();
			shadow.effectColor = m_OptionTextEffectColor;
			shadow.effectDistance = m_OptionTextEffectDistance;
			shadow.useGraphicAlpha = m_OptionTextEffectUseGraphicAlpha;
		}
		else if (m_OptionTextEffect == TextEffectType.Outline)
		{
			Outline outline = gObject.AddComponent<Outline>();
			outline.effectColor = m_OptionTextEffectColor;
			outline.effectDistance = m_OptionTextEffectDistance;
			outline.useGraphicAlpha = m_OptionTextEffectUseGraphicAlpha;
		}
	}

	private void UpdateTextEffect(GameObject gObject)
	{
		if (m_OptionTextEffect == TextEffectType.Shadow)
		{
			Shadow s = gObject.GetComponent<Shadow>();
			if (s == null)
			{
				s = gObject.AddComponent<Shadow>();
			}
			s.effectColor = m_OptionTextEffectColor;
			s.effectDistance = m_OptionTextEffectDistance;
			s.useGraphicAlpha = m_OptionTextEffectUseGraphicAlpha;
		}
		else if (m_OptionTextEffect == TextEffectType.Outline)
		{
			Outline o = gObject.GetComponent<Outline>();
			if (o == null)
			{
				o = gObject.AddComponent<Outline>();
			}
			o.effectColor = m_OptionTextEffectColor;
			o.effectDistance = m_OptionTextEffectDistance;
			o.useGraphicAlpha = m_OptionTextEffectUseGraphicAlpha;
		}
	}

	public void RebuildTextEffects()
	{
		foreach (GameObject optionGameObject in m_OptionGameObjects)
		{
			Text text = optionGameObject.GetComponentInChildren<Text>();
			if (!(text != null))
			{
				continue;
			}
			Shadow s = text.gameObject.GetComponent<Shadow>();
			Outline o = text.gameObject.GetComponent<Outline>();
			if (Application.isPlaying)
			{
				if (s != null)
				{
					Object.Destroy(s);
				}
				if (o != null)
				{
					Object.Destroy(o);
				}
			}
			else
			{
				if (s != null)
				{
					Object.DestroyImmediate(s);
				}
				if (o != null)
				{
					Object.DestroyImmediate(o);
				}
			}
			AddTextEffect(text.gameObject);
		}
	}

	protected void DestroyOptions()
	{
		foreach (GameObject g in m_OptionGameObjects)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(g);
			}
			else
			{
				Object.DestroyImmediate(g);
			}
		}
		m_OptionGameObjects.Clear();
	}

	protected void CreateOptionsContainer()
	{
		m_OptionsContGameObject = new GameObject("Options Grid", typeof(RectTransform), typeof(GridLayoutGroup));
		m_OptionsContGameObject.layer = base.gameObject.layer;
		m_OptionsContGameObject.transform.SetParent(base.transform, worldPositionStays: false);
		m_OptionsContGameObject.transform.SetAsFirstSibling();
		m_OptionsContGameObject.transform.localScale = Vector3.one;
		m_OptionsContGameObject.transform.localPosition = Vector3.zero;
		m_OptionsContRect = m_OptionsContGameObject.GetComponent<RectTransform>();
		m_OptionsContRect.sizeDelta = new Vector2(0f, 0f);
		m_OptionsContRect.anchorMin = new Vector2(0f, 0f);
		m_OptionsContRect.anchorMax = new Vector2(1f, 1f);
		m_OptionsContRect.pivot = new Vector2(0f, 1f);
		m_OptionsContRect.anchoredPosition = new Vector2(0f, 0f);
		m_OptionsContGrid = m_OptionsContGameObject.GetComponent<GridLayoutGroup>();
	}
}
