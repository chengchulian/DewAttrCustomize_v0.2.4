using System;
using System.Collections;

public class LucidDream_HarmlessWhispers : LucidDream
{
	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			NetworkedManagerBase<ActorManager>.instance.onEntityAdd += new Action<Entity>(OnEntityAdd);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && NetworkedManagerBase<ActorManager>.instance != null)
		{
			NetworkedManagerBase<ActorManager>.instance.onEntityAdd -= new Action<Entity>(OnEntityAdd);
		}
	}

	private void OnEntityAdd(Entity obj)
	{
		if (obj is Monster)
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			do
			{
				yield return null;
				if (obj.IsNullInactiveDeadOrKnockedOut())
				{
					yield break;
				}
			}
			while (obj.Visual.isSpawning);
			if (obj.IsAnyBoss() && !(obj.Control.baseAgentSpeed < 0.001f))
			{
				CreateStatusEffect<Se_HarmlessWhispers>(obj, new CastInfo(null));
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
