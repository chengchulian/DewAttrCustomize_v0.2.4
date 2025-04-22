using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_Constellations_MasteryLevels : MonoBehaviour
{
	public TextMeshProUGUI heroMasteryText;

	public Image heroMasteryFill;

	public TextMeshProUGUI totalMasteryText;

	private void OnEnable()
	{
		if (!(DewPlayer.local == null))
		{
			DewProfile.HeroMasteryData mastery = DewSave.profile.heroMasteries[DewPlayer.local.selectedHeroType];
			heroMasteryText.text = mastery.currentLevel.ToString("#,##0");
			heroMasteryFill.fillAmount = (float)mastery.currentPoints / (float)Dew.GetRequiredMasteryPointsToLevelUp(mastery.currentLevel);
			totalMasteryText.text = DewSave.profile.totalMasteryLevel.ToString("#,##0");
		}
	}
}
