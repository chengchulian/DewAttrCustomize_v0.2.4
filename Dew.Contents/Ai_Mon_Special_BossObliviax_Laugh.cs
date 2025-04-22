using System.Collections;

public class Ai_Mon_Special_BossObliviax_Laugh : AbilityInstance
{
	public float duration;

	public DewAnimationClip clip;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			base.info.caster.Control.StartDaze(duration);
			base.info.caster.Animation.PlayAbilityAnimation(clip);
			yield return new SI.WaitForSeconds(duration);
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
