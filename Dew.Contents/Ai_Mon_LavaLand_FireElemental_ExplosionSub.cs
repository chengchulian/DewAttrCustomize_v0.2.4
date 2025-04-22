using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Ai_Mon_LavaLand_FireElemental_ExplosionSub : AbilityInstance
{
	public DewCollider range;

	public float intervalDelay = 0.5f;

	public GameObject hitEffect;

	public ScalingValue dmgFactor = new ScalingValue(0f, 0f, 1f, 0f);

	public float existTime;

	public float slowStrength;

	public float slowDuration = 3f;

	private float _lastDamagedTime;

	protected override void OnCreate()
	{
		Vector3 pos = base.transform.position;
		for (int i = -2; i <= 2; i++)
		{
			for (int j = -2; j <= 2; j++)
			{
				CheckOffsetGround(new Vector3((float)i * 1.5f, 0f, (float)j * 1.5f), ref pos);
			}
		}
		base.transform.position = pos;
		if (base.isServer && Physics.Raycast(new Ray(base.transform.position, Vector3.down), out var hitInfo, 3f, LayerMask.GetMask("Ground")) && hitInfo.collider.gameObject.TryGetComponent<LavaLand_Lava>(out var component) && component.isActiveAndEnabled)
		{
			FxStopNetworked(startEffect);
			Destroy();
		}
		base.OnCreate();
		if (base.isServer)
		{
			_lastDamagedTime = Time.time;
		}
	}

	private void CheckOffsetGround(Vector3 offset, ref Vector3 pos)
	{
		Vector3 positionOnGround = Dew.GetPositionOnGround(pos + offset);
		Debug.DrawLine(positionOnGround, positionOnGround + Vector3.up * 3f, Color.yellow, 1f);
		if (!(positionOnGround.y < pos.y) && positionOnGround.y - pos.y < 2f && Dew.GetNavMeshPathStatus(pos, positionOnGround) == NavMeshPathStatus.PathComplete)
		{
			pos.y = positionOnGround.y + 0.1f;
		}
	}

	protected override IEnumerator OnCreateSequenced()
	{
		yield return base.OnCreateSequenced();
		if (base.isServer)
		{
			yield return new SI.WaitForSeconds(existTime);
			Destroy();
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && !(Time.time - _lastDamagedTime < intervalDelay))
		{
			_lastDamagedTime = Time.time;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity e = entities[i];
				OnHit(e);
			}
			handle.Return();
		}
	}

	private void OnHit(Entity e)
	{
		FxPlayNewNetworked(hitEffect, e);
		CreateDamage(DamageData.SourceType.Default, dmgFactor).SetElemental(ElementalType.Fire).SetAttr(DamageAttribute.DamageOverTime).Dispatch(e);
		CreateBasicEffect(e, new SlowEffect
		{
			strength = slowStrength
		}, slowDuration);
	}

	private void MirrorProcessed()
	{
	}
}
