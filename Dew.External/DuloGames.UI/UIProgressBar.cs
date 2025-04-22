using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Bars/Progress Bar")]
public class UIProgressBar : MonoBehaviour, IUIProgressBar
{
	[Serializable]
	public class ChangeEvent : UnityEvent<float>
	{
	}

	public enum Type
	{
		Filled,
		Resize,
		Sprites
	}

	public enum FillSizing
	{
		Parent,
		Fixed
	}

	[SerializeField]
	private Type m_Type;

	[SerializeField]
	private Image m_TargetImage;

	[SerializeField]
	private Sprite[] m_Sprites;

	[SerializeField]
	private RectTransform m_TargetTransform;

	[SerializeField]
	private FillSizing m_FillSizing;

	[SerializeField]
	private float m_MinWidth;

	[SerializeField]
	private float m_MaxWidth = 100f;

	[SerializeField]
	[Range(0f, 1f)]
	private float m_FillAmount = 1f;

	[SerializeField]
	private int m_Steps;

	public ChangeEvent onChange = new ChangeEvent();

	public Type type
	{
		get
		{
			return m_Type;
		}
		set
		{
			m_Type = value;
		}
	}

	public Image targetImage
	{
		get
		{
			return m_TargetImage;
		}
		set
		{
			m_TargetImage = value;
		}
	}

	public Sprite[] sprites
	{
		get
		{
			return m_Sprites;
		}
		set
		{
			m_Sprites = value;
		}
	}

	public RectTransform targetTransform
	{
		get
		{
			return m_TargetTransform;
		}
		set
		{
			m_TargetTransform = value;
		}
	}

	public float minWidth
	{
		get
		{
			return m_MinWidth;
		}
		set
		{
			m_MinWidth = value;
			UpdateBarFill();
		}
	}

	public float maxWidth
	{
		get
		{
			return m_MaxWidth;
		}
		set
		{
			m_MaxWidth = value;
			UpdateBarFill();
		}
	}

	public float fillAmount
	{
		get
		{
			return m_FillAmount;
		}
		set
		{
			if (m_FillAmount != Mathf.Clamp01(value))
			{
				m_FillAmount = Mathf.Clamp01(value);
				UpdateBarFill();
				onChange.Invoke(m_FillAmount);
			}
		}
	}

	public int steps
	{
		get
		{
			return m_Steps;
		}
		set
		{
			m_Steps = value;
		}
	}

	public int currentStep
	{
		get
		{
			if (m_Steps == 0)
			{
				return 0;
			}
			float perStep = 1f / (float)(m_Steps - 1);
			return Mathf.RoundToInt(fillAmount / perStep);
		}
		set
		{
			if (m_Steps > 0)
			{
				float perStep = 1f / (float)(m_Steps - 1);
				fillAmount = (float)Mathf.Clamp(value, 0, m_Steps) * perStep;
			}
		}
	}

	protected virtual void Start()
	{
		if (m_Type == Type.Resize && m_FillSizing == FillSizing.Parent && m_TargetTransform != null)
		{
			float height = m_TargetTransform.rect.height;
			m_TargetTransform.anchorMin = m_TargetTransform.pivot;
			m_TargetTransform.anchorMax = m_TargetTransform.pivot;
			m_TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
		}
		UpdateBarFill();
	}

	protected virtual void OnRectTransformDimensionsChange()
	{
		UpdateBarFill();
	}

	public void UpdateBarFill()
	{
		if (!base.isActiveAndEnabled || (m_Type == Type.Filled && m_TargetImage == null) || (m_Type == Type.Resize && m_TargetTransform == null) || (m_Type == Type.Sprites && m_Sprites.Length == 0))
		{
			return;
		}
		float fill = m_FillAmount;
		if (m_Steps > 0)
		{
			fill = Mathf.Round(m_FillAmount * (float)(m_Steps - 1)) / (float)(m_Steps - 1);
		}
		if (m_Type == Type.Resize)
		{
			if (m_FillSizing == FillSizing.Fixed)
			{
				m_TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_MinWidth + (m_MaxWidth - m_MinWidth) * fill);
			}
			else
			{
				m_TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (m_TargetTransform.parent as RectTransform).rect.width * fill);
			}
		}
		else if (m_Type == Type.Sprites)
		{
			int spriteIndex = Mathf.RoundToInt(fill * (float)m_Sprites.Length) - 1;
			if (spriteIndex > -1)
			{
				targetImage.overrideSprite = m_Sprites[spriteIndex];
				targetImage.canvasRenderer.SetAlpha(1f);
			}
			else
			{
				targetImage.overrideSprite = null;
				targetImage.canvasRenderer.SetAlpha(0f);
			}
		}
		else
		{
			m_TargetImage.fillAmount = fill;
		}
	}

	public void AddFill()
	{
		if (m_Steps > 0)
		{
			currentStep++;
		}
		else
		{
			fillAmount += 0.1f;
		}
	}

	public void RemoveFill()
	{
		if (m_Steps > 0)
		{
			currentStep--;
		}
		else
		{
			fillAmount -= 0.1f;
		}
	}
}
