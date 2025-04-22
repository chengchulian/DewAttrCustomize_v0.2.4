using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

public class Demo_Chat : MonoBehaviour
{
	[Serializable]
	public enum TextEffect
	{
		None,
		Shadow,
		Outline
	}

	[Serializable]
	public class SendMessageEvent : UnityEvent<int, string>
	{
	}

	[Serializable]
	public class TabInfo
	{
		public int id;

		public UITab button;

		public Transform content;

		public ScrollRect scrollRect;
	}

	[SerializeField]
	private InputField m_InputField;

	[Header("Buttons")]
	[SerializeField]
	private Button m_Submit;

	[SerializeField]
	private Button m_ScrollTopButton;

	[SerializeField]
	private Button m_ScrollBottomButton;

	[SerializeField]
	private Button m_ScrollUpButton;

	[SerializeField]
	private Button m_ScrollDownButton;

	[Header("Tab Properties")]
	[SerializeField]
	private List<TabInfo> m_Tabs = new List<TabInfo>();

	[Header("Text Properties")]
	[SerializeField]
	private Font m_TextFont = FontData.defaultFontData.font;

	[SerializeField]
	private int m_TextFontSize = FontData.defaultFontData.fontSize;

	[SerializeField]
	private float m_TextLineSpacing = FontData.defaultFontData.lineSpacing;

	[SerializeField]
	private Color m_TextColor = Color.white;

	[SerializeField]
	private TextEffect m_TextEffect;

	[SerializeField]
	private Color m_TextEffectColor = Color.black;

	[SerializeField]
	private Vector2 m_TextEffectDistance = new Vector2(1f, -1f);

	[Header("Events")]
	public SendMessageEvent onSendMessage = new SendMessageEvent();

	private TabInfo m_ActiveTabInfo;

	protected void Awake()
	{
		m_ActiveTabInfo = FindActiveTab();
		if (m_Tabs == null || m_Tabs.Count <= 0)
		{
			return;
		}
		foreach (TabInfo info in m_Tabs)
		{
			if (!(info.content != null))
			{
				continue;
			}
			foreach (Transform item in info.content)
			{
				global::UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}

	protected void OnEnable()
	{
		if (m_Submit != null)
		{
			m_Submit.onClick.AddListener(OnSubmitClick);
		}
		if (m_ScrollUpButton != null)
		{
			m_ScrollUpButton.onClick.AddListener(OnScrollUpClick);
		}
		if (m_ScrollDownButton != null)
		{
			m_ScrollDownButton.onClick.AddListener(OnScrollDownClick);
		}
		if (m_ScrollTopButton != null)
		{
			m_ScrollTopButton.onClick.AddListener(OnScrollToTopClick);
		}
		if (m_ScrollBottomButton != null)
		{
			m_ScrollBottomButton.onClick.AddListener(OnScrollToBottomClick);
		}
		if (m_InputField != null)
		{
			m_InputField.onEndEdit.AddListener(OnInputEndEdit);
		}
		if (m_Tabs == null || m_Tabs.Count <= 0)
		{
			return;
		}
		foreach (TabInfo info in m_Tabs)
		{
			if (info.button != null)
			{
				info.button.onValueChanged.AddListener(OnTabStateChange);
			}
		}
	}

	protected void OnDisable()
	{
		if (m_Submit != null)
		{
			m_Submit.onClick.RemoveListener(OnSubmitClick);
		}
		if (m_ScrollUpButton != null)
		{
			m_ScrollUpButton.onClick.RemoveListener(OnScrollUpClick);
		}
		if (m_ScrollDownButton != null)
		{
			m_ScrollDownButton.onClick.RemoveListener(OnScrollDownClick);
		}
		if (m_ScrollTopButton != null)
		{
			m_ScrollTopButton.onClick.RemoveListener(OnScrollToTopClick);
		}
		if (m_ScrollBottomButton != null)
		{
			m_ScrollBottomButton.onClick.RemoveListener(OnScrollToBottomClick);
		}
		if (m_Tabs == null || m_Tabs.Count <= 0)
		{
			return;
		}
		foreach (TabInfo info in m_Tabs)
		{
			if (info.button != null)
			{
				info.button.onValueChanged.RemoveListener(OnTabStateChange);
			}
		}
	}

	public void OnSubmitClick()
	{
		if (m_InputField != null)
		{
			string text = m_InputField.text;
			if (!string.IsNullOrEmpty(text))
			{
				SendChatMessage(text);
			}
		}
	}

	public void OnScrollUpClick()
	{
		if (m_ActiveTabInfo != null && !(m_ActiveTabInfo.scrollRect == null))
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.scrollDelta = new Vector2(0f, 1f);
			m_ActiveTabInfo.scrollRect.OnScroll(pointerEventData);
		}
	}

	public void OnScrollDownClick()
	{
		if (m_ActiveTabInfo != null && !(m_ActiveTabInfo.scrollRect == null))
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.scrollDelta = new Vector2(0f, -1f);
			m_ActiveTabInfo.scrollRect.OnScroll(pointerEventData);
		}
	}

