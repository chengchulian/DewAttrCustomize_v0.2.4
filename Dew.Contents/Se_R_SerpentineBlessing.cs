public class Se_R_SerpentineBlessing : StatusEffect
{
	public ScalingValue armorAmount;

	public ScalingValue hasteAmount;

	public ScalingValue summonHealRatio;

	public float perTargetCooldown;

	public float duration;

	private EntityTransformModifier _mod;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	protected override void OnDestroy()
	{
	}

	private void MirrorProcessed()
	{
	}
}
