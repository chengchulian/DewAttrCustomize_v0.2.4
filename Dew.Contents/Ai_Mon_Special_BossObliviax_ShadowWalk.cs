using System.Collections;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_ShadowWalk : AbilityInstance
{
	public float appearDistance;

	public float startTime;

	public float disappearTime;

	public float appearTime;

	public float postDelay;

	public GameObject fxDisappear;

	public GameObject fxAppear;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			postDelay = ((Random.value <= 0.8f) ? 0f : postDelay);
			float duration = startTime + disappearTime + appearTime + postDelay;
			base.info.caster.Control.StartChannel(new Channel
			{
				blockedActions = Channel.BlockedAction.Everything,
				duration = duration
			});
			Entity target = base.info.target;
			Vector3 agentPosition = target.agentPosition;
			Vector3 normalized = (agentPosition - AbilityTrigger.PredictPoint_Simple(1f, base.info.caster, disappearTime + appearTime)).normalized;
			Vector3 dest = Dew.GetValidAgentDestination_LinearSweep(end: agentPosition + normalized * appearDistance, start: base.info.caster.agentPosition);
			FxPlayNetworked(fxDisappear, base.info.caster);
			yield return new SI.WaitForSeconds(startTime);
			CreateStatusEffect(base.info.caster, delegate(Se_HunterBuff_ShadowWalk_Disappear s)
			{
				s.duration = disappearTime;
			});
			yield return null;
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				destination = dest,
				canGoOverTerrain = true,
				duration = disappearTime,
				ease = DewEase.Linear,
				isCanceledByCC = false,
				isFriendly = true,
				rotateForward = false
			});
			normalized = (target.agentPosition - dest).normalized;
			base.info.caster.Control.Rotate(normalized, immediately: true);
			yield return new SI.WaitForSeconds(disappearTime);
			FxPlayNetworked(fxAppear, base.info.caster);
			yield return new SI.WaitForSeconds(appearTime);
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
