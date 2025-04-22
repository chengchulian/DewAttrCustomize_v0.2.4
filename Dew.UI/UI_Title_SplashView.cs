using System;
using System.Globalization;
using Steamworks;
using UnityEngine;

public class UI_Title_SplashView : View
{
	public GameObject splashPhotosensitivity;

	public GameObject splashGRAC;

	public GameObject fxMoveToNext;

	public GameObject fxClose;

	private float _lastSplashChangeTime;

	protected override void OnShow()
	{
		base.OnShow();
		if (Application.IsPlaying(this))
		{
			ShowNext();
		}
	}

	public void Click()
	{
		if (base.isShowing && !(Time.unscaledTime - _lastSplashChangeTime < 0.45f))
		{
			ShowNext();
		}
	}

	public void ShowNext()
	{
		_lastSplashChangeTime = Time.unscaledTime;
		if (splashGRAC != null && splashGRAC.activeSelf)
		{
			global::UnityEngine.Object.DestroyImmediate(splashGRAC);
		}
		if (splashPhotosensitivity != null && splashPhotosensitivity.activeSelf)
		{
			global::UnityEngine.Object.DestroyImmediate(splashPhotosensitivity);
		}
		if (IsUserInSouthKorea() && splashGRAC != null)
		{
			splashGRAC.SetActive(value: true);
			DewEffect.PlayNew(fxMoveToNext);
		}
		else if (!DewSave.platformSettings.gameplay.skipPhotosensitivityWarning && splashPhotosensitivity != null)
		{
			splashPhotosensitivity.SetActive(value: true);
			DewEffect.PlayNew(fxMoveToNext);
		}
		else
		{
			DewEffect.PlayNew(fxClose);
			Hide();
		}
	}

	public static bool IsUserInSouthKorea()
	{
		if (SteamManagerBase.Initialized)
		{
			return SteamUtils.GetIPCountry() == "KR";
		}
		return new RegionInfo(CultureInfo.CurrentCulture.LCID).TwoLetterISORegionName.Equals("KR", StringComparison.OrdinalIgnoreCase);
	}
}
