using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Mon_Despair_UnstableRat_Explosion : InstantDamageInstance
{
	public bool disableRenderers;

	public Vector2 delayMultiplierRange;

	[SyncVar]
	private float _delayMultiplier;

	public float Network_delayMultiplier
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

	protected override void OnPrepare()
	{
	}

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
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
