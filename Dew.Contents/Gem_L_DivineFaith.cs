using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Gem_L_DivineFaith : Gem
{
	public float dmgAmpPerStack;

	public float maxHealthPerStack;

	public ScalingValue maxStacks;

	[SyncVar]
	public int currentStack;

	public float gracePeriod = 6f;

	public GameObject fxStackUp;

	private KillTracker _tracker;

	private StatBonus _bonus;

	public int NetworkcurrentStack
	{
		get
		{
			return currentStack;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref currentStack, 8192uL, null);
		}
	}

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			_bonus = new StatBonus();
			newOwner.Status.AddStatBonus(_bonus);
			_bonus.maxHealthFlat = maxHealthPerStack * (float)currentStack;
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer && oldOwner != null)
		{
			oldOwner.Status.RemoveStatBonus(_bonus);
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (base.isServer && !(base.owner == null))
		{
			base.numberDisplay = Mathf.RoundToInt(currentStack);
		}
	}

	private void IncreaseAmp(EventInfoKill obj)
	{
		if (obj.victim is Monster)
		{
			int num = Mathf.RoundToInt(GetValue(maxStacks));
			if (currentStack < num)
			{
				NetworkcurrentStack = Mathf.Clamp(currentStack + 1, 0, num);
				_bonus.maxHealthFlat = maxHealthPerStack * (float)currentStack;
				ShowStackUp(base.owner.owner, obj.victim.position);
				NotifyUse();
			}
		}
	}

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			_tracker = newSkill.TrackKills(gracePeriod, IncreaseAmp);
			newSkill.dealtDamageProcessor.Add(Amplify);
			newSkill.dealtHealProcessor.Add(Amplify);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer)
		{
			if (oldSkill != null)
			{
				oldSkill.dealtDamageProcessor.Remove(Amplify);
				oldSkill.dealtHealProcessor.Remove(Amplify);
			}
			_tracker.Stop();
		}
	}

	private void Amplify(ref DamageData data, Actor actor, Entity target)
	{
		if (!data.IsAmountModifiedBy(this) && base.owner.CheckEnemyOrNeutral(target))
		{
			data.ApplyAmplification((float)currentStack * dmgAmpPerStack);
			data.SetAmountModifiedBy(this);
		}
	}

	private void Amplify(ref HealData data, Actor actor, Entity target)
	{
		if (!data.IsAmountModifiedBy(this) && !base.owner.CheckEnemyOrNeutral(target))
		{
			data.ApplyAmplification((float)currentStack * dmgAmpPerStack);
			data.SetAmountModifiedBy(this);
		}
	}

	protected override void OnQualityChange(int oldQuality, int newQuality)
	{
		base.OnQualityChange(oldQuality, newQuality);
		if (base.isServer)
		{
			int num = Mathf.RoundToInt(GetValue(maxStacks));
			if (currentStack > num)
			{
				NetworkcurrentStack = num;
			}
		}
	}

	[TargetRpc]
	public void ShowStackUp(NetworkConnectionToClient conn, Vector3 pos)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(pos);
		SendTargetRPCInternal(conn, "System.Void Gem_L_DivineFaith::ShowStackUp(Mirror.NetworkConnectionToClient,UnityEngine.Vector3)", -1268230581, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_ShowStackUp__NetworkConnectionToClient__Vector3(NetworkConnectionToClient conn, Vector3 pos)
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(0.25f);
			FxPlayNew(fxStackUp, pos, null);
			InGameUIManager.instance.ShowWorldPopMessage(new WorldMessageSetting
			{
				rawText = "+1",
				color = Color.white,
				worldPos = pos
			});
		}
	}

	protected static void InvokeUserCode_ShowStackUp__NetworkConnectionToClient__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC ShowStackUp called on server.");
		}
		else
		{
			((Gem_L_DivineFaith)obj).UserCode_ShowStackUp__NetworkConnectionToClient__Vector3((NetworkConnectionToClient)NetworkClient.connection, reader.ReadVector3());
		}
	}

	static Gem_L_DivineFaith()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Gem_L_DivineFaith), "System.Void Gem_L_DivineFaith::ShowStackUp(Mirror.NetworkConnectionToClient,UnityEngine.Vector3)", InvokeUserCode_ShowStackUp__NetworkConnectionToClient__Vector3);
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteInt(currentStack);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x2000L) != 0L)
		{
			writer.WriteInt(currentStack);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref currentStack, null, reader.ReadInt());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x2000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref currentStack, null, reader.ReadInt());
		}
	}
}
