using System.Collections;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Shrine_StarCookie : Shrine, ICustomInteractable
{
	public float chanceToAdapt;

	public float startDelay;

	public float healRatio;

	public int ticks;

	public float tickInterval;

	public float dealRatio;

	public float baseDreamDustAmount;

	public float addedPerZoneIndex;

	public GameObject fxHot;

	public GameObject fxShocking;

	public string nameRawText => DewLocalization.GetUIValue(GetType().Name + "_Name");

	public string interactActionRawText => DewLocalization.GetUIValue("InGame_Tooltip_PickUp");

	public Vector3 worldOffset => new Vector3(0f, 1.5f, 0f);

	protected override bool OnUse(Entity entity)
	{
		StartCoroutine(Routine());
		return true;
		IEnumerator Routine()
		{
			if (Random.Range(0f, 1f) < chanceToAdapt || entity is Hero_Yubar)
			{
				if (entity is Hero_Yubar)
				{
					healRatio *= 2f;
					baseDreamDustAmount *= 2f;
				}
				CreateStatusEffect(entity, default(CastInfo), delegate(Se_GenericHealOverTime b)
				{
					b.ticks = ticks;
					b.tickInterval = tickInterval;
					b.totalAmount = healRatio * entity.maxHealth;
				});
				float f = baseDreamDustAmount + addedPerZoneIndex * (float)NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex;
				NetworkedManagerBase<PickupManager>.instance.DropDreamDust(isGivenByOtherPlayer: false, Mathf.RoundToInt(f), Dew.GetPositionOnGround(base.transform.position), (Hero)entity);
				NetworkedManagerBase<PickupManager>.instance.DropGold(isKillGold: false, isGivenByOtherPlayer: false, Mathf.RoundToInt(f), Dew.GetPositionOnGround(base.transform.position), (Hero)entity);
				switch (Random.Range(0, 3))
				{
				case 0:
					RpcPopMessage(entity, "RoomMod_StarCookie_Flavor_Buttery", new Color(1f, 1f, 0.7f));
					break;
				case 1:
					RpcPopMessage(entity, "RoomMod_StarCookie_Flavor_Chocolatey", new Color(1f, 0.6f, 0.6f));
					break;
				default:
					RpcPopMessage(entity, "RoomMod_StarCookie_Flavor_Sweet", new Color(0.5f, 1f, 0.7f));
					break;
				}
				Destroy();
			}
			else
			{
				int rand = Random.Range(0, 2);
				if (rand == 0)
				{
					RpcPopMessage(entity, "RoomMod_StarCookie_Flavor_Hot", new Color(1f, 0.4f, 0.4f));
				}
				else
				{
					RpcPopMessage(entity, "RoomMod_StarCookie_Flavor_Shocking", new Color(0.4f, 0.8f, 1f));
				}
				yield return new WaitForSeconds(startDelay);
				if (rand == 0)
				{
					FxPlayNewNetworked(fxHot, entity);
					entity.ApplyElemental(ElementalType.Fire, entity, Random.Range(1, 5));
				}
				else
				{
					FxPlayNewNetworked(fxShocking, entity);
					CreateBasicEffect(entity, new StunEffect(), 2f, "StarcookieStun", DuplicateEffectBehavior.UsePrevious);
				}
				PureDamage(entity.maxHealth * dealRatio, 0f).SetOriginPosition(base.transform.position).Dispatch(entity);
				Destroy();
			}
		}
	}

	[ClientRpc]
	private void RpcPopMessage(Entity e, string uiValue, Color c)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteNetworkBehaviour(e);
		writer.WriteString(uiValue);
		writer.WriteColor(c);
		SendRPCInternal("System.Void Shrine_StarCookie::RpcPopMessage(Entity,System.String,UnityEngine.Color)", 1374468007, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcPopMessage__Entity__String__Color(Entity e, string uiValue, Color c)
	{
		InGameUIManager.instance.ShowWorldPopMessage(new WorldMessageSetting
		{
			rawText = DewLocalization.GetUIValue(uiValue),
			color = c,
			worldPosGetter = () => (!(e != null)) ? Vector3.zero : e.Visual.GetCenterPosition()
		});
	}

	protected static void InvokeUserCode_RpcPopMessage__Entity__String__Color(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPopMessage called on server.");
		}
		else
		{
			((Shrine_StarCookie)obj).UserCode_RpcPopMessage__Entity__String__Color(reader.ReadNetworkBehaviour<Entity>(), reader.ReadString(), reader.ReadColor());
		}
	}

	static Shrine_StarCookie()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Shrine_StarCookie), "System.Void Shrine_StarCookie::RpcPopMessage(Entity,System.String,UnityEngine.Color)", InvokeUserCode_RpcPopMessage__Entity__String__Color);
	}
}
