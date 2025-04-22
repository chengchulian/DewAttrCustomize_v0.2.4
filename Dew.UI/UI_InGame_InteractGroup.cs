[LogicUpdatePriority(600)]
public class UI_InGame_InteractGroup : LogicBehaviour
{
	private UI_InGame_Interact_Base[] _groups;

	private int _activeGroup = -1;

	private IInteractable _interactable;

	private void Awake()
	{
		_groups = GetComponentsInChildren<UI_InGame_Interact_Base>(includeInactive: true);
		UI_InGame_Interact_Base[] groups = _groups;
		for (int i = 0; i < groups.Length; i++)
		{
			groups[i].OnDeactivate();
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (ManagerBase<ControlManager>.instance.focusedInteractable.IsUnityNull() || !InGameUIManager.instance.IsState("Playing"))
		{
			if (_activeGroup != -1)
			{
				_interactable = null;
				_groups[_activeGroup].OnDeactivate();
				_activeGroup = -1;
			}
		}
		else
		{
			if (ManagerBase<ControlManager>.instance.focusedInteractable == _interactable)
			{
				return;
			}
			if (_activeGroup != -1)
			{
				_interactable = null;
				_groups[_activeGroup].OnDeactivate();
				_activeGroup = -1;
			}
			for (int i = 0; i < _groups.Length; i++)
			{
				if (_groups[i].CanActivate(ManagerBase<ControlManager>.instance.focusedInteractable))
				{
					_groups[i].interactable = ManagerBase<ControlManager>.instance.focusedInteractable;
					_groups[i].OnActivate();
					_activeGroup = i;
					_interactable = ManagerBase<ControlManager>.instance.focusedInteractable;
					break;
				}
			}
		}
	}
}
