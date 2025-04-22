using TMPro;

public class UI_Title_ProfileSelection_DeleteProfileWindow : UI_Title_ProfileSelection_Window
{
	public TextMeshProUGUI confirmText;

	public DewProfileItem deletingItem;

	public override UI_Title_ProfileSelection.StateType GetMyState()
	{
		return UI_Title_ProfileSelection.StateType.Delete;
	}

	private void OnEnable()
	{
		if (deletingItem == null)
		{
			base.parent.SetState(UI_Title_ProfileSelection.StateType.List);
		}
		else
		{
			confirmText.text = string.Format(DewLocalization.GetUIValue("Title_Profile_ConfirmDelete"), (deletingItem.peek == null) ? "???" : deletingItem.peek.name);
		}
	}

	public void ConfirmDelete()
	{
		if (deletingItem.path == DewSave.profilePath)
		{
			DewSave.LoadProfile(null);
		}
		DewSave.DeleteProfile(deletingItem.path);
		if (DewSave.GetProfiles().Count > 0)
		{
			base.parent.SetState(UI_Title_ProfileSelection.StateType.List);
		}
		else
		{
			base.parent.SetState(UI_Title_ProfileSelection.StateType.Create);
		}
	}

	public void Cancel()
	{
		base.parent.SetState(UI_Title_ProfileSelection.StateType.List);
	}
}
