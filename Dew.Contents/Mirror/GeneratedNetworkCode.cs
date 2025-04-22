using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Mirror;

[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public static class GeneratedNetworkCode
{
	public static TimeSnapshotMessage _Read_Mirror_002ETimeSnapshotMessage(NetworkReader reader)
	{
		return default(TimeSnapshotMessage);
	}

	public static void _Write_Mirror_002ETimeSnapshotMessage(NetworkWriter writer, TimeSnapshotMessage value)
	{
	}

	public static ReadyMessage _Read_Mirror_002EReadyMessage(NetworkReader reader)
	{
		return default(ReadyMessage);
	}

	public static void _Write_Mirror_002EReadyMessage(NetworkWriter writer, ReadyMessage value)
	{
	}

	public static NotReadyMessage _Read_Mirror_002ENotReadyMessage(NetworkReader reader)
	{
		return default(NotReadyMessage);
	}

	public static void _Write_Mirror_002ENotReadyMessage(NetworkWriter writer, NotReadyMessage value)
	{
	}

	public static AddPlayerMessage _Read_Mirror_002EAddPlayerMessage(NetworkReader reader)
	{
		return default(AddPlayerMessage);
	}

	public static void _Write_Mirror_002EAddPlayerMessage(NetworkWriter writer, AddPlayerMessage value)
	{
	}

	public static SceneMessage _Read_Mirror_002ESceneMessage(NetworkReader reader)
	{
		SceneMessage result = default(SceneMessage);
		result.sceneName = reader.ReadString();
		result.sceneOperation = _Read_Mirror_002ESceneOperation(reader);
		result.customHandling = reader.ReadBool();
		return result;
	}

	public static SceneOperation _Read_Mirror_002ESceneOperation(NetworkReader reader)
	{
		return (SceneOperation)NetworkReaderExtensions.ReadByte(reader);
	}

	public static void _Write_Mirror_002ESceneMessage(NetworkWriter writer, SceneMessage value)
	{
		writer.WriteString(value.sceneName);
		_Write_Mirror_002ESceneOperation(writer, value.sceneOperation);
		writer.WriteBool(value.customHandling);
	}

	public static void _Write_Mirror_002ESceneOperation(NetworkWriter writer, SceneOperation value)
	{
		NetworkWriterExtensions.WriteByte(writer, (byte)value);
	}

	public static CommandMessage _Read_Mirror_002ECommandMessage(NetworkReader reader)
	{
		CommandMessage result = default(CommandMessage);
		result.netId = reader.ReadUInt();
		result.componentIndex = NetworkReaderExtensions.ReadByte(reader);
		result.functionHash = reader.ReadUShort();
		result.payload = reader.ReadBytesAndSizeSegment();
		return result;
	}

	public static void _Write_Mirror_002ECommandMessage(NetworkWriter writer, CommandMessage value)
	{
		writer.WriteUInt(value.netId);
		NetworkWriterExtensions.WriteByte(writer, value.componentIndex);
		writer.WriteUShort(value.functionHash);
		writer.WriteBytesAndSizeSegment(value.payload);
	}

	public static RpcMessage _Read_Mirror_002ERpcMessage(NetworkReader reader)
	{
		RpcMessage result = default(RpcMessage);
		result.netId = reader.ReadUInt();
		result.componentIndex = NetworkReaderExtensions.ReadByte(reader);
		result.functionHash = reader.ReadUShort();
		result.payload = reader.ReadBytesAndSizeSegment();
		return result;
	}

	public static void _Write_Mirror_002ERpcMessage(NetworkWriter writer, RpcMessage value)
	{
		writer.WriteUInt(value.netId);
		NetworkWriterExtensions.WriteByte(writer, value.componentIndex);
		writer.WriteUShort(value.functionHash);
		writer.WriteBytesAndSizeSegment(value.payload);
	}

	public static RpcBufferMessage _Read_Mirror_002ERpcBufferMessage(NetworkReader reader)
	{
		RpcBufferMessage result = default(RpcBufferMessage);
		result.payload = reader.ReadBytesAndSizeSegment();
		return result;
	}

	public static void _Write_Mirror_002ERpcBufferMessage(NetworkWriter writer, RpcBufferMessage value)
	{
		writer.WriteBytesAndSizeSegment(value.payload);
	}

	public static SpawnMessage _Read_Mirror_002ESpawnMessage(NetworkReader reader)
	{
		SpawnMessage result = default(SpawnMessage);
		result.netId = reader.ReadUInt();
		result.isLocalPlayer = reader.ReadBool();
		result.isOwner = reader.ReadBool();
		result.sceneId = reader.ReadULong();
		result.assetId = reader.ReadUInt();
		result.position = reader.ReadVector3();
		result.rotation = reader.ReadQuaternion();
		result.scale = reader.ReadVector3();
		result.payload = reader.ReadBytesAndSizeSegment();
		return result;
	}

	public static void _Write_Mirror_002ESpawnMessage(NetworkWriter writer, SpawnMessage value)
	{
		writer.WriteUInt(value.netId);
		writer.WriteBool(value.isLocalPlayer);
		writer.WriteBool(value.isOwner);
		writer.WriteULong(value.sceneId);
		writer.WriteUInt(value.assetId);
		writer.WriteVector3(value.position);
		writer.WriteQuaternion(value.rotation);
		writer.WriteVector3(value.scale);
		writer.WriteBytesAndSizeSegment(value.payload);
	}

	public static ChangeOwnerMessage _Read_Mirror_002EChangeOwnerMessage(NetworkReader reader)
	{
		ChangeOwnerMessage result = default(ChangeOwnerMessage);
		result.netId = reader.ReadUInt();
		result.isOwner = reader.ReadBool();
		result.isLocalPlayer = reader.ReadBool();
		return result;
	}

	public static void _Write_Mirror_002EChangeOwnerMessage(NetworkWriter writer, ChangeOwnerMessage value)
	{
		writer.WriteUInt(value.netId);
		writer.WriteBool(value.isOwner);
		writer.WriteBool(value.isLocalPlayer);
	}

	public static ObjectSpawnStartedMessage _Read_Mirror_002EObjectSpawnStartedMessage(NetworkReader reader)
	{
		return default(ObjectSpawnStartedMessage);
	}

	public static void _Write_Mirror_002EObjectSpawnStartedMessage(NetworkWriter writer, ObjectSpawnStartedMessage value)
	{
	}

	public static ObjectSpawnFinishedMessage _Read_Mirror_002EObjectSpawnFinishedMessage(NetworkReader reader)
	{
		return default(ObjectSpawnFinishedMessage);
	}

	public static void _Write_Mirror_002EObjectSpawnFinishedMessage(NetworkWriter writer, ObjectSpawnFinishedMessage value)
	{
	}

	public static ObjectDestroyMessage _Read_Mirror_002EObjectDestroyMessage(NetworkReader reader)
	{
		ObjectDestroyMessage result = default(ObjectDestroyMessage);
		result.netId = reader.ReadUInt();
		return result;
	}

	public static void _Write_Mirror_002EObjectDestroyMessage(NetworkWriter writer, ObjectDestroyMessage value)
	{
		writer.WriteUInt(value.netId);
	}

	public static ObjectHideMessage _Read_Mirror_002EObjectHideMessage(NetworkReader reader)
	{
		ObjectHideMessage result = default(ObjectHideMessage);
		result.netId = reader.ReadUInt();
		return result;
	}

	public static void _Write_Mirror_002EObjectHideMessage(NetworkWriter writer, ObjectHideMessage value)
	{
		writer.WriteUInt(value.netId);
	}

	public static EntityStateMessage _Read_Mirror_002EEntityStateMessage(NetworkReader reader)
	{
		EntityStateMessage result = default(EntityStateMessage);
		result.netId = reader.ReadUInt();
		result.payload = reader.ReadBytesAndSizeSegment();
		return result;
	}

	public static void _Write_Mirror_002EEntityStateMessage(NetworkWriter writer, EntityStateMessage value)
	{
		writer.WriteUInt(value.netId);
		writer.WriteBytesAndSizeSegment(value.payload);
	}

	public static NetworkPingMessage _Read_Mirror_002ENetworkPingMessage(NetworkReader reader)
	{
		NetworkPingMessage result = default(NetworkPingMessage);
		result.clientTime = reader.ReadDouble();
		return result;
	}

	public static void _Write_Mirror_002ENetworkPingMessage(NetworkWriter writer, NetworkPingMessage value)
	{
		writer.WriteDouble(value.clientTime);
	}

	public static NetworkPongMessage _Read_Mirror_002ENetworkPongMessage(NetworkReader reader)
	{
		NetworkPongMessage result = default(NetworkPongMessage);
		result.clientTime = reader.ReadDouble();
		return result;
	}

	public static void _Write_Mirror_002ENetworkPongMessage(NetworkWriter writer, NetworkPongMessage value)
	{
		writer.WriteDouble(value.clientTime);
	}

	public static void _Write_CastInfo(NetworkWriter writer, CastInfo value)
	{
		writer.WriteNetworkBehaviour(value.caster);
		writer.WriteNetworkBehaviour(value.target);
		writer.WriteVector3(value.point);
		writer.WriteFloat(value.angle);
		writer.WriteFloat(value.animSelectValue);
	}

	public static CastInfo _Read_CastInfo(NetworkReader reader)
	{
		CastInfo result = default(CastInfo);
		result.caster = reader.ReadNetworkBehaviour<Entity>();
		result.target = reader.ReadNetworkBehaviour<Entity>();
		result.point = reader.ReadVector3();
		result.angle = reader.ReadFloat();
		result.animSelectValue = reader.ReadFloat();
		return result;
	}

	public static void _Write_HatredStrengthType(NetworkWriter writer, HatredStrengthType value)
	{
		writer.WriteInt((int)value);
	}

	public static void _Write_QuestProgressType(NetworkWriter writer, QuestProgressType value)
	{
		writer.WriteInt((int)value);
	}

	public static HatredStrengthType _Read_HatredStrengthType(NetworkReader reader)
	{
		return (HatredStrengthType)reader.ReadInt();
	}

	public static QuestProgressType _Read_QuestProgressType(NetworkReader reader)
	{
		return (QuestProgressType)reader.ReadInt();
	}

	public static void _Write_Projectile_002FEntityHit(NetworkWriter writer, Projectile.EntityHit value)
	{
		writer.WriteNetworkBehaviour(value.entity);
		writer.WriteVector3(value.point);
	}

	public static Projectile.EntityHit _Read_Projectile_002FEntityHit(NetworkReader reader)
	{
		Projectile.EntityHit result = default(Projectile.EntityHit);
		result.entity = reader.ReadNetworkBehaviour<Entity>();
		result.point = reader.ReadVector3();
		return result;
	}

	public static void _Write_Projectile_002FProjectileMode(NetworkWriter writer, Projectile.ProjectileMode value)
	{
		writer.WriteInt((int)value);
	}

	public static Projectile.ProjectileMode _Read_Projectile_002FProjectileMode(NetworkReader reader)
	{
		return (Projectile.ProjectileMode)reader.ReadInt();
	}

	public static void _Write_AbilityTrigger_002FConfigSyncData_005B_005D(NetworkWriter writer, AbilityTrigger.ConfigSyncData[] value)
	{
		writer.WriteArray(value);
	}

	public static void _Write_AbilityTrigger_002FConfigSyncData(NetworkWriter writer, AbilityTrigger.ConfigSyncData value)
	{
		writer.WriteFloat(value.manaCost);
		writer.WriteInt(value.maxCharges);
		writer.WriteInt(value.addedCharges);
		writer.WriteFloat(value.cooldownTime);
		writer.WriteFloat(value.minimumDelay);
	}

	public static AbilityTrigger.ConfigSyncData[] _Read_AbilityTrigger_002FConfigSyncData_005B_005D(NetworkReader reader)
	{
		return reader.ReadArray<AbilityTrigger.ConfigSyncData>();
	}

	public static AbilityTrigger.ConfigSyncData _Read_AbilityTrigger_002FConfigSyncData(NetworkReader reader)
	{
		AbilityTrigger.ConfigSyncData result = default(AbilityTrigger.ConfigSyncData);
		result.manaCost = reader.ReadFloat();
		result.maxCharges = reader.ReadInt();
		result.addedCharges = reader.ReadInt();
		result.cooldownTime = reader.ReadFloat();
		result.minimumDelay = reader.ReadFloat();
		return result;
	}

	public static void _Write_CastMethodData(NetworkWriter writer, CastMethodData value)
	{
		if (value == null)
		{
			writer.WriteBool(value: false);
			return;
		}
		writer.WriteBool(value: true);
		_Write_CastMethodType(writer, value.type);
		writer.WriteFloat(value._angle);
		writer.WriteFloat(value._length);
		writer.WriteFloat(value._width);
		writer.WriteFloat(value._range);
		writer.WriteFloat(value._radius);
		writer.WriteBool(value._isClamping);
	}

	public static void _Write_CastMethodType(NetworkWriter writer, CastMethodType value)
	{
		writer.WriteInt((int)value);
	}

	public static CastMethodData _Read_CastMethodData(NetworkReader reader)
	{
		if (!reader.ReadBool())
		{
			return null;
		}
		CastMethodData castMethodData = new CastMethodData();
		castMethodData.type = _Read_CastMethodType(reader);
		castMethodData._angle = reader.ReadFloat();
		castMethodData._length = reader.ReadFloat();
		castMethodData._width = reader.ReadFloat();
		castMethodData._range = reader.ReadFloat();
		castMethodData._radius = reader.ReadFloat();
		castMethodData._isClamping = reader.ReadBool();
		return castMethodData;
	}

	public static CastMethodType _Read_CastMethodType(NetworkReader reader)
	{
		return (CastMethodType)reader.ReadInt();
	}

	public static void _Write_EventInfoSkillUse(NetworkWriter writer, EventInfoSkillUse value)
	{
		_Write_HeroSkillLocation(writer, value.type);
		writer.WriteNetworkBehaviour(value.skill);
	}

	public static void _Write_HeroSkillLocation(NetworkWriter writer, HeroSkillLocation value)
	{
		writer.WriteInt((int)value);
	}

	public static EventInfoSkillUse _Read_EventInfoSkillUse(NetworkReader reader)
	{
		EventInfoSkillUse result = default(EventInfoSkillUse);
		result.type = _Read_HeroSkillLocation(reader);
		result.skill = reader.ReadNetworkBehaviour<SkillTrigger>();
		return result;
	}

	public static HeroSkillLocation _Read_HeroSkillLocation(NetworkReader reader)
	{
		return (HeroSkillLocation)reader.ReadInt();
	}

	public static void _Write_DescriptionTags(NetworkWriter writer, DescriptionTags value)
	{
		writer.WriteInt((int)value);
	}

	public static DescriptionTags _Read_DescriptionTags(NetworkReader reader)
	{
		return (DescriptionTags)reader.ReadInt();
	}

	public static void _Write_EventInfoKill(NetworkWriter writer, EventInfoKill value)
	{
		writer.WriteNetworkBehaviour(value.actor);
		writer.WriteNetworkBehaviour(value.victim);
	}

	public static EventInfoKill _Read_EventInfoKill(NetworkReader reader)
	{
		EventInfoKill result = default(EventInfoKill);
		result.actor = reader.ReadNetworkBehaviour<Actor>();
		result.victim = reader.ReadNetworkBehaviour<Entity>();
		return result;
	}

	public static void _Write_HeroLoadoutData(NetworkWriter writer, HeroLoadoutData value)
	{
		if (value == null)
		{
			writer.WriteBool(value: false);
			return;
		}
		writer.WriteBool(value: true);
		writer.WriteInt(value.skillQ);
		writer.WriteInt(value.skillR);
		writer.WriteInt(value.skillTrait);
		writer.WriteInt(value.skillMovement);
		_Write_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E(writer, value.cDestruction);
		_Write_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E(writer, value.cLife);
		_Write_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E(writer, value.cImagination);
		_Write_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E(writer, value.cFlexible);
	}

	public static void _Write_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E(NetworkWriter writer, List<LoadoutStarItem> value)
	{
		writer.WriteList(value);
	}

	public static void _Write_LoadoutStarItem(NetworkWriter writer, LoadoutStarItem value)
	{
		writer.WriteString(value.name);
		writer.WriteInt(value.level);
	}

	public static HeroLoadoutData _Read_HeroLoadoutData(NetworkReader reader)
	{
		if (!reader.ReadBool())
		{
			return null;
		}
		HeroLoadoutData heroLoadoutData = new HeroLoadoutData();
		heroLoadoutData.skillQ = reader.ReadInt();
		heroLoadoutData.skillR = reader.ReadInt();
		heroLoadoutData.skillTrait = reader.ReadInt();
		heroLoadoutData.skillMovement = reader.ReadInt();
		heroLoadoutData.cDestruction = _Read_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E(reader);
		heroLoadoutData.cLife = _Read_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E(reader);
		heroLoadoutData.cImagination = _Read_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E(reader);
		heroLoadoutData.cFlexible = _Read_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E(reader);
		return heroLoadoutData;
	}

	public static List<LoadoutStarItem> _Read_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E(NetworkReader reader)
	{
		return reader.ReadList<LoadoutStarItem>();
	}

	public static LoadoutStarItem _Read_LoadoutStarItem(NetworkReader reader)
	{
		LoadoutStarItem result = default(LoadoutStarItem);
		result.name = reader.ReadString();
		result.level = reader.ReadInt();
		return result;
	}

	public static void _Write_Hero_Aurena_002FClawState(NetworkWriter writer, Hero_Aurena.ClawState value)
	{
		writer.WriteInt((int)value);
	}

	public static Hero_Aurena.ClawState _Read_Hero_Aurena_002FClawState(NetworkReader reader)
	{
		return (Hero_Aurena.ClawState)reader.ReadInt();
	}

	public static void _Write_Monster_002FMonsterType(NetworkWriter writer, Monster.MonsterType value)
	{
		NetworkWriterExtensions.WriteByte(writer, (byte)value);
	}

	public static Monster.MonsterType _Read_Monster_002FMonsterType(NetworkReader reader)
	{
		return (Monster.MonsterType)NetworkReaderExtensions.ReadByte(reader);
	}

	public static void _Write_DewDifficultySettings(NetworkWriter writer, DewDifficultySettings value)
	{
		if ((object)value == null)
		{
			writer.WriteBool(value: false);
			return;
		}
		writer.WriteBool(value: true);
		writer.WriteColor(value.difficultyColor);
		writer.WriteSprite(value.icon);
		writer.WriteFloat(value.iconScale);
		writer.WriteFloat(value.maxPopulationMultiplier);
		writer.WriteFloat(value.regenOrbChanceMultiplier);
		_Write_UnityEngine_002EAnimationCurve(writer, value.predictionStrengthCurve);
		writer.WriteFloat(value.healRawMultiplier);
		writer.WriteFloat(value.scoreMultiplier);
		writer.WriteFloat(value.specialSkillChanceMultiplier);
		writer.WriteInt(value.gainedStardustAmountOffset);
		writer.WriteBool(value.enableBleedOuts);
		writer.WriteVector2Int(value.lostSoulDistance);
		writer.WriteFloat(value.beneficialNodeMultiplier);
		writer.WriteFloat(value.harmfulNodeMultiplier);
		writer.WriteFloat(value.hunterSpreadChance);
		writer.WriteFloat(value.enemyHealthPercentage);
		writer.WriteFloat(value.enemyPowerPercentage);
		writer.WriteFloat(value.enemyMovementSpeedPercentage);
		writer.WriteFloat(value.enemyAttackSpeedPercentage);
		writer.WriteFloat(value.enemyAbilityHasteFlat);
		writer.WriteFloat(value.scalingFactor);
		writer.WriteInt(value.positionSampleCount);
		writer.WriteInt(value.positionSampleLagBehindFrames);
		writer.WriteFloat(value.positionSampleInterval);
	}

	public static void _Write_UnityEngine_002EAnimationCurve(NetworkWriter writer, AnimationCurve value)
	{
		if (value == null)
		{
			writer.WriteBool(value: false);
			return;
		}
		writer.WriteBool(value: true);
		_Write_System_002EIntPtr(writer, value.m_Ptr);
	}

	public static void _Write_System_002EIntPtr(NetworkWriter writer, IntPtr value)
	{
	}

	public static DewDifficultySettings _Read_DewDifficultySettings(NetworkReader reader)
	{
		if (!reader.ReadBool())
		{
			return null;
		}
		DewDifficultySettings dewDifficultySettings = ScriptableObject.CreateInstance<DewDifficultySettings>();
		dewDifficultySettings.difficultyColor = reader.ReadColor();
		dewDifficultySettings.icon = reader.ReadSprite();
		dewDifficultySettings.iconScale = reader.ReadFloat();
		dewDifficultySettings.maxPopulationMultiplier = reader.ReadFloat();
		dewDifficultySettings.regenOrbChanceMultiplier = reader.ReadFloat();
		dewDifficultySettings.predictionStrengthCurve = _Read_UnityEngine_002EAnimationCurve(reader);
		dewDifficultySettings.healRawMultiplier = reader.ReadFloat();
		dewDifficultySettings.scoreMultiplier = reader.ReadFloat();
		dewDifficultySettings.specialSkillChanceMultiplier = reader.ReadFloat();
		dewDifficultySettings.gainedStardustAmountOffset = reader.ReadInt();
		dewDifficultySettings.enableBleedOuts = reader.ReadBool();
		dewDifficultySettings.lostSoulDistance = reader.ReadVector2Int();
		dewDifficultySettings.beneficialNodeMultiplier = reader.ReadFloat();
		dewDifficultySettings.harmfulNodeMultiplier = reader.ReadFloat();
		dewDifficultySettings.hunterSpreadChance = reader.ReadFloat();
		dewDifficultySettings.enemyHealthPercentage = reader.ReadFloat();
		dewDifficultySettings.enemyPowerPercentage = reader.ReadFloat();
		dewDifficultySettings.enemyMovementSpeedPercentage = reader.ReadFloat();
		dewDifficultySettings.enemyAttackSpeedPercentage = reader.ReadFloat();
		dewDifficultySettings.enemyAbilityHasteFlat = reader.ReadFloat();
		dewDifficultySettings.scalingFactor = reader.ReadFloat();
		dewDifficultySettings.positionSampleCount = reader.ReadInt();
		dewDifficultySettings.positionSampleLagBehindFrames = reader.ReadInt();
		dewDifficultySettings.positionSampleInterval = reader.ReadFloat();
		return dewDifficultySettings;
	}

	public static AnimationCurve _Read_UnityEngine_002EAnimationCurve(NetworkReader reader)
	{
		if (!reader.ReadBool())
		{
			return null;
		}
		AnimationCurve animationCurve = new AnimationCurve();
		animationCurve.m_Ptr = _Read_System_002EIntPtr(reader);
		return animationCurve;
	}

	public static IntPtr _Read_System_002EIntPtr(NetworkReader reader)
	{
		return default(IntPtr);
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void InitReadWriters()
	{
		Writer<byte>.write = NetworkWriterExtensions.WriteByte;
		Writer<byte?>.write = NetworkWriterExtensions.WriteByteNullable;
		Writer<sbyte>.write = NetworkWriterExtensions.WriteSByte;
		Writer<sbyte?>.write = NetworkWriterExtensions.WriteSByteNullable;
		Writer<char>.write = NetworkWriterExtensions.WriteChar;
		Writer<char?>.write = NetworkWriterExtensions.WriteCharNullable;
		Writer<bool>.write = NetworkWriterExtensions.WriteBool;
		Writer<bool?>.write = NetworkWriterExtensions.WriteBoolNullable;
		Writer<short>.write = NetworkWriterExtensions.WriteShort;
		Writer<short?>.write = NetworkWriterExtensions.WriteShortNullable;
		Writer<ushort>.write = NetworkWriterExtensions.WriteUShort;
		Writer<ushort?>.write = NetworkWriterExtensions.WriteUShortNullable;
		Writer<int>.write = NetworkWriterExtensions.WriteInt;
		Writer<int?>.write = NetworkWriterExtensions.WriteIntNullable;
		Writer<uint>.write = NetworkWriterExtensions.WriteUInt;
		Writer<uint?>.write = NetworkWriterExtensions.WriteUIntNullable;
		Writer<long>.write = NetworkWriterExtensions.WriteLong;
		Writer<long?>.write = NetworkWriterExtensions.WriteLongNullable;
		Writer<ulong>.write = NetworkWriterExtensions.WriteULong;
		Writer<ulong?>.write = NetworkWriterExtensions.WriteULongNullable;
		Writer<float>.write = NetworkWriterExtensions.WriteFloat;
		Writer<float?>.write = NetworkWriterExtensions.WriteFloatNullable;
		Writer<double>.write = NetworkWriterExtensions.WriteDouble;
		Writer<double?>.write = NetworkWriterExtensions.WriteDoubleNullable;
		Writer<decimal>.write = NetworkWriterExtensions.WriteDecimal;
		Writer<decimal?>.write = NetworkWriterExtensions.WriteDecimalNullable;
		Writer<string>.write = NetworkWriterExtensions.WriteString;
		Writer<ArraySegment<byte>>.write = NetworkWriterExtensions.WriteBytesAndSizeSegment;
		Writer<byte[]>.write = NetworkWriterExtensions.WriteBytesAndSize;
		Writer<Vector2>.write = NetworkWriterExtensions.WriteVector2;
		Writer<Vector2?>.write = NetworkWriterExtensions.WriteVector2Nullable;
		Writer<Vector3>.write = NetworkWriterExtensions.WriteVector3;
		Writer<Vector3?>.write = NetworkWriterExtensions.WriteVector3Nullable;
		Writer<Vector4>.write = NetworkWriterExtensions.WriteVector4;
		Writer<Vector4?>.write = NetworkWriterExtensions.WriteVector4Nullable;
		Writer<Vector2Int>.write = NetworkWriterExtensions.WriteVector2Int;
		Writer<Vector2Int?>.write = NetworkWriterExtensions.WriteVector2IntNullable;
		Writer<Vector3Int>.write = NetworkWriterExtensions.WriteVector3Int;
		Writer<Vector3Int?>.write = NetworkWriterExtensions.WriteVector3IntNullable;
		Writer<Color>.write = NetworkWriterExtensions.WriteColor;
		Writer<Color?>.write = NetworkWriterExtensions.WriteColorNullable;
		Writer<Color32>.write = NetworkWriterExtensions.WriteColor32;
		Writer<Color32?>.write = NetworkWriterExtensions.WriteColor32Nullable;
		Writer<Quaternion>.write = NetworkWriterExtensions.WriteQuaternion;
		Writer<Quaternion?>.write = NetworkWriterExtensions.WriteQuaternionNullable;
		Writer<Rect>.write = NetworkWriterExtensions.WriteRect;
		Writer<Rect?>.write = NetworkWriterExtensions.WriteRectNullable;
		Writer<Plane>.write = NetworkWriterExtensions.WritePlane;
		Writer<Plane?>.write = NetworkWriterExtensions.WritePlaneNullable;
		Writer<Ray>.write = NetworkWriterExtensions.WriteRay;
		Writer<Ray?>.write = NetworkWriterExtensions.WriteRayNullable;
		Writer<Matrix4x4>.write = NetworkWriterExtensions.WriteMatrix4x4;
		Writer<Matrix4x4?>.write = NetworkWriterExtensions.WriteMatrix4x4Nullable;
		Writer<Guid>.write = NetworkWriterExtensions.WriteGuid;
		Writer<Guid?>.write = NetworkWriterExtensions.WriteGuidNullable;
		Writer<NetworkIdentity>.write = NetworkWriterExtensions.WriteNetworkIdentity;
		Writer<NetworkBehaviour>.write = NetworkWriterExtensions.WriteNetworkBehaviour;
		Writer<Transform>.write = NetworkWriterExtensions.WriteTransform;
		Writer<GameObject>.write = NetworkWriterExtensions.WriteGameObject;
		Writer<Uri>.write = NetworkWriterExtensions.WriteUri;
		Writer<Texture2D>.write = NetworkWriterExtensions.WriteTexture2D;
		Writer<Sprite>.write = NetworkWriterExtensions.WriteSprite;
		Writer<DateTime>.write = NetworkWriterExtensions.WriteDateTime;
		Writer<DateTime?>.write = NetworkWriterExtensions.WriteDateTimeNullable;
		Writer<TimeSnapshotMessage>.write = _Write_Mirror_002ETimeSnapshotMessage;
		Writer<ReadyMessage>.write = _Write_Mirror_002EReadyMessage;
		Writer<NotReadyMessage>.write = _Write_Mirror_002ENotReadyMessage;
		Writer<AddPlayerMessage>.write = _Write_Mirror_002EAddPlayerMessage;
		Writer<SceneMessage>.write = _Write_Mirror_002ESceneMessage;
		Writer<SceneOperation>.write = _Write_Mirror_002ESceneOperation;
		Writer<CommandMessage>.write = _Write_Mirror_002ECommandMessage;
		Writer<RpcMessage>.write = _Write_Mirror_002ERpcMessage;
		Writer<RpcBufferMessage>.write = _Write_Mirror_002ERpcBufferMessage;
		Writer<SpawnMessage>.write = _Write_Mirror_002ESpawnMessage;
		Writer<ChangeOwnerMessage>.write = _Write_Mirror_002EChangeOwnerMessage;
		Writer<ObjectSpawnStartedMessage>.write = _Write_Mirror_002EObjectSpawnStartedMessage;
		Writer<ObjectSpawnFinishedMessage>.write = _Write_Mirror_002EObjectSpawnFinishedMessage;
		Writer<ObjectDestroyMessage>.write = _Write_Mirror_002EObjectDestroyMessage;
		Writer<ObjectHideMessage>.write = _Write_Mirror_002EObjectHideMessage;
		Writer<EntityStateMessage>.write = _Write_Mirror_002EEntityStateMessage;
		Writer<NetworkPingMessage>.write = _Write_Mirror_002ENetworkPingMessage;
		Writer<NetworkPongMessage>.write = _Write_Mirror_002ENetworkPongMessage;
		Writer<CastInfo>.write = _Write_CastInfo;
		Writer<Entity>.write = NetworkWriterExtensions.WriteNetworkBehaviour;
		Writer<HatredStrengthType>.write = _Write_HatredStrengthType;
		Writer<QuestProgressType>.write = _Write_QuestProgressType;
		Writer<Projectile.EntityHit>.write = _Write_Projectile_002FEntityHit;
		Writer<Projectile.ProjectileMode>.write = _Write_Projectile_002FProjectileMode;
		Writer<AbilityTrigger.ConfigSyncData[]>.write = _Write_AbilityTrigger_002FConfigSyncData_005B_005D;
		Writer<AbilityTrigger.ConfigSyncData>.write = _Write_AbilityTrigger_002FConfigSyncData;
		Writer<CastMethodData>.write = _Write_CastMethodData;
		Writer<CastMethodType>.write = _Write_CastMethodType;
		Writer<EventInfoSkillUse>.write = _Write_EventInfoSkillUse;
		Writer<HeroSkillLocation>.write = _Write_HeroSkillLocation;
		Writer<SkillTrigger>.write = NetworkWriterExtensions.WriteNetworkBehaviour;
		Writer<DescriptionTags>.write = _Write_DescriptionTags;
		Writer<EventInfoKill>.write = _Write_EventInfoKill;
		Writer<Actor>.write = NetworkWriterExtensions.WriteNetworkBehaviour;
		Writer<Room_Waypoint>.write = NetworkWriterExtensions.WriteNetworkBehaviour;
		Writer<HeroLoadoutData>.write = _Write_HeroLoadoutData;
		Writer<List<LoadoutStarItem>>.write = _Write_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E;
		Writer<LoadoutStarItem>.write = _Write_LoadoutStarItem;
		Writer<Hero_Aurena.ClawState>.write = _Write_Hero_Aurena_002FClawState;
		Writer<Summon>.write = NetworkWriterExtensions.WriteNetworkBehaviour;
		Writer<Monster.MonsterType>.write = _Write_Monster_002FMonsterType;
		Writer<DewDifficultySettings>.write = _Write_DewDifficultySettings;
		Writer<AnimationCurve>.write = _Write_UnityEngine_002EAnimationCurve;
		Writer<IntPtr>.write = _Write_System_002EIntPtr;
		Reader<byte>.read = NetworkReaderExtensions.ReadByte;
		Reader<byte?>.read = NetworkReaderExtensions.ReadByteNullable;
		Reader<sbyte>.read = NetworkReaderExtensions.ReadSByte;
		Reader<sbyte?>.read = NetworkReaderExtensions.ReadSByteNullable;
		Reader<char>.read = NetworkReaderExtensions.ReadChar;
		Reader<char?>.read = NetworkReaderExtensions.ReadCharNullable;
		Reader<bool>.read = NetworkReaderExtensions.ReadBool;
		Reader<bool?>.read = NetworkReaderExtensions.ReadBoolNullable;
		Reader<short>.read = NetworkReaderExtensions.ReadShort;
		Reader<short?>.read = NetworkReaderExtensions.ReadShortNullable;
		Reader<ushort>.read = NetworkReaderExtensions.ReadUShort;
		Reader<ushort?>.read = NetworkReaderExtensions.ReadUShortNullable;
		Reader<int>.read = NetworkReaderExtensions.ReadInt;
		Reader<int?>.read = NetworkReaderExtensions.ReadIntNullable;
		Reader<uint>.read = NetworkReaderExtensions.ReadUInt;
		Reader<uint?>.read = NetworkReaderExtensions.ReadUIntNullable;
		Reader<long>.read = NetworkReaderExtensions.ReadLong;
		Reader<long?>.read = NetworkReaderExtensions.ReadLongNullable;
		Reader<ulong>.read = NetworkReaderExtensions.ReadULong;
		Reader<ulong?>.read = NetworkReaderExtensions.ReadULongNullable;
		Reader<float>.read = NetworkReaderExtensions.ReadFloat;
		Reader<float?>.read = NetworkReaderExtensions.ReadFloatNullable;
		Reader<double>.read = NetworkReaderExtensions.ReadDouble;
		Reader<double?>.read = NetworkReaderExtensions.ReadDoubleNullable;
		Reader<decimal>.read = NetworkReaderExtensions.ReadDecimal;
		Reader<decimal?>.read = NetworkReaderExtensions.ReadDecimalNullable;
		Reader<string>.read = NetworkReaderExtensions.ReadString;
		Reader<byte[]>.read = NetworkReaderExtensions.ReadBytesAndSize;
		Reader<ArraySegment<byte>>.read = NetworkReaderExtensions.ReadBytesAndSizeSegment;
		Reader<Vector2>.read = NetworkReaderExtensions.ReadVector2;
		Reader<Vector2?>.read = NetworkReaderExtensions.ReadVector2Nullable;
		Reader<Vector3>.read = NetworkReaderExtensions.ReadVector3;
		Reader<Vector3?>.read = NetworkReaderExtensions.ReadVector3Nullable;
		Reader<Vector4>.read = NetworkReaderExtensions.ReadVector4;
		Reader<Vector4?>.read = NetworkReaderExtensions.ReadVector4Nullable;
		Reader<Vector2Int>.read = NetworkReaderExtensions.ReadVector2Int;
		Reader<Vector2Int?>.read = NetworkReaderExtensions.ReadVector2IntNullable;
		Reader<Vector3Int>.read = NetworkReaderExtensions.ReadVector3Int;
		Reader<Vector3Int?>.read = NetworkReaderExtensions.ReadVector3IntNullable;
		Reader<Color>.read = NetworkReaderExtensions.ReadColor;
		Reader<Color?>.read = NetworkReaderExtensions.ReadColorNullable;
		Reader<Color32>.read = NetworkReaderExtensions.ReadColor32;
		Reader<Color32?>.read = NetworkReaderExtensions.ReadColor32Nullable;
		Reader<Quaternion>.read = NetworkReaderExtensions.ReadQuaternion;
		Reader<Quaternion?>.read = NetworkReaderExtensions.ReadQuaternionNullable;
		Reader<Rect>.read = NetworkReaderExtensions.ReadRect;
		Reader<Rect?>.read = NetworkReaderExtensions.ReadRectNullable;
		Reader<Plane>.read = NetworkReaderExtensions.ReadPlane;
		Reader<Plane?>.read = NetworkReaderExtensions.ReadPlaneNullable;
		Reader<Ray>.read = NetworkReaderExtensions.ReadRay;
		Reader<Ray?>.read = NetworkReaderExtensions.ReadRayNullable;
		Reader<Matrix4x4>.read = NetworkReaderExtensions.ReadMatrix4x4;
		Reader<Matrix4x4?>.read = NetworkReaderExtensions.ReadMatrix4x4Nullable;
		Reader<Guid>.read = NetworkReaderExtensions.ReadGuid;
		Reader<Guid?>.read = NetworkReaderExtensions.ReadGuidNullable;
		Reader<NetworkIdentity>.read = NetworkReaderExtensions.ReadNetworkIdentity;
		Reader<NetworkBehaviour>.read = NetworkReaderExtensions.ReadNetworkBehaviour;
		Reader<NetworkBehaviourSyncVar>.read = NetworkReaderExtensions.ReadNetworkBehaviourSyncVar;
		Reader<Transform>.read = NetworkReaderExtensions.ReadTransform;
		Reader<GameObject>.read = NetworkReaderExtensions.ReadGameObject;
		Reader<Uri>.read = NetworkReaderExtensions.ReadUri;
		Reader<Texture2D>.read = NetworkReaderExtensions.ReadTexture2D;
		Reader<Sprite>.read = NetworkReaderExtensions.ReadSprite;
		Reader<DateTime>.read = NetworkReaderExtensions.ReadDateTime;
		Reader<DateTime?>.read = NetworkReaderExtensions.ReadDateTimeNullable;
		Reader<TimeSnapshotMessage>.read = _Read_Mirror_002ETimeSnapshotMessage;
		Reader<ReadyMessage>.read = _Read_Mirror_002EReadyMessage;
		Reader<NotReadyMessage>.read = _Read_Mirror_002ENotReadyMessage;
		Reader<AddPlayerMessage>.read = _Read_Mirror_002EAddPlayerMessage;
		Reader<SceneMessage>.read = _Read_Mirror_002ESceneMessage;
		Reader<SceneOperation>.read = _Read_Mirror_002ESceneOperation;
		Reader<CommandMessage>.read = _Read_Mirror_002ECommandMessage;
		Reader<RpcMessage>.read = _Read_Mirror_002ERpcMessage;
		Reader<RpcBufferMessage>.read = _Read_Mirror_002ERpcBufferMessage;
		Reader<SpawnMessage>.read = _Read_Mirror_002ESpawnMessage;
		Reader<ChangeOwnerMessage>.read = _Read_Mirror_002EChangeOwnerMessage;
		Reader<ObjectSpawnStartedMessage>.read = _Read_Mirror_002EObjectSpawnStartedMessage;
		Reader<ObjectSpawnFinishedMessage>.read = _Read_Mirror_002EObjectSpawnFinishedMessage;
		Reader<ObjectDestroyMessage>.read = _Read_Mirror_002EObjectDestroyMessage;
		Reader<ObjectHideMessage>.read = _Read_Mirror_002EObjectHideMessage;
		Reader<EntityStateMessage>.read = _Read_Mirror_002EEntityStateMessage;
		Reader<NetworkPingMessage>.read = _Read_Mirror_002ENetworkPingMessage;
		Reader<NetworkPongMessage>.read = _Read_Mirror_002ENetworkPongMessage;
		Reader<CastInfo>.read = _Read_CastInfo;
		Reader<Entity>.read = NetworkReaderExtensions.ReadNetworkBehaviour<Entity>;
		Reader<HatredStrengthType>.read = _Read_HatredStrengthType;
		Reader<QuestProgressType>.read = _Read_QuestProgressType;
		Reader<Projectile.EntityHit>.read = _Read_Projectile_002FEntityHit;
		Reader<Projectile.ProjectileMode>.read = _Read_Projectile_002FProjectileMode;
		Reader<AbilityTrigger.ConfigSyncData[]>.read = _Read_AbilityTrigger_002FConfigSyncData_005B_005D;
		Reader<AbilityTrigger.ConfigSyncData>.read = _Read_AbilityTrigger_002FConfigSyncData;
		Reader<CastMethodData>.read = _Read_CastMethodData;
		Reader<CastMethodType>.read = _Read_CastMethodType;
		Reader<EventInfoSkillUse>.read = _Read_EventInfoSkillUse;
		Reader<HeroSkillLocation>.read = _Read_HeroSkillLocation;
		Reader<SkillTrigger>.read = NetworkReaderExtensions.ReadNetworkBehaviour<SkillTrigger>;
		Reader<DescriptionTags>.read = _Read_DescriptionTags;
		Reader<EventInfoKill>.read = _Read_EventInfoKill;
		Reader<Actor>.read = NetworkReaderExtensions.ReadNetworkBehaviour<Actor>;
		Reader<Room_Waypoint>.read = NetworkReaderExtensions.ReadNetworkBehaviour<Room_Waypoint>;
		Reader<HeroLoadoutData>.read = _Read_HeroLoadoutData;
		Reader<List<LoadoutStarItem>>.read = _Read_System_002ECollections_002EGeneric_002EList_00601_003CLoadoutStarItem_003E;
		Reader<LoadoutStarItem>.read = _Read_LoadoutStarItem;
		Reader<Hero_Aurena.ClawState>.read = _Read_Hero_Aurena_002FClawState;
		Reader<Summon>.read = NetworkReaderExtensions.ReadNetworkBehaviour<Summon>;
		Reader<Monster.MonsterType>.read = _Read_Monster_002FMonsterType;
		Reader<DewDifficultySettings>.read = _Read_DewDifficultySettings;
		Reader<AnimationCurve>.read = _Read_UnityEngine_002EAnimationCurve;
		Reader<IntPtr>.read = _Read_System_002EIntPtr;
	}
}
