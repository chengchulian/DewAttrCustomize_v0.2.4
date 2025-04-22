using UnityEngine;

public class ObjectMove : MonoBehaviour
{
	public float time;

	private float m_time;

	private float m_time2;

	public float MoveSpeed = 10f;

	public bool AbleHit;

	public float HitDelay;

	public GameObject m_hitObject;

	private GameObject m_makedObject;

	public float MaxLength;

	public float DestroyTime2;

	private float m_scalefactor;

	private void Start()
	{
		m_scalefactor = VariousEffectsScene.m_gaph_scenesizefactor;
		m_time = Time.time;
		m_time2 = Time.time;
	}

	private void LateUpdate()
	{
		if (Time.time > m_time + time)
		{
			Object.Destroy(base.gameObject);
		}
		base.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed * m_scalefactor);
		if (AbleHit && Physics.Raycast(base.transform.position, base.transform.forward, out var hit, MaxLength) && Time.time > m_time2 + HitDelay)
		{
			m_time2 = Time.time;
			HitObj(hit);
		}
	}

	private void HitObj(RaycastHit hit)
	{
		m_makedObject = Object.Instantiate(m_hitObject, hit.point, Quaternion.LookRotation(hit.normal)).gameObject;
		Object.Destroy(m_makedObject, DestroyTime2);
	}
}
