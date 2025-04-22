using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Trap_UnstableMushroom : InstantDamageInstance
{
	[SyncVar]
	public Color mainColor;

	public float slowAmount;

	public float slowDuration;

	public Color NetworkmainColor
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

	public override void OnStartClient()
	{
	}

	protected override void OnHit(Entity entity)
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
