using UnityEngine;

namespace DuloGames.UI;

public class UITalentDatabase : ScriptableObject
{
	private static UITalentDatabase m_Instance;

	public UITalentInfo[] talents;

	public static UITalentDatabase Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = Resources.Load("Databases/TalentDatabase") as UITalentDatabase;
			}
			return m_Instance;
		}
	}

	public UITalentInfo Get(int index)
	{
		return talents[index];
	}

	public UITalentInfo GetByID(int ID)
	{
		for (int i = 0; i < talents.Length; i++)
		{
			if (talents[i].ID == ID)
			{
				return talents[i];
			}
		}
		return null;
	}
}
