using UnityEngine;

public class VariousTranslateMove : MonoBehaviour
{
	public float m_power;

	public float m_reduceTime;

	public bool m_fowardMove;

	public bool m_rightMove;

	public bool m_upMove;

	public float m_changedFactor;

	private float m_Time;

	private void Start()
	{
		m_Time = Time.time;
	}

	private void Update()
	{
		m_changedFactor = VariousEffectsScene.m_gaph_scenesizefactor;
		if (m_fowardMove)
		{
			base.transform.Translate(base.transform.forward * m_power * m_changedFactor);
		}
		if (m_rightMove)
		{
			base.transform.Translate(base.transform.right * m_power * m_changedFactor);
		}
		if (m_upMove)
		{
			base.transform.Translate(base.transform.up * m_power * m_changedFactor);
		}
		if (m_Time + m_reduceTime < Time.time && m_reduceTime != 0f)
		{
			m_power -= Time.deltaTime / 10f;
			m_power = Mathf.Clamp01(m_power);
		}
	}
}
