using System.Runtime.InteropServices;
using Mirror;

public class St_L_CoinExplosion : SkillTrigger
{
	[SyncVar]
	public float damageMultiplier;

	public float NetworkdamageMultiplier
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
