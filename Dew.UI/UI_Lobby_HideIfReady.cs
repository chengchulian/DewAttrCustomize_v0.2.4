using UnityEngine;

public class UI_Lobby_HideIfReady : LogicBehaviour
{
	private CanvasGroup _cg;

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		bool shouldBeVisible = DewPlayer.local != null && !DewPlayer.local.isReady;
		_cg.SetActivationState(shouldBeVisible);
	}
}
