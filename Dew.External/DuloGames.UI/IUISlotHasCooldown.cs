namespace DuloGames.UI;

public interface IUISlotHasCooldown
{
	UISlotCooldown cooldownComponent { get; }

	UISpellInfo GetSpellInfo();

	void SetCooldownComponent(UISlotCooldown cooldown);
}
