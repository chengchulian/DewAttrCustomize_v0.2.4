using TMPro;
using UnityEngine;

public class UI_ApplicationVersion : MonoBehaviour, ICinematicCameraHelperStateReceiver
{
	private void Start()
	{
		GetComponent<TextMeshProUGUI>().text = Application.version;
	}

	public void OnCinematicCameraHelperChanged(bool on)
	{
		base.gameObject.SetActive(!on);
	}
}
