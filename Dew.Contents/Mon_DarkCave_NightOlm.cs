using Mirror;
using UnityEngine;

public class Mon_DarkCave_NightOlm : Monster
{
	public GameObject fxCamouflageOn;

	public GameObject fxCamouflageOff;

	public float forceAtkChance;

	public float desiredDistance;

	public float runDirRefreshTime;

	public Vector2 runDuration;

	public float maxDistance;

	public float baseOpacity;

	public float camouflageCooldown;

	private EntityColorModifier _entModifier;

	private float _camouflageOffTime;

	private bool _isCamouflageOn;

	private float _currentRunDuration;

	private float _runStartTime;

	private float _lastRunDirectionTime;

	private Vector3 _dir;

	private float _rotSpeed;

	private bool _isRunning;

	private float _baseWalkAnimSpeed;

	private float _baseWalkSpeed;

	private Entity _target;

	protected override void OnCreate()
	{
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
	}

	public void StartRunning()
	{
	}

	public void StopRunning()
	{
	}

	private Vector3 GetRunFromTargetDestination(Entity target)
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void OnAttackStart(EventInfoCast obj)
	{
	}

	private void OnTakeDamage(EventInfoDamage obj)
	{
	}

	[ClientRpc]
	private void ChangeEntityOpacityToOne(float speed)
	{
	}

	[ClientRpc]
	private void ChangeEntityOpacityToBase()
	{
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_ChangeEntityOpacityToOne__Single(float speed)
	{
	}

	protected static void InvokeUserCode_ChangeEntityOpacityToOne__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
	}

	protected void UserCode_ChangeEntityOpacityToBase()
	{
	}

	protected static void InvokeUserCode_ChangeEntityOpacityToBase(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
	}

	static Mon_DarkCave_NightOlm()
	{
	}
}
