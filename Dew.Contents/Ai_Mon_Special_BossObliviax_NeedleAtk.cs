using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_NeedleAtk : AbilityInstance
{
	public float postDelay;

	public float startDelay;

	public GameObject fxTelegraph;

	public GameObject fxExplode;

	public DewAnimationClip startAnim;

	public DewAnimationClip endAnim;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		FxPlayNetworked(fxTelegraph, base.info.caster);
		base.info.caster.Animation.PlayAbilityAnimation(startAnim);
		base.info.caster.Control.StartDaze(startDelay);
		yield return new SI.WaitForSeconds(startDelay);
		FxPlayNetworked(fxExplode, base.info.caster);
		CreateAbilityInstance<Ai_Mon_Special_BossObliviax_NeedleAtk_AtkInstance>(base.info.caster.agentPosition, null, new CastInfo(base.info.caster));
		base.info.caster.Animation.StopAbilityAnimation(startAnim);
		base.info.caster.Animation.PlayAbilityAnimation(endAnim);
		base.info.caster.Control.StartDaze(postDelay);
		foreach (KeyValuePair<int, AbilityTrigger> ability in base.info.caster.Ability.abilities)
		{
			if (!(ability.Value is At_Mon_Special_BossObliviax_NeedleAtk) && !(ability.Value is At_Mon_Special_BossLightElemental_BeamAtk))
			{
				ability.Value.ResetCooldown();
			}
		}
		yield return new SI.WaitForSeconds(postDelay);
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
