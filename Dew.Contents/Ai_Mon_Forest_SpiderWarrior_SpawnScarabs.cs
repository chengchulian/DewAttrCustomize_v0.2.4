using System.Collections;
using UnityEngine;

public class Ai_Mon_Forest_SpiderWarrior_SpawnScarabs : AbilityInstance
{
	public int numberOfScarabs;

	public float spawnInterval;

	public float radius;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			yield return new SI.WaitForSeconds(spawnInterval);
			for (int i = 0; i < numberOfScarabs; i++)
			{
				Vector3 vector = base.info.caster.position + Random.onUnitSphere * radius;
				vector = Dew.GetPositionOnGround(vector);
				vector = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, vector);
				Dew.SpawnEntity<Mon_Forest_Scarab>(vector, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), base.info.caster, base.info.caster.owner, base.info.caster.level);
				yield return new SI.WaitForSeconds(spawnInterval);
			}
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
