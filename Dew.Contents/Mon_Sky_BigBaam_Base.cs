using UnityEngine;

public class Mon_Sky_BigBaam_Base : Monster
{
	public float fleeChance;

	public float fleeStartDistance;

	public float fleeDisableAITime = 4f;

	public Vector2 fleeDistance;

	public float fleeCooldownTime;

	public float fleeLookDuration;

	public float fleeAngleDeviation;

	public int fleeDestinationSteps;

	public bool activelyChase;

	private float _lastFleeTime;

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (!(context.targetEnemy == null) && !MainSkillRoutine() && !(Time.time - _lastFleeTime < fleeDisableAITime))
		{
			if (Time.time > _lastFleeTime + fleeCooldownTime && Random.value < fleeChance * context.deltaTime && Vector2.Distance(context.targetEnemy.agentPosition.ToXY(), base.agentPosition.ToXY()) < fleeStartDistance)
			{
				_lastFleeTime = Time.time;
				Vector3 fleeDestination = GetFleeDestination();
				base.Control.MoveToDestination(fleeDestination, immediately: true);
				base.Control.RotateTowards(context.targetEnemy, immediately: false, fleeLookDuration);
			}
			else if (base.AI.Helper_CanBeCast<At_Mon_Sky_BigBaam_Melee>() && base.AI.Helper_IsTargetInRange<At_Mon_Sky_BigBaam_Melee>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Sky_BigBaam_Melee>();
			}
			else if (activelyChase || base.AI.Helper_IsTargetInRangeOfAttack())
			{
				base.AI.Helper_ChaseTarget();
			}
		}
	}

	protected virtual Vector3 GetFleeDestination()
	{
		Vector3 vector = base.AI.context.targetEnemy.agentPosition;
		Vector3 vector2 = base.agentPosition;
		float num = float.NegativeInfinity;
		Vector3 result = base.agentPosition;
		float num2 = Random.Range(fleeDistance.x, fleeDistance.y);
		for (int i = 0; i < fleeDestinationSteps; i++)
		{
			float y = Mathf.Lerp(0f - fleeAngleDeviation, fleeAngleDeviation, (float)i / (float)(fleeDestinationSteps - 1));
			Vector3 end = vector2 + Quaternion.Euler(0f, y, 0f) * (vector2 - vector).normalized * num2;
			end = Dew.GetValidAgentDestination_Closest(vector2, end);
			float num3 = Vector2.Distance(vector2.ToXY(), end.ToXY());
			float num4 = 100f - Mathf.Abs(num3 - num2) + Random.value;
			if (!(num4 <= num))
			{
				num = num4;
				result = end;
			}
		}
		return result;
	}

	protected virtual bool MainSkillRoutine()
	{
		return false;
	}

	private void MirrorProcessed()
	{
	}
}
