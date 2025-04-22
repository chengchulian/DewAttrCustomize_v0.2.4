using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;

public class UI_NetworkStatus : MonoBehaviour, ICinematicCameraHelperStateReceiver
{
	public TextMeshProUGUI text;

	private void OnEnable()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			while (true)
			{
				UpdateText();
				yield return new WaitForSeconds(2f);
			}
		}
	}

	private void UpdateText()
	{
		if (NetworkServer.active)
		{
			if (NetworkServer.dontListen)
			{
				text.text = "local";
			}
			else
			{
				text.text = $"host, {DewPlayer.humanPlayers.Count} players";
			}
		}
		else if (NetworkClient.active)
		{
			text.text = $"client, {DewPlayer.humanPlayers.Count} players, {NetworkTime.rtt * 1000.0:#,##0}ms";
		}
		else
		{
			text.text = "";
		}
	}

	public void OnCinematicCameraHelperChanged(bool on)
	{
		base.gameObject.SetActive(!on);
	}
}
