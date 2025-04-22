using System;
using System.Collections;
using UnityEngine;

public class UI_InGame_LoopNotice : View
{
	protected override void Awake()
	{
		base.Awake();
		if (Application.IsPlaying(this))
		{
			NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += new Action<EventInfoLoadRoom>(ClientEventOnRoomLoaded);
		}
	}

	private void ClientEventOnRoomLoaded(EventInfoLoadRoom _)
	{
		if (!base.isShowing && !DewSave.profile.didReadLoopNotice && !(PlayGameManager.instance == null) && NetworkedManagerBase<ZoneManager>.instance.loopIndex >= 1)
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			yield return new WaitWhile(() => NetworkedManagerBase<ZoneManager>.instance.isInRoomTransition);
			Show();
			DewSave.profile.didReadLoopNotice = true;
			DewSave.SaveProfile();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.F4))
		{
			Show();
			DewSave.profile.didReadLoopNotice = true;
			DewSave.SaveProfile();
		}
	}

	public void Close()
	{
		Hide();
	}
}
