using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("UI/Switch Select Field", 58)]
public class UISwitchSelect : MonoBehaviour
{
	[Serializable]
	public class ChangeEvent : UnityEvent<int, string>
	{
	}

	[SerializeField]
	private Text m_Text;

	[SerializeField]
	private Button m_PrevButton;

	[SerializeField]
	private Button m_NextButton;

	[HideInInspector]
	[SerializeField]
	private string m_SelectedItem;

	[SerializeField]
	private List<string> m_Options = new List<string>();

	public ChangeEvent onChange = new ChangeEvent();

	public List<string> options => m_Options;

	public string value
	{
		get
		{
			return m_SelectedItem;
		}
		set
		{
			SelectOption(value);
		}
	}

	public int selectedOptionIndex => GetOptionIndex(m_SelectedItem);

	protected void OnEnable()
	{
		if (m_PrevButton != null)
		{
			m_PrevButton.onClick.AddListener(OnPrevButtonClick);
		}
		if (m_NextButton != null)
		{
			m_NextButton.onClick.AddListener(OnNextButtonClick);
		}
	}

	protected void OnDisable()
	{
		if (m_PrevButton != null)
		{
			m_PrevButton.onClick.RemoveListener(OnPrevButtonClick);
		}
		if (m_NextButton != null)
		{
			m_NextButton.onClick.RemoveListener(OnNextButtonClick);
		}
	}

	protected void OnPrevButtonClick()
	{
		int prevIndex = selectedOptionIndex - 1;
		if (prevIndex < 0)
		{
			prevIndex = m_Options.Count - 1;
		}
		if (prevIndex >= m_Options.Count)
		{
			prevIndex = 0;
		}
		SelectOptionByIndex(prevIndex);
	}

	protected void OnNextButtonClick()
	{
		int nextIndex = selectedOptionIndex + 1;
		if (nextIndex < 0)
		{
			nextIndex = m_Options.Count - 1;
		}
		if (nextIndex >= m_Options.Count)
		{
			nextIndex = 0;
		}
		SelectOptionByIndex(nextIndex);
	}

	public int GetOptionIndex(string optionValue)
	{
		if (m_Options != null && m_Options.Count > 0 && !string.IsNullOrEmpty(optionValue))
		{
			for (int i = 0; i < m_Options.Count; i++)
			{
				if (optionValue.Equals(m_Options[i], StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
		}
		return -1;
	}

	public void SelectOptionByIndex(int index)
	{
		if (index >= 0 && index < m_Options.Count)
		{
			string newOption = m_Options[index];
			if (!newOption.Equals(m_SelectedItem))
			{
				m_SelectedItem = newOption;
				TriggerChangeEvent();
			}
		}
	}

	public void SelectOption(string optionValue)
	{
		if (!string.IsNullOrEmpty(optionValue))
		{
			int index = GetOptionIndex(optionValue);
			if (index >= 0 && index < m_Options.Count)
			{
				SelectOptionByIndex(index);
			}
		}
	}

	public void AddOption(string optionValue)
	{
		if (m_Options != null)
		{
			m_Options.Add(optionValue);
		}
	}

	public void AddOptionAtIndex(string optionValue, int index)
	{
		if (m_Options != null)
		{
			if (index >= m_Options.Count)
			{
				m_Options.Add(optionValue);
			}
			else
			{
				m_Options.Insert(index, optionValue);
			}
		}
	}

	public void RemoveOption(string optionValue)
	{
		if (m_Options != null && m_Options.Contains(optionValue))
		{
			m_Options.Remove(optionValue);
			ValidateSelectedOption();
		}
	}

	public void RemoveOptionAtIndex(int index)
	{
		if (m_Options != null && index >= 0 && index < m_Options.Count)
		{
			m_Options.RemoveAt(index);
			ValidateSelectedOption();
		}
	}

	public void ValidateSelectedOption()
	{
		if (m_Options != null && !m_Options.Contains(m_SelectedItem))
		{
			SelectOptionByIndex(0);
		}
	}

	protected virtual void TriggerChangeEvent()
	{
		if (m_Text != null)
		{
			m_Text.text = m_SelectedItem;
		}
		if (onChange != null)
		{
			onChange.Invoke(selectedOptionIndex, m_SelectedItem);
		}
	}
}
