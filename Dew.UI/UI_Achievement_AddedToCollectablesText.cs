using System;
using TMPro;
using UnityEngine;

public class UI_Achievement_AddedToCollectablesText : MonoBehaviour
{
	public void Setup(Type type)
	{
		TextMeshProUGUI t = GetComponent<TextMeshProUGUI>();
		if (type.IsSubclassOf(typeof(Gem)))
		{
			t.text = DewLocalization.GetUIValue("Achievement_Unlocked_Essence");
		}
		else if (type.IsSubclassOf(typeof(SkillTrigger)))
		{
			t.text = DewLocalization.GetUIValue("Achievement_Unlocked_Skill");
		}
		else if (type.IsSubclassOf(typeof(Hero)))
		{
			t.text = DewLocalization.GetUIValue("Achievement_Unlocked_Hero");
		}
		else if (type.IsSubclassOf(typeof(LucidDream)))
		{
			t.text = DewLocalization.GetUIValue("Achievement_Unlocked_LucidDream");
		}
		else
		{
			t.text = "";
		}
	}

	public void Setup(global::UnityEngine.Object target)
	{
		Setup(target.GetType());
	}
}
