using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class At_Mon_Special_BossLightElemental_Summon : AbilityTrigger
{
	public float maxPopulation = 6f;

	public float cooldownMultOnNoPopulation;

	public float cooldownMultOnMaxPopulation;

	[SyncVar]
	private float _currentCooldownMult;

	public float Network_currentCooldownMult
	{
		get
		{
			return _currentCooldownMult;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _currentCooldownMult, 256uL, null);
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer)
		{
			float populationCost = ((Monster)base.owner).populationCost;
			float num = maxPopulation * NetworkedManagerBase<GameManager>.instance.difficulty.maxPopulationMultiplier - populationCost;
			float t = (NetworkedManagerBase<GameManager>.instance.spawnedPopulation - populationCost) / num;
			Network_currentCooldownMult = Mathf.Lerp(cooldownMultOnNoPopulation, cooldownMultOnMaxPopulation, t);
		}
	}

	public override float GetCooldownTimeMultiplier(int configIndex)
	{
		return base.GetCooldownTimeMultiplier(configIndex) * _currentCooldownMult;
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(_currentCooldownMult);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x100L) != 0L)
		{
			writer.WriteFloat(_currentCooldownMult);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _currentCooldownMult, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x100L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _currentCooldownMult, null, reader.ReadFloat());
		}
	}
}
