using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Achievement_Icon : MonoBehaviour
{
	public GameObject gemObject;

	public GameObject skillObject;

	public GameObject heroObject;

	public GameObject lucidDreamObject;

	public Image gemImage;

	public Image skillImage;

	public UI_HeroIcon heroIcon;

	public UI_LucidDreamIcon lucidDreamIcon;

	public Image rarityImage;

	public void SetupByItem(global::UnityEngine.Object target)
	{
		if (target is Gem g)
		{
			gemObject.SetActive(value: true);
			skillObject.SetActive(value: false);
			heroObject.SetActive(value: false);
			lucidDreamObject.SetActive(value: false);
			gemImage.sprite = g.icon;
			rarityImage.color = Dew.GetRarityColor(g.rarity);
		}
		else if (target is Artifact a)
		{
			gemObject.SetActive(value: true);
			skillObject.SetActive(value: false);
			heroObject.SetActive(value: false);
			lucidDreamObject.SetActive(value: false);
			gemImage.sprite = a.icon;
			rarityImage.color = a.mainColor.WithA(1f);
		}
		else if (target is SkillTrigger s)
		{
			gemObject.SetActive(value: false);
			skillObject.SetActive(value: true);
			heroObject.SetActive(value: false);
			lucidDreamObject.SetActive(value: false);
			skillImage.sprite = s.configs[0].triggerIcon;
			rarityImage.color = Dew.GetRarityColor(s.rarity);
		}
		else if (target is Hero h)
		{
			gemObject.SetActive(value: false);
			skillObject.SetActive(value: false);
			heroObject.SetActive(value: true);
			lucidDreamObject.SetActive(value: false);
			heroIcon.Setup(h.GetType().Name);
			rarityImage.color = Color.clear;
		}
		else if (target is LucidDream d)
		{
			gemObject.SetActive(value: false);
			skillObject.SetActive(value: false);
			heroObject.SetActive(value: false);
			lucidDreamObject.SetActive(value: true);
			lucidDreamIcon.Setup(d.GetType().Name);
			rarityImage.color = d.color;
		}
		else
		{
			gemObject.SetActive(value: false);
			skillObject.SetActive(value: false);
			heroObject.SetActive(value: false);
			lucidDreamObject.SetActive(value: false);
		}
	}

	public void Setup(Type achType)
	{
		List<Type> unlocked = Dew.GetUnlockedTargetsOfAchievement(achType);
		if (unlocked.Count > 0)
		{
			global::UnityEngine.Object target = DewResources.GetByType(unlocked[0]);
			SetupByItem(target);
		}
	}
}
