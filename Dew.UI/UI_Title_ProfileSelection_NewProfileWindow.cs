using TMPro;
using UnityEngine.UI;

public class UI_Title_ProfileSelection_NewProfileWindow : UI_Title_ProfileSelection_Window
{
	public TMP_InputField nameInput;

	public Button createButton;

	public Button cancelButton;

	public override UI_Title_ProfileSelection.StateType GetMyState()
	{
		return UI_Title_ProfileSelection.StateType.Create;
	}

	protected override void Awake()
	{
		base.Awake();
		nameInput.onValueChanged.AddListener(delegate(string text)
		{
			createButton.interactable = DewProfile.ValidateProfileName(text.Trim());
		});
		createButton.onClick.AddListener(CreateProfile);
		cancelButton.onClick.AddListener(delegate
		{
			base.parent.SetState(UI_Title_ProfileSelection.StateType.List);
		});
	}

	private void OnEnable()
	{
		nameInput.text = "";
		createButton.interactable = false;
		cancelButton.gameObject.SetActive(DewSave.GetProfiles().Count > 0);
		nameInput.ActivateInputField();
	}

	private void CreateProfile()
	{
		DewSave.CreateProfile(nameInput.text.Trim());
		if (DewSave.GetProfiles().Count == 1)
		{
			ManagerBase<UIManager>.instance.SetState("Title");
		}
		else
		{
			base.parent.SetState(UI_Title_ProfileSelection.StateType.List);
		}
	}
}
