using UnityEngine;
using UnityEngine.EventSystems;

namespace DuloGames.UI;

[AddComponentMenu("UI/Audio/Play Audio")]
public class UIPlayAudio : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	public enum Event
	{
		None,
		PointerEnter,
		PointerExit,
		PointerDown,
		PointerUp,
		Click,
		DoubleClick
	}

	[SerializeField]
	private AudioClip m_AudioClip;

	[SerializeField]
	[Range(0f, 1f)]
	private float m_Volume = 1f;

	[SerializeField]
	private Event m_PlayOnEvent;

	private bool m_Pressed;

	public AudioClip audioClip
	{
		get
		{
			return m_AudioClip;
		}
		set
		{
			m_AudioClip = value;
		}
	}

	public float volume
	{
		get
		{
			return m_Volume;
		}
		set
		{
			m_Volume = value;
		}
	}

	public Event playOnEvent
	{
		get
		{
			return m_PlayOnEvent;
		}
		set
		{
			m_PlayOnEvent = value;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!m_Pressed)
		{
			TriggerEvent(Event.PointerEnter);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!m_Pressed)
		{
			TriggerEvent(Event.PointerExit);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			TriggerEvent(Event.PointerDown);
			m_Pressed = true;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button != 0)
		{
			return;
		}
		TriggerEvent(Event.PointerUp);
		if (m_Pressed)
		{
			if (eventData.clickCount > 1)
			{
				TriggerEvent(Event.DoubleClick);
				eventData.clickCount = 0;
			}
			else
			{
				TriggerEvent(Event.Click);
			}
		}
		m_Pressed = false;
	}

	private void TriggerEvent(Event e)
	{
		if (e == m_PlayOnEvent)
		{
			PlayAudio();
		}
	}

	public void PlayAudio()
	{
		if (base.enabled && base.gameObject.activeInHierarchy && !(m_AudioClip == null))
		{
			if (UIAudioSource.Instance == null)
			{
				Debug.LogWarning("You dont have UIAudioSource in your scene. Cannot play audio clip.");
			}
			else
			{
				UIAudioSource.Instance.PlayAudio(m_AudioClip, m_Volume);
			}
		}
	}
}
