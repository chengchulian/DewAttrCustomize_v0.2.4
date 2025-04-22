using UnityEngine;

namespace DuloGames.UI;

public class UILoadingOverlayManager : ScriptableObject
{
	private static UILoadingOverlayManager m_Instance;

	[SerializeField]
	private GameObject m_LoadingOverlayPrefab;

	public static UILoadingOverlayManager Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = Resources.Load("LoadingOverlayManager") as UILoadingOverlayManager;
			}
			return m_Instance;
		}
	}

	public GameObject prefab => m_LoadingOverlayPrefab;

	public UILoadingOverlay Create()
	{
		if (m_LoadingOverlayPrefab == null)
		{
			return null;
		}
		return Object.Instantiate(m_LoadingOverlayPrefab).GetComponent<UILoadingOverlay>();
	}
}
