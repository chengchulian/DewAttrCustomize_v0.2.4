using UnityEngine;

public class DelayActive : MonoBehaviour
{
	public GameObject[] m_activeObj;

	public float m_delayTime;

	private float m_time;

	private void Start()
	{
		m_time = Time.time;
	}

	private void Update()
	{
		if (!(Time.time > m_time + m_delayTime))
		{
			return;
		}
		for (int i = 0; i < m_activeObj.Length; i++)
		{
			if (m_activeObj[i] != null)
			{
				m_activeObj[i].SetActive(value: true);
			}
		}
	}
}
