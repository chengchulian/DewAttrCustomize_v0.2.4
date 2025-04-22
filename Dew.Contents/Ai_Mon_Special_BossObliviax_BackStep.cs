using System.Collections;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_BackStep : AbilityInstance
{
	public float startDelay;

	public float postDelay;

	public float moveDistance;

	public float moveDuration;

	public DewEase ease;

	public DewAnimationClip startAnimClip;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		CreateBasicEffect(base.info.caster, new UnstoppableEffect(), 10f, "ObliviaxUnstoppable").DestroyOnDestroy(this);
		DestroyOnDeath(base.info.caster);
		base.info.caster.Control.StartDaze(startDelay);
		base.info.caster.Control.RotateTowards(base.info.target.position, immediately: true);
		yield return new SI.WaitForSeconds(startDelay);
		if (Vector3.Distance(base.info.caster.position, base.info.target.position) < 8f)
		{
			base.info.caster.Animation.PlayAbilityAnimation(startAnimClip);
			Vector3 end = base.info.caster.agentPosition + (base.info.caster.agentPosition - base.info.target.agentPosition).normalized * moveDistance;
			end = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, end);
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				affectedByMovementSpeed = false,
				canGoOverTerrain = false,
				destination = end,
				duration = moveDuration,
				ease = ease,
				isCanceledByCC = false,
				isFriendly = true,
				rotateForward = false,
				onFinish = delegate
				{
					base.info.caster.Control.RotateTowards(base.info.target.position, immediately: true);
					CreateAbilityInstance<Ai_Mon_Special_BossObliviax_BackStep_Instance>(base.info.caster.position, null, base.info);
				}
			});
		}
		else
		{
			base.info.caster.Control.RotateTowards(base.info.target.position, immediately: true);
			CreateAbilityInstance<Ai_Mon_Special_BossObliviax_BackStep_Instance>(base.info.caster.position, null, base.info);
		}
		base.info.caster.Control.StartDaze(postDelay);
		yield return new SI.WaitForSeconds(postDelay);
		Destroy();
	}

	private void Rotate()
	{
		float initAngle = base.info.caster.Control.desiredAngle;
		float angle = 0f;
		float angularVelocity = 180f / moveDuration;
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			while (angle < 180f)
			{
				angle += angularVelocity * Time.deltaTime;
				base.info.caster.Control.Rotate(initAngle + angle, immediately: false);
				yield return null;
			}
			base.info.caster.Control.Rotate(initAngle + 180f, immediately: false);
		}
	}

	private void MirrorProcessed()
	{
	}
}
