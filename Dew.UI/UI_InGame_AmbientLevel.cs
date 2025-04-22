using System;
using TMPro;
using UnityEngine;

public class UI_InGame_AmbientLevel : MonoBehaviour
{
	private TextMeshProUGUI _text;

	private void Start()
	{
		_text = GetComponent<TextMeshProUGUI>();
		NetworkedManagerBase<GameManager>.instance.ClientEvent_OnAmbientLevelChanged += new Action<int, int>(ClientEventOnAmbientLevelChanged);
		ClientEventOnAmbientLevelChanged(0, NetworkedManagerBase<GameManager>.instance.ambientLevel);
	}

	private void ClientEventOnAmbientLevelChanged(int arg1, int arg2)
	{
		_text.text = string.Format(DewLocalization.GetUIValue("InGame_ThreatLevelDisplay_AmbientLevel"), arg2);
	}
}
