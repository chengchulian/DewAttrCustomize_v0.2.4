using System.Collections.Generic;
using System.Linq;

public class Se_GravityTraining : StatusEffect
{
	private AbilityLockHandle _handle;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		if (base.victim is Hero)
		{
			DoSlow(40f);
			return;
		}
		DoSlow(25f);
		base.victim.Control.StartDaze(0.35f);
		_handle = base.victim.Ability.GetNewAbilityLockHandle();
		KeyValuePair<int, AbilityTrigger>[] array = base.victim.Ability.abilities.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			KeyValuePair<int, AbilityTrigger> keyValuePair = array[i];
			if (keyValuePair.Value.currentConfig.selfValidator.isMovementAbility)
			{
				_handle.LockAbilityCast(keyValuePair.Key);
			}
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && _handle != null)
		{
			_handle.UnlockAllAbilitiesCast();
			_handle.Stop();
			_handle = null;
		}
	}

	private void MirrorProcessed()
	{
	}
}
