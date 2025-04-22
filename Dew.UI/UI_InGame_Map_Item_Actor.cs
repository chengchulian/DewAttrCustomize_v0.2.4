using UnityEngine;

public abstract class UI_InGame_Map_Item_Actor : UI_InGame_Map_Item
{
	public new Actor target => (Actor)base.target;

	public abstract bool IsSupported(Actor a);

	public override bool ShouldBeDestroyed()
	{
		return target.IsNullOrInactive();
	}

	public override Vector3 GetWorldPosition()
	{
		return target.position;
	}
}
