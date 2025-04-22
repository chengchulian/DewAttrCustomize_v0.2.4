using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title_SwitchLobbyServer : MonoBehaviour, ISettingsChangedCallback, ILangaugeChangedCallback
{
	public TextMeshProUGUI buttonText;

	public TextMeshProUGUI tooltipText;

	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(Switch);
	}

	private void Start()
	{
		UpdateText();
	}

	private void Switch()
	{
		DewSave.SavePlatformSettings();
		DewSave.ApplySettings();
	}

	public void OnSettingsChanged()
	{
		UpdateText();
	}

	public void OnLanguageChanged()
	{
		UpdateText();
	}

	private void UpdateText()
	{
	}
}
