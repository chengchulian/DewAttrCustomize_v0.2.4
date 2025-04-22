using System.Reflection;
using TMPro;
using UnityEngine;

public class UI_InGame_Result_PlayerStatsItem : MonoBehaviour, IGameResultStatItem
{
	public enum BestType
	{
		DontShow,
		GreaterBetter,
		LesserBetter
	}

	public TextMeshProUGUI headerText;

	public TextMeshProUGUI contentText;

	public TextMeshProUGUI scoreText;

	public bool setHeaderText = true;

	public string key;

	public string format = "#,##0";

	public float gainedScoreMultiplier;

	public BestType showBest;

	public GameObject bestObject;

	private FieldInfo _field;

	public double UpdateAndGetScore(DewGameResult data, int playerIndex, float scoreMultiplier)
	{
		_field = typeof(DewGameResult.PlayerData).GetField(key);
		if (bestObject != null)
		{
			CalculateBest(data, playerIndex);
		}
		if (setHeaderText && headerText != null)
		{
			headerText.text = DewLocalization.GetUIValue("InGame_Result_" + key);
		}
		double doubleVal = double.Parse(_field.GetValue(data.players[playerIndex]).ToString());
		if (contentText != null)
		{
			contentText.text = doubleVal.ToString(format);
		}
		doubleVal *= (double)(gainedScoreMultiplier * scoreMultiplier);
		if (scoreText != null)
		{
			if (gainedScoreMultiplier <= 0.0001f)
			{
				scoreText.text = "";
			}
			else
			{
				scoreText.text = string.Format(DewLocalization.GetUIValue("InGame_Result_ScoreFormat"), doubleVal.ToString("#,##0"));
			}
		}
		return doubleVal;
	}

	private void CalculateBest(DewGameResult data, int playerIndex)
	{
		if (showBest == BestType.DontShow || data.players.Count < 2)
		{
			bestObject.SetActive(value: false);
			return;
		}
		double myValue = double.Parse(_field.GetValue(data.players[playerIndex]).ToString());
		double bestValue = myValue;
		foreach (DewGameResult.PlayerData p in data.players)
		{
			double value = double.Parse(_field.GetValue(p).ToString());
			if (showBest == BestType.GreaterBetter && value > bestValue)
			{
				bestValue = value;
			}
			else if (showBest == BestType.LesserBetter && value < bestValue)
			{
				bestValue = value;
			}
		}
		bestObject.SetActive(myValue == bestValue);
	}
}
