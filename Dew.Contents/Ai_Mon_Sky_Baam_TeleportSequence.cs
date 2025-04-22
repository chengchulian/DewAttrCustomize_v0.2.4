using System.Collections;
using UnityEngine;

public class Ai_Mon_Sky_Baam_TeleportSequence : AbilityInstance
{
	public float postDelay;

	public DewAnimationClip startAnimation;

	public GameObject telegraph;

	public ChannelData channel;

	protected override IEnumerator OnCreateSequenced()
	{
		yield return base.OnCreateSequenced();
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		Ai_Mon_Sky_Baam_Teleport byType = DewResources.GetByType<Ai_Mon_Sky_Baam_Teleport>();
		Vector3 end = AbilityTrigger.PredictPoint_Simple(NetworkedManagerBase<GameManager>.instance.GetPredictionStrength(), base.info.target, channel.duration + byType.appearDuration);
		Vector3 dest = Dew.GetValidAgentDestination_Closest(base.info.caster.agentPosition, end);
		channel.Get().AddOnCancel(base.DestroyIfActive).AddOnComplete(delegate
		{
			CreateAbilityInstance(base.info.caster.agentPosition, base.info.caster.rotation, base.info, delegate(Ai_Mon_Sky_Baam_Teleport s)
			{
				s.targetPos = dest;
			});
			base.info.caster.Control.StartDaze(postDelay);
			base.info.caster.Animation.StopAbilityAnimation(startAnimation);
			DestroyIfActive();
		})
			.Dispatch(base.info.caster);
		base.info.caster.Animation.PlayAbilityAnimation(startAnimation);
		FxPlayNetworked(telegraph, end, null);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.info.caster.Status.currentHealth <= 0.1f || base.info.caster.IsNullInactiveDeadOrKnockedOut())
		{
			FxStop(telegraph);
		}
	}

	private void MirrorProcessed()
	{
	}
}
