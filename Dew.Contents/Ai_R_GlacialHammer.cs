using UnityEngine;

public class Ai_R_GlacialHammer : InstantDamageInstance
{
	public DewEase pullEase;

	public Vector2 pullDuration;

	public Vector2 pullGoalDistance;

	public float randomMagnitude;

	public float centerDamageAmp;

	public DewCollider centerRange;

	protected override void OnBeforeDispatchDamage(ref DamageData dmg, Entity target)
	{
		base.OnBeforeDispatchDamage(ref dmg, target);
		dmg.SetDirection(Vector3.up);
		if (centerRange.OverlapPoint(target.agentPosition.ToXY()))
		{
			dmg.ApplyAmplification(centerDamageAmp);
			dmg.SetAttr(DamageAttribute.IsCrit);
		}
	}

	protected override void OnHit(Entity entity)
	{
		base.OnHit(entity);
		float num = Random.Range(pullGoalDistance.x, pullGoalDistance.y);
		float duration = Random.Range(pullDuration.x, pullDuration.y);
		Vector3 vector;
		if (Vector2.Distance(base.info.caster.agentPosition.ToXY(), entity.agentPosition.ToXY()) < num)
		{
			vector = base.info.caster.agentPosition + Random.insideUnitSphere * randomMagnitude;
			vector = Dew.GetPositionOnGround(vector);
			vector = Dew.GetValidAgentDestination_LinearSweep(entity.agentPosition, vector);
		}
		else
		{
			Vector3 normalized = (entity.agentPosition - base.info.caster.agentPosition).normalized;
			vector = base.info.caster.agentPosition + normalized * num;
			vector = Dew.GetValidAgentDestination_Closest(base.info.caster.agentPosition, vector);
		}
		entity.Control.StartDisplacement(new DispByDestination
		{
			destination = vector,
			ease = pullEase,
			duration = duration,
			affectedByMovementSpeed = false,
			canGoOverTerrain = true,
			isCanceledByCC = false,
			isFriendly = false
		});
	}

	private void MirrorProcessed()
	{
	}
}
