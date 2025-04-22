using System;
using UnityEngine;

namespace DuloGames.UI;

[Serializable]
public struct ColorBlockExtended
{
	[SerializeField]
	private Color m_NormalColor;

	[SerializeField]
	private Color m_HighlightedColor;

	[SerializeField]
	private Color m_PressedColor;

	[SerializeField]
	private Color m_ActiveColor;

	[SerializeField]
	private Color m_ActiveHighlightedColor;

	[SerializeField]
	private Color m_ActivePressedColor;

	[SerializeField]
	private Color m_DisabledColor;

	[Range(1f, 5f)]
	[SerializeField]
	private float m_ColorMultiplier;

	[SerializeField]
	private float m_FadeDuration;

	public static ColorBlockExtended defaultColorBlock
	{
		get
		{
			ColorBlockExtended result = default(ColorBlockExtended);
			result.m_NormalColor = new Color32(128, 128, 128, 128);
			result.m_HighlightedColor = new Color32(128, 128, 128, 178);
			result.m_PressedColor = new Color32(88, 88, 88, 178);
			result.m_ActiveColor = new Color32(128, 128, 128, 128);
			result.m_ActiveHighlightedColor = new Color32(128, 128, 128, 178);
			result.m_ActivePressedColor = new Color32(88, 88, 88, 178);
			result.m_DisabledColor = new Color32(64, 64, 64, 128);
			result.m_ColorMultiplier = 1f;
			result.m_FadeDuration = 0.1f;
			return result;
		}
	}

	public Color normalColor
	{
		get
		{
			return m_NormalColor;
		}
		set
		{
			m_NormalColor = value;
		}
	}

	public Color highlightedColor
	{
		get
		{
			return m_HighlightedColor;
		}
		set
		{
			m_HighlightedColor = value;
		}
	}

	public Color pressedColor
	{
		get
		{
			return m_PressedColor;
		}
		set
		{
			m_PressedColor = value;
		}
	}

	public Color disabledColor
	{
		get
		{
			return m_DisabledColor;
		}
		set
		{
			m_DisabledColor = value;
		}
	}

	public Color activeColor
	{
		get
		{
			return m_ActiveColor;
		}
		set
		{
			m_ActiveColor = value;
		}
	}

	public Color activeHighlightedColor
	{
		get
		{
			return m_ActiveHighlightedColor;
		}
		set
		{
			m_ActiveHighlightedColor = value;
		}
	}

	public Color activePressedColor
	{
		get
		{
			return m_ActivePressedColor;
		}
		set
		{
			m_ActivePressedColor = value;
		}
	}

	public float colorMultiplier
	{
		get
		{
			return m_ColorMultiplier;
		}
		set
		{
			m_ColorMultiplier = value;
		}
	}

	public float fadeDuration
	{
		get
		{
			return m_FadeDuration;
		}
		set
		{
			m_FadeDuration = value;
		}
	}
}
