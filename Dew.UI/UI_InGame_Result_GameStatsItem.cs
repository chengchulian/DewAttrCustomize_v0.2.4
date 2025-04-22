using System.Reflection;
using TMPro;
using UnityEngine;

public class UI_InGame_Result_GameStatsItem : MonoBehaviour, IGameResultStatItem
{
	public TextMeshProUGUI headerText;

	public TextMeshProUGUI contentText;

	public TextMeshProUGUI scoreText;

	public bool setHeaderText = true;

	public string key;

	public bool isTime;

	public string format = "#,##0";

	public float gainedScoreMultiplier;

	private FieldInfo _field;

	public double UpdateAndGetScore(DewGameResult data, int playerIndex, float scoreMultiplier)
	{
		_field = typeof(DewGameResult).GetField(key);
		if (setHeaderText)
		{
			headerText.text = DewLocalization.GetUIValue("InGame_Result_" + key);
		}
		double doubleValue = double.Parse(_field.GetValue(data).ToString());
		if (isTime)
		{
			long num = (long)doubleValue;
			long minutes = num / 60;
			long seconds = num % 60;
			contentText.text = $"{minutes:00}:{seconds:00}";
		}
		else
		{
			contentText.text = doubleValue.ToString(format);
		}
		doubleValue *= (double)(gainedScoreMultiplier * scoreMultiplier);
		if (gainedScoreMultiplier <= 0.0001f)
		{
			scoreText.text = "";
		}
		else
		{
			scoreText.text = string.Format(DewLocalization.GetUIValue("InGame_Result_ScoreFormat"), doubleValue.ToString("#,##0"));
		}
		return doubleValue;
	}
}
