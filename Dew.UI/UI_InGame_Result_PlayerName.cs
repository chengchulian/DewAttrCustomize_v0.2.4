using TMPro;
using UnityEngine;

public class UI_InGame_Result_PlayerName : MonoBehaviour, IGameResultStatItem
{
	public TextMeshProUGUI heroName;

	public TextMeshProUGUI playerName;

	public UI_HeroIcon heroIcon;

	public double UpdateAndGetScore(DewGameResult data, int playerIndex, float scoreMultiplier)
	{
		DewGameResult.PlayerData p = data.players[playerIndex];
		heroName.text = DewLocalization.GetUIValue(p.heroType + "_Name");
		playerName.text = p.playerProfileName;
		heroIcon.Setup(p.heroType);
		return 0.0;
	}
}
