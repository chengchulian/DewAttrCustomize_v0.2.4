using UnityEngine;

namespace DuloGames.UI;

public class UISpellDatabase : ScriptableObject
{
	private static UISpellDatabase m_Instance;

	public UISpellInfo[] spells;

	public static UISpellDatabase Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = Resources.Load("Databases/SpellDatabase") as UISpellDatabase;
			}
			return m_Instance;
		}
	}

	public UISpellInfo Get(int index)
	{
		return spells[index];
	}

	public UISpellInfo GetByID(int ID)
	{
		for (int i = 0; i < spells.Length; i++)
		{
			if (spells[i].ID == ID)
			{
				return spells[i];
			}
		}
		return null;
	}
}
