using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_QR_DistortedMind_Spawner : AbilityInstance
{
	public ScalingValue countRaw;

	public ChargingChannelData channel;

	public GameObject fxShoot;

	public GameObject fxChargeSingle;

	public ParticleSystem psUpArrows;

	[SyncVar]
	private float _chargeAmount;

	private ChargingChannel _currentChannel;

	private int _lastCount;

	public int clampedCount
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public float Network_chargeAmount
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

	private int GetCountFromNormalizedProgress(float value)
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	[ClientRpc]
	private void PlayShootEffect(int count)
	{
	}

	[Server]
	private void ShootAndDestroy()
	{
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_PlayShootEffect__Int32(int count)
	{
	}

	protected static void InvokeUserCode_PlayShootEffect__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
	}

	static Ai_QR_DistortedMind_Spawner()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
	}
}
