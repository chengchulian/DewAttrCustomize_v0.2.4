using UnityEngine;

namespace DuloGames.UI;

public class UIBlackOverlayManager : ScriptableObject
{
	private static UIBlackOverlayManager m_Instance;

	[SerializeField]
	private GameObject m_BlackOverlayPrefab;

	public static UIBlackOverlayManager Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = Resources.Load("BlackOverlayManager") as UIBlackOverlayManager;
			}
			return m_Instance;
		}
	}

	public GameObject prefab => m_BlackOverlayPrefab;

	public UIBlackOverlay Create(Transform parent)
	{
		if (m_BlackOverlayPrefab == null)
		{
			return null;
		}
		return Object.Instantiate(m_BlackOverlayPrefab, parent).GetComponent<UIBlackOverlay>();
	}
}
