using UnityEngine;

namespace DuloGames.UI;

public class Test_SelectAddOption : MonoBehaviour
{
	[SerializeField]
	private UISelectField m_SelectField;

	[SerializeField]
	private string m_Text;

	[ContextMenu("Add Option")]
	public void AddOption()
	{
		if (m_SelectField != null)
		{
			m_SelectField.AddOption(m_Text);
		}
	}
}
