using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

public class Demo_Quit : MonoBehaviour
{
	[SerializeField]
	private Button m_HookToButton;

	protected void OnEnable()
	{
		if (m_HookToButton != null)
		{
			m_HookToButton.onClick.AddListener(ExitGame);
		}
	}

	protected void OnDisable()
	{
		if (m_HookToButton != null)
		{
			m_HookToButton.onClick.RemoveListener(ExitGame);
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
