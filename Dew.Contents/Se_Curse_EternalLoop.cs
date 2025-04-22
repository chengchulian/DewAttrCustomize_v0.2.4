public class Se_Curse_EternalLoop : CurseStatusEffect
{
	private Se_MirageSkin_Delusion_Delusional _delusional;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			_delusional = CreateStatusEffect(base.victim, delegate(Se_MirageSkin_Delusion_Delusional se)
			{
				se.isEternal = true;
			});
			_delusional.AddStack(9999);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !(base.victim == null) && !_delusional.IsNullOrInactive())
		{
			_delusional.Destroy();
			_delusional = null;
		}
	}

	private void MirrorProcessed()
	{
	}
}
