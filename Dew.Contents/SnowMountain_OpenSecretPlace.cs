using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class SnowMountain_OpenSecretPlace : DewNetworkBehaviour
{
	private int DissolveStrength = Shader.PropertyToID("_DissolveStrength");

	public float delay = 2f;

	public float duration = 5f;

	public int triggerCnt = 2;

	public List<Transform> posList;

	public SnowMountain_SecretPlaceTriggerInstance trigger;

	public List<MeshRenderer> renderers;

	public Action OnTriggerActivated;

	private List<SnowMountain_SecretPlaceTriggerInstance> _triggerList;

	private List<Transform> _posList;

	private Room_Barrier _obstacle;

	private List<Material> _materials = new List<Material>();

	protected override void Awake()
	{
		base.Awake();
		_obstacle = GetComponent<Room_Barrier>();
		OnTriggerActivated = (Action)Delegate.Combine(OnTriggerActivated, new Action(CheckAllTriggers));
	}

	private void Start()
	{
		foreach (MeshRenderer renderer in renderers)
		{
			Material[] sharedMaterials = renderer.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				Material item = (sharedMaterials[i] = global::UnityEngine.Object.Instantiate(sharedMaterials[i]));
				_materials.Add(item);
			}
			renderer.sharedMaterials = sharedMaterials;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		foreach (Material material in _materials)
		{
			global::UnityEngine.Object.Destroy(material);
		}
		_materials.Clear();
	}

	public override void OnStartServer()
	{
		base.OnStartServer();
		_posList = new List<Transform>(posList);
		_triggerList = new List<SnowMountain_SecretPlaceTriggerInstance>();
		int num = 0;
		for (int i = 0; i < triggerCnt; i++)
		{
			num = global::UnityEngine.Random.Range(0, _posList.Count);
			SnowMountain_SecretPlaceTriggerInstance snowMountain_SecretPlaceTriggerInstance = global::UnityEngine.Object.Instantiate(trigger, _posList[num].position, _posList[num].rotation);
			NetworkServer.Spawn(snowMountain_SecretPlaceTriggerInstance.gameObject);
			snowMountain_SecretPlaceTriggerInstance.gate = this;
			_triggerList.Add(snowMountain_SecretPlaceTriggerInstance);
			_posList.RemoveAt(num);
		}
	}

	public void CheckAllTriggers()
	{
		int num = 0;
		foreach (SnowMountain_SecretPlaceTriggerInstance trigger in _triggerList)
		{
			if (!trigger.isActivated)
			{
				return;
			}
			num++;
		}
		if (num == triggerCnt)
		{
			RunOpenAnimation();
			OpenSecretPlace();
		}
	}

	private void OpenSecretPlace()
	{
		_obstacle.Open();
	}

	[ClientRpc]
	private void RunOpenAnimation()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void SnowMountain_OpenSecretPlace::RunOpenAnimation()", 344487378, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private IEnumerator RoutineAnimateDissolve(float delay, float duration)
	{
		float factor = 1f / duration;
		yield return new WaitForSeconds(delay);
		for (float v = 0f; v <= 1f; v += Time.deltaTime * factor)
		{
			SetShaderPropertyLocal(DissolveStrength, v);
			yield return null;
		}
		SetShaderPropertyLocal(DissolveStrength, 1f);
	}

	private void SetShaderPropertyLocal(int propertyId, float value)
	{
		for (int i = 0; i < _materials.Count; i++)
		{
			_materials[i].SetFloat(propertyId, value);
		}
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RunOpenAnimation()
	{
		StartCoroutine(RoutineAnimateDissolve(delay, duration));
	}

	protected static void InvokeUserCode_RunOpenAnimation(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RunOpenAnimation called on server.");
		}
		else
		{
			((SnowMountain_OpenSecretPlace)obj).UserCode_RunOpenAnimation();
		}
	}

	static SnowMountain_OpenSecretPlace()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(SnowMountain_OpenSecretPlace), "System.Void SnowMountain_OpenSecretPlace::RunOpenAnimation()", InvokeUserCode_RunOpenAnimation);
	}
}
