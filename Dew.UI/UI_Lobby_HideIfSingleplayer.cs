using UnityEngine;

public class UI_Lobby_HideIfSingleplayer : MonoBehaviour
{
	private void Start()
	{
		if (DewNetworkManager.networkMode == DewNetworkManager.Mode.Singleplayer)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
