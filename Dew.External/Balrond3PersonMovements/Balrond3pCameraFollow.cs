using UnityEngine;

namespace Balrond3PersonMovements;

public class Balrond3pCameraFollow : MonoBehaviour
{
	[Header("Target to follow")]
	public Transform target;

	[Header("Target's height")]
	public float setTargetHeight;

	[Header("Distance")]
	public float maxDistance = 2f;

	public float minDistance = 1f;

	[Header("Zoom speed")]
	public float smooth = 10f;

	private void Start()
	{
		base.transform.position = target.position;
	}

	private void Update()
	{
		base.transform.position = target.position;
		base.transform.position += new Vector3(0f, setTargetHeight, 0f);
	}
}
