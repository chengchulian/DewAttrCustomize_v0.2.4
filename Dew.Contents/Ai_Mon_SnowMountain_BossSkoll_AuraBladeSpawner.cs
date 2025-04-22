using System.Collections;
using UnityEngine;

public class Ai_Mon_SnowMountain_BossSkoll_AuraBladeSpawner : AbilityInstance
{
	public GameObject telegraph;

	public int bladeCount;

	public float delay;

	public float interval;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		CreateBasicEffect(base.info.caster, new UnstoppableEffect(), delay + interval * (float)bladeCount);
		float rayCastLength = DewResources.GetByType<Ai_Mon_SnowMountain_BossSkoll_AuraBlade>().range.size.x * 0.5f + 5f;
		Vector3 finalPos = default(Vector3);
		Quaternion finalRot = default(Quaternion);
		RoomSection section = base.info.caster.section;
		if (section == null)
		{
			section = SingletonDewNetworkBehaviour<Room>.instance.GetFinalSection();
		}
		for (int i = 0; i < bladeCount; i++)
		{
			int num = 5;
			bool flag = false;
			for (int j = 0; j < num; j++)
			{
				finalPos = ((i < DewPlayer.humanPlayers.Count && !DewPlayer.humanPlayers[i].hero.isKnockedOut) ? DewPlayer.humanPlayers[i].hero.position : ((i < bladeCount - DewPlayer.humanPlayers.Count || DewPlayer.humanPlayers[i - bladeCount + DewPlayer.humanPlayers.Count].hero.isKnockedOut) ? (section.GetAnyRandomNode() + Random.insideUnitSphere.Flattened() * 1.5f) : AbilityTrigger.PredictPoint_Simple(Random.Range(0.7f, 1f), DewPlayer.humanPlayers[i - bladeCount + DewPlayer.humanPlayers.Count].hero, delay)));
				finalPos = Dew.GetPositionOnGround(finalPos);
				finalRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
				if (CheckValidPositionOnGround(ref finalPos, finalRot, rayCastLength))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				FxPlayNewNetworked(telegraph, finalPos, finalRot);
				CreateAbilityInstance(finalPos, finalRot, new CastInfo(base.info.caster, finalPos), delegate(Ai_Mon_SnowMountain_BossSkoll_AuraBlade b)
				{
					b.delay = delay;
				});
				yield return new SI.WaitForSeconds(interval);
			}
		}
		Destroy();
	}

	private bool CheckValidPositionOnGround(ref Vector3 pos, Quaternion rot, float raycastLength)
	{
		float num = 0f;
		float y = pos.y;
		while (num < raycastLength)
		{
			Vector3 vector = pos + rot * new Vector3(num, 0f, 0f);
			num += 1f;
			if (!(Mathf.Abs(y - Dew.GetPositionOnGround(vector).y) <= 2f))
			{
				pos += rot * new Vector3(num - raycastLength, 0f, 0f);
				break;
			}
		}
		num = 0f;
		while (num > 0f - raycastLength)
		{
			Vector3 vector2 = pos + rot * new Vector3(num, 0f, 0f);
			num -= 1f;
			if (!(Mathf.Abs(y - Dew.GetPositionOnGround(vector2).y) <= 2f))
			{
				return false;
			}
		}
		return true;
	}

	private void MirrorProcessed()
	{
	}
}
