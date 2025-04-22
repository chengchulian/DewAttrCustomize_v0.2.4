using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

public class Demo_Spellbook_SpellRow : MonoBehaviour
{
	[SerializeField]
	private UISpellSlot m_Slot;

	[SerializeField]
	private Text m_NameText;

	[SerializeField]
	private Text m_RankText;

	[SerializeField]
	private Text m_DescriptionText;

	[SerializeField]
	private bool m_IsDemo;

	private void Start()
	{
		if (!(UISpellDatabase.Instance == null) && m_IsDemo)
		{
			UISpellInfo[] spells = UISpellDatabase.Instance.spells;
			UISpellInfo spell = spells[Random.Range(0, spells.Length)];
			if (m_Slot != null)
			{
				m_Slot.Assign(spell);
			}
			if (m_NameText != null)
			{
				m_NameText.text = spell.Name;
			}
			if (m_RankText != null)
			{
				m_RankText.text = Random.Range(1, 6).ToString();
			}
			if (m_DescriptionText != null)
			{
				m_DescriptionText.text = spell.Description;
			}
		}
	}
}
