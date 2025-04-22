using UnityEngine;

public class ScaleFactorApplyToMaterial : MonoBehaviour
{
	private ParticleSystemRenderer ps;

	private float value;

	private float m_scaleFactor;

	private float m_changedFactor;

	private void Awake()
	{
		ps = GetComponent<ParticleSystemRenderer>();
		value = ps.material.GetFloat("_NoiseScale");
		m_scaleFactor = 1f;
	}

	private void Update()
	{
		m_changedFactor = VariousEffectsScene.m_gaph_scenesizefactor;
		if (m_scaleFactor != m_changedFactor && m_changedFactor <= 1f)
		{
			m_scaleFactor = m_changedFactor;
			if (m_scaleFactor <= 0.5f)
			{
				ps.material.SetFloat("_NoiseScale", value * 0.25f);
			}
			else
			{
				ps.material.SetFloat("_NoiseScale", value * m_scaleFactor);
			}
		}
	}
}
