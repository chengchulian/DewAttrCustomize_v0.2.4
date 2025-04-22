using System;
using UnityEngine;

public class Forest_Fireplace : Actor
{
	public float damageMaxHealthRatio;

	public float damageInterval;

	public float damageProcCoeff;

	public float radius;

	public GameObject fxHit;

	private float _lastDamageTime;

	public override bool isDestroyedOnRoomChange => true;

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && Time.time - _lastDamageTime > damageInterval)
		{
			_lastDamageTime = Time.time;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.transform.position, radius);
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				Entity entity = readOnlySpan[i];
				DefaultDamage(damageMaxHealthRatio * entity.maxHealth, damageProcCoeff).SetElemental(ElementalType.Fire).SetOriginPosition(base.transform.position).Dispatch(entity);
				FxPlayNewNetworked(fxHit, entity);
			}
			handle.Return();
		}
	}

	private void MirrorProcessed()
	{
	}
}
