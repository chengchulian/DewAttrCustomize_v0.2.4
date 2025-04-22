using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI;

public class Demo_CharacterSelectList_Character : MonoBehaviour
{
	[Serializable]
	public class OnCharacterSelectEvent : UnityEvent<Demo_CharacterSelectList_Character>
	{
	}

	[Serializable]
	public class OnCharacterDeleteEvent : UnityEvent<Demo_CharacterSelectList_Character>
	{
	}

	[SerializeField]
	private Toggle m_Toggle;

	[SerializeField]
	private Button m_Delete;

	[Header("Texts")]
	[SerializeField]
	private Text m_NameText;

	[SerializeField]
	private Text m_LevelText;

	[SerializeField]
	private Text m_RaceText;

	[SerializeField]
	private Text m_ClassText;

	[SerializeField]
	private Image m_Avatar;

	private Demo_CharacterInfo m_CharacterInfo;

	private OnCharacterSelectEvent m_OnCharacterSelected;

	private OnCharacterDeleteEvent m_OnCharacterDelete;

	public Demo_CharacterInfo characterInfo => m_CharacterInfo;

	public bool isSelected
	{
		get
		{
			if (!(m_Toggle != null))
			{
				return false;
			}
			return m_Toggle.isOn;
		}
	}

	protected void Awake()
	{
		m_OnCharacterSelected = new OnCharacterSelectEvent();
		m_OnCharacterDelete = new OnCharacterDeleteEvent();
	}

	protected void OnEnable()
	{
		if (m_Toggle != null)
		{
			m_Toggle.isOn = false;
			m_Toggle.onValueChanged.AddListener(OnToggleValueChanged);
		}
		if (m_Delete != null)
		{
			m_Delete.onClick.AddListener(OnDeleteClick);
		}
	}

	protected void OnDisable()
	{
		if (m_Toggle != null)
		{
			m_Toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
		}
		if (m_Delete != null)
		{
			m_Delete.onClick.RemoveListener(OnDeleteClick);
		}
	}

	public void SetCharacterInfo(Demo_CharacterInfo info)
	{
		if (info != null)
		{
			if (m_NameText != null)
			{
				m_NameText.text = info.name.ToUpper();
			}
			if (m_LevelText != null)
			{
				m_LevelText.text = info.level.ToString();
			}
			if (m_RaceText != null)
			{
				m_RaceText.text = info.raceString;
			}
			if (m_ClassText != null)
			{
				m_ClassText.text = info.classString;
			}
			m_CharacterInfo = info;
		}
	}

	public void SetAvatar(Sprite sprite)
	{
		if (m_Avatar != null)
		{
			m_Avatar.sprite = sprite;
		}
	}

	public void SetToggleGroup(ToggleGroup group)
	{
		if (m_Toggle != null)
		{
			m_Toggle.group = group;
		}
	}

	public void SetSelected(bool selected)
	{
		if (m_Toggle != null)
		{
			m_Toggle.isOn = selected;
		}
	}

	private void OnToggleValueChanged(bool value)
	{
		if (value && m_OnCharacterSelected != null)
		{
			m_OnCharacterSelected.Invoke(this);
		}
	}

	private void OnDeleteClick()
	{
		if (m_OnCharacterDelete != null)
		{
			m_OnCharacterDelete.Invoke(this);
		}
	}

	public void AddOnSelectListener(UnityAction<Demo_CharacterSelectList_Character> call)
	{
		m_OnCharacterSelected.AddListener(call);
	}

	public void RemoveOnSelectListener(UnityAction<Demo_CharacterSelectList_Character> call)
	{
		m_OnCharacterSelected.RemoveListener(call);
	}

	public void AddOnDeleteListener(UnityAction<Demo_CharacterSelectList_Character> call)
	{
		m_OnCharacterDelete.AddListener(call);
	}

	public void RemoveOnDeleteListener(UnityAction<Demo_CharacterSelectList_Character> call)
	{
		m_OnCharacterDelete.RemoveListener(call);
	}
}
