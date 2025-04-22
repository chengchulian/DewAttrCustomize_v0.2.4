using System.Collections;
using DG.Tweening;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class St_L_ShoutOfOblivion : SkillTrigger
{
	private StatusEffect _invul;

	private CameraModifierOffset _offset;

	public override void OnCastStart(int configIndex, CastInfo info)
	{
		_invul = CreateBasicEffect(base.owner, new InvulnerableEffect(), 5f);
		RpcDoCameraOffset(info.forward);
		base.OnCastStart(configIndex, info);
	}

	protected override void OnCastCancel(int configIndex, CastInfo info)
	{
		if (!_invul.IsNullOrInactive())
		{
			_invul.Destroy();
			_invul = null;
		}
		RpcRemoveCameraOffset();
		base.OnCastCancel(configIndex, info);
	}

	public override AbilityInstance OnCastComplete(int configIndex, CastInfo info)
	{
		if (!_invul.IsNullOrInactive())
		{
			_invul.SetTimer(1f);
			_invul = null;
		}
		RpcRemoveCameraOffset();
		return base.OnCastComplete(configIndex, info);
	}

	[ClientRpc]
	private void RpcDoCameraOffset(Vector3 forward)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(forward);
		SendRPCInternal("System.Void St_L_ShoutOfOblivion::RpcDoCameraOffset(UnityEngine.Vector3)", -105298443, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcRemoveCameraOffset()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void St_L_ShoutOfOblivion::RpcRemoveCameraOffset()", -2070946915, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcDoCameraOffset__Vector3(Vector3 forward)
	{
		if (base.owner.owner.isLocalPlayer)
		{
			_offset = new CameraModifierOffset
			{
				offset = Vector3.zero
			}.Apply();
			CameraModifierOffset z = _offset;
			DOTween.Kill(z);
			DOTween.To(() => z.offset, delegate(Vector3 x)
			{
				z.offset = x;
			}, forward * 5f, 0.75f).SetId(z);
		}
	}

	protected static void InvokeUserCode_RpcDoCameraOffset__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcDoCameraOffset called on server.");
		}
		else
		{
			((St_L_ShoutOfOblivion)obj).UserCode_RpcDoCameraOffset__Vector3(reader.ReadVector3());
		}
	}

	protected void UserCode_RpcRemoveCameraOffset()
	{
		CameraModifierOffset z;
		if (_offset != null)
		{
			z = _offset;
			_offset = null;
			ManagerBase<CameraManager>.instance.StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			DOTween.Kill(z);
			DOTween.To(() => z.offset, delegate(Vector3 x)
			{
				z.offset = x;
			}, Vector3.zero, 0.5f).SetId(z);
			yield return new WaitForSeconds(0.5f);
			z.Remove();
		}
	}

	protected static void InvokeUserCode_RpcRemoveCameraOffset(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRemoveCameraOffset called on server.");
		}
		else
		{
			((St_L_ShoutOfOblivion)obj).UserCode_RpcRemoveCameraOffset();
		}
	}

	static St_L_ShoutOfOblivion()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(St_L_ShoutOfOblivion), "System.Void St_L_ShoutOfOblivion::RpcDoCameraOffset(UnityEngine.Vector3)", InvokeUserCode_RpcDoCameraOffset__Vector3);
		RemoteProcedureCalls.RegisterRpc(typeof(St_L_ShoutOfOblivion), "System.Void St_L_ShoutOfOblivion::RpcRemoveCameraOffset()", InvokeUserCode_RpcRemoveCameraOffset);
	}
}
