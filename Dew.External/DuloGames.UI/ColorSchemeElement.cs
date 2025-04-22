using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[ExecuteAlways]
[AddComponentMenu("UI/Color Scheme Element", 48)]
public class ColorSchemeElement : MonoBehaviour, IColorSchemeElement
{
	[SerializeField]
	private ColorSchemeShade m_Shade;

	public ColorSchemeShade shade
	{
		get
		{
			return m_Shade;
		}
		set
		{
			m_Shade = value;
		}
	}

	protected void Awake()
	{
		if (ColorSchemeManager.Instance != null && ColorSchemeManager.Instance.activeColorScheme != null)
		{
			ColorSchemeManager.Instance.activeColorScheme.ApplyToElement(this);
		}
	}

	public void Apply(Color newColor)
	{
		Graphic graphic = base.gameObject.GetComponent<Graphic>();
		if (!(graphic == null))
		{
			Color colorToChange = new Color(newColor.r, newColor.g, newColor.b, graphic.color.a);
			if (!(Math.Abs(graphic.color.r - colorToChange.r) < 0.01f) || !(Math.Abs(graphic.color.g - colorToChange.g) < 0.01f) || !(Math.Abs(graphic.color.b - colorToChange.b) < 0.01f) || !(Math.Abs(graphic.color.a - colorToChange.a) < 0.01f))
			{
				graphic.color = colorToChange;
			}
		}
	}
}