	public void OnScrollToTopClick()
	{
		if (m_ActiveTabInfo != null && !(m_ActiveTabInfo.scrollRect == null))
		{
			m_ActiveTabInfo.scrollRect.verticalNormalizedPosition = 1f;
		}
	}

	public void OnScrollToBottomClick()
	{
		if (m_ActiveTabInfo != null && !(m_ActiveTabInfo.scrollRect == null))
		{
			m_ActiveTabInfo.scrollRect.verticalNormalizedPosition = 0f;
		}
	}

	public void OnInputEndEdit(string text)
	{
		if (!string.IsNullOrEmpty(text) && Input.GetKey(KeyCode.Return))
		{
			SendChatMessage(text);
		}
	}

	public void OnTabStateChange(bool state)
	{
		if (state)
		{
			m_ActiveTabInfo = FindActiveTab();
		}
	}

	private TabInfo FindActiveTab()
	{
		if (m_Tabs != null && m_Tabs.Count > 0)
		{
			foreach (TabInfo info in m_Tabs)
			{
				if (info.button != null && info.button.isOn)
				{
					return info;
				}
			}
		}
		return null;
	}

	public TabInfo GetTabInfo(int tabId)
	{
		if (m_Tabs != null && m_Tabs.Count > 0)
		{
			foreach (TabInfo info in m_Tabs)
			{
				if (info.id == tabId)
				{
					return info;
				}
			}
		}
		return null;
	}

	private void SendChatMessage(string text)
	{
		int tabId = ((m_ActiveTabInfo != null) ? m_ActiveTabInfo.id : 0);
		if (onSendMessage != null)
		{
			onSendMessage.Invoke(tabId, text);
		}
		if (m_InputField != null)
		{
			m_InputField.text = "";
		}
	}

	public void ReceiveChatMessage(int tabId, string text)
	{
		TabInfo tabInfo = GetTabInfo(tabId);
		if (tabInfo == null || tabInfo.content == null)
		{
			return;
		}
		GameObject obj = new GameObject("Text " + tabInfo.content.childCount, typeof(RectTransform));
		obj.layer = base.gameObject.layer;
		RectTransform obj2 = obj.transform as RectTransform;
		obj2.localScale = new Vector3(1f, 1f, 1f);
		obj2.pivot = new Vector2(0f, 1f);
		obj2.anchorMin = new Vector2(0f, 1f);
		obj2.anchorMax = new Vector2(0f, 1f);
		obj2.SetParent(tabInfo.content, worldPositionStays: false);
		Text text2 = obj.AddComponent<Text>();
		text2.font = m_TextFont;
		text2.fontSize = m_TextFontSize;
		text2.lineSpacing = m_TextLineSpacing;
		text2.color = m_TextColor;
		text2.text = text;
		if (m_TextEffect != 0)
		{
			switch (m_TextEffect)
			{
			case TextEffect.Shadow:
			{
				Shadow shadow = obj.AddComponent<Shadow>();
				shadow.effectColor = m_TextEffectColor;
				shadow.effectDistance = m_TextEffectDistance;
				break;
			}
			case TextEffect.Outline:
			{
				Outline outline = obj.AddComponent<Outline>();
				outline.effectColor = m_TextEffectColor;
				outline.effectDistance = m_TextEffectDistance;
				break;
			}
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(tabInfo.content as RectTransform);
		OnScrollToBottomClick();
	}
}
