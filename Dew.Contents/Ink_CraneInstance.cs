using UnityEngine;

public class Ink_CraneInstance : MonoBehaviour
{
	private float _startYPos;

	private float _sinSpeed;

	private float _yPosRange;

	private void Start()
	{
		_startYPos = base.transform.position.y;
		_sinSpeed = Random.Range(0.5f, 1f);
		_yPosRange = Random.Range(0.8f, 1.2f);
		base.transform.GetComponent<Animator>().speed = Random.Range(0.9f, 1.2f);
	}

	private void Update()
	{
		Vector3 position = base.transform.position;
		float y = (_startYPos + Mathf.Sin(Time.time * _sinSpeed)) * _yPosRange;
		base.transform.position = new Vector3(position.x, y, position.z);
	}
}
