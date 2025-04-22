using System;
using System.Collections;
using Mirror;
using UnityEngine;

public class Se_MiniBoss_OrbSpitter : MiniBossEffect
{
	public Vector2 range;

	public Vector2 landTime;

	public GameObject fxShootCaster;

	public float startDelay;

	public float periodicShotInterval;

	public int perCastOrbs;

	public float perCastOrbInterval;

	public float targetedShotChance;

	public float targetedShotRandomMag;

	private float _lastShootTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.victim.EntityEvent_OnCastComplete += new Action<EventInfoCast>(EntityEventOnCastComplete);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.EntityEvent_OnCastComplete -= new Action<EventInfoCast>(EntityEventOnCastComplete);
		}
	}

	private void EntityEventOnCastComplete(EventInfoCast obj)
	{
		StopAllCoroutines();
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			for (int i = 0; i < perCastOrbs; i++)
			{
				ShootOrb();
				yield return new WaitForSeconds(perCastOrbInterval);
			}
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && !(Time.time - base.creationTime < startDelay) && Time.time - _lastShootTime > periodicShotInterval)
		{
			ShootOrb();
		}
	}

	[Server]
	public void ShootOrb()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void Se_MiniBoss_OrbSpitter::ShootOrb()' called when server was not active");
			return;
		}
		FxPlayNewNetworked(fxShootCaster, base.victim);
		_lastShootTime = Time.time;
		Vector3 pos = base.victim.position + global::UnityEngine.Random.insideUnitCircle.ToXZ() * global::UnityEngine.Random.Range(range.x, range.y);
		float landT = global::UnityEngine.Random.Range(landTime.x, landTime.y);
		if (global::UnityEngine.Random.value < targetedShotChance)
		{
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.victim.position, range.y, tvDefaultHarmfulEffectTargets);
			if (readOnlySpan.Length > 0)
			{
				Entity target = readOnlySpan[global::UnityEngine.Random.Range(0, readOnlySpan.Length)];
				pos = AbilityTrigger.PredictPoint_Simple(global::UnityEngine.Random.value, target, landT) + global::UnityEngine.Random.insideUnitCircle.ToXZ() * targetedShotRandomMag;
			}
			handle.Return();
		}
		Vector3 vector = (pos - base.victim.position).Flattened();
		if (vector.sqrMagnitude < range.x * range.x)
		{
			vector = vector.normalized * range.x;
			pos = base.victim.position + vector;
		}
		pos = Dew.GetPositionOnGround(pos);
		CreateAbilityInstance(base.info.caster.position, null, new CastInfo(base.info.caster, pos), delegate(Ai_MiniBoss_OrbSpitter_Orb a)
		{
			a.initialSpeed = Vector3.Distance(base.info.caster.position, pos) / landT;
			a.targetSpeed = a.initialSpeed;
			a.acceleration = 0f;
		});
	}

	private void MirrorProcessed()
	{
	}
}
