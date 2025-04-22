public class RoomMod_AcceleratedTime : RoomModifierBase
{
	public override void OnStartServer()
	{
		base.OnStartServer();
		ModifyEntities(delegate(Entity e)
		{
			if (e is Hero || e is Monster)
			{
				e.CreateStatusEffect<Se_AcceleratedTime>(e, new CastInfo(e));
			}
		}, delegate(Entity e)
		{
			if (e.Status.TryGetStatusEffect<Se_AcceleratedTime>(out var effect))
			{
				effect.Destroy();
			}
		});
	}

	private void MirrorProcessed()
	{
	}
}
