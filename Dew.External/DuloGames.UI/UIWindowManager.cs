using System.Collections.Generic;
using UnityEngine;

namespace DuloGames.UI;

public class UIWindowManager : MonoBehaviour
{
	private static UIWindowManager m_Instance;

	[SerializeField]
	private string m_EscapeInputName = "Cancel";

	private bool m_EscapeUsed;

	public static UIWindowManager Instance => m_Instance;

	public string escapeInputName => m_EscapeInputName;

	public bool escapedUsed => m_EscapeUsed;

	protected virtual void Awake()
	{
		if (m_Instance == null)
		{
			m_Instance = this;
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	protected virtual void OnDestroy()
	{
		if (m_Instance.Equals(this))
		{
			m_Instance = null;
		}
	}

	protected virtual void Update()
	{
		if (m_EscapeUsed)
		{
			m_EscapeUsed = false;
		}
		if (!Input.GetButtonDown(m_EscapeInputName))
		{
			return;
		}
		UIModalBox[] modalBoxes = Object.FindObjectsOfType<UIModalBox>();
		if (modalBoxes.Length != 0)
		{
			UIModalBox[] array = modalBoxes;
			foreach (UIModalBox box in array)
			{
				if (box.isActive && box.isActiveAndEnabled && box.gameObject.activeInHierarchy)
				{
					return;
				}
			}
		}
		List<UIWindow> windows = UIWindow.GetWindows();
		foreach (UIWindow window in windows)
		{
			if (window.escapeKeyAction != 0 && window.IsOpen && (window.escapeKeyAction == UIWindow.EscapeKeyAction.Hide || window.escapeKeyAction == UIWindow.EscapeKeyAction.Toggle || (window.escapeKeyAction == UIWindow.EscapeKeyAction.HideIfFocused && window.IsFocused)))
			{
				window.Hide();
				m_EscapeUsed = true;
			}
		}
		if (m_EscapeUsed)
		{
			return;
		}
		foreach (UIWindow window2 in windows)
		{
			if (!window2.IsOpen && window2.escapeKeyAction == UIWindow.EscapeKeyAction.Toggle)
			{
				window2.Show();
			}
		}
	}
}
