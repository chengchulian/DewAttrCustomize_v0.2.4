using TMPro;
using UnityEngine;

public class UI_NametagList_PreviewWindow : MonoBehaviour
{
	public RectTransform nametagContainer;

	public GameObject shownObject;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI descText;

	private Nametag _shownInstance;

	private void Start()
	{
		UI_ToggleGroup tg = SingletonBehaviour<UI_NametagList>.instance.GetComponentInChildren<UI_ToggleGroup>();
		tg.onCurrentIndexChanged.AddListener(UpdatePreviewWindow);
		UpdatePreviewWindow(tg.currentIndex);
	}

	private void UpdatePreviewWindow(int index)
	{
		if (index < 0 || index >= SingletonBehaviour<UI_NametagList>.instance.shownItems.Count || !DewSave.profile.nametags[SingletonBehaviour<UI_NametagList>.instance.shownItems[index].nametagName].isUnlocked)
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
		UI_NametagList_Item selectedItem = SingletonBehaviour<UI_NametagList>.instance.shownItems[index];
		Nametag prefab = DewResources.GetByName<Nametag>(selectedItem.nametagName);
		_shownInstance = Object.Instantiate(prefab, nametagContainer);
		_shownInstance.Setup(nametagContainer, isIcon: false);
		nameText.text = DewLocalization.GetUIValue(selectedItem.nametagName + "_Name");
		descText.text = DewLocalization.GetUIValue(selectedItem.nametagName + "_Description");
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
