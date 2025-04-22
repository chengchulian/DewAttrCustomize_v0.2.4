using UnityEngine;

public class UI_Lobby_LucidDreams : MonoBehaviour
{
	public string[] lucidDreams;

	public UI_Lobby_LucidDreams_Item itemPrefab;

	public Transform groupParent;

	private void Start()
	{
		bool hasLucidDream = false;
		string[] array = lucidDreams;
		foreach (string l in array)
		{
			if (Dew.IsLucidDreamIncludedInGame(l))
			{
				Object.Instantiate(itemPrefab, groupParent).Setup(l);
				hasLucidDream = true;
			}
		}
		base.gameObject.SetActive(hasLucidDream);
	}
}
