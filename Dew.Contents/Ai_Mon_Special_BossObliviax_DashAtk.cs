using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_DashAtk : AbilityInstance
{
	public ScalingValue damage = "1.5ap";

	public GameObject fxTelegraph;

	public GameObject fxDash;

	public GameObject fxHit;

	public ChannelData channel;

	public float dashDuration;

	public DewCollider range;

	public float postDelay;

	public Knockback knockback;

	[SyncVar]
	public float telegraphDuration = 1f;

	public float NetworktelegraphDuration
	{
		get
		{
			return telegraphDuration;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref telegraphDuration, 32uL, null);
		}
	}

	protected override void OnCreate()
	{
		base.position = base.info.caster.agentPosition;
		base.rotation = base.info.rotation;
		fxTelegraph.ApplySpeedMultiplier(1f / telegraphDuration);
		BoxTelegraphController componentInChildren = fxTelegraph.GetComponentInChildren<BoxTelegraphController>();
		componentInChildren.width = range.size.x;
		componentInChildren.height = range.size.y;
		base.OnCreate();
		if (base.isServer)
		{
			base.info.caster.Control.Rotate(base.info.rotation, immediately: true);
			FxPlayNetworked(fxTelegraph, base.info.caster);
			channel.duration = telegraphDuration;
			channel.Get().AddOnComplete(DoDash).AddOnCancel(base.DestroyIfActive)
				.Dispatch(base.info.caster);
			CreateBasicEffect(base.info.caster, new UnstoppableEffect(), telegraphDuration + dashDuration, "ObliviaxUnstoppable").DestroyOnDestroy(this);
		}
	}

	private void DoDash()
	{
		FxPlayNetworked(fxDash, base.info.caster);
		Vector3 dest = base.transform.position + base.info.forward * (range.size.y - 3f);
		base.info.caster.Control.StartDaze(dashDuration + postDelay);
		base.info.caster.Control.StartDisplacement(new DispByDestination
		{
			canGoOverTerrain = false,
			destination = dest,
			isFriendly = true,
			rotateForward = true,
			affectedByMovementSpeed = false,
			isCanceledByCC = false,
			duration = dashDuration,
			ease = DewEase.EaseOutQuad,
			rotateSmoothly = false
		});
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			int iterations = 4;
			foreach (List<Entity> item in range.SweepEntitiesFromOrigin(iterations, tvDefaultHarmfulEffectTargets))
			{
				foreach (Entity item2 in item)
				{
					DefaultDamage(damage).SetDirection(base.info.forward).Dispatch(item2);
					knockback.distance = Vector3.Distance(item2.agentPosition, dest) + 3f;
					knockback.ApplyWithDirection(base.info.forward, item2);
					FxPlayNewNetworked(fxHit, item2);
				}
				yield return new WaitForSeconds(dashDuration / (float)iterations);
			}
			DestroyIfActive();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			FxStopNetworked(fxTelegraph);
		}
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(telegraphDuration);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteFloat(telegraphDuration);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref telegraphDuration, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref telegraphDuration, null, reader.ReadFloat());
		}
	}
}
