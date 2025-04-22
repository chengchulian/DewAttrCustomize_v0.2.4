public class Ai_Q_SuperNova_Activate : AbilityInstance
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
