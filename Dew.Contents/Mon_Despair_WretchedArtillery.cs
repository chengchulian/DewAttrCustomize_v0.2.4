using UnityEngine;

public class Mon_Despair_WretchedArtillery : Monster
{
	public bool isUnstoppable;

	public Transform[] partTransforms;

	private Vector3[] _localScales;

	private void Start()
	{
	}

	public override void OnStartServer()
	{
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
	}

	private void LateUpdate()
	{
	}

	private void MirrorProcessed()
	{
	}
}
