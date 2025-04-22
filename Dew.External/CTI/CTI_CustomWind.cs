using UnityEngine;

namespace CTI;

[RequireComponent(typeof(WindZone))]
public class CTI_CustomWind : MonoBehaviour
{
	private WindZone m_WindZone;

	private Vector3 WindDirection;

	private float WindStrength;

	private float WindTurbulence;

	public float WindMultiplier = 1f;

	private bool init;

	private int TerrainLODWindPID;

	private void Init()
	{
		m_WindZone = GetComponent<WindZone>();
		TerrainLODWindPID = Shader.PropertyToID("_TerrainLODWind");
	}

	private void OnValidate()
	{
		Update();
	}

	private void Update()
	{
		if (!init)
		{
			Init();
		}
		WindDirection = base.transform.forward;
		if (m_WindZone == null)
		{
			m_WindZone = GetComponent<WindZone>();
		}
		WindStrength = m_WindZone.windMain * WindMultiplier;
		WindStrength += m_WindZone.windPulseMagnitude * (1f + Mathf.Sin(Time.time * m_WindZone.windPulseFrequency) + 1f + Mathf.Sin(Time.time * m_WindZone.windPulseFrequency * 3f)) * 0.5f;
		WindTurbulence = m_WindZone.windTurbulence * m_WindZone.windMain * WindMultiplier;
		WindDirection.x *= WindStrength;
		WindDirection.y *= WindStrength;
		WindDirection.z *= WindStrength;
		Shader.SetGlobalVector(TerrainLODWindPID, new Vector4(WindDirection.x, WindDirection.y, WindDirection.z, WindTurbulence));
	}
}
