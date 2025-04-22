using System.Collections;
using UnityEngine;

public class Ai_Mon_SnowMountain_VikingWarrior_SpawnSword : AbilityInstance
{
	public float delay;

	public GameObject fxTelegraph;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			FxPlayNetworked(fxTelegraph, base.info.point, null);
			base.info.caster.Control.StartDaze(delay);
			yield return new SI.WaitForSeconds(delay);
			CreateAbilityInstance<Ai_Mon_SnowMountain_VikingWarrior_Sword>(base.info.point, null, base.info);
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
