using System;
using UnityEngine;

namespace DuloGames.UI;

[Serializable]
public class UITooltipTextEffect
{
	public UITooltipTextEffectType Effect;

	public Color EffectColor;

	public Vector2 EffectDistance;

	public bool UseGraphicAlpha;

	public UITooltipTextEffect()
	{
		Effect = UITooltipTextEffectType.Shadow;
		EffectColor = new Color(0f, 0f, 0f, 128f);
		EffectDistance = new Vector2(1f, -1f);
		UseGraphicAlpha = true;
	}
}
