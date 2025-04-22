using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Se_D_AstridsMasterpiece_Exposed : StatusEffect
{
	public int maxCharge = 3;

	public MeshRenderer chargeRenderer;

	public Material[] perChargeMaterials;

	[NonSerialized]
	[SyncVar]
	private int _chargeCount;

	[SyncVar]
	private float _chargeElapsedTime;

	public float rechargeTime => Mathf.Max(0.5f, 6f - 0.7f * (float)(base.skillLevel - 1));

	public int chargeCount
	{
		get
		{
			return _chargeCount;
		}
		set
		{
			Network_chargeCount = value;
		}
	}

	public int Network_chargeCount
	{
		get
		{
			return _chargeCount;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _chargeCount, 512uL, null);
		}
	}

	public float Network_chargeElapsedTime
	{
		get
		{
			return _chargeElapsedTime;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _chargeElapsedTime, 1024uL, null);
		}
	}

	protected override void OnPrepare()
	{
		base.OnPrepare();
		Network_chargeCount = maxCharge;
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		UpdateVisual();
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			DestroyOnDestroy(base.parentActor);
			base.victim.Visual.ClientEvent_OnRendererEnabledChanged += new Action<bool>(ClientEventOnRendererEnabledChanged);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.Visual.ClientEvent_OnRendererEnabledChanged -= new Action<bool>(ClientEventOnRendererEnabledChanged);
		}
	}

	private void ClientEventOnRendererEnabledChanged(bool obj)
	{
		UpdateVisual();
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		UpdateVisual();
		if (!base.isServer)
		{
			return;
		}
		if (chargeCount <= 0)
		{
			Network_chargeElapsedTime = _chargeElapsedTime + dt;
			if (_chargeElapsedTime > rechargeTime)
			{
				Network_chargeElapsedTime = 0f;
				chargeCount = maxCharge;
			}
		}
		if (base.info.caster == null || !base.info.caster.isActive || !base.info.caster.CheckEnemyOrNeutral(base.victim))
		{
			Destroy();
		}
	}

	private void UpdateVisual()
	{
		int num = chargeCount - 1;
		if (num >= 0 && num < perChargeMaterials.Length && !base.victim.Visual.isRendererOff)
		{
			chargeRenderer.enabled = true;
			chargeRenderer.sharedMaterial = perChargeMaterials[chargeCount - 1];
		}
		else
		{
			chargeRenderer.enabled = false;
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
			writer.WriteInt(_chargeCount);
			writer.WriteFloat(_chargeElapsedTime);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x200L) != 0L)
		{
			writer.WriteInt(_chargeCount);
		}
		if ((base.syncVarDirtyBits & 0x400L) != 0L)
		{
			writer.WriteFloat(_chargeElapsedTime);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _chargeCount, null, reader.ReadInt());
			GeneratedSyncVarDeserialize(ref _chargeElapsedTime, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x200L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _chargeCount, null, reader.ReadInt());
		}
		if ((num & 0x400L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _chargeElapsedTime, null, reader.ReadFloat());
		}
	}
}
