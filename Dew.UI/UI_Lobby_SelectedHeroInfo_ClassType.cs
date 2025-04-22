using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_SelectedHeroInfo_ClassType : UI_Lobby_SelectedHeroInfo_Base
{
	[Serializable]
	public struct SerializedColorEntry
	{
		public Hero.HeroClassType type;

		public Color textColor;
	}

	[Serializable]
	public struct SerializedSpriteEntry
	{
		public Hero.HeroClassType type;

		public Sprite sprite;
	}

	public List<SerializedColorEntry> perClassTextColor;

	public Image classIcon;

	public List<SerializedSpriteEntry> perClassSprite;

	protected override void OnHeroChanged()
	{
		base.OnHeroChanged();
		if (base.selectedHero == null)
		{
			return;
		}
		base.text.color = perClassTextColor.Find((SerializedColorEntry e) => e.type == base.selectedHero.classType).textColor;
		base.text.text = DewLocalization.GetUIValue($"HeroClass_{base.selectedHero.classType}");
		if (classIcon != null)
		{
			classIcon.sprite = perClassSprite.Find((SerializedSpriteEntry e) => e.type == base.selectedHero.classType).sprite;
			classIcon.color = base.text.color;
		}
	}
}
