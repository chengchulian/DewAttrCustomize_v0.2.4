using System.Collections;
using UnityEngine;

public class Ai_Mon_Sky_BossNyx_Starfall : AbilityInstance
{
	public int starfallWavaCount;

	public int starfallCount;

	public float starfallDelay;

	public float starfallInterval;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		RoomSection section = base.info.caster.section;
		if (section == null)
		{
			section = SingletonDewNetworkBehaviour<Room>.instance.GetFinalSection();
		}
		if (section == null)
		{
			Destroy();
			yield break;
		}
		for (int i = 0; i < starfallWavaCount; i++)
		{
			for (int j = 0; j < starfallCount; j++)
			{
				Vector3 vector = ((j < DewPlayer.humanPlayers.Count && !DewPlayer.humanPlayers[j].hero.isKnockedOut) ? (DewPlayer.humanPlayers[j].hero.position + Random.insideUnitSphere.Flattened() * 4.5f) : ((j < starfallCount - DewPlayer.humanPlayers.Count || DewPlayer.humanPlayers[j - starfallCount + DewPlayer.humanPlayers.Count].hero.isKnockedOut) ? (section.GetAnyRandomNode() + Random.insideUnitSphere.Flattened() * 1.5f) : (AbilityTrigger.PredictPoint_Simple(Random.Range(0.7f, 1f), DewPlayer.humanPlayers[j - starfallCount + DewPlayer.humanPlayers.Count].hero, starfallDelay) + Random.insideUnitSphere.Flattened() * 4.5f)));
				vector = Dew.GetPositionOnGround(vector);
				CreateAbilityInstance<Ai_Mon_Sky_BossNyx_Starfall_Instance>(vector, null, new CastInfo(base.info.caster, vector));
				yield return new SI.WaitForSeconds(starfallInterval);
			}
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
