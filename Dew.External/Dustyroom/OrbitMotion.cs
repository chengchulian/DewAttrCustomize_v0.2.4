using UnityEngine;

namespace Dustyroom;

public class OrbitMotion : MonoBehaviour
{
	public enum TargetMode
	{
		Transform,
		Position
	}

	public TargetMode targetMode = TargetMode.Position;

	public Transform targetTransform;

	public bool followTargetTransform = true;

	public Vector3 targetOffset = Vector3.zero;

	public Vector3 targetPosition;

	[Space]
	public float distanceHorizontal = 60f;

	public float distanceVertical = 60f;

	public float xSpeed = 120f;

	public float ySpeed = 120f;

	public float damping = 3f;

	[Space]
	public bool clampAngle;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	[Space]
	public bool allowZoom;

	public float distanceMin = 0.5f;

	public float distanceMax = 15f;

	private float _x;

	private float _y;

	[Space]
	public bool autoMovement;

	public float autoSpeedX = 0.2f;

	public float autoSpeedY = 0.1f;

	public float autoSpeedDistance = -0.1f;

	[Space]
	public bool interactive = true;

	private float _lastMoveTime;

	[HideInInspector]
	public float timeSinceLastMove;

	private void Start()
	{
		Vector3 angles = base.transform.eulerAngles;
		_x = angles.y;
		_y = angles.x;
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		if (rigidbody != null)
		{
			rigidbody.freezeRotation = true;
		}
		if (targetMode == TargetMode.Transform)
		{
			if (targetTransform != null)
			{
				targetPosition = targetTransform.position + targetOffset;
			}
			else
			{
				Debug.LogWarning("Reference transform is not set.");
			}
		}
	}

	private void Update()
	{
		if (targetMode == TargetMode.Transform && followTargetTransform)
		{
			if (targetTransform != null)
			{
				targetPosition = targetTransform.position + targetOffset;
			}
			else
			{
				Debug.LogWarning("Reference transform is not set.");
			}
		}
		if (Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y")) > 0f)
		{
			_lastMoveTime = Time.time;
		}
		timeSinceLastMove = Time.time - _lastMoveTime;
		if (interactive && Input.GetMouseButton(0))
		{
			_x += Input.GetAxis("Mouse X") * xSpeed * 40f * 0.02f;
			_y -= Input.GetAxis("Mouse Y") * ySpeed * 40f * 0.02f;
		}
		else if (autoMovement)
		{
			_x += autoSpeedX * 40f * Time.deltaTime * 10f;
			_y -= autoSpeedY * 40f * Time.deltaTime * 10f;
			distanceHorizontal += autoSpeedDistance;
		}
		if (clampAngle)
		{
			_y = ClampAngle(_y, yMinLimit, yMaxLimit);
		}
		Quaternion rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(_y, _x, 0f), Time.deltaTime * damping);
		if (allowZoom)
		{
			distanceHorizontal = Mathf.Clamp(distanceHorizontal - Input.GetAxis("Mouse ScrollWheel") * 5f, distanceMin, distanceMax);
		}
		float rotationX = rotation.eulerAngles.x;
		if (rotationX > 90f)
		{
			rotationX -= 360f;
		}
		float usedDistance = Mathf.Lerp(distanceHorizontal, distanceVertical, Mathf.Abs(rotationX / 90f));
		Vector3 negDistance = new Vector3(0f, 0f, 0f - usedDistance);
		Vector3 position = rotation * negDistance + targetPosition;
		base.transform.rotation = rotation;
		base.transform.position = position;
	}

	private static float ClampAngle(float angle, float min, float max)
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
