using TMPro;
using UnityEngine;

public class UI_Title_ProfileSelection_ListWindow_Item : MonoBehaviour
{
	public GameObject loadedObject;

	public GameObject unsupportedObject;

	public GameObject convertibleObject;

	public GameObject corruptedObject;

	public TextMeshProUGUI profileNameText;

	public TextMeshProUGUI playTimeText;

	public void Setup(DewProfileItem item)
	{
		if (item.peek == null)
		{
			profileNameText.text = "???";
			playTimeText.text = "";
		}
		else
		{
			profileNameText.text = item.peek.name;
			long mins = item.peek.totalPlayTimeMinutes;
			playTimeText.text = $"{mins / 60:00}:{mins % 60:00}";
		}
		loadedObject.SetActive(item.path == DewSave.profilePath);
		GameObject obj = unsupportedObject;
		DewProfileState state = item.state;
		obj.SetActive(state == DewProfileState.UnsupportedEdition || state == DewProfileState.UnsupportedVersion);
		convertibleObject.SetActive(item.state == DewProfileState.Convertible);
		corruptedObject.SetActive(item.state == DewProfileState.Corrupted);
	}
}
