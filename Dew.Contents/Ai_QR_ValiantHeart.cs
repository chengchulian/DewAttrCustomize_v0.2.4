using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_QR_ValiantHeart : InstantDamageInstance
{
	[SyncVar]
	private Quaternion _rotation;

	private bool _didHit;

	public Quaternion Network_rotation
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

	protected override void OnHit(Entity entity)
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
