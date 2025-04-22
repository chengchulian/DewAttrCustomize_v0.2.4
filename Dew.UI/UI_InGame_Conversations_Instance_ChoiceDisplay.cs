using System;
using DewInternal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_Conversations_Instance_ChoiceDisplay : MonoBehaviour
{
	public Action<int> OnChoiceClick;

	public GameObject[] choiceObjects;

	private int[] _choices;

	private void Awake()
	{
		for (int i = 0; i < choiceObjects.Length; i++)
		{
			int index = i;
			choiceObjects[i].GetComponentInChildren<Button>().onClick.AddListener(delegate
			{
				OnChoiceClick?.Invoke(_choices[index]);
			});
		}
	}

	public void Setup(ConversationData data, int[] choices)
	{
		_choices = choices;
		for (int i = 0; i < choiceObjects.Length; i++)
		{
			choiceObjects[i].SetActive(choices != null && i < choices.Length);
			if (choices != null && i < choices.Length)
			{
				choiceObjects[i].GetComponentInChildren<TextMeshProUGUI>().text = data.lines[choices[i]].text;
			}
		}
	}
}
