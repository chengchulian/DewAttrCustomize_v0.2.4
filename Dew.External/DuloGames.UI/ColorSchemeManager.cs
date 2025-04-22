using UnityEngine;

namespace DuloGames.UI;

public class ColorSchemeManager : ScriptableObject
{
	private static ColorSchemeManager m_Instance;

	[SerializeField]
	private ColorScheme m_ActiveColorScheme;

	public static ColorSchemeManager Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = Resources.Load("ColorSchemeManager") as ColorSchemeManager;
			}
			return m_Instance;
		}
	}

	public ColorScheme activeColorScheme
	{
		get
		{
			return m_ActiveColorScheme;
		}
		set
		{
			m_ActiveColorScheme = value;
		}
	}
}
