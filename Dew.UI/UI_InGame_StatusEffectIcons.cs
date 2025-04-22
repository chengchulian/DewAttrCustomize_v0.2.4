using System;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_InGame_StatusEffectIcons : MonoBehaviour
{
	[FormerlySerializedAs("ignoreInCombat")]
	public bool isWorldHealthBar;

	public UI_InGame_StatusEffectIcons_Item itemPrefab;

	public Transform itemParent;

	private UI_EntityProvider _provider;

	private void Start()
	{
		_provider = GetComponentInParent<UI_EntityProvider>();
		NetworkedManagerBase<ClientEventManager>.instance.OnShowStatusEffectIcon += new Action<StatusEffect>(ShowStatusEffectIcon);
	}

	private void ShowStatusEffectIcon(StatusEffect obj)
	{
		if (!(obj.victim != _provider.target) && (!isWorldHealthBar || !obj.hideOnWorldHealthBar))
		{
			global::UnityEngine.Object.Instantiate(itemPrefab, itemParent).Setup(obj);
		}
	}
}
