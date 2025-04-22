using System;
using System.Collections;
using UnityEngine;

public class UI_InGame_OffscreenIndicator : MonoBehaviour
{
	public Vector2 screenMargin;

	public UI_InGame_OffscreenIndicator_Item_Entity partyMemberPrefab;

	public UI_InGame_OffscreenIndicator_Item_Entity enemyPrefab;

	public Transform partyMemberParent;

	public Transform enemyParent;

	private void Start()
	{
		NetworkedManagerBase<ActorManager>.instance.onEntityAdd += new Action<Entity>(HandleNewEntity);
		foreach (Hero h in NetworkedManagerBase<ActorManager>.instance.allHeroes)
		{
			HandleNewEntity(h);
		}
	}

	private void HandleNewEntity(Entity e)
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(0.1f);
			if (!e.isOwned && !(e == null) && e.isActive && !(DewPlayer.local == null))
			{
				UI_InGame_OffscreenIndicator_Item_Entity prefab;
				Transform parent;
				if (e is Hero)
				{
					prefab = partyMemberPrefab;
					parent = partyMemberParent;
				}
				else
				{
					if (DewPlayer.local.GetTeamRelation(e) != TeamRelation.Enemy)
					{
						yield break;
					}
					prefab = enemyPrefab;
					parent = enemyParent;
				}
				if (!(prefab == null))
				{
					UI_InGame_OffscreenIndicator_Item_Entity uI_InGame_OffscreenIndicator_Item_Entity = global::UnityEngine.Object.Instantiate(prefab, parent);
					uI_InGame_OffscreenIndicator_Item_Entity.GetComponent<UI_EntityProvider>().target = e;
					uI_InGame_OffscreenIndicator_Item_Entity.screenMargin = screenMargin;
				}
			}
		}
	}
}
