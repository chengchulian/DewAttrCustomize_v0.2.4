using System;
using UnityEngine;

public class PerPlatformVisibility : MonoBehaviour
{
	public bool editor;

	public bool drmfree;

	public bool steam;

	public bool stove;

	private void Awake()
	{
		switch (DewBuildProfile.current.platform)
		{
		case PlatformType.DRMFREE:
			if (!drmfree)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case PlatformType.STEAM:
			if (!steam)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case PlatformType.STOVE:
			if (!stove)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}
}
