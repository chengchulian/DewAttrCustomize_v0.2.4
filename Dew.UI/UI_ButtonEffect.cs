using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ButtonEffect : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
	public GameObject fxMouseOver;

	public GameObject fxMouseExit;

	public GameObject fxMouseDown;

	public GameObject fxMouseUp;

	public GameObject fxClick;

	private Selectable _selectable;

	private void Awake()
	{
		_selectable = GetComponent<Selectable>();
		if (_selectable is Button b)
		{
			b.onClick.AddListener(delegate
			{
				Play(fxClick);
			});
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!(_selectable != null) || _selectable.IsInteractable())
		{
			Play(fxMouseOver);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!(_selectable != null) || _selectable.IsInteractable())
		{
			Play(fxMouseExit);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!(_selectable != null) || _selectable.IsInteractable())
		{
			Play(fxMouseDown);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!(_selectable != null) || _selectable.IsInteractable())
		{
			Play(fxMouseUp);
		}
	}

	private void Play(GameObject gobj)
	{
		if (!(gobj == null))
		{
			DewEffect.Play(gobj);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!(_selectable is Button) && (!(_selectable != null) || _selectable.IsInteractable()))
		{
			Play(fxClick);
		}
	}
}
