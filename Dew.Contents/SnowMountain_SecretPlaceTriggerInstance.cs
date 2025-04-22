using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class SnowMountain_SecretPlaceTriggerInstance : DewNetworkBehaviour, IInteractable, IActivatable
{
	[SyncVar]
	public bool isActivated;

	public SnowMountain_OpenSecretPlace gate;

	public GameObject activateSound;

	public float intensityAmp;

	private Material _mat;

	private float _emissionIntensity;

	private Color _emissionColor;

	int IInteractable.priority => 50;

	public Transform interactPivot => base.transform;

	public bool canInteractWithMouse => false;

	public float focusDistance => 2.5f;

	public bool NetworkisActivated
	{
		get
		{
			return isActivated;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref isActivated, 1uL, null);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_emissionColor = new Color(77f, 134f, 191f);
		_mat = GetComponent<MeshRenderer>().material;
		_mat.SetColor("_EmissionColor", _emissionColor * _emissionIntensity);
	}

	public bool CanInteract(Entity entity)
	{
		return !isActivated;
	}

	public void OnInteract(Entity entity, bool alt)
	{
		FxPlay(activateSound);
		StartCoroutine(OnInteractRoutine());
		if (base.isServer)
		{
			NetworkisActivated = true;
			gate.OnTriggerActivated?.Invoke();
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Object.Destroy(_mat);
	}

	private IEnumerator OnInteractRoutine()
	{
		while ((double)_emissionIntensity < 0.02)
		{
			_emissionIntensity += intensityAmp / 10000f;
			_mat.SetColor("_EmissionColor", _emissionColor * _emissionIntensity);
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(isActivated);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteBool(isActivated);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref isActivated, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref isActivated, null, reader.ReadBool());
		}
	}
}
