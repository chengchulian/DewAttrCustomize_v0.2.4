using UnityEngine;

public class MultipleObjectsMake : _ObjectsMakeBase
{
	public float m_startDelay;

	public int m_makeCount;

	public float m_makeDelay;

	public Vector3 m_randomPos;

	public Vector3 m_randomRot;

	public Vector3 m_randomScale;

	private float m_Time;

	private float m_Time2;

	private float m_delayTime;

	private float m_count;

	private float m_scalefactor;

	private void Start()
	{
		m_Time = (m_Time2 = Time.time);
		m_scalefactor = VariousEffectsScene.m_gaph_scenesizefactor;
	}

	private void Update()
	{
		if (Time.time > m_Time + m_startDelay && Time.time > m_Time2 + m_makeDelay && m_count < (float)m_makeCount)
		{
			Vector3 m_pos = base.transform.position + GetRandomVector(m_randomPos) * m_scalefactor;
			Quaternion m_rot = base.transform.rotation * Quaternion.Euler(GetRandomVector(m_randomRot));
			for (int i = 0; i < m_makeObjs.Length; i++)
			{
				GameObject obj = Object.Instantiate(m_makeObjs[i], m_pos, m_rot);
				Vector3 m_scale = m_makeObjs[i].transform.localScale + GetRandomVector2(m_randomScale);
				obj.transform.parent = base.transform;
				obj.transform.localScale = m_scale;
			}
			m_Time2 = Time.time;
			m_count += 1f;
		}
	}
}
