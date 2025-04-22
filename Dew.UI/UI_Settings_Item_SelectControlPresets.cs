using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Settings_Item_SelectControlPresets : MonoBehaviour, IGamepadFocusable, IGamepadFocusListener, IGamepadFocusableOverrideInput
{
	public void OnFocusStateChanged(bool state)
	{
	}

	public bool CanBeFocused()
	{
		return base.isActiveAndEnabled;
	}

	public bool OnGamepadConfirm()
	{
		IPointerClickHandler[] components = GetComponentInChildren<UI_Settings_ControlPresetButton>().GetComponents<IPointerClickHandler>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].OnPointerClick(new PointerEventData(EventSystem.current)
			{
				button = PointerEventData.InputButton.Left
			});
		}
		return true;
	}
}
