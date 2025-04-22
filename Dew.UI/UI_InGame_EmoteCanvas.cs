using System;
using UnityEngine;

public class UI_InGame_EmoteCanvas : MonoBehaviour
{
	private Canvas _canvas;

	private void Start()
	{
		_canvas = GetComponent<Canvas>();
		NetworkedManagerBase<ChatManager>.instance.ClientEvent_OnEmoteReceived += new Action<DewPlayer, string>(ClientEventOnEmoteReceived);
		InGameUIManager instance = InGameUIManager.instance;
		instance.onWorldDisplayedChanged = (Action<WorldDisplayStatus>)Delegate.Combine(instance.onWorldDisplayedChanged, (Action<WorldDisplayStatus>)delegate(WorldDisplayStatus status)
		{
			_canvas.enabled = status == WorldDisplayStatus.None;
		});
	}

	private void OnDestroy()
	{
		if (NetworkedManagerBase<ChatManager>.instance != null)
		{
			NetworkedManagerBase<ChatManager>.instance.ClientEvent_OnEmoteReceived -= new Action<DewPlayer, string>(ClientEventOnEmoteReceived);
		}
	}

	private void ClientEventOnEmoteReceived(DewPlayer sender, string emoteName)
	{
		Emote prefab = DewResources.GetByName<Emote>(emoteName);
		if (!(prefab == null))
		{
			global::UnityEngine.Object.Instantiate(prefab, base.transform).posGetter = delegate
			{
				Hero hero = sender.hero;
				Vector3 position = ((hero.Visual.healthBarPosition != null) ? hero.Visual.healthBarPosition.position : hero.Visual.GetAbovePosition());
				Vector3 position2 = hero.position;
				position.x = position2.x;
				position.z = position2.z;
				return ManagerBase<DewCamera>.instance.mainCamera.WorldToScreenPoint(position) + new Vector3(0f, 210f, 0f) * base.transform.lossyScale.x;
			};
		}
	}
}
