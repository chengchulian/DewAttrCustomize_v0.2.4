using System.Collections.Generic;

public class Ai_Mon_SnowMountain_VikingWarrior_Atk : InstantDamageInstance
{
	public float stunDuration;

	private TriggerConfig _config;

	private float _postDelay;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			AbilityTrigger abilityTrigger = base.firstTrigger;
			_config = abilityTrigger.currentConfig;
			_postDelay = _config.postDelay;
		}
	}

	protected override void OnHit(Entity entity)
	{
		_config.postDelay = 0.5f;
		CreateBasicEffect(entity, new StunEffect(), stunDuration, "viking_stun");
		foreach (KeyValuePair<int, AbilityTrigger> ability in base.info.caster.Ability.abilities)
		{
			if (ability.Key != base.info.caster.Ability.attackAbility.abilityIndex)
			{
				ability.Value.ResetCooldown();
			}
		}
		base.OnHit(entity);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && _config != null)
		{
			_config.postDelay = _postDelay;
		}
	}

	private void MirrorProcessed()
	{
	}
}
