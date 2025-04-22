using UnityEngine;

public class RotateCircle : DewNetworkBehaviour
{
	public float angle;

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		base.transform.Rotate(Vector3.up, angle * Time.deltaTime, Space.World);
	}

	private void MirrorProcessed()
	{
	}
}
