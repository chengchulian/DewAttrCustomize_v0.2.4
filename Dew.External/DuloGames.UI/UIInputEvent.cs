using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

public class UIInputEvent : MonoBehaviour
{
	[SerializeField]
	private string m_InputName;

	[SerializeField]
	private UnityEvent m_OnButton;

	[SerializeField]
	private UnityEvent m_OnButtonDown;

	[SerializeField]
	private UnityEvent m_OnButtonUp;

	private Selectable m_Selectable;

	protected void Awake()
	{
		m_Selectable = base.gameObject.GetComponent<Selectable>();
	}

	protected void Update()
	{
		if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy || string.IsNullOrEmpty(m_InputName))
		{
			return;
		}
		if (EventSystem.current.currentSelectedGameObject != null)
		{
			Selectable targetSelectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
			if ((m_Selectable == null && targetSelectable != null) || (m_Selectable != null && targetSelectable != null && !m_Selectable.Equals(targetSelectable)))
			{
				return;
			}
		}
		if (UIWindowManager.Instance != null && UIWindowManager.Instance.escapeInputName == m_InputName && UIWindowManager.Instance.escapedUsed)
		{
			return;
		}
		try
		{
			if (Input.GetButton(m_InputName))
			{
				m_OnButton.Invoke();
			}
			if (Input.GetButtonDown(m_InputName))
			{
				m_OnButtonDown.Invoke();
			}
			if (Input.GetButtonUp(m_InputName))
			{
				m_OnButtonUp.Invoke();
			}
		}
		catch (ArgumentException)
		{
			base.enabled = false;
			Debug.LogWarning("Input \"" + m_InputName + "\" used by game object \"" + base.gameObject.name + "\" is not defined.");
		}
	}
}
