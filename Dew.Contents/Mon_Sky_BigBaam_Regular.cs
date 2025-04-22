using System;

public class Mon_Sky_BigBaam_Regular : Mon_Sky_BigBaam_Base, ISpawnableAsMiniBoss
{
	public void OnBeforeSpawnAsMiniBoss()
	{
	}

	public void OnCreateAsMiniBoss()
	{
		if (!base.isServer)
		{
			return;
		}
		ISpawnableAsMiniBoss.GiveGenericMiniBossBonus(this);
		base.Ability.GetAbility<At_Mon_Sky_BigBaam_BeamAtk>().spawnAdditionalProjectile = true;
		ActorEvent_OnAbilityInstanceBeforePrepare += (Action<EventInfoAbilityInstance>)delegate(EventInfoAbilityInstance instance)
		{
			if (instance.instance is Ai_Mon_Sky_BigBaam_BeamAtk ai_Mon_Sky_BigBaam_BeamAtk)
			{
				ai_Mon_Sky_BigBaam_BeamAtk.dealtDamageProcessor.Add(delegate(ref DamageData data, Actor actor, Entity target)
				{
					data.ApplyRawMultiplier(0.6f);
				});
			}
		};
	}

	private void MirrorProcessed()
	{
	}
}
