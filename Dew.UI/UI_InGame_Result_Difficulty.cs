using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_Result_Difficulty : MonoBehaviour, IGameResultStatItem
{
	public UI_DifficultyIcon icon;

	public Image glow;

	public TextMeshProUGUI scoreMultiplierText;

	public double UpdateAndGetScore(DewGameResult data, int playerIndex, float scoreMultiplier)
	{
		DewDifficultySettings diff = DewResources.GetByName<DewDifficultySettings>(data.difficulty);
		icon.Setup(data.difficulty);
		glow.color = diff.difficultyColor.WithA(0.1f);
		scoreMultiplierText.text = $"x{scoreMultiplier:0.0}";
		return 0.0;
	}
}
