using System;
using System.Collections;
using UnityEngine;

public class Ai_R_Cataclysm_Meteor : InstantDamageInstance
{
	public Transform rotationTransform;

	public Transform fallTransform;

	public GameObject flyEffect;

	public GameObject impactEffect;

	public float fallSpeed;

	public float stunDuration;

	[NonSerialized]
	public bool spawnBurnInstance;

	protected override void OnCreate()
	{
		rotationTransform.rotation = Quaternion.Euler(0f, global::UnityEngine.Random.Range(0, 360), 0f);
		if (base.isServer)
		{
			StartSequence(Sequence());
		}
		base.OnCreate();
		IEnumerator Sequence()
		{
			FxPlayNetworked(flyEffect);
			yield return new SI.WaitForSeconds(damageDelay);
			FxPlayNetworked(impactEffect);
			FxStopNetworked(flyEffect);
			if (spawnBurnInstance)
			{
				CreateAbilityInstance<Ai_R_Cataclysm_Meteor_Burn>(base.transform.position, null, new CastInfo(base.info.caster));
			}
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		fallTransform.position += fallTransform.forward * (fallSpeed * Time.deltaTime);
	}

	protected override void OnHit(Entity entity)
	{
		base.OnHit(entity);
		CreateBasicEffect(entity, new StunEffect(), stunDuration, "cataclysm_stun");
	}

	private void MirrorProcessed()
	{
	}
}
