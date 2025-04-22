using UnityEngine;

public class camera : MonoBehaviour
{
	public Transform _target;

	public float _distance = 20f;

	public float _zoomStep = 1f;

	public float _xSpeed = 1f;

	public float _ySpeed = 1f;

	public float _dragSpeed = 0.1f;

	private float _x;

	private float _y = 0.5f;

	private Vector3 dragOrigin;

	private bool dragging;

	private Vector3 _distanceVector;

	private void Start()
	{
		_distanceVector = new Vector3(0f, 0.5f, 0f - _distance);
		Vector2 angles = base.transform.localEulerAngles;
		_x = angles.x;
		_y = angles.y;
		Rotate(_x, _y);
	}

	private void LateUpdate()
	{
		if ((bool)_target)
		{
			RotateControls();
			MoveControls();
			Zoom();
		}
	}

	private void RotateControls()
	{
		if (Input.GetButton("Fire2"))
		{
			_x += Input.GetAxis("Mouse X") * _xSpeed;
			_y += (0f - Input.GetAxis("Mouse Y")) * _ySpeed;
			Rotate(_x, _y);
		}
	}

	private void MoveControls()
	{
		if (Input.GetButton("Fire3") && !dragging)
		{
			dragOrigin = Input.mousePosition;
			dragging = true;
		}
		if (Input.GetButton("Fire3") && dragging)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
			Vector3 move = new Vector3(pos.x, pos.y, 0f);
			base.transform.Translate(move);
		}
		else
		{
			dragging = false;
		}
	}

	private void Rotate(float x, float y)
	{
		Quaternion rotation = Quaternion.Euler(y, x, 0f);
		Vector3 position = rotation * _distanceVector + _target.position;
		base.transform.rotation = rotation;
		base.transform.position = position;
	}

	private void Zoom()
	{
		if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			ZoomOut();
		}
		else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			ZoomIn();
		}
	}

	private void ZoomIn()
	{
		_distance -= _zoomStep;
		_distanceVector = new Vector3(0f, 0.5f, 0f - _distance);
		Rotate(_x, _y);
	}

	private void ZoomOut()
	{
		_distance += _zoomStep;
		_distanceVector = new Vector3(0f, 0.5f, 0f - _distance);
		Rotate(_x, _y);
	}
}
