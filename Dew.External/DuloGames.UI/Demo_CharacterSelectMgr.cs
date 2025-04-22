using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI;

public class Demo_CharacterSelectMgr : MonoBehaviour
{
	[Serializable]
	public class OnCharacterSelectedEvent : UnityEvent<Demo_CharacterInfo>
	{
	}

	[Serializable]
	public class OnCharacterDeleteEvent : UnityEvent<Demo_CharacterInfo>
	{
	}

	private static Demo_CharacterSelectMgr m_Mgr;

	[SerializeField]
	private int m_IngameSceneId;

	[Header("Camera Properties")]
	[SerializeField]
	private Camera m_Camera;

	[SerializeField]
	private float m_CameraSpeed = 10f;

	[SerializeField]
	private float m_CameraDistance = 10f;

	[SerializeField]
	private Vector3 m_CameraDirection = Vector3.forward;

	[Header("Character Slots")]
	[SerializeField]
	private List<Transform> m_Slots;

	[Header("Selected Character Info")]
	[SerializeField]
	private GameObject m_InfoContainer;

	[SerializeField]
	private Text m_NameText;

	[SerializeField]
	private Text m_LevelText;

	[SerializeField]
	private Text m_RaceText;

	[SerializeField]
	private Text m_ClassText;

	[Header("Demo Properties")]
	[SerializeField]
	private bool m_IsDemo;

	[SerializeField]
	private GameObject m_CharacterPrefab;

	[SerializeField]
	private int m_AddCharacters = 5;

	[Header("Events")]
	[SerializeField]
	private OnCharacterSelectedEvent m_OnCharacterSelected = new OnCharacterSelectedEvent();

	[SerializeField]
	private OnCharacterDeleteEvent m_OnCharacterDelete = new OnCharacterDeleteEvent();

	private int m_SelectedIndex = -1;

	private Transform m_SelectedTransform;

	public static Demo_CharacterSelectMgr instance => m_Mgr;

	protected void Awake()
	{
		m_Mgr = this;
		if (m_Camera == null)
		{
			m_Camera = Camera.main;
		}
		if (m_InfoContainer != null)
		{
			m_InfoContainer.SetActive(value: false);
		}
	}

	protected void OnDestroy()
	{
		m_Mgr = null;
	}

