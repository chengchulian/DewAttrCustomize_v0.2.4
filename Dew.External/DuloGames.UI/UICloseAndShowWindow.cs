using UnityEngine;

namespace DuloGames.UI;

public class UICloseAndShowWindow : MonoBehaviour
{
	[SerializeField]
	private UIWindow m_CloseWindow;

	[SerializeField]
	private UIWindow m_ShowWindow;

	public void CloseAndShow()
	{
		if (m_CloseWindow != null)
		{
			m_CloseWindow.Hide();
		}
		if (m_ShowWindow != null)
		{
			m_ShowWindow.Show();
		}
	}
}
