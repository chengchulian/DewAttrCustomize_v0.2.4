using UnityEngine;

public class Se_L_Hysteria : StatusEffect
{
	public float duration;

	public float clawInterval;

	public bool isUnstoppable = true;

	private float _lastClawTime;

	private bool _isRight;

	private AbilityLockHandle _handle;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			if (isUnstoppable)
			{
				DoUnstoppable();
			}
			DoSpeed(-50f);
			SetTimer(duration);
			ShowOnScreenTimer();
			_handle = base.info.caster.Ability.GetNewAbilityLockHandle();
			_handle.LockAllAbilitiesCast();
			_handle.UnlockMovementSkillCast();
			_lastClawTime = Time.time;
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && _handle != null)
		{
			_handle.Stop();
			_handle = null;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer)
		{
			base.info.caster.Control.MoveToDestination(base.info.caster.owner.cursorWorldPos, immediately: false);
		}
		if (!base.isServer)
		{
			return;
		}
		if (base.victim.IsNullInactiveDeadOrKnockedOut() || NetworkedManagerBase<ZoneManager>.instance.isInAnyTransition)
		{
			Destroy();
		}
		else
		{
			if (base.victim.Control.isDisplacing)
			{
				return;
			}
			float num = clawInterval / Mathf.Max(base.victim.Status.attackSpeedMultiplier, 1f);
			if (Time.time - _lastClawTime > num)
			{
				_isRight = !_isRight;
				_lastClawTime = Time.time;
				float angle = CastInfo.GetAngle(base.victim.owner.cursorWorldPos - base.victim.position);
				CreateAbilityInstance(base.position, Quaternion.Euler(0f, angle, 0f), new CastInfo(base.victim, angle), delegate(Ai_L_Hysteria_Claw claw)
				{
					claw.isRight = _isRight;
				});
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