	protected void Start()
	{
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
				AddCharacter(info, m_CharacterPrefab, i);
			}
		}
		SelectFirstAvailable();
	}

	protected void Update()
	{
		if ((!base.isActiveAndEnabled || m_Slots.Count != 0) && m_SelectedTransform != null)
		{
			Vector3 targetPos = m_SelectedTransform.position + m_CameraDirection * m_CameraDistance;
			targetPos.y = m_Camera.transform.position.y;
			m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, targetPos, Time.deltaTime * m_CameraSpeed);
		}
	}

	public void AddCharacter(Demo_CharacterInfo info, GameObject modelPrefab, int index)
	{
		if (m_Slots.Count == 0 || m_Slots.Count < index + 1 || modelPrefab == null)
		{
			return;
		}
		Transform slotTrans = m_Slots[index];
		if (slotTrans == null)
		{
			return;
		}
		Demo_CharacterSelectSlot csc = slotTrans.gameObject.GetComponent<Demo_CharacterSelectSlot>();
		if (csc != null)
		{
			csc.info = info;
			csc.index = index;
		}
		foreach (Transform item in slotTrans)
		{
			global::UnityEngine.Object.Destroy(item.gameObject);
		}
		GameObject obj = global::UnityEngine.Object.Instantiate(modelPrefab);
		obj.layer = slotTrans.gameObject.layer;
		obj.transform.SetParent(slotTrans, worldPositionStays: false);
		obj.transform.localScale = modelPrefab.transform.localScale;
		obj.transform.localPosition = modelPrefab.transform.localPosition;
		obj.transform.localRotation = modelPrefab.transform.localRotation;
	}

	public void SelectFirstAvailable()
	{
		if (m_Slots.Count == 0)
		{
			return;
		}
		foreach (Transform trans in m_Slots)
		{
			if (!(trans == null))
			{
				Demo_CharacterSelectSlot slot = trans.gameObject.GetComponent<Demo_CharacterSelectSlot>();
				if (slot != null && slot.info != null)
				{
					SelectCharacter(slot);
					break;
				}
			}
		}
	}

	public void SelectCharacter(int index)
	{
		if (m_Slots.Count == 0)
		{
			return;
		}
		Transform slotTrans = m_Slots[index];
		if (!(slotTrans == null))
		{
			Demo_CharacterSelectSlot slot = slotTrans.gameObject.GetComponent<Demo_CharacterSelectSlot>();
			if (slot != null)
			{
				SelectCharacter(slot);
			}
		}
	}

	public void SelectCharacter(Demo_CharacterSelectSlot slot)
	{
		if (m_SelectedIndex == slot.index)
		{
			return;
		}
		if (m_SelectedIndex > -1)
		{
			Transform selectedSlotTrans = m_Slots[m_SelectedIndex];
			if (selectedSlotTrans != null)
			{
				Demo_CharacterSelectSlot selectedSlot = selectedSlotTrans.gameObject.GetComponent<Demo_CharacterSelectSlot>();
				if (selectedSlot != null)
				{
					selectedSlot.OnDeselected();
				}
			}
		}
		m_SelectedIndex = slot.index;
		m_SelectedTransform = slot.transform;
		if (slot.info != null)
		{
			if (m_InfoContainer != null)
			{
				m_InfoContainer.SetActive(value: true);
			}
			if (m_NameText != null)
			{
				m_NameText.text = slot.info.name.ToUpper();
			}
			if (m_LevelText != null)
			{
				m_LevelText.text = slot.info.level.ToString();
			}
			if (m_RaceText != null)
			{
				m_RaceText.text = slot.info.raceString;
			}
			if (m_ClassText != null)
			{
				m_ClassText.text = slot.info.classString;
			}
			if (m_OnCharacterSelected != null)
			{
				m_OnCharacterSelected.Invoke(slot.info);
			}
		}
		else
		{
			if (m_InfoContainer != null)
			{
				m_InfoContainer.SetActive(value: false);
			}
			if (m_NameText != null)
			{
				m_NameText.text = "";
			}
			if (m_LevelText != null)
			{
				m_LevelText.text = "";
			}
			if (m_RaceText != null)
			{
				m_RaceText.text = "";
			}
			if (m_ClassText != null)
			{
				m_ClassText.text = "";
			}
		}
		slot.OnSelected();
	}

	public Demo_CharacterSelectSlot GetCharacterInDirection(float direction)
	{
		if (m_Slots.Count == 0)
		{
			return null;
		}
		if (m_SelectedTransform == null && m_Slots[0] != null)
		{
			return m_Slots[0].gameObject.GetComponent<Demo_CharacterSelectSlot>();
		}
		Demo_CharacterSelectSlot closest = null;
		float lastDistance = 0f;
		foreach (Transform trans in m_Slots)
		{
			if (trans.Equals(m_SelectedTransform))
			{
				continue;
			}
			float curDirection = trans.position.x - m_SelectedTransform.position.x;
			if ((!(direction > 0f) || !(curDirection > 0f)) && (!(direction < 0f) || !(curDirection < 0f)))
			{
				continue;
			}
			Demo_CharacterSelectSlot slot = trans.GetComponent<Demo_CharacterSelectSlot>();
			if (!(slot == null) && slot.info != null)
			{
				if (closest == null)
				{
					closest = slot;
					lastDistance = Vector3.Distance(m_SelectedTransform.position, trans.position);
				}
				else if (Vector3.Distance(m_SelectedTransform.position, trans.position) <= lastDistance)
				{
					closest = slot;
					lastDistance = Vector3.Distance(m_SelectedTransform.position, trans.position);
				}
			}
		}
		return closest;
	}

	public void SelectNext()
	{
		Demo_CharacterSelectSlot next = GetCharacterInDirection(1f);
		if (next != null)
		{
			SelectCharacter(next);
		}
	}

	public void SelectPrevious()
	{
		Demo_CharacterSelectSlot prev = GetCharacterInDirection(-1f);
		if (prev != null)
		{
			SelectCharacter(prev);
		}
	}

	public void RemoveCharacter(int index)
	{
		if (m_Slots.Count == 0)
		{
			return;
		}
		Transform slotTrans = m_Slots[index];
		if (slotTrans == null)
		{
			return;
		}
		Demo_CharacterSelectSlot slot = slotTrans.gameObject.GetComponent<Demo_CharacterSelectSlot>();
		if (m_OnCharacterDelete != null && slot.info != null)
		{
			m_OnCharacterDelete.Invoke(slot.info);
		}
		if (slot != null)
		{
			slot.info = null;
		}
		if (slot != null)
		{
			slot.OnDeselected();
		}
		foreach (Transform item in slotTrans)
		{
			global::UnityEngine.Object.Destroy(item.gameObject);
		}
		if (index == m_SelectedIndex)
		{
			if (m_InfoContainer != null)
			{
				m_InfoContainer.SetActive(value: false);
			}
			if (m_NameText != null)
			{
				m_NameText.text = "";
			}
			if (m_LevelText != null)
			{
				m_LevelText.text = "";
			}
			if (m_RaceText != null)
			{
				m_RaceText.text = "";
			}
			if (m_ClassText != null)
			{
				m_ClassText.text = "";
			}
			SelectFirstAvailable();
		}
	}

	public void DeleteSelected()
	{
		if (m_SelectedIndex > -1)
		{
			RemoveCharacter(m_SelectedIndex);
		}
	}

	public void OnPlayClick()
	{
		UILoadingOverlay loadingOverlay = UILoadingOverlayManager.Instance.Create();
		if (loadingOverlay != null)
		{
			loadingOverlay.LoadScene(m_IngameSceneId);
		}
	}
}
