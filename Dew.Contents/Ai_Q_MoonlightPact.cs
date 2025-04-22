using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Q_MoonlightPact : AbilityInstance
{
	public DewAnimationClip prepareAnim;

	public DewAnimationClip startAnim;

	public DewAnimationClip endAnim;

	public AnimationCurve yOffsetCurve;

	private EntityTransformModifier _mod;

	private Sum_Q_MoonlightPact_Fenrir _fenrir;

	[SyncVar]
	private float _duration;

	public float Network_duration
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

	protected override void ActiveFrameUpdate()
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
