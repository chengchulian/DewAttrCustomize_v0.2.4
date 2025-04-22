using UnityEngine;

public class Ai_Mon_SnowMountain_BossSkoll_Whirlwind : DamageInstance
{
	public DewAnimationClip animSpin;

	public float duration;

	public float selfSlowAmount;

	public float tickInterval;

	public float postDelay;

	public DewAnimationClip animSpinEnd;

	public float spinSpeed;

	private float _lastTickTime;

	private StatusEffect _slow;

	private EntityTransformModifier _mod;

	private float _currentAngle;

	protected override void OnCreate()
	{
		base.OnCreate();
		_mod = base.info.caster.Visual.GetNewTransformModifier();
		if (base.isServer)
		{
			base.transform.position = base.info.caster.position;
			CreateBasicEffect(base.info.caster, new UnstoppableEffect(), 60f).DestroyOnDestroy(this);
			_slow = CreateBasicEffect(base.info.caster, new SlowEffect
			{
				strength = selfSlowAmount
			}, duration, "whirlwind_slow");
			base.info.caster.Control.StartChannel(new Channel
			{
				duration = duration,
				blockedActions = (Channel.BlockedAction.Ability | Channel.BlockedAction.Attack),
				onCancel = base.DestroyIfActive,
				onComplete = base.DestroyIfActive
			});
			base.info.caster.Animation.PlayAbilityAnimation(animSpin);
			base.info.caster.Control.Rotate(Quaternion.identity, immediately: true, duration);
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		base.transform.position = base.info.caster.position;
		_currentAngle += spinSpeed * Time.deltaTime;
		_mod.rotation = Quaternion.Euler(0f, _currentAngle, 0f);
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && Time.time - _lastTickTime > tickInterval)
		{
			_lastTickTime = Time.time;
			DoCollisionChecks();
		}
		if (base.isServer)
		{
			Hero closestAliveHero = Dew.GetClosestAliveHero(base.info.caster.agentPosition);
			if (closestAliveHero != null)
			{
				base.info.caster.Control.MoveToDestination(closestAliveHero.agentPosition, immediately: false);
			}
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (_mod != null)
		{
			_mod.Stop();
			_mod = null;
		}
		if (!base.isServer)
		{
			return;
		}
		base.info.caster.Control.StopOverrideRotation();
		if (!_slow.IsNullOrInactive())
		{
			_slow.Destroy();
			_slow = null;
		}
		if (base.info.caster.isActive)
		{
			base.info.caster.Animation.PlayAbilityAnimation(animSpinEnd);
			base.info.caster.Control.StartDaze(postDelay);
			Hero closestAliveHero = Dew.GetClosestAliveHero(base.info.caster.agentPosition);
			if (closestAliveHero != null)
			{
				base.info.caster.Control.RotateTowards(closestAliveHero.agentPosition, immediately: true);
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
