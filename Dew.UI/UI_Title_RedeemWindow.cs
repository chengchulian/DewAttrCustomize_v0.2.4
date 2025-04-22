using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title_RedeemWindow : MonoBehaviour
{
	public TMP_InputField codeInputField;

	public Button submitButton;

	public Button backButton;

	public Button restoreGiftsButton;

	private void Start()
	{
		submitButton.onClick.AddListener(Submit);
		backButton.onClick.AddListener(delegate
		{
			ManagerBase<UIManager>.instance.SetState("Title");
		});
		codeInputField.onSubmit.AddListener(delegate
		{
			Submit();
		});
		codeInputField.onValueChanged.AddListener(delegate
		{
			submitButton.interactable = !string.IsNullOrEmpty(codeInputField.text) && codeInputField.text.Length > 2;
		});
		restoreGiftsButton.onClick.AddListener(RestoreGifts);
	}

	private void OnEnable()
	{
		codeInputField.text = "";
		submitButton.interactable = false;
		Dew.CallDelayed(delegate
		{
			codeInputField.ActivateInputField();
		});
	}

	private async void Submit()
	{
		if (!string.IsNullOrEmpty(codeInputField.text) && codeInputField.text.Length > 2 && await DewItem.RedeemCode(codeInputField.text))
		{
			codeInputField.text = "";
		}
	}

	private async void RestoreGifts()
	{
		if (await DewItem.RedeemCode("*"))
		{
			restoreGiftsButton.gameObject.SetActive(value: false);
		}
	}
}
