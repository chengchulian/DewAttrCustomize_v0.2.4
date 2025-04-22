using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_Constellations_TitleAndEmblem : MonoBehaviour
{
	public TextMeshProUGUI nameText;

	public Image[] coloredImages;

	public TextMeshProUGUI subtitleText;

	public UI_HeroIcon heroIcon;

	private void OnEnable()
	{
		UpdateColors();
	}

	private void UpdateColors()
	{
		if (!(DewPlayer.local == null))
		{
			Hero hero = DewResources.GetByShortTypeName<Hero>(DewPlayer.local.selectedHeroType);
			heroIcon.Setup(DewPlayer.local.selectedHeroType);
			nameText.text = DewLocalization.GetUIValue(DewPlayer.local.selectedHeroType + "_Name");
			nameText.color = Color.Lerp(hero.mainColor, Color.white, 0.45f);
			subtitleText.text = DewLocalization.GetUIValue(DewPlayer.local.selectedHeroType + "_Subtitle");
			subtitleText.color = Color.Lerp(hero.mainColor, Color.white, 0.75f);
			Image[] array = coloredImages;
			foreach (Image c in array)
			{
				c.color = Color.Lerp(hero.mainColor, Color.white, 0.3f).WithA(c.color.a);
			}
		}
	}
}
