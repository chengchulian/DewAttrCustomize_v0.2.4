using System;
using UnityEngine;

public class Trap_FlameJet : Room_Trap_Toggleable
{
	public GameObject fxHit;

	public float startGracePeriod;

	public float dmgMaxHpRatio;

	public float dmgProcCoefficient;

	public float dmgInterval;

	public float monsterDmgMultiplier;

	public DewCollider range;

	private float _lastDamageTick;

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (!base.isServer || !base.isOn || Time.time - _lastDamageTick < dmgInterval || Time.time - base.startTime < startGracePeriod)
		{
			return;
		}
		_lastDamageTick = Time.time;
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			float num = dmgMaxHpRatio * entity.maxHealth;
			if (entity is Monster)
			{
				num *= monsterDmgMultiplier;
			}
			DefaultDamage(num, dmgProcCoefficient).SetElemental(ElementalType.Fire).SetOriginPosition(base.transform.position).Dispatch(entity);
			FxPlayNewNetworked(fxHit, entity);
		}
		handle.Return();
	}

	private void MirrorProcessed()
	{
	}
}
