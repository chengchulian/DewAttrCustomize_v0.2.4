using System.Collections;
using UnityEngine;

public class Ai_Mon_SnowMountain_IceElemental_IceBreaker : InstantDamageInstance
{
	public float knockupStrength;

	public int subExplosionCount;

	public float startSubExplosionDistance;

	public float perExplosionDistance;

	public float perExplosionInterval;

	public float initDelay;

	protected override void OnHit(Entity entity)
	{
		entity.Visual.KnockUp(knockupStrength, isFriendly: false);
		base.OnHit(entity);
	}

	protected override IEnumerator OnCreateSequenced()
	{
		yield return base.OnCreateSequenced();
		if (!base.isServer)
		{
			yield break;
		}
		Vector3 pullDir = -base.info.forward.Flattened().normalized;
		yield return new SI.WaitForSeconds(initDelay);
		for (int i = 0; i < subExplosionCount; i++)
		{
			Vector3 positionOnGround = Dew.GetPositionOnGround(base.transform.position + base.info.forward * (startSubExplosionDistance + perExplosionDistance * (float)i));
			CreateAbilityInstance(positionOnGround, null, new CastInfo(base.info.caster), delegate(Ai_Mon_SnowMountain_IceElemental_IceBreaker_SubExplosion s)
			{
				s.pullDirection = pullDir;
			});
			yield return new SI.WaitForSeconds(perExplosionInterval);
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
