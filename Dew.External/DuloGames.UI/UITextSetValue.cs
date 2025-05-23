using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[RequireComponent(typeof(Text))]
public class UITextSetValue : MonoBehaviour
{
	private Text m_Text;

	public string floatFormat = "0.00";

	protected void Awake()
	{
		m_Text = base.gameObject.GetComponent<Text>();
	}

	public void SetFloat(float value)
	{
		if (m_Text != null)
		{
			m_Text.text = value.ToString(floatFormat);
		}
	}
}
