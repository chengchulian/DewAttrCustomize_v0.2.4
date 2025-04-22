using UnityEngine;

public class Ai_Mon_Special_AmbushSpawner_Spawn : AbilityInstance
{
	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		AbilityTrigger abilityTrigger = base.firstTrigger;
		At_Mon_Special_AmbushSpawner_Spawn trg = abilityTrigger as At_Mon_Special_AmbushSpawner_Spawn;
		if ((object)trg != null)
		{
			int num = 1;
			if (trg.nextWaveIndex == 0)
			{
				num = 3;
			}
			if (trg.nextWaveIndex == 1)
			{
				num = 3;
			}
			if (trg.nextWaveIndex == 2)
			{
				num = 2;
			}
			if (trg.nextWaveIndex == 3)
			{
				num = 2;
			}
			if (trg.nextWaveIndex == 4)
			{
				num = 2;
			}
			if (trg.nextWaveIndex == 5)
			{
				num = 2;
			}
			if (trg.nextWaveIndex > 10 && trg.nextWaveIndex % 10 == 0)
			{
				num = Random.Range(2, 10);
			}
			for (int i = 0; i < num; i++)
			{
				CreateAbilityInstance(base.position, null, new CastInfo(base.info.caster), delegate(Ai_Mon_Special_AmbushSpawner_Projectile ai)
				{
					ai._waveIndex = trg.nextWaveIndex;
				});
			}
			trg.nextWaveIndex++;
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
