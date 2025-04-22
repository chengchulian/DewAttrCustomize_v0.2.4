using UnityEngine;

namespace DuloGames.UI;

public class UIItemDatabase : ScriptableObject
{
	private static UIItemDatabase m_Instance;

	public UIItemInfo[] items;

	public static UIItemDatabase Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = Resources.Load("Databases/ItemDatabase") as UIItemDatabase;
			}
			return m_Instance;
		}
	}

	public UIItemInfo Get(int index)
	{
		return items[index];
	}

	public UIItemInfo GetByID(int ID)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].ID == ID)
			{
				return items[i];
			}
		}
		return null;
	}
}
