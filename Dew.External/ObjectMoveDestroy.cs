using UnityEngine;

public class ObjectMoveDestroy : MonoBehaviour
{
	public GameObject m_gameObjectMain;

	public GameObject m_gameObjectTail;

	private GameObject m_makedObject;

	public Transform m_hitObject;

	public float maxLength;

	public bool isDestroy;

	public float ObjectDestroyTime;

	public float TailDestroyTime;

	public float HitObjectDestroyTime;

	public float maxTime = 1f;

	public float MoveSpeed = 10f;

	public bool isCheckHitTag;

	public string mtag;

	public bool isShieldActive;

	public bool isHitMake = true;

	private float time;

	private bool ishit;

	private float m_scalefactor;

	private void Start()
	{
		m_scalefactor = VariousEffectsScene.m_gaph_scenesizefactor;
		time = Time.time;
	}

	private void LateUpdate()
	{
		base.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed * m_scalefactor);
		if (!ishit && Physics.Raycast(base.transform.position, base.transform.forward, out var hit, maxLength))
		{
			HitObj(hit);
		}
		if (isDestroy && Time.time > time + ObjectDestroyTime)
		{
			MakeHitObject(base.transform);
			Object.Destroy(base.gameObject);
		}
	}

	private void MakeHitObject(RaycastHit hit)
	{
		if (isHitMake)
		{
			m_makedObject = Object.Instantiate(m_hitObject, hit.point, Quaternion.LookRotation(hit.normal)).gameObject;
			m_makedObject.transform.parent = base.transform.parent;
			m_makedObject.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	private void MakeHitObject(Transform point)
	{
		if (isHitMake)
		{
			m_makedObject = Object.Instantiate(m_hitObject, point.transform.position, point.rotation).gameObject;
			m_makedObject.transform.parent = base.transform.parent;
			m_makedObject.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	private void HitObj(RaycastHit hit)
	{
		if (isCheckHitTag && hit.transform.tag != mtag)
		{
			return;
		}
		ishit = true;
		if ((bool)m_gameObjectTail)
		{
			m_gameObjectTail.transform.parent = null;
		}
		MakeHitObject(hit);
		if (isShieldActive)
		{
			ShieldActivate m_sc = hit.transform.GetComponent<ShieldActivate>();
			if ((bool)m_sc)
			{
				m_sc.AddHitObject(hit.point);
			}
		}
		Object.Destroy(base.gameObject);
		Object.Destroy(m_gameObjectTail, TailDestroyTime);
		Object.Destroy(m_makedObject, HitObjectDestroyTime);
	}
}
