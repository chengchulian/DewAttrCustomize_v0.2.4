using UnityEngine;

public class Ai_R_Chomp : AbilityInstance
{
	public float dashSpeed;

	public float dashGoalDist;

	public GameObject chompHitEffect;

	public ScalingValue damage;

	public ScalingValue healAmount;

	public float postDaze;

	public GameObject chompCasterEffect;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.info.caster.Control.StartDisplacement(new DispByTarget
			{
				affectedByMovementSpeed = false,
				cancelTime = 3f,
				goalDistance = dashGoalDist,
				isCanceledByCC = false,
				isFriendly = true,
				onCancel = base.Destroy,
				onFinish = OnFinish,
				rotateForward = true,
				speed = dashSpeed,
				target = base.info.target
			});
		}
	}

	private void OnFinish()
	{
		if (base.info.caster.IsNullInactiveDeadOrKnockedOut())
		{
			Destroy();
			return;
		}
		base.info.caster.Control.StartDaze(postDaze);
		FxPlayNetworked(chompHitEffect, base.info.target);
		FxPlayNetworked(chompCasterEffect, base.info.caster);
		Damage(damage).SetOriginPosition(base.info.caster.position).Dispatch(base.info.target);
		DoHeal(new HealData(GetValue(healAmount)), base.info.caster);
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
