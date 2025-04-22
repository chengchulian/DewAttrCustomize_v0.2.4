using UnityEngine;

public class VariousMouseOrbit : MonoBehaviour
{
	private Transform Target;

	public Transform[] Targets;

	private int i;

	public float distance;

	public float xSpeed = 250f;

	public float ySpeed = 120f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	private float x;

	private float y;

	public float CameraDist = 10f;

	private void Start()
	{
		Vector3 angles = base.transform.eulerAngles;
		x = angles.x + 50f;
		y = angles.y;
		distance = 30f;
		Target = Targets[0];
		if ((bool)GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
	}

	private void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (i < Targets.Length - 1)
			{
				i++;
			}
			else if (i >= Targets.Length - 1)
			{
				i = 0;
			}
			Target = Targets[i];
		}
		if (Input.GetKey(KeyCode.Mouse1) && (bool)Target)
		{
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y += Input.GetAxis("Mouse Y") * ySpeed * 0.05f;
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			Quaternion rotation = Quaternion.Euler(y, x, 0f);
			Vector3 position = rotation * new Vector3(0f, 0f, 0f - distance) + Target.position;
			base.transform.rotation = rotation;
			base.transform.position = position;
			distance = CameraDist;
			if (Input.GetKey(KeyCode.W))
			{
				CameraDist -= Time.deltaTime * 20f;
				CameraDist = Mathf.Clamp(CameraDist, 2f, 80f);
			}
			if (Input.GetKey(KeyCode.S))
			{
				CameraDist += Time.deltaTime * 20f;
				CameraDist = Mathf.Clamp(CameraDist, 2f, 80f);
			}
		}
	}

	private float ClampAngle(float ag, float min, float max)
	{
		if (ag < -360f)
		{
			ag += 360f;
		}
		if (ag > 360f)
		{
			ag -= 360f;
		}
		return Mathf.Clamp(ag, min, max);
	}
}
