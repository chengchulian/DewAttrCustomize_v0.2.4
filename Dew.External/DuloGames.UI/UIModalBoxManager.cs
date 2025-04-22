using System.Collections.Generic;
using UnityEngine;

namespace DuloGames.UI;

public class UIModalBoxManager : ScriptableObject
{
	private static UIModalBoxManager m_Instance;

	[SerializeField]
	private GameObject m_ModalBoxPrefab;

	private List<UIModalBox> m_ActiveBoxes = new List<UIModalBox>();

	public static UIModalBoxManager Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = Resources.Load("ModalBoxManager") as UIModalBoxManager;
			}
			return m_Instance;
		}
	}

	public GameObject prefab => m_ModalBoxPrefab;

	public UIModalBox[] activeBoxes
	{
		get
		{
			m_ActiveBoxes.RemoveAll((UIModalBox item) => item == null);
			return m_ActiveBoxes.ToArray();
		}
	}

	public UIModalBox Create(GameObject rel)
	{
		if (m_ModalBoxPrefab == null || rel == null)
		{
			return null;
		}
		Canvas canvas = UIUtility.FindInParents<Canvas>(rel);
		if (canvas != null)
		{
			return Object.Instantiate(m_ModalBoxPrefab, canvas.transform, worldPositionStays: false).GetComponent<UIModalBox>();
		}
		return null;
	}

	public void RegisterActiveBox(UIModalBox box)
	{
		if (!m_ActiveBoxes.Contains(box))
		{
			m_ActiveBoxes.Add(box);
		}
	}

	public void UnregisterActiveBox(UIModalBox box)
	{
		if (m_ActiveBoxes.Contains(box))
		{
			m_ActiveBoxes.Remove(box);
		}
	}
}
