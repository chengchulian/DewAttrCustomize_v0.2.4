using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Lobby_Constellations_StarDetails_BuyButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public SafeAction onIsHoveringChanged;

	public Button button { get; private set; }

	public bool isHovering { get; private set; }

	private void Awake()
	{
		button = GetComponent<Button>();
	}

	private void OnEnable()
	{
		SetHover(value: false);
	}

	private void OnDisable()
	{
		SetHover(value: false);
	}

	private void SetHover(bool value)
	{
		if (isHovering != value)
		{
			isHovering = value;
			onIsHoveringChanged?.Invoke();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SetHover(value: true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SetHover(value: false);
	}
}
