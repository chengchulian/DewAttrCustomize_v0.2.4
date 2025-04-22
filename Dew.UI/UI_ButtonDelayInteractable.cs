using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_ButtonDelayInteractable : MonoBehaviour
{
	public bool focusOnEnable = true;

	private Button _button;

	private void Awake()
	{
		_button = GetComponent<Button>();
	}

	private void OnEnable()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			_button.interactable = false;
			yield return new WaitForSeconds(0.8f);
			_button.interactable = true;
			yield return null;
			IGamepadFocusable focusable = GetComponent<IGamepadFocusable>();
			if (focusable != null && DewInput.currentMode == InputMode.Gamepad)
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(focusable);
			}
		}
	}
}
