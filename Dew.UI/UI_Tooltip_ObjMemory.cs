using System.Collections;
using UnityEngine;

public class UI_Tooltip_ObjMemory : UI_Tooltip_BaseObj
{
	public bool doAnimate = true;

	public override void OnSetup()
	{
		base.OnSetup();
		if (base.currentObject is SkillTrigger skill)
		{
			text.text = DewLocalization.GetSkillMemory(DewLocalization.GetSkillKey(skill.GetType()));
		}
		else if (base.currentObject is Gem gem)
		{
			text.text = DewLocalization.GetGemMemory(DewLocalization.GetGemKey(gem.GetType()));
		}
		else if (base.currentObject is Artifact a)
		{
			text.text = DewLocalization.GetArtifactStory(DewLocalization.GetArtifactKey(a.GetType()));
		}
		text.maxVisibleCharacters = ((!doAnimate) ? int.MaxValue : 0);
	}

	private void OnEnable()
	{
		StartCoroutine(AnimateRoutine());
		IEnumerator AnimateRoutine()
		{
			while (true)
			{
				if (text != null && text.text != null && text.maxVisibleCharacters < text.text.Length)
				{
					text.maxVisibleCharacters += 12;
				}
				yield return new WaitForSecondsRealtime(1f / 30f);
			}
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}
}
