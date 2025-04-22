using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AccList_Item : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IShowTooltip, IPointerEnterHandler, IPointerExitHandler
{
	public GameObject[] typeObjects;

	public Transform tooltipPivot;

	public Image previewImage;

	public GameObject lockedObject;

	public GameObject newObject;

	public GameObject inUseObject;

	private UI_Toggle _toggle;

	public string accName { get; private set; }

	private void Start()
	{
		_toggle = GetComponentInChildren<UI_Toggle>();
		newObject.SetActive(DewSave.profile.accessories[accName].isNew);
		lockedObject.SetActive(!DewSave.profile.accessories[accName].isUnlocked);
		inUseObject.SetActive(value: false);
	}

	public void Setup(string acc)
	{
		Accessory prefab = DewResources.GetByName<Accessory>(acc);
		DewProfile.CosmeticsData data;
		bool isUnlocked = DewSave.profile.accessories.TryGetValue(acc, out data) && data.isUnlocked;
		if (!isUnlocked)
		{
			typeObjects.SetActiveAll(value: false);
		}
		else
		{
			for (int i = 0; i < typeObjects.Length; i++)
			{
				typeObjects[i].SetActive(i == (int)prefab.type);
			}
		}
		accName = acc;
		previewImage.sprite = prefab.previewImage;
		previewImage.color = (isUnlocked ? Color.white : Color.black);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		DewSave.profile.accessories[accName].isNew = false;
		newObject.SetActive(value: false);
		SingletonBehaviour<UI_AccList>.instance.UpdateNewStatus();
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		Func<Vector2> posGetter = () => tooltipPivot.transform.position;
		if (DewSave.profile.accessories[accName].isUnlocked)
		{
			tooltip.ShowSouvenirTooltip(posGetter, accName);
		}
		else
		{
			tooltip.ShowRawTextTooltip(posGetter, "???\n" + DewLocalization.GetUIValue("Collectables_YouHaveNotDiscoveredThisYet"));
		}
	}
}
