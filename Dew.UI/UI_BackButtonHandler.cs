using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_BackButtonHandler : MonoBehaviour
{
	public int priority;

	public bool consume = true;

	public bool checkUIOcclusion;

	public bool activateOnRightClickAnywhere;

	public UnityEvent onHandle;

	private BackHandler _handle;

	private void OnEnable()
	{
		if (ManagerBase<GlobalUIManager>.instance != null)
		{
			_handle = ManagerBase<GlobalUIManager>.instance.AddBackHandler(this, priority, () => (!checkUIOcclusion || ManagerBase<GlobalUIManager>.instance.IsUIElementClickable((RectTransform)base.transform)) && Callback());
		}
	}

	private void Update()
	{
		if (activateOnRightClickAnywhere && DewInput.GetButtonDown(MouseButton.Right, checkGameArea: false))
		{
			Callback();
		}
	}

	private void OnDisable()
	{
		if (_handle != null)
		{
			_handle.Remove();
			_handle = null;
		}
	}

	private bool Callback()
	{
		Button btn = GetComponent<Button>();
		if (btn != null)
		{
			if (!btn.IsInteractable())
			{
				return false;
			}
			btn.onClick?.Invoke();
		}
		onHandle?.Invoke();
		return consume;
	}
}
