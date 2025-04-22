using UnityEngine;

public abstract class UI_InGame_Map_Item_NetBehaviour : UI_InGame_Map_Item
{
	public new DewNetworkBehaviour target => (DewNetworkBehaviour)base.target;

	public abstract bool IsSupported(DewNetworkBehaviour a);

	public override bool ShouldBeDestroyed()
	{
		if (!(target == null))
		{
			return !target.isClient;
		}
		return true;
	}

	public override Vector3 GetWorldPosition()
	{
		return target.transform.position;
	}
}
