using System;
using UnityEngine;

namespace DuloGames.UI;

public class UITooltipManager : ScriptableObject
{
	private static UITooltipManager m_Instance;

	[SerializeField]
	private GameObject m_TooltipPrefab;

	[SerializeField]
	private int m_SpacerHeight = 6;

	[SerializeField]
	private int m_ItemTooltipWidth = 514;

	[SerializeField]
	private int m_SpellTooltipWidth = 514;

	[Header("Styles")]
	[SerializeField]
	private UITooltipLineStyle m_DefaultLineStyle = new UITooltipLineStyle(displayName: false);

	[SerializeField]
	private UITooltipLineStyle m_TitleLineStyle = new UITooltipLineStyle(displayName: false);

	[SerializeField]
	private UITooltipLineStyle m_DescriptionLineStyle = new UITooltipLineStyle(displayName: false);

	[SerializeField]
	private UITooltipLineStyle[] m_CustomStyles = new UITooltipLineStyle[0];

	public static UITooltipManager Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = Resources.Load("TooltipManager") as UITooltipManager;
			}
			return m_Instance;
		}
	}

	public GameObject prefab => m_TooltipPrefab;

	public int spacerHeight => m_SpacerHeight;

	public int itemTooltipWidth => m_ItemTooltipWidth;

	public int spellTooltipWidth => m_SpellTooltipWidth;

	public UITooltipLineStyle defaultLineStyle => m_DefaultLineStyle;

	public UITooltipLineStyle titleLineStyle => m_TitleLineStyle;

	public UITooltipLineStyle descriptionLineStyle => m_DescriptionLineStyle;

	public UITooltipLineStyle[] customStyles => m_CustomStyles;

	public UITooltipLineStyle GetCustomStyle(string name)
	{
		if (m_CustomStyles.Length != 0)
		{
			UITooltipLineStyle[] array = m_CustomStyles;
			foreach (UITooltipLineStyle style in array)
			{
				if (style.Name.Equals(name))
				{
					return style;
				}
			}
		}
		return m_DefaultLineStyle;
	}

	[ContextMenu("Sort Custom Styles")]
	public void SortCustomStyles()
	{
		Array.Sort(m_CustomStyles);
	}
}
