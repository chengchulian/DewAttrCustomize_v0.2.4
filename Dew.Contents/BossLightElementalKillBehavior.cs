using System.Collections;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class BossLightElementalKillBehavior : DewNetworkBehaviour
{
	public GameObject sunLight;

	public GameObject sfDecal;

	public GameObject lightDecal;

	public GameObject lightJail;

	public float fadeDuration;

	[ClientRpc]
	public void PlayBossKillBehaviorNetworked()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void BossLightElementalKillBehavior::PlayBossKillBehaviorNetworked()", -766955189, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private IEnumerator FadeAlpha(Material mat)
	{
		float startFactor = mat.GetFloat("_ColorFactor");
		float endFactor = 0.9f;
		float time = 0f;
		while (time < 3f)
		{
			float value = Mathf.Lerp(startFactor, endFactor, time / fadeDuration);
			mat.SetFloat("_ColorFactor", value);
			time += Time.deltaTime;
			yield return null;
		}
		mat.SetFloat("_ColorFactor", endFactor);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_PlayBossKillBehaviorNetworked()
	{
		sunLight.SetActive(value: true);
		Material lj = lightJail.GetComponent<MeshRenderer>().material;
		Material material = sfDecal.GetComponent<MeshRenderer>().material;
		Material material2 = lightDecal.GetComponent<MeshRenderer>().material;
		Material[] array = new Material[2] { material, material2 };
		for (int i = 0; i < array.Length; i++)
		{
			StartCoroutine(FadeAlpha(array[i]));
		}
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			Material mat = lj;
			Color startEmissionColor = mat.GetColor("_EmissionColor");
			Color endColor = new Color(0f, 0f, 0f);
			float time = 0f;
			while (time < fadeDuration)
			{
				Color value = Color.Lerp(startEmissionColor, endColor, time / fadeDuration);
				mat.SetColor("_EmissionColor", value);
				time += Time.deltaTime;
				yield return null;
			}
			mat.SetColor("_EmissionColor", endColor);
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
			((BossLightElementalKillBehavior)obj).UserCode_PlayBossKillBehaviorNetworked();
		}
	}

	static BossLightElementalKillBehavior()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(BossLightElementalKillBehavior), "System.Void BossLightElementalKillBehavior::PlayBossKillBehaviorNetworked()", InvokeUserCode_PlayBossKillBehaviorNetworked);
	}
}
