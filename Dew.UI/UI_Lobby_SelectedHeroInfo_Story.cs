public class UI_Lobby_SelectedHeroInfo_Story : UI_Lobby_SelectedHeroInfo_Base
{
	protected override void OnHeroChanged()
	{
		base.OnHeroChanged();
		if (!(base.selectedHero == null))
		{
			base.text.text = DewLocalization.GetUIValue(base.selectedHeroName + "_Story");
		}
	}
}
