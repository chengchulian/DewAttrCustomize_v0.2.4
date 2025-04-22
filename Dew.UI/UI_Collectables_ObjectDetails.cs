using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Collectables_ObjectDetails : MonoBehaviour
{
	public GameObject showObject;

	public GameObject[] skillObjects;

	public GameObject[] gemObjects;

	public GameObject[] artifactObjects;

	public GameObject achievementObject;

	public TextMeshProUGUI achNameText;

	public TextMeshProUGUI achDescText;

	public ScrollRect scrollRect;

	public void Show(global::UnityEngine.Object obj)
	{
		Clear();
		showObject.SetActive(value: true);
		GameObject[] array = skillObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(obj is SkillTrigger);
		}
		array = gemObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(obj is Gem);
		}
		array = artifactObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(obj is Artifact);
		}
		UI_Tooltip_BaseObj[] objs = GetComponentsInChildren<UI_Tooltip_BaseObj>(includeInactive: true);
		object[] data = ((!(obj is Gem)) ? new object[1] { obj } : new object[2] { obj, 101 });
		UI_Tooltip_BaseObj[] array2 = objs;
		foreach (UI_Tooltip_BaseObj obj2 in array2)
		{
			obj2.currentObjects = data;
			obj2.OnSetup();
		}
		Type ach = Dew.GetRequiredAchievementOfTarget(obj.GetType());
		if (ach == null)
		{
			achievementObject.SetActive(value: false);
		}
		else
		{
			achievementObject.SetActive(value: true);
			achNameText.text = DewLocalization.GetAchievementName(ach.Name);
			achDescText.text = DewLocalization.GetAchievementDescription(ach.Name);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)base.transform);
		scrollRect.normalizedPosition = Vector2.one;
	}

	private void Update()
	{
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			float value = Mathf.Clamp(DewInput.GetRightJoystick().y, -1f, 1f);
			scrollRect.content.transform.position += Vector3.down * (value * 600f * Time.unscaledDeltaTime);
		}
	}

	public void Clear()
	{
		GameObject[] array = skillObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		array = gemObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		array = artifactObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		showObject.SetActive(value: false);
	}
}
