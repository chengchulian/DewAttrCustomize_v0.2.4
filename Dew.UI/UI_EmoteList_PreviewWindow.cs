using TMPro;
using UnityEngine;

public class UI_EmoteList_PreviewWindow : MonoBehaviour
{
	public float scaleMultiplier = 1.5f;

	public RectTransform previewParent;

	public GameObject shownObject;

	public TextMeshProUGUI emoteNameText;

	public TextMeshProUGUI emoteDescText;

	private Emote _shownInstance;

	private void Start()
	{
		UI_ToggleGroup tg = SingletonBehaviour<UI_EmoteList>.instance.GetComponentInChildren<UI_ToggleGroup>();
		tg.onCurrentIndexChanged.AddListener(UpdatePreviewWindow);
		UpdatePreviewWindow(tg.currentIndex);
	}

	private void UpdatePreviewWindow(int index)
	{
		if (index < 0 || index >= SingletonBehaviour<UI_EmoteList>.instance.shownItems.Count || !DewSave.profile.emotes[SingletonBehaviour<UI_EmoteList>.instance.shownItems[index].currentEmoteName].isUnlocked)
		{
			ShowEmpty();
			return;
		}
		if (_shownInstance != null)
		{
			Object.Destroy(_shownInstance.gameObject);
			_shownInstance = null;
		}
		shownObject.SetActive(value: true);
		UI_EmoteList_Item selectedItem = SingletonBehaviour<UI_EmoteList>.instance.shownItems[index];
		_shownInstance = Object.Instantiate(selectedItem.currentEmotePrefab, previewParent);
		_shownInstance.customDuration = float.PositiveInfinity;
		_shownInstance.transform.localScale *= scaleMultiplier;
		_shownInstance.posGetter = () => previewParent.GetScreenSpaceRect().center;
		emoteNameText.text = DewLocalization.GetUIValue(selectedItem.currentEmoteName + "_Name");
		emoteDescText.text = DewLocalization.GetUIValue(selectedItem.currentEmoteName + "_Description");
	}

	private void ShowEmpty()
	{
		if (_shownInstance != null)
		{
			Object.Destroy(_shownInstance.gameObject);
			_shownInstance = null;
		}
		shownObject.SetActive(value: false);
	}
}
