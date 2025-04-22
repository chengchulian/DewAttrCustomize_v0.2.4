using System.Collections;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_TurretMode : AbilityInstance
{
	public float radius;

	public int shootCount;

	public float maxDeviation;

	public float interval;

	public float minRangeFromSelf = 2.5f;

	public float maxRangeFromSelf;

	public float postDelay;

	public DewAnimationClip castingAnimClip;

	public DewAnimationClip endAnimClip;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		CreateBasicEffect(base.info.caster, new UnstoppableEffect(), (float)shootCount * interval, "ObliviaxUnstoppable");
		base.info.caster.Control.StartDaze((float)shootCount * interval + postDelay);
		base.info.caster.Animation.PlayAbilityAnimation(castingAnimClip);
		Entity target = base.info.target;
		for (int i = 0; i < shootCount; i++)
		{
			if (target.IsNullInactiveDeadOrKnockedOut())
			{
				target = Dew.GetClosestAliveHero(base.info.caster.agentPosition);
				if (target.IsNullInactiveDeadOrKnockedOut())
				{
					base.info.caster.Animation.PlayAbilityAnimation(endAnimClip);
					Destroy();
					yield break;
				}
			}
			Vector3 normalized = (target.position - base.info.caster.agentPosition).normalized;
			Vector3 vector = AbilityTrigger.PredictPoint_Simple((float)i / (float)shootCount + 0.1f, target, 1f) + Random.onUnitSphere.Flattened().normalized * (Random.value * maxDeviation);
			Vector3 vector2 = (vector - base.info.caster.position).Flattened();
			if (vector2.sqrMagnitude < minRangeFromSelf * minRangeFromSelf)
			{
				vector2 = vector2.normalized * minRangeFromSelf;
				vector = base.info.caster.position + vector2;
			}
			else if (vector2.sqrMagnitude >= maxRangeFromSelf * maxRangeFromSelf)
			{
				vector2 = vector2.normalized * Random.Range(maxRangeFromSelf / 1.5f, maxRangeFromSelf);
				vector = base.info.caster.position + vector2;
			}
			vector = Dew.GetPositionOnGround(vector);
			CreateAbilityInstance<Ai_Mon_Special_BossObliviax_TurretMode_Projectile>(base.info.caster.position, Quaternion.identity, new CastInfo(base.info.caster, vector));
			base.info.caster.Control.Rotate(normalized, immediately: false, interval);
			yield return new SI.WaitForSeconds(interval);
		}
		base.info.caster.Animation.PlayAbilityAnimation(endAnimClip);
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
