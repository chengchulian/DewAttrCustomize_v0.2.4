using System;

public class St_C_Starfall : SkillTrigger
{
	public AbilityTargetValidator hittable;

	public float radius;

	public override bool CanBeCast()
	{
		if (base.CanBeCast())
		{
			return Check();
		}
		return false;
	}

	public override bool CanBeReserved()
	{
		if (base.CanBeReserved())
		{
			return Check();
		}
		return false;
	}

	private bool Check()
	{
		if (base.owner == null)
		{
			return false;
		}
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.owner.agentPosition, radius, hittable, base.owner);
		handle.Return();
		if (readOnlySpan.Length == 0)
		{
			return false;
		}
		return true;
	}

	private void MirrorProcessed()
	{
	}
}
