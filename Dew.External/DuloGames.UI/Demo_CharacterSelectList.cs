using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI;

[RequireComponent(typeof(ToggleGroup))]
public class Demo_CharacterSelectList : MonoBehaviour
{
	[Serializable]
	public class OnCharacterSelectedEvent : UnityEvent<Demo_CharacterInfo>
	{
	}

	[Serializable]
	public class OnCharacterDeleteEvent : UnityEvent<Demo_CharacterInfo>
	{
	}

	[SerializeField]
	private GameObject m_CharacterPrefab;

	[SerializeField]
	private Transform m_CharactersContainer;

	[Header("Demo Properties")]
	[SerializeField]
	private bool m_IsDemo;

	[SerializeField]
	private int m_AddCharacters = 5;

	[Header("Events")]
	[SerializeField]
	private OnCharacterSelectedEvent m_OnCharacterSelected = new OnCharacterSelectedEvent();

	[SerializeField]
	private OnCharacterDeleteEvent m_OnCharacterDelete = new OnCharacterDeleteEvent();

	private ToggleGroup m_ToggleGroup;

	private Demo_CharacterSelectList_Character m_DeletingCharacter;

	protected void Awake()
	{
		m_ToggleGroup = base.gameObject.GetComponent<ToggleGroup>();
	}

	protected void Start()
	{
		if (m_CharactersContainer != null)
		{
			foreach (Transform item in m_CharactersContainer)
			{
				global::UnityEngine.Object.Destroy(item.gameObject);
			}
		}
		if (m_IsDemo && (bool)m_CharacterPrefab)
		{
			for (int i = 0; i < m_AddCharacters; i++)
			{
				string[] names = new string[10] { "Annika", "Evita", "Herb", "Thad", "Myesha", "Lucile", "Sharice", "Tatiana", "Isis", "Allen" };
				string[] races = new string[5] { "Human", "Elf", "Orc", "Undead", "Programmer" };
				string[] classes = new string[5] { "Warrior", "Mage", "Hunter", "Priest", "Designer" };
				Demo_CharacterInfo info = new Demo_CharacterInfo();
				info.name = names[global::UnityEngine.Random.Range(0, 10)];
				info.raceString = races[global::UnityEngine.Random.Range(0, 5)];
				info.classString = classes[global::UnityEngine.Random.Range(0, 5)];
				info.level = global::UnityEngine.Random.Range(1, 61);
				AddCharacter(info, i == 0);
			}
		}
	}

	public void AddCharacter(Demo_CharacterInfo info, bool selected)
	{
		if (!(m_CharacterPrefab == null) && !(m_CharactersContainer == null))
		{
			GameObject obj = global::UnityEngine.Object.Instantiate(m_CharacterPrefab);
			obj.layer = m_CharactersContainer.gameObject.layer;
			obj.transform.SetParent(m_CharactersContainer, worldPositionStays: false);
			obj.transform.localScale = m_CharacterPrefab.transform.localScale;
			obj.transform.localPosition = m_CharacterPrefab.transform.localPosition;
			obj.transform.localRotation = m_CharacterPrefab.transform.localRotation;
			Demo_CharacterSelectList_Character character = obj.GetComponent<Demo_CharacterSelectList_Character>();
			if (character != null)
			{
				character.SetCharacterInfo(info);
				character.SetToggleGroup(m_ToggleGroup);
				character.SetSelected(selected);
				character.AddOnSelectListener(OnCharacterSelected);
				character.AddOnDeleteListener(OnCharacterDeleteRequested);
			}
		}
	}

	private void OnCharacterSelected(Demo_CharacterSelectList_Character character)
	{
		if (m_OnCharacterSelected != null)
		{
			m_OnCharacterSelected.Invoke(character.characterInfo);
		}
	}

	private void OnCharacterDeleteRequested(Demo_CharacterSelectList_Character character)
	{
		m_DeletingCharacter = character;
		UIModalBox box = UIModalBoxManager.Instance.Create(base.gameObject);
		if (box != null)
		{
			box.SetText1("Do you really want to delete this character?");
			box.SetText2("You wont be able to reverse this operation and your\u0003charcater will be permamently removed.");
			box.SetConfirmButtonText("DELETE");
			box.onConfirm.AddListener(OnCharacterDeleteConfirm);
			box.onCancel.AddListener(OnCharacterDeleteCancel);
			box.Show();
		}
	}

	private void OnCharacterDeleteConfirm()
	{
		if (m_DeletingCharacter == null)
		{
			return;
		}
		if (m_DeletingCharacter.isSelected && m_CharactersContainer != null)
		{
			foreach (Transform item in m_CharactersContainer)
			{
				Demo_CharacterSelectList_Character character = item.gameObject.GetComponent<Demo_CharacterSelectList_Character>();
				if (!character.Equals(m_DeletingCharacter))
				{
					character.SetSelected(selected: true);
					break;
				}
			}
		}
		if (m_OnCharacterDelete != null)
		{
			m_OnCharacterDelete.Invoke(m_DeletingCharacter.characterInfo);
		}
		global::UnityEngine.Object.Destroy(m_DeletingCharacter.gameObject);
	}

	private void OnCharacterDeleteCancel()
	{
		m_DeletingCharacter = null;
	}
}
