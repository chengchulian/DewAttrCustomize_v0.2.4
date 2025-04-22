using System;
using System.Runtime.InteropServices;
using Mirror;

public class Ai_Mon_Sky_StarSeed_Atk : InstantDamageInstance
{
	[NonSerialized]
	[SyncVar]
	public bool isMegaExplosion;

	public bool NetworkisMegaExplosion
	{
		get
		{
			return isMegaExplosion;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref isMegaExplosion, 64uL, null);
		}
	}

	protected override void OnCreate()
	{
		if (isMegaExplosion)
		{
			range.transform.localScale *= 1.5f;
			startEffectNoStop.transform.localScale *= 1.5f;
			ListReturnHandle<DewAudioSource> handle;
			foreach (DewAudioSource item in startEffectNoStop.GetComponentsInChildrenNonAlloc(out handle))
			{
				item.pitchMultiplier *= 0.7f;
			}
			handle.Return();
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
			writer.WriteBool(isMegaExplosion);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x40L) != 0L)
		{
			writer.WriteBool(isMegaExplosion);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref isMegaExplosion, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x40L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref isMegaExplosion, null, reader.ReadBool());
		}
	}
}
