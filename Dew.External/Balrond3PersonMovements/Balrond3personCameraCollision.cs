using UnityEngine;

namespace Balrond3PersonMovements;

public class Balrond3personCameraCollision : MonoBehaviour
{
	private Vector3 dollyDir;

	private Vector3 dollyDirAdjusted;

	private Balrond3pCameraFollow follow;

	private Balrond3pMainCamera cam;

	private void Awake()
	{
		follow = base.transform.parent.parent.GetComponent<Balrond3pCameraFollow>();
		cam = base.transform.parent.GetComponent<Balrond3pMainCamera>();
		dollyDir = base.transform.parent.localPosition;
	}

	private void FixedUpdate()
	{
		Vector3 desiredCameraPos = base.transform.parent.TransformPoint(dollyDir * follow.maxDistance);
		if (Physics.Linecast(base.transform.parent.localPosition, desiredCameraPos, out var hit))
		{
			if (base.transform.localPosition.z <= follow.minDistance && !hit.transform.name.Equals(follow.target.transform.gameObject.name) && !hit.transform.gameObject.name.Equals(base.transform.gameObject.name) && !hit.transform.gameObject.name.Equals(cam.transform.gameObject.name))
			{
				base.transform.localPosition += new Vector3(0f, 0f, follow.smooth * Time.deltaTime);
			}
		}
		else if (0f - follow.maxDistance < base.transform.localPosition.z)
		{
			base.transform.localPosition -= new Vector3(0f, 0f, follow.smooth * Time.deltaTime);
		}
	}
}
