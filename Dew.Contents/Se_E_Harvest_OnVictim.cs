using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Se_E_Harvest_OnVictim : StatusEffect
{
	[NonSerialized]
	[SyncVar(hook = "OnEffectCountChanged")]
	public int effectCount;

	[NonSerialized]
	[SyncVar]
	public float totalStrength;

	public GameObject fxExplode;

	public GameObject fxHealSelf;

	public GameObject effectPrefab;

	public Transform effectParent;

	public ScalingValue baseDamage;

	public ScalingValue healOnKill;

	public float ampPerStrength;

	public float slowDuration;

	public float slowAmount;

	private List<GameObject> _effects;

	public int NetworkeffectCount
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
		[param: In]
		set
		{
		}
	}

	public float NetworktotalStrength
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
		[param: In]
		set
		{
		}
	}

	protected override void OnCreate()
	{
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	[Server]
	public void Explode(Actor parent)
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void OnEffectCountChanged(int prev, int newVal)
	{
	}

	private void Clear()
	{
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
	}
}
