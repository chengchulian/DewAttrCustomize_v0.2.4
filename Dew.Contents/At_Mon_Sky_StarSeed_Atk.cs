using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class At_Mon_Sky_StarSeed_Atk : AttackTrigger
{
	[NonSerialized]
	[SyncVar(hook = "OnIsMegaExplosionChanged")]
	public bool isMegaExplosion;

	public GameObject megaExplosionTelegraphObject;

	public bool NetworkisMegaExplosion
	{
		get
		{
			return isMegaExplosion;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref isMegaExplosion, 256uL, OnIsMegaExplosionChanged);
		}
	}

	private void OnIsMegaExplosionChanged(bool from, bool to)
	{
		megaExplosionTelegraphObject.SetActive(to);
		ListReturnHandle<DewAudioSource> handle;
		foreach (DewAudioSource item in configs[0].effectOnCast.GetComponentsInChildrenNonAlloc(out handle))
		{
			if (to)
			{
				item.pitchMultiplier *= 0.7f;
			}
			else
			{
				item.pitchMultiplier /= 0.7f;
			}
		}
		handle.Return();
	}

	public override void OnCastCompleteBeforePrepare(EventInfoCast cast)
	{
		base.OnCastCompleteBeforePrepare(cast);
		((Ai_Mon_Sky_StarSeed_Atk)cast.instance).NetworkisMegaExplosion = isMegaExplosion;
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
		if ((base.syncVarDirtyBits & 0x100L) != 0L)
		{
			writer.WriteBool(isMegaExplosion);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref isMegaExplosion, OnIsMegaExplosionChanged, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x100L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref isMegaExplosion, OnIsMegaExplosionChanged, reader.ReadBool());
		}
	}
}
