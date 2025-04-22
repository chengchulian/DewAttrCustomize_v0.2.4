using UnityEngine;

public class Ai_Mon_SnowMountain_VikingWarrior_Sword : InstantDamageInstance
{
	public GameObject fxMain;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.info.point != default(Vector3))
		{
			base.transform.position = base.info.point;
		}
		FxPlay(fxMain, base.info.point, null);
	}

	private void MirrorProcessed()
	{
	}
}
