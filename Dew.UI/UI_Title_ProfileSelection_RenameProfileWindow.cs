using TMPro;
using UnityEngine.UI;

public class UI_Title_ProfileSelection_RenameProfileWindow : UI_Title_ProfileSelection_Window
{
	public TMP_InputField nameInput;

	public Button renameButton;

	public Button cancelButton;

	public override UI_Title_ProfileSelection.StateType GetMyState()
	{
		return UI_Title_ProfileSelection.StateType.Rename;
	}

	protected override void Awake()
	{
		base.Awake();
		nameInput.onValueChanged.AddListener(delegate(string text)
		{
			renameButton.interactable = DewProfile.ValidateProfileName(text.Trim()) && text.Trim() != DewSave.profile.name;
		});
		renameButton.onClick.AddListener(RenameProfile);
		cancelButton.onClick.AddListener(delegate
		{
			ManagerBase<UIManager>.instance.SetState("Title");
		});
	}

	private void OnEnable()
	{
		nameInput.text = "";
		renameButton.interactable = false;
		cancelButton.gameObject.SetActive(value: true);
		nameInput.ActivateInputField();
		nameInput.text = DewSave.profile.name;
	}

	private void RenameProfile()
	{
		if (DewProfile.ValidateProfileName(nameInput.text.Trim()))
		{
			DewSave.profile.name = nameInput.text.Trim();
			DewSave.SaveProfile();
			ManagerBase<UIManager>.instance.SetState("Title");
		}
	}
}
