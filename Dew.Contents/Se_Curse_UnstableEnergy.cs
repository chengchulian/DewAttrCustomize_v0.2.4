using UnityEngine;

public class Se_Curse_UnstableEnergy : CurseStatusEffect
{
	public float[] selfDamageHpRatio;

	public GameObject fxDamageSelf;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void ClientHeroEventOnSkillUse(EventInfoSkillUse obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
