using UnityEngine;

public class Ai_Mon_SnowMountain_BossSkoll_Sword : InstantDamageInstance
{
	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.info.point != default(Vector3))
		{
			base.transform.position = base.info.point;
		}
	}

	private void MirrorProcessed()
	{
	}
}
