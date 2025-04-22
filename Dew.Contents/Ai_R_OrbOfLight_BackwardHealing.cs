using System;
using System.Runtime.InteropServices;
using Mirror;

public class Ai_R_OrbOfLight_BackwardHealing : StandardProjectile
{
	public ScalingValue healAmountPerEnemy;

	[NonSerialized]
	[SyncVar]
	public int hitEnemies;

	public int NetworkhitEnemies
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

	protected override void OnEntity(EntityHit hit)
	{
	}

	protected override void OnComplete()
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
