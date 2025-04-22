using System.Collections.Generic;

public class St_D_WildEnergy : SkillTrigger
{
	private Dictionary<Entity, float> _lastApplyTime;

	public float perTargetCooldown
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void ClientEventOnRoomLoaded(EventInfoLoadRoom obj)
	{
	}

	protected override void OnEquip(Entity newOwner)
	{
	}

	private void ActorEventOnDoHeal(EventInfoHeal obj)
	{
	}

	private void ClientActorEventOnCreate(Actor obj)
	{
	}

	private void TryApplyCharge(Entity target)
	{
	}

	protected override void OnUnequip(Entity formerOwner)
	{
	}

	private void MirrorProcessed()
	{
	}
}
