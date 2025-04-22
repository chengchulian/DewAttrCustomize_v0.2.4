using System;

public class UI_Lobby_SelectedHeroInfo_HeroStat : UI_Lobby_SelectedHeroInfo_Base
{
	public enum ShownStat
	{
		MaxHealth,
		AttackDamage,
		AttackSpeed,
		AbilityPower,
		Armor
	}

	public string template;

	public ShownStat type;

	protected override void OnHeroChanged()
	{
		base.OnHeroChanged();
		if (base.selectedHero == null)
		{
			return;
		}
		EntityStatus s = base.selectedHero.GetComponent<EntityStatus>();
		string scaling = "";
		float stat;
		switch (type)
		{
		case ShownStat.MaxHealth:
			stat = s.baseStats.maxHealth;
			if (s.scalingStats.maxHealthPercentage > 0f)
			{
				scaling = $"+{s.scalingStats.maxHealthPercentage:#,##0}%";
			}
			break;
		case ShownStat.AttackDamage:
			stat = s.baseStats.attackDamage;
			if (s.scalingStats.attackDamageFlat > 0f)
			{
				scaling = $"+{s.scalingStats.attackDamageFlat:#,##0}";
			}
			break;
		case ShownStat.AttackSpeed:
			stat = 1f / base.selectedHero.GetComponent<EntityAbility>().attackAbilityPreset.configs[0].cooldownTime;
			break;
		case ShownStat.AbilityPower:
			stat = s.baseStats.abilityPower;
			if (s.scalingStats.abilityPowerFlat > 0f)
			{
				scaling = $"+{s.scalingStats.abilityPowerFlat:#,##0}";
			}
			break;
		case ShownStat.Armor:
			stat = s.baseStats.armor;
			if (s.scalingStats.armorFlat > 0f)
			{
				scaling = $"+{s.scalingStats.armorFlat:#,##0}";
			}
			base.gameObject.SetActive(stat > 0f);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		base.text.text = string.Format(template, stat);
		if (!string.IsNullOrEmpty(scaling))
		{
			base.text.text = base.text.text + " <size=85%><sprite=5><color=yellow>(" + scaling + ")</color>";
		}
	}
}
