using System;
using System.Collections.Generic;
using DEShaders;
using DEShaders.Utils;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("DE Environment/Global Controller")]
public class DE_EnvironmentGlobalController : MonoBehaviour
{
	public WindZone windZone;

	public bool SynchWindZone;

	public float WindStrength = 0.01f;

	public int FadeWindDistanceMode;

	public float FadeWindDistanceBias;

	public float WindPulse = 10f;

	public float WindTurbulence = 0.01f;

	public float WindRandomness;

	public bool BillboardEnabled;

	public float BillboardIntensity;

	public bool ClothEnabled;

	public float ClothIntensity;

	public bool SnowEnabled;

	public float SnowIntensityTopDown = 1f;

	public float SnowIntensityBottomUp = 1f;

	public bool RainEnbaled;

	public float RainIntensity = 1f;

	public float RainWetness;

	public bool WaterEnabled;

	[HideInInspector]
	public List<bool> foldouts;

	[HideInInspector]
	public List<Action> actions;

	[HideInInspector]
	public List<GUIContent> guiContent;

	private float windStrength;

	private float windDirection;

	private float windPulse;

	private float windTurbulence;

	private readonly string _WindStrength = "_Global_Wind_Main_Intensity";

	private readonly string _WindFadeDistanceMode = "_Global_Wind_Main_DistanceFade_Enabled";

	private readonly string _WindFadeDistanceBias = "_Global_Wind_Main_DistanceFade_Bias";

	private readonly string _WindDirection = "_Global_Wind_Main_Direction";

	private readonly string _WindPulse = "_Global_Wind_Main_Pulse";

	private readonly string _WindTurbulence = "_Global_Wind_Main_Turbulence";

	private readonly string _RandomWind = "_Global_Wind_Main_RandomOffset";

	private readonly string _BillboardEnabled = "_Global_Wind_Billboard_Enabled";

	private readonly string _BillboardIntensity = "_Global_Wind_Billboard_Intensity";

	private readonly string _ClothEnabled = "_Global_Wind_Cloth_Enabled";

	private readonly string _Cloth_WindIntensity = "_Global_Wind_Cloth_Intensity";

	private readonly string _SnowEnabled = "_Global_Snow_Enabled";

	private readonly string _SnowIntensity_TopDown = "_Global_Snow_Intensity_TopDown";

	private readonly string _SnowIntensity_BottomUp = "_Global_Snow_Intensity_BottomUp";

	private readonly string _RainEnabled = "_Global_Rain_Enabled";

	private readonly string _RainIntensity = "_Global_Rain_Intensity";

	private readonly string _RainWetness = "_Global_Rain_Wetness";

	private readonly string _WaterEnabled = "_Global_Wind_Water_Enabled";

	public string DEVersion => SchematicVersionControl.VERSION;

	private void OnDisable()
	{
		ResetShaders();
	}

	private void OnDestroy()
	{
		ResetShaders();
	}

	private void OnEnable()
	{
		SetShaders();
	}

	private void Update()
	{
		SetUpdateValues();
	}

	private void Reset()
	{
		WindStrength = 0.01f;
		FadeWindDistanceMode = 0;
		FadeWindDistanceBias = 0f;
		WindRandomness = 0f;
		WindPulse = 10f;
		WindTurbulence = 0.01f;
		BillboardEnabled = false;
		BillboardIntensity = 0f;
		ClothEnabled = false;
		ClothIntensity = 1f;
		SnowEnabled = false;
		SnowIntensityTopDown = 1f;
		SnowIntensityBottomUp = 1f;
		RainEnbaled = false;
		RainIntensity = 1f;
		WaterEnabled = false;
		ResetVSPProperties();
		SetShaders();
	}

	public void SetUpdateValues()
	{
		GetDefaultValues();
		GetWindZoneValues();
	}

