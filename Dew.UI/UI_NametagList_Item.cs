using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_NametagList_Item : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IShowTooltip, IPointerEnterHandler, IPointerExitHandler
{
	public RectTransform nametagContainer;

	public Transform tooltipPivot;

	public GameObject lockedObject;

	public GameObject newObject;

	public GameObject inUseObject;

	private UI_Toggle _toggle;

	private Nametag _currentNametag;

	public string nametagName { get; private set; }

	private void Start()
	{
		_toggle = GetComponentInChildren<UI_Toggle>();
		newObject.SetActive(DewSave.profile.nametags[nametagName].isNew);
		lockedObject.SetActive(!DewSave.profile.nametags[nametagName].isUnlocked);
		inUseObject.SetActive(value: false);
	}

	public void Setup(string ntName)
	{
		if (_currentNametag != null)
		{
			global::UnityEngine.Object.Destroy(_currentNametag.gameObject);
			_currentNametag = null;
		}
		Nametag prefab = DewResources.GetByName<Nametag>(ntName);
		DewProfile.CosmeticsData data;
		bool num = DewSave.profile.nametags.TryGetValue(ntName, out data) && data.isUnlocked;
		nametagName = ntName;
		_currentNametag = global::UnityEngine.Object.Instantiate(prefab, nametagContainer);
		_currentNametag.Setup(nametagContainer, isIcon: true);
		if (!num)
		{
			Image[] componentsInChildren = _currentNametag.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = Color.black;
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		DewSave.profile.nametags[nametagName].isNew = false;
		newObject.SetActive(value: false);
		if (UI_Lobby_NametagList.instance != null)
		{
			UI_Lobby_NametagList.instance.UpdateNewStatus();
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		Func<Vector2> posGetter = () => tooltipPivot.transform.position;
		if (DewSave.profile.nametags[nametagName].isUnlocked)
		{
			tooltip.ShowRawTextTooltip(posGetter, DewLocalization.GetUIValue(nametagName + "_Name"));
		}
		else
		{
			tooltip.ShowRawTextTooltip(posGetter, "???\n" + DewLocalization.GetUIValue("Collectables_YouHaveNotDiscoveredThisYet"));
		}
	}
}
