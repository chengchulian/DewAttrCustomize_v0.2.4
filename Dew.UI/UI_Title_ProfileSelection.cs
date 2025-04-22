using System;

public class UI_Title_ProfileSelection : View
{
	public enum StateType
	{
		None,
		List,
		Create,
		Delete,
		Rename
	}

	public Action<StateType> onStateChanged;

	public StateType state { get; private set; }

	public void SetState(StateType type)
	{
		if (state != type)
		{
			state = type;
			onStateChanged?.Invoke(type);
		}
	}

	public void SetStateToRename()
	{
		SetState(StateType.Rename);
	}

	protected override void OnShow()
	{
		base.OnShow();
		if (DewSave.GetProfiles().Count > 0)
		{
			SetState(StateType.List);
		}
		else
		{
			SetState(StateType.Create);
		}
	}

	protected override void OnHide()
	{
		base.OnHide();
		SetState(StateType.None);
	}
}
