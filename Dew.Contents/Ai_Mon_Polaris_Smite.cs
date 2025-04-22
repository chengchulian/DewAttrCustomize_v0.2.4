using UnityEngine;

public class Ai_Mon_Polaris_Smite : AbilityInstance
{
	public ScalingValue damage;

	public GameObject fxHit;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.info.target == null)
		{
			if (base.isServer)
			{
				Destroy();
			}
			return;
		}
		base.transform.position = base.info.target.position;
		if (base.isServer)
		{
			FxPlayNewNetworked(fxHit, base.info.target);
			CreateBasicEffect(base.info.target, new StunEffect(), 3f, "PolarisSmiteStun", DuplicateEffectBehavior.UsePrevious);
			float num = GetValue(damage);
			if (base.info.target is Monster)
			{
				num *= 7f;
			}
			DefaultDamage(num).SetElemental(ElementalType.Light).Dispatch(base.info.target);
		}
	}

	private void MirrorProcessed()
	{
	}
}
