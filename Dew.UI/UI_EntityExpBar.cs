public class UI_EntityExpBar : UI_EntityBar
{
	protected override float GetFillAmount()
	{
		if (!(base.target is Hero hero) || NetworkedManagerBase<GameManager>.softInstance == null)
		{
			return 0f;
		}
		if (hero.maxExp == 0)
		{
			return 1f;
		}
		return (float)hero.exp / (float)hero.maxExp;
	}
}
