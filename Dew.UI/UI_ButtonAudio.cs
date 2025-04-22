using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ButtonAudio : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
	public AudioClip sfxMouseOver;

	public AudioClip sfxMouseExit;

	public AudioClip sfxMouseDown;

	public AudioClip sfxMouseUp;

	public AudioClip sfxClick;

	private Selectable _selectable;

	private void Awake()
	{
		_selectable = GetComponent<Selectable>();
		if (_selectable is Button b)
		{
			b.onClick.AddListener(delegate
			{
				Play(sfxClick);
			});
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!(_selectable != null) || _selectable.IsInteractable())
		{
			Play(sfxMouseOver);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!(_selectable != null) || _selectable.IsInteractable())
		{
			Play(sfxMouseExit);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!(_selectable != null) || _selectable.IsInteractable())
		{
			Play(sfxMouseDown);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!(_selectable != null) || _selectable.IsInteractable())
		{
			Play(sfxMouseUp);
		}
	}

	private void Play(AudioClip clip)
	{
		if (!(clip == null))
		{
			ManagerBase<AudioManager>.instance.PlayUISound(clip);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!(_selectable is Button) && (!(_selectable != null) || _selectable.IsInteractable()))
		{
			Play(sfxClick);
		}
	}
}
