using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ai_Mon_Sky_BossNyx_Atk : AbilityInstance
{
	public GameObject fxExplode;

	public ChannelData channel;

	public float maxRange;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		List<Entity> list = new List<Entity>();
		list.Add(base.info.target);
		foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
		{
			if (!humanPlayer.hero.IsNullInactiveDeadOrKnockedOut() && !list.Contains(humanPlayer.hero) && Vector2.Distance(humanPlayer.hero.position.ToXY(), base.info.caster.position.ToXY()) < maxRange)
			{
				list.Add(humanPlayer.hero);
			}
		}
		float initDelay = DewResources.GetByType<Ai_Mon_Sky_BossNyx_PillarOfStars>().initDelay;
		foreach (Entity item in list)
		{
			Vector3 point = AbilityTrigger.PredictPoint_Simple(NetworkedManagerBase<GameManager>.instance.GetPredictionStrength(), item, initDelay);
			CreateAbilityInstance<Ai_Mon_Sky_BossNyx_PillarOfStars>(point, null, new CastInfo(base.info.caster, point));
		}
		Channel obj = channel.Get();
		obj.duration = initDelay;
		obj.onCancel = (Action)Delegate.Combine(obj.onCancel, (Action)delegate
		{
			Actor[] array = base.children.ToArray();
			foreach (Actor actor in array)
			{
				if (actor is Ai_Mon_Sky_BossNyx_PillarOfStars)
				{
					actor.DestroyIfActive();
				}
			}
			DestroyIfActive();
			if (base.firstTrigger != null)
			{
				base.firstTrigger.SetCooldownTime(0, 1f);
			}
		});
		obj.onComplete = (Action)Delegate.Combine(obj.onComplete, (Action)delegate
		{
			DewEffect.Play(fxExplode);
			DestroyIfActive();
		});
		obj.Dispatch(base.info.caster);
	}

	private void MirrorProcessed()
	{
	}
}
