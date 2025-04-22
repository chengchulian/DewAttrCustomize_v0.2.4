public class UI_Tooltip_ObjLevelExpObject : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult && currentObjects[1] is int && currentObjects[2] is GemLocation)
		{
			base.gameObject.SetActive(value: true);
		}
		else if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult && currentObjects[1] is int && currentObjects[2] is HeroSkillLocation)
		{
			base.gameObject.SetActive(value: true);
		}
		else if (base.currentObject is SkillTrigger { isLevelUpEnabled: not false })
		{
			base.gameObject.SetActive(value: true);
		}
		else if (base.currentObject is Gem)
		{
			base.gameObject.SetActive(value: true);
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
