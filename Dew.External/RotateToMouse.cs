using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
	public Camera cam;

	public float maximumLength;

	private Ray rayMouse;

	private Vector3 pos;

	private Vector3 direction;

	private Quaternion rotation;

	private void Update()
	{
		if (cam != null)
		{
			Vector3 mousePos = Input.mousePosition;
			rayMouse = cam.ScreenPointToRay(mousePos);
			if (Physics.Raycast(rayMouse.origin, rayMouse.direction, out var hit, maximumLength))
			{
				RotateToMouseDirection(base.gameObject, hit.point);
				return;
			}
			Vector3 pos = rayMouse.GetPoint(maximumLength);
			RotateToMouseDirection(base.gameObject, pos);
		}
		else
		{
			Debug.Log("No Camera");
		}
	}

	private void RotateToMouseDirection(GameObject obj, Vector3 destination)
	{
		direction = destination - obj.transform.position;
		rotation = Quaternion.LookRotation(direction);
		obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1f);
	}

	public Quaternion GetRotation()
	{
		return rotation;
	}
}
