using System;
using UnityEngine;

public class Gem_R_Spiral : Gem
{
	public ScalingValue shootSpeed;

	public float maxShootSpeed;

	public float searchRadius;

	public AbilityTargetValidator targets;

	private float _lastCheckTime;

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer || !base.isValid || base.owner.isKnockedOut)
		{
			return;
		}
		float num = 1f / Mathf.Min(GetValue(shootSpeed), maxShootSpeed);
		if (!(Time.time - _lastCheckTime < num))
		{
			_lastCheckTime = Time.time;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.owner.agentPosition, searchRadius, targets, base.owner);
			if (readOnlySpan.Length > 0)
			{
				Entity target = readOnlySpan[global::UnityEngine.Random.Range(0, readOnlySpan.Length)];
				CreateAbilityInstance<Ai_Gem_R_Spiral_Fireball>(base.owner.position, null, new CastInfo(base.owner, target));
				NotifyUse();
			}
			handle.Return();
		}
	}

	private void MirrorProcessed()
	{
	}
}
