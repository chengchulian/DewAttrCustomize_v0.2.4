using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Settings_BindingWindow_ClickReceiver : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			GetComponentInParent<UI_Settings_BindingWindow>().DoLeftClick();
		}
	}
}
