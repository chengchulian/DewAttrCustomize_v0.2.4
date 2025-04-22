using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

[LogicUpdatePriority(400)]
public class ContextMenu : LogicBehaviour, IDeselectHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	public int backButtonPriority = 10;

	private List<RaycastResult> _results = new List<RaycastResult>();

	private bool _isMouseOver;

	private BackHandler _handle;

	private IGamepadFocusable _lastFocusable;

	protected override void OnEnable()
	{
		base.OnEnable();
		EventSystem.current.SetSelectedGameObject(base.gameObject);
		Dew.RaycastAllUIElementsBelowCursor(_results);
		if (_results.Any((RaycastResult r) => r.gameObject.GetComponentInParent<ContextMenu>() == this))
		{
			_isMouseOver = true;
		}
		else
		{
			_isMouseOver = false;
		}
		_handle = ManagerBase<GlobalUIManager>.instance.AddBackHandler(this, backButtonPriority, delegate
		{
			base.gameObject.SetActive(value: false);
			return true;
		});
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			_lastFocusable = ManagerBase<GlobalUIManager>.instance.focused;
			Dew.CallDelayed(delegate
			{
				ManagerBase<GlobalUIManager>.instance.SetFocusOnFirstFocusable(base.gameObject);
			});
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (_handle != null)
		{
			_handle.Remove();
			_handle = null;
		}
		GoBackToPreviousFocusable();
	}

	private void GoBackToPreviousFocusable()
	{
		if (DewInput.currentMode == InputMode.Gamepad && ManagerBase<GlobalUIManager>.instance != null)
		{
			IGamepadFocusable f = _lastFocusable;
			_lastFocusable = null;
			Dew.CallDelayed(delegate
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(f);
			});
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		EventSystem.current.SetSelectedGameObject(base.gameObject);
	}

	public virtual void OnDeselect(BaseEventData eventData)
	{
		if (!_isMouseOver && DewInput.currentMode != InputMode.Gamepad)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		_isMouseOver = true;
		EventSystem.current.SetSelectedGameObject(base.gameObject);
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		_isMouseOver = false;
		EventSystem.current.SetSelectedGameObject(base.gameObject);
	}
}
