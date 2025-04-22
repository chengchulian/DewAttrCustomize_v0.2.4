using UnityEngine;

public class UI_Lobby_SelectedHeroInfo_Difficulty : UI_Lobby_SelectedHeroInfo_Base
{
	public Color[] diffColors;

	protected override void OnHeroChanged()
	{
		base.OnHeroChanged();
		if (!(base.selectedHero == null))
		{
			base.text.text = DewLocalization.GetUIValue($"HeroDifficulty_{base.selectedHero.difficulty}");
			base.text.color = diffColors[(int)base.selectedHero.difficulty];
		}
	}
}
