using UnityEngine;

[DewResourceLink(ResourceLinkBy.None)]
public class Trap_ArrowShooter : Actor, IActivatableTrap, IBanRoomNodesNearby, IBanCampsNearby
{
	public void ActivateTrap()
	{
		CreateAbilityInstance<Ai_Trap_ArrowShooter_Arrow>(base.transform.position, Quaternion.identity, new CastInfo(null, CastInfo.GetAngle(base.transform.rotation)));
	}

	private void MirrorProcessed()
	{
	}
}
