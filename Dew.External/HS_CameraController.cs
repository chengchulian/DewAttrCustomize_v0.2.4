using UnityEngine;

public class HS_CameraController : MonoBehaviour
{
	public Transform Holder;

	public Vector3 cameraPos = new Vector3(0f, 0f, 0f);

	public float currDistance = 5f;

	public float xRotate = 250f;

	public float yRotate = 120f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	public float prevDistance;

	private float x;

	private float y;

	private RaycastHit hit;

	public LayerMask collidingLayers = -1;

	private float distanceHit;

	private void Start()
	{
		Vector3 angles = base.transform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}

	private void LateUpdate()
	{
		if (currDistance < 2f)
		{
			currDistance = 2f;
		}
		Vector3 targetPos = Holder.position + new Vector3(0f, (distanceHit - 2f) / 3f + cameraPos[1], 0f);
		currDistance -= Input.GetAxis("Mouse ScrollWheel") * 2f;
		if ((bool)Holder)
		{
			Vector3 pos = Input.mousePosition;
			float dpiScale = 1f;
			if (Screen.dpi < 1f)
			{
				dpiScale = 1f;
			}
			dpiScale = ((!(Screen.dpi < 200f)) ? (Screen.dpi / 200f) : 1f);
			if (pos.x < 380f * dpiScale && (float)Screen.height - pos.y < 250f * dpiScale)
			{
				return;
			}
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			x += (float)((double)(Input.GetAxis("Mouse X") * xRotate) * 0.02);
			y -= (float)((double)(Input.GetAxis("Mouse Y") * yRotate) * 0.02);
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			Quaternion rotation = Quaternion.Euler(y, x, 0f);
			Vector3 position = rotation * new Vector3(0f, 0f, 0f - currDistance) + targetPos;
			if (Physics.Raycast(targetPos, position - targetPos, out hit, (position - targetPos).magnitude, collidingLayers))
			{
				base.transform.position = hit.point;
				distanceHit = Mathf.Clamp(Vector3.Distance(targetPos, hit.point), 4f, 600f);
			}
			else
			{
				base.transform.position = position;
				distanceHit = currDistance;
			}
			base.transform.rotation = rotation;
		}
		else
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		if (prevDistance != currDistance)
		{
			prevDistance = currDistance;
			Quaternion rot = Quaternion.Euler(y, x, 0f);
			Vector3 po = rot * new Vector3(0f, 0f, 0f - currDistance) + targetPos;
			base.transform.rotation = rot;
			base.transform.position = po;
		}
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
