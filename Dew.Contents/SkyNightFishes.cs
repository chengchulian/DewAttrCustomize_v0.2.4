using UnityEngine;

public class SkyNightFishes : MonoBehaviour
{
	private float _startYPos;

	private float _sinSpeed;

	private float _yPosRange;

	private void Start()
	{
		_startYPos = base.transform.position.y;
		_sinSpeed = Random.Range(1f, 2f);
		_yPosRange = Random.Range(0.5f, 1f);
		base.transform.GetComponent<Animator>().speed = Random.Range(1f, 1.5f);
	}

	private void Update()
	{
		Vector3 position = base.transform.position;
		float y = (_startYPos + Mathf.Sin(Time.time * _sinSpeed)) * _yPosRange;
		base.transform.position = new Vector3(position.x, y, position.z);
	}
}
