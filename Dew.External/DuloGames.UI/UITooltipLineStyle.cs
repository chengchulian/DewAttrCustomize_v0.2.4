using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[Serializable]
public class UITooltipLineStyle : IComparable<UITooltipLineStyle>
{
	public string Name;

	public Font TextFont;

	public FontStyle TextFontStyle;

	public int TextFontSize;

	public float TextFontLineSpacing;

	public OverrideTextAlignment OverrideTextAlignment;

	public Color TextFontColor;

	public UITooltipTextEffect[] TextEffects;

	public bool DisplayName = true;

	public UITooltipLineStyle()
	{
		Defaults();
	}

	public UITooltipLineStyle(bool displayName)
	{
		Defaults();
		DisplayName = displayName;
	}

	private void Defaults()
	{
		Name = "";
		TextFont = FontData.defaultFontData.font;
		TextFontStyle = FontData.defaultFontData.fontStyle;
		TextFontSize = FontData.defaultFontData.fontSize;
		TextFontLineSpacing = FontData.defaultFontData.lineSpacing;
		OverrideTextAlignment = OverrideTextAlignment.No;
		TextFontColor = Color.white;
		TextEffects = new UITooltipTextEffect[0];
	}

	public int CompareTo(UITooltipLineStyle other)
	{
		return Name.CompareTo(other.Name);
	}
}
