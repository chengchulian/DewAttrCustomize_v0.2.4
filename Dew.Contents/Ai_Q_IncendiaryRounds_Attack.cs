using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Q_IncendiaryRounds_Attack : AbilityInstance
{
	public ScalingValue damage;

	public GameObject hitEffect;

	public GameObject addedFlyEffect;

	public GameObject aoeEffect;

	public DewCollider aoeRange;

	[NonSerialized]
	[SyncVar]
	public AbilityInstance attackInstance;

	[NonSerialized]
	public bool doAreaOfEffectDamage;

	protected NetworkBehaviourSyncVar ___attackInstanceNetId;

	public AbilityInstance NetworkattackInstance
	{
		get
		{
			return GetSyncVarNetworkBehaviour(___attackInstanceNetId, ref attackInstance);
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter_NetworkBehaviour(value, ref attackInstance, 32uL, null, ref ___attackInstanceNetId);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		if (NetworkattackInstance is Projectile projectile)
		{
			projectile.AddEffect(Projectile.AddEffectTarget.Fly, addedFlyEffect);
		}
		if (base.isServer)
		{
			NetworkattackInstance.ActorEvent_OnAttackHit += new Action<EventInfoAttackHit>(ActorEventOnAttackHit);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && NetworkattackInstance != null)
		{
			NetworkattackInstance.ActorEvent_OnAttackHit -= new Action<EventInfoAttackHit>(ActorEventOnAttackHit);
		}
	}

	private void ActorEventOnAttackHit(EventInfoAttackHit obj)
	{
		Damage(damage).SetElemental(ElementalType.Fire).Dispatch(obj.victim);
		FxPlayNewNetworked(hitEffect, obj.victim);
		if (doAreaOfEffectDamage)
		{
			aoeRange.transform.position = obj.victim.position;
			FxPlayNewNetworked(aoeEffect, obj.victim);
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = aoeRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				if (!(entity == obj.victim))
				{
					Damage(damage).SetElemental(ElementalType.Fire).Dispatch(entity);
				}
			}
			handle.Return();
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteNetworkBehaviour(NetworkattackInstance);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteNetworkBehaviour(NetworkattackInstance);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize_NetworkBehaviour(ref attackInstance, null, reader, ref ___attackInstanceNetId);
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize_NetworkBehaviour(ref attackInstance, null, reader, ref ___attackInstanceNetId);
		}
	}
}
