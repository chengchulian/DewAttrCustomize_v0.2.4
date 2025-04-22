using System;
using UnityEngine;

public class DewGameplaySettings_User : ICloneable, IInitializableSettings
{
	public float uiScale = 1f;

	public DamageNumberVisibility damageNumberVisibility = DamageNumberVisibility.All;

	public bool abbreviateBigDamageNumbers = true;

	public bool disableTutorial;

	public float cursorScale = 1f;

	public float cursorColor;

	public bool unlockSkillsOutsideEditMode;

	public float killScreenEffectsStrength = 1f;

	public float screenShakeStrength = 1f;

	public float gamepadVibrationStrength = 1f;

	public void Initialize()
	{
		if (SystemInfo.deviceName == "steamdeck")
		{
			uiScale = 1.2f;
			cursorScale = 1.2f;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
