using System;
using System.Collections;
using UnityEngine;

public class Mon_SnowMountain_LivingShards : Monster
{
	public RotateGameObject rot;

	public GameObject model;

	public float runDirRefreshTime = 1f;

	public Vector2 runDuration;

	public float maxDistance;

	private float _currentRunDuration;

	private float _runStartTime;

	private float _lastRunDirectionTime;

	private Vector3 _dir;

	private float _rotSpeed;

	private bool _isRunning;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			_rotSpeed = 15f;
			ActorEvent_OnAbilityInstanceCreated += new Action<EventInfoAbilityInstance>(OnAttackRoutine);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			ActorEvent_OnAbilityInstanceBeforePrepare -= new Action<EventInfoAbilityInstance>(OnAttackRoutine);
		}
	}

	private void LateUpdate()
	{
		model.transform.rotation = Quaternion.identity;
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (context.targetEnemy == null)
		{
			return;
		}
		if (!base.AI.Helper_CanBeCast<At_Mon_SnowMountain_LivingShards_Atk>() && !_isRunning)
		{
			StartRunning();
		}
		else if (Time.time - _runStartTime < _currentRunDuration)
		{
			if (context.targetEnemy == null || !context.targetEnemy.isActive || Vector2.Distance(context.targetEnemy.position.ToXY(), base.position.ToXY()) > maxDistance)
			{
				StopRunning();
			}
			else if (Time.time - _lastRunDirectionTime > runDirRefreshTime)
			{
				_lastRunDirectionTime = Time.time;
				_dir = GetRunFromTargetDestination(context.targetEnemy);
				base.Control.MoveToDestination(_dir, immediately: false);
			}
		}
		else
		{
			_isRunning = false;
			base.AI.Helper_ChaseTarget();
		}
	}

	private void OnAttackRoutine(EventInfoAbilityInstance info)
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			if (base.isServer && info.actor is At_Mon_SnowMountain_LivingShards_Atk)
			{
				rot.rot_speed_y *= _rotSpeed;
				yield return new WaitForSeconds(0.8f);
				rot.rot_speed_y /= _rotSpeed;
			}
		}
	}

	public void StartRunning()
	{
		_isRunning = true;
		_runStartTime = Time.time;
		_currentRunDuration = global::UnityEngine.Random.Range(runDuration.x, runDuration.y);
	}

	public void StopRunning()
	{
		_isRunning = false;
		_runStartTime = float.NegativeInfinity;
		if (base.AI.context.targetEnemy != null)
		{
			base.Control.Attack(base.AI.context.targetEnemy, doChase: true);
		}
	}

	private Vector3 GetRunFromTargetDestination(Entity target)
	{
		if (target == null)
		{
			Dew.SelectRandomAliveHero();
		}
		float num = 5f;
		Vector3 normalized = (base.agentPosition - target.agentPosition).normalized;
		Vector3 vector = Quaternion.AngleAxis(global::UnityEngine.Random.Range(-45f, 45f), Vector3.up) * normalized;
		Vector3 vector2 = base.agentPosition + vector * num;
		Vector3 validAgentDestination_LinearSweep = Dew.GetValidAgentDestination_LinearSweep(base.agentPosition, vector2);
		if (Vector3.SqrMagnitude(vector2) > Vector3.SqrMagnitude(validAgentDestination_LinearSweep))
		{
			validAgentDestination_LinearSweep = Dew.GetValidAgentDestination_LinearSweep(base.agentPosition, Vector3.Reflect(-validAgentDestination_LinearSweep, Vector3.forward));
		}
		return validAgentDestination_LinearSweep;
	}

	private void MirrorProcessed()
	{
	}
}
