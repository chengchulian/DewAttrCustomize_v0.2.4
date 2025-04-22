using UnityEngine;

public class NewMaterialChange : MonoBehaviour
{
	public bool isParticleSystem;

	public Material m_inputMaterial;

	private Material m_objectMaterial;

	private MeshRenderer m_meshRenderer;

	private ParticleSystemRenderer m_particleRenderer;

	public float m_timeToReduce;

	public float m_reduceFactor;

	private float m_time;

	private float m_submitReduceFactor;

	private float m_cutOutFactor;

	public float m_upFactor;

	private float upFactor;

	private bool isupfactor = true;

	private void Awake()
	{
		if (isParticleSystem)
		{
			m_particleRenderer = base.gameObject.GetComponent<ParticleSystemRenderer>();
			m_particleRenderer.material = m_inputMaterial;
			m_objectMaterial = m_particleRenderer.material;
		}
		else
		{
			m_meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
			m_meshRenderer.material = m_inputMaterial;
			m_objectMaterial = m_meshRenderer.material;
		}
		m_submitReduceFactor = 0f;
		m_cutOutFactor = 1f;
	}

	private void LateUpdate()
	{
		m_time += Time.deltaTime;
		if (m_time > m_timeToReduce)
		{
			m_cutOutFactor -= m_submitReduceFactor;
			m_submitReduceFactor = Mathf.Lerp(m_submitReduceFactor, m_reduceFactor, Time.deltaTime / 50f);
		}
		m_cutOutFactor = Mathf.Clamp01(m_cutOutFactor);
		if (m_cutOutFactor <= 0f && m_time > m_timeToReduce)
		{
			Object.Destroy(base.gameObject);
		}
		m_objectMaterial.SetFloat("_MaskCutOut", m_cutOutFactor);
		if (m_upFactor != 0f && isupfactor)
		{
			upFactor += m_upFactor * Time.deltaTime;
			upFactor = Mathf.Clamp01(upFactor);
			m_objectMaterial.SetFloat("_MaskCutOut", upFactor);
			if (upFactor >= 1f)
			{
				isupfactor = false;
			}
		}
	}
}
