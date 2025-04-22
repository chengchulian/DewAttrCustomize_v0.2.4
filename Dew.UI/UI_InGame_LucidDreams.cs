using UnityEngine;

public class UI_InGame_LucidDreams : MonoBehaviour
{
	public UI_LucidDreamIcon iconPrefab;

	private void Start()
	{
		foreach (string l in NetworkedManagerBase<GameManager>.instance.GetLucidDreams())
		{
			Object.Instantiate(iconPrefab, base.transform).Setup(l);
		}
	}
}
