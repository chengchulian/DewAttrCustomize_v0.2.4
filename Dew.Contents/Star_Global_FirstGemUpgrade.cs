using System;
using System.Collections;
using UnityEngine;

public class Star_Global_FirstGemUpgrade : DewStarItemOld
{
	public static readonly int[] BonusQuality = new int[3] { 30, 50, 80 };

	public override int maxLevel => 3;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		base.hero.Skill.ClientHeroEvent_OnGemEquip += new Action<Gem>(ClientHeroEventOnGemEquip);
	}

	private void ClientHeroEventOnGemEquip(Gem obj)
	{
		base.hero.Skill.ClientHeroEvent_OnGemEquip -= new Action<Gem>(ClientHeroEventOnGemEquip);
		base.hero.StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(0.5f);
			if (!obj.IsNullOrInactive() && !base.hero.IsNullInactiveDeadOrKnockedOut())
			{
				base.hero.CreateAbilityInstance(obj.position, null, new CastInfo(base.hero), delegate(Ai_FirstBonusQualityUpgrader ai)
				{
					ai.target = obj;
					ai.upgradeAmount = BonusQuality.GetClamped(base.level - 1);
				});
			}
		}
	}
}
