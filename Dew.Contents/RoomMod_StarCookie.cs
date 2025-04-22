using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMod_StarCookie : RoomModifierBase
{
	public int spawnedCount;

	public float initDelay;

	public int activeSpawnCount;

	public float spawnRadius;

	public Vector2 interval;

	public override void OnStartServer()
	{
		base.OnStartServer();
		if (!base.isNewInstance)
		{
			return;
		}
		GameManager.CallOnReady(delegate
		{
			foreach (RoomSection section in SingletonDewNetworkBehaviour<Room>.instance.sections)
			{
				for (int i = 0; i < spawnedCount / 2; i++)
				{
					section.TryGetGoodNodePosition(out var pos);
					CreateActor<Shrine_StarCookie>(pos, null);
				}
			}
			StartCoroutine(SpawnStarCookieRoutine());
		});
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			StopAllCoroutines();
		}
	}

	private IEnumerator SpawnStarCookieRoutine()
	{
		yield return new WaitForSeconds(Random.Range(initDelay / 2f, initDelay));
		int heroCount = 0;
		IList<DewPlayer> humanPlayers = DewPlayer.humanPlayers;
		for (int i = 0; i < humanPlayers.Count; i++)
		{
			if (!humanPlayers[i].hero.IsNullInactiveDeadOrKnockedOut())
			{
				heroCount++;
			}
		}
		for (int j = 0; j < Random.Range(1, activeSpawnCount * heroCount); j++)
		{
			Hero hero = Dew.SelectRandomAliveHero();
			Vector3 normalized = Random.insideUnitCircle.ToXZ().normalized;
			Vector3 vector = hero.agentPosition + normalized * Random.Range(0f, spawnRadius);
			vector = Dew.GetPositionOnGround(vector);
			CreateAbilityInstance<Ai_StarCookie_Projectile>(vector, null, default(CastInfo));
			yield return new WaitForSeconds(Random.Range(interval.x, interval.y));
		}
	}

	private void MirrorProcessed()
	{
	}
}
