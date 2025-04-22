using UnityEngine;

public class Ai_R_Scattershot_Spawner : AbilityInstance
{
	public float maxAngle = 30f;

	public float frontDistance = 0.5f;

	public ScalingValue shootCount;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			int num = Mathf.RoundToInt(GetValue(shootCount));
			float a = 0f - maxAngle;
			float b = maxAngle;
			for (int i = 0; i < num; i++)
			{
				float num2 = Mathf.Lerp(a, b, (float)i / (float)(num - 1));
				CreateAbilityInstance<Ai_R_Scattershot_Projectile>(base.info.caster.Visual.GetCenterPosition() + base.info.forward * frontDistance, Quaternion.identity, new CastInfo(base.info.caster, base.info.angle + num2));
			}
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
