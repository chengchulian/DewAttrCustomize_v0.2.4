using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

public class UIStatsAdd : MonoBehaviour
{
	[SerializeField]
	private Text m_ValueText;

	public void OnButtonPress()
	{
		if (!(m_ValueText == null))
		{
			m_ValueText.text = (int.Parse(m_ValueText.text) + 1).ToString();
		}
	}
}
