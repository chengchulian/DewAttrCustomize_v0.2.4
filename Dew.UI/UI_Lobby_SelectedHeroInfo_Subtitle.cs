public class UI_Lobby_SelectedHeroInfo_Subtitle : UI_Lobby_SelectedHeroInfo_Base
{
	protected override void OnHeroChanged()
	{
		base.OnHeroChanged();
		if (!(base.selectedHero == null))
		{
			base.text.text = DewLocalization.GetUIValue(base.selectedHeroName + "_Subtitle");
		}
	}
}
