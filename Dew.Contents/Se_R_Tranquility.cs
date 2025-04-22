using System.Collections;
using System.Collections.Generic;

public class Se_R_Tranquility : StatusEffect
{
	public DewAnimationClip endAnim;

	public float movementSpeedPercentage;

	public ScalingValue totalReducedSeconds;

	public int ticks;

	public float duration;

	private int _doneTicks;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			base.victim.Control.StartChannel(new Channel
			{
				blockedActions = (Channel.BlockedAction.Ability | Channel.BlockedAction.Attack),
				duration = duration
			});
			DoUncollidable();
			DoInvulnerable();
			DoStatBonus(new StatBonus
			{
				movementSpeedPercentage = movementSpeedPercentage
			});
			SetTimer(duration);
			ShowOnScreenTimer();
			for (int i = 0; i < ticks; i++)
			{
				DoTick();
				yield return new SI.WaitForSeconds(duration / (float)ticks);
			}
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			if (!base.victim.IsNullInactiveDeadOrKnockedOut() && endAnim != null)
			{
				base.victim.Animation.PlayAbilityAnimation(endAnim);
			}
			int num = ticks - _doneTicks;
			for (int i = 0; i < num; i++)
			{
				DoTick();
			}
		}
	}

	private void DoTick()
	{
		if (_doneTicks > ticks)
		{
			return;
		}
		_doneTicks++;
		if (base.victim == null || !base.victim.isActive || !(base.victim is Hero hero))
		{
			return;
		}
		AbilityTrigger abilityTrigger = base.firstTrigger;
		foreach (KeyValuePair<int, AbilityTrigger> ability in hero.Ability.abilities)
		{
			if (!(abilityTrigger == ability.Value) && ability.Value is SkillTrigger skill)
			{
				ReduceSkillCooldown(skill);
			}
		}
	}

	private void ReduceSkillCooldown(SkillTrigger skill)
	{
		if (!(skill == null) && skill.isActive)
		{
			skill.ApplyCooldownReduction(GetValue(totalReducedSeconds) / (float)ticks);
		}
	}

	private void MirrorProcessed()
	{
	}
}
