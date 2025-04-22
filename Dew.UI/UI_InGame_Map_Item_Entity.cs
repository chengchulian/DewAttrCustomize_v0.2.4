using UnityEngine;

public class UI_InGame_Map_Item_Entity : UI_InGame_Map_Item_Actor
{
	public GameObject heroOwnObject;

	public GameObject heroAllyObject;

	public GameObject heroEnemyObject;

	public GameObject allyObject;

	public GameObject enemyObject;

	public GameObject neutralObject;

	public new Entity target => (Entity)base.target;

	public override bool IsSupported(Actor a)
	{
		return a is Entity;
	}

	public override MapItemOrder OnSetup(object t)
	{
		base.OnSetup(t);
		if (target is Hero)
		{
			TeamRelation rel = DewPlayer.local.GetTeamRelation(target);
			heroOwnObject.SetActive(rel == TeamRelation.Own);
			heroAllyObject.SetActive(rel == TeamRelation.Ally);
			heroEnemyObject.SetActive(rel == TeamRelation.Enemy);
			allyObject.SetActive(value: false);
			enemyObject.SetActive(value: false);
			neutralObject.SetActive(value: false);
			return rel switch
			{
				TeamRelation.Own => MapItemOrder.LocalHero, 
				TeamRelation.Enemy => MapItemOrder.HeroicEnemies, 
				_ => MapItemOrder.HeroicAllies, 
			};
		}
		if (target is Monster { type: Monster.MonsterType.Boss })
		{
			heroOwnObject.SetActive(value: false);
			heroAllyObject.SetActive(value: false);
			heroEnemyObject.SetActive(value: true);
			allyObject.SetActive(value: false);
			enemyObject.SetActive(value: false);
			neutralObject.SetActive(value: false);
			return MapItemOrder.HeroicEnemies;
		}
		TeamRelation r = DewPlayer.local.GetTeamRelation(target);
		heroOwnObject.SetActive(value: false);
		heroAllyObject.SetActive(value: false);
		heroEnemyObject.SetActive(value: false);
		allyObject.SetActive(r == TeamRelation.Ally);
		enemyObject.SetActive(r == TeamRelation.Enemy);
		neutralObject.SetActive(r == TeamRelation.Neutral);
		return MapItemOrder.NonImportantEntities;
	}

	public override bool ShouldBeDestroyed()
	{
		if (!(target == null) && target.isActive)
		{
			return target.isSleeping;
		}
		return true;
	}
}
