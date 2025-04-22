using TMPro;
using UnityEngine;
using UnityEngine.UI;

[LogicUpdatePriority(1050)]
public class UI_Lobby_PlayerInfoBase : LogicBehaviour
{
	public int index;

	public GameObject playerObjects;

	public GameObject emptyObjects;

	public RawImage heroIcon;

	public GameObject readyObjects;

	public TextMeshProUGUI playerPersonaName;

	public GameObject hostObjects;

	public Color selfColor;

	public Color othersColor;

	protected virtual void Start()
	{
		base.transform.position = Vector3.left * 50000f;
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		DewPlayer target = DewPlayer.humanPlayers.GetOrDefault(index);
		playerObjects.SetActive(target != null);
		emptyObjects.SetActive(target == null);
		if (!(target == null))
		{
			hostObjects.SetActive(target.isHostPlayer);
			heroIcon.texture = target.avatar;
			readyObjects.SetActive(target.isReady);
			playerPersonaName.color = (target.isLocalPlayer ? selfColor : othersColor);
			playerPersonaName.text = target.playerName;
		}
	}
}
