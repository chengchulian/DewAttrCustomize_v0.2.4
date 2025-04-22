using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title_LanguageButton : MonoBehaviour, ILangaugeChangedCallback
{
	public TextMeshProUGUI text;

	public Image flag;

	private void Start()
	{
		OnLanguageChanged();
	}

	public void OnLanguageChanged()
	{
	}
}
