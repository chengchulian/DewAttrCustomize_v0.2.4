using DG.Tweening;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class BossSeekerKillBehavior : DewNetworkBehaviour
{
	public GameObject[] lightsGroup;

	public GameObject[] particles;

	public DewChildrenCullingGroup cullingGroup;

	public Light sun;

	public GameObject sunLightParticle;

	public GameObject sunLight;

	[ClientRpc]
	public void PlayBossKillBehaviorNetworked()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void BossSeekerKillBehavior::PlayBossKillBehaviorNetworked()", -1317798057, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_PlayBossKillBehaviorNetworked()
	{
		GameObject[] array = particles;
		foreach (GameObject effect in array)
		{
			FxStop(effect);
		}
		cullingGroup.enabled = false;
		sun.DOIntensity(2f, 1f);
		sunLight.gameObject.SetActive(value: true);
		sunLight.GetComponent<FxPointLight>().Play();
		FxPlay(sunLightParticle);
		array = lightsGroup;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(value: false);
		}
	}

	protected static void InvokeUserCode_PlayBossKillBehaviorNetworked(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC PlayBossKillBehaviorNetworked called on server.");
		}
		else
		{
			((BossSeekerKillBehavior)obj).UserCode_PlayBossKillBehaviorNetworked();
		}
	}

	static BossSeekerKillBehavior()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(BossSeekerKillBehavior), "System.Void BossSeekerKillBehavior::PlayBossKillBehaviorNetworked()", InvokeUserCode_PlayBossKillBehaviorNetworked);
	}
}
