using System;
using System.Collections;
using UnityEngine;

public class UI_EntityHealthBarGroup : MonoBehaviour
{
	public Transform myHeroParent;

	public Transform heroicParent;

	public Transform othersParent;

	public UI_EntityProvider heroMine;

	public UI_EntityProvider heroEnemy;

	public UI_EntityProvider heroNeutral;

	public UI_EntityProvider heroAlly;

	public UI_EntityProvider simpleAlly;

	public UI_EntityProvider simpleMine;

	public UI_EntityProvider simpleNeutral;

	public UI_EntityProvider simpleEnemy;

	private void Start()
	{
		NetworkedManagerBase<ActorManager>.instance.onAwakeEntityAdd += new Action<Entity>(CreateHealthBar);
		NetworkedManagerBase<ClientEventManager>.instance.OnRefreshEntityHealthbar += new Action<Entity>(OnRefreshEntityHealthbar);
		foreach (Entity ent in NetworkedManagerBase<ActorManager>.instance.allEntities)
		{
			if (!ent.isSleeping)
			{
				CreateHealthBar(ent);
			}
		}
	}

	private void OnRefreshEntityHealthbar(Entity obj)
	{
		CreateHealthBar(obj);
	}

	private void CreateHealthBar(Entity entity)
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return null;
			if (!(entity is IHideHealthbar))
			{
				if (entity is Hero || entity is IForceHeroicHealthbar || entity.IsAnyBoss())
				{
					switch (DewPlayer.local.GetTeamRelation(entity.owner))
					{
					case TeamRelation.Own:
						global::UnityEngine.Object.Instantiate(heroMine, myHeroParent).target = entity;
						break;
					case TeamRelation.Neutral:
						global::UnityEngine.Object.Instantiate(heroNeutral, heroicParent).target = entity;
						break;
					case TeamRelation.Enemy:
						global::UnityEngine.Object.Instantiate(heroEnemy, heroicParent).target = entity;
						break;
					case TeamRelation.Ally:
						global::UnityEngine.Object.Instantiate(heroAlly, heroicParent).target = entity;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				else
				{
					switch ((DewPlayer.local == null) ? TeamRelation.Neutral : DewPlayer.local.GetTeamRelation(entity.owner))
					{
					case TeamRelation.Own:
						global::UnityEngine.Object.Instantiate(simpleMine, othersParent).target = entity;
						break;
					case TeamRelation.Neutral:
						global::UnityEngine.Object.Instantiate(simpleNeutral, othersParent).target = entity;
						break;
					case TeamRelation.Enemy:
						global::UnityEngine.Object.Instantiate(simpleEnemy, othersParent).target = entity;
						break;
					case TeamRelation.Ally:
						global::UnityEngine.Object.Instantiate(simpleAlly, othersParent).target = entity;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
			}
		}
	}
}
