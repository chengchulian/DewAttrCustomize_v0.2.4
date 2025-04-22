using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Se_R_NightSky_AtkSpd : StatusEffect
{
	public AnimationCurve scaleCurve;

	public float scaleSpeed;

	[SyncVar]
	internal float _effectStrength;

	public HasteEffect haste { get; private set; }

	public float Network_effectStrength
	{
		get
		{
			return _effectStrength;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _effectStrength, 512uL, null);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			haste = DoHaste(0f);
			startEffectVictim.transform.localScale = Vector3.zero;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		float x = startEffectVictim.transform.localScale.x;
		float target = scaleCurve.Evaluate(_effectStrength);
		startEffectVictim.transform.localScale = Vector3.one * Mathf.MoveTowards(x, target, scaleSpeed * dt);
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(_effectStrength);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x200L) != 0L)
		{
			writer.WriteFloat(_effectStrength);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _effectStrength, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x200L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _effectStrength, null, reader.ReadFloat());
		}
	}
}
