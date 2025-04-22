using System;
using UnityEngine;

public class Ai_Q_EtherealInfluence : AbilityInstance
{
	public DewCollider explodeRange;

	public GameObject explodeEffect;

	public GameObject explodeHitEffect;

	public ScalingValue explodeDamage;

	public float explodeProcCoefficient;

	internal Vector3 _projectilePos;

	private AbilityTrigger.ChangedConfigHandle _handle;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		CreateAbilityInstance<Ai_Q_EtherealInfluence_Projectile_Forward>(base.info.caster.position, Quaternion.identity, new CastInfo(base.info.caster, base.info.angle));
		if (base.firstTrigger != null)
		{
			_handle = base.firstTrigger.ChangeConfigTimedOnce(1, 10f, OnUse, delegate
			{
				if (base.isActive)
				{
					Destroy();
				}
			}, setFillAmount: false);
		}
		ActorEvent_OnAbilityInstanceCreated += (Action<EventInfoAbilityInstance>)delegate(EventInfoAbilityInstance obj)
		{
			if (obj.instance is Ai_Q_EtherealInfluence_Projectile_Backward)
			{
				obj.instance.ClientActorEvent_OnDestroyed += (Action<Actor>)delegate
				{
					if (_handle != null && _handle.isActive)
					{
						_handle.Stop();
						_handle = null;
						OnUse(default(EventInfoAbilityInstance));
					}
					if (base.isActive)
					{
						Destroy();
					}
				};
			}
		};
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && Time.time - base.creationTime > 20f)
		{
			Destroy();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && _handle != null && _handle.isActive)
		{
			_handle.Stop();
		}
	}

	private void OnUse(EventInfoAbilityInstance obj)
	{
		_handle = null;
		FxPlayNewNetworked(explodeEffect, _projectilePos, Quaternion.identity);
		explodeRange.transform.position = _projectilePos;
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = explodeRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			Damage(explodeDamage, explodeProcCoefficient).SetElemental(ElementalType.Light).SetOriginPosition(_projectilePos).Dispatch(entity);
			FxPlayNewNetworked(explodeHitEffect, entity);
			CreateStatusEffect<Se_Q_EtherealInfluence_Root>(entity);
		}
		handle.Return();
	}

	private void MirrorProcessed()
	{
	}
}
