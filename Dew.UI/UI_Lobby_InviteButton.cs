using Mirror;
using UnityEngine;

[LogicUpdatePriority(1050)]
public class UI_Lobby_InviteButton : LogicBehaviour
{
	public Transform[] followTargets;

	public Vector3 worldOffset;

	private CanvasGroup _cg;

	private void Awake()
	{
		GetComponent(out _cg);
	}

	private void Start()
	{
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (NetworkServer.dontListen || DewPlayer.humanPlayers.Count >= followTargets.Length)
		{
			_cg.alpha = 0f;
			_cg.interactable = false;
			_cg.blocksRaycasts = false;
		}
		else
		{
			_cg.alpha = 1f;
			_cg.interactable = true;
			_cg.blocksRaycasts = true;
			base.transform.position = Dew.mainCamera.WorldToScreenPoint(followTargets[DewPlayer.humanPlayers.Count].position + worldOffset).Quantitized();
		}
	}
}
