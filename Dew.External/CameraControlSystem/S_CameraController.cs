using UnityEngine;

namespace CameraControlSystem;

public class S_CameraController : MonoBehaviour
{
	public GameObject character;

	public Transform cameraHolder;

	public float zoomSpeed = 0.5f;

	public float rotationSpeed = 3f;

	public float smoothTime = 0.3f;

	private float currentZoom;

	private float zoomMin = 0.5f;

	private float zoomMax = 3f;

	private Vector3 zoomVelocity = Vector3.zero;

	private Vector3 positionVelocity = Vector3.zero;

	private Vector3 initialRelativePosition;

	private float lastRotateHorizontal;

	private void Start()
	{
		Cursor.visible = true;
		currentZoom = cameraHolder.localPosition.z;
	}

	private void Update()
	{
		HandleZoom();
		HandleRotation();
		HandleCursorVisibility();
	}

	private void HandleZoom()
	{
		float scroll = 0f - Input.GetAxis("Mouse ScrollWheel");
		currentZoom += scroll * zoomSpeed;
		currentZoom = Mathf.Clamp(currentZoom, zoomMin, zoomMax);
		float targetZ = Mathf.SmoothDamp(cameraHolder.localPosition.z, currentZoom, ref zoomVelocity.z, smoothTime);
		float zoomFactorInverse = 1f - (currentZoom - zoomMin) / (zoomMax - zoomMin);
		float headFocusY = 1.63f;
		float targetY = Mathf.Lerp(1.128f, headFocusY, zoomFactorInverse);
		targetY = Mathf.SmoothDamp(cameraHolder.localPosition.y, targetY, ref positionVelocity.y, smoothTime);
		cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, targetY, targetZ);
	}

	private void HandleRotation()
	{
		if (Input.GetMouseButton(0))
		{
			lastRotateHorizontal = (0f - Input.GetAxis("Mouse X")) * rotationSpeed;
			Quaternion rotation = Quaternion.Euler(0f, lastRotateHorizontal, 0f);
			character.transform.rotation *= rotation;
		}
		else if (!Mathf.Approximately(lastRotateHorizontal, 0f))
		{
			Quaternion rotation2 = Quaternion.Euler(0f, lastRotateHorizontal, 0f);
			character.transform.rotation *= rotation2;
			lastRotateHorizontal = Mathf.Lerp(lastRotateHorizontal, 0f, Time.deltaTime * rotationSpeed * 3f);
		}
	}

	private void HandleCursorVisibility()
	{
		if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		else
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
