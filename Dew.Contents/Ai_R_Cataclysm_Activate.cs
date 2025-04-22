public class Ai_R_Cataclysm_Activate : AbilityInstance
{
	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
