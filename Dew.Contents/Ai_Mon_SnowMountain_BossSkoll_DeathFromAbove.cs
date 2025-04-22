using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Ai_Mon_SnowMountain_BossSkoll_DeathFromAbove : AbilityInstance
{
	[Serializable]
	public class Pattern
	{
		public Vector3[] positions;
	}

	public float ascendTime;

	public float ascendHeight;

	public DewAnimationClip animAscend;

	public float delay0;

	public int patternSwordWaves = 3;

	public Pattern[] patterns;

	public Vector2Int countPerWave;

	public int atHeroSwordWavesSolo = 2;

	public int atHeroSwordWavesMultiplayerPerPlayer = 1;

	public float atHeroRandomMag;

	public Vector2 intervalPerSword;

	public float perWaveDelay;

	public float delay1;

	public GameObject fxFollowTelegraph;

	public float followDuration;

	public float followStartRandomMag;

	public float followSpeed;

	public DewAnimationClip animDescendFalling;

	public DewAnimationClip animLand;

	public float descendTime;

	public bool resetWhirlwind;

	public float postDaze;

	private EntityTransformModifier _entTransform;

	[SyncVar]
	private Vector3 _followPos;

	public Vector3 Network_followPos
	{
		get
		{
			return _followPos;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _followPos, 32uL, null);
		}
	}

	protected override IEnumerator OnCreateSequenced()
	{
		_entTransform = base.info.caster.Visual.GetNewTransformModifier();
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		CreateStatusEffect<Se_Mon_SnowMountain_BossSkoll_DeathFromAbove_Invulnerable>(base.info.caster).DestroyOnDestroy(this);
		base.info.caster.Control.IncrementBlockCounters(Channel.BlockedAction.Everything);
		RpcAscend();
		yield return new SI.WaitForSeconds(ascendTime + delay0);
		RefValue<int> lastWave = new RefValue<int>(-1);
		RefValue<Pattern> selectedPattern = new RefValue<Pattern>();
		RefValue<Quaternion> patternRot = new RefValue<Quaternion>();
		int patternOffsetIndex = global::UnityEngine.Random.Range(0, patterns.Length);
		RefValue<Vector3> pivot = new RefValue<Vector3>();
		yield return SwordRoutine(patternSwordWaves, delegate(int waveIndex, int swordIndex)
		{
			if ((int)lastWave != waveIndex)
			{
				lastWave.value = waveIndex;
				selectedPattern.value = patterns[(patternOffsetIndex + waveIndex) % patterns.Length];
				patternRot.value = Quaternion.Euler(0f, global::UnityEngine.Random.Range(0f, 360f), 0f);
				Hero hero = Dew.SelectRandomAliveHero();
				pivot.value = hero.agentPosition;
			}
			Vector3[] positions = selectedPattern.value.positions;
			return pivot.value + positions[swordIndex % positions.Length];
		});
		int playerCount = DewPlayer.humanPlayers.Count;
		foreach (DewPlayer h in DewPlayer.humanPlayers)
		{
			if (!h.hero.IsNullInactiveDeadOrKnockedOut())
			{
				int waves = ((playerCount > 1) ? atHeroSwordWavesMultiplayerPerPlayer : atHeroSwordWavesSolo);
				yield return SwordRoutine(waves, (int waveIndex, int swordIndex) => h.hero.agentPosition + global::UnityEngine.Random.onUnitSphere * atHeroRandomMag);
			}
		}
		yield return new SI.WaitForSeconds(delay1);
		Hero hero2 = Dew.SelectRandomAliveHero();
		Network_followPos = Dew.GetPositionOnGround(hero2.agentPosition + global::UnityEngine.Random.onUnitSphere * followStartRandomMag);
		Network_followPos = Dew.GetValidAgentPosition(_followPos);
		FxPlayNetworked(fxFollowTelegraph);
		for (float t = 0f; t < followDuration; t += 1f / 30f)
		{
			hero2 = Dew.GetClosestAliveHero(_followPos);
			Network_followPos = Dew.GetPositionOnGround(Vector3.MoveTowards(_followPos, hero2.agentPosition, followSpeed * (1f / 30f)));
			Teleport(base.info.caster, _followPos);
			yield return null;
		}
		RpcDescendAndFinish();
	}

	private IEnumerator SwordRoutine(int waves, Func<int, int, Vector3> posGetter)
	{
		for (int i = 0; i < waves; i++)
		{
			int swordCount = global::UnityEngine.Random.Range(countPerWave.x, countPerWave.y + 1);
			for (int j = 0; j < swordCount; j++)
			{
				Vector3 vector = FilterSwordPosition(posGetter(i, j));
				CreateAbilityInstance<Ai_Mon_SnowMountain_BossSkoll_Sword>(vector, Quaternion.Euler(0f, global::UnityEngine.Random.Range(0f, 360f), 0f), new CastInfo(base.info.caster));
				yield return new SI.WaitForSeconds(global::UnityEngine.Random.Range(intervalPerSword.x, intervalPerSword.y));
			}
			yield return new SI.WaitForSeconds(perWaveDelay);
		}
	}

	private Vector3 FilterSwordPosition(Vector3 input)
	{
		input = Dew.GetPositionOnGround(input);
		input = Dew.GetValidAgentDestination_Closest(base.info.caster.agentPosition, input);
		return input;
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		fxFollowTelegraph.transform.position = _followPos;
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (_entTransform != null)
		{
			_entTransform.Stop();
		}
		if (base.isServer && !base.info.caster.IsNullInactiveDeadOrKnockedOut())
		{
			base.info.caster.Control.DecrementBlockCounters(Channel.BlockedAction.Everything);
			base.info.caster.Control.StartDaze(postDaze);
			if (resetWhirlwind && base.info.caster.Ability.TryGetAbility<At_Mon_SnowMountain_BossSkoll_Whirlwind>(out var trigger))
			{
				trigger.ResetCooldown();
			}
		}
	}

	[ClientRpc]
	private void RpcAscend()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void Ai_Mon_SnowMountain_BossSkoll_DeathFromAbove::RpcAscend()", 1514687603, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcDescendAndFinish()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void Ai_Mon_SnowMountain_BossSkoll_DeathFromAbove::RpcDescendAndFinish()", 323676137, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcAscend()
	{
		StartSequence(Routine());
		IEnumerator Routine()
		{
			if (base.isServer)
			{
				base.info.caster.Animation.PlayAbilityAnimation(animAscend);
			}
			for (float t = 0f; t < ascendTime; t += 1f / 30f)
			{
				float num = t / ascendTime;
				_entTransform.worldOffset = Vector3.up * num * ascendHeight;
				_entTransform.scaleMultiplier = Vector3.one * (1f - num);
				yield return null;
			}
			_entTransform.worldOffset = Vector3.up * ascendHeight;
			_entTransform.scaleMultiplier = Vector3.zero;
			base.info.caster.Visual.DisableRenderersLocal();
		}
	}

	protected static void InvokeUserCode_RpcAscend(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcAscend called on server.");
		}
		else
		{
			((Ai_Mon_SnowMountain_BossSkoll_DeathFromAbove)obj).UserCode_RpcAscend();
		}
	}

	protected void UserCode_RpcDescendAndFinish()
	{
		StartSequence(Routine());
		IEnumerator Routine()
		{
			if (base.isServer)
			{
				base.info.caster.Control.Teleport(_followPos);
				base.info.caster.Animation.PlayAbilityAnimation(animDescendFalling);
				base.info.caster.Control.RotateTowards(Dew.GetClosestAliveHero(_followPos), immediately: true);
			}
			base.info.caster.Visual.EnableRenderersLocal();
			for (float t = 0f; t < descendTime; t += 1f / 30f)
			{
				float num = t / descendTime;
				_entTransform.worldOffset = Vector3.up * ((1f - num) * ascendHeight);
				_entTransform.scaleMultiplier = Vector3.one * num;
				yield return null;
			}
			_entTransform.worldOffset = Vector3.zero;
			_entTransform.scaleMultiplier = Vector3.one;
			if (base.isServer)
			{
				base.info.caster.Animation.PlayAbilityAnimation(animLand);
				CreateAbilityInstance<Ai_Mon_SnowMountain_BossSkoll_DeathFromAbove_Land>(base.info.caster.agentPosition, null, new CastInfo(base.info.caster));
				base.info.caster.Control.StartDaze(postDaze);
				Destroy();
			}
		}
	}

	protected static void InvokeUserCode_RpcDescendAndFinish(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcDescendAndFinish called on server.");
		}
		else
		{
			((Ai_Mon_SnowMountain_BossSkoll_DeathFromAbove)obj).UserCode_RpcDescendAndFinish();
		}
	}

	static Ai_Mon_SnowMountain_BossSkoll_DeathFromAbove()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Ai_Mon_SnowMountain_BossSkoll_DeathFromAbove), "System.Void Ai_Mon_SnowMountain_BossSkoll_DeathFromAbove::RpcAscend()", InvokeUserCode_RpcAscend);
		RemoteProcedureCalls.RegisterRpc(typeof(Ai_Mon_SnowMountain_BossSkoll_DeathFromAbove), "System.Void Ai_Mon_SnowMountain_BossSkoll_DeathFromAbove::RpcDescendAndFinish()", InvokeUserCode_RpcDescendAndFinish);
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteVector3(_followPos);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteVector3(_followPos);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _followPos, null, reader.ReadVector3());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _followPos, null, reader.ReadVector3());
		}
	}
}
