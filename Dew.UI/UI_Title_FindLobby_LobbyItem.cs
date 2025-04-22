using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Title_FindLobby_LobbyItem : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public Action onDoubleClick;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI memberText;

	public UI_DifficultyIcon difficultyIcon;

	public GameObject[] connectionQualityObjects;

	public void Setup(LobbyInstance lobby, int index)
	{
		nameText.text = lobby.name;
		memberText.text = $"{lobby.currentPlayers}/{lobby.maxPlayers}";
		if (!string.IsNullOrEmpty(lobby.difficulty) && DewLocalization.TryGetUIValue("Difficulty_" + lobby.difficulty + "_Name", out var _))
		{
			difficultyIcon.Setup(lobby.difficulty);
			nameText.color = DewResources.GetByName<DewDifficultySettings>(lobby.difficulty).difficultyColor.WithS(0.2f).WithV(1f);
		}
		else
		{
			difficultyIcon.Setup("diffNormal");
			nameText.color = Color.white;
		}
		GameObject[] array = connectionQualityObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		connectionQualityObjects[(int)lobby.connectionQuality].SetActive(value: true);
		GetComponent<UI_Toggle>().index = index;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.clickCount == 2)
		{
			try
			{
				onDoubleClick?.Invoke();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}
}
