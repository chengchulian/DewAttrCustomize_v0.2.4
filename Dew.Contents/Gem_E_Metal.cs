using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Gem_E_Metal : Gem
{
	public ScalingValue cooldownReduction;

	public GameObject reducedEffect;

	private KillTracker _tracker;

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			_tracker = newOwner.TrackKills(10f, KillCallback);
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer)
		{
			_tracker?.Stop();
		}
	}

	private void KillCallback(EventInfoKill obj)
	{
		if (!(base.skill == null))
		{
			float value = GetValue(cooldownReduction);
			base.skill.ApplyCooldownReduction(value);
			RpcPlayFeedback();
			NotifyUse();
		}
	}

	[ClientRpc]
	private void RpcPlayFeedback()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void Gem_E_Metal::RpcPlayFeedback()", -1327080922, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcPlayFeedback()
	{
		FxPlay(reducedEffect, base.owner);
	}

	protected static void InvokeUserCode_RpcPlayFeedback(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlayFeedback called on server.");
		}
		else
		{
			((Gem_E_Metal)obj).UserCode_RpcPlayFeedback();
		}
	}

	static Gem_E_Metal()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Gem_E_Metal), "System.Void Gem_E_Metal::RpcPlayFeedback()", InvokeUserCode_RpcPlayFeedback);
	}
}
