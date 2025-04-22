using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class SpawnGemStarProp : DewNetworkBehaviour, IInteractable, IActivatable
{
	public float effectDelay;

	public DewCollider range;

	public AbilityTargetValidator hittable;

	public Knockback knockback;

	[SyncVar]
	public bool isActivated;

	public GameObject fxActivate;

	private Room_Barrier _obstacle;

	int IInteractable.priority => 50;

	public bool canInteractWithMouse => false;

	public Transform interactPivot => base.transform;

	public float focusDistance => 3f;

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
		_obstacle = GetComponent<Room_Barrier>();
	}

	public bool CanInteract(Entity entity)
	{
		return !isActivated;
	}

	public void OnInteract(Entity entity, bool alt)
	{
		FxPlayNew(fxActivate);
		Vector3 pos;
		int quality;
		if (base.isServer)
		{
			NetworkisActivated = true;
			pos = Dew.GetPositionOnGround(base.transform.position);
			quality = NetworkedManagerBase<LootManager>.instance.GetLootInstance<Loot_Gem>().SelectQuality(Rarity.Rare) * 2;
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, hittable, entity);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity to = entities[i];
				knockback.ApplyWithOrigin(pos, to);
			}
			handle.Return();
			yield return new SI.WaitForSeconds(effectDelay);
			Dew.CreateGem<Gem_R_Celestial>(pos, quality, entity.owner);
			global::UnityEngine.Object.Destroy(base.gameObject);
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
