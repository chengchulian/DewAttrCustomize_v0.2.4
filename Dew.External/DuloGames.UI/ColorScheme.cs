using UnityEngine;

namespace DuloGames.UI;

public class ColorScheme : ScriptableObject
{
	[Header("Image Colors")]
	[SerializeField]
	private Color m_ImagePrimary = Color.white;

	[SerializeField]
	private Color m_ImageSecondary = Color.white;

	[SerializeField]
	private Color m_ImageLight = Color.white;

	[SerializeField]
	private Color m_ImageDark = Color.white;

	[SerializeField]
	private Color m_ImageEffect = Color.white;

	[SerializeField]
	private Color m_ImageEffect2 = Color.white;

	[SerializeField]
	private Color m_ImageEffect3 = Color.white;

	[SerializeField]
	private Color m_ImageBordersPrimary = Color.white;

	[HideInInspector]
	[SerializeField]
	private Color m_ImageBordersSecondary = Color.white;

	[Header("Button Colors")]
	[SerializeField]
	private Color m_ButtonForeground = Color.white;

	[SerializeField]
	private Color m_ButtonEffect = Color.white;

	[Header("Window Colors")]
	[SerializeField]
	private Color m_WindowHeader = Color.white;

	[Header("Animation Colors")]
	[SerializeField]
	private Color m_Animation = Color.white;

	public Color imagePrimary
	{
		get
		{
			return m_ImagePrimary;
		}
		set
		{
			m_ImagePrimary = value;
		}
	}

	public Color imageSecondary
	{
		get
		{
			return m_ImageSecondary;
		}
		set
		{
			m_ImageSecondary = value;
		}
	}

	public Color imageLight
	{
		get
		{
			return m_ImageLight;
		}
		set
		{
			m_ImageLight = value;
		}
	}

	public Color imageDark
	{
		get
		{
			return m_ImageDark;
		}
		set
		{
			m_ImageDark = value;
		}
	}

	public Color imageEffect
	{
		get
		{
			return m_ImageEffect;
		}
		set
		{
			m_ImageEffect = value;
		}
	}

	public Color imageEffect2
	{
		get
		{
			return m_ImageEffect2;
		}
		set
		{
			m_ImageEffect2 = value;
		}
	}

	public Color imageEffect3
	{
		get
		{
			return m_ImageEffect3;
		}
		set
		{
			m_ImageEffect3 = value;
		}
	}

	public Color imageBordersPrimary
	{
		get
		{
			return m_ImageBordersPrimary;
		}
		set
		{
			m_ImageBordersPrimary = value;
		}
	}

	public Color imageBordersSecondary
	{
		get
		{
			return m_ImageBordersSecondary;
		}
		set
		{
			m_ImageBordersSecondary = value;
		}
	}

	public Color buttonForeground
	{
		get
		{
			return m_ButtonForeground;
		}
		set
		{
			m_ButtonForeground = value;
		}
	}

	public Color buttonEffect
	{
		get
		{
			return m_ButtonEffect;
		}
		set
		{
			m_ButtonEffect = value;
		}
	}

	public Color windowHeader
	{
		get
		{
			return m_WindowHeader;
		}
		set
		{
			m_WindowHeader = value;
		}
	}

	public Color animation
	{
		get
		{
			return m_Animation;
		}
		set
		{
			m_Animation = value;
		}
	}

	public void ApplyColorScheme()
	{
		ColorSchemeElement[] array = Object.FindObjectsOfType<ColorSchemeElement>();
		foreach (ColorSchemeElement element in array)
		{
			ApplyToElement(element);
		}
		ColorSchemeElement_SelectField[] array2 = Object.FindObjectsOfType<ColorSchemeElement_SelectField>();
		foreach (ColorSchemeElement_SelectField element2 in array2)
		{
			ApplyToElement(element2);
		}
		if (ColorSchemeManager.Instance != null)
		{
			ColorSchemeManager.Instance.activeColorScheme = this;
		}
	}

	public Color GetColorShade(ColorSchemeShade shade)
	{
		Color newColor = Color.white;
		switch (shade)
		{
		case ColorSchemeShade.Primary:
			newColor = m_ImagePrimary;
			break;
		case ColorSchemeShade.Secondary:
			newColor = m_ImageSecondary;
			break;
		case ColorSchemeShade.Light:
			newColor = m_ImageLight;
			break;
		case ColorSchemeShade.Dark:
			newColor = m_ImageDark;
			break;
		case ColorSchemeShade.Effect:
			newColor = m_ImageEffect;
			break;
		case ColorSchemeShade.Effect2:
			newColor = m_ImageEffect2;
			break;
		case ColorSchemeShade.Effect3:
			newColor = m_ImageEffect3;
			break;
		case ColorSchemeShade.BordersPrimary:
			newColor = m_ImageBordersPrimary;
			break;
		case ColorSchemeShade.BordersSecondary:
			newColor = m_ImageBordersSecondary;
			break;
		case ColorSchemeShade.ButtonPrimary:
			newColor = m_ButtonForeground;
			break;
		case ColorSchemeShade.ButtonSecondary:
			newColor = m_ButtonEffect;
			break;
		case ColorSchemeShade.WindowHeader:
			newColor = m_WindowHeader;
			break;
		case ColorSchemeShade.Animation:
			newColor = m_Animation;
			break;
		}
		return newColor;
	}

	public void ApplyToElement(IColorSchemeElement element)
	{
		if (element != null)
		{
			Color newColor = GetColorShade(element.shade);
			element.Apply(newColor);
		}
	}
}
