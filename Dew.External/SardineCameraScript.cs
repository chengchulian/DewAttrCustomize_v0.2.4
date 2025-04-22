using UnityEngine;

public class SardineCameraScript : MonoBehaviour
{
	public GameObject target;

	public float turnSpeed = 0.2f;

	private void FixedUpdate()
	{
		base.transform.position = Vector3.Lerp(base.transform.position, target.transform.position, Time.deltaTime);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, target.transform.rotation, Time.deltaTime * turnSpeed);
	}
}
