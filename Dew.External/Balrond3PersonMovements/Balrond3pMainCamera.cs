using UnityEngine;

namespace Balrond3PersonMovements;

public class Balrond3pMainCamera : MonoBehaviour
{
	private Transform target;

	private float rotationSmoothing = 0.1f;

	private float distanceToTarget;

	private float velocityX;

	private float velocityY;

	private float rotationYAxis;

	private float rotationXAxis;

	public float zoomSpeed = 0.5f;

	public float distance;

	public float minDistance;

	public float maxDistance = 7f;

	private Balrond3pCameraFollow follow;

	private void Start()
	{
		follow = base.transform.parent.GetComponent<Balrond3pCameraFollow>();
		setBasePosition();
	}

	private void FixedUpdate()
	{
		rotation();
	}

	private void setBasePosition()
	{
		target = base.transform.parent.transform;
		distanceToTarget = Vector3.Distance(target.position, base.transform.position) + follow.maxDistance;
		Vector3 angles = base.transform.eulerAngles;
		rotationYAxis = angles.y;
		rotationXAxis = angles.x;
		if ((bool)GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
	}

	private void rotation()
	{
		if ((bool)target)
		{
			if (Input.GetMouseButton(0))
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				velocityX += rotationSmoothing * 150f * Input.GetAxis("Mouse X") * distanceToTarget * 10f * 0.02f;
				velocityY += rotationSmoothing * 150f * Input.GetAxis("Mouse Y") * 0.02f;
			}
			else
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			rotationYAxis += velocityX;
			rotationXAxis -= velocityY;
			rotationXAxis = ClampAngle(rotationXAxis, -90f, 90f);
			Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, 0f);
			Quaternion rotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0f);
			Vector3 negDistance = new Vector3(0f, 0f, 0f - distanceToTarget);
			Vector3 position = rotation * negDistance + target.position;
			base.transform.rotation = rotation;
			base.transform.position = position;
			velocityX = Mathf.Lerp(velocityX, 0f, rotationSmoothing * 20f);
			velocityY = Mathf.Lerp(velocityY, 0f, rotationSmoothing * 20f);
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
