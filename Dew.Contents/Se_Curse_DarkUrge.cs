using System.Collections.Generic;
using System.Linq;

public class Se_Curse_DarkUrge : CurseStatusEffect
{
	private List<DewPlayer> _addedPlayers = new List<DewPlayer>();

	public float[] damageMultipliers;

	public float lowHpRatio;

	public float lowHpMultiplier;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer || base.victim.owner == null)
		{
			return;
		}
		base.victim.dealtDamageProcessor.Add(DamageBetweenPlayersProcessor);
		base.victim.takenDamageProcessor.Add(DamageBetweenPlayersProcessor);
		foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
		{
			if (!(humanPlayer == base.victim.owner))
			{
				_addedPlayers.Add(humanPlayer);
				base.victim.owner.neutrals.Add(humanPlayer);
				humanPlayer.neutrals.Add(base.victim.owner);
			}
		}
	}

	private void DamageBetweenPlayersProcessor(ref DamageData data, Actor actor, Entity to)
	{
		if (data.IsAmountModifiedBy(this))
		{
			return;
		}
		Hero hero = actor.FindFirstOfType<Hero>();
		if (!(hero == null) && !(to == null) && !(hero == to) && !(hero.owner == null) && !(to.owner == null) && hero.owner.isHumanPlayer && to.owner.isHumanPlayer && !(hero.owner == to.owner))
		{
			float num = GetValue(damageMultipliers);
			if (to.normalizedHealth < lowHpRatio)
			{
				num *= lowHpMultiplier;
			}
			data.ApplyRawMultiplier(num);
			data.SetAmountModifiedBy(this);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (!base.isServer || base.victim == null || base.victim.owner == null)
		{
			return;
		}
		base.victim.dealtDamageProcessor.Remove(DamageBetweenPlayersProcessor);
		foreach (DewPlayer addedPlayer in _addedPlayers)
		{
			base.victim.owner.neutrals.Remove(addedPlayer);
			addedPlayer.neutrals.Remove(base.victim.owner);
		}
	}

	public override bool IsViable(Entity target)
	{
		return DewPlayer.humanPlayers.Count((DewPlayer h) => !h.hero.IsNullInactiveDeadOrKnockedOut()) > 1;
	}

	private void MirrorProcessed()
	{
	}
}
