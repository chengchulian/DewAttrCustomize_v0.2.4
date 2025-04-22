using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Mon_Sky_BossNyx_Swipe : InstantDamageInstance
{
	public DewCollider improvedRange;

	public GameObject improvedStartEffect;

	[NonSerialized]
	[SyncVar]
	public bool enableImprovedSwipe;

	public bool NetworkenableImprovedSwipe
	{
		get
		{
			return enableImprovedSwipe;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref enableImprovedSwipe, 64uL, null);
		}
	}

	protected override void OnPrepare()
	{
		base.OnPrepare();
		if (enableImprovedSwipe)
		{
			range = improvedRange;
		}
	}

	protected override void OnCreate()
	{
		if (enableImprovedSwipe)
		{
			startEffect = improvedStartEffect;
		}
		if (SingletonBehaviour<Sky_BossRoomCenter>.instance != null)
		{
			float num = Dew.GetPositionOnGround(SingletonBehaviour<Sky_BossRoomCenter>.instance.transform.position).y + 0.15f;
			if (base.transform.position.y < num)
			{
				base.transform.position = base.transform.position.WithY(num);
			}
		}
		base.OnCreate();
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(enableImprovedSwipe);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x40L) != 0L)
		{
			writer.WriteBool(enableImprovedSwipe);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref enableImprovedSwipe, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x40L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref enableImprovedSwipe, null, reader.ReadBool());
		}
	}
}
