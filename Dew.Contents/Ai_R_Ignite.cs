using System;
using UnityEngine;

public class Ai_R_Ignite : AbilityInstance
{
	public DewCollider range;

	public ScalingValue dmgFactor;

	public float explodeDmgPercentage;

	public GameObject igniteEffect;

	public GameObject explodeEffect;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		CreateAbilityInstance<Ai_R_Ignite_FakeProjectile>(base.info.caster.agentPosition, null, new CastInfo(base.info.caster, base.info.target));
		DestroyOnDeath(base.info.caster);
		Entity target = base.info.target;
		if (target.Status.fireStack > 0)
		{
			range.transform.position = target.agentPosition;
			FxPlayNetworked(explodeEffect, target.agentPosition, null);
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				FxPlayNewNetworked(igniteEffect, entity);
				Damage(dmgFactor).ApplyRawMultiplier(1f + explodeDmgPercentage).SetElemental(ElementalType.Fire).SetAttr(DamageAttribute.IsCrit)
					.SetOriginPosition(target.agentPosition)
					.Dispatch(entity);
			}
			handle.Return();
			Destroy();
		}
		else
		{
			FxPlayNetworked(igniteEffect, target);
			Damage(dmgFactor).SetElemental(ElementalType.Fire).SetOriginPosition(base.info.caster.agentPosition).Dispatch(target);
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
