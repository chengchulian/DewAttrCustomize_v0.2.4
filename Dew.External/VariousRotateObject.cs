using UnityEngine;

public class VariousRotateObject : MonoBehaviour
{
	public Vector3 RotateOffset;

	private Vector3 RotateMulti;

	public float m_delay;

	private float m_Time;

	private void Awake()
	{
		m_Time = Time.time;
	}

	private void Update()
	{
		if (!(Time.time < m_Time + m_delay))
		{
			RotateMulti = Vector3.Lerp(RotateMulti, RotateOffset, Time.deltaTime);
			base.transform.rotation *= Quaternion.Euler(RotateMulti);
		}
	}
}
