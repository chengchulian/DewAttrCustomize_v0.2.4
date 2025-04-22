using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Guide_ListItem : MonoBehaviour, ILangaugeChangedCallback
{
	public TextMeshProUGUI text;

	public GameObject unreadObject;

	private void Start()
	{
		GetComponentInParent<UI_Guide_View>().onShow.AddListener(delegate
		{
			unreadObject.SetActive(!DewSave.profile.seenGuides.Contains(base.name));
		});
		GetComponentInChildren<Button>().onClick.AddListener(delegate
		{
			GetComponentInParent<UI_Guide_View>().ShowItem(base.name);
			unreadObject.SetActive(value: false);
		});
		text.text = DewLocalization.GetUIValue(base.name);
	}

	public void OnLanguageChanged()
	{
		text.text = DewLocalization.GetUIValue(base.name);
	}
}
