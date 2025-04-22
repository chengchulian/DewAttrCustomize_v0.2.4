using System;
using TMPro;
using UnityEngine;

public class UI_InGame_Location : MonoBehaviour
{
	private TextMeshProUGUI _text;

	private void Start()
	{
		_text = GetComponent<TextMeshProUGUI>();
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += new Action<EventInfoLoadRoom>(OnRoomLoaded);
		if (NetworkedManagerBase<ZoneManager>.instance.netIdentity != null)
		{
			OnRoomLoaded(default(EventInfoLoadRoom));
		}
	}

	private void OnRoomLoaded(EventInfoLoadRoom _)
	{
		if (NetworkedManagerBase<ZoneManager>.instance.currentRoom == null || NetworkedManagerBase<ZoneManager>.instance.currentZone == null)
		{
			_text.text = "";
			return;
		}
		Special_RoomAnnouncer special = global::UnityEngine.Object.FindObjectOfType<Special_RoomAnnouncer>();
		if (special != null && special.setRoomName)
		{
			_text.text = "(?). " + DewLocalization.GetUIValue(special.key + "_Name");
		}
		else
		{
			_text.text = string.Format("{0}. {1}", NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex + 1, DewLocalization.GetUIValue(NetworkedManagerBase<ZoneManager>.instance.currentZone.name + "_Name"));
		}
	}
}