	private void GetDefaultValues()
	{
		if (!SynchWindZone && (windStrength != _WindStrength.GetGlobalFloat() || base.transform.rotation.eulerAngles.y != _WindDirection.GetGlobalFloat() || windPulse != _WindPulse.GetGlobalFloat() || windTurbulence != _WindTurbulence.GetGlobalFloat() || windDirection != _WindDirection.GetGlobalFloat()))
		{
			SetShaders();
			windStrength = _WindStrength.GetGlobalFloat();
			windDirection = _WindDirection.GetGlobalFloat();
			windPulse = _WindPulse.GetGlobalFloat();
			windTurbulence = _WindTurbulence.GetGlobalFloat();
		}
	}

	private void GetWindZoneValues()
	{
		if ((bool)windZone && SynchWindZone && (windZone.windMain != WindStrength || windZone.windPulseFrequency != WindPulse || windZone.windTurbulence != windTurbulence))
		{
			WindStrength = windZone.windMain;
			WindPulse = windZone.windPulseFrequency;
			WindTurbulence = windZone.windTurbulence;
			SetShaders();
		}
	}

	public void SetShaders()
	{
		_WindStrength.SetGlobalFloat(WindStrength);
		_WindFadeDistanceMode.SetGlobalInt(FadeWindDistanceMode);
		_WindFadeDistanceBias.SetGlobalFloat(FadeWindDistanceBias);
		_WindDirection.SetGlobalFloat(base.transform.rotation.eulerAngles.y);
		_WindPulse.SetGlobalFloat(WindPulse);
		_WindTurbulence.SetGlobalFloat(WindTurbulence);
		_RandomWind.SetGlobalFloat(WindRandomness);
		if (BillboardEnabled)
		{
			_BillboardEnabled.SetGlobalInt(1);
			_BillboardIntensity.SetGlobalFloat(BillboardIntensity);
		}
		else
		{
			_BillboardEnabled.SetGlobalInt(0);
		}
		if (ClothEnabled)
		{
			_ClothEnabled.SetGlobalInt(1);
			_Cloth_WindIntensity.SetGlobalFloat(ClothIntensity);
		}
		else
		{
			_ClothEnabled.SetGlobalInt(0);
		}
		if (SnowEnabled)
		{
			_SnowEnabled.SetGlobalInt(1);
			_SnowIntensity_TopDown.SetGlobalFloat(SnowIntensityTopDown);
			_SnowIntensity_BottomUp.SetGlobalFloat(SnowIntensityBottomUp);
		}
		else
		{
			_SnowEnabled.SetGlobalInt(0);
		}
		if (RainEnbaled)
		{
			_RainEnabled.SetGlobalInt(1);
			_RainIntensity.SetGlobalFloat(RainIntensity);
			_RainWetness.SetGlobalFloat(RainWetness);
		}
		else
		{
			_RainEnabled.SetGlobalInt(0);
		}
		if (WaterEnabled)
		{
			_WaterEnabled.SetGlobalInt(1);
		}
		else
		{
			_WaterEnabled.SetGlobalInt(0);
		}
		UpdateSineSpace();
	}

	private void ResetShaders()
	{
		_WindStrength.SetGlobalFloat(0f);
		_WindFadeDistanceMode.SetGlobalInt(0);
		_WindFadeDistanceBias.SetGlobalFloat(0f);
		_WindPulse.SetGlobalFloat(0f);
		_WindTurbulence.SetGlobalFloat(0f);
		_RandomWind.SetGlobalFloat(0f);
		_BillboardEnabled.SetGlobalInt(0);
		_BillboardIntensity.SetGlobalFloat(0f);
		_ClothEnabled.SetGlobalInt(0);
		_SnowEnabled.SetGlobalInt(0);
		_RainEnabled.SetGlobalInt(0);
		_RainIntensity.SetGlobalFloat(0f);
		_RainWetness.SetGlobalFloat(0f);
	}

	private void OnRenderObject()
	{
	}

	private void ResetVSPProperties()
	{
	}

	private void UpdateSineSpace()
	{
	}
}
