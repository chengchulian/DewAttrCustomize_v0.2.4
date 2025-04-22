using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Sum_Q_SylvanCall_LeafHound : Summon
{
	public GameObject leafPuppyHatObject;

	[SyncVar]
	private bool _hasLeafPuppyHat;

	public bool Network_hasLeafPuppyHat
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
