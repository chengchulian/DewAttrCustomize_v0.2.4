using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Collectables_ObjectItem : UI_GamepadFocusable, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public Image[] rarityImages;

	public Image icon;

	public GameObject lockedObject;

	public GameObject notDiscoveredObject;

	public Transform tooltipPivot;

	public GameObject newObject;

	public global::UnityEngine.Object targetObj { get; private set; }

	public UI_Collectables_ObjectItemProvider provider { get; private set; }

	public virtual void Setup(global::UnityEngine.Object obj, int index)
	{
		provider = GetComponent<UI_Collectables_ObjectItemProvider>();
		provider.targetObj = obj;
		targetObj = obj;
		UnlockStatus status = provider.GetUnlockData().status;
		lockedObject.SetActive(status == UnlockStatus.Locked);
		notDiscoveredObject.SetActive(status == UnlockStatus.NotDiscovered);
		icon.sprite = provider.GetIcon();
		icon.color = ((status != UnlockStatus.Complete) ? Color.black : Color.white);
		GetComponent<UI_Toggle>().index = index;
		UpdateNewStatus();
		Image[] array = rarityImages;
		foreach (Image r in array)
		{
			Color color = provider.GetRarityColor();
			color.a = r.color.a;
			r.color = color;
		}
		provider.OnSetup(obj, status, index);
	}

	private void UpdateNewStatus()
	{
		DewProfile.UnlockData data = provider.GetUnlockData();
		newObject.SetActive(data.status == UnlockStatus.Complete && !data.didReadMemory);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!(provider == null))
		{
			DewProfile.UnlockData data = provider.GetUnlockData();
			if (data.status == UnlockStatus.Complete)
			{
				data.didReadMemory = true;
			}
			UpdateNewStatus();
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		Func<Vector2> posGetter = () => tooltipPivot.position;
		if (targetObj is SkillTrigger skill)
		{
			switch (DewSave.profile.skills[skill.GetType().Name].status)
			{
			case UnlockStatus.Locked:
			case UnlockStatus.Complete:
				tooltip.ShowCollectableTooltip(posGetter, skill.GetType());
				break;
			case UnlockStatus.NotDiscovered:
				tooltip.ShowNotDiscoveredObjectTooltip(posGetter);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		if (targetObj is Gem gem)
		{
			switch (DewSave.profile.gems[gem.GetType().Name].status)
			{
			case UnlockStatus.Locked:
			case UnlockStatus.Complete:
				tooltip.ShowCollectableTooltip(posGetter, gem.GetType());
				break;
			case UnlockStatus.NotDiscovered:
				tooltip.ShowNotDiscoveredObjectTooltip(posGetter);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		if (targetObj is Artifact a)
		{
			switch (DewSave.profile.artifacts[a.GetType().Name].status)
			{
			case UnlockStatus.Locked:
			case UnlockStatus.NotDiscovered:
				tooltip.ShowNotDiscoveredObjectTooltip(posGetter);
				break;
			case UnlockStatus.Complete:
				tooltip.ShowArtifactTooltip(posGetter, a);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	public override void OnFocusStateChanged(bool state)
	{
		base.OnFocusStateChanged(state);
		if (state)
		{
			OnPointerClick(null);
			GetComponent<UI_Toggle>().isChecked = true;
		}
	}
}
