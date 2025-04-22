[DewResourceLink(ResourceLinkBy.None)]
public class Trap_Lightning : Actor, IActivatableTrap, IBanRoomNodesNearby, IBanCampsNearby
{
	public void ActivateTrap()
	{
		CreateAbilityInstance<Ai_Trap_Lightning>(base.transform.position, null, default(CastInfo));
	}

	private void MirrorProcessed()
	{
	}
}
