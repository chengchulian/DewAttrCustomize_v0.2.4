using System;
using UnityEngine;

public class UI_InGame_WorldView : View
{
	protected override void Start()
	{
		base.Start();
		if (Application.IsPlaying(this))
		{
			InGameUIManager instance = InGameUIManager.instance;
			instance.onWorldDisplayedChanged = (Action<WorldDisplayStatus>)Delegate.Combine(instance.onWorldDisplayedChanged, new Action<WorldDisplayStatus>(OnWorldDisplayedChanged));
		}
	}

	private void OnWorldDisplayedChanged(WorldDisplayStatus obj)
	{
		if (obj != 0)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	protected override void OnShow()
	{
		base.OnShow();
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_RoomRift_OpenLoop");
	}

	protected override void OnHide()
	{
		base.OnHide();
		ManagerBase<FeedbackManager>.instance.StopFeedbackEffect("UI_RoomRift_OpenLoop");
	}
}
