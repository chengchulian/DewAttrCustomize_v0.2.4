using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_HuntLevel : MonoBehaviour
{
	private UI_Common_DecoConstellation _constellation;

	private void Start()
	{
		_constellation = GetComponentInChildren<UI_Common_DecoConstellation>();
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnCurrentHuntLevelChanged += new Action(UpdateStatus);
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += (Action<EventInfoLoadRoom>)delegate
		{
			UpdateStatus();
		};
		base.gameObject.SetActive(value: false);
	}

	private void UpdateStatus()
	{
		base.gameObject.SetActive(NetworkedManagerBase<ZoneManager>.instance.currentHuntLevel > 0 && !NetworkedManagerBase<ZoneManager>.instance.isSidetracking);
		_constellation.timeScale = 1f + (float)Mathf.Clamp(NetworkedManagerBase<ZoneManager>.instance.currentHuntLevel, 0, 5) * 0.2f;
		for (int i = 0; i < _constellation.stars.Length; i++)
		{
			ListReturnHandle<Image> handle;
			foreach (Image item in ((Component)_constellation.stars[i]).GetComponentsInChildrenNonAlloc(out handle))
			{
				item.color = ((i < NetworkedManagerBase<ZoneManager>.instance.currentHuntLevel) ? Color.white : new Color(1f, 1f, 1f, 0.2f));
			}
			handle.Return();
		}
		_constellation.edges.Clear();
		for (int j = 0; j < NetworkedManagerBase<ZoneManager>.instance.currentHuntLevel - 1 && j < 4; j++)
		{
			_constellation.edges.Add(new UI_Common_DecoConstellation.Edge
			{
				a = j,
				b = j + 1
			});
		}
	}
}
