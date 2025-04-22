using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Constellations_UnselectButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			SingletonBehaviour<UI_Constellations>.instance.Unselect();
		}
	}
}
