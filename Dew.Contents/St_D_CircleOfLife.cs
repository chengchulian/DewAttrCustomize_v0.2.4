using Mirror;
using UnityEngine;

public class St_D_CircleOfLife : SkillTrigger
{
	public GameObject fxEffectOnSummon;

	public ScalingValue summonHealRatio;

	public ScalingValue summonAddedDuration;

	protected override void OnEquip(Entity newOwner)
	{
	}

	private void ClientActorEventOnCreate(Actor obj)
	{
	}

	private void EntityEventOnAttackEffectTriggered(EventInfoAttackEffect obj)
	{
	}

	protected override void OnUnequip(Entity formerOwner)
	{
	}

	[TargetRpc]
	private void TpcShowSummonDurationIncrease(NetworkConnectionToClient target, Summon s)
	{
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_TpcShowSummonDurationIncrease__NetworkConnectionToClient__Summon(NetworkConnectionToClient target, Summon s)
	{
	}

	protected static void InvokeUserCode_TpcShowSummonDurationIncrease__NetworkConnectionToClient__Summon(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
	}

	static St_D_CircleOfLife()
	{
	}
}
