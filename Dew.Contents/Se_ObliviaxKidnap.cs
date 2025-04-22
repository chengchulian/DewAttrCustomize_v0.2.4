using System.Collections;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Se_ObliviaxKidnap : StatusEffect
{
	public Transform tentacleTransform;

	public Transform tentacleTip;

	public AnimationCurve yOffsetCurve;

	public float duration;

	private EntityTransformModifier _modifier;

	private bool _didDisableRenderer;

	private bool _didDisableCharacterControls;

	public override bool isDestroyedOnRoomChange => true;

	protected override void OnCreate()
	{
		base.OnCreate();
		ManagerBase<MusicManager>.instance.Stop();
		tentacleTransform.localPosition = new Vector3(0f, yOffsetCurve.Evaluate(0f) - 5f, 0f);
		if (base.victim.isOwned)
		{
			ManagerBase<ControlManager>.instance.DisableCharacterControls();
			_didDisableCharacterControls = true;
		}
		if (base.isServer)
		{
			DoProtected(null);
			DoInvisible();
			DoUncollidable();
			DoRoot();
			DoStun();
			DoSilence();
			base.victim.Control.Stop();
			base.victim.Control.CancelOngoingChannels();
			RpcPlayAnimation();
			base.victim.Control.Rotate(ManagerBase<CameraManager>.instance.entityCamAngle + 180f + (float)Random.Range(-60, 60), immediately: true);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (_modifier != null)
		{
			_modifier.Stop();
			_modifier = null;
		}
		if (_didDisableRenderer)
		{
			base.victim.Visual.EnableRenderersLocal();
			_didDisableRenderer = false;
		}
		if (_didDisableCharacterControls && ManagerBase<ControlManager>.instance != null)
		{
			ManagerBase<ControlManager>.instance.EnableCharacterControls();
			_didDisableCharacterControls = false;
		}
		if (NetworkedManagerBase<ZoneManager>.instance != null && !NetworkedManagerBase<ZoneManager>.instance.isInAnyTransition && SingletonDewNetworkBehaviour<Room>.instance != null)
		{
			ManagerBase<MusicManager>.instance.Play(SingletonDewNetworkBehaviour<Room>.instance.music);
		}
	}

	[ClientRpc]
	private void RpcPlayAnimation()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void Se_ObliviaxKidnap::RpcPlayAnimation()", -1061791930, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void LateUpdate()
	{
		if (_modifier != null && tentacleTip != null)
		{
			_modifier.rotation = Quaternion.Inverse(base.victim.rotation) * tentacleTip.rotation;
			_modifier.worldOffset = tentacleTip.position - base.victim.agentPosition;
		}
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcPlayAnimation()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			Debug.Log("HELLO" + base.victim);
			_modifier = base.victim.Visual.GetNewTransformModifier();
			for (float t = 0f; t < duration; t += Time.deltaTime)
			{
				if (_modifier == null)
				{
					yield break;
				}
				tentacleTransform.localPosition = new Vector3(0f, yOffsetCurve.Evaluate(t / duration) - 5f, 0f);
				yield return null;
			}
			if (!_didDisableRenderer)
			{
				_didDisableRenderer = true;
				base.victim.Visual.DisableRenderersLocal();
			}
		}
	}

	protected static void InvokeUserCode_RpcPlayAnimation(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlayAnimation called on server.");
		}
		else
		{
			((Se_ObliviaxKidnap)obj).UserCode_RpcPlayAnimation();
		}
	}

	static Se_ObliviaxKidnap()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Se_ObliviaxKidnap), "System.Void Se_ObliviaxKidnap::RpcPlayAnimation()", InvokeUserCode_RpcPlayAnimation);
	}
}
