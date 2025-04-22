using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.Achievements;
using Epic.OnlineServices.AntiCheatClient;
using Epic.OnlineServices.AntiCheatCommon;
using Epic.OnlineServices.AntiCheatServer;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.CustomInvites;
using Epic.OnlineServices.Ecom;
using Epic.OnlineServices.Friends;
using Epic.OnlineServices.IntegratedPlatform;
using Epic.OnlineServices.KWS;
using Epic.OnlineServices.Leaderboards;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Metrics;
using Epic.OnlineServices.Mods;
using Epic.OnlineServices.P2P;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.PlayerDataStorage;
using Epic.OnlineServices.Presence;
using Epic.OnlineServices.ProgressionSnapshot;
using Epic.OnlineServices.Reports;
using Epic.OnlineServices.RTC;
using Epic.OnlineServices.RTCAdmin;
using Epic.OnlineServices.RTCAudio;
using Epic.OnlineServices.Sanctions;
using Epic.OnlineServices.Sessions;
using Epic.OnlineServices.Stats;
using Epic.OnlineServices.TitleStorage;
using Epic.OnlineServices.UI;
using Epic.OnlineServices.UserInfo;

namespace Epic.OnlineServices;

public static class Bindings
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Achievements_AddNotifyAchievementsUnlockedDelegate(IntPtr handle, ref AddNotifyAchievementsUnlockedOptionsInternal options, IntPtr clientData, OnAchievementsUnlockedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Achievements_AddNotifyAchievementsUnlockedV2Delegate(IntPtr handle, ref AddNotifyAchievementsUnlockedV2OptionsInternal options, IntPtr clientData, OnAchievementsUnlockedCallbackV2Internal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Achievements_CopyAchievementDefinitionByAchievementIdDelegate(IntPtr handle, ref CopyAchievementDefinitionByAchievementIdOptionsInternal options, ref IntPtr outDefinition);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Achievements_CopyAchievementDefinitionByIndexDelegate(IntPtr handle, ref CopyAchievementDefinitionByIndexOptionsInternal options, ref IntPtr outDefinition);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Achievements_CopyAchievementDefinitionV2ByAchievementIdDelegate(IntPtr handle, ref CopyAchievementDefinitionV2ByAchievementIdOptionsInternal options, ref IntPtr outDefinition);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Achievements_CopyAchievementDefinitionV2ByIndexDelegate(IntPtr handle, ref CopyAchievementDefinitionV2ByIndexOptionsInternal options, ref IntPtr outDefinition);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Achievements_CopyPlayerAchievementByAchievementIdDelegate(IntPtr handle, ref CopyPlayerAchievementByAchievementIdOptionsInternal options, ref IntPtr outAchievement);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Achievements_CopyPlayerAchievementByIndexDelegate(IntPtr handle, ref CopyPlayerAchievementByIndexOptionsInternal options, ref IntPtr outAchievement);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Achievements_CopyUnlockedAchievementByAchievementIdDelegate(IntPtr handle, ref CopyUnlockedAchievementByAchievementIdOptionsInternal options, ref IntPtr outAchievement);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Achievements_CopyUnlockedAchievementByIndexDelegate(IntPtr handle, ref CopyUnlockedAchievementByIndexOptionsInternal options, ref IntPtr outAchievement);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Achievements_DefinitionV2_ReleaseDelegate(IntPtr achievementDefinition);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Achievements_Definition_ReleaseDelegate(IntPtr achievementDefinition);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Achievements_GetAchievementDefinitionCountDelegate(IntPtr handle, ref GetAchievementDefinitionCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Achievements_GetPlayerAchievementCountDelegate(IntPtr handle, ref GetPlayerAchievementCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Achievements_GetUnlockedAchievementCountDelegate(IntPtr handle, ref GetUnlockedAchievementCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Achievements_PlayerAchievement_ReleaseDelegate(IntPtr achievement);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Achievements_QueryDefinitionsDelegate(IntPtr handle, ref QueryDefinitionsOptionsInternal options, IntPtr clientData, OnQueryDefinitionsCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Achievements_QueryPlayerAchievementsDelegate(IntPtr handle, ref QueryPlayerAchievementsOptionsInternal options, IntPtr clientData, OnQueryPlayerAchievementsCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Achievements_RemoveNotifyAchievementsUnlockedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Achievements_UnlockAchievementsDelegate(IntPtr handle, ref UnlockAchievementsOptionsInternal options, IntPtr clientData, OnUnlockAchievementsCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Achievements_UnlockedAchievement_ReleaseDelegate(IntPtr achievement);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_ActiveSession_CopyInfoDelegate(IntPtr handle, ref ActiveSessionCopyInfoOptionsInternal options, ref IntPtr outActiveSessionInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_ActiveSession_GetRegisteredPlayerByIndexDelegate(IntPtr handle, ref ActiveSessionGetRegisteredPlayerByIndexOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_ActiveSession_GetRegisteredPlayerCountDelegate(IntPtr handle, ref ActiveSessionGetRegisteredPlayerCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_ActiveSession_Info_ReleaseDelegate(IntPtr activeSessionInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_ActiveSession_ReleaseDelegate(IntPtr activeSessionHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatClient_AddExternalIntegrityCatalogDelegate(IntPtr handle, ref AddExternalIntegrityCatalogOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_AntiCheatClient_AddNotifyClientIntegrityViolatedDelegate(IntPtr handle, ref AddNotifyClientIntegrityViolatedOptionsInternal options, IntPtr clientData, OnClientIntegrityViolatedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_AntiCheatClient_AddNotifyMessageToPeerDelegate(IntPtr handle, ref AddNotifyMessageToPeerOptionsInternal options, IntPtr clientData, OnMessageToPeerCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_AntiCheatClient_AddNotifyMessageToServerDelegate(IntPtr handle, ref AddNotifyMessageToServerOptionsInternal options, IntPtr clientData, OnMessageToServerCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_AntiCheatClient_AddNotifyPeerActionRequiredDelegate(IntPtr handle, ref AddNotifyPeerActionRequiredOptionsInternal options, IntPtr clientData, OnPeerActionRequiredCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_AntiCheatClient_AddNotifyPeerAuthStatusChangedDelegate(IntPtr handle, ref AddNotifyPeerAuthStatusChangedOptionsInternal options, IntPtr clientData, OnPeerAuthStatusChangedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatClient_BeginSessionDelegate(IntPtr handle, ref global::Epic.OnlineServices.AntiCheatClient.BeginSessionOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatClient_EndSessionDelegate(IntPtr handle, ref global::Epic.OnlineServices.AntiCheatClient.EndSessionOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatClient_GetProtectMessageOutputLengthDelegate(IntPtr handle, ref global::Epic.OnlineServices.AntiCheatClient.GetProtectMessageOutputLengthOptionsInternal options, ref uint outBufferSizeBytes);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatClient_PollStatusDelegate(IntPtr handle, ref PollStatusOptionsInternal options, ref AntiCheatClientViolationType outViolationType, IntPtr outMessage);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatClient_ProtectMessageDelegate(IntPtr handle, ref global::Epic.OnlineServices.AntiCheatClient.ProtectMessageOptionsInternal options, IntPtr outBuffer, ref uint outBytesWritten);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatClient_ReceiveMessageFromPeerDelegate(IntPtr handle, ref ReceiveMessageFromPeerOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatClient_ReceiveMessageFromServerDelegate(IntPtr handle, ref ReceiveMessageFromServerOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatClient_RegisterPeerDelegate(IntPtr handle, ref RegisterPeerOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolatedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_AntiCheatClient_RemoveNotifyMessageToPeerDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_AntiCheatClient_RemoveNotifyMessageToServerDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_AntiCheatClient_RemoveNotifyPeerActionRequiredDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChangedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatClient_UnprotectMessageDelegate(IntPtr handle, ref global::Epic.OnlineServices.AntiCheatClient.UnprotectMessageOptionsInternal options, IntPtr outBuffer, ref uint outBytesWritten);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatClient_UnregisterPeerDelegate(IntPtr handle, ref UnregisterPeerOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_AntiCheatServer_AddNotifyClientActionRequiredDelegate(IntPtr handle, ref AddNotifyClientActionRequiredOptionsInternal options, IntPtr clientData, OnClientActionRequiredCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_AntiCheatServer_AddNotifyClientAuthStatusChangedDelegate(IntPtr handle, ref AddNotifyClientAuthStatusChangedOptionsInternal options, IntPtr clientData, OnClientAuthStatusChangedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_AntiCheatServer_AddNotifyMessageToClientDelegate(IntPtr handle, ref AddNotifyMessageToClientOptionsInternal options, IntPtr clientData, OnMessageToClientCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_BeginSessionDelegate(IntPtr handle, ref global::Epic.OnlineServices.AntiCheatServer.BeginSessionOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_EndSessionDelegate(IntPtr handle, ref global::Epic.OnlineServices.AntiCheatServer.EndSessionOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_GetProtectMessageOutputLengthDelegate(IntPtr handle, ref global::Epic.OnlineServices.AntiCheatServer.GetProtectMessageOutputLengthOptionsInternal options, ref uint outBufferSizeBytes);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_LogEventDelegate(IntPtr handle, ref LogEventOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_LogGameRoundEndDelegate(IntPtr handle, ref LogGameRoundEndOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_LogGameRoundStartDelegate(IntPtr handle, ref LogGameRoundStartOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_LogPlayerDespawnDelegate(IntPtr handle, ref LogPlayerDespawnOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_LogPlayerReviveDelegate(IntPtr handle, ref LogPlayerReviveOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_LogPlayerSpawnDelegate(IntPtr handle, ref LogPlayerSpawnOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_LogPlayerTakeDamageDelegate(IntPtr handle, ref LogPlayerTakeDamageOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_LogPlayerTickDelegate(IntPtr handle, ref LogPlayerTickOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_LogPlayerUseAbilityDelegate(IntPtr handle, ref LogPlayerUseAbilityOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_LogPlayerUseWeaponDelegate(IntPtr handle, ref LogPlayerUseWeaponOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_ProtectMessageDelegate(IntPtr handle, ref global::Epic.OnlineServices.AntiCheatServer.ProtectMessageOptionsInternal options, IntPtr outBuffer, ref uint outBytesWritten);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_ReceiveMessageFromClientDelegate(IntPtr handle, ref ReceiveMessageFromClientOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_RegisterClientDelegate(IntPtr handle, ref RegisterClientOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_RegisterEventDelegate(IntPtr handle, ref RegisterEventOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_AntiCheatServer_RemoveNotifyClientActionRequiredDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChangedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_AntiCheatServer_RemoveNotifyMessageToClientDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_SetClientDetailsDelegate(IntPtr handle, ref SetClientDetailsOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_SetClientNetworkStateDelegate(IntPtr handle, ref SetClientNetworkStateOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_SetGameSessionIdDelegate(IntPtr handle, ref SetGameSessionIdOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_UnprotectMessageDelegate(IntPtr handle, ref global::Epic.OnlineServices.AntiCheatServer.UnprotectMessageOptionsInternal options, IntPtr outBuffer, ref uint outBytesWritten);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_AntiCheatServer_UnregisterClientDelegate(IntPtr handle, ref UnregisterClientOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Auth_AddNotifyLoginStatusChangedDelegate(IntPtr handle, ref global::Epic.OnlineServices.Auth.AddNotifyLoginStatusChangedOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Auth.OnLoginStatusChangedCallbackInternal notification);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Auth_CopyIdTokenDelegate(IntPtr handle, ref global::Epic.OnlineServices.Auth.CopyIdTokenOptionsInternal options, ref IntPtr outIdToken);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Auth_CopyUserAuthTokenDelegate(IntPtr handle, ref CopyUserAuthTokenOptionsInternal options, IntPtr localUserId, ref IntPtr outUserAuthToken);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Auth_DeletePersistentAuthDelegate(IntPtr handle, ref DeletePersistentAuthOptionsInternal options, IntPtr clientData, OnDeletePersistentAuthCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Auth_GetLoggedInAccountByIndexDelegate(IntPtr handle, int index);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_Auth_GetLoggedInAccountsCountDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate LoginStatus EOS_Auth_GetLoginStatusDelegate(IntPtr handle, IntPtr localUserId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Auth_GetMergedAccountByIndexDelegate(IntPtr handle, IntPtr localUserId, uint index);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Auth_GetMergedAccountsCountDelegate(IntPtr handle, IntPtr localUserId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Auth_GetSelectedAccountIdDelegate(IntPtr handle, IntPtr localUserId, ref IntPtr outSelectedAccountId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Auth_IdToken_ReleaseDelegate(IntPtr idToken);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Auth_LinkAccountDelegate(IntPtr handle, ref global::Epic.OnlineServices.Auth.LinkAccountOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Auth.OnLinkAccountCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Auth_LoginDelegate(IntPtr handle, ref global::Epic.OnlineServices.Auth.LoginOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Auth.OnLoginCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Auth_LogoutDelegate(IntPtr handle, ref LogoutOptionsInternal options, IntPtr clientData, OnLogoutCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Auth_QueryIdTokenDelegate(IntPtr handle, ref QueryIdTokenOptionsInternal options, IntPtr clientData, OnQueryIdTokenCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Auth_RemoveNotifyLoginStatusChangedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Auth_Token_ReleaseDelegate(IntPtr authToken);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Auth_VerifyIdTokenDelegate(IntPtr handle, ref global::Epic.OnlineServices.Auth.VerifyIdTokenOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Auth.OnVerifyIdTokenCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Auth_VerifyUserAuthDelegate(IntPtr handle, ref VerifyUserAuthOptionsInternal options, IntPtr clientData, OnVerifyUserAuthCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_ByteArray_ToStringDelegate(IntPtr byteArray, uint length, IntPtr outBuffer, ref uint inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Connect_AddNotifyAuthExpirationDelegate(IntPtr handle, ref AddNotifyAuthExpirationOptionsInternal options, IntPtr clientData, OnAuthExpirationCallbackInternal notification);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Connect_AddNotifyLoginStatusChangedDelegate(IntPtr handle, ref global::Epic.OnlineServices.Connect.AddNotifyLoginStatusChangedOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Connect.OnLoginStatusChangedCallbackInternal notification);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Connect_CopyIdTokenDelegate(IntPtr handle, ref global::Epic.OnlineServices.Connect.CopyIdTokenOptionsInternal options, ref IntPtr outIdToken);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Connect_CopyProductUserExternalAccountByAccountIdDelegate(IntPtr handle, ref CopyProductUserExternalAccountByAccountIdOptionsInternal options, ref IntPtr outExternalAccountInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Connect_CopyProductUserExternalAccountByAccountTypeDelegate(IntPtr handle, ref CopyProductUserExternalAccountByAccountTypeOptionsInternal options, ref IntPtr outExternalAccountInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Connect_CopyProductUserExternalAccountByIndexDelegate(IntPtr handle, ref CopyProductUserExternalAccountByIndexOptionsInternal options, ref IntPtr outExternalAccountInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Connect_CopyProductUserInfoDelegate(IntPtr handle, ref CopyProductUserInfoOptionsInternal options, ref IntPtr outExternalAccountInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_CreateDeviceIdDelegate(IntPtr handle, ref CreateDeviceIdOptionsInternal options, IntPtr clientData, OnCreateDeviceIdCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_CreateUserDelegate(IntPtr handle, ref global::Epic.OnlineServices.Connect.CreateUserOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Connect.OnCreateUserCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_DeleteDeviceIdDelegate(IntPtr handle, ref DeleteDeviceIdOptionsInternal options, IntPtr clientData, OnDeleteDeviceIdCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_ExternalAccountInfo_ReleaseDelegate(IntPtr externalAccountInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Connect_GetExternalAccountMappingDelegate(IntPtr handle, ref GetExternalAccountMappingsOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Connect_GetLoggedInUserByIndexDelegate(IntPtr handle, int index);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_Connect_GetLoggedInUsersCountDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate LoginStatus EOS_Connect_GetLoginStatusDelegate(IntPtr handle, IntPtr localUserId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Connect_GetProductUserExternalAccountCountDelegate(IntPtr handle, ref GetProductUserExternalAccountCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Connect_GetProductUserIdMappingDelegate(IntPtr handle, ref GetProductUserIdMappingOptionsInternal options, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_IdToken_ReleaseDelegate(IntPtr idToken);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_LinkAccountDelegate(IntPtr handle, ref global::Epic.OnlineServices.Connect.LinkAccountOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Connect.OnLinkAccountCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_LoginDelegate(IntPtr handle, ref global::Epic.OnlineServices.Connect.LoginOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Connect.OnLoginCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_QueryExternalAccountMappingsDelegate(IntPtr handle, ref QueryExternalAccountMappingsOptionsInternal options, IntPtr clientData, OnQueryExternalAccountMappingsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_QueryProductUserIdMappingsDelegate(IntPtr handle, ref QueryProductUserIdMappingsOptionsInternal options, IntPtr clientData, OnQueryProductUserIdMappingsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_RemoveNotifyAuthExpirationDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_RemoveNotifyLoginStatusChangedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_TransferDeviceIdAccountDelegate(IntPtr handle, ref TransferDeviceIdAccountOptionsInternal options, IntPtr clientData, OnTransferDeviceIdAccountCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_UnlinkAccountDelegate(IntPtr handle, ref UnlinkAccountOptionsInternal options, IntPtr clientData, OnUnlinkAccountCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Connect_VerifyIdTokenDelegate(IntPtr handle, ref global::Epic.OnlineServices.Connect.VerifyIdTokenOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Connect.OnVerifyIdTokenCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_ContinuanceToken_ToStringDelegate(IntPtr continuanceToken, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_AcceptRequestToJoinDelegate(IntPtr handle, ref AcceptRequestToJoinOptionsInternal options, IntPtr clientData, OnAcceptRequestToJoinCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_CustomInvites_AddNotifyCustomInviteAcceptedDelegate(IntPtr handle, ref AddNotifyCustomInviteAcceptedOptionsInternal options, IntPtr clientData, OnCustomInviteAcceptedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_CustomInvites_AddNotifyCustomInviteReceivedDelegate(IntPtr handle, ref AddNotifyCustomInviteReceivedOptionsInternal options, IntPtr clientData, OnCustomInviteReceivedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_CustomInvites_AddNotifyCustomInviteRejectedDelegate(IntPtr handle, ref AddNotifyCustomInviteRejectedOptionsInternal options, IntPtr clientData, OnCustomInviteRejectedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_CustomInvites_AddNotifyRequestToJoinAcceptedDelegate(IntPtr handle, ref AddNotifyRequestToJoinAcceptedOptionsInternal options, IntPtr clientData, OnRequestToJoinAcceptedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_CustomInvites_AddNotifyRequestToJoinReceivedDelegate(IntPtr handle, ref AddNotifyRequestToJoinReceivedOptionsInternal options, IntPtr clientData, OnRequestToJoinReceivedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_CustomInvites_AddNotifyRequestToJoinRejectedDelegate(IntPtr handle, ref AddNotifyRequestToJoinRejectedOptionsInternal options, IntPtr clientData, OnRequestToJoinRejectedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_CustomInvites_AddNotifyRequestToJoinResponseReceivedDelegate(IntPtr handle, ref AddNotifyRequestToJoinResponseReceivedOptionsInternal options, IntPtr clientData, OnRequestToJoinResponseReceivedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_CustomInvites_AddNotifySendCustomNativeInviteRequestedDelegate(IntPtr handle, ref AddNotifySendCustomNativeInviteRequestedOptionsInternal options, IntPtr clientData, OnSendCustomNativeInviteRequestedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_CustomInvites_FinalizeInviteDelegate(IntPtr handle, ref FinalizeInviteOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_RejectRequestToJoinDelegate(IntPtr handle, ref RejectRequestToJoinOptionsInternal options, IntPtr clientData, OnRejectRequestToJoinCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_RemoveNotifyCustomInviteAcceptedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_RemoveNotifyCustomInviteReceivedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_RemoveNotifyCustomInviteRejectedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_RemoveNotifyRequestToJoinAcceptedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_RemoveNotifyRequestToJoinReceivedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_RemoveNotifyRequestToJoinRejectedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceivedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequestedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_SendCustomInviteDelegate(IntPtr handle, ref SendCustomInviteOptionsInternal options, IntPtr clientData, OnSendCustomInviteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_CustomInvites_SendRequestToJoinDelegate(IntPtr handle, ref SendRequestToJoinOptionsInternal options, IntPtr clientData, OnSendRequestToJoinCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_CustomInvites_SetCustomInviteDelegate(IntPtr handle, ref SetCustomInviteOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_EApplicationStatus_ToStringDelegate(ApplicationStatus applicationStatus);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_ENetworkStatus_ToStringDelegate(NetworkStatus networkStatus);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_EResult_IsOperationCompleteDelegate(Result result);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_EResult_ToStringDelegate(Result result);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_CatalogItem_ReleaseDelegate(IntPtr catalogItem);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_CatalogOffer_ReleaseDelegate(IntPtr catalogOffer);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_CatalogRelease_ReleaseDelegate(IntPtr catalogRelease);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_CheckoutDelegate(IntPtr handle, ref CheckoutOptionsInternal options, IntPtr clientData, OnCheckoutCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyEntitlementByIdDelegate(IntPtr handle, ref CopyEntitlementByIdOptionsInternal options, ref IntPtr outEntitlement);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyEntitlementByIndexDelegate(IntPtr handle, ref CopyEntitlementByIndexOptionsInternal options, ref IntPtr outEntitlement);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyEntitlementByNameAndIndexDelegate(IntPtr handle, ref CopyEntitlementByNameAndIndexOptionsInternal options, ref IntPtr outEntitlement);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyItemByIdDelegate(IntPtr handle, ref CopyItemByIdOptionsInternal options, ref IntPtr outItem);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyItemImageInfoByIndexDelegate(IntPtr handle, ref CopyItemImageInfoByIndexOptionsInternal options, ref IntPtr outImageInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyItemReleaseByIndexDelegate(IntPtr handle, ref CopyItemReleaseByIndexOptionsInternal options, ref IntPtr outRelease);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyLastRedeemedEntitlementByIndexDelegate(IntPtr handle, ref CopyLastRedeemedEntitlementByIndexOptionsInternal options, IntPtr outRedeemedEntitlementId, ref int inOutRedeemedEntitlementIdLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyOfferByIdDelegate(IntPtr handle, ref CopyOfferByIdOptionsInternal options, ref IntPtr outOffer);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyOfferByIndexDelegate(IntPtr handle, ref CopyOfferByIndexOptionsInternal options, ref IntPtr outOffer);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyOfferImageInfoByIndexDelegate(IntPtr handle, ref CopyOfferImageInfoByIndexOptionsInternal options, ref IntPtr outImageInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyOfferItemByIndexDelegate(IntPtr handle, ref CopyOfferItemByIndexOptionsInternal options, ref IntPtr outItem);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyTransactionByIdDelegate(IntPtr handle, ref CopyTransactionByIdOptionsInternal options, ref IntPtr outTransaction);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_CopyTransactionByIndexDelegate(IntPtr handle, ref CopyTransactionByIndexOptionsInternal options, ref IntPtr outTransaction);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_Entitlement_ReleaseDelegate(IntPtr entitlement);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Ecom_GetEntitlementsByNameCountDelegate(IntPtr handle, ref GetEntitlementsByNameCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Ecom_GetEntitlementsCountDelegate(IntPtr handle, ref GetEntitlementsCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Ecom_GetItemImageInfoCountDelegate(IntPtr handle, ref GetItemImageInfoCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Ecom_GetItemReleaseCountDelegate(IntPtr handle, ref GetItemReleaseCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Ecom_GetLastRedeemedEntitlementsCountDelegate(IntPtr handle, ref GetLastRedeemedEntitlementsCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Ecom_GetOfferCountDelegate(IntPtr handle, ref GetOfferCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Ecom_GetOfferImageInfoCountDelegate(IntPtr handle, ref GetOfferImageInfoCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Ecom_GetOfferItemCountDelegate(IntPtr handle, ref GetOfferItemCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Ecom_GetTransactionCountDelegate(IntPtr handle, ref GetTransactionCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_KeyImageInfo_ReleaseDelegate(IntPtr keyImageInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_QueryEntitlementTokenDelegate(IntPtr handle, ref QueryEntitlementTokenOptionsInternal options, IntPtr clientData, OnQueryEntitlementTokenCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_QueryEntitlementsDelegate(IntPtr handle, ref QueryEntitlementsOptionsInternal options, IntPtr clientData, OnQueryEntitlementsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_QueryOffersDelegate(IntPtr handle, ref QueryOffersOptionsInternal options, IntPtr clientData, OnQueryOffersCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_QueryOwnershipDelegate(IntPtr handle, ref QueryOwnershipOptionsInternal options, IntPtr clientData, OnQueryOwnershipCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_QueryOwnershipBySandboxIdsDelegate(IntPtr handle, ref QueryOwnershipBySandboxIdsOptionsInternal options, IntPtr clientData, OnQueryOwnershipBySandboxIdsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_QueryOwnershipTokenDelegate(IntPtr handle, ref QueryOwnershipTokenOptionsInternal options, IntPtr clientData, OnQueryOwnershipTokenCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_RedeemEntitlementsDelegate(IntPtr handle, ref RedeemEntitlementsOptionsInternal options, IntPtr clientData, OnRedeemEntitlementsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_Transaction_CopyEntitlementByIndexDelegate(IntPtr handle, ref TransactionCopyEntitlementByIndexOptionsInternal options, ref IntPtr outEntitlement);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Ecom_Transaction_GetEntitlementsCountDelegate(IntPtr handle, ref TransactionGetEntitlementsCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Ecom_Transaction_GetTransactionIdDelegate(IntPtr handle, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Ecom_Transaction_ReleaseDelegate(IntPtr transaction);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_EpicAccountId_FromStringDelegate(IntPtr accountIdString);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_EpicAccountId_IsValidDelegate(IntPtr accountId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_EpicAccountId_ToStringDelegate(IntPtr accountId, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Friends_AcceptInviteDelegate(IntPtr handle, ref AcceptInviteOptionsInternal options, IntPtr clientData, OnAcceptInviteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Friends_AddNotifyBlockedUsersUpdateDelegate(IntPtr handle, ref AddNotifyBlockedUsersUpdateOptionsInternal options, IntPtr clientData, OnBlockedUsersUpdateCallbackInternal blockedUsersUpdateHandler);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Friends_AddNotifyFriendsUpdateDelegate(IntPtr handle, ref AddNotifyFriendsUpdateOptionsInternal options, IntPtr clientData, OnFriendsUpdateCallbackInternal friendsUpdateHandler);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Friends_GetBlockedUserAtIndexDelegate(IntPtr handle, ref GetBlockedUserAtIndexOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_Friends_GetBlockedUsersCountDelegate(IntPtr handle, ref GetBlockedUsersCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Friends_GetFriendAtIndexDelegate(IntPtr handle, ref GetFriendAtIndexOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_Friends_GetFriendsCountDelegate(IntPtr handle, ref GetFriendsCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate FriendsStatus EOS_Friends_GetStatusDelegate(IntPtr handle, ref GetStatusOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Friends_QueryFriendsDelegate(IntPtr handle, ref QueryFriendsOptionsInternal options, IntPtr clientData, OnQueryFriendsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Friends_RejectInviteDelegate(IntPtr handle, ref global::Epic.OnlineServices.Friends.RejectInviteOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Friends.OnRejectInviteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Friends_RemoveNotifyBlockedUsersUpdateDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Friends_RemoveNotifyFriendsUpdateDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Friends_SendInviteDelegate(IntPtr handle, ref global::Epic.OnlineServices.Friends.SendInviteOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Friends.OnSendInviteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_GetVersionDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_InitializeDelegate(ref InitializeOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_IntegratedPlatformOptionsContainer_AddDelegate(IntPtr handle, ref IntegratedPlatformOptionsContainerAddOptionsInternal inOptions);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_IntegratedPlatformOptionsContainer_ReleaseDelegate(IntPtr integratedPlatformOptionsContainerHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_IntegratedPlatform_AddNotifyUserLoginStatusChangedDelegate(IntPtr handle, ref AddNotifyUserLoginStatusChangedOptionsInternal options, IntPtr clientData, OnUserLoginStatusChangedCallbackInternal callbackFunction);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_IntegratedPlatform_ClearUserPreLogoutCallbackDelegate(IntPtr handle, ref ClearUserPreLogoutCallbackOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainerDelegate(ref CreateIntegratedPlatformOptionsContainerOptionsInternal options, ref IntPtr outIntegratedPlatformOptionsContainerHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_IntegratedPlatform_FinalizeDeferredUserLogoutDelegate(IntPtr handle, ref FinalizeDeferredUserLogoutOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChangedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_IntegratedPlatform_SetUserLoginStatusDelegate(IntPtr handle, ref SetUserLoginStatusOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_IntegratedPlatform_SetUserPreLogoutCallbackDelegate(IntPtr handle, ref SetUserPreLogoutCallbackOptionsInternal options, IntPtr clientData, OnUserPreLogoutCallbackInternal callbackFunction);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_KWS_AddNotifyPermissionsUpdateReceivedDelegate(IntPtr handle, ref AddNotifyPermissionsUpdateReceivedOptionsInternal options, IntPtr clientData, OnPermissionsUpdateReceivedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_KWS_CopyPermissionByIndexDelegate(IntPtr handle, ref CopyPermissionByIndexOptionsInternal options, ref IntPtr outPermission);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_KWS_CreateUserDelegate(IntPtr handle, ref global::Epic.OnlineServices.KWS.CreateUserOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.KWS.OnCreateUserCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_KWS_GetPermissionByKeyDelegate(IntPtr handle, ref GetPermissionByKeyOptionsInternal options, ref KWSPermissionStatus outPermission);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_KWS_GetPermissionsCountDelegate(IntPtr handle, ref GetPermissionsCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_KWS_PermissionStatus_ReleaseDelegate(IntPtr permissionStatus);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_KWS_QueryAgeGateDelegate(IntPtr handle, ref QueryAgeGateOptionsInternal options, IntPtr clientData, OnQueryAgeGateCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_KWS_QueryPermissionsDelegate(IntPtr handle, ref QueryPermissionsOptionsInternal options, IntPtr clientData, OnQueryPermissionsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_KWS_RemoveNotifyPermissionsUpdateReceivedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_KWS_RequestPermissionsDelegate(IntPtr handle, ref RequestPermissionsOptionsInternal options, IntPtr clientData, OnRequestPermissionsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_KWS_UpdateParentEmailDelegate(IntPtr handle, ref UpdateParentEmailOptionsInternal options, IntPtr clientData, OnUpdateParentEmailCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Leaderboards_CopyLeaderboardDefinitionByIndexDelegate(IntPtr handle, ref CopyLeaderboardDefinitionByIndexOptionsInternal options, ref IntPtr outLeaderboardDefinition);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardIdDelegate(IntPtr handle, ref CopyLeaderboardDefinitionByLeaderboardIdOptionsInternal options, ref IntPtr outLeaderboardDefinition);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Leaderboards_CopyLeaderboardRecordByIndexDelegate(IntPtr handle, ref CopyLeaderboardRecordByIndexOptionsInternal options, ref IntPtr outLeaderboardRecord);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Leaderboards_CopyLeaderboardRecordByUserIdDelegate(IntPtr handle, ref CopyLeaderboardRecordByUserIdOptionsInternal options, ref IntPtr outLeaderboardRecord);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Leaderboards_CopyLeaderboardUserScoreByIndexDelegate(IntPtr handle, ref CopyLeaderboardUserScoreByIndexOptionsInternal options, ref IntPtr outLeaderboardUserScore);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Leaderboards_CopyLeaderboardUserScoreByUserIdDelegate(IntPtr handle, ref CopyLeaderboardUserScoreByUserIdOptionsInternal options, ref IntPtr outLeaderboardUserScore);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Leaderboards_Definition_ReleaseDelegate(IntPtr leaderboardDefinition);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Leaderboards_GetLeaderboardDefinitionCountDelegate(IntPtr handle, ref GetLeaderboardDefinitionCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Leaderboards_GetLeaderboardRecordCountDelegate(IntPtr handle, ref GetLeaderboardRecordCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Leaderboards_GetLeaderboardUserScoreCountDelegate(IntPtr handle, ref GetLeaderboardUserScoreCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Leaderboards_LeaderboardDefinition_ReleaseDelegate(IntPtr leaderboardDefinition);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Leaderboards_LeaderboardRecord_ReleaseDelegate(IntPtr leaderboardRecord);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Leaderboards_LeaderboardUserScore_ReleaseDelegate(IntPtr leaderboardUserScore);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Leaderboards_QueryLeaderboardDefinitionsDelegate(IntPtr handle, ref QueryLeaderboardDefinitionsOptionsInternal options, IntPtr clientData, OnQueryLeaderboardDefinitionsCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Leaderboards_QueryLeaderboardRanksDelegate(IntPtr handle, ref QueryLeaderboardRanksOptionsInternal options, IntPtr clientData, OnQueryLeaderboardRanksCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Leaderboards_QueryLeaderboardUserScoresDelegate(IntPtr handle, ref QueryLeaderboardUserScoresOptionsInternal options, IntPtr clientData, OnQueryLeaderboardUserScoresCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyDetails_CopyAttributeByIndexDelegate(IntPtr handle, ref LobbyDetailsCopyAttributeByIndexOptionsInternal options, ref IntPtr outAttribute);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyDetails_CopyAttributeByKeyDelegate(IntPtr handle, ref LobbyDetailsCopyAttributeByKeyOptionsInternal options, ref IntPtr outAttribute);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyDetails_CopyInfoDelegate(IntPtr handle, ref LobbyDetailsCopyInfoOptionsInternal options, ref IntPtr outLobbyDetailsInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyDetails_CopyMemberAttributeByIndexDelegate(IntPtr handle, ref LobbyDetailsCopyMemberAttributeByIndexOptionsInternal options, ref IntPtr outAttribute);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyDetails_CopyMemberAttributeByKeyDelegate(IntPtr handle, ref LobbyDetailsCopyMemberAttributeByKeyOptionsInternal options, ref IntPtr outAttribute);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyDetails_CopyMemberInfoDelegate(IntPtr handle, ref LobbyDetailsCopyMemberInfoOptionsInternal options, ref IntPtr outLobbyDetailsMemberInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_LobbyDetails_GetAttributeCountDelegate(IntPtr handle, ref LobbyDetailsGetAttributeCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_LobbyDetails_GetLobbyOwnerDelegate(IntPtr handle, ref LobbyDetailsGetLobbyOwnerOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_LobbyDetails_GetMemberAttributeCountDelegate(IntPtr handle, ref LobbyDetailsGetMemberAttributeCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_LobbyDetails_GetMemberByIndexDelegate(IntPtr handle, ref LobbyDetailsGetMemberByIndexOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_LobbyDetails_GetMemberCountDelegate(IntPtr handle, ref LobbyDetailsGetMemberCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_LobbyDetails_Info_ReleaseDelegate(IntPtr lobbyDetailsInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_LobbyDetails_MemberInfo_ReleaseDelegate(IntPtr lobbyDetailsMemberInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_LobbyDetails_ReleaseDelegate(IntPtr lobbyHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyModification_AddAttributeDelegate(IntPtr handle, ref LobbyModificationAddAttributeOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyModification_AddMemberAttributeDelegate(IntPtr handle, ref LobbyModificationAddMemberAttributeOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_LobbyModification_ReleaseDelegate(IntPtr lobbyModificationHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyModification_RemoveAttributeDelegate(IntPtr handle, ref LobbyModificationRemoveAttributeOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyModification_RemoveMemberAttributeDelegate(IntPtr handle, ref LobbyModificationRemoveMemberAttributeOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyModification_SetAllowedPlatformIdsDelegate(IntPtr handle, ref LobbyModificationSetAllowedPlatformIdsOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyModification_SetBucketIdDelegate(IntPtr handle, ref LobbyModificationSetBucketIdOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyModification_SetInvitesAllowedDelegate(IntPtr handle, ref LobbyModificationSetInvitesAllowedOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyModification_SetMaxMembersDelegate(IntPtr handle, ref LobbyModificationSetMaxMembersOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbyModification_SetPermissionLevelDelegate(IntPtr handle, ref LobbyModificationSetPermissionLevelOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbySearch_CopySearchResultByIndexDelegate(IntPtr handle, ref LobbySearchCopySearchResultByIndexOptionsInternal options, ref IntPtr outLobbyDetailsHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_LobbySearch_FindDelegate(IntPtr handle, ref LobbySearchFindOptionsInternal options, IntPtr clientData, LobbySearchOnFindCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_LobbySearch_GetSearchResultCountDelegate(IntPtr handle, ref LobbySearchGetSearchResultCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_LobbySearch_ReleaseDelegate(IntPtr lobbySearchHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbySearch_RemoveParameterDelegate(IntPtr handle, ref LobbySearchRemoveParameterOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbySearch_SetLobbyIdDelegate(IntPtr handle, ref LobbySearchSetLobbyIdOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbySearch_SetMaxResultsDelegate(IntPtr handle, ref LobbySearchSetMaxResultsOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbySearch_SetParameterDelegate(IntPtr handle, ref LobbySearchSetParameterOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_LobbySearch_SetTargetUserIdDelegate(IntPtr handle, ref LobbySearchSetTargetUserIdOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Lobby_AddNotifyJoinLobbyAcceptedDelegate(IntPtr handle, ref AddNotifyJoinLobbyAcceptedOptionsInternal options, IntPtr clientData, OnJoinLobbyAcceptedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Lobby_AddNotifyLeaveLobbyRequestedDelegate(IntPtr handle, ref AddNotifyLeaveLobbyRequestedOptionsInternal options, IntPtr clientData, OnLeaveLobbyRequestedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Lobby_AddNotifyLobbyInviteAcceptedDelegate(IntPtr handle, ref AddNotifyLobbyInviteAcceptedOptionsInternal options, IntPtr clientData, OnLobbyInviteAcceptedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Lobby_AddNotifyLobbyInviteReceivedDelegate(IntPtr handle, ref AddNotifyLobbyInviteReceivedOptionsInternal options, IntPtr clientData, OnLobbyInviteReceivedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Lobby_AddNotifyLobbyInviteRejectedDelegate(IntPtr handle, ref AddNotifyLobbyInviteRejectedOptionsInternal options, IntPtr clientData, OnLobbyInviteRejectedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Lobby_AddNotifyLobbyMemberStatusReceivedDelegate(IntPtr handle, ref AddNotifyLobbyMemberStatusReceivedOptionsInternal options, IntPtr clientData, OnLobbyMemberStatusReceivedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Lobby_AddNotifyLobbyMemberUpdateReceivedDelegate(IntPtr handle, ref AddNotifyLobbyMemberUpdateReceivedOptionsInternal options, IntPtr clientData, OnLobbyMemberUpdateReceivedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Lobby_AddNotifyLobbyUpdateReceivedDelegate(IntPtr handle, ref AddNotifyLobbyUpdateReceivedOptionsInternal options, IntPtr clientData, OnLobbyUpdateReceivedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Lobby_AddNotifyRTCRoomConnectionChangedDelegate(IntPtr handle, ref AddNotifyRTCRoomConnectionChangedOptionsInternal options, IntPtr clientData, OnRTCRoomConnectionChangedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Lobby_AddNotifySendLobbyNativeInviteRequestedDelegate(IntPtr handle, ref AddNotifySendLobbyNativeInviteRequestedOptionsInternal options, IntPtr clientData, OnSendLobbyNativeInviteRequestedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_Attribute_ReleaseDelegate(IntPtr lobbyAttribute);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Lobby_CopyLobbyDetailsHandleDelegate(IntPtr handle, ref CopyLobbyDetailsHandleOptionsInternal options, ref IntPtr outLobbyDetailsHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Lobby_CopyLobbyDetailsHandleByInviteIdDelegate(IntPtr handle, ref CopyLobbyDetailsHandleByInviteIdOptionsInternal options, ref IntPtr outLobbyDetailsHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Lobby_CopyLobbyDetailsHandleByUiEventIdDelegate(IntPtr handle, ref CopyLobbyDetailsHandleByUiEventIdOptionsInternal options, ref IntPtr outLobbyDetailsHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_CreateLobbyDelegate(IntPtr handle, ref CreateLobbyOptionsInternal options, IntPtr clientData, OnCreateLobbyCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Lobby_CreateLobbySearchDelegate(IntPtr handle, ref CreateLobbySearchOptionsInternal options, ref IntPtr outLobbySearchHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_DestroyLobbyDelegate(IntPtr handle, ref DestroyLobbyOptionsInternal options, IntPtr clientData, OnDestroyLobbyCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Lobby_GetConnectStringDelegate(IntPtr handle, ref GetConnectStringOptionsInternal options, IntPtr outBuffer, ref uint inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Lobby_GetInviteCountDelegate(IntPtr handle, ref global::Epic.OnlineServices.Lobby.GetInviteCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Lobby_GetInviteIdByIndexDelegate(IntPtr handle, ref global::Epic.OnlineServices.Lobby.GetInviteIdByIndexOptionsInternal options, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Lobby_GetRTCRoomNameDelegate(IntPtr handle, ref GetRTCRoomNameOptionsInternal options, IntPtr outBuffer, ref uint inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_HardMuteMemberDelegate(IntPtr handle, ref HardMuteMemberOptionsInternal options, IntPtr clientData, OnHardMuteMemberCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Lobby_IsRTCRoomConnectedDelegate(IntPtr handle, ref IsRTCRoomConnectedOptionsInternal options, ref int bOutIsConnected);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_JoinLobbyDelegate(IntPtr handle, ref JoinLobbyOptionsInternal options, IntPtr clientData, OnJoinLobbyCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_JoinLobbyByIdDelegate(IntPtr handle, ref JoinLobbyByIdOptionsInternal options, IntPtr clientData, OnJoinLobbyByIdCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_KickMemberDelegate(IntPtr handle, ref KickMemberOptionsInternal options, IntPtr clientData, OnKickMemberCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_LeaveLobbyDelegate(IntPtr handle, ref LeaveLobbyOptionsInternal options, IntPtr clientData, OnLeaveLobbyCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Lobby_ParseConnectStringDelegate(IntPtr handle, ref ParseConnectStringOptionsInternal options, IntPtr outBuffer, ref uint inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_PromoteMemberDelegate(IntPtr handle, ref PromoteMemberOptionsInternal options, IntPtr clientData, OnPromoteMemberCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_QueryInvitesDelegate(IntPtr handle, ref global::Epic.OnlineServices.Lobby.QueryInvitesOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Lobby.OnQueryInvitesCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_RejectInviteDelegate(IntPtr handle, ref global::Epic.OnlineServices.Lobby.RejectInviteOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Lobby.OnRejectInviteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_RemoveNotifyJoinLobbyAcceptedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_RemoveNotifyLeaveLobbyRequestedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_RemoveNotifyLobbyInviteAcceptedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_RemoveNotifyLobbyInviteReceivedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_RemoveNotifyLobbyInviteRejectedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_RemoveNotifyLobbyMemberStatusReceivedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceivedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_RemoveNotifyLobbyUpdateReceivedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_RemoveNotifyRTCRoomConnectionChangedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequestedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_SendInviteDelegate(IntPtr handle, ref global::Epic.OnlineServices.Lobby.SendInviteOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Lobby.OnSendInviteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Lobby_UpdateLobbyDelegate(IntPtr handle, ref UpdateLobbyOptionsInternal options, IntPtr clientData, OnUpdateLobbyCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Lobby_UpdateLobbyModificationDelegate(IntPtr handle, ref UpdateLobbyModificationOptionsInternal options, ref IntPtr outLobbyModificationHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Logging_SetCallbackDelegate(LogMessageFuncInternal callback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Logging_SetLogLevelDelegate(LogCategory logCategory, LogLevel logLevel);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Metrics_BeginPlayerSessionDelegate(IntPtr handle, ref BeginPlayerSessionOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Metrics_EndPlayerSessionDelegate(IntPtr handle, ref EndPlayerSessionOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Mods_CopyModInfoDelegate(IntPtr handle, ref CopyModInfoOptionsInternal options, ref IntPtr outEnumeratedMods);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Mods_EnumerateModsDelegate(IntPtr handle, ref EnumerateModsOptionsInternal options, IntPtr clientData, OnEnumerateModsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Mods_InstallModDelegate(IntPtr handle, ref InstallModOptionsInternal options, IntPtr clientData, OnInstallModCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Mods_ModInfo_ReleaseDelegate(IntPtr modInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Mods_UninstallModDelegate(IntPtr handle, ref UninstallModOptionsInternal options, IntPtr clientData, OnUninstallModCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Mods_UpdateModDelegate(IntPtr handle, ref UpdateModOptionsInternal options, IntPtr clientData, OnUpdateModCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_AcceptConnectionDelegate(IntPtr handle, ref AcceptConnectionOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_P2P_AddNotifyIncomingPacketQueueFullDelegate(IntPtr handle, ref AddNotifyIncomingPacketQueueFullOptionsInternal options, IntPtr clientData, OnIncomingPacketQueueFullCallbackInternal incomingPacketQueueFullHandler);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_P2P_AddNotifyPeerConnectionClosedDelegate(IntPtr handle, ref AddNotifyPeerConnectionClosedOptionsInternal options, IntPtr clientData, OnRemoteConnectionClosedCallbackInternal connectionClosedHandler);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_P2P_AddNotifyPeerConnectionEstablishedDelegate(IntPtr handle, ref AddNotifyPeerConnectionEstablishedOptionsInternal options, IntPtr clientData, OnPeerConnectionEstablishedCallbackInternal connectionEstablishedHandler);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_P2P_AddNotifyPeerConnectionInterruptedDelegate(IntPtr handle, ref AddNotifyPeerConnectionInterruptedOptionsInternal options, IntPtr clientData, OnPeerConnectionInterruptedCallbackInternal connectionInterruptedHandler);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_P2P_AddNotifyPeerConnectionRequestDelegate(IntPtr handle, ref AddNotifyPeerConnectionRequestOptionsInternal options, IntPtr clientData, OnIncomingConnectionRequestCallbackInternal connectionRequestHandler);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_ClearPacketQueueDelegate(IntPtr handle, ref ClearPacketQueueOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_CloseConnectionDelegate(IntPtr handle, ref CloseConnectionOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_CloseConnectionsDelegate(IntPtr handle, ref CloseConnectionsOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_GetNATTypeDelegate(IntPtr handle, ref GetNATTypeOptionsInternal options, ref NATType outNATType);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_GetNextReceivedPacketSizeDelegate(IntPtr handle, ref GetNextReceivedPacketSizeOptionsInternal options, ref uint outPacketSizeBytes);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_GetPacketQueueInfoDelegate(IntPtr handle, ref GetPacketQueueInfoOptionsInternal options, ref PacketQueueInfoInternal outPacketQueueInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_GetPortRangeDelegate(IntPtr handle, ref GetPortRangeOptionsInternal options, ref ushort outPort, ref ushort outNumAdditionalPortsToTry);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_GetRelayControlDelegate(IntPtr handle, ref GetRelayControlOptionsInternal options, ref RelayControl outRelayControl);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_P2P_QueryNATTypeDelegate(IntPtr handle, ref QueryNATTypeOptionsInternal options, IntPtr clientData, OnQueryNATTypeCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_ReceivePacketDelegate(IntPtr handle, ref ReceivePacketOptionsInternal options, ref IntPtr outPeerId, ref SocketIdInternal outSocketId, ref byte outChannel, IntPtr outData, ref uint outBytesWritten);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_P2P_RemoveNotifyIncomingPacketQueueFullDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_P2P_RemoveNotifyPeerConnectionClosedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_P2P_RemoveNotifyPeerConnectionEstablishedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_P2P_RemoveNotifyPeerConnectionInterruptedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_P2P_RemoveNotifyPeerConnectionRequestDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_SendPacketDelegate(IntPtr handle, ref SendPacketOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_SetPacketQueueSizeDelegate(IntPtr handle, ref SetPacketQueueSizeOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_SetPortRangeDelegate(IntPtr handle, ref SetPortRangeOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_P2P_SetRelayControlDelegate(IntPtr handle, ref SetRelayControlOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Platform_CheckForLauncherAndRestartDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_CreateDelegate(ref global::Epic.OnlineServices.Platform.OptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetAchievementsInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Platform_GetActiveCountryCodeDelegate(IntPtr handle, IntPtr localUserId, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Platform_GetActiveLocaleCodeDelegate(IntPtr handle, IntPtr localUserId, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetAntiCheatClientInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetAntiCheatServerInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ApplicationStatus EOS_Platform_GetApplicationStatusDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetAuthInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetConnectInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetCustomInvitesInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Platform_GetDesktopCrossplayStatusDelegate(IntPtr handle, ref GetDesktopCrossplayStatusOptionsInternal options, ref DesktopCrossplayStatusInfoInternal outDesktopCrossplayStatusInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetEcomInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetFriendsInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetIntegratedPlatformInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetKWSInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetLeaderboardsInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetLobbyInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetMetricsInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetModsInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate NetworkStatus EOS_Platform_GetNetworkStatusDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Platform_GetOverrideCountryCodeDelegate(IntPtr handle, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Platform_GetOverrideLocaleCodeDelegate(IntPtr handle, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetP2PInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetPlayerDataStorageInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetPresenceInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetProgressionSnapshotInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetRTCAdminInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetRTCInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetReportsInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetSanctionsInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetSessionsInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetStatsInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetTitleStorageInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetUIInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_Platform_GetUserInfoInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Platform_ReleaseDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Platform_SetApplicationStatusDelegate(IntPtr handle, ApplicationStatus newStatus);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Platform_SetNetworkStatusDelegate(IntPtr handle, NetworkStatus newStatus);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Platform_SetOverrideCountryCodeDelegate(IntPtr handle, IntPtr newCountryCode);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Platform_SetOverrideLocaleCodeDelegate(IntPtr handle, IntPtr newLocaleCode);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Platform_TickDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PlayerDataStorageFileTransferRequest_CancelRequestDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PlayerDataStorageFileTransferRequest_GetFileRequestStateDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PlayerDataStorageFileTransferRequest_GetFilenameDelegate(IntPtr handle, uint filenameStringBufferSizeBytes, IntPtr outStringBuffer, ref int outStringLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_PlayerDataStorageFileTransferRequest_ReleaseDelegate(IntPtr playerDataStorageFileTransferHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PlayerDataStorage_CopyFileMetadataAtIndexDelegate(IntPtr handle, ref global::Epic.OnlineServices.PlayerDataStorage.CopyFileMetadataAtIndexOptionsInternal copyFileMetadataOptions, ref IntPtr outMetadata);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PlayerDataStorage_CopyFileMetadataByFilenameDelegate(IntPtr handle, ref global::Epic.OnlineServices.PlayerDataStorage.CopyFileMetadataByFilenameOptionsInternal copyFileMetadataOptions, ref IntPtr outMetadata);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PlayerDataStorage_DeleteCacheDelegate(IntPtr handle, ref global::Epic.OnlineServices.PlayerDataStorage.DeleteCacheOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.PlayerDataStorage.OnDeleteCacheCompleteCallbackInternal completionCallback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_PlayerDataStorage_DeleteFileDelegate(IntPtr handle, ref DeleteFileOptionsInternal deleteOptions, IntPtr clientData, OnDeleteFileCompleteCallbackInternal completionCallback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_PlayerDataStorage_DuplicateFileDelegate(IntPtr handle, ref DuplicateFileOptionsInternal duplicateOptions, IntPtr clientData, OnDuplicateFileCompleteCallbackInternal completionCallback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_PlayerDataStorage_FileMetadata_ReleaseDelegate(IntPtr fileMetadata);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PlayerDataStorage_GetFileMetadataCountDelegate(IntPtr handle, ref global::Epic.OnlineServices.PlayerDataStorage.GetFileMetadataCountOptionsInternal getFileMetadataCountOptions, ref int outFileMetadataCount);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_PlayerDataStorage_QueryFileDelegate(IntPtr handle, ref global::Epic.OnlineServices.PlayerDataStorage.QueryFileOptionsInternal queryFileOptions, IntPtr clientData, global::Epic.OnlineServices.PlayerDataStorage.OnQueryFileCompleteCallbackInternal completionCallback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_PlayerDataStorage_QueryFileListDelegate(IntPtr handle, ref global::Epic.OnlineServices.PlayerDataStorage.QueryFileListOptionsInternal queryFileListOptions, IntPtr clientData, global::Epic.OnlineServices.PlayerDataStorage.OnQueryFileListCompleteCallbackInternal completionCallback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_PlayerDataStorage_ReadFileDelegate(IntPtr handle, ref global::Epic.OnlineServices.PlayerDataStorage.ReadFileOptionsInternal readOptions, IntPtr clientData, global::Epic.OnlineServices.PlayerDataStorage.OnReadFileCompleteCallbackInternal completionCallback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_PlayerDataStorage_WriteFileDelegate(IntPtr handle, ref WriteFileOptionsInternal writeOptions, IntPtr clientData, OnWriteFileCompleteCallbackInternal completionCallback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PresenceModification_DeleteDataDelegate(IntPtr handle, ref PresenceModificationDeleteDataOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_PresenceModification_ReleaseDelegate(IntPtr presenceModificationHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PresenceModification_SetDataDelegate(IntPtr handle, ref PresenceModificationSetDataOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PresenceModification_SetJoinInfoDelegate(IntPtr handle, ref PresenceModificationSetJoinInfoOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PresenceModification_SetRawRichTextDelegate(IntPtr handle, ref PresenceModificationSetRawRichTextOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_PresenceModification_SetStatusDelegate(IntPtr handle, ref PresenceModificationSetStatusOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Presence_AddNotifyJoinGameAcceptedDelegate(IntPtr handle, ref AddNotifyJoinGameAcceptedOptionsInternal options, IntPtr clientData, OnJoinGameAcceptedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Presence_AddNotifyOnPresenceChangedDelegate(IntPtr handle, ref AddNotifyOnPresenceChangedOptionsInternal options, IntPtr clientData, OnPresenceChangedCallbackInternal notificationHandler);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Presence_CopyPresenceDelegate(IntPtr handle, ref CopyPresenceOptionsInternal options, ref IntPtr outPresence);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Presence_CreatePresenceModificationDelegate(IntPtr handle, ref CreatePresenceModificationOptionsInternal options, ref IntPtr outPresenceModificationHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Presence_GetJoinInfoDelegate(IntPtr handle, ref GetJoinInfoOptionsInternal options, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_Presence_HasPresenceDelegate(IntPtr handle, ref HasPresenceOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Presence_Info_ReleaseDelegate(IntPtr presenceInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Presence_QueryPresenceDelegate(IntPtr handle, ref QueryPresenceOptionsInternal options, IntPtr clientData, OnQueryPresenceCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Presence_RemoveNotifyJoinGameAcceptedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Presence_RemoveNotifyOnPresenceChangedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Presence_SetPresenceDelegate(IntPtr handle, ref SetPresenceOptionsInternal options, IntPtr clientData, SetPresenceCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_ProductUserId_FromStringDelegate(IntPtr productUserIdString);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_ProductUserId_IsValidDelegate(IntPtr accountId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_ProductUserId_ToStringDelegate(IntPtr accountId, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_ProgressionSnapshot_AddProgressionDelegate(IntPtr handle, ref AddProgressionOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_ProgressionSnapshot_BeginSnapshotDelegate(IntPtr handle, ref BeginSnapshotOptionsInternal options, ref uint outSnapshotId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_ProgressionSnapshot_DeleteSnapshotDelegate(IntPtr handle, ref DeleteSnapshotOptionsInternal options, IntPtr clientData, OnDeleteSnapshotCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_ProgressionSnapshot_EndSnapshotDelegate(IntPtr handle, ref EndSnapshotOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_ProgressionSnapshot_SubmitSnapshotDelegate(IntPtr handle, ref SubmitSnapshotOptionsInternal options, IntPtr clientData, OnSubmitSnapshotCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_RTCAdmin_CopyUserTokenByIndexDelegate(IntPtr handle, ref CopyUserTokenByIndexOptionsInternal options, ref IntPtr outUserToken);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_RTCAdmin_CopyUserTokenByUserIdDelegate(IntPtr handle, ref CopyUserTokenByUserIdOptionsInternal options, ref IntPtr outUserToken);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAdmin_KickDelegate(IntPtr handle, ref KickOptionsInternal options, IntPtr clientData, OnKickCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAdmin_QueryJoinRoomTokenDelegate(IntPtr handle, ref QueryJoinRoomTokenOptionsInternal options, IntPtr clientData, OnQueryJoinRoomTokenCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAdmin_SetParticipantHardMuteDelegate(IntPtr handle, ref SetParticipantHardMuteOptionsInternal options, IntPtr clientData, OnSetParticipantHardMuteCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAdmin_UserToken_ReleaseDelegate(IntPtr userToken);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_RTCAudio_AddNotifyAudioBeforeRenderDelegate(IntPtr handle, ref AddNotifyAudioBeforeRenderOptionsInternal options, IntPtr clientData, OnAudioBeforeRenderCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_RTCAudio_AddNotifyAudioBeforeSendDelegate(IntPtr handle, ref AddNotifyAudioBeforeSendOptionsInternal options, IntPtr clientData, OnAudioBeforeSendCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_RTCAudio_AddNotifyAudioDevicesChangedDelegate(IntPtr handle, ref AddNotifyAudioDevicesChangedOptionsInternal options, IntPtr clientData, OnAudioDevicesChangedCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_RTCAudio_AddNotifyAudioInputStateDelegate(IntPtr handle, ref AddNotifyAudioInputStateOptionsInternal options, IntPtr clientData, OnAudioInputStateCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_RTCAudio_AddNotifyAudioOutputStateDelegate(IntPtr handle, ref AddNotifyAudioOutputStateOptionsInternal options, IntPtr clientData, OnAudioOutputStateCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_RTCAudio_AddNotifyParticipantUpdatedDelegate(IntPtr handle, ref AddNotifyParticipantUpdatedOptionsInternal options, IntPtr clientData, OnParticipantUpdatedCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_RTCAudio_CopyInputDeviceInformationByIndexDelegate(IntPtr handle, ref CopyInputDeviceInformationByIndexOptionsInternal options, ref IntPtr outInputDeviceInformation);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_RTCAudio_CopyOutputDeviceInformationByIndexDelegate(IntPtr handle, ref CopyOutputDeviceInformationByIndexOptionsInternal options, ref IntPtr outOutputDeviceInformation);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_RTCAudio_GetAudioInputDeviceByIndexDelegate(IntPtr handle, ref GetAudioInputDeviceByIndexOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_RTCAudio_GetAudioInputDevicesCountDelegate(IntPtr handle, ref GetAudioInputDevicesCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_RTCAudio_GetAudioOutputDeviceByIndexDelegate(IntPtr handle, ref GetAudioOutputDeviceByIndexOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_RTCAudio_GetAudioOutputDevicesCountDelegate(IntPtr handle, ref GetAudioOutputDevicesCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_RTCAudio_GetInputDevicesCountDelegate(IntPtr handle, ref GetInputDevicesCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_RTCAudio_GetOutputDevicesCountDelegate(IntPtr handle, ref GetOutputDevicesCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_InputDeviceInformation_ReleaseDelegate(IntPtr deviceInformation);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_OutputDeviceInformation_ReleaseDelegate(IntPtr deviceInformation);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_QueryInputDevicesInformationDelegate(IntPtr handle, ref QueryInputDevicesInformationOptionsInternal options, IntPtr clientData, OnQueryInputDevicesInformationCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_QueryOutputDevicesInformationDelegate(IntPtr handle, ref QueryOutputDevicesInformationOptionsInternal options, IntPtr clientData, OnQueryOutputDevicesInformationCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_RTCAudio_RegisterPlatformAudioUserDelegate(IntPtr handle, ref RegisterPlatformAudioUserOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_RegisterPlatformUserDelegate(IntPtr handle, ref RegisterPlatformUserOptionsInternal options, IntPtr clientData, OnRegisterPlatformUserCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_RemoveNotifyAudioBeforeRenderDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_RemoveNotifyAudioBeforeSendDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_RemoveNotifyAudioDevicesChangedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_RemoveNotifyAudioInputStateDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_RemoveNotifyAudioOutputStateDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_RemoveNotifyParticipantUpdatedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_RTCAudio_SendAudioDelegate(IntPtr handle, ref SendAudioOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_RTCAudio_SetAudioInputSettingsDelegate(IntPtr handle, ref SetAudioInputSettingsOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_RTCAudio_SetAudioOutputSettingsDelegate(IntPtr handle, ref SetAudioOutputSettingsOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_SetInputDeviceSettingsDelegate(IntPtr handle, ref SetInputDeviceSettingsOptionsInternal options, IntPtr clientData, OnSetInputDeviceSettingsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_SetOutputDeviceSettingsDelegate(IntPtr handle, ref SetOutputDeviceSettingsOptionsInternal options, IntPtr clientData, OnSetOutputDeviceSettingsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_RTCAudio_UnregisterPlatformAudioUserDelegate(IntPtr handle, ref UnregisterPlatformAudioUserOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_UnregisterPlatformUserDelegate(IntPtr handle, ref UnregisterPlatformUserOptionsInternal options, IntPtr clientData, OnUnregisterPlatformUserCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_UpdateParticipantVolumeDelegate(IntPtr handle, ref UpdateParticipantVolumeOptionsInternal options, IntPtr clientData, OnUpdateParticipantVolumeCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_UpdateReceivingDelegate(IntPtr handle, ref UpdateReceivingOptionsInternal options, IntPtr clientData, OnUpdateReceivingCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_UpdateReceivingVolumeDelegate(IntPtr handle, ref UpdateReceivingVolumeOptionsInternal options, IntPtr clientData, OnUpdateReceivingVolumeCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_UpdateSendingDelegate(IntPtr handle, ref UpdateSendingOptionsInternal options, IntPtr clientData, OnUpdateSendingCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTCAudio_UpdateSendingVolumeDelegate(IntPtr handle, ref UpdateSendingVolumeOptionsInternal options, IntPtr clientData, OnUpdateSendingVolumeCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_RTC_AddNotifyDisconnectedDelegate(IntPtr handle, ref AddNotifyDisconnectedOptionsInternal options, IntPtr clientData, OnDisconnectedCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_RTC_AddNotifyParticipantStatusChangedDelegate(IntPtr handle, ref AddNotifyParticipantStatusChangedOptionsInternal options, IntPtr clientData, OnParticipantStatusChangedCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_RTC_AddNotifyRoomStatisticsUpdatedDelegate(IntPtr handle, ref AddNotifyRoomStatisticsUpdatedOptionsInternal options, IntPtr clientData, OnRoomStatisticsUpdatedCallbackInternal statisticsUpdateHandler);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTC_BlockParticipantDelegate(IntPtr handle, ref BlockParticipantOptionsInternal options, IntPtr clientData, OnBlockParticipantCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_RTC_GetAudioInterfaceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTC_JoinRoomDelegate(IntPtr handle, ref JoinRoomOptionsInternal options, IntPtr clientData, OnJoinRoomCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTC_LeaveRoomDelegate(IntPtr handle, ref LeaveRoomOptionsInternal options, IntPtr clientData, OnLeaveRoomCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTC_RemoveNotifyDisconnectedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTC_RemoveNotifyParticipantStatusChangedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_RTC_RemoveNotifyRoomStatisticsUpdatedDelegate(IntPtr handle, ulong notificationId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_RTC_SetRoomSettingDelegate(IntPtr handle, ref SetRoomSettingOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_RTC_SetSettingDelegate(IntPtr handle, ref SetSettingOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Reports_SendPlayerBehaviorReportDelegate(IntPtr handle, ref SendPlayerBehaviorReportOptionsInternal options, IntPtr clientData, OnSendPlayerBehaviorReportCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Sanctions_CopyPlayerSanctionByIndexDelegate(IntPtr handle, ref CopyPlayerSanctionByIndexOptionsInternal options, ref IntPtr outSanction);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Sanctions_GetPlayerSanctionCountDelegate(IntPtr handle, ref GetPlayerSanctionCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sanctions_PlayerSanction_ReleaseDelegate(IntPtr sanction);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sanctions_QueryActivePlayerSanctionsDelegate(IntPtr handle, ref QueryActivePlayerSanctionsOptionsInternal options, IntPtr clientData, OnQueryActivePlayerSanctionsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_SessionDetails_Attribute_ReleaseDelegate(IntPtr sessionAttribute);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionDetails_CopyInfoDelegate(IntPtr handle, ref SessionDetailsCopyInfoOptionsInternal options, ref IntPtr outSessionInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionDetails_CopySessionAttributeByIndexDelegate(IntPtr handle, ref SessionDetailsCopySessionAttributeByIndexOptionsInternal options, ref IntPtr outSessionAttribute);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionDetails_CopySessionAttributeByKeyDelegate(IntPtr handle, ref SessionDetailsCopySessionAttributeByKeyOptionsInternal options, ref IntPtr outSessionAttribute);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_SessionDetails_GetSessionAttributeCountDelegate(IntPtr handle, ref SessionDetailsGetSessionAttributeCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_SessionDetails_Info_ReleaseDelegate(IntPtr sessionInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_SessionDetails_ReleaseDelegate(IntPtr sessionHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionModification_AddAttributeDelegate(IntPtr handle, ref SessionModificationAddAttributeOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_SessionModification_ReleaseDelegate(IntPtr sessionModificationHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionModification_RemoveAttributeDelegate(IntPtr handle, ref SessionModificationRemoveAttributeOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionModification_SetAllowedPlatformIdsDelegate(IntPtr handle, ref SessionModificationSetAllowedPlatformIdsOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionModification_SetBucketIdDelegate(IntPtr handle, ref SessionModificationSetBucketIdOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionModification_SetHostAddressDelegate(IntPtr handle, ref SessionModificationSetHostAddressOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionModification_SetInvitesAllowedDelegate(IntPtr handle, ref SessionModificationSetInvitesAllowedOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionModification_SetJoinInProgressAllowedDelegate(IntPtr handle, ref SessionModificationSetJoinInProgressAllowedOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionModification_SetMaxPlayersDelegate(IntPtr handle, ref SessionModificationSetMaxPlayersOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionModification_SetPermissionLevelDelegate(IntPtr handle, ref SessionModificationSetPermissionLevelOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionSearch_CopySearchResultByIndexDelegate(IntPtr handle, ref SessionSearchCopySearchResultByIndexOptionsInternal options, ref IntPtr outSessionHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_SessionSearch_FindDelegate(IntPtr handle, ref SessionSearchFindOptionsInternal options, IntPtr clientData, SessionSearchOnFindCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_SessionSearch_GetSearchResultCountDelegate(IntPtr handle, ref SessionSearchGetSearchResultCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_SessionSearch_ReleaseDelegate(IntPtr sessionSearchHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionSearch_RemoveParameterDelegate(IntPtr handle, ref SessionSearchRemoveParameterOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionSearch_SetMaxResultsDelegate(IntPtr handle, ref SessionSearchSetMaxResultsOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionSearch_SetParameterDelegate(IntPtr handle, ref SessionSearchSetParameterOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionSearch_SetSessionIdDelegate(IntPtr handle, ref SessionSearchSetSessionIdOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_SessionSearch_SetTargetUserIdDelegate(IntPtr handle, ref SessionSearchSetTargetUserIdOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Sessions_AddNotifyJoinSessionAcceptedDelegate(IntPtr handle, ref AddNotifyJoinSessionAcceptedOptionsInternal options, IntPtr clientData, OnJoinSessionAcceptedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Sessions_AddNotifyLeaveSessionRequestedDelegate(IntPtr handle, ref AddNotifyLeaveSessionRequestedOptionsInternal options, IntPtr clientData, OnLeaveSessionRequestedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Sessions_AddNotifySendSessionNativeInviteRequestedDelegate(IntPtr handle, ref AddNotifySendSessionNativeInviteRequestedOptionsInternal options, IntPtr clientData, OnSendSessionNativeInviteRequestedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Sessions_AddNotifySessionInviteAcceptedDelegate(IntPtr handle, ref AddNotifySessionInviteAcceptedOptionsInternal options, IntPtr clientData, OnSessionInviteAcceptedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Sessions_AddNotifySessionInviteReceivedDelegate(IntPtr handle, ref AddNotifySessionInviteReceivedOptionsInternal options, IntPtr clientData, OnSessionInviteReceivedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_Sessions_AddNotifySessionInviteRejectedDelegate(IntPtr handle, ref AddNotifySessionInviteRejectedOptionsInternal options, IntPtr clientData, OnSessionInviteRejectedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Sessions_CopyActiveSessionHandleDelegate(IntPtr handle, ref CopyActiveSessionHandleOptionsInternal options, ref IntPtr outSessionHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Sessions_CopySessionHandleByInviteIdDelegate(IntPtr handle, ref CopySessionHandleByInviteIdOptionsInternal options, ref IntPtr outSessionHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Sessions_CopySessionHandleByUiEventIdDelegate(IntPtr handle, ref CopySessionHandleByUiEventIdOptionsInternal options, ref IntPtr outSessionHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Sessions_CopySessionHandleForPresenceDelegate(IntPtr handle, ref CopySessionHandleForPresenceOptionsInternal options, ref IntPtr outSessionHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Sessions_CreateSessionModificationDelegate(IntPtr handle, ref CreateSessionModificationOptionsInternal options, ref IntPtr outSessionModificationHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Sessions_CreateSessionSearchDelegate(IntPtr handle, ref CreateSessionSearchOptionsInternal options, ref IntPtr outSessionSearchHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_DestroySessionDelegate(IntPtr handle, ref DestroySessionOptionsInternal options, IntPtr clientData, OnDestroySessionCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Sessions_DumpSessionStateDelegate(IntPtr handle, ref DumpSessionStateOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_EndSessionDelegate(IntPtr handle, ref global::Epic.OnlineServices.Sessions.EndSessionOptionsInternal options, IntPtr clientData, OnEndSessionCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Sessions_GetInviteCountDelegate(IntPtr handle, ref global::Epic.OnlineServices.Sessions.GetInviteCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Sessions_GetInviteIdByIndexDelegate(IntPtr handle, ref global::Epic.OnlineServices.Sessions.GetInviteIdByIndexOptionsInternal options, IntPtr outBuffer, ref int inOutBufferLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Sessions_IsUserInSessionDelegate(IntPtr handle, ref IsUserInSessionOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_JoinSessionDelegate(IntPtr handle, ref JoinSessionOptionsInternal options, IntPtr clientData, OnJoinSessionCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_QueryInvitesDelegate(IntPtr handle, ref global::Epic.OnlineServices.Sessions.QueryInvitesOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Sessions.OnQueryInvitesCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_RegisterPlayersDelegate(IntPtr handle, ref RegisterPlayersOptionsInternal options, IntPtr clientData, OnRegisterPlayersCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_RejectInviteDelegate(IntPtr handle, ref global::Epic.OnlineServices.Sessions.RejectInviteOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Sessions.OnRejectInviteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_RemoveNotifyJoinSessionAcceptedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_RemoveNotifyLeaveSessionRequestedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_RemoveNotifySendSessionNativeInviteRequestedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_RemoveNotifySessionInviteAcceptedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_RemoveNotifySessionInviteReceivedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_RemoveNotifySessionInviteRejectedDelegate(IntPtr handle, ulong inId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_SendInviteDelegate(IntPtr handle, ref global::Epic.OnlineServices.Sessions.SendInviteOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.Sessions.OnSendInviteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_StartSessionDelegate(IntPtr handle, ref StartSessionOptionsInternal options, IntPtr clientData, OnStartSessionCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_UnregisterPlayersDelegate(IntPtr handle, ref UnregisterPlayersOptionsInternal options, IntPtr clientData, OnUnregisterPlayersCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Sessions_UpdateSessionDelegate(IntPtr handle, ref UpdateSessionOptionsInternal options, IntPtr clientData, OnUpdateSessionCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Sessions_UpdateSessionModificationDelegate(IntPtr handle, ref UpdateSessionModificationOptionsInternal options, ref IntPtr outSessionModificationHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_ShutdownDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Stats_CopyStatByIndexDelegate(IntPtr handle, ref CopyStatByIndexOptionsInternal options, ref IntPtr outStat);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_Stats_CopyStatByNameDelegate(IntPtr handle, ref CopyStatByNameOptionsInternal options, ref IntPtr outStat);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_Stats_GetStatsCountDelegate(IntPtr handle, ref GetStatCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Stats_IngestStatDelegate(IntPtr handle, ref IngestStatOptionsInternal options, IntPtr clientData, OnIngestStatCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Stats_QueryStatsDelegate(IntPtr handle, ref QueryStatsOptionsInternal options, IntPtr clientData, OnQueryStatsCompleteCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_Stats_Stat_ReleaseDelegate(IntPtr stat);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_TitleStorageFileTransferRequest_CancelRequestDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_TitleStorageFileTransferRequest_GetFileRequestStateDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_TitleStorageFileTransferRequest_GetFilenameDelegate(IntPtr handle, uint filenameStringBufferSizeBytes, IntPtr outStringBuffer, ref int outStringLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_TitleStorageFileTransferRequest_ReleaseDelegate(IntPtr titleStorageFileTransferHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_TitleStorage_CopyFileMetadataAtIndexDelegate(IntPtr handle, ref global::Epic.OnlineServices.TitleStorage.CopyFileMetadataAtIndexOptionsInternal options, ref IntPtr outMetadata);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_TitleStorage_CopyFileMetadataByFilenameDelegate(IntPtr handle, ref global::Epic.OnlineServices.TitleStorage.CopyFileMetadataByFilenameOptionsInternal options, ref IntPtr outMetadata);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_TitleStorage_DeleteCacheDelegate(IntPtr handle, ref global::Epic.OnlineServices.TitleStorage.DeleteCacheOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.TitleStorage.OnDeleteCacheCompleteCallbackInternal completionCallback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_TitleStorage_FileMetadata_ReleaseDelegate(IntPtr fileMetadata);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_TitleStorage_GetFileMetadataCountDelegate(IntPtr handle, ref global::Epic.OnlineServices.TitleStorage.GetFileMetadataCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_TitleStorage_QueryFileDelegate(IntPtr handle, ref global::Epic.OnlineServices.TitleStorage.QueryFileOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.TitleStorage.OnQueryFileCompleteCallbackInternal completionCallback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_TitleStorage_QueryFileListDelegate(IntPtr handle, ref global::Epic.OnlineServices.TitleStorage.QueryFileListOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.TitleStorage.OnQueryFileListCompleteCallbackInternal completionCallback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr EOS_TitleStorage_ReadFileDelegate(IntPtr handle, ref global::Epic.OnlineServices.TitleStorage.ReadFileOptionsInternal options, IntPtr clientData, global::Epic.OnlineServices.TitleStorage.OnReadFileCompleteCallbackInternal completionCallback);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UI_AcknowledgeEventIdDelegate(IntPtr handle, ref AcknowledgeEventIdOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_UI_AddNotifyDisplaySettingsUpdatedDelegate(IntPtr handle, ref AddNotifyDisplaySettingsUpdatedOptionsInternal options, IntPtr clientData, OnDisplaySettingsUpdatedCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ulong EOS_UI_AddNotifyMemoryMonitorDelegate(IntPtr handle, ref AddNotifyMemoryMonitorOptionsInternal options, IntPtr clientData, OnMemoryMonitorCallbackInternal notificationFn);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_UI_GetFriendsExclusiveInputDelegate(IntPtr handle, ref GetFriendsExclusiveInputOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_UI_GetFriendsVisibleDelegate(IntPtr handle, ref GetFriendsVisibleOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate NotificationLocation EOS_UI_GetNotificationLocationPreferenceDelegate(IntPtr handle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate InputStateButtonFlags EOS_UI_GetToggleFriendsButtonDelegate(IntPtr handle, ref GetToggleFriendsButtonOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate KeyCombination EOS_UI_GetToggleFriendsKeyDelegate(IntPtr handle, ref GetToggleFriendsKeyOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UI_HideFriendsDelegate(IntPtr handle, ref HideFriendsOptionsInternal options, IntPtr clientData, OnHideFriendsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_UI_IsSocialOverlayPausedDelegate(IntPtr handle, ref IsSocialOverlayPausedOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_UI_IsValidButtonCombinationDelegate(IntPtr handle, InputStateButtonFlags buttonCombination);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int EOS_UI_IsValidKeyCombinationDelegate(IntPtr handle, KeyCombination keyCombination);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UI_PauseSocialOverlayDelegate(IntPtr handle, ref PauseSocialOverlayOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UI_PrePresentDelegate(IntPtr handle, ref PrePresentOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UI_RemoveNotifyDisplaySettingsUpdatedDelegate(IntPtr handle, ulong id);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UI_RemoveNotifyMemoryMonitorDelegate(IntPtr handle, ulong id);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UI_ReportInputStateDelegate(IntPtr handle, ref ReportInputStateOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UI_SetDisplayPreferenceDelegate(IntPtr handle, ref SetDisplayPreferenceOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UI_SetToggleFriendsButtonDelegate(IntPtr handle, ref SetToggleFriendsButtonOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UI_SetToggleFriendsKeyDelegate(IntPtr handle, ref SetToggleFriendsKeyOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UI_ShowBlockPlayerDelegate(IntPtr handle, ref ShowBlockPlayerOptionsInternal options, IntPtr clientData, OnShowBlockPlayerCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UI_ShowFriendsDelegate(IntPtr handle, ref ShowFriendsOptionsInternal options, IntPtr clientData, OnShowFriendsCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UI_ShowNativeProfileDelegate(IntPtr handle, ref ShowNativeProfileOptionsInternal options, IntPtr clientData, OnShowNativeProfileCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UI_ShowReportPlayerDelegate(IntPtr handle, ref ShowReportPlayerOptionsInternal options, IntPtr clientData, OnShowReportPlayerCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UserInfo_BestDisplayName_ReleaseDelegate(IntPtr bestDisplayName);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UserInfo_CopyBestDisplayNameDelegate(IntPtr handle, ref CopyBestDisplayNameOptionsInternal options, ref IntPtr outBestDisplayName);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UserInfo_CopyBestDisplayNameWithPlatformDelegate(IntPtr handle, ref CopyBestDisplayNameWithPlatformOptionsInternal options, ref IntPtr outBestDisplayName);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UserInfo_CopyExternalUserInfoByAccountIdDelegate(IntPtr handle, ref CopyExternalUserInfoByAccountIdOptionsInternal options, ref IntPtr outExternalUserInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UserInfo_CopyExternalUserInfoByAccountTypeDelegate(IntPtr handle, ref CopyExternalUserInfoByAccountTypeOptionsInternal options, ref IntPtr outExternalUserInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UserInfo_CopyExternalUserInfoByIndexDelegate(IntPtr handle, ref CopyExternalUserInfoByIndexOptionsInternal options, ref IntPtr outExternalUserInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate Result EOS_UserInfo_CopyUserInfoDelegate(IntPtr handle, ref CopyUserInfoOptionsInternal options, ref IntPtr outUserInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UserInfo_ExternalUserInfo_ReleaseDelegate(IntPtr externalUserInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_UserInfo_GetExternalUserInfoCountDelegate(IntPtr handle, ref GetExternalUserInfoCountOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint EOS_UserInfo_GetLocalPlatformTypeDelegate(IntPtr handle, ref GetLocalPlatformTypeOptionsInternal options);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UserInfo_QueryUserInfoDelegate(IntPtr handle, ref QueryUserInfoOptionsInternal options, IntPtr clientData, OnQueryUserInfoCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UserInfo_QueryUserInfoByDisplayNameDelegate(IntPtr handle, ref QueryUserInfoByDisplayNameOptionsInternal options, IntPtr clientData, OnQueryUserInfoByDisplayNameCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UserInfo_QueryUserInfoByExternalAccountDelegate(IntPtr handle, ref QueryUserInfoByExternalAccountOptionsInternal options, IntPtr clientData, OnQueryUserInfoByExternalAccountCallbackInternal completionDelegate);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EOS_UserInfo_ReleaseDelegate(IntPtr userInfo);

	private const string EOS_Achievements_AddNotifyAchievementsUnlockedName = "EOS_Achievements_AddNotifyAchievementsUnlocked";

	private const string EOS_Achievements_AddNotifyAchievementsUnlockedV2Name = "EOS_Achievements_AddNotifyAchievementsUnlockedV2";

	private const string EOS_Achievements_CopyAchievementDefinitionByAchievementIdName = "EOS_Achievements_CopyAchievementDefinitionByAchievementId";

	private const string EOS_Achievements_CopyAchievementDefinitionByIndexName = "EOS_Achievements_CopyAchievementDefinitionByIndex";

	private const string EOS_Achievements_CopyAchievementDefinitionV2ByAchievementIdName = "EOS_Achievements_CopyAchievementDefinitionV2ByAchievementId";

	private const string EOS_Achievements_CopyAchievementDefinitionV2ByIndexName = "EOS_Achievements_CopyAchievementDefinitionV2ByIndex";

	private const string EOS_Achievements_CopyPlayerAchievementByAchievementIdName = "EOS_Achievements_CopyPlayerAchievementByAchievementId";

	private const string EOS_Achievements_CopyPlayerAchievementByIndexName = "EOS_Achievements_CopyPlayerAchievementByIndex";

	private const string EOS_Achievements_CopyUnlockedAchievementByAchievementIdName = "EOS_Achievements_CopyUnlockedAchievementByAchievementId";

	private const string EOS_Achievements_CopyUnlockedAchievementByIndexName = "EOS_Achievements_CopyUnlockedAchievementByIndex";

	private const string EOS_Achievements_DefinitionV2_ReleaseName = "EOS_Achievements_DefinitionV2_Release";

	private const string EOS_Achievements_Definition_ReleaseName = "EOS_Achievements_Definition_Release";

	private const string EOS_Achievements_GetAchievementDefinitionCountName = "EOS_Achievements_GetAchievementDefinitionCount";

	private const string EOS_Achievements_GetPlayerAchievementCountName = "EOS_Achievements_GetPlayerAchievementCount";

	private const string EOS_Achievements_GetUnlockedAchievementCountName = "EOS_Achievements_GetUnlockedAchievementCount";

	private const string EOS_Achievements_PlayerAchievement_ReleaseName = "EOS_Achievements_PlayerAchievement_Release";

	private const string EOS_Achievements_QueryDefinitionsName = "EOS_Achievements_QueryDefinitions";

	private const string EOS_Achievements_QueryPlayerAchievementsName = "EOS_Achievements_QueryPlayerAchievements";

	private const string EOS_Achievements_RemoveNotifyAchievementsUnlockedName = "EOS_Achievements_RemoveNotifyAchievementsUnlocked";

	private const string EOS_Achievements_UnlockAchievementsName = "EOS_Achievements_UnlockAchievements";

	private const string EOS_Achievements_UnlockedAchievement_ReleaseName = "EOS_Achievements_UnlockedAchievement_Release";

	private const string EOS_ActiveSession_CopyInfoName = "EOS_ActiveSession_CopyInfo";

	private const string EOS_ActiveSession_GetRegisteredPlayerByIndexName = "EOS_ActiveSession_GetRegisteredPlayerByIndex";

	private const string EOS_ActiveSession_GetRegisteredPlayerCountName = "EOS_ActiveSession_GetRegisteredPlayerCount";

	private const string EOS_ActiveSession_Info_ReleaseName = "EOS_ActiveSession_Info_Release";

	private const string EOS_ActiveSession_ReleaseName = "EOS_ActiveSession_Release";

	private const string EOS_AntiCheatClient_AddExternalIntegrityCatalogName = "EOS_AntiCheatClient_AddExternalIntegrityCatalog";

	private const string EOS_AntiCheatClient_AddNotifyClientIntegrityViolatedName = "EOS_AntiCheatClient_AddNotifyClientIntegrityViolated";

	private const string EOS_AntiCheatClient_AddNotifyMessageToPeerName = "EOS_AntiCheatClient_AddNotifyMessageToPeer";

	private const string EOS_AntiCheatClient_AddNotifyMessageToServerName = "EOS_AntiCheatClient_AddNotifyMessageToServer";

	private const string EOS_AntiCheatClient_AddNotifyPeerActionRequiredName = "EOS_AntiCheatClient_AddNotifyPeerActionRequired";

	private const string EOS_AntiCheatClient_AddNotifyPeerAuthStatusChangedName = "EOS_AntiCheatClient_AddNotifyPeerAuthStatusChanged";

	private const string EOS_AntiCheatClient_BeginSessionName = "EOS_AntiCheatClient_BeginSession";

	private const string EOS_AntiCheatClient_EndSessionName = "EOS_AntiCheatClient_EndSession";

	private const string EOS_AntiCheatClient_GetProtectMessageOutputLengthName = "EOS_AntiCheatClient_GetProtectMessageOutputLength";

	private const string EOS_AntiCheatClient_PollStatusName = "EOS_AntiCheatClient_PollStatus";

	private const string EOS_AntiCheatClient_ProtectMessageName = "EOS_AntiCheatClient_ProtectMessage";

	private const string EOS_AntiCheatClient_ReceiveMessageFromPeerName = "EOS_AntiCheatClient_ReceiveMessageFromPeer";

	private const string EOS_AntiCheatClient_ReceiveMessageFromServerName = "EOS_AntiCheatClient_ReceiveMessageFromServer";

	private const string EOS_AntiCheatClient_RegisterPeerName = "EOS_AntiCheatClient_RegisterPeer";

	private const string EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolatedName = "EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolated";

	private const string EOS_AntiCheatClient_RemoveNotifyMessageToPeerName = "EOS_AntiCheatClient_RemoveNotifyMessageToPeer";

	private const string EOS_AntiCheatClient_RemoveNotifyMessageToServerName = "EOS_AntiCheatClient_RemoveNotifyMessageToServer";

	private const string EOS_AntiCheatClient_RemoveNotifyPeerActionRequiredName = "EOS_AntiCheatClient_RemoveNotifyPeerActionRequired";

	private const string EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChangedName = "EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChanged";

	private const string EOS_AntiCheatClient_UnprotectMessageName = "EOS_AntiCheatClient_UnprotectMessage";

	private const string EOS_AntiCheatClient_UnregisterPeerName = "EOS_AntiCheatClient_UnregisterPeer";

	private const string EOS_AntiCheatServer_AddNotifyClientActionRequiredName = "EOS_AntiCheatServer_AddNotifyClientActionRequired";

	private const string EOS_AntiCheatServer_AddNotifyClientAuthStatusChangedName = "EOS_AntiCheatServer_AddNotifyClientAuthStatusChanged";

	private const string EOS_AntiCheatServer_AddNotifyMessageToClientName = "EOS_AntiCheatServer_AddNotifyMessageToClient";

	private const string EOS_AntiCheatServer_BeginSessionName = "EOS_AntiCheatServer_BeginSession";

	private const string EOS_AntiCheatServer_EndSessionName = "EOS_AntiCheatServer_EndSession";

	private const string EOS_AntiCheatServer_GetProtectMessageOutputLengthName = "EOS_AntiCheatServer_GetProtectMessageOutputLength";

	private const string EOS_AntiCheatServer_LogEventName = "EOS_AntiCheatServer_LogEvent";

	private const string EOS_AntiCheatServer_LogGameRoundEndName = "EOS_AntiCheatServer_LogGameRoundEnd";

	private const string EOS_AntiCheatServer_LogGameRoundStartName = "EOS_AntiCheatServer_LogGameRoundStart";

	private const string EOS_AntiCheatServer_LogPlayerDespawnName = "EOS_AntiCheatServer_LogPlayerDespawn";

	private const string EOS_AntiCheatServer_LogPlayerReviveName = "EOS_AntiCheatServer_LogPlayerRevive";

	private const string EOS_AntiCheatServer_LogPlayerSpawnName = "EOS_AntiCheatServer_LogPlayerSpawn";

	private const string EOS_AntiCheatServer_LogPlayerTakeDamageName = "EOS_AntiCheatServer_LogPlayerTakeDamage";

	private const string EOS_AntiCheatServer_LogPlayerTickName = "EOS_AntiCheatServer_LogPlayerTick";

	private const string EOS_AntiCheatServer_LogPlayerUseAbilityName = "EOS_AntiCheatServer_LogPlayerUseAbility";

	private const string EOS_AntiCheatServer_LogPlayerUseWeaponName = "EOS_AntiCheatServer_LogPlayerUseWeapon";

	private const string EOS_AntiCheatServer_ProtectMessageName = "EOS_AntiCheatServer_ProtectMessage";

	private const string EOS_AntiCheatServer_ReceiveMessageFromClientName = "EOS_AntiCheatServer_ReceiveMessageFromClient";

	private const string EOS_AntiCheatServer_RegisterClientName = "EOS_AntiCheatServer_RegisterClient";

	private const string EOS_AntiCheatServer_RegisterEventName = "EOS_AntiCheatServer_RegisterEvent";

	private const string EOS_AntiCheatServer_RemoveNotifyClientActionRequiredName = "EOS_AntiCheatServer_RemoveNotifyClientActionRequired";

	private const string EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChangedName = "EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChanged";

	private const string EOS_AntiCheatServer_RemoveNotifyMessageToClientName = "EOS_AntiCheatServer_RemoveNotifyMessageToClient";

	private const string EOS_AntiCheatServer_SetClientDetailsName = "EOS_AntiCheatServer_SetClientDetails";

	private const string EOS_AntiCheatServer_SetClientNetworkStateName = "EOS_AntiCheatServer_SetClientNetworkState";

	private const string EOS_AntiCheatServer_SetGameSessionIdName = "EOS_AntiCheatServer_SetGameSessionId";

	private const string EOS_AntiCheatServer_UnprotectMessageName = "EOS_AntiCheatServer_UnprotectMessage";

	private const string EOS_AntiCheatServer_UnregisterClientName = "EOS_AntiCheatServer_UnregisterClient";

	private const string EOS_Auth_AddNotifyLoginStatusChangedName = "EOS_Auth_AddNotifyLoginStatusChanged";

	private const string EOS_Auth_CopyIdTokenName = "EOS_Auth_CopyIdToken";

	private const string EOS_Auth_CopyUserAuthTokenName = "EOS_Auth_CopyUserAuthToken";

	private const string EOS_Auth_DeletePersistentAuthName = "EOS_Auth_DeletePersistentAuth";

	private const string EOS_Auth_GetLoggedInAccountByIndexName = "EOS_Auth_GetLoggedInAccountByIndex";

	private const string EOS_Auth_GetLoggedInAccountsCountName = "EOS_Auth_GetLoggedInAccountsCount";

	private const string EOS_Auth_GetLoginStatusName = "EOS_Auth_GetLoginStatus";

	private const string EOS_Auth_GetMergedAccountByIndexName = "EOS_Auth_GetMergedAccountByIndex";

	private const string EOS_Auth_GetMergedAccountsCountName = "EOS_Auth_GetMergedAccountsCount";

	private const string EOS_Auth_GetSelectedAccountIdName = "EOS_Auth_GetSelectedAccountId";

	private const string EOS_Auth_IdToken_ReleaseName = "EOS_Auth_IdToken_Release";

	private const string EOS_Auth_LinkAccountName = "EOS_Auth_LinkAccount";

	private const string EOS_Auth_LoginName = "EOS_Auth_Login";

	private const string EOS_Auth_LogoutName = "EOS_Auth_Logout";

	private const string EOS_Auth_QueryIdTokenName = "EOS_Auth_QueryIdToken";

	private const string EOS_Auth_RemoveNotifyLoginStatusChangedName = "EOS_Auth_RemoveNotifyLoginStatusChanged";

	private const string EOS_Auth_Token_ReleaseName = "EOS_Auth_Token_Release";

	private const string EOS_Auth_VerifyIdTokenName = "EOS_Auth_VerifyIdToken";

	private const string EOS_Auth_VerifyUserAuthName = "EOS_Auth_VerifyUserAuth";

	private const string EOS_ByteArray_ToStringName = "EOS_ByteArray_ToString";

	private const string EOS_Connect_AddNotifyAuthExpirationName = "EOS_Connect_AddNotifyAuthExpiration";

	private const string EOS_Connect_AddNotifyLoginStatusChangedName = "EOS_Connect_AddNotifyLoginStatusChanged";

	private const string EOS_Connect_CopyIdTokenName = "EOS_Connect_CopyIdToken";

	private const string EOS_Connect_CopyProductUserExternalAccountByAccountIdName = "EOS_Connect_CopyProductUserExternalAccountByAccountId";

	private const string EOS_Connect_CopyProductUserExternalAccountByAccountTypeName = "EOS_Connect_CopyProductUserExternalAccountByAccountType";

	private const string EOS_Connect_CopyProductUserExternalAccountByIndexName = "EOS_Connect_CopyProductUserExternalAccountByIndex";

	private const string EOS_Connect_CopyProductUserInfoName = "EOS_Connect_CopyProductUserInfo";

	private const string EOS_Connect_CreateDeviceIdName = "EOS_Connect_CreateDeviceId";

	private const string EOS_Connect_CreateUserName = "EOS_Connect_CreateUser";

	private const string EOS_Connect_DeleteDeviceIdName = "EOS_Connect_DeleteDeviceId";

	private const string EOS_Connect_ExternalAccountInfo_ReleaseName = "EOS_Connect_ExternalAccountInfo_Release";

	private const string EOS_Connect_GetExternalAccountMappingName = "EOS_Connect_GetExternalAccountMapping";

	private const string EOS_Connect_GetLoggedInUserByIndexName = "EOS_Connect_GetLoggedInUserByIndex";

	private const string EOS_Connect_GetLoggedInUsersCountName = "EOS_Connect_GetLoggedInUsersCount";

	private const string EOS_Connect_GetLoginStatusName = "EOS_Connect_GetLoginStatus";

	private const string EOS_Connect_GetProductUserExternalAccountCountName = "EOS_Connect_GetProductUserExternalAccountCount";

	private const string EOS_Connect_GetProductUserIdMappingName = "EOS_Connect_GetProductUserIdMapping";

	private const string EOS_Connect_IdToken_ReleaseName = "EOS_Connect_IdToken_Release";

	private const string EOS_Connect_LinkAccountName = "EOS_Connect_LinkAccount";

	private const string EOS_Connect_LoginName = "EOS_Connect_Login";

	private const string EOS_Connect_QueryExternalAccountMappingsName = "EOS_Connect_QueryExternalAccountMappings";

	private const string EOS_Connect_QueryProductUserIdMappingsName = "EOS_Connect_QueryProductUserIdMappings";

	private const string EOS_Connect_RemoveNotifyAuthExpirationName = "EOS_Connect_RemoveNotifyAuthExpiration";

	private const string EOS_Connect_RemoveNotifyLoginStatusChangedName = "EOS_Connect_RemoveNotifyLoginStatusChanged";

	private const string EOS_Connect_TransferDeviceIdAccountName = "EOS_Connect_TransferDeviceIdAccount";

	private const string EOS_Connect_UnlinkAccountName = "EOS_Connect_UnlinkAccount";

	private const string EOS_Connect_VerifyIdTokenName = "EOS_Connect_VerifyIdToken";

	private const string EOS_ContinuanceToken_ToStringName = "EOS_ContinuanceToken_ToString";

	private const string EOS_CustomInvites_AcceptRequestToJoinName = "EOS_CustomInvites_AcceptRequestToJoin";

	private const string EOS_CustomInvites_AddNotifyCustomInviteAcceptedName = "EOS_CustomInvites_AddNotifyCustomInviteAccepted";

	private const string EOS_CustomInvites_AddNotifyCustomInviteReceivedName = "EOS_CustomInvites_AddNotifyCustomInviteReceived";

	private const string EOS_CustomInvites_AddNotifyCustomInviteRejectedName = "EOS_CustomInvites_AddNotifyCustomInviteRejected";

	private const string EOS_CustomInvites_AddNotifyRequestToJoinAcceptedName = "EOS_CustomInvites_AddNotifyRequestToJoinAccepted";

	private const string EOS_CustomInvites_AddNotifyRequestToJoinReceivedName = "EOS_CustomInvites_AddNotifyRequestToJoinReceived";

	private const string EOS_CustomInvites_AddNotifyRequestToJoinRejectedName = "EOS_CustomInvites_AddNotifyRequestToJoinRejected";

	private const string EOS_CustomInvites_AddNotifyRequestToJoinResponseReceivedName = "EOS_CustomInvites_AddNotifyRequestToJoinResponseReceived";

	private const string EOS_CustomInvites_AddNotifySendCustomNativeInviteRequestedName = "EOS_CustomInvites_AddNotifySendCustomNativeInviteRequested";

	private const string EOS_CustomInvites_FinalizeInviteName = "EOS_CustomInvites_FinalizeInvite";

	private const string EOS_CustomInvites_RejectRequestToJoinName = "EOS_CustomInvites_RejectRequestToJoin";

	private const string EOS_CustomInvites_RemoveNotifyCustomInviteAcceptedName = "EOS_CustomInvites_RemoveNotifyCustomInviteAccepted";

	private const string EOS_CustomInvites_RemoveNotifyCustomInviteReceivedName = "EOS_CustomInvites_RemoveNotifyCustomInviteReceived";

	private const string EOS_CustomInvites_RemoveNotifyCustomInviteRejectedName = "EOS_CustomInvites_RemoveNotifyCustomInviteRejected";

	private const string EOS_CustomInvites_RemoveNotifyRequestToJoinAcceptedName = "EOS_CustomInvites_RemoveNotifyRequestToJoinAccepted";

	private const string EOS_CustomInvites_RemoveNotifyRequestToJoinReceivedName = "EOS_CustomInvites_RemoveNotifyRequestToJoinReceived";

	private const string EOS_CustomInvites_RemoveNotifyRequestToJoinRejectedName = "EOS_CustomInvites_RemoveNotifyRequestToJoinRejected";

	private const string EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceivedName = "EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceived";

	private const string EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequestedName = "EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequested";

	private const string EOS_CustomInvites_SendCustomInviteName = "EOS_CustomInvites_SendCustomInvite";

	private const string EOS_CustomInvites_SendRequestToJoinName = "EOS_CustomInvites_SendRequestToJoin";

	private const string EOS_CustomInvites_SetCustomInviteName = "EOS_CustomInvites_SetCustomInvite";

	private const string EOS_EApplicationStatus_ToStringName = "EOS_EApplicationStatus_ToString";

	private const string EOS_ENetworkStatus_ToStringName = "EOS_ENetworkStatus_ToString";

	private const string EOS_EResult_IsOperationCompleteName = "EOS_EResult_IsOperationComplete";

	private const string EOS_EResult_ToStringName = "EOS_EResult_ToString";

	private const string EOS_Ecom_CatalogItem_ReleaseName = "EOS_Ecom_CatalogItem_Release";

	private const string EOS_Ecom_CatalogOffer_ReleaseName = "EOS_Ecom_CatalogOffer_Release";

	private const string EOS_Ecom_CatalogRelease_ReleaseName = "EOS_Ecom_CatalogRelease_Release";

	private const string EOS_Ecom_CheckoutName = "EOS_Ecom_Checkout";

	private const string EOS_Ecom_CopyEntitlementByIdName = "EOS_Ecom_CopyEntitlementById";

	private const string EOS_Ecom_CopyEntitlementByIndexName = "EOS_Ecom_CopyEntitlementByIndex";

	private const string EOS_Ecom_CopyEntitlementByNameAndIndexName = "EOS_Ecom_CopyEntitlementByNameAndIndex";

	private const string EOS_Ecom_CopyItemByIdName = "EOS_Ecom_CopyItemById";

	private const string EOS_Ecom_CopyItemImageInfoByIndexName = "EOS_Ecom_CopyItemImageInfoByIndex";

	private const string EOS_Ecom_CopyItemReleaseByIndexName = "EOS_Ecom_CopyItemReleaseByIndex";

	private const string EOS_Ecom_CopyLastRedeemedEntitlementByIndexName = "EOS_Ecom_CopyLastRedeemedEntitlementByIndex";

	private const string EOS_Ecom_CopyOfferByIdName = "EOS_Ecom_CopyOfferById";

	private const string EOS_Ecom_CopyOfferByIndexName = "EOS_Ecom_CopyOfferByIndex";

	private const string EOS_Ecom_CopyOfferImageInfoByIndexName = "EOS_Ecom_CopyOfferImageInfoByIndex";

	private const string EOS_Ecom_CopyOfferItemByIndexName = "EOS_Ecom_CopyOfferItemByIndex";

	private const string EOS_Ecom_CopyTransactionByIdName = "EOS_Ecom_CopyTransactionById";

	private const string EOS_Ecom_CopyTransactionByIndexName = "EOS_Ecom_CopyTransactionByIndex";

	private const string EOS_Ecom_Entitlement_ReleaseName = "EOS_Ecom_Entitlement_Release";

	private const string EOS_Ecom_GetEntitlementsByNameCountName = "EOS_Ecom_GetEntitlementsByNameCount";

	private const string EOS_Ecom_GetEntitlementsCountName = "EOS_Ecom_GetEntitlementsCount";

	private const string EOS_Ecom_GetItemImageInfoCountName = "EOS_Ecom_GetItemImageInfoCount";

	private const string EOS_Ecom_GetItemReleaseCountName = "EOS_Ecom_GetItemReleaseCount";

	private const string EOS_Ecom_GetLastRedeemedEntitlementsCountName = "EOS_Ecom_GetLastRedeemedEntitlementsCount";

	private const string EOS_Ecom_GetOfferCountName = "EOS_Ecom_GetOfferCount";

	private const string EOS_Ecom_GetOfferImageInfoCountName = "EOS_Ecom_GetOfferImageInfoCount";

	private const string EOS_Ecom_GetOfferItemCountName = "EOS_Ecom_GetOfferItemCount";

	private const string EOS_Ecom_GetTransactionCountName = "EOS_Ecom_GetTransactionCount";

	private const string EOS_Ecom_KeyImageInfo_ReleaseName = "EOS_Ecom_KeyImageInfo_Release";

	private const string EOS_Ecom_QueryEntitlementTokenName = "EOS_Ecom_QueryEntitlementToken";

	private const string EOS_Ecom_QueryEntitlementsName = "EOS_Ecom_QueryEntitlements";

	private const string EOS_Ecom_QueryOffersName = "EOS_Ecom_QueryOffers";

	private const string EOS_Ecom_QueryOwnershipName = "EOS_Ecom_QueryOwnership";

	private const string EOS_Ecom_QueryOwnershipBySandboxIdsName = "EOS_Ecom_QueryOwnershipBySandboxIds";

	private const string EOS_Ecom_QueryOwnershipTokenName = "EOS_Ecom_QueryOwnershipToken";

	private const string EOS_Ecom_RedeemEntitlementsName = "EOS_Ecom_RedeemEntitlements";

	private const string EOS_Ecom_Transaction_CopyEntitlementByIndexName = "EOS_Ecom_Transaction_CopyEntitlementByIndex";

	private const string EOS_Ecom_Transaction_GetEntitlementsCountName = "EOS_Ecom_Transaction_GetEntitlementsCount";

	private const string EOS_Ecom_Transaction_GetTransactionIdName = "EOS_Ecom_Transaction_GetTransactionId";

	private const string EOS_Ecom_Transaction_ReleaseName = "EOS_Ecom_Transaction_Release";

	private const string EOS_EpicAccountId_FromStringName = "EOS_EpicAccountId_FromString";

	private const string EOS_EpicAccountId_IsValidName = "EOS_EpicAccountId_IsValid";

	private const string EOS_EpicAccountId_ToStringName = "EOS_EpicAccountId_ToString";

	private const string EOS_Friends_AcceptInviteName = "EOS_Friends_AcceptInvite";

	private const string EOS_Friends_AddNotifyBlockedUsersUpdateName = "EOS_Friends_AddNotifyBlockedUsersUpdate";

	private const string EOS_Friends_AddNotifyFriendsUpdateName = "EOS_Friends_AddNotifyFriendsUpdate";

	private const string EOS_Friends_GetBlockedUserAtIndexName = "EOS_Friends_GetBlockedUserAtIndex";

	private const string EOS_Friends_GetBlockedUsersCountName = "EOS_Friends_GetBlockedUsersCount";

	private const string EOS_Friends_GetFriendAtIndexName = "EOS_Friends_GetFriendAtIndex";

	private const string EOS_Friends_GetFriendsCountName = "EOS_Friends_GetFriendsCount";

	private const string EOS_Friends_GetStatusName = "EOS_Friends_GetStatus";

	private const string EOS_Friends_QueryFriendsName = "EOS_Friends_QueryFriends";

	private const string EOS_Friends_RejectInviteName = "EOS_Friends_RejectInvite";

	private const string EOS_Friends_RemoveNotifyBlockedUsersUpdateName = "EOS_Friends_RemoveNotifyBlockedUsersUpdate";

	private const string EOS_Friends_RemoveNotifyFriendsUpdateName = "EOS_Friends_RemoveNotifyFriendsUpdate";

	private const string EOS_Friends_SendInviteName = "EOS_Friends_SendInvite";

	private const string EOS_GetVersionName = "EOS_GetVersion";

	private const string EOS_InitializeName = "EOS_Initialize";

	private const string EOS_IntegratedPlatformOptionsContainer_AddName = "EOS_IntegratedPlatformOptionsContainer_Add";

	private const string EOS_IntegratedPlatformOptionsContainer_ReleaseName = "EOS_IntegratedPlatformOptionsContainer_Release";

	private const string EOS_IntegratedPlatform_AddNotifyUserLoginStatusChangedName = "EOS_IntegratedPlatform_AddNotifyUserLoginStatusChanged";

	private const string EOS_IntegratedPlatform_ClearUserPreLogoutCallbackName = "EOS_IntegratedPlatform_ClearUserPreLogoutCallback";

	private const string EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainerName = "EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainer";

	private const string EOS_IntegratedPlatform_FinalizeDeferredUserLogoutName = "EOS_IntegratedPlatform_FinalizeDeferredUserLogout";

	private const string EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChangedName = "EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChanged";

	private const string EOS_IntegratedPlatform_SetUserLoginStatusName = "EOS_IntegratedPlatform_SetUserLoginStatus";

	private const string EOS_IntegratedPlatform_SetUserPreLogoutCallbackName = "EOS_IntegratedPlatform_SetUserPreLogoutCallback";

	private const string EOS_KWS_AddNotifyPermissionsUpdateReceivedName = "EOS_KWS_AddNotifyPermissionsUpdateReceived";

	private const string EOS_KWS_CopyPermissionByIndexName = "EOS_KWS_CopyPermissionByIndex";

	private const string EOS_KWS_CreateUserName = "EOS_KWS_CreateUser";

	private const string EOS_KWS_GetPermissionByKeyName = "EOS_KWS_GetPermissionByKey";

	private const string EOS_KWS_GetPermissionsCountName = "EOS_KWS_GetPermissionsCount";

	private const string EOS_KWS_PermissionStatus_ReleaseName = "EOS_KWS_PermissionStatus_Release";

	private const string EOS_KWS_QueryAgeGateName = "EOS_KWS_QueryAgeGate";

	private const string EOS_KWS_QueryPermissionsName = "EOS_KWS_QueryPermissions";

	private const string EOS_KWS_RemoveNotifyPermissionsUpdateReceivedName = "EOS_KWS_RemoveNotifyPermissionsUpdateReceived";

	private const string EOS_KWS_RequestPermissionsName = "EOS_KWS_RequestPermissions";

	private const string EOS_KWS_UpdateParentEmailName = "EOS_KWS_UpdateParentEmail";

	private const string EOS_Leaderboards_CopyLeaderboardDefinitionByIndexName = "EOS_Leaderboards_CopyLeaderboardDefinitionByIndex";

	private const string EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardIdName = "EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardId";

	private const string EOS_Leaderboards_CopyLeaderboardRecordByIndexName = "EOS_Leaderboards_CopyLeaderboardRecordByIndex";

	private const string EOS_Leaderboards_CopyLeaderboardRecordByUserIdName = "EOS_Leaderboards_CopyLeaderboardRecordByUserId";

	private const string EOS_Leaderboards_CopyLeaderboardUserScoreByIndexName = "EOS_Leaderboards_CopyLeaderboardUserScoreByIndex";

	private const string EOS_Leaderboards_CopyLeaderboardUserScoreByUserIdName = "EOS_Leaderboards_CopyLeaderboardUserScoreByUserId";

	private const string EOS_Leaderboards_Definition_ReleaseName = "EOS_Leaderboards_Definition_Release";

	private const string EOS_Leaderboards_GetLeaderboardDefinitionCountName = "EOS_Leaderboards_GetLeaderboardDefinitionCount";

	private const string EOS_Leaderboards_GetLeaderboardRecordCountName = "EOS_Leaderboards_GetLeaderboardRecordCount";

	private const string EOS_Leaderboards_GetLeaderboardUserScoreCountName = "EOS_Leaderboards_GetLeaderboardUserScoreCount";

	private const string EOS_Leaderboards_LeaderboardDefinition_ReleaseName = "EOS_Leaderboards_LeaderboardDefinition_Release";

	private const string EOS_Leaderboards_LeaderboardRecord_ReleaseName = "EOS_Leaderboards_LeaderboardRecord_Release";

	private const string EOS_Leaderboards_LeaderboardUserScore_ReleaseName = "EOS_Leaderboards_LeaderboardUserScore_Release";

	private const string EOS_Leaderboards_QueryLeaderboardDefinitionsName = "EOS_Leaderboards_QueryLeaderboardDefinitions";

	private const string EOS_Leaderboards_QueryLeaderboardRanksName = "EOS_Leaderboards_QueryLeaderboardRanks";

	private const string EOS_Leaderboards_QueryLeaderboardUserScoresName = "EOS_Leaderboards_QueryLeaderboardUserScores";

	private const string EOS_LobbyDetails_CopyAttributeByIndexName = "EOS_LobbyDetails_CopyAttributeByIndex";

	private const string EOS_LobbyDetails_CopyAttributeByKeyName = "EOS_LobbyDetails_CopyAttributeByKey";

	private const string EOS_LobbyDetails_CopyInfoName = "EOS_LobbyDetails_CopyInfo";

	private const string EOS_LobbyDetails_CopyMemberAttributeByIndexName = "EOS_LobbyDetails_CopyMemberAttributeByIndex";

	private const string EOS_LobbyDetails_CopyMemberAttributeByKeyName = "EOS_LobbyDetails_CopyMemberAttributeByKey";

	private const string EOS_LobbyDetails_CopyMemberInfoName = "EOS_LobbyDetails_CopyMemberInfo";

	private const string EOS_LobbyDetails_GetAttributeCountName = "EOS_LobbyDetails_GetAttributeCount";

	private const string EOS_LobbyDetails_GetLobbyOwnerName = "EOS_LobbyDetails_GetLobbyOwner";

	private const string EOS_LobbyDetails_GetMemberAttributeCountName = "EOS_LobbyDetails_GetMemberAttributeCount";

	private const string EOS_LobbyDetails_GetMemberByIndexName = "EOS_LobbyDetails_GetMemberByIndex";

	private const string EOS_LobbyDetails_GetMemberCountName = "EOS_LobbyDetails_GetMemberCount";

	private const string EOS_LobbyDetails_Info_ReleaseName = "EOS_LobbyDetails_Info_Release";

	private const string EOS_LobbyDetails_MemberInfo_ReleaseName = "EOS_LobbyDetails_MemberInfo_Release";

	private const string EOS_LobbyDetails_ReleaseName = "EOS_LobbyDetails_Release";

	private const string EOS_LobbyModification_AddAttributeName = "EOS_LobbyModification_AddAttribute";

	private const string EOS_LobbyModification_AddMemberAttributeName = "EOS_LobbyModification_AddMemberAttribute";

	private const string EOS_LobbyModification_ReleaseName = "EOS_LobbyModification_Release";

	private const string EOS_LobbyModification_RemoveAttributeName = "EOS_LobbyModification_RemoveAttribute";

	private const string EOS_LobbyModification_RemoveMemberAttributeName = "EOS_LobbyModification_RemoveMemberAttribute";

	private const string EOS_LobbyModification_SetAllowedPlatformIdsName = "EOS_LobbyModification_SetAllowedPlatformIds";

	private const string EOS_LobbyModification_SetBucketIdName = "EOS_LobbyModification_SetBucketId";

	private const string EOS_LobbyModification_SetInvitesAllowedName = "EOS_LobbyModification_SetInvitesAllowed";

	private const string EOS_LobbyModification_SetMaxMembersName = "EOS_LobbyModification_SetMaxMembers";

	private const string EOS_LobbyModification_SetPermissionLevelName = "EOS_LobbyModification_SetPermissionLevel";

	private const string EOS_LobbySearch_CopySearchResultByIndexName = "EOS_LobbySearch_CopySearchResultByIndex";

	private const string EOS_LobbySearch_FindName = "EOS_LobbySearch_Find";

	private const string EOS_LobbySearch_GetSearchResultCountName = "EOS_LobbySearch_GetSearchResultCount";

	private const string EOS_LobbySearch_ReleaseName = "EOS_LobbySearch_Release";

	private const string EOS_LobbySearch_RemoveParameterName = "EOS_LobbySearch_RemoveParameter";

	private const string EOS_LobbySearch_SetLobbyIdName = "EOS_LobbySearch_SetLobbyId";

	private const string EOS_LobbySearch_SetMaxResultsName = "EOS_LobbySearch_SetMaxResults";

	private const string EOS_LobbySearch_SetParameterName = "EOS_LobbySearch_SetParameter";

	private const string EOS_LobbySearch_SetTargetUserIdName = "EOS_LobbySearch_SetTargetUserId";

	private const string EOS_Lobby_AddNotifyJoinLobbyAcceptedName = "EOS_Lobby_AddNotifyJoinLobbyAccepted";

	private const string EOS_Lobby_AddNotifyLeaveLobbyRequestedName = "EOS_Lobby_AddNotifyLeaveLobbyRequested";

	private const string EOS_Lobby_AddNotifyLobbyInviteAcceptedName = "EOS_Lobby_AddNotifyLobbyInviteAccepted";

	private const string EOS_Lobby_AddNotifyLobbyInviteReceivedName = "EOS_Lobby_AddNotifyLobbyInviteReceived";

	private const string EOS_Lobby_AddNotifyLobbyInviteRejectedName = "EOS_Lobby_AddNotifyLobbyInviteRejected";

	private const string EOS_Lobby_AddNotifyLobbyMemberStatusReceivedName = "EOS_Lobby_AddNotifyLobbyMemberStatusReceived";

	private const string EOS_Lobby_AddNotifyLobbyMemberUpdateReceivedName = "EOS_Lobby_AddNotifyLobbyMemberUpdateReceived";

	private const string EOS_Lobby_AddNotifyLobbyUpdateReceivedName = "EOS_Lobby_AddNotifyLobbyUpdateReceived";

	private const string EOS_Lobby_AddNotifyRTCRoomConnectionChangedName = "EOS_Lobby_AddNotifyRTCRoomConnectionChanged";

	private const string EOS_Lobby_AddNotifySendLobbyNativeInviteRequestedName = "EOS_Lobby_AddNotifySendLobbyNativeInviteRequested";

	private const string EOS_Lobby_Attribute_ReleaseName = "EOS_Lobby_Attribute_Release";

	private const string EOS_Lobby_CopyLobbyDetailsHandleName = "EOS_Lobby_CopyLobbyDetailsHandle";

	private const string EOS_Lobby_CopyLobbyDetailsHandleByInviteIdName = "EOS_Lobby_CopyLobbyDetailsHandleByInviteId";

	private const string EOS_Lobby_CopyLobbyDetailsHandleByUiEventIdName = "EOS_Lobby_CopyLobbyDetailsHandleByUiEventId";

	private const string EOS_Lobby_CreateLobbyName = "EOS_Lobby_CreateLobby";

	private const string EOS_Lobby_CreateLobbySearchName = "EOS_Lobby_CreateLobbySearch";

	private const string EOS_Lobby_DestroyLobbyName = "EOS_Lobby_DestroyLobby";

	private const string EOS_Lobby_GetConnectStringName = "EOS_Lobby_GetConnectString";

	private const string EOS_Lobby_GetInviteCountName = "EOS_Lobby_GetInviteCount";

	private const string EOS_Lobby_GetInviteIdByIndexName = "EOS_Lobby_GetInviteIdByIndex";

	private const string EOS_Lobby_GetRTCRoomNameName = "EOS_Lobby_GetRTCRoomName";

	private const string EOS_Lobby_HardMuteMemberName = "EOS_Lobby_HardMuteMember";

	private const string EOS_Lobby_IsRTCRoomConnectedName = "EOS_Lobby_IsRTCRoomConnected";

	private const string EOS_Lobby_JoinLobbyName = "EOS_Lobby_JoinLobby";

	private const string EOS_Lobby_JoinLobbyByIdName = "EOS_Lobby_JoinLobbyById";

	private const string EOS_Lobby_KickMemberName = "EOS_Lobby_KickMember";

	private const string EOS_Lobby_LeaveLobbyName = "EOS_Lobby_LeaveLobby";

	private const string EOS_Lobby_ParseConnectStringName = "EOS_Lobby_ParseConnectString";

	private const string EOS_Lobby_PromoteMemberName = "EOS_Lobby_PromoteMember";

	private const string EOS_Lobby_QueryInvitesName = "EOS_Lobby_QueryInvites";

	private const string EOS_Lobby_RejectInviteName = "EOS_Lobby_RejectInvite";

	private const string EOS_Lobby_RemoveNotifyJoinLobbyAcceptedName = "EOS_Lobby_RemoveNotifyJoinLobbyAccepted";

	private const string EOS_Lobby_RemoveNotifyLeaveLobbyRequestedName = "EOS_Lobby_RemoveNotifyLeaveLobbyRequested";

	private const string EOS_Lobby_RemoveNotifyLobbyInviteAcceptedName = "EOS_Lobby_RemoveNotifyLobbyInviteAccepted";

	private const string EOS_Lobby_RemoveNotifyLobbyInviteReceivedName = "EOS_Lobby_RemoveNotifyLobbyInviteReceived";

	private const string EOS_Lobby_RemoveNotifyLobbyInviteRejectedName = "EOS_Lobby_RemoveNotifyLobbyInviteRejected";

	private const string EOS_Lobby_RemoveNotifyLobbyMemberStatusReceivedName = "EOS_Lobby_RemoveNotifyLobbyMemberStatusReceived";

	private const string EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceivedName = "EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceived";

	private const string EOS_Lobby_RemoveNotifyLobbyUpdateReceivedName = "EOS_Lobby_RemoveNotifyLobbyUpdateReceived";

	private const string EOS_Lobby_RemoveNotifyRTCRoomConnectionChangedName = "EOS_Lobby_RemoveNotifyRTCRoomConnectionChanged";

	private const string EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequestedName = "EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequested";

	private const string EOS_Lobby_SendInviteName = "EOS_Lobby_SendInvite";

	private const string EOS_Lobby_UpdateLobbyName = "EOS_Lobby_UpdateLobby";

	private const string EOS_Lobby_UpdateLobbyModificationName = "EOS_Lobby_UpdateLobbyModification";

	private const string EOS_Logging_SetCallbackName = "EOS_Logging_SetCallback";

	private const string EOS_Logging_SetLogLevelName = "EOS_Logging_SetLogLevel";

	private const string EOS_Metrics_BeginPlayerSessionName = "EOS_Metrics_BeginPlayerSession";

	private const string EOS_Metrics_EndPlayerSessionName = "EOS_Metrics_EndPlayerSession";

	private const string EOS_Mods_CopyModInfoName = "EOS_Mods_CopyModInfo";

	private const string EOS_Mods_EnumerateModsName = "EOS_Mods_EnumerateMods";

	private const string EOS_Mods_InstallModName = "EOS_Mods_InstallMod";

	private const string EOS_Mods_ModInfo_ReleaseName = "EOS_Mods_ModInfo_Release";

	private const string EOS_Mods_UninstallModName = "EOS_Mods_UninstallMod";

	private const string EOS_Mods_UpdateModName = "EOS_Mods_UpdateMod";

	private const string EOS_P2P_AcceptConnectionName = "EOS_P2P_AcceptConnection";

	private const string EOS_P2P_AddNotifyIncomingPacketQueueFullName = "EOS_P2P_AddNotifyIncomingPacketQueueFull";

	private const string EOS_P2P_AddNotifyPeerConnectionClosedName = "EOS_P2P_AddNotifyPeerConnectionClosed";

	private const string EOS_P2P_AddNotifyPeerConnectionEstablishedName = "EOS_P2P_AddNotifyPeerConnectionEstablished";

	private const string EOS_P2P_AddNotifyPeerConnectionInterruptedName = "EOS_P2P_AddNotifyPeerConnectionInterrupted";

	private const string EOS_P2P_AddNotifyPeerConnectionRequestName = "EOS_P2P_AddNotifyPeerConnectionRequest";

	private const string EOS_P2P_ClearPacketQueueName = "EOS_P2P_ClearPacketQueue";

	private const string EOS_P2P_CloseConnectionName = "EOS_P2P_CloseConnection";

	private const string EOS_P2P_CloseConnectionsName = "EOS_P2P_CloseConnections";

	private const string EOS_P2P_GetNATTypeName = "EOS_P2P_GetNATType";

	private const string EOS_P2P_GetNextReceivedPacketSizeName = "EOS_P2P_GetNextReceivedPacketSize";

	private const string EOS_P2P_GetPacketQueueInfoName = "EOS_P2P_GetPacketQueueInfo";

	private const string EOS_P2P_GetPortRangeName = "EOS_P2P_GetPortRange";

	private const string EOS_P2P_GetRelayControlName = "EOS_P2P_GetRelayControl";

	private const string EOS_P2P_QueryNATTypeName = "EOS_P2P_QueryNATType";

	private const string EOS_P2P_ReceivePacketName = "EOS_P2P_ReceivePacket";

	private const string EOS_P2P_RemoveNotifyIncomingPacketQueueFullName = "EOS_P2P_RemoveNotifyIncomingPacketQueueFull";

	private const string EOS_P2P_RemoveNotifyPeerConnectionClosedName = "EOS_P2P_RemoveNotifyPeerConnectionClosed";

	private const string EOS_P2P_RemoveNotifyPeerConnectionEstablishedName = "EOS_P2P_RemoveNotifyPeerConnectionEstablished";

	private const string EOS_P2P_RemoveNotifyPeerConnectionInterruptedName = "EOS_P2P_RemoveNotifyPeerConnectionInterrupted";

	private const string EOS_P2P_RemoveNotifyPeerConnectionRequestName = "EOS_P2P_RemoveNotifyPeerConnectionRequest";

	private const string EOS_P2P_SendPacketName = "EOS_P2P_SendPacket";

	private const string EOS_P2P_SetPacketQueueSizeName = "EOS_P2P_SetPacketQueueSize";

	private const string EOS_P2P_SetPortRangeName = "EOS_P2P_SetPortRange";

	private const string EOS_P2P_SetRelayControlName = "EOS_P2P_SetRelayControl";

	private const string EOS_Platform_CheckForLauncherAndRestartName = "EOS_Platform_CheckForLauncherAndRestart";

	private const string EOS_Platform_CreateName = "EOS_Platform_Create";

	private const string EOS_Platform_GetAchievementsInterfaceName = "EOS_Platform_GetAchievementsInterface";

	private const string EOS_Platform_GetActiveCountryCodeName = "EOS_Platform_GetActiveCountryCode";

	private const string EOS_Platform_GetActiveLocaleCodeName = "EOS_Platform_GetActiveLocaleCode";

	private const string EOS_Platform_GetAntiCheatClientInterfaceName = "EOS_Platform_GetAntiCheatClientInterface";

	private const string EOS_Platform_GetAntiCheatServerInterfaceName = "EOS_Platform_GetAntiCheatServerInterface";

	private const string EOS_Platform_GetApplicationStatusName = "EOS_Platform_GetApplicationStatus";

	private const string EOS_Platform_GetAuthInterfaceName = "EOS_Platform_GetAuthInterface";

	private const string EOS_Platform_GetConnectInterfaceName = "EOS_Platform_GetConnectInterface";

	private const string EOS_Platform_GetCustomInvitesInterfaceName = "EOS_Platform_GetCustomInvitesInterface";

	private const string EOS_Platform_GetDesktopCrossplayStatusName = "EOS_Platform_GetDesktopCrossplayStatus";

	private const string EOS_Platform_GetEcomInterfaceName = "EOS_Platform_GetEcomInterface";

	private const string EOS_Platform_GetFriendsInterfaceName = "EOS_Platform_GetFriendsInterface";

	private const string EOS_Platform_GetIntegratedPlatformInterfaceName = "EOS_Platform_GetIntegratedPlatformInterface";

	private const string EOS_Platform_GetKWSInterfaceName = "EOS_Platform_GetKWSInterface";

	private const string EOS_Platform_GetLeaderboardsInterfaceName = "EOS_Platform_GetLeaderboardsInterface";

	private const string EOS_Platform_GetLobbyInterfaceName = "EOS_Platform_GetLobbyInterface";

	private const string EOS_Platform_GetMetricsInterfaceName = "EOS_Platform_GetMetricsInterface";

	private const string EOS_Platform_GetModsInterfaceName = "EOS_Platform_GetModsInterface";

	private const string EOS_Platform_GetNetworkStatusName = "EOS_Platform_GetNetworkStatus";

	private const string EOS_Platform_GetOverrideCountryCodeName = "EOS_Platform_GetOverrideCountryCode";

	private const string EOS_Platform_GetOverrideLocaleCodeName = "EOS_Platform_GetOverrideLocaleCode";

	private const string EOS_Platform_GetP2PInterfaceName = "EOS_Platform_GetP2PInterface";

	private const string EOS_Platform_GetPlayerDataStorageInterfaceName = "EOS_Platform_GetPlayerDataStorageInterface";

	private const string EOS_Platform_GetPresenceInterfaceName = "EOS_Platform_GetPresenceInterface";

	private const string EOS_Platform_GetProgressionSnapshotInterfaceName = "EOS_Platform_GetProgressionSnapshotInterface";

	private const string EOS_Platform_GetRTCAdminInterfaceName = "EOS_Platform_GetRTCAdminInterface";

	private const string EOS_Platform_GetRTCInterfaceName = "EOS_Platform_GetRTCInterface";

	private const string EOS_Platform_GetReportsInterfaceName = "EOS_Platform_GetReportsInterface";

	private const string EOS_Platform_GetSanctionsInterfaceName = "EOS_Platform_GetSanctionsInterface";

	private const string EOS_Platform_GetSessionsInterfaceName = "EOS_Platform_GetSessionsInterface";

	private const string EOS_Platform_GetStatsInterfaceName = "EOS_Platform_GetStatsInterface";

	private const string EOS_Platform_GetTitleStorageInterfaceName = "EOS_Platform_GetTitleStorageInterface";

	private const string EOS_Platform_GetUIInterfaceName = "EOS_Platform_GetUIInterface";

	private const string EOS_Platform_GetUserInfoInterfaceName = "EOS_Platform_GetUserInfoInterface";

	private const string EOS_Platform_ReleaseName = "EOS_Platform_Release";

	private const string EOS_Platform_SetApplicationStatusName = "EOS_Platform_SetApplicationStatus";

	private const string EOS_Platform_SetNetworkStatusName = "EOS_Platform_SetNetworkStatus";

	private const string EOS_Platform_SetOverrideCountryCodeName = "EOS_Platform_SetOverrideCountryCode";

	private const string EOS_Platform_SetOverrideLocaleCodeName = "EOS_Platform_SetOverrideLocaleCode";

	private const string EOS_Platform_TickName = "EOS_Platform_Tick";

	private const string EOS_PlayerDataStorageFileTransferRequest_CancelRequestName = "EOS_PlayerDataStorageFileTransferRequest_CancelRequest";

	private const string EOS_PlayerDataStorageFileTransferRequest_GetFileRequestStateName = "EOS_PlayerDataStorageFileTransferRequest_GetFileRequestState";

	private const string EOS_PlayerDataStorageFileTransferRequest_GetFilenameName = "EOS_PlayerDataStorageFileTransferRequest_GetFilename";

	private const string EOS_PlayerDataStorageFileTransferRequest_ReleaseName = "EOS_PlayerDataStorageFileTransferRequest_Release";

	private const string EOS_PlayerDataStorage_CopyFileMetadataAtIndexName = "EOS_PlayerDataStorage_CopyFileMetadataAtIndex";

	private const string EOS_PlayerDataStorage_CopyFileMetadataByFilenameName = "EOS_PlayerDataStorage_CopyFileMetadataByFilename";

	private const string EOS_PlayerDataStorage_DeleteCacheName = "EOS_PlayerDataStorage_DeleteCache";

	private const string EOS_PlayerDataStorage_DeleteFileName = "EOS_PlayerDataStorage_DeleteFile";

	private const string EOS_PlayerDataStorage_DuplicateFileName = "EOS_PlayerDataStorage_DuplicateFile";

	private const string EOS_PlayerDataStorage_FileMetadata_ReleaseName = "EOS_PlayerDataStorage_FileMetadata_Release";

	private const string EOS_PlayerDataStorage_GetFileMetadataCountName = "EOS_PlayerDataStorage_GetFileMetadataCount";

	private const string EOS_PlayerDataStorage_QueryFileName = "EOS_PlayerDataStorage_QueryFile";

	private const string EOS_PlayerDataStorage_QueryFileListName = "EOS_PlayerDataStorage_QueryFileList";

	private const string EOS_PlayerDataStorage_ReadFileName = "EOS_PlayerDataStorage_ReadFile";

	private const string EOS_PlayerDataStorage_WriteFileName = "EOS_PlayerDataStorage_WriteFile";

	private const string EOS_PresenceModification_DeleteDataName = "EOS_PresenceModification_DeleteData";

	private const string EOS_PresenceModification_ReleaseName = "EOS_PresenceModification_Release";

	private const string EOS_PresenceModification_SetDataName = "EOS_PresenceModification_SetData";

	private const string EOS_PresenceModification_SetJoinInfoName = "EOS_PresenceModification_SetJoinInfo";

	private const string EOS_PresenceModification_SetRawRichTextName = "EOS_PresenceModification_SetRawRichText";

	private const string EOS_PresenceModification_SetStatusName = "EOS_PresenceModification_SetStatus";

	private const string EOS_Presence_AddNotifyJoinGameAcceptedName = "EOS_Presence_AddNotifyJoinGameAccepted";

	private const string EOS_Presence_AddNotifyOnPresenceChangedName = "EOS_Presence_AddNotifyOnPresenceChanged";

	private const string EOS_Presence_CopyPresenceName = "EOS_Presence_CopyPresence";

	private const string EOS_Presence_CreatePresenceModificationName = "EOS_Presence_CreatePresenceModification";

	private const string EOS_Presence_GetJoinInfoName = "EOS_Presence_GetJoinInfo";

	private const string EOS_Presence_HasPresenceName = "EOS_Presence_HasPresence";

	private const string EOS_Presence_Info_ReleaseName = "EOS_Presence_Info_Release";

	private const string EOS_Presence_QueryPresenceName = "EOS_Presence_QueryPresence";

	private const string EOS_Presence_RemoveNotifyJoinGameAcceptedName = "EOS_Presence_RemoveNotifyJoinGameAccepted";

	private const string EOS_Presence_RemoveNotifyOnPresenceChangedName = "EOS_Presence_RemoveNotifyOnPresenceChanged";

	private const string EOS_Presence_SetPresenceName = "EOS_Presence_SetPresence";

	private const string EOS_ProductUserId_FromStringName = "EOS_ProductUserId_FromString";

	private const string EOS_ProductUserId_IsValidName = "EOS_ProductUserId_IsValid";

	private const string EOS_ProductUserId_ToStringName = "EOS_ProductUserId_ToString";

	private const string EOS_ProgressionSnapshot_AddProgressionName = "EOS_ProgressionSnapshot_AddProgression";

	private const string EOS_ProgressionSnapshot_BeginSnapshotName = "EOS_ProgressionSnapshot_BeginSnapshot";

	private const string EOS_ProgressionSnapshot_DeleteSnapshotName = "EOS_ProgressionSnapshot_DeleteSnapshot";

	private const string EOS_ProgressionSnapshot_EndSnapshotName = "EOS_ProgressionSnapshot_EndSnapshot";

	private const string EOS_ProgressionSnapshot_SubmitSnapshotName = "EOS_ProgressionSnapshot_SubmitSnapshot";

	private const string EOS_RTCAdmin_CopyUserTokenByIndexName = "EOS_RTCAdmin_CopyUserTokenByIndex";

	private const string EOS_RTCAdmin_CopyUserTokenByUserIdName = "EOS_RTCAdmin_CopyUserTokenByUserId";

	private const string EOS_RTCAdmin_KickName = "EOS_RTCAdmin_Kick";

	private const string EOS_RTCAdmin_QueryJoinRoomTokenName = "EOS_RTCAdmin_QueryJoinRoomToken";

	private const string EOS_RTCAdmin_SetParticipantHardMuteName = "EOS_RTCAdmin_SetParticipantHardMute";

	private const string EOS_RTCAdmin_UserToken_ReleaseName = "EOS_RTCAdmin_UserToken_Release";

	private const string EOS_RTCAudio_AddNotifyAudioBeforeRenderName = "EOS_RTCAudio_AddNotifyAudioBeforeRender";

	private const string EOS_RTCAudio_AddNotifyAudioBeforeSendName = "EOS_RTCAudio_AddNotifyAudioBeforeSend";

	private const string EOS_RTCAudio_AddNotifyAudioDevicesChangedName = "EOS_RTCAudio_AddNotifyAudioDevicesChanged";

	private const string EOS_RTCAudio_AddNotifyAudioInputStateName = "EOS_RTCAudio_AddNotifyAudioInputState";

	private const string EOS_RTCAudio_AddNotifyAudioOutputStateName = "EOS_RTCAudio_AddNotifyAudioOutputState";

	private const string EOS_RTCAudio_AddNotifyParticipantUpdatedName = "EOS_RTCAudio_AddNotifyParticipantUpdated";

	private const string EOS_RTCAudio_CopyInputDeviceInformationByIndexName = "EOS_RTCAudio_CopyInputDeviceInformationByIndex";

	private const string EOS_RTCAudio_CopyOutputDeviceInformationByIndexName = "EOS_RTCAudio_CopyOutputDeviceInformationByIndex";

	private const string EOS_RTCAudio_GetAudioInputDeviceByIndexName = "EOS_RTCAudio_GetAudioInputDeviceByIndex";

	private const string EOS_RTCAudio_GetAudioInputDevicesCountName = "EOS_RTCAudio_GetAudioInputDevicesCount";

	private const string EOS_RTCAudio_GetAudioOutputDeviceByIndexName = "EOS_RTCAudio_GetAudioOutputDeviceByIndex";

	private const string EOS_RTCAudio_GetAudioOutputDevicesCountName = "EOS_RTCAudio_GetAudioOutputDevicesCount";

	private const string EOS_RTCAudio_GetInputDevicesCountName = "EOS_RTCAudio_GetInputDevicesCount";

	private const string EOS_RTCAudio_GetOutputDevicesCountName = "EOS_RTCAudio_GetOutputDevicesCount";

	private const string EOS_RTCAudio_InputDeviceInformation_ReleaseName = "EOS_RTCAudio_InputDeviceInformation_Release";

	private const string EOS_RTCAudio_OutputDeviceInformation_ReleaseName = "EOS_RTCAudio_OutputDeviceInformation_Release";

	private const string EOS_RTCAudio_QueryInputDevicesInformationName = "EOS_RTCAudio_QueryInputDevicesInformation";

	private const string EOS_RTCAudio_QueryOutputDevicesInformationName = "EOS_RTCAudio_QueryOutputDevicesInformation";

	private const string EOS_RTCAudio_RegisterPlatformAudioUserName = "EOS_RTCAudio_RegisterPlatformAudioUser";

	private const string EOS_RTCAudio_RegisterPlatformUserName = "EOS_RTCAudio_RegisterPlatformUser";

	private const string EOS_RTCAudio_RemoveNotifyAudioBeforeRenderName = "EOS_RTCAudio_RemoveNotifyAudioBeforeRender";

	private const string EOS_RTCAudio_RemoveNotifyAudioBeforeSendName = "EOS_RTCAudio_RemoveNotifyAudioBeforeSend";

	private const string EOS_RTCAudio_RemoveNotifyAudioDevicesChangedName = "EOS_RTCAudio_RemoveNotifyAudioDevicesChanged";

	private const string EOS_RTCAudio_RemoveNotifyAudioInputStateName = "EOS_RTCAudio_RemoveNotifyAudioInputState";

	private const string EOS_RTCAudio_RemoveNotifyAudioOutputStateName = "EOS_RTCAudio_RemoveNotifyAudioOutputState";

	private const string EOS_RTCAudio_RemoveNotifyParticipantUpdatedName = "EOS_RTCAudio_RemoveNotifyParticipantUpdated";

	private const string EOS_RTCAudio_SendAudioName = "EOS_RTCAudio_SendAudio";

	private const string EOS_RTCAudio_SetAudioInputSettingsName = "EOS_RTCAudio_SetAudioInputSettings";

	private const string EOS_RTCAudio_SetAudioOutputSettingsName = "EOS_RTCAudio_SetAudioOutputSettings";

	private const string EOS_RTCAudio_SetInputDeviceSettingsName = "EOS_RTCAudio_SetInputDeviceSettings";

	private const string EOS_RTCAudio_SetOutputDeviceSettingsName = "EOS_RTCAudio_SetOutputDeviceSettings";

	private const string EOS_RTCAudio_UnregisterPlatformAudioUserName = "EOS_RTCAudio_UnregisterPlatformAudioUser";

	private const string EOS_RTCAudio_UnregisterPlatformUserName = "EOS_RTCAudio_UnregisterPlatformUser";

	private const string EOS_RTCAudio_UpdateParticipantVolumeName = "EOS_RTCAudio_UpdateParticipantVolume";

	private const string EOS_RTCAudio_UpdateReceivingName = "EOS_RTCAudio_UpdateReceiving";

	private const string EOS_RTCAudio_UpdateReceivingVolumeName = "EOS_RTCAudio_UpdateReceivingVolume";

	private const string EOS_RTCAudio_UpdateSendingName = "EOS_RTCAudio_UpdateSending";

	private const string EOS_RTCAudio_UpdateSendingVolumeName = "EOS_RTCAudio_UpdateSendingVolume";

	private const string EOS_RTC_AddNotifyDisconnectedName = "EOS_RTC_AddNotifyDisconnected";

	private const string EOS_RTC_AddNotifyParticipantStatusChangedName = "EOS_RTC_AddNotifyParticipantStatusChanged";

	private const string EOS_RTC_AddNotifyRoomStatisticsUpdatedName = "EOS_RTC_AddNotifyRoomStatisticsUpdated";

	private const string EOS_RTC_BlockParticipantName = "EOS_RTC_BlockParticipant";

	private const string EOS_RTC_GetAudioInterfaceName = "EOS_RTC_GetAudioInterface";

	private const string EOS_RTC_JoinRoomName = "EOS_RTC_JoinRoom";

	private const string EOS_RTC_LeaveRoomName = "EOS_RTC_LeaveRoom";

	private const string EOS_RTC_RemoveNotifyDisconnectedName = "EOS_RTC_RemoveNotifyDisconnected";

	private const string EOS_RTC_RemoveNotifyParticipantStatusChangedName = "EOS_RTC_RemoveNotifyParticipantStatusChanged";

	private const string EOS_RTC_RemoveNotifyRoomStatisticsUpdatedName = "EOS_RTC_RemoveNotifyRoomStatisticsUpdated";

	private const string EOS_RTC_SetRoomSettingName = "EOS_RTC_SetRoomSetting";

	private const string EOS_RTC_SetSettingName = "EOS_RTC_SetSetting";

	private const string EOS_Reports_SendPlayerBehaviorReportName = "EOS_Reports_SendPlayerBehaviorReport";

	private const string EOS_Sanctions_CopyPlayerSanctionByIndexName = "EOS_Sanctions_CopyPlayerSanctionByIndex";

	private const string EOS_Sanctions_GetPlayerSanctionCountName = "EOS_Sanctions_GetPlayerSanctionCount";

	private const string EOS_Sanctions_PlayerSanction_ReleaseName = "EOS_Sanctions_PlayerSanction_Release";

	private const string EOS_Sanctions_QueryActivePlayerSanctionsName = "EOS_Sanctions_QueryActivePlayerSanctions";

	private const string EOS_SessionDetails_Attribute_ReleaseName = "EOS_SessionDetails_Attribute_Release";

	private const string EOS_SessionDetails_CopyInfoName = "EOS_SessionDetails_CopyInfo";

	private const string EOS_SessionDetails_CopySessionAttributeByIndexName = "EOS_SessionDetails_CopySessionAttributeByIndex";

	private const string EOS_SessionDetails_CopySessionAttributeByKeyName = "EOS_SessionDetails_CopySessionAttributeByKey";

	private const string EOS_SessionDetails_GetSessionAttributeCountName = "EOS_SessionDetails_GetSessionAttributeCount";

	private const string EOS_SessionDetails_Info_ReleaseName = "EOS_SessionDetails_Info_Release";

	private const string EOS_SessionDetails_ReleaseName = "EOS_SessionDetails_Release";

	private const string EOS_SessionModification_AddAttributeName = "EOS_SessionModification_AddAttribute";

	private const string EOS_SessionModification_ReleaseName = "EOS_SessionModification_Release";

	private const string EOS_SessionModification_RemoveAttributeName = "EOS_SessionModification_RemoveAttribute";

	private const string EOS_SessionModification_SetAllowedPlatformIdsName = "EOS_SessionModification_SetAllowedPlatformIds";

	private const string EOS_SessionModification_SetBucketIdName = "EOS_SessionModification_SetBucketId";

	private const string EOS_SessionModification_SetHostAddressName = "EOS_SessionModification_SetHostAddress";

	private const string EOS_SessionModification_SetInvitesAllowedName = "EOS_SessionModification_SetInvitesAllowed";

	private const string EOS_SessionModification_SetJoinInProgressAllowedName = "EOS_SessionModification_SetJoinInProgressAllowed";

	private const string EOS_SessionModification_SetMaxPlayersName = "EOS_SessionModification_SetMaxPlayers";

	private const string EOS_SessionModification_SetPermissionLevelName = "EOS_SessionModification_SetPermissionLevel";

	private const string EOS_SessionSearch_CopySearchResultByIndexName = "EOS_SessionSearch_CopySearchResultByIndex";

	private const string EOS_SessionSearch_FindName = "EOS_SessionSearch_Find";

	private const string EOS_SessionSearch_GetSearchResultCountName = "EOS_SessionSearch_GetSearchResultCount";

	private const string EOS_SessionSearch_ReleaseName = "EOS_SessionSearch_Release";

	private const string EOS_SessionSearch_RemoveParameterName = "EOS_SessionSearch_RemoveParameter";

	private const string EOS_SessionSearch_SetMaxResultsName = "EOS_SessionSearch_SetMaxResults";

	private const string EOS_SessionSearch_SetParameterName = "EOS_SessionSearch_SetParameter";

	private const string EOS_SessionSearch_SetSessionIdName = "EOS_SessionSearch_SetSessionId";

	private const string EOS_SessionSearch_SetTargetUserIdName = "EOS_SessionSearch_SetTargetUserId";

	private const string EOS_Sessions_AddNotifyJoinSessionAcceptedName = "EOS_Sessions_AddNotifyJoinSessionAccepted";

	private const string EOS_Sessions_AddNotifyLeaveSessionRequestedName = "EOS_Sessions_AddNotifyLeaveSessionRequested";

	private const string EOS_Sessions_AddNotifySendSessionNativeInviteRequestedName = "EOS_Sessions_AddNotifySendSessionNativeInviteRequested";

	private const string EOS_Sessions_AddNotifySessionInviteAcceptedName = "EOS_Sessions_AddNotifySessionInviteAccepted";

	private const string EOS_Sessions_AddNotifySessionInviteReceivedName = "EOS_Sessions_AddNotifySessionInviteReceived";

	private const string EOS_Sessions_AddNotifySessionInviteRejectedName = "EOS_Sessions_AddNotifySessionInviteRejected";

	private const string EOS_Sessions_CopyActiveSessionHandleName = "EOS_Sessions_CopyActiveSessionHandle";

	private const string EOS_Sessions_CopySessionHandleByInviteIdName = "EOS_Sessions_CopySessionHandleByInviteId";

	private const string EOS_Sessions_CopySessionHandleByUiEventIdName = "EOS_Sessions_CopySessionHandleByUiEventId";

	private const string EOS_Sessions_CopySessionHandleForPresenceName = "EOS_Sessions_CopySessionHandleForPresence";

	private const string EOS_Sessions_CreateSessionModificationName = "EOS_Sessions_CreateSessionModification";

	private const string EOS_Sessions_CreateSessionSearchName = "EOS_Sessions_CreateSessionSearch";

	private const string EOS_Sessions_DestroySessionName = "EOS_Sessions_DestroySession";

	private const string EOS_Sessions_DumpSessionStateName = "EOS_Sessions_DumpSessionState";

	private const string EOS_Sessions_EndSessionName = "EOS_Sessions_EndSession";

	private const string EOS_Sessions_GetInviteCountName = "EOS_Sessions_GetInviteCount";

	private const string EOS_Sessions_GetInviteIdByIndexName = "EOS_Sessions_GetInviteIdByIndex";

	private const string EOS_Sessions_IsUserInSessionName = "EOS_Sessions_IsUserInSession";

	private const string EOS_Sessions_JoinSessionName = "EOS_Sessions_JoinSession";

	private const string EOS_Sessions_QueryInvitesName = "EOS_Sessions_QueryInvites";

	private const string EOS_Sessions_RegisterPlayersName = "EOS_Sessions_RegisterPlayers";

	private const string EOS_Sessions_RejectInviteName = "EOS_Sessions_RejectInvite";

	private const string EOS_Sessions_RemoveNotifyJoinSessionAcceptedName = "EOS_Sessions_RemoveNotifyJoinSessionAccepted";

	private const string EOS_Sessions_RemoveNotifyLeaveSessionRequestedName = "EOS_Sessions_RemoveNotifyLeaveSessionRequested";

	private const string EOS_Sessions_RemoveNotifySendSessionNativeInviteRequestedName = "EOS_Sessions_RemoveNotifySendSessionNativeInviteRequested";

	private const string EOS_Sessions_RemoveNotifySessionInviteAcceptedName = "EOS_Sessions_RemoveNotifySessionInviteAccepted";

	private const string EOS_Sessions_RemoveNotifySessionInviteReceivedName = "EOS_Sessions_RemoveNotifySessionInviteReceived";

	private const string EOS_Sessions_RemoveNotifySessionInviteRejectedName = "EOS_Sessions_RemoveNotifySessionInviteRejected";

	private const string EOS_Sessions_SendInviteName = "EOS_Sessions_SendInvite";

	private const string EOS_Sessions_StartSessionName = "EOS_Sessions_StartSession";

	private const string EOS_Sessions_UnregisterPlayersName = "EOS_Sessions_UnregisterPlayers";

	private const string EOS_Sessions_UpdateSessionName = "EOS_Sessions_UpdateSession";

	private const string EOS_Sessions_UpdateSessionModificationName = "EOS_Sessions_UpdateSessionModification";

	private const string EOS_ShutdownName = "EOS_Shutdown";

	private const string EOS_Stats_CopyStatByIndexName = "EOS_Stats_CopyStatByIndex";

	private const string EOS_Stats_CopyStatByNameName = "EOS_Stats_CopyStatByName";

	private const string EOS_Stats_GetStatsCountName = "EOS_Stats_GetStatsCount";

	private const string EOS_Stats_IngestStatName = "EOS_Stats_IngestStat";

	private const string EOS_Stats_QueryStatsName = "EOS_Stats_QueryStats";

	private const string EOS_Stats_Stat_ReleaseName = "EOS_Stats_Stat_Release";

	private const string EOS_TitleStorageFileTransferRequest_CancelRequestName = "EOS_TitleStorageFileTransferRequest_CancelRequest";

	private const string EOS_TitleStorageFileTransferRequest_GetFileRequestStateName = "EOS_TitleStorageFileTransferRequest_GetFileRequestState";

	private const string EOS_TitleStorageFileTransferRequest_GetFilenameName = "EOS_TitleStorageFileTransferRequest_GetFilename";

	private const string EOS_TitleStorageFileTransferRequest_ReleaseName = "EOS_TitleStorageFileTransferRequest_Release";

	private const string EOS_TitleStorage_CopyFileMetadataAtIndexName = "EOS_TitleStorage_CopyFileMetadataAtIndex";

	private const string EOS_TitleStorage_CopyFileMetadataByFilenameName = "EOS_TitleStorage_CopyFileMetadataByFilename";

	private const string EOS_TitleStorage_DeleteCacheName = "EOS_TitleStorage_DeleteCache";

	private const string EOS_TitleStorage_FileMetadata_ReleaseName = "EOS_TitleStorage_FileMetadata_Release";

	private const string EOS_TitleStorage_GetFileMetadataCountName = "EOS_TitleStorage_GetFileMetadataCount";

	private const string EOS_TitleStorage_QueryFileName = "EOS_TitleStorage_QueryFile";

	private const string EOS_TitleStorage_QueryFileListName = "EOS_TitleStorage_QueryFileList";

	private const string EOS_TitleStorage_ReadFileName = "EOS_TitleStorage_ReadFile";

	private const string EOS_UI_AcknowledgeEventIdName = "EOS_UI_AcknowledgeEventId";

	private const string EOS_UI_AddNotifyDisplaySettingsUpdatedName = "EOS_UI_AddNotifyDisplaySettingsUpdated";

	private const string EOS_UI_AddNotifyMemoryMonitorName = "EOS_UI_AddNotifyMemoryMonitor";

	private const string EOS_UI_GetFriendsExclusiveInputName = "EOS_UI_GetFriendsExclusiveInput";

	private const string EOS_UI_GetFriendsVisibleName = "EOS_UI_GetFriendsVisible";

	private const string EOS_UI_GetNotificationLocationPreferenceName = "EOS_UI_GetNotificationLocationPreference";

	private const string EOS_UI_GetToggleFriendsButtonName = "EOS_UI_GetToggleFriendsButton";

	private const string EOS_UI_GetToggleFriendsKeyName = "EOS_UI_GetToggleFriendsKey";

	private const string EOS_UI_HideFriendsName = "EOS_UI_HideFriends";

	private const string EOS_UI_IsSocialOverlayPausedName = "EOS_UI_IsSocialOverlayPaused";

	private const string EOS_UI_IsValidButtonCombinationName = "EOS_UI_IsValidButtonCombination";

	private const string EOS_UI_IsValidKeyCombinationName = "EOS_UI_IsValidKeyCombination";

	private const string EOS_UI_PauseSocialOverlayName = "EOS_UI_PauseSocialOverlay";

	private const string EOS_UI_PrePresentName = "EOS_UI_PrePresent";

	private const string EOS_UI_RemoveNotifyDisplaySettingsUpdatedName = "EOS_UI_RemoveNotifyDisplaySettingsUpdated";

	private const string EOS_UI_RemoveNotifyMemoryMonitorName = "EOS_UI_RemoveNotifyMemoryMonitor";

	private const string EOS_UI_ReportInputStateName = "EOS_UI_ReportInputState";

	private const string EOS_UI_SetDisplayPreferenceName = "EOS_UI_SetDisplayPreference";

	private const string EOS_UI_SetToggleFriendsButtonName = "EOS_UI_SetToggleFriendsButton";

	private const string EOS_UI_SetToggleFriendsKeyName = "EOS_UI_SetToggleFriendsKey";

	private const string EOS_UI_ShowBlockPlayerName = "EOS_UI_ShowBlockPlayer";

	private const string EOS_UI_ShowFriendsName = "EOS_UI_ShowFriends";

	private const string EOS_UI_ShowNativeProfileName = "EOS_UI_ShowNativeProfile";

	private const string EOS_UI_ShowReportPlayerName = "EOS_UI_ShowReportPlayer";

	private const string EOS_UserInfo_BestDisplayName_ReleaseName = "EOS_UserInfo_BestDisplayName_Release";

	private const string EOS_UserInfo_CopyBestDisplayNameName = "EOS_UserInfo_CopyBestDisplayName";

	private const string EOS_UserInfo_CopyBestDisplayNameWithPlatformName = "EOS_UserInfo_CopyBestDisplayNameWithPlatform";

	private const string EOS_UserInfo_CopyExternalUserInfoByAccountIdName = "EOS_UserInfo_CopyExternalUserInfoByAccountId";

	private const string EOS_UserInfo_CopyExternalUserInfoByAccountTypeName = "EOS_UserInfo_CopyExternalUserInfoByAccountType";

	private const string EOS_UserInfo_CopyExternalUserInfoByIndexName = "EOS_UserInfo_CopyExternalUserInfoByIndex";

	private const string EOS_UserInfo_CopyUserInfoName = "EOS_UserInfo_CopyUserInfo";

	private const string EOS_UserInfo_ExternalUserInfo_ReleaseName = "EOS_UserInfo_ExternalUserInfo_Release";

	private const string EOS_UserInfo_GetExternalUserInfoCountName = "EOS_UserInfo_GetExternalUserInfoCount";

	private const string EOS_UserInfo_GetLocalPlatformTypeName = "EOS_UserInfo_GetLocalPlatformType";

	private const string EOS_UserInfo_QueryUserInfoName = "EOS_UserInfo_QueryUserInfo";

	private const string EOS_UserInfo_QueryUserInfoByDisplayNameName = "EOS_UserInfo_QueryUserInfoByDisplayName";

	private const string EOS_UserInfo_QueryUserInfoByExternalAccountName = "EOS_UserInfo_QueryUserInfoByExternalAccount";

	private const string EOS_UserInfo_ReleaseName = "EOS_UserInfo_Release";

	internal static EOS_Achievements_AddNotifyAchievementsUnlockedDelegate EOS_Achievements_AddNotifyAchievementsUnlocked;

	internal static EOS_Achievements_AddNotifyAchievementsUnlockedV2Delegate EOS_Achievements_AddNotifyAchievementsUnlockedV2;

	internal static EOS_Achievements_CopyAchievementDefinitionByAchievementIdDelegate EOS_Achievements_CopyAchievementDefinitionByAchievementId;

	internal static EOS_Achievements_CopyAchievementDefinitionByIndexDelegate EOS_Achievements_CopyAchievementDefinitionByIndex;

	internal static EOS_Achievements_CopyAchievementDefinitionV2ByAchievementIdDelegate EOS_Achievements_CopyAchievementDefinitionV2ByAchievementId;

	internal static EOS_Achievements_CopyAchievementDefinitionV2ByIndexDelegate EOS_Achievements_CopyAchievementDefinitionV2ByIndex;

	internal static EOS_Achievements_CopyPlayerAchievementByAchievementIdDelegate EOS_Achievements_CopyPlayerAchievementByAchievementId;

	internal static EOS_Achievements_CopyPlayerAchievementByIndexDelegate EOS_Achievements_CopyPlayerAchievementByIndex;

	internal static EOS_Achievements_CopyUnlockedAchievementByAchievementIdDelegate EOS_Achievements_CopyUnlockedAchievementByAchievementId;

	internal static EOS_Achievements_CopyUnlockedAchievementByIndexDelegate EOS_Achievements_CopyUnlockedAchievementByIndex;

	internal static EOS_Achievements_DefinitionV2_ReleaseDelegate EOS_Achievements_DefinitionV2_Release;

	internal static EOS_Achievements_Definition_ReleaseDelegate EOS_Achievements_Definition_Release;

	internal static EOS_Achievements_GetAchievementDefinitionCountDelegate EOS_Achievements_GetAchievementDefinitionCount;

	internal static EOS_Achievements_GetPlayerAchievementCountDelegate EOS_Achievements_GetPlayerAchievementCount;

	internal static EOS_Achievements_GetUnlockedAchievementCountDelegate EOS_Achievements_GetUnlockedAchievementCount;

	internal static EOS_Achievements_PlayerAchievement_ReleaseDelegate EOS_Achievements_PlayerAchievement_Release;

	internal static EOS_Achievements_QueryDefinitionsDelegate EOS_Achievements_QueryDefinitions;

	internal static EOS_Achievements_QueryPlayerAchievementsDelegate EOS_Achievements_QueryPlayerAchievements;

	internal static EOS_Achievements_RemoveNotifyAchievementsUnlockedDelegate EOS_Achievements_RemoveNotifyAchievementsUnlocked;

	internal static EOS_Achievements_UnlockAchievementsDelegate EOS_Achievements_UnlockAchievements;

	internal static EOS_Achievements_UnlockedAchievement_ReleaseDelegate EOS_Achievements_UnlockedAchievement_Release;

	internal static EOS_ActiveSession_CopyInfoDelegate EOS_ActiveSession_CopyInfo;

	internal static EOS_ActiveSession_GetRegisteredPlayerByIndexDelegate EOS_ActiveSession_GetRegisteredPlayerByIndex;

	internal static EOS_ActiveSession_GetRegisteredPlayerCountDelegate EOS_ActiveSession_GetRegisteredPlayerCount;

	internal static EOS_ActiveSession_Info_ReleaseDelegate EOS_ActiveSession_Info_Release;

	internal static EOS_ActiveSession_ReleaseDelegate EOS_ActiveSession_Release;

	internal static EOS_AntiCheatClient_AddExternalIntegrityCatalogDelegate EOS_AntiCheatClient_AddExternalIntegrityCatalog;

	internal static EOS_AntiCheatClient_AddNotifyClientIntegrityViolatedDelegate EOS_AntiCheatClient_AddNotifyClientIntegrityViolated;

	internal static EOS_AntiCheatClient_AddNotifyMessageToPeerDelegate EOS_AntiCheatClient_AddNotifyMessageToPeer;

	internal static EOS_AntiCheatClient_AddNotifyMessageToServerDelegate EOS_AntiCheatClient_AddNotifyMessageToServer;

	internal static EOS_AntiCheatClient_AddNotifyPeerActionRequiredDelegate EOS_AntiCheatClient_AddNotifyPeerActionRequired;

	internal static EOS_AntiCheatClient_AddNotifyPeerAuthStatusChangedDelegate EOS_AntiCheatClient_AddNotifyPeerAuthStatusChanged;

	internal static EOS_AntiCheatClient_BeginSessionDelegate EOS_AntiCheatClient_BeginSession;

	internal static EOS_AntiCheatClient_EndSessionDelegate EOS_AntiCheatClient_EndSession;

	internal static EOS_AntiCheatClient_GetProtectMessageOutputLengthDelegate EOS_AntiCheatClient_GetProtectMessageOutputLength;

	internal static EOS_AntiCheatClient_PollStatusDelegate EOS_AntiCheatClient_PollStatus;

	internal static EOS_AntiCheatClient_ProtectMessageDelegate EOS_AntiCheatClient_ProtectMessage;

	internal static EOS_AntiCheatClient_ReceiveMessageFromPeerDelegate EOS_AntiCheatClient_ReceiveMessageFromPeer;

	internal static EOS_AntiCheatClient_ReceiveMessageFromServerDelegate EOS_AntiCheatClient_ReceiveMessageFromServer;

	internal static EOS_AntiCheatClient_RegisterPeerDelegate EOS_AntiCheatClient_RegisterPeer;

	internal static EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolatedDelegate EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolated;

	internal static EOS_AntiCheatClient_RemoveNotifyMessageToPeerDelegate EOS_AntiCheatClient_RemoveNotifyMessageToPeer;

	internal static EOS_AntiCheatClient_RemoveNotifyMessageToServerDelegate EOS_AntiCheatClient_RemoveNotifyMessageToServer;

	internal static EOS_AntiCheatClient_RemoveNotifyPeerActionRequiredDelegate EOS_AntiCheatClient_RemoveNotifyPeerActionRequired;

	internal static EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChangedDelegate EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChanged;

	internal static EOS_AntiCheatClient_UnprotectMessageDelegate EOS_AntiCheatClient_UnprotectMessage;

	internal static EOS_AntiCheatClient_UnregisterPeerDelegate EOS_AntiCheatClient_UnregisterPeer;

	internal static EOS_AntiCheatServer_AddNotifyClientActionRequiredDelegate EOS_AntiCheatServer_AddNotifyClientActionRequired;

	internal static EOS_AntiCheatServer_AddNotifyClientAuthStatusChangedDelegate EOS_AntiCheatServer_AddNotifyClientAuthStatusChanged;

	internal static EOS_AntiCheatServer_AddNotifyMessageToClientDelegate EOS_AntiCheatServer_AddNotifyMessageToClient;

	internal static EOS_AntiCheatServer_BeginSessionDelegate EOS_AntiCheatServer_BeginSession;

	internal static EOS_AntiCheatServer_EndSessionDelegate EOS_AntiCheatServer_EndSession;

	internal static EOS_AntiCheatServer_GetProtectMessageOutputLengthDelegate EOS_AntiCheatServer_GetProtectMessageOutputLength;

	internal static EOS_AntiCheatServer_LogEventDelegate EOS_AntiCheatServer_LogEvent;

	internal static EOS_AntiCheatServer_LogGameRoundEndDelegate EOS_AntiCheatServer_LogGameRoundEnd;

	internal static EOS_AntiCheatServer_LogGameRoundStartDelegate EOS_AntiCheatServer_LogGameRoundStart;

	internal static EOS_AntiCheatServer_LogPlayerDespawnDelegate EOS_AntiCheatServer_LogPlayerDespawn;

	internal static EOS_AntiCheatServer_LogPlayerReviveDelegate EOS_AntiCheatServer_LogPlayerRevive;

	internal static EOS_AntiCheatServer_LogPlayerSpawnDelegate EOS_AntiCheatServer_LogPlayerSpawn;

	internal static EOS_AntiCheatServer_LogPlayerTakeDamageDelegate EOS_AntiCheatServer_LogPlayerTakeDamage;

	internal static EOS_AntiCheatServer_LogPlayerTickDelegate EOS_AntiCheatServer_LogPlayerTick;

	internal static EOS_AntiCheatServer_LogPlayerUseAbilityDelegate EOS_AntiCheatServer_LogPlayerUseAbility;

	internal static EOS_AntiCheatServer_LogPlayerUseWeaponDelegate EOS_AntiCheatServer_LogPlayerUseWeapon;

	internal static EOS_AntiCheatServer_ProtectMessageDelegate EOS_AntiCheatServer_ProtectMessage;

	internal static EOS_AntiCheatServer_ReceiveMessageFromClientDelegate EOS_AntiCheatServer_ReceiveMessageFromClient;

	internal static EOS_AntiCheatServer_RegisterClientDelegate EOS_AntiCheatServer_RegisterClient;

	internal static EOS_AntiCheatServer_RegisterEventDelegate EOS_AntiCheatServer_RegisterEvent;

	internal static EOS_AntiCheatServer_RemoveNotifyClientActionRequiredDelegate EOS_AntiCheatServer_RemoveNotifyClientActionRequired;

	internal static EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChangedDelegate EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChanged;

	internal static EOS_AntiCheatServer_RemoveNotifyMessageToClientDelegate EOS_AntiCheatServer_RemoveNotifyMessageToClient;

	internal static EOS_AntiCheatServer_SetClientDetailsDelegate EOS_AntiCheatServer_SetClientDetails;

	internal static EOS_AntiCheatServer_SetClientNetworkStateDelegate EOS_AntiCheatServer_SetClientNetworkState;

	internal static EOS_AntiCheatServer_SetGameSessionIdDelegate EOS_AntiCheatServer_SetGameSessionId;

	internal static EOS_AntiCheatServer_UnprotectMessageDelegate EOS_AntiCheatServer_UnprotectMessage;

	internal static EOS_AntiCheatServer_UnregisterClientDelegate EOS_AntiCheatServer_UnregisterClient;

	internal static EOS_Auth_AddNotifyLoginStatusChangedDelegate EOS_Auth_AddNotifyLoginStatusChanged;

	internal static EOS_Auth_CopyIdTokenDelegate EOS_Auth_CopyIdToken;

	internal static EOS_Auth_CopyUserAuthTokenDelegate EOS_Auth_CopyUserAuthToken;

	internal static EOS_Auth_DeletePersistentAuthDelegate EOS_Auth_DeletePersistentAuth;

	internal static EOS_Auth_GetLoggedInAccountByIndexDelegate EOS_Auth_GetLoggedInAccountByIndex;

	internal static EOS_Auth_GetLoggedInAccountsCountDelegate EOS_Auth_GetLoggedInAccountsCount;

	internal static EOS_Auth_GetLoginStatusDelegate EOS_Auth_GetLoginStatus;

	internal static EOS_Auth_GetMergedAccountByIndexDelegate EOS_Auth_GetMergedAccountByIndex;

	internal static EOS_Auth_GetMergedAccountsCountDelegate EOS_Auth_GetMergedAccountsCount;

	internal static EOS_Auth_GetSelectedAccountIdDelegate EOS_Auth_GetSelectedAccountId;

	internal static EOS_Auth_IdToken_ReleaseDelegate EOS_Auth_IdToken_Release;

	internal static EOS_Auth_LinkAccountDelegate EOS_Auth_LinkAccount;

	internal static EOS_Auth_LoginDelegate EOS_Auth_Login;

	internal static EOS_Auth_LogoutDelegate EOS_Auth_Logout;

	internal static EOS_Auth_QueryIdTokenDelegate EOS_Auth_QueryIdToken;

	internal static EOS_Auth_RemoveNotifyLoginStatusChangedDelegate EOS_Auth_RemoveNotifyLoginStatusChanged;

	internal static EOS_Auth_Token_ReleaseDelegate EOS_Auth_Token_Release;

	internal static EOS_Auth_VerifyIdTokenDelegate EOS_Auth_VerifyIdToken;

	internal static EOS_Auth_VerifyUserAuthDelegate EOS_Auth_VerifyUserAuth;

	internal static EOS_ByteArray_ToStringDelegate EOS_ByteArray_ToString;

	internal static EOS_Connect_AddNotifyAuthExpirationDelegate EOS_Connect_AddNotifyAuthExpiration;

	internal static EOS_Connect_AddNotifyLoginStatusChangedDelegate EOS_Connect_AddNotifyLoginStatusChanged;

	internal static EOS_Connect_CopyIdTokenDelegate EOS_Connect_CopyIdToken;

	internal static EOS_Connect_CopyProductUserExternalAccountByAccountIdDelegate EOS_Connect_CopyProductUserExternalAccountByAccountId;

	internal static EOS_Connect_CopyProductUserExternalAccountByAccountTypeDelegate EOS_Connect_CopyProductUserExternalAccountByAccountType;

	internal static EOS_Connect_CopyProductUserExternalAccountByIndexDelegate EOS_Connect_CopyProductUserExternalAccountByIndex;

	internal static EOS_Connect_CopyProductUserInfoDelegate EOS_Connect_CopyProductUserInfo;

	internal static EOS_Connect_CreateDeviceIdDelegate EOS_Connect_CreateDeviceId;

	internal static EOS_Connect_CreateUserDelegate EOS_Connect_CreateUser;

	internal static EOS_Connect_DeleteDeviceIdDelegate EOS_Connect_DeleteDeviceId;

	internal static EOS_Connect_ExternalAccountInfo_ReleaseDelegate EOS_Connect_ExternalAccountInfo_Release;

	internal static EOS_Connect_GetExternalAccountMappingDelegate EOS_Connect_GetExternalAccountMapping;

	internal static EOS_Connect_GetLoggedInUserByIndexDelegate EOS_Connect_GetLoggedInUserByIndex;

	internal static EOS_Connect_GetLoggedInUsersCountDelegate EOS_Connect_GetLoggedInUsersCount;

	internal static EOS_Connect_GetLoginStatusDelegate EOS_Connect_GetLoginStatus;

	internal static EOS_Connect_GetProductUserExternalAccountCountDelegate EOS_Connect_GetProductUserExternalAccountCount;

	internal static EOS_Connect_GetProductUserIdMappingDelegate EOS_Connect_GetProductUserIdMapping;

	internal static EOS_Connect_IdToken_ReleaseDelegate EOS_Connect_IdToken_Release;

	internal static EOS_Connect_LinkAccountDelegate EOS_Connect_LinkAccount;

	internal static EOS_Connect_LoginDelegate EOS_Connect_Login;

	internal static EOS_Connect_QueryExternalAccountMappingsDelegate EOS_Connect_QueryExternalAccountMappings;

	internal static EOS_Connect_QueryProductUserIdMappingsDelegate EOS_Connect_QueryProductUserIdMappings;

	internal static EOS_Connect_RemoveNotifyAuthExpirationDelegate EOS_Connect_RemoveNotifyAuthExpiration;

	internal static EOS_Connect_RemoveNotifyLoginStatusChangedDelegate EOS_Connect_RemoveNotifyLoginStatusChanged;

	internal static EOS_Connect_TransferDeviceIdAccountDelegate EOS_Connect_TransferDeviceIdAccount;

	internal static EOS_Connect_UnlinkAccountDelegate EOS_Connect_UnlinkAccount;

	internal static EOS_Connect_VerifyIdTokenDelegate EOS_Connect_VerifyIdToken;

	internal static EOS_ContinuanceToken_ToStringDelegate EOS_ContinuanceToken_ToString;

	internal static EOS_CustomInvites_AcceptRequestToJoinDelegate EOS_CustomInvites_AcceptRequestToJoin;

	internal static EOS_CustomInvites_AddNotifyCustomInviteAcceptedDelegate EOS_CustomInvites_AddNotifyCustomInviteAccepted;

	internal static EOS_CustomInvites_AddNotifyCustomInviteReceivedDelegate EOS_CustomInvites_AddNotifyCustomInviteReceived;

	internal static EOS_CustomInvites_AddNotifyCustomInviteRejectedDelegate EOS_CustomInvites_AddNotifyCustomInviteRejected;

	internal static EOS_CustomInvites_AddNotifyRequestToJoinAcceptedDelegate EOS_CustomInvites_AddNotifyRequestToJoinAccepted;

	internal static EOS_CustomInvites_AddNotifyRequestToJoinReceivedDelegate EOS_CustomInvites_AddNotifyRequestToJoinReceived;

	internal static EOS_CustomInvites_AddNotifyRequestToJoinRejectedDelegate EOS_CustomInvites_AddNotifyRequestToJoinRejected;

	internal static EOS_CustomInvites_AddNotifyRequestToJoinResponseReceivedDelegate EOS_CustomInvites_AddNotifyRequestToJoinResponseReceived;

	internal static EOS_CustomInvites_AddNotifySendCustomNativeInviteRequestedDelegate EOS_CustomInvites_AddNotifySendCustomNativeInviteRequested;

	internal static EOS_CustomInvites_FinalizeInviteDelegate EOS_CustomInvites_FinalizeInvite;

	internal static EOS_CustomInvites_RejectRequestToJoinDelegate EOS_CustomInvites_RejectRequestToJoin;

	internal static EOS_CustomInvites_RemoveNotifyCustomInviteAcceptedDelegate EOS_CustomInvites_RemoveNotifyCustomInviteAccepted;

	internal static EOS_CustomInvites_RemoveNotifyCustomInviteReceivedDelegate EOS_CustomInvites_RemoveNotifyCustomInviteReceived;

	internal static EOS_CustomInvites_RemoveNotifyCustomInviteRejectedDelegate EOS_CustomInvites_RemoveNotifyCustomInviteRejected;

	internal static EOS_CustomInvites_RemoveNotifyRequestToJoinAcceptedDelegate EOS_CustomInvites_RemoveNotifyRequestToJoinAccepted;

	internal static EOS_CustomInvites_RemoveNotifyRequestToJoinReceivedDelegate EOS_CustomInvites_RemoveNotifyRequestToJoinReceived;

	internal static EOS_CustomInvites_RemoveNotifyRequestToJoinRejectedDelegate EOS_CustomInvites_RemoveNotifyRequestToJoinRejected;

	internal static EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceivedDelegate EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceived;

	internal static EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequestedDelegate EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequested;

	internal static EOS_CustomInvites_SendCustomInviteDelegate EOS_CustomInvites_SendCustomInvite;

	internal static EOS_CustomInvites_SendRequestToJoinDelegate EOS_CustomInvites_SendRequestToJoin;

	internal static EOS_CustomInvites_SetCustomInviteDelegate EOS_CustomInvites_SetCustomInvite;

	internal static EOS_EApplicationStatus_ToStringDelegate EOS_EApplicationStatus_ToString;

	internal static EOS_ENetworkStatus_ToStringDelegate EOS_ENetworkStatus_ToString;

	internal static EOS_EResult_IsOperationCompleteDelegate EOS_EResult_IsOperationComplete;

	internal static EOS_EResult_ToStringDelegate EOS_EResult_ToString;

	internal static EOS_Ecom_CatalogItem_ReleaseDelegate EOS_Ecom_CatalogItem_Release;

	internal static EOS_Ecom_CatalogOffer_ReleaseDelegate EOS_Ecom_CatalogOffer_Release;

	internal static EOS_Ecom_CatalogRelease_ReleaseDelegate EOS_Ecom_CatalogRelease_Release;

	internal static EOS_Ecom_CheckoutDelegate EOS_Ecom_Checkout;

	internal static EOS_Ecom_CopyEntitlementByIdDelegate EOS_Ecom_CopyEntitlementById;

	internal static EOS_Ecom_CopyEntitlementByIndexDelegate EOS_Ecom_CopyEntitlementByIndex;

	internal static EOS_Ecom_CopyEntitlementByNameAndIndexDelegate EOS_Ecom_CopyEntitlementByNameAndIndex;

	internal static EOS_Ecom_CopyItemByIdDelegate EOS_Ecom_CopyItemById;

	internal static EOS_Ecom_CopyItemImageInfoByIndexDelegate EOS_Ecom_CopyItemImageInfoByIndex;

	internal static EOS_Ecom_CopyItemReleaseByIndexDelegate EOS_Ecom_CopyItemReleaseByIndex;

	internal static EOS_Ecom_CopyLastRedeemedEntitlementByIndexDelegate EOS_Ecom_CopyLastRedeemedEntitlementByIndex;

	internal static EOS_Ecom_CopyOfferByIdDelegate EOS_Ecom_CopyOfferById;

	internal static EOS_Ecom_CopyOfferByIndexDelegate EOS_Ecom_CopyOfferByIndex;

	internal static EOS_Ecom_CopyOfferImageInfoByIndexDelegate EOS_Ecom_CopyOfferImageInfoByIndex;

	internal static EOS_Ecom_CopyOfferItemByIndexDelegate EOS_Ecom_CopyOfferItemByIndex;

	internal static EOS_Ecom_CopyTransactionByIdDelegate EOS_Ecom_CopyTransactionById;

	internal static EOS_Ecom_CopyTransactionByIndexDelegate EOS_Ecom_CopyTransactionByIndex;

	internal static EOS_Ecom_Entitlement_ReleaseDelegate EOS_Ecom_Entitlement_Release;

	internal static EOS_Ecom_GetEntitlementsByNameCountDelegate EOS_Ecom_GetEntitlementsByNameCount;

	internal static EOS_Ecom_GetEntitlementsCountDelegate EOS_Ecom_GetEntitlementsCount;

	internal static EOS_Ecom_GetItemImageInfoCountDelegate EOS_Ecom_GetItemImageInfoCount;

	internal static EOS_Ecom_GetItemReleaseCountDelegate EOS_Ecom_GetItemReleaseCount;

	internal static EOS_Ecom_GetLastRedeemedEntitlementsCountDelegate EOS_Ecom_GetLastRedeemedEntitlementsCount;

	internal static EOS_Ecom_GetOfferCountDelegate EOS_Ecom_GetOfferCount;

	internal static EOS_Ecom_GetOfferImageInfoCountDelegate EOS_Ecom_GetOfferImageInfoCount;

	internal static EOS_Ecom_GetOfferItemCountDelegate EOS_Ecom_GetOfferItemCount;

	internal static EOS_Ecom_GetTransactionCountDelegate EOS_Ecom_GetTransactionCount;

	internal static EOS_Ecom_KeyImageInfo_ReleaseDelegate EOS_Ecom_KeyImageInfo_Release;

	internal static EOS_Ecom_QueryEntitlementTokenDelegate EOS_Ecom_QueryEntitlementToken;

	internal static EOS_Ecom_QueryEntitlementsDelegate EOS_Ecom_QueryEntitlements;

	internal static EOS_Ecom_QueryOffersDelegate EOS_Ecom_QueryOffers;

	internal static EOS_Ecom_QueryOwnershipDelegate EOS_Ecom_QueryOwnership;

	internal static EOS_Ecom_QueryOwnershipBySandboxIdsDelegate EOS_Ecom_QueryOwnershipBySandboxIds;

	internal static EOS_Ecom_QueryOwnershipTokenDelegate EOS_Ecom_QueryOwnershipToken;

	internal static EOS_Ecom_RedeemEntitlementsDelegate EOS_Ecom_RedeemEntitlements;

	internal static EOS_Ecom_Transaction_CopyEntitlementByIndexDelegate EOS_Ecom_Transaction_CopyEntitlementByIndex;

	internal static EOS_Ecom_Transaction_GetEntitlementsCountDelegate EOS_Ecom_Transaction_GetEntitlementsCount;

	internal static EOS_Ecom_Transaction_GetTransactionIdDelegate EOS_Ecom_Transaction_GetTransactionId;

	internal static EOS_Ecom_Transaction_ReleaseDelegate EOS_Ecom_Transaction_Release;

	internal static EOS_EpicAccountId_FromStringDelegate EOS_EpicAccountId_FromString;

	internal static EOS_EpicAccountId_IsValidDelegate EOS_EpicAccountId_IsValid;

	internal static EOS_EpicAccountId_ToStringDelegate EOS_EpicAccountId_ToString;

	internal static EOS_Friends_AcceptInviteDelegate EOS_Friends_AcceptInvite;

	internal static EOS_Friends_AddNotifyBlockedUsersUpdateDelegate EOS_Friends_AddNotifyBlockedUsersUpdate;

	internal static EOS_Friends_AddNotifyFriendsUpdateDelegate EOS_Friends_AddNotifyFriendsUpdate;

	internal static EOS_Friends_GetBlockedUserAtIndexDelegate EOS_Friends_GetBlockedUserAtIndex;

	internal static EOS_Friends_GetBlockedUsersCountDelegate EOS_Friends_GetBlockedUsersCount;

	internal static EOS_Friends_GetFriendAtIndexDelegate EOS_Friends_GetFriendAtIndex;

	internal static EOS_Friends_GetFriendsCountDelegate EOS_Friends_GetFriendsCount;

	internal static EOS_Friends_GetStatusDelegate EOS_Friends_GetStatus;

	internal static EOS_Friends_QueryFriendsDelegate EOS_Friends_QueryFriends;

	internal static EOS_Friends_RejectInviteDelegate EOS_Friends_RejectInvite;

	internal static EOS_Friends_RemoveNotifyBlockedUsersUpdateDelegate EOS_Friends_RemoveNotifyBlockedUsersUpdate;

	internal static EOS_Friends_RemoveNotifyFriendsUpdateDelegate EOS_Friends_RemoveNotifyFriendsUpdate;

	internal static EOS_Friends_SendInviteDelegate EOS_Friends_SendInvite;

	internal static EOS_GetVersionDelegate EOS_GetVersion;

	internal static EOS_InitializeDelegate EOS_Initialize;

	internal static EOS_IntegratedPlatformOptionsContainer_AddDelegate EOS_IntegratedPlatformOptionsContainer_Add;

	internal static EOS_IntegratedPlatformOptionsContainer_ReleaseDelegate EOS_IntegratedPlatformOptionsContainer_Release;

	internal static EOS_IntegratedPlatform_AddNotifyUserLoginStatusChangedDelegate EOS_IntegratedPlatform_AddNotifyUserLoginStatusChanged;

	internal static EOS_IntegratedPlatform_ClearUserPreLogoutCallbackDelegate EOS_IntegratedPlatform_ClearUserPreLogoutCallback;

	internal static EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainerDelegate EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainer;

	internal static EOS_IntegratedPlatform_FinalizeDeferredUserLogoutDelegate EOS_IntegratedPlatform_FinalizeDeferredUserLogout;

	internal static EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChangedDelegate EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChanged;

	internal static EOS_IntegratedPlatform_SetUserLoginStatusDelegate EOS_IntegratedPlatform_SetUserLoginStatus;

	internal static EOS_IntegratedPlatform_SetUserPreLogoutCallbackDelegate EOS_IntegratedPlatform_SetUserPreLogoutCallback;

	internal static EOS_KWS_AddNotifyPermissionsUpdateReceivedDelegate EOS_KWS_AddNotifyPermissionsUpdateReceived;

	internal static EOS_KWS_CopyPermissionByIndexDelegate EOS_KWS_CopyPermissionByIndex;

	internal static EOS_KWS_CreateUserDelegate EOS_KWS_CreateUser;

	internal static EOS_KWS_GetPermissionByKeyDelegate EOS_KWS_GetPermissionByKey;

	internal static EOS_KWS_GetPermissionsCountDelegate EOS_KWS_GetPermissionsCount;

	internal static EOS_KWS_PermissionStatus_ReleaseDelegate EOS_KWS_PermissionStatus_Release;

	internal static EOS_KWS_QueryAgeGateDelegate EOS_KWS_QueryAgeGate;

	internal static EOS_KWS_QueryPermissionsDelegate EOS_KWS_QueryPermissions;

	internal static EOS_KWS_RemoveNotifyPermissionsUpdateReceivedDelegate EOS_KWS_RemoveNotifyPermissionsUpdateReceived;

	internal static EOS_KWS_RequestPermissionsDelegate EOS_KWS_RequestPermissions;

	internal static EOS_KWS_UpdateParentEmailDelegate EOS_KWS_UpdateParentEmail;

	internal static EOS_Leaderboards_CopyLeaderboardDefinitionByIndexDelegate EOS_Leaderboards_CopyLeaderboardDefinitionByIndex;

	internal static EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardIdDelegate EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardId;

	internal static EOS_Leaderboards_CopyLeaderboardRecordByIndexDelegate EOS_Leaderboards_CopyLeaderboardRecordByIndex;

	internal static EOS_Leaderboards_CopyLeaderboardRecordByUserIdDelegate EOS_Leaderboards_CopyLeaderboardRecordByUserId;

	internal static EOS_Leaderboards_CopyLeaderboardUserScoreByIndexDelegate EOS_Leaderboards_CopyLeaderboardUserScoreByIndex;

	internal static EOS_Leaderboards_CopyLeaderboardUserScoreByUserIdDelegate EOS_Leaderboards_CopyLeaderboardUserScoreByUserId;

	internal static EOS_Leaderboards_Definition_ReleaseDelegate EOS_Leaderboards_Definition_Release;

	internal static EOS_Leaderboards_GetLeaderboardDefinitionCountDelegate EOS_Leaderboards_GetLeaderboardDefinitionCount;

	internal static EOS_Leaderboards_GetLeaderboardRecordCountDelegate EOS_Leaderboards_GetLeaderboardRecordCount;

	internal static EOS_Leaderboards_GetLeaderboardUserScoreCountDelegate EOS_Leaderboards_GetLeaderboardUserScoreCount;

	internal static EOS_Leaderboards_LeaderboardDefinition_ReleaseDelegate EOS_Leaderboards_LeaderboardDefinition_Release;

	internal static EOS_Leaderboards_LeaderboardRecord_ReleaseDelegate EOS_Leaderboards_LeaderboardRecord_Release;

	internal static EOS_Leaderboards_LeaderboardUserScore_ReleaseDelegate EOS_Leaderboards_LeaderboardUserScore_Release;

	internal static EOS_Leaderboards_QueryLeaderboardDefinitionsDelegate EOS_Leaderboards_QueryLeaderboardDefinitions;

	internal static EOS_Leaderboards_QueryLeaderboardRanksDelegate EOS_Leaderboards_QueryLeaderboardRanks;

	internal static EOS_Leaderboards_QueryLeaderboardUserScoresDelegate EOS_Leaderboards_QueryLeaderboardUserScores;

	internal static EOS_LobbyDetails_CopyAttributeByIndexDelegate EOS_LobbyDetails_CopyAttributeByIndex;

	internal static EOS_LobbyDetails_CopyAttributeByKeyDelegate EOS_LobbyDetails_CopyAttributeByKey;

	internal static EOS_LobbyDetails_CopyInfoDelegate EOS_LobbyDetails_CopyInfo;

	internal static EOS_LobbyDetails_CopyMemberAttributeByIndexDelegate EOS_LobbyDetails_CopyMemberAttributeByIndex;

	internal static EOS_LobbyDetails_CopyMemberAttributeByKeyDelegate EOS_LobbyDetails_CopyMemberAttributeByKey;

	internal static EOS_LobbyDetails_CopyMemberInfoDelegate EOS_LobbyDetails_CopyMemberInfo;

	internal static EOS_LobbyDetails_GetAttributeCountDelegate EOS_LobbyDetails_GetAttributeCount;

	internal static EOS_LobbyDetails_GetLobbyOwnerDelegate EOS_LobbyDetails_GetLobbyOwner;

	internal static EOS_LobbyDetails_GetMemberAttributeCountDelegate EOS_LobbyDetails_GetMemberAttributeCount;

	internal static EOS_LobbyDetails_GetMemberByIndexDelegate EOS_LobbyDetails_GetMemberByIndex;

	internal static EOS_LobbyDetails_GetMemberCountDelegate EOS_LobbyDetails_GetMemberCount;

	internal static EOS_LobbyDetails_Info_ReleaseDelegate EOS_LobbyDetails_Info_Release;

	internal static EOS_LobbyDetails_MemberInfo_ReleaseDelegate EOS_LobbyDetails_MemberInfo_Release;

	internal static EOS_LobbyDetails_ReleaseDelegate EOS_LobbyDetails_Release;

	internal static EOS_LobbyModification_AddAttributeDelegate EOS_LobbyModification_AddAttribute;

	internal static EOS_LobbyModification_AddMemberAttributeDelegate EOS_LobbyModification_AddMemberAttribute;

	internal static EOS_LobbyModification_ReleaseDelegate EOS_LobbyModification_Release;

	internal static EOS_LobbyModification_RemoveAttributeDelegate EOS_LobbyModification_RemoveAttribute;

	internal static EOS_LobbyModification_RemoveMemberAttributeDelegate EOS_LobbyModification_RemoveMemberAttribute;

	internal static EOS_LobbyModification_SetAllowedPlatformIdsDelegate EOS_LobbyModification_SetAllowedPlatformIds;

	internal static EOS_LobbyModification_SetBucketIdDelegate EOS_LobbyModification_SetBucketId;

	internal static EOS_LobbyModification_SetInvitesAllowedDelegate EOS_LobbyModification_SetInvitesAllowed;

	internal static EOS_LobbyModification_SetMaxMembersDelegate EOS_LobbyModification_SetMaxMembers;

	internal static EOS_LobbyModification_SetPermissionLevelDelegate EOS_LobbyModification_SetPermissionLevel;

	internal static EOS_LobbySearch_CopySearchResultByIndexDelegate EOS_LobbySearch_CopySearchResultByIndex;

	internal static EOS_LobbySearch_FindDelegate EOS_LobbySearch_Find;

	internal static EOS_LobbySearch_GetSearchResultCountDelegate EOS_LobbySearch_GetSearchResultCount;

	internal static EOS_LobbySearch_ReleaseDelegate EOS_LobbySearch_Release;

	internal static EOS_LobbySearch_RemoveParameterDelegate EOS_LobbySearch_RemoveParameter;

	internal static EOS_LobbySearch_SetLobbyIdDelegate EOS_LobbySearch_SetLobbyId;

	internal static EOS_LobbySearch_SetMaxResultsDelegate EOS_LobbySearch_SetMaxResults;

	internal static EOS_LobbySearch_SetParameterDelegate EOS_LobbySearch_SetParameter;

	internal static EOS_LobbySearch_SetTargetUserIdDelegate EOS_LobbySearch_SetTargetUserId;

	internal static EOS_Lobby_AddNotifyJoinLobbyAcceptedDelegate EOS_Lobby_AddNotifyJoinLobbyAccepted;

	internal static EOS_Lobby_AddNotifyLeaveLobbyRequestedDelegate EOS_Lobby_AddNotifyLeaveLobbyRequested;

	internal static EOS_Lobby_AddNotifyLobbyInviteAcceptedDelegate EOS_Lobby_AddNotifyLobbyInviteAccepted;

	internal static EOS_Lobby_AddNotifyLobbyInviteReceivedDelegate EOS_Lobby_AddNotifyLobbyInviteReceived;

	internal static EOS_Lobby_AddNotifyLobbyInviteRejectedDelegate EOS_Lobby_AddNotifyLobbyInviteRejected;

	internal static EOS_Lobby_AddNotifyLobbyMemberStatusReceivedDelegate EOS_Lobby_AddNotifyLobbyMemberStatusReceived;

	internal static EOS_Lobby_AddNotifyLobbyMemberUpdateReceivedDelegate EOS_Lobby_AddNotifyLobbyMemberUpdateReceived;

	internal static EOS_Lobby_AddNotifyLobbyUpdateReceivedDelegate EOS_Lobby_AddNotifyLobbyUpdateReceived;

	internal static EOS_Lobby_AddNotifyRTCRoomConnectionChangedDelegate EOS_Lobby_AddNotifyRTCRoomConnectionChanged;

	internal static EOS_Lobby_AddNotifySendLobbyNativeInviteRequestedDelegate EOS_Lobby_AddNotifySendLobbyNativeInviteRequested;

	internal static EOS_Lobby_Attribute_ReleaseDelegate EOS_Lobby_Attribute_Release;

	internal static EOS_Lobby_CopyLobbyDetailsHandleDelegate EOS_Lobby_CopyLobbyDetailsHandle;

	internal static EOS_Lobby_CopyLobbyDetailsHandleByInviteIdDelegate EOS_Lobby_CopyLobbyDetailsHandleByInviteId;

	internal static EOS_Lobby_CopyLobbyDetailsHandleByUiEventIdDelegate EOS_Lobby_CopyLobbyDetailsHandleByUiEventId;

	internal static EOS_Lobby_CreateLobbyDelegate EOS_Lobby_CreateLobby;

	internal static EOS_Lobby_CreateLobbySearchDelegate EOS_Lobby_CreateLobbySearch;

	internal static EOS_Lobby_DestroyLobbyDelegate EOS_Lobby_DestroyLobby;

	internal static EOS_Lobby_GetConnectStringDelegate EOS_Lobby_GetConnectString;

	internal static EOS_Lobby_GetInviteCountDelegate EOS_Lobby_GetInviteCount;

	internal static EOS_Lobby_GetInviteIdByIndexDelegate EOS_Lobby_GetInviteIdByIndex;

	internal static EOS_Lobby_GetRTCRoomNameDelegate EOS_Lobby_GetRTCRoomName;

	internal static EOS_Lobby_HardMuteMemberDelegate EOS_Lobby_HardMuteMember;

	internal static EOS_Lobby_IsRTCRoomConnectedDelegate EOS_Lobby_IsRTCRoomConnected;

	internal static EOS_Lobby_JoinLobbyDelegate EOS_Lobby_JoinLobby;

	internal static EOS_Lobby_JoinLobbyByIdDelegate EOS_Lobby_JoinLobbyById;

	internal static EOS_Lobby_KickMemberDelegate EOS_Lobby_KickMember;

	internal static EOS_Lobby_LeaveLobbyDelegate EOS_Lobby_LeaveLobby;

	internal static EOS_Lobby_ParseConnectStringDelegate EOS_Lobby_ParseConnectString;

	internal static EOS_Lobby_PromoteMemberDelegate EOS_Lobby_PromoteMember;

	internal static EOS_Lobby_QueryInvitesDelegate EOS_Lobby_QueryInvites;

	internal static EOS_Lobby_RejectInviteDelegate EOS_Lobby_RejectInvite;

	internal static EOS_Lobby_RemoveNotifyJoinLobbyAcceptedDelegate EOS_Lobby_RemoveNotifyJoinLobbyAccepted;

	internal static EOS_Lobby_RemoveNotifyLeaveLobbyRequestedDelegate EOS_Lobby_RemoveNotifyLeaveLobbyRequested;

	internal static EOS_Lobby_RemoveNotifyLobbyInviteAcceptedDelegate EOS_Lobby_RemoveNotifyLobbyInviteAccepted;

	internal static EOS_Lobby_RemoveNotifyLobbyInviteReceivedDelegate EOS_Lobby_RemoveNotifyLobbyInviteReceived;

	internal static EOS_Lobby_RemoveNotifyLobbyInviteRejectedDelegate EOS_Lobby_RemoveNotifyLobbyInviteRejected;

	internal static EOS_Lobby_RemoveNotifyLobbyMemberStatusReceivedDelegate EOS_Lobby_RemoveNotifyLobbyMemberStatusReceived;

	internal static EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceivedDelegate EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceived;

	internal static EOS_Lobby_RemoveNotifyLobbyUpdateReceivedDelegate EOS_Lobby_RemoveNotifyLobbyUpdateReceived;

	internal static EOS_Lobby_RemoveNotifyRTCRoomConnectionChangedDelegate EOS_Lobby_RemoveNotifyRTCRoomConnectionChanged;

	internal static EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequestedDelegate EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequested;

	internal static EOS_Lobby_SendInviteDelegate EOS_Lobby_SendInvite;

	internal static EOS_Lobby_UpdateLobbyDelegate EOS_Lobby_UpdateLobby;

	internal static EOS_Lobby_UpdateLobbyModificationDelegate EOS_Lobby_UpdateLobbyModification;

	internal static EOS_Logging_SetCallbackDelegate EOS_Logging_SetCallback;

	internal static EOS_Logging_SetLogLevelDelegate EOS_Logging_SetLogLevel;

	internal static EOS_Metrics_BeginPlayerSessionDelegate EOS_Metrics_BeginPlayerSession;

	internal static EOS_Metrics_EndPlayerSessionDelegate EOS_Metrics_EndPlayerSession;

	internal static EOS_Mods_CopyModInfoDelegate EOS_Mods_CopyModInfo;

	internal static EOS_Mods_EnumerateModsDelegate EOS_Mods_EnumerateMods;

	internal static EOS_Mods_InstallModDelegate EOS_Mods_InstallMod;

	internal static EOS_Mods_ModInfo_ReleaseDelegate EOS_Mods_ModInfo_Release;

	internal static EOS_Mods_UninstallModDelegate EOS_Mods_UninstallMod;

	internal static EOS_Mods_UpdateModDelegate EOS_Mods_UpdateMod;

	internal static EOS_P2P_AcceptConnectionDelegate EOS_P2P_AcceptConnection;

	internal static EOS_P2P_AddNotifyIncomingPacketQueueFullDelegate EOS_P2P_AddNotifyIncomingPacketQueueFull;

	internal static EOS_P2P_AddNotifyPeerConnectionClosedDelegate EOS_P2P_AddNotifyPeerConnectionClosed;

	internal static EOS_P2P_AddNotifyPeerConnectionEstablishedDelegate EOS_P2P_AddNotifyPeerConnectionEstablished;

	internal static EOS_P2P_AddNotifyPeerConnectionInterruptedDelegate EOS_P2P_AddNotifyPeerConnectionInterrupted;

	internal static EOS_P2P_AddNotifyPeerConnectionRequestDelegate EOS_P2P_AddNotifyPeerConnectionRequest;

	internal static EOS_P2P_ClearPacketQueueDelegate EOS_P2P_ClearPacketQueue;

	internal static EOS_P2P_CloseConnectionDelegate EOS_P2P_CloseConnection;

	internal static EOS_P2P_CloseConnectionsDelegate EOS_P2P_CloseConnections;

	internal static EOS_P2P_GetNATTypeDelegate EOS_P2P_GetNATType;

	internal static EOS_P2P_GetNextReceivedPacketSizeDelegate EOS_P2P_GetNextReceivedPacketSize;

	internal static EOS_P2P_GetPacketQueueInfoDelegate EOS_P2P_GetPacketQueueInfo;

	internal static EOS_P2P_GetPortRangeDelegate EOS_P2P_GetPortRange;

	internal static EOS_P2P_GetRelayControlDelegate EOS_P2P_GetRelayControl;

	internal static EOS_P2P_QueryNATTypeDelegate EOS_P2P_QueryNATType;

	internal static EOS_P2P_ReceivePacketDelegate EOS_P2P_ReceivePacket;

	internal static EOS_P2P_RemoveNotifyIncomingPacketQueueFullDelegate EOS_P2P_RemoveNotifyIncomingPacketQueueFull;

	internal static EOS_P2P_RemoveNotifyPeerConnectionClosedDelegate EOS_P2P_RemoveNotifyPeerConnectionClosed;

	internal static EOS_P2P_RemoveNotifyPeerConnectionEstablishedDelegate EOS_P2P_RemoveNotifyPeerConnectionEstablished;

	internal static EOS_P2P_RemoveNotifyPeerConnectionInterruptedDelegate EOS_P2P_RemoveNotifyPeerConnectionInterrupted;

	internal static EOS_P2P_RemoveNotifyPeerConnectionRequestDelegate EOS_P2P_RemoveNotifyPeerConnectionRequest;

	internal static EOS_P2P_SendPacketDelegate EOS_P2P_SendPacket;

	internal static EOS_P2P_SetPacketQueueSizeDelegate EOS_P2P_SetPacketQueueSize;

	internal static EOS_P2P_SetPortRangeDelegate EOS_P2P_SetPortRange;

	internal static EOS_P2P_SetRelayControlDelegate EOS_P2P_SetRelayControl;

	internal static EOS_Platform_CheckForLauncherAndRestartDelegate EOS_Platform_CheckForLauncherAndRestart;

	internal static EOS_Platform_CreateDelegate EOS_Platform_Create;

	internal static EOS_Platform_GetAchievementsInterfaceDelegate EOS_Platform_GetAchievementsInterface;

	internal static EOS_Platform_GetActiveCountryCodeDelegate EOS_Platform_GetActiveCountryCode;

	internal static EOS_Platform_GetActiveLocaleCodeDelegate EOS_Platform_GetActiveLocaleCode;

	internal static EOS_Platform_GetAntiCheatClientInterfaceDelegate EOS_Platform_GetAntiCheatClientInterface;

	internal static EOS_Platform_GetAntiCheatServerInterfaceDelegate EOS_Platform_GetAntiCheatServerInterface;

	internal static EOS_Platform_GetApplicationStatusDelegate EOS_Platform_GetApplicationStatus;

	internal static EOS_Platform_GetAuthInterfaceDelegate EOS_Platform_GetAuthInterface;

	internal static EOS_Platform_GetConnectInterfaceDelegate EOS_Platform_GetConnectInterface;

	internal static EOS_Platform_GetCustomInvitesInterfaceDelegate EOS_Platform_GetCustomInvitesInterface;

	internal static EOS_Platform_GetDesktopCrossplayStatusDelegate EOS_Platform_GetDesktopCrossplayStatus;

	internal static EOS_Platform_GetEcomInterfaceDelegate EOS_Platform_GetEcomInterface;

	internal static EOS_Platform_GetFriendsInterfaceDelegate EOS_Platform_GetFriendsInterface;

	internal static EOS_Platform_GetIntegratedPlatformInterfaceDelegate EOS_Platform_GetIntegratedPlatformInterface;

	internal static EOS_Platform_GetKWSInterfaceDelegate EOS_Platform_GetKWSInterface;

	internal static EOS_Platform_GetLeaderboardsInterfaceDelegate EOS_Platform_GetLeaderboardsInterface;

	internal static EOS_Platform_GetLobbyInterfaceDelegate EOS_Platform_GetLobbyInterface;

	internal static EOS_Platform_GetMetricsInterfaceDelegate EOS_Platform_GetMetricsInterface;

	internal static EOS_Platform_GetModsInterfaceDelegate EOS_Platform_GetModsInterface;

	internal static EOS_Platform_GetNetworkStatusDelegate EOS_Platform_GetNetworkStatus;

	internal static EOS_Platform_GetOverrideCountryCodeDelegate EOS_Platform_GetOverrideCountryCode;

	internal static EOS_Platform_GetOverrideLocaleCodeDelegate EOS_Platform_GetOverrideLocaleCode;

	internal static EOS_Platform_GetP2PInterfaceDelegate EOS_Platform_GetP2PInterface;

	internal static EOS_Platform_GetPlayerDataStorageInterfaceDelegate EOS_Platform_GetPlayerDataStorageInterface;

	internal static EOS_Platform_GetPresenceInterfaceDelegate EOS_Platform_GetPresenceInterface;

	internal static EOS_Platform_GetProgressionSnapshotInterfaceDelegate EOS_Platform_GetProgressionSnapshotInterface;

	internal static EOS_Platform_GetRTCAdminInterfaceDelegate EOS_Platform_GetRTCAdminInterface;

	internal static EOS_Platform_GetRTCInterfaceDelegate EOS_Platform_GetRTCInterface;

	internal static EOS_Platform_GetReportsInterfaceDelegate EOS_Platform_GetReportsInterface;

	internal static EOS_Platform_GetSanctionsInterfaceDelegate EOS_Platform_GetSanctionsInterface;

	internal static EOS_Platform_GetSessionsInterfaceDelegate EOS_Platform_GetSessionsInterface;

	internal static EOS_Platform_GetStatsInterfaceDelegate EOS_Platform_GetStatsInterface;

	internal static EOS_Platform_GetTitleStorageInterfaceDelegate EOS_Platform_GetTitleStorageInterface;

	internal static EOS_Platform_GetUIInterfaceDelegate EOS_Platform_GetUIInterface;

	internal static EOS_Platform_GetUserInfoInterfaceDelegate EOS_Platform_GetUserInfoInterface;

	internal static EOS_Platform_ReleaseDelegate EOS_Platform_Release;

	internal static EOS_Platform_SetApplicationStatusDelegate EOS_Platform_SetApplicationStatus;

	internal static EOS_Platform_SetNetworkStatusDelegate EOS_Platform_SetNetworkStatus;

	internal static EOS_Platform_SetOverrideCountryCodeDelegate EOS_Platform_SetOverrideCountryCode;

	internal static EOS_Platform_SetOverrideLocaleCodeDelegate EOS_Platform_SetOverrideLocaleCode;

	internal static EOS_Platform_TickDelegate EOS_Platform_Tick;

	internal static EOS_PlayerDataStorageFileTransferRequest_CancelRequestDelegate EOS_PlayerDataStorageFileTransferRequest_CancelRequest;

	internal static EOS_PlayerDataStorageFileTransferRequest_GetFileRequestStateDelegate EOS_PlayerDataStorageFileTransferRequest_GetFileRequestState;

	internal static EOS_PlayerDataStorageFileTransferRequest_GetFilenameDelegate EOS_PlayerDataStorageFileTransferRequest_GetFilename;

	internal static EOS_PlayerDataStorageFileTransferRequest_ReleaseDelegate EOS_PlayerDataStorageFileTransferRequest_Release;

	internal static EOS_PlayerDataStorage_CopyFileMetadataAtIndexDelegate EOS_PlayerDataStorage_CopyFileMetadataAtIndex;

	internal static EOS_PlayerDataStorage_CopyFileMetadataByFilenameDelegate EOS_PlayerDataStorage_CopyFileMetadataByFilename;

	internal static EOS_PlayerDataStorage_DeleteCacheDelegate EOS_PlayerDataStorage_DeleteCache;

	internal static EOS_PlayerDataStorage_DeleteFileDelegate EOS_PlayerDataStorage_DeleteFile;

	internal static EOS_PlayerDataStorage_DuplicateFileDelegate EOS_PlayerDataStorage_DuplicateFile;

	internal static EOS_PlayerDataStorage_FileMetadata_ReleaseDelegate EOS_PlayerDataStorage_FileMetadata_Release;

	internal static EOS_PlayerDataStorage_GetFileMetadataCountDelegate EOS_PlayerDataStorage_GetFileMetadataCount;

	internal static EOS_PlayerDataStorage_QueryFileDelegate EOS_PlayerDataStorage_QueryFile;

	internal static EOS_PlayerDataStorage_QueryFileListDelegate EOS_PlayerDataStorage_QueryFileList;

	internal static EOS_PlayerDataStorage_ReadFileDelegate EOS_PlayerDataStorage_ReadFile;

	internal static EOS_PlayerDataStorage_WriteFileDelegate EOS_PlayerDataStorage_WriteFile;

	internal static EOS_PresenceModification_DeleteDataDelegate EOS_PresenceModification_DeleteData;

	internal static EOS_PresenceModification_ReleaseDelegate EOS_PresenceModification_Release;

	internal static EOS_PresenceModification_SetDataDelegate EOS_PresenceModification_SetData;

	internal static EOS_PresenceModification_SetJoinInfoDelegate EOS_PresenceModification_SetJoinInfo;

	internal static EOS_PresenceModification_SetRawRichTextDelegate EOS_PresenceModification_SetRawRichText;

	internal static EOS_PresenceModification_SetStatusDelegate EOS_PresenceModification_SetStatus;

	internal static EOS_Presence_AddNotifyJoinGameAcceptedDelegate EOS_Presence_AddNotifyJoinGameAccepted;

	internal static EOS_Presence_AddNotifyOnPresenceChangedDelegate EOS_Presence_AddNotifyOnPresenceChanged;

	internal static EOS_Presence_CopyPresenceDelegate EOS_Presence_CopyPresence;

	internal static EOS_Presence_CreatePresenceModificationDelegate EOS_Presence_CreatePresenceModification;

	internal static EOS_Presence_GetJoinInfoDelegate EOS_Presence_GetJoinInfo;

	internal static EOS_Presence_HasPresenceDelegate EOS_Presence_HasPresence;

	internal static EOS_Presence_Info_ReleaseDelegate EOS_Presence_Info_Release;

	internal static EOS_Presence_QueryPresenceDelegate EOS_Presence_QueryPresence;

	internal static EOS_Presence_RemoveNotifyJoinGameAcceptedDelegate EOS_Presence_RemoveNotifyJoinGameAccepted;

	internal static EOS_Presence_RemoveNotifyOnPresenceChangedDelegate EOS_Presence_RemoveNotifyOnPresenceChanged;

	internal static EOS_Presence_SetPresenceDelegate EOS_Presence_SetPresence;

	internal static EOS_ProductUserId_FromStringDelegate EOS_ProductUserId_FromString;

	internal static EOS_ProductUserId_IsValidDelegate EOS_ProductUserId_IsValid;

	internal static EOS_ProductUserId_ToStringDelegate EOS_ProductUserId_ToString;

	internal static EOS_ProgressionSnapshot_AddProgressionDelegate EOS_ProgressionSnapshot_AddProgression;

	internal static EOS_ProgressionSnapshot_BeginSnapshotDelegate EOS_ProgressionSnapshot_BeginSnapshot;

	internal static EOS_ProgressionSnapshot_DeleteSnapshotDelegate EOS_ProgressionSnapshot_DeleteSnapshot;

	internal static EOS_ProgressionSnapshot_EndSnapshotDelegate EOS_ProgressionSnapshot_EndSnapshot;

	internal static EOS_ProgressionSnapshot_SubmitSnapshotDelegate EOS_ProgressionSnapshot_SubmitSnapshot;

	internal static EOS_RTCAdmin_CopyUserTokenByIndexDelegate EOS_RTCAdmin_CopyUserTokenByIndex;

	internal static EOS_RTCAdmin_CopyUserTokenByUserIdDelegate EOS_RTCAdmin_CopyUserTokenByUserId;

	internal static EOS_RTCAdmin_KickDelegate EOS_RTCAdmin_Kick;

	internal static EOS_RTCAdmin_QueryJoinRoomTokenDelegate EOS_RTCAdmin_QueryJoinRoomToken;

	internal static EOS_RTCAdmin_SetParticipantHardMuteDelegate EOS_RTCAdmin_SetParticipantHardMute;

	internal static EOS_RTCAdmin_UserToken_ReleaseDelegate EOS_RTCAdmin_UserToken_Release;

	internal static EOS_RTCAudio_AddNotifyAudioBeforeRenderDelegate EOS_RTCAudio_AddNotifyAudioBeforeRender;

	internal static EOS_RTCAudio_AddNotifyAudioBeforeSendDelegate EOS_RTCAudio_AddNotifyAudioBeforeSend;

	internal static EOS_RTCAudio_AddNotifyAudioDevicesChangedDelegate EOS_RTCAudio_AddNotifyAudioDevicesChanged;

	internal static EOS_RTCAudio_AddNotifyAudioInputStateDelegate EOS_RTCAudio_AddNotifyAudioInputState;

	internal static EOS_RTCAudio_AddNotifyAudioOutputStateDelegate EOS_RTCAudio_AddNotifyAudioOutputState;

	internal static EOS_RTCAudio_AddNotifyParticipantUpdatedDelegate EOS_RTCAudio_AddNotifyParticipantUpdated;

	internal static EOS_RTCAudio_CopyInputDeviceInformationByIndexDelegate EOS_RTCAudio_CopyInputDeviceInformationByIndex;

	internal static EOS_RTCAudio_CopyOutputDeviceInformationByIndexDelegate EOS_RTCAudio_CopyOutputDeviceInformationByIndex;

	internal static EOS_RTCAudio_GetAudioInputDeviceByIndexDelegate EOS_RTCAudio_GetAudioInputDeviceByIndex;

	internal static EOS_RTCAudio_GetAudioInputDevicesCountDelegate EOS_RTCAudio_GetAudioInputDevicesCount;

	internal static EOS_RTCAudio_GetAudioOutputDeviceByIndexDelegate EOS_RTCAudio_GetAudioOutputDeviceByIndex;

	internal static EOS_RTCAudio_GetAudioOutputDevicesCountDelegate EOS_RTCAudio_GetAudioOutputDevicesCount;

	internal static EOS_RTCAudio_GetInputDevicesCountDelegate EOS_RTCAudio_GetInputDevicesCount;

	internal static EOS_RTCAudio_GetOutputDevicesCountDelegate EOS_RTCAudio_GetOutputDevicesCount;

	internal static EOS_RTCAudio_InputDeviceInformation_ReleaseDelegate EOS_RTCAudio_InputDeviceInformation_Release;

	internal static EOS_RTCAudio_OutputDeviceInformation_ReleaseDelegate EOS_RTCAudio_OutputDeviceInformation_Release;

	internal static EOS_RTCAudio_QueryInputDevicesInformationDelegate EOS_RTCAudio_QueryInputDevicesInformation;

	internal static EOS_RTCAudio_QueryOutputDevicesInformationDelegate EOS_RTCAudio_QueryOutputDevicesInformation;

	internal static EOS_RTCAudio_RegisterPlatformAudioUserDelegate EOS_RTCAudio_RegisterPlatformAudioUser;

	internal static EOS_RTCAudio_RegisterPlatformUserDelegate EOS_RTCAudio_RegisterPlatformUser;

	internal static EOS_RTCAudio_RemoveNotifyAudioBeforeRenderDelegate EOS_RTCAudio_RemoveNotifyAudioBeforeRender;

	internal static EOS_RTCAudio_RemoveNotifyAudioBeforeSendDelegate EOS_RTCAudio_RemoveNotifyAudioBeforeSend;

	internal static EOS_RTCAudio_RemoveNotifyAudioDevicesChangedDelegate EOS_RTCAudio_RemoveNotifyAudioDevicesChanged;

	internal static EOS_RTCAudio_RemoveNotifyAudioInputStateDelegate EOS_RTCAudio_RemoveNotifyAudioInputState;

	internal static EOS_RTCAudio_RemoveNotifyAudioOutputStateDelegate EOS_RTCAudio_RemoveNotifyAudioOutputState;

	internal static EOS_RTCAudio_RemoveNotifyParticipantUpdatedDelegate EOS_RTCAudio_RemoveNotifyParticipantUpdated;

	internal static EOS_RTCAudio_SendAudioDelegate EOS_RTCAudio_SendAudio;

	internal static EOS_RTCAudio_SetAudioInputSettingsDelegate EOS_RTCAudio_SetAudioInputSettings;

	internal static EOS_RTCAudio_SetAudioOutputSettingsDelegate EOS_RTCAudio_SetAudioOutputSettings;

	internal static EOS_RTCAudio_SetInputDeviceSettingsDelegate EOS_RTCAudio_SetInputDeviceSettings;

	internal static EOS_RTCAudio_SetOutputDeviceSettingsDelegate EOS_RTCAudio_SetOutputDeviceSettings;

	internal static EOS_RTCAudio_UnregisterPlatformAudioUserDelegate EOS_RTCAudio_UnregisterPlatformAudioUser;

	internal static EOS_RTCAudio_UnregisterPlatformUserDelegate EOS_RTCAudio_UnregisterPlatformUser;

	internal static EOS_RTCAudio_UpdateParticipantVolumeDelegate EOS_RTCAudio_UpdateParticipantVolume;

	internal static EOS_RTCAudio_UpdateReceivingDelegate EOS_RTCAudio_UpdateReceiving;

	internal static EOS_RTCAudio_UpdateReceivingVolumeDelegate EOS_RTCAudio_UpdateReceivingVolume;

	internal static EOS_RTCAudio_UpdateSendingDelegate EOS_RTCAudio_UpdateSending;

	internal static EOS_RTCAudio_UpdateSendingVolumeDelegate EOS_RTCAudio_UpdateSendingVolume;

	internal static EOS_RTC_AddNotifyDisconnectedDelegate EOS_RTC_AddNotifyDisconnected;

	internal static EOS_RTC_AddNotifyParticipantStatusChangedDelegate EOS_RTC_AddNotifyParticipantStatusChanged;

	internal static EOS_RTC_AddNotifyRoomStatisticsUpdatedDelegate EOS_RTC_AddNotifyRoomStatisticsUpdated;

	internal static EOS_RTC_BlockParticipantDelegate EOS_RTC_BlockParticipant;

	internal static EOS_RTC_GetAudioInterfaceDelegate EOS_RTC_GetAudioInterface;

	internal static EOS_RTC_JoinRoomDelegate EOS_RTC_JoinRoom;

	internal static EOS_RTC_LeaveRoomDelegate EOS_RTC_LeaveRoom;

	internal static EOS_RTC_RemoveNotifyDisconnectedDelegate EOS_RTC_RemoveNotifyDisconnected;

	internal static EOS_RTC_RemoveNotifyParticipantStatusChangedDelegate EOS_RTC_RemoveNotifyParticipantStatusChanged;

	internal static EOS_RTC_RemoveNotifyRoomStatisticsUpdatedDelegate EOS_RTC_RemoveNotifyRoomStatisticsUpdated;

	internal static EOS_RTC_SetRoomSettingDelegate EOS_RTC_SetRoomSetting;

	internal static EOS_RTC_SetSettingDelegate EOS_RTC_SetSetting;

	internal static EOS_Reports_SendPlayerBehaviorReportDelegate EOS_Reports_SendPlayerBehaviorReport;

	internal static EOS_Sanctions_CopyPlayerSanctionByIndexDelegate EOS_Sanctions_CopyPlayerSanctionByIndex;

	internal static EOS_Sanctions_GetPlayerSanctionCountDelegate EOS_Sanctions_GetPlayerSanctionCount;

	internal static EOS_Sanctions_PlayerSanction_ReleaseDelegate EOS_Sanctions_PlayerSanction_Release;

	internal static EOS_Sanctions_QueryActivePlayerSanctionsDelegate EOS_Sanctions_QueryActivePlayerSanctions;

	internal static EOS_SessionDetails_Attribute_ReleaseDelegate EOS_SessionDetails_Attribute_Release;

	internal static EOS_SessionDetails_CopyInfoDelegate EOS_SessionDetails_CopyInfo;

	internal static EOS_SessionDetails_CopySessionAttributeByIndexDelegate EOS_SessionDetails_CopySessionAttributeByIndex;

	internal static EOS_SessionDetails_CopySessionAttributeByKeyDelegate EOS_SessionDetails_CopySessionAttributeByKey;

	internal static EOS_SessionDetails_GetSessionAttributeCountDelegate EOS_SessionDetails_GetSessionAttributeCount;

	internal static EOS_SessionDetails_Info_ReleaseDelegate EOS_SessionDetails_Info_Release;

	internal static EOS_SessionDetails_ReleaseDelegate EOS_SessionDetails_Release;

	internal static EOS_SessionModification_AddAttributeDelegate EOS_SessionModification_AddAttribute;

	internal static EOS_SessionModification_ReleaseDelegate EOS_SessionModification_Release;

	internal static EOS_SessionModification_RemoveAttributeDelegate EOS_SessionModification_RemoveAttribute;

	internal static EOS_SessionModification_SetAllowedPlatformIdsDelegate EOS_SessionModification_SetAllowedPlatformIds;

	internal static EOS_SessionModification_SetBucketIdDelegate EOS_SessionModification_SetBucketId;

	internal static EOS_SessionModification_SetHostAddressDelegate EOS_SessionModification_SetHostAddress;

	internal static EOS_SessionModification_SetInvitesAllowedDelegate EOS_SessionModification_SetInvitesAllowed;

	internal static EOS_SessionModification_SetJoinInProgressAllowedDelegate EOS_SessionModification_SetJoinInProgressAllowed;

	internal static EOS_SessionModification_SetMaxPlayersDelegate EOS_SessionModification_SetMaxPlayers;

	internal static EOS_SessionModification_SetPermissionLevelDelegate EOS_SessionModification_SetPermissionLevel;

	internal static EOS_SessionSearch_CopySearchResultByIndexDelegate EOS_SessionSearch_CopySearchResultByIndex;

	internal static EOS_SessionSearch_FindDelegate EOS_SessionSearch_Find;

	internal static EOS_SessionSearch_GetSearchResultCountDelegate EOS_SessionSearch_GetSearchResultCount;

	internal static EOS_SessionSearch_ReleaseDelegate EOS_SessionSearch_Release;

	internal static EOS_SessionSearch_RemoveParameterDelegate EOS_SessionSearch_RemoveParameter;

	internal static EOS_SessionSearch_SetMaxResultsDelegate EOS_SessionSearch_SetMaxResults;

	internal static EOS_SessionSearch_SetParameterDelegate EOS_SessionSearch_SetParameter;

	internal static EOS_SessionSearch_SetSessionIdDelegate EOS_SessionSearch_SetSessionId;

	internal static EOS_SessionSearch_SetTargetUserIdDelegate EOS_SessionSearch_SetTargetUserId;

	internal static EOS_Sessions_AddNotifyJoinSessionAcceptedDelegate EOS_Sessions_AddNotifyJoinSessionAccepted;

	internal static EOS_Sessions_AddNotifyLeaveSessionRequestedDelegate EOS_Sessions_AddNotifyLeaveSessionRequested;

	internal static EOS_Sessions_AddNotifySendSessionNativeInviteRequestedDelegate EOS_Sessions_AddNotifySendSessionNativeInviteRequested;

	internal static EOS_Sessions_AddNotifySessionInviteAcceptedDelegate EOS_Sessions_AddNotifySessionInviteAccepted;

	internal static EOS_Sessions_AddNotifySessionInviteReceivedDelegate EOS_Sessions_AddNotifySessionInviteReceived;

	internal static EOS_Sessions_AddNotifySessionInviteRejectedDelegate EOS_Sessions_AddNotifySessionInviteRejected;

	internal static EOS_Sessions_CopyActiveSessionHandleDelegate EOS_Sessions_CopyActiveSessionHandle;

	internal static EOS_Sessions_CopySessionHandleByInviteIdDelegate EOS_Sessions_CopySessionHandleByInviteId;

	internal static EOS_Sessions_CopySessionHandleByUiEventIdDelegate EOS_Sessions_CopySessionHandleByUiEventId;

	internal static EOS_Sessions_CopySessionHandleForPresenceDelegate EOS_Sessions_CopySessionHandleForPresence;

	internal static EOS_Sessions_CreateSessionModificationDelegate EOS_Sessions_CreateSessionModification;

	internal static EOS_Sessions_CreateSessionSearchDelegate EOS_Sessions_CreateSessionSearch;

	internal static EOS_Sessions_DestroySessionDelegate EOS_Sessions_DestroySession;

	internal static EOS_Sessions_DumpSessionStateDelegate EOS_Sessions_DumpSessionState;

	internal static EOS_Sessions_EndSessionDelegate EOS_Sessions_EndSession;

	internal static EOS_Sessions_GetInviteCountDelegate EOS_Sessions_GetInviteCount;

	internal static EOS_Sessions_GetInviteIdByIndexDelegate EOS_Sessions_GetInviteIdByIndex;

	internal static EOS_Sessions_IsUserInSessionDelegate EOS_Sessions_IsUserInSession;

	internal static EOS_Sessions_JoinSessionDelegate EOS_Sessions_JoinSession;

	internal static EOS_Sessions_QueryInvitesDelegate EOS_Sessions_QueryInvites;

	internal static EOS_Sessions_RegisterPlayersDelegate EOS_Sessions_RegisterPlayers;

	internal static EOS_Sessions_RejectInviteDelegate EOS_Sessions_RejectInvite;

	internal static EOS_Sessions_RemoveNotifyJoinSessionAcceptedDelegate EOS_Sessions_RemoveNotifyJoinSessionAccepted;

	internal static EOS_Sessions_RemoveNotifyLeaveSessionRequestedDelegate EOS_Sessions_RemoveNotifyLeaveSessionRequested;

	internal static EOS_Sessions_RemoveNotifySendSessionNativeInviteRequestedDelegate EOS_Sessions_RemoveNotifySendSessionNativeInviteRequested;

	internal static EOS_Sessions_RemoveNotifySessionInviteAcceptedDelegate EOS_Sessions_RemoveNotifySessionInviteAccepted;

	internal static EOS_Sessions_RemoveNotifySessionInviteReceivedDelegate EOS_Sessions_RemoveNotifySessionInviteReceived;

	internal static EOS_Sessions_RemoveNotifySessionInviteRejectedDelegate EOS_Sessions_RemoveNotifySessionInviteRejected;

	internal static EOS_Sessions_SendInviteDelegate EOS_Sessions_SendInvite;

	internal static EOS_Sessions_StartSessionDelegate EOS_Sessions_StartSession;

	internal static EOS_Sessions_UnregisterPlayersDelegate EOS_Sessions_UnregisterPlayers;

	internal static EOS_Sessions_UpdateSessionDelegate EOS_Sessions_UpdateSession;

	internal static EOS_Sessions_UpdateSessionModificationDelegate EOS_Sessions_UpdateSessionModification;

	internal static EOS_ShutdownDelegate EOS_Shutdown;

	internal static EOS_Stats_CopyStatByIndexDelegate EOS_Stats_CopyStatByIndex;

	internal static EOS_Stats_CopyStatByNameDelegate EOS_Stats_CopyStatByName;

	internal static EOS_Stats_GetStatsCountDelegate EOS_Stats_GetStatsCount;

	internal static EOS_Stats_IngestStatDelegate EOS_Stats_IngestStat;

	internal static EOS_Stats_QueryStatsDelegate EOS_Stats_QueryStats;

	internal static EOS_Stats_Stat_ReleaseDelegate EOS_Stats_Stat_Release;

	internal static EOS_TitleStorageFileTransferRequest_CancelRequestDelegate EOS_TitleStorageFileTransferRequest_CancelRequest;

	internal static EOS_TitleStorageFileTransferRequest_GetFileRequestStateDelegate EOS_TitleStorageFileTransferRequest_GetFileRequestState;

	internal static EOS_TitleStorageFileTransferRequest_GetFilenameDelegate EOS_TitleStorageFileTransferRequest_GetFilename;

	internal static EOS_TitleStorageFileTransferRequest_ReleaseDelegate EOS_TitleStorageFileTransferRequest_Release;

	internal static EOS_TitleStorage_CopyFileMetadataAtIndexDelegate EOS_TitleStorage_CopyFileMetadataAtIndex;

	internal static EOS_TitleStorage_CopyFileMetadataByFilenameDelegate EOS_TitleStorage_CopyFileMetadataByFilename;

	internal static EOS_TitleStorage_DeleteCacheDelegate EOS_TitleStorage_DeleteCache;

	internal static EOS_TitleStorage_FileMetadata_ReleaseDelegate EOS_TitleStorage_FileMetadata_Release;

	internal static EOS_TitleStorage_GetFileMetadataCountDelegate EOS_TitleStorage_GetFileMetadataCount;

	internal static EOS_TitleStorage_QueryFileDelegate EOS_TitleStorage_QueryFile;

	internal static EOS_TitleStorage_QueryFileListDelegate EOS_TitleStorage_QueryFileList;

	internal static EOS_TitleStorage_ReadFileDelegate EOS_TitleStorage_ReadFile;

	internal static EOS_UI_AcknowledgeEventIdDelegate EOS_UI_AcknowledgeEventId;

	internal static EOS_UI_AddNotifyDisplaySettingsUpdatedDelegate EOS_UI_AddNotifyDisplaySettingsUpdated;

	internal static EOS_UI_AddNotifyMemoryMonitorDelegate EOS_UI_AddNotifyMemoryMonitor;

	internal static EOS_UI_GetFriendsExclusiveInputDelegate EOS_UI_GetFriendsExclusiveInput;

	internal static EOS_UI_GetFriendsVisibleDelegate EOS_UI_GetFriendsVisible;

	internal static EOS_UI_GetNotificationLocationPreferenceDelegate EOS_UI_GetNotificationLocationPreference;

	internal static EOS_UI_GetToggleFriendsButtonDelegate EOS_UI_GetToggleFriendsButton;

	internal static EOS_UI_GetToggleFriendsKeyDelegate EOS_UI_GetToggleFriendsKey;

	internal static EOS_UI_HideFriendsDelegate EOS_UI_HideFriends;

	internal static EOS_UI_IsSocialOverlayPausedDelegate EOS_UI_IsSocialOverlayPaused;

	internal static EOS_UI_IsValidButtonCombinationDelegate EOS_UI_IsValidButtonCombination;

	internal static EOS_UI_IsValidKeyCombinationDelegate EOS_UI_IsValidKeyCombination;

	internal static EOS_UI_PauseSocialOverlayDelegate EOS_UI_PauseSocialOverlay;

	internal static EOS_UI_PrePresentDelegate EOS_UI_PrePresent;

	internal static EOS_UI_RemoveNotifyDisplaySettingsUpdatedDelegate EOS_UI_RemoveNotifyDisplaySettingsUpdated;

	internal static EOS_UI_RemoveNotifyMemoryMonitorDelegate EOS_UI_RemoveNotifyMemoryMonitor;

	internal static EOS_UI_ReportInputStateDelegate EOS_UI_ReportInputState;

	internal static EOS_UI_SetDisplayPreferenceDelegate EOS_UI_SetDisplayPreference;

	internal static EOS_UI_SetToggleFriendsButtonDelegate EOS_UI_SetToggleFriendsButton;

	internal static EOS_UI_SetToggleFriendsKeyDelegate EOS_UI_SetToggleFriendsKey;

	internal static EOS_UI_ShowBlockPlayerDelegate EOS_UI_ShowBlockPlayer;

	internal static EOS_UI_ShowFriendsDelegate EOS_UI_ShowFriends;

	internal static EOS_UI_ShowNativeProfileDelegate EOS_UI_ShowNativeProfile;

	internal static EOS_UI_ShowReportPlayerDelegate EOS_UI_ShowReportPlayer;

	internal static EOS_UserInfo_BestDisplayName_ReleaseDelegate EOS_UserInfo_BestDisplayName_Release;

	internal static EOS_UserInfo_CopyBestDisplayNameDelegate EOS_UserInfo_CopyBestDisplayName;

	internal static EOS_UserInfo_CopyBestDisplayNameWithPlatformDelegate EOS_UserInfo_CopyBestDisplayNameWithPlatform;

	internal static EOS_UserInfo_CopyExternalUserInfoByAccountIdDelegate EOS_UserInfo_CopyExternalUserInfoByAccountId;

	internal static EOS_UserInfo_CopyExternalUserInfoByAccountTypeDelegate EOS_UserInfo_CopyExternalUserInfoByAccountType;

	internal static EOS_UserInfo_CopyExternalUserInfoByIndexDelegate EOS_UserInfo_CopyExternalUserInfoByIndex;

	internal static EOS_UserInfo_CopyUserInfoDelegate EOS_UserInfo_CopyUserInfo;

	internal static EOS_UserInfo_ExternalUserInfo_ReleaseDelegate EOS_UserInfo_ExternalUserInfo_Release;

	internal static EOS_UserInfo_GetExternalUserInfoCountDelegate EOS_UserInfo_GetExternalUserInfoCount;

	internal static EOS_UserInfo_GetLocalPlatformTypeDelegate EOS_UserInfo_GetLocalPlatformType;

	internal static EOS_UserInfo_QueryUserInfoDelegate EOS_UserInfo_QueryUserInfo;

	internal static EOS_UserInfo_QueryUserInfoByDisplayNameDelegate EOS_UserInfo_QueryUserInfoByDisplayName;

	internal static EOS_UserInfo_QueryUserInfoByExternalAccountDelegate EOS_UserInfo_QueryUserInfoByExternalAccount;

	internal static EOS_UserInfo_ReleaseDelegate EOS_UserInfo_Release;

	public static void Hook<TLibraryHandle>(TLibraryHandle libraryHandle, Func<TLibraryHandle, string, IntPtr> getFunctionPointer)
	{
		IntPtr intPtr = getFunctionPointer(libraryHandle, "EOS_Achievements_AddNotifyAchievementsUnlocked");
		if (intPtr == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_AddNotifyAchievementsUnlocked");
		}
		EOS_Achievements_AddNotifyAchievementsUnlocked = (EOS_Achievements_AddNotifyAchievementsUnlockedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(EOS_Achievements_AddNotifyAchievementsUnlockedDelegate));
		IntPtr intPtr2 = getFunctionPointer(libraryHandle, "EOS_Achievements_AddNotifyAchievementsUnlockedV2");
		if (intPtr2 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_AddNotifyAchievementsUnlockedV2");
		}
		EOS_Achievements_AddNotifyAchievementsUnlockedV2 = (EOS_Achievements_AddNotifyAchievementsUnlockedV2Delegate)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(EOS_Achievements_AddNotifyAchievementsUnlockedV2Delegate));
		IntPtr intPtr3 = getFunctionPointer(libraryHandle, "EOS_Achievements_CopyAchievementDefinitionByAchievementId");
		if (intPtr3 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_CopyAchievementDefinitionByAchievementId");
		}
		EOS_Achievements_CopyAchievementDefinitionByAchievementId = (EOS_Achievements_CopyAchievementDefinitionByAchievementIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr3, typeof(EOS_Achievements_CopyAchievementDefinitionByAchievementIdDelegate));
		IntPtr intPtr4 = getFunctionPointer(libraryHandle, "EOS_Achievements_CopyAchievementDefinitionByIndex");
		if (intPtr4 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_CopyAchievementDefinitionByIndex");
		}
		EOS_Achievements_CopyAchievementDefinitionByIndex = (EOS_Achievements_CopyAchievementDefinitionByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr4, typeof(EOS_Achievements_CopyAchievementDefinitionByIndexDelegate));
		IntPtr intPtr5 = getFunctionPointer(libraryHandle, "EOS_Achievements_CopyAchievementDefinitionV2ByAchievementId");
		if (intPtr5 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_CopyAchievementDefinitionV2ByAchievementId");
		}
		EOS_Achievements_CopyAchievementDefinitionV2ByAchievementId = (EOS_Achievements_CopyAchievementDefinitionV2ByAchievementIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr5, typeof(EOS_Achievements_CopyAchievementDefinitionV2ByAchievementIdDelegate));
		IntPtr intPtr6 = getFunctionPointer(libraryHandle, "EOS_Achievements_CopyAchievementDefinitionV2ByIndex");
		if (intPtr6 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_CopyAchievementDefinitionV2ByIndex");
		}
		EOS_Achievements_CopyAchievementDefinitionV2ByIndex = (EOS_Achievements_CopyAchievementDefinitionV2ByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr6, typeof(EOS_Achievements_CopyAchievementDefinitionV2ByIndexDelegate));
		IntPtr intPtr7 = getFunctionPointer(libraryHandle, "EOS_Achievements_CopyPlayerAchievementByAchievementId");
		if (intPtr7 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_CopyPlayerAchievementByAchievementId");
		}
		EOS_Achievements_CopyPlayerAchievementByAchievementId = (EOS_Achievements_CopyPlayerAchievementByAchievementIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr7, typeof(EOS_Achievements_CopyPlayerAchievementByAchievementIdDelegate));
		IntPtr intPtr8 = getFunctionPointer(libraryHandle, "EOS_Achievements_CopyPlayerAchievementByIndex");
		if (intPtr8 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_CopyPlayerAchievementByIndex");
		}
		EOS_Achievements_CopyPlayerAchievementByIndex = (EOS_Achievements_CopyPlayerAchievementByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr8, typeof(EOS_Achievements_CopyPlayerAchievementByIndexDelegate));
		IntPtr intPtr9 = getFunctionPointer(libraryHandle, "EOS_Achievements_CopyUnlockedAchievementByAchievementId");
		if (intPtr9 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_CopyUnlockedAchievementByAchievementId");
		}
		EOS_Achievements_CopyUnlockedAchievementByAchievementId = (EOS_Achievements_CopyUnlockedAchievementByAchievementIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr9, typeof(EOS_Achievements_CopyUnlockedAchievementByAchievementIdDelegate));
		IntPtr intPtr10 = getFunctionPointer(libraryHandle, "EOS_Achievements_CopyUnlockedAchievementByIndex");
		if (intPtr10 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_CopyUnlockedAchievementByIndex");
		}
		EOS_Achievements_CopyUnlockedAchievementByIndex = (EOS_Achievements_CopyUnlockedAchievementByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr10, typeof(EOS_Achievements_CopyUnlockedAchievementByIndexDelegate));
		IntPtr intPtr11 = getFunctionPointer(libraryHandle, "EOS_Achievements_DefinitionV2_Release");
		if (intPtr11 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_DefinitionV2_Release");
		}
		EOS_Achievements_DefinitionV2_Release = (EOS_Achievements_DefinitionV2_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr11, typeof(EOS_Achievements_DefinitionV2_ReleaseDelegate));
		IntPtr intPtr12 = getFunctionPointer(libraryHandle, "EOS_Achievements_Definition_Release");
		if (intPtr12 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_Definition_Release");
		}
		EOS_Achievements_Definition_Release = (EOS_Achievements_Definition_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr12, typeof(EOS_Achievements_Definition_ReleaseDelegate));
		IntPtr intPtr13 = getFunctionPointer(libraryHandle, "EOS_Achievements_GetAchievementDefinitionCount");
		if (intPtr13 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_GetAchievementDefinitionCount");
		}
		EOS_Achievements_GetAchievementDefinitionCount = (EOS_Achievements_GetAchievementDefinitionCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr13, typeof(EOS_Achievements_GetAchievementDefinitionCountDelegate));
		IntPtr intPtr14 = getFunctionPointer(libraryHandle, "EOS_Achievements_GetPlayerAchievementCount");
		if (intPtr14 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_GetPlayerAchievementCount");
		}
		EOS_Achievements_GetPlayerAchievementCount = (EOS_Achievements_GetPlayerAchievementCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr14, typeof(EOS_Achievements_GetPlayerAchievementCountDelegate));
		IntPtr intPtr15 = getFunctionPointer(libraryHandle, "EOS_Achievements_GetUnlockedAchievementCount");
		if (intPtr15 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_GetUnlockedAchievementCount");
		}
		EOS_Achievements_GetUnlockedAchievementCount = (EOS_Achievements_GetUnlockedAchievementCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr15, typeof(EOS_Achievements_GetUnlockedAchievementCountDelegate));
		IntPtr intPtr16 = getFunctionPointer(libraryHandle, "EOS_Achievements_PlayerAchievement_Release");
		if (intPtr16 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_PlayerAchievement_Release");
		}
		EOS_Achievements_PlayerAchievement_Release = (EOS_Achievements_PlayerAchievement_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr16, typeof(EOS_Achievements_PlayerAchievement_ReleaseDelegate));
		IntPtr intPtr17 = getFunctionPointer(libraryHandle, "EOS_Achievements_QueryDefinitions");
		if (intPtr17 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_QueryDefinitions");
		}
		EOS_Achievements_QueryDefinitions = (EOS_Achievements_QueryDefinitionsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr17, typeof(EOS_Achievements_QueryDefinitionsDelegate));
		IntPtr intPtr18 = getFunctionPointer(libraryHandle, "EOS_Achievements_QueryPlayerAchievements");
		if (intPtr18 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_QueryPlayerAchievements");
		}
		EOS_Achievements_QueryPlayerAchievements = (EOS_Achievements_QueryPlayerAchievementsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr18, typeof(EOS_Achievements_QueryPlayerAchievementsDelegate));
		IntPtr intPtr19 = getFunctionPointer(libraryHandle, "EOS_Achievements_RemoveNotifyAchievementsUnlocked");
		if (intPtr19 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_RemoveNotifyAchievementsUnlocked");
		}
		EOS_Achievements_RemoveNotifyAchievementsUnlocked = (EOS_Achievements_RemoveNotifyAchievementsUnlockedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr19, typeof(EOS_Achievements_RemoveNotifyAchievementsUnlockedDelegate));
		IntPtr intPtr20 = getFunctionPointer(libraryHandle, "EOS_Achievements_UnlockAchievements");
		if (intPtr20 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_UnlockAchievements");
		}
		EOS_Achievements_UnlockAchievements = (EOS_Achievements_UnlockAchievementsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr20, typeof(EOS_Achievements_UnlockAchievementsDelegate));
		IntPtr intPtr21 = getFunctionPointer(libraryHandle, "EOS_Achievements_UnlockedAchievement_Release");
		if (intPtr21 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Achievements_UnlockedAchievement_Release");
		}
		EOS_Achievements_UnlockedAchievement_Release = (EOS_Achievements_UnlockedAchievement_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr21, typeof(EOS_Achievements_UnlockedAchievement_ReleaseDelegate));
		IntPtr intPtr22 = getFunctionPointer(libraryHandle, "EOS_ActiveSession_CopyInfo");
		if (intPtr22 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ActiveSession_CopyInfo");
		}
		EOS_ActiveSession_CopyInfo = (EOS_ActiveSession_CopyInfoDelegate)Marshal.GetDelegateForFunctionPointer(intPtr22, typeof(EOS_ActiveSession_CopyInfoDelegate));
		IntPtr intPtr23 = getFunctionPointer(libraryHandle, "EOS_ActiveSession_GetRegisteredPlayerByIndex");
		if (intPtr23 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ActiveSession_GetRegisteredPlayerByIndex");
		}
		EOS_ActiveSession_GetRegisteredPlayerByIndex = (EOS_ActiveSession_GetRegisteredPlayerByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr23, typeof(EOS_ActiveSession_GetRegisteredPlayerByIndexDelegate));
		IntPtr intPtr24 = getFunctionPointer(libraryHandle, "EOS_ActiveSession_GetRegisteredPlayerCount");
		if (intPtr24 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ActiveSession_GetRegisteredPlayerCount");
		}
		EOS_ActiveSession_GetRegisteredPlayerCount = (EOS_ActiveSession_GetRegisteredPlayerCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr24, typeof(EOS_ActiveSession_GetRegisteredPlayerCountDelegate));
		IntPtr intPtr25 = getFunctionPointer(libraryHandle, "EOS_ActiveSession_Info_Release");
		if (intPtr25 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ActiveSession_Info_Release");
		}
		EOS_ActiveSession_Info_Release = (EOS_ActiveSession_Info_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr25, typeof(EOS_ActiveSession_Info_ReleaseDelegate));
		IntPtr intPtr26 = getFunctionPointer(libraryHandle, "EOS_ActiveSession_Release");
		if (intPtr26 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ActiveSession_Release");
		}
		EOS_ActiveSession_Release = (EOS_ActiveSession_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr26, typeof(EOS_ActiveSession_ReleaseDelegate));
		IntPtr intPtr27 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_AddExternalIntegrityCatalog");
		if (intPtr27 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_AddExternalIntegrityCatalog");
		}
		EOS_AntiCheatClient_AddExternalIntegrityCatalog = (EOS_AntiCheatClient_AddExternalIntegrityCatalogDelegate)Marshal.GetDelegateForFunctionPointer(intPtr27, typeof(EOS_AntiCheatClient_AddExternalIntegrityCatalogDelegate));
		IntPtr intPtr28 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_AddNotifyClientIntegrityViolated");
		if (intPtr28 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_AddNotifyClientIntegrityViolated");
		}
		EOS_AntiCheatClient_AddNotifyClientIntegrityViolated = (EOS_AntiCheatClient_AddNotifyClientIntegrityViolatedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr28, typeof(EOS_AntiCheatClient_AddNotifyClientIntegrityViolatedDelegate));
		IntPtr intPtr29 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_AddNotifyMessageToPeer");
		if (intPtr29 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_AddNotifyMessageToPeer");
		}
		EOS_AntiCheatClient_AddNotifyMessageToPeer = (EOS_AntiCheatClient_AddNotifyMessageToPeerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr29, typeof(EOS_AntiCheatClient_AddNotifyMessageToPeerDelegate));
		IntPtr intPtr30 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_AddNotifyMessageToServer");
		if (intPtr30 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_AddNotifyMessageToServer");
		}
		EOS_AntiCheatClient_AddNotifyMessageToServer = (EOS_AntiCheatClient_AddNotifyMessageToServerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr30, typeof(EOS_AntiCheatClient_AddNotifyMessageToServerDelegate));
		IntPtr intPtr31 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_AddNotifyPeerActionRequired");
		if (intPtr31 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_AddNotifyPeerActionRequired");
		}
		EOS_AntiCheatClient_AddNotifyPeerActionRequired = (EOS_AntiCheatClient_AddNotifyPeerActionRequiredDelegate)Marshal.GetDelegateForFunctionPointer(intPtr31, typeof(EOS_AntiCheatClient_AddNotifyPeerActionRequiredDelegate));
		IntPtr intPtr32 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_AddNotifyPeerAuthStatusChanged");
		if (intPtr32 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_AddNotifyPeerAuthStatusChanged");
		}
		EOS_AntiCheatClient_AddNotifyPeerAuthStatusChanged = (EOS_AntiCheatClient_AddNotifyPeerAuthStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr32, typeof(EOS_AntiCheatClient_AddNotifyPeerAuthStatusChangedDelegate));
		IntPtr intPtr33 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_BeginSession");
		if (intPtr33 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_BeginSession");
		}
		EOS_AntiCheatClient_BeginSession = (EOS_AntiCheatClient_BeginSessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr33, typeof(EOS_AntiCheatClient_BeginSessionDelegate));
		IntPtr intPtr34 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_EndSession");
		if (intPtr34 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_EndSession");
		}
		EOS_AntiCheatClient_EndSession = (EOS_AntiCheatClient_EndSessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr34, typeof(EOS_AntiCheatClient_EndSessionDelegate));
		IntPtr intPtr35 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_GetProtectMessageOutputLength");
		if (intPtr35 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_GetProtectMessageOutputLength");
		}
		EOS_AntiCheatClient_GetProtectMessageOutputLength = (EOS_AntiCheatClient_GetProtectMessageOutputLengthDelegate)Marshal.GetDelegateForFunctionPointer(intPtr35, typeof(EOS_AntiCheatClient_GetProtectMessageOutputLengthDelegate));
		IntPtr intPtr36 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_PollStatus");
		if (intPtr36 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_PollStatus");
		}
		EOS_AntiCheatClient_PollStatus = (EOS_AntiCheatClient_PollStatusDelegate)Marshal.GetDelegateForFunctionPointer(intPtr36, typeof(EOS_AntiCheatClient_PollStatusDelegate));
		IntPtr intPtr37 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_ProtectMessage");
		if (intPtr37 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_ProtectMessage");
		}
		EOS_AntiCheatClient_ProtectMessage = (EOS_AntiCheatClient_ProtectMessageDelegate)Marshal.GetDelegateForFunctionPointer(intPtr37, typeof(EOS_AntiCheatClient_ProtectMessageDelegate));
		IntPtr intPtr38 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_ReceiveMessageFromPeer");
		if (intPtr38 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_ReceiveMessageFromPeer");
		}
		EOS_AntiCheatClient_ReceiveMessageFromPeer = (EOS_AntiCheatClient_ReceiveMessageFromPeerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr38, typeof(EOS_AntiCheatClient_ReceiveMessageFromPeerDelegate));
		IntPtr intPtr39 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_ReceiveMessageFromServer");
		if (intPtr39 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_ReceiveMessageFromServer");
		}
		EOS_AntiCheatClient_ReceiveMessageFromServer = (EOS_AntiCheatClient_ReceiveMessageFromServerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr39, typeof(EOS_AntiCheatClient_ReceiveMessageFromServerDelegate));
		IntPtr intPtr40 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_RegisterPeer");
		if (intPtr40 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_RegisterPeer");
		}
		EOS_AntiCheatClient_RegisterPeer = (EOS_AntiCheatClient_RegisterPeerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr40, typeof(EOS_AntiCheatClient_RegisterPeerDelegate));
		IntPtr intPtr41 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolated");
		if (intPtr41 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolated");
		}
		EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolated = (EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolatedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr41, typeof(EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolatedDelegate));
		IntPtr intPtr42 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_RemoveNotifyMessageToPeer");
		if (intPtr42 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_RemoveNotifyMessageToPeer");
		}
		EOS_AntiCheatClient_RemoveNotifyMessageToPeer = (EOS_AntiCheatClient_RemoveNotifyMessageToPeerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr42, typeof(EOS_AntiCheatClient_RemoveNotifyMessageToPeerDelegate));
		IntPtr intPtr43 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_RemoveNotifyMessageToServer");
		if (intPtr43 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_RemoveNotifyMessageToServer");
		}
		EOS_AntiCheatClient_RemoveNotifyMessageToServer = (EOS_AntiCheatClient_RemoveNotifyMessageToServerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr43, typeof(EOS_AntiCheatClient_RemoveNotifyMessageToServerDelegate));
		IntPtr intPtr44 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_RemoveNotifyPeerActionRequired");
		if (intPtr44 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_RemoveNotifyPeerActionRequired");
		}
		EOS_AntiCheatClient_RemoveNotifyPeerActionRequired = (EOS_AntiCheatClient_RemoveNotifyPeerActionRequiredDelegate)Marshal.GetDelegateForFunctionPointer(intPtr44, typeof(EOS_AntiCheatClient_RemoveNotifyPeerActionRequiredDelegate));
		IntPtr intPtr45 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChanged");
		if (intPtr45 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChanged");
		}
		EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChanged = (EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr45, typeof(EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChangedDelegate));
		IntPtr intPtr46 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_UnprotectMessage");
		if (intPtr46 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_UnprotectMessage");
		}
		EOS_AntiCheatClient_UnprotectMessage = (EOS_AntiCheatClient_UnprotectMessageDelegate)Marshal.GetDelegateForFunctionPointer(intPtr46, typeof(EOS_AntiCheatClient_UnprotectMessageDelegate));
		IntPtr intPtr47 = getFunctionPointer(libraryHandle, "EOS_AntiCheatClient_UnregisterPeer");
		if (intPtr47 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatClient_UnregisterPeer");
		}
		EOS_AntiCheatClient_UnregisterPeer = (EOS_AntiCheatClient_UnregisterPeerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr47, typeof(EOS_AntiCheatClient_UnregisterPeerDelegate));
		IntPtr intPtr48 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_AddNotifyClientActionRequired");
		if (intPtr48 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_AddNotifyClientActionRequired");
		}
		EOS_AntiCheatServer_AddNotifyClientActionRequired = (EOS_AntiCheatServer_AddNotifyClientActionRequiredDelegate)Marshal.GetDelegateForFunctionPointer(intPtr48, typeof(EOS_AntiCheatServer_AddNotifyClientActionRequiredDelegate));
		IntPtr intPtr49 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_AddNotifyClientAuthStatusChanged");
		if (intPtr49 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_AddNotifyClientAuthStatusChanged");
		}
		EOS_AntiCheatServer_AddNotifyClientAuthStatusChanged = (EOS_AntiCheatServer_AddNotifyClientAuthStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr49, typeof(EOS_AntiCheatServer_AddNotifyClientAuthStatusChangedDelegate));
		IntPtr intPtr50 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_AddNotifyMessageToClient");
		if (intPtr50 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_AddNotifyMessageToClient");
		}
		EOS_AntiCheatServer_AddNotifyMessageToClient = (EOS_AntiCheatServer_AddNotifyMessageToClientDelegate)Marshal.GetDelegateForFunctionPointer(intPtr50, typeof(EOS_AntiCheatServer_AddNotifyMessageToClientDelegate));
		IntPtr intPtr51 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_BeginSession");
		if (intPtr51 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_BeginSession");
		}
		EOS_AntiCheatServer_BeginSession = (EOS_AntiCheatServer_BeginSessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr51, typeof(EOS_AntiCheatServer_BeginSessionDelegate));
		IntPtr intPtr52 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_EndSession");
		if (intPtr52 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_EndSession");
		}
		EOS_AntiCheatServer_EndSession = (EOS_AntiCheatServer_EndSessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr52, typeof(EOS_AntiCheatServer_EndSessionDelegate));
		IntPtr intPtr53 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_GetProtectMessageOutputLength");
		if (intPtr53 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_GetProtectMessageOutputLength");
		}
		EOS_AntiCheatServer_GetProtectMessageOutputLength = (EOS_AntiCheatServer_GetProtectMessageOutputLengthDelegate)Marshal.GetDelegateForFunctionPointer(intPtr53, typeof(EOS_AntiCheatServer_GetProtectMessageOutputLengthDelegate));
		IntPtr intPtr54 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_LogEvent");
		if (intPtr54 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_LogEvent");
		}
		EOS_AntiCheatServer_LogEvent = (EOS_AntiCheatServer_LogEventDelegate)Marshal.GetDelegateForFunctionPointer(intPtr54, typeof(EOS_AntiCheatServer_LogEventDelegate));
		IntPtr intPtr55 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_LogGameRoundEnd");
		if (intPtr55 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_LogGameRoundEnd");
		}
		EOS_AntiCheatServer_LogGameRoundEnd = (EOS_AntiCheatServer_LogGameRoundEndDelegate)Marshal.GetDelegateForFunctionPointer(intPtr55, typeof(EOS_AntiCheatServer_LogGameRoundEndDelegate));
		IntPtr intPtr56 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_LogGameRoundStart");
		if (intPtr56 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_LogGameRoundStart");
		}
		EOS_AntiCheatServer_LogGameRoundStart = (EOS_AntiCheatServer_LogGameRoundStartDelegate)Marshal.GetDelegateForFunctionPointer(intPtr56, typeof(EOS_AntiCheatServer_LogGameRoundStartDelegate));
		IntPtr intPtr57 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_LogPlayerDespawn");
		if (intPtr57 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_LogPlayerDespawn");
		}
		EOS_AntiCheatServer_LogPlayerDespawn = (EOS_AntiCheatServer_LogPlayerDespawnDelegate)Marshal.GetDelegateForFunctionPointer(intPtr57, typeof(EOS_AntiCheatServer_LogPlayerDespawnDelegate));
		IntPtr intPtr58 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_LogPlayerRevive");
		if (intPtr58 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_LogPlayerRevive");
		}
		EOS_AntiCheatServer_LogPlayerRevive = (EOS_AntiCheatServer_LogPlayerReviveDelegate)Marshal.GetDelegateForFunctionPointer(intPtr58, typeof(EOS_AntiCheatServer_LogPlayerReviveDelegate));
		IntPtr intPtr59 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_LogPlayerSpawn");
		if (intPtr59 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_LogPlayerSpawn");
		}
		EOS_AntiCheatServer_LogPlayerSpawn = (EOS_AntiCheatServer_LogPlayerSpawnDelegate)Marshal.GetDelegateForFunctionPointer(intPtr59, typeof(EOS_AntiCheatServer_LogPlayerSpawnDelegate));
		IntPtr intPtr60 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_LogPlayerTakeDamage");
		if (intPtr60 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_LogPlayerTakeDamage");
		}
		EOS_AntiCheatServer_LogPlayerTakeDamage = (EOS_AntiCheatServer_LogPlayerTakeDamageDelegate)Marshal.GetDelegateForFunctionPointer(intPtr60, typeof(EOS_AntiCheatServer_LogPlayerTakeDamageDelegate));
		IntPtr intPtr61 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_LogPlayerTick");
		if (intPtr61 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_LogPlayerTick");
		}
		EOS_AntiCheatServer_LogPlayerTick = (EOS_AntiCheatServer_LogPlayerTickDelegate)Marshal.GetDelegateForFunctionPointer(intPtr61, typeof(EOS_AntiCheatServer_LogPlayerTickDelegate));
		IntPtr intPtr62 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_LogPlayerUseAbility");
		if (intPtr62 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_LogPlayerUseAbility");
		}
		EOS_AntiCheatServer_LogPlayerUseAbility = (EOS_AntiCheatServer_LogPlayerUseAbilityDelegate)Marshal.GetDelegateForFunctionPointer(intPtr62, typeof(EOS_AntiCheatServer_LogPlayerUseAbilityDelegate));
		IntPtr intPtr63 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_LogPlayerUseWeapon");
		if (intPtr63 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_LogPlayerUseWeapon");
		}
		EOS_AntiCheatServer_LogPlayerUseWeapon = (EOS_AntiCheatServer_LogPlayerUseWeaponDelegate)Marshal.GetDelegateForFunctionPointer(intPtr63, typeof(EOS_AntiCheatServer_LogPlayerUseWeaponDelegate));
		IntPtr intPtr64 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_ProtectMessage");
		if (intPtr64 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_ProtectMessage");
		}
		EOS_AntiCheatServer_ProtectMessage = (EOS_AntiCheatServer_ProtectMessageDelegate)Marshal.GetDelegateForFunctionPointer(intPtr64, typeof(EOS_AntiCheatServer_ProtectMessageDelegate));
		IntPtr intPtr65 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_ReceiveMessageFromClient");
		if (intPtr65 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_ReceiveMessageFromClient");
		}
		EOS_AntiCheatServer_ReceiveMessageFromClient = (EOS_AntiCheatServer_ReceiveMessageFromClientDelegate)Marshal.GetDelegateForFunctionPointer(intPtr65, typeof(EOS_AntiCheatServer_ReceiveMessageFromClientDelegate));
		IntPtr intPtr66 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_RegisterClient");
		if (intPtr66 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_RegisterClient");
		}
		EOS_AntiCheatServer_RegisterClient = (EOS_AntiCheatServer_RegisterClientDelegate)Marshal.GetDelegateForFunctionPointer(intPtr66, typeof(EOS_AntiCheatServer_RegisterClientDelegate));
		IntPtr intPtr67 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_RegisterEvent");
		if (intPtr67 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_RegisterEvent");
		}
		EOS_AntiCheatServer_RegisterEvent = (EOS_AntiCheatServer_RegisterEventDelegate)Marshal.GetDelegateForFunctionPointer(intPtr67, typeof(EOS_AntiCheatServer_RegisterEventDelegate));
		IntPtr intPtr68 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_RemoveNotifyClientActionRequired");
		if (intPtr68 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_RemoveNotifyClientActionRequired");
		}
		EOS_AntiCheatServer_RemoveNotifyClientActionRequired = (EOS_AntiCheatServer_RemoveNotifyClientActionRequiredDelegate)Marshal.GetDelegateForFunctionPointer(intPtr68, typeof(EOS_AntiCheatServer_RemoveNotifyClientActionRequiredDelegate));
		IntPtr intPtr69 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChanged");
		if (intPtr69 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChanged");
		}
		EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChanged = (EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr69, typeof(EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChangedDelegate));
		IntPtr intPtr70 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_RemoveNotifyMessageToClient");
		if (intPtr70 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_RemoveNotifyMessageToClient");
		}
		EOS_AntiCheatServer_RemoveNotifyMessageToClient = (EOS_AntiCheatServer_RemoveNotifyMessageToClientDelegate)Marshal.GetDelegateForFunctionPointer(intPtr70, typeof(EOS_AntiCheatServer_RemoveNotifyMessageToClientDelegate));
		IntPtr intPtr71 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_SetClientDetails");
		if (intPtr71 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_SetClientDetails");
		}
		EOS_AntiCheatServer_SetClientDetails = (EOS_AntiCheatServer_SetClientDetailsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr71, typeof(EOS_AntiCheatServer_SetClientDetailsDelegate));
		IntPtr intPtr72 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_SetClientNetworkState");
		if (intPtr72 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_SetClientNetworkState");
		}
		EOS_AntiCheatServer_SetClientNetworkState = (EOS_AntiCheatServer_SetClientNetworkStateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr72, typeof(EOS_AntiCheatServer_SetClientNetworkStateDelegate));
		IntPtr intPtr73 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_SetGameSessionId");
		if (intPtr73 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_SetGameSessionId");
		}
		EOS_AntiCheatServer_SetGameSessionId = (EOS_AntiCheatServer_SetGameSessionIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr73, typeof(EOS_AntiCheatServer_SetGameSessionIdDelegate));
		IntPtr intPtr74 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_UnprotectMessage");
		if (intPtr74 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_UnprotectMessage");
		}
		EOS_AntiCheatServer_UnprotectMessage = (EOS_AntiCheatServer_UnprotectMessageDelegate)Marshal.GetDelegateForFunctionPointer(intPtr74, typeof(EOS_AntiCheatServer_UnprotectMessageDelegate));
		IntPtr intPtr75 = getFunctionPointer(libraryHandle, "EOS_AntiCheatServer_UnregisterClient");
		if (intPtr75 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_AntiCheatServer_UnregisterClient");
		}
		EOS_AntiCheatServer_UnregisterClient = (EOS_AntiCheatServer_UnregisterClientDelegate)Marshal.GetDelegateForFunctionPointer(intPtr75, typeof(EOS_AntiCheatServer_UnregisterClientDelegate));
		IntPtr intPtr76 = getFunctionPointer(libraryHandle, "EOS_Auth_AddNotifyLoginStatusChanged");
		if (intPtr76 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_AddNotifyLoginStatusChanged");
		}
		EOS_Auth_AddNotifyLoginStatusChanged = (EOS_Auth_AddNotifyLoginStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr76, typeof(EOS_Auth_AddNotifyLoginStatusChangedDelegate));
		IntPtr intPtr77 = getFunctionPointer(libraryHandle, "EOS_Auth_CopyIdToken");
		if (intPtr77 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_CopyIdToken");
		}
		EOS_Auth_CopyIdToken = (EOS_Auth_CopyIdTokenDelegate)Marshal.GetDelegateForFunctionPointer(intPtr77, typeof(EOS_Auth_CopyIdTokenDelegate));
		IntPtr intPtr78 = getFunctionPointer(libraryHandle, "EOS_Auth_CopyUserAuthToken");
		if (intPtr78 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_CopyUserAuthToken");
		}
		EOS_Auth_CopyUserAuthToken = (EOS_Auth_CopyUserAuthTokenDelegate)Marshal.GetDelegateForFunctionPointer(intPtr78, typeof(EOS_Auth_CopyUserAuthTokenDelegate));
		IntPtr intPtr79 = getFunctionPointer(libraryHandle, "EOS_Auth_DeletePersistentAuth");
		if (intPtr79 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_DeletePersistentAuth");
		}
		EOS_Auth_DeletePersistentAuth = (EOS_Auth_DeletePersistentAuthDelegate)Marshal.GetDelegateForFunctionPointer(intPtr79, typeof(EOS_Auth_DeletePersistentAuthDelegate));
		IntPtr intPtr80 = getFunctionPointer(libraryHandle, "EOS_Auth_GetLoggedInAccountByIndex");
		if (intPtr80 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_GetLoggedInAccountByIndex");
		}
		EOS_Auth_GetLoggedInAccountByIndex = (EOS_Auth_GetLoggedInAccountByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr80, typeof(EOS_Auth_GetLoggedInAccountByIndexDelegate));
		IntPtr intPtr81 = getFunctionPointer(libraryHandle, "EOS_Auth_GetLoggedInAccountsCount");
		if (intPtr81 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_GetLoggedInAccountsCount");
		}
		EOS_Auth_GetLoggedInAccountsCount = (EOS_Auth_GetLoggedInAccountsCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr81, typeof(EOS_Auth_GetLoggedInAccountsCountDelegate));
		IntPtr intPtr82 = getFunctionPointer(libraryHandle, "EOS_Auth_GetLoginStatus");
		if (intPtr82 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_GetLoginStatus");
		}
		EOS_Auth_GetLoginStatus = (EOS_Auth_GetLoginStatusDelegate)Marshal.GetDelegateForFunctionPointer(intPtr82, typeof(EOS_Auth_GetLoginStatusDelegate));
		IntPtr intPtr83 = getFunctionPointer(libraryHandle, "EOS_Auth_GetMergedAccountByIndex");
		if (intPtr83 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_GetMergedAccountByIndex");
		}
		EOS_Auth_GetMergedAccountByIndex = (EOS_Auth_GetMergedAccountByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr83, typeof(EOS_Auth_GetMergedAccountByIndexDelegate));
		IntPtr intPtr84 = getFunctionPointer(libraryHandle, "EOS_Auth_GetMergedAccountsCount");
		if (intPtr84 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_GetMergedAccountsCount");
		}
		EOS_Auth_GetMergedAccountsCount = (EOS_Auth_GetMergedAccountsCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr84, typeof(EOS_Auth_GetMergedAccountsCountDelegate));
		IntPtr intPtr85 = getFunctionPointer(libraryHandle, "EOS_Auth_GetSelectedAccountId");
		if (intPtr85 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_GetSelectedAccountId");
		}
		EOS_Auth_GetSelectedAccountId = (EOS_Auth_GetSelectedAccountIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr85, typeof(EOS_Auth_GetSelectedAccountIdDelegate));
		IntPtr intPtr86 = getFunctionPointer(libraryHandle, "EOS_Auth_IdToken_Release");
		if (intPtr86 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_IdToken_Release");
		}
		EOS_Auth_IdToken_Release = (EOS_Auth_IdToken_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr86, typeof(EOS_Auth_IdToken_ReleaseDelegate));
		IntPtr intPtr87 = getFunctionPointer(libraryHandle, "EOS_Auth_LinkAccount");
		if (intPtr87 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_LinkAccount");
		}
		EOS_Auth_LinkAccount = (EOS_Auth_LinkAccountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr87, typeof(EOS_Auth_LinkAccountDelegate));
		IntPtr intPtr88 = getFunctionPointer(libraryHandle, "EOS_Auth_Login");
		if (intPtr88 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_Login");
		}
		EOS_Auth_Login = (EOS_Auth_LoginDelegate)Marshal.GetDelegateForFunctionPointer(intPtr88, typeof(EOS_Auth_LoginDelegate));
		IntPtr intPtr89 = getFunctionPointer(libraryHandle, "EOS_Auth_Logout");
		if (intPtr89 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_Logout");
		}
		EOS_Auth_Logout = (EOS_Auth_LogoutDelegate)Marshal.GetDelegateForFunctionPointer(intPtr89, typeof(EOS_Auth_LogoutDelegate));
		IntPtr intPtr90 = getFunctionPointer(libraryHandle, "EOS_Auth_QueryIdToken");
		if (intPtr90 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_QueryIdToken");
		}
		EOS_Auth_QueryIdToken = (EOS_Auth_QueryIdTokenDelegate)Marshal.GetDelegateForFunctionPointer(intPtr90, typeof(EOS_Auth_QueryIdTokenDelegate));
		IntPtr intPtr91 = getFunctionPointer(libraryHandle, "EOS_Auth_RemoveNotifyLoginStatusChanged");
		if (intPtr91 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_RemoveNotifyLoginStatusChanged");
		}
		EOS_Auth_RemoveNotifyLoginStatusChanged = (EOS_Auth_RemoveNotifyLoginStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr91, typeof(EOS_Auth_RemoveNotifyLoginStatusChangedDelegate));
		IntPtr intPtr92 = getFunctionPointer(libraryHandle, "EOS_Auth_Token_Release");
		if (intPtr92 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_Token_Release");
		}
		EOS_Auth_Token_Release = (EOS_Auth_Token_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr92, typeof(EOS_Auth_Token_ReleaseDelegate));
		IntPtr intPtr93 = getFunctionPointer(libraryHandle, "EOS_Auth_VerifyIdToken");
		if (intPtr93 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_VerifyIdToken");
		}
		EOS_Auth_VerifyIdToken = (EOS_Auth_VerifyIdTokenDelegate)Marshal.GetDelegateForFunctionPointer(intPtr93, typeof(EOS_Auth_VerifyIdTokenDelegate));
		IntPtr intPtr94 = getFunctionPointer(libraryHandle, "EOS_Auth_VerifyUserAuth");
		if (intPtr94 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Auth_VerifyUserAuth");
		}
		EOS_Auth_VerifyUserAuth = (EOS_Auth_VerifyUserAuthDelegate)Marshal.GetDelegateForFunctionPointer(intPtr94, typeof(EOS_Auth_VerifyUserAuthDelegate));
		IntPtr intPtr95 = getFunctionPointer(libraryHandle, "EOS_ByteArray_ToString");
		if (intPtr95 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ByteArray_ToString");
		}
		EOS_ByteArray_ToString = (EOS_ByteArray_ToStringDelegate)Marshal.GetDelegateForFunctionPointer(intPtr95, typeof(EOS_ByteArray_ToStringDelegate));
		IntPtr intPtr96 = getFunctionPointer(libraryHandle, "EOS_Connect_AddNotifyAuthExpiration");
		if (intPtr96 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_AddNotifyAuthExpiration");
		}
		EOS_Connect_AddNotifyAuthExpiration = (EOS_Connect_AddNotifyAuthExpirationDelegate)Marshal.GetDelegateForFunctionPointer(intPtr96, typeof(EOS_Connect_AddNotifyAuthExpirationDelegate));
		IntPtr intPtr97 = getFunctionPointer(libraryHandle, "EOS_Connect_AddNotifyLoginStatusChanged");
		if (intPtr97 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_AddNotifyLoginStatusChanged");
		}
		EOS_Connect_AddNotifyLoginStatusChanged = (EOS_Connect_AddNotifyLoginStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr97, typeof(EOS_Connect_AddNotifyLoginStatusChangedDelegate));
		IntPtr intPtr98 = getFunctionPointer(libraryHandle, "EOS_Connect_CopyIdToken");
		if (intPtr98 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_CopyIdToken");
		}
		EOS_Connect_CopyIdToken = (EOS_Connect_CopyIdTokenDelegate)Marshal.GetDelegateForFunctionPointer(intPtr98, typeof(EOS_Connect_CopyIdTokenDelegate));
		IntPtr intPtr99 = getFunctionPointer(libraryHandle, "EOS_Connect_CopyProductUserExternalAccountByAccountId");
		if (intPtr99 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_CopyProductUserExternalAccountByAccountId");
		}
		EOS_Connect_CopyProductUserExternalAccountByAccountId = (EOS_Connect_CopyProductUserExternalAccountByAccountIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr99, typeof(EOS_Connect_CopyProductUserExternalAccountByAccountIdDelegate));
		IntPtr intPtr100 = getFunctionPointer(libraryHandle, "EOS_Connect_CopyProductUserExternalAccountByAccountType");
		if (intPtr100 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_CopyProductUserExternalAccountByAccountType");
		}
		EOS_Connect_CopyProductUserExternalAccountByAccountType = (EOS_Connect_CopyProductUserExternalAccountByAccountTypeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr100, typeof(EOS_Connect_CopyProductUserExternalAccountByAccountTypeDelegate));
		IntPtr intPtr101 = getFunctionPointer(libraryHandle, "EOS_Connect_CopyProductUserExternalAccountByIndex");
		if (intPtr101 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_CopyProductUserExternalAccountByIndex");
		}
		EOS_Connect_CopyProductUserExternalAccountByIndex = (EOS_Connect_CopyProductUserExternalAccountByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr101, typeof(EOS_Connect_CopyProductUserExternalAccountByIndexDelegate));
		IntPtr intPtr102 = getFunctionPointer(libraryHandle, "EOS_Connect_CopyProductUserInfo");
		if (intPtr102 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_CopyProductUserInfo");
		}
		EOS_Connect_CopyProductUserInfo = (EOS_Connect_CopyProductUserInfoDelegate)Marshal.GetDelegateForFunctionPointer(intPtr102, typeof(EOS_Connect_CopyProductUserInfoDelegate));
		IntPtr intPtr103 = getFunctionPointer(libraryHandle, "EOS_Connect_CreateDeviceId");
		if (intPtr103 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_CreateDeviceId");
		}
		EOS_Connect_CreateDeviceId = (EOS_Connect_CreateDeviceIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr103, typeof(EOS_Connect_CreateDeviceIdDelegate));
		IntPtr intPtr104 = getFunctionPointer(libraryHandle, "EOS_Connect_CreateUser");
		if (intPtr104 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_CreateUser");
		}
		EOS_Connect_CreateUser = (EOS_Connect_CreateUserDelegate)Marshal.GetDelegateForFunctionPointer(intPtr104, typeof(EOS_Connect_CreateUserDelegate));
		IntPtr intPtr105 = getFunctionPointer(libraryHandle, "EOS_Connect_DeleteDeviceId");
		if (intPtr105 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_DeleteDeviceId");
		}
		EOS_Connect_DeleteDeviceId = (EOS_Connect_DeleteDeviceIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr105, typeof(EOS_Connect_DeleteDeviceIdDelegate));
		IntPtr intPtr106 = getFunctionPointer(libraryHandle, "EOS_Connect_ExternalAccountInfo_Release");
		if (intPtr106 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_ExternalAccountInfo_Release");
		}
		EOS_Connect_ExternalAccountInfo_Release = (EOS_Connect_ExternalAccountInfo_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr106, typeof(EOS_Connect_ExternalAccountInfo_ReleaseDelegate));
		IntPtr intPtr107 = getFunctionPointer(libraryHandle, "EOS_Connect_GetExternalAccountMapping");
		if (intPtr107 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_GetExternalAccountMapping");
		}
		EOS_Connect_GetExternalAccountMapping = (EOS_Connect_GetExternalAccountMappingDelegate)Marshal.GetDelegateForFunctionPointer(intPtr107, typeof(EOS_Connect_GetExternalAccountMappingDelegate));
		IntPtr intPtr108 = getFunctionPointer(libraryHandle, "EOS_Connect_GetLoggedInUserByIndex");
		if (intPtr108 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_GetLoggedInUserByIndex");
		}
		EOS_Connect_GetLoggedInUserByIndex = (EOS_Connect_GetLoggedInUserByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr108, typeof(EOS_Connect_GetLoggedInUserByIndexDelegate));
		IntPtr intPtr109 = getFunctionPointer(libraryHandle, "EOS_Connect_GetLoggedInUsersCount");
		if (intPtr109 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_GetLoggedInUsersCount");
		}
		EOS_Connect_GetLoggedInUsersCount = (EOS_Connect_GetLoggedInUsersCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr109, typeof(EOS_Connect_GetLoggedInUsersCountDelegate));
		IntPtr intPtr110 = getFunctionPointer(libraryHandle, "EOS_Connect_GetLoginStatus");
		if (intPtr110 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_GetLoginStatus");
		}
		EOS_Connect_GetLoginStatus = (EOS_Connect_GetLoginStatusDelegate)Marshal.GetDelegateForFunctionPointer(intPtr110, typeof(EOS_Connect_GetLoginStatusDelegate));
		IntPtr intPtr111 = getFunctionPointer(libraryHandle, "EOS_Connect_GetProductUserExternalAccountCount");
		if (intPtr111 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_GetProductUserExternalAccountCount");
		}
		EOS_Connect_GetProductUserExternalAccountCount = (EOS_Connect_GetProductUserExternalAccountCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr111, typeof(EOS_Connect_GetProductUserExternalAccountCountDelegate));
		IntPtr intPtr112 = getFunctionPointer(libraryHandle, "EOS_Connect_GetProductUserIdMapping");
		if (intPtr112 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_GetProductUserIdMapping");
		}
		EOS_Connect_GetProductUserIdMapping = (EOS_Connect_GetProductUserIdMappingDelegate)Marshal.GetDelegateForFunctionPointer(intPtr112, typeof(EOS_Connect_GetProductUserIdMappingDelegate));
		IntPtr intPtr113 = getFunctionPointer(libraryHandle, "EOS_Connect_IdToken_Release");
		if (intPtr113 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_IdToken_Release");
		}
		EOS_Connect_IdToken_Release = (EOS_Connect_IdToken_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr113, typeof(EOS_Connect_IdToken_ReleaseDelegate));
		IntPtr intPtr114 = getFunctionPointer(libraryHandle, "EOS_Connect_LinkAccount");
		if (intPtr114 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_LinkAccount");
		}
		EOS_Connect_LinkAccount = (EOS_Connect_LinkAccountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr114, typeof(EOS_Connect_LinkAccountDelegate));
		IntPtr intPtr115 = getFunctionPointer(libraryHandle, "EOS_Connect_Login");
		if (intPtr115 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_Login");
		}
		EOS_Connect_Login = (EOS_Connect_LoginDelegate)Marshal.GetDelegateForFunctionPointer(intPtr115, typeof(EOS_Connect_LoginDelegate));
		IntPtr intPtr116 = getFunctionPointer(libraryHandle, "EOS_Connect_QueryExternalAccountMappings");
		if (intPtr116 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_QueryExternalAccountMappings");
		}
		EOS_Connect_QueryExternalAccountMappings = (EOS_Connect_QueryExternalAccountMappingsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr116, typeof(EOS_Connect_QueryExternalAccountMappingsDelegate));
		IntPtr intPtr117 = getFunctionPointer(libraryHandle, "EOS_Connect_QueryProductUserIdMappings");
		if (intPtr117 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_QueryProductUserIdMappings");
		}
		EOS_Connect_QueryProductUserIdMappings = (EOS_Connect_QueryProductUserIdMappingsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr117, typeof(EOS_Connect_QueryProductUserIdMappingsDelegate));
		IntPtr intPtr118 = getFunctionPointer(libraryHandle, "EOS_Connect_RemoveNotifyAuthExpiration");
		if (intPtr118 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_RemoveNotifyAuthExpiration");
		}
		EOS_Connect_RemoveNotifyAuthExpiration = (EOS_Connect_RemoveNotifyAuthExpirationDelegate)Marshal.GetDelegateForFunctionPointer(intPtr118, typeof(EOS_Connect_RemoveNotifyAuthExpirationDelegate));
		IntPtr intPtr119 = getFunctionPointer(libraryHandle, "EOS_Connect_RemoveNotifyLoginStatusChanged");
		if (intPtr119 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_RemoveNotifyLoginStatusChanged");
		}
		EOS_Connect_RemoveNotifyLoginStatusChanged = (EOS_Connect_RemoveNotifyLoginStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr119, typeof(EOS_Connect_RemoveNotifyLoginStatusChangedDelegate));
		IntPtr intPtr120 = getFunctionPointer(libraryHandle, "EOS_Connect_TransferDeviceIdAccount");
		if (intPtr120 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_TransferDeviceIdAccount");
		}
		EOS_Connect_TransferDeviceIdAccount = (EOS_Connect_TransferDeviceIdAccountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr120, typeof(EOS_Connect_TransferDeviceIdAccountDelegate));
		IntPtr intPtr121 = getFunctionPointer(libraryHandle, "EOS_Connect_UnlinkAccount");
		if (intPtr121 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_UnlinkAccount");
		}
		EOS_Connect_UnlinkAccount = (EOS_Connect_UnlinkAccountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr121, typeof(EOS_Connect_UnlinkAccountDelegate));
		IntPtr intPtr122 = getFunctionPointer(libraryHandle, "EOS_Connect_VerifyIdToken");
		if (intPtr122 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Connect_VerifyIdToken");
		}
		EOS_Connect_VerifyIdToken = (EOS_Connect_VerifyIdTokenDelegate)Marshal.GetDelegateForFunctionPointer(intPtr122, typeof(EOS_Connect_VerifyIdTokenDelegate));
		IntPtr intPtr123 = getFunctionPointer(libraryHandle, "EOS_ContinuanceToken_ToString");
		if (intPtr123 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ContinuanceToken_ToString");
		}
		EOS_ContinuanceToken_ToString = (EOS_ContinuanceToken_ToStringDelegate)Marshal.GetDelegateForFunctionPointer(intPtr123, typeof(EOS_ContinuanceToken_ToStringDelegate));
		IntPtr intPtr124 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_AcceptRequestToJoin");
		if (intPtr124 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_AcceptRequestToJoin");
		}
		EOS_CustomInvites_AcceptRequestToJoin = (EOS_CustomInvites_AcceptRequestToJoinDelegate)Marshal.GetDelegateForFunctionPointer(intPtr124, typeof(EOS_CustomInvites_AcceptRequestToJoinDelegate));
		IntPtr intPtr125 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_AddNotifyCustomInviteAccepted");
		if (intPtr125 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_AddNotifyCustomInviteAccepted");
		}
		EOS_CustomInvites_AddNotifyCustomInviteAccepted = (EOS_CustomInvites_AddNotifyCustomInviteAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr125, typeof(EOS_CustomInvites_AddNotifyCustomInviteAcceptedDelegate));
		IntPtr intPtr126 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_AddNotifyCustomInviteReceived");
		if (intPtr126 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_AddNotifyCustomInviteReceived");
		}
		EOS_CustomInvites_AddNotifyCustomInviteReceived = (EOS_CustomInvites_AddNotifyCustomInviteReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr126, typeof(EOS_CustomInvites_AddNotifyCustomInviteReceivedDelegate));
		IntPtr intPtr127 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_AddNotifyCustomInviteRejected");
		if (intPtr127 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_AddNotifyCustomInviteRejected");
		}
		EOS_CustomInvites_AddNotifyCustomInviteRejected = (EOS_CustomInvites_AddNotifyCustomInviteRejectedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr127, typeof(EOS_CustomInvites_AddNotifyCustomInviteRejectedDelegate));
		IntPtr intPtr128 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_AddNotifyRequestToJoinAccepted");
		if (intPtr128 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_AddNotifyRequestToJoinAccepted");
		}
		EOS_CustomInvites_AddNotifyRequestToJoinAccepted = (EOS_CustomInvites_AddNotifyRequestToJoinAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr128, typeof(EOS_CustomInvites_AddNotifyRequestToJoinAcceptedDelegate));
		IntPtr intPtr129 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_AddNotifyRequestToJoinReceived");
		if (intPtr129 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_AddNotifyRequestToJoinReceived");
		}
		EOS_CustomInvites_AddNotifyRequestToJoinReceived = (EOS_CustomInvites_AddNotifyRequestToJoinReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr129, typeof(EOS_CustomInvites_AddNotifyRequestToJoinReceivedDelegate));
		IntPtr intPtr130 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_AddNotifyRequestToJoinRejected");
		if (intPtr130 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_AddNotifyRequestToJoinRejected");
		}
		EOS_CustomInvites_AddNotifyRequestToJoinRejected = (EOS_CustomInvites_AddNotifyRequestToJoinRejectedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr130, typeof(EOS_CustomInvites_AddNotifyRequestToJoinRejectedDelegate));
		IntPtr intPtr131 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_AddNotifyRequestToJoinResponseReceived");
		if (intPtr131 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_AddNotifyRequestToJoinResponseReceived");
		}
		EOS_CustomInvites_AddNotifyRequestToJoinResponseReceived = (EOS_CustomInvites_AddNotifyRequestToJoinResponseReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr131, typeof(EOS_CustomInvites_AddNotifyRequestToJoinResponseReceivedDelegate));
		IntPtr intPtr132 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_AddNotifySendCustomNativeInviteRequested");
		if (intPtr132 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_AddNotifySendCustomNativeInviteRequested");
		}
		EOS_CustomInvites_AddNotifySendCustomNativeInviteRequested = (EOS_CustomInvites_AddNotifySendCustomNativeInviteRequestedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr132, typeof(EOS_CustomInvites_AddNotifySendCustomNativeInviteRequestedDelegate));
		IntPtr intPtr133 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_FinalizeInvite");
		if (intPtr133 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_FinalizeInvite");
		}
		EOS_CustomInvites_FinalizeInvite = (EOS_CustomInvites_FinalizeInviteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr133, typeof(EOS_CustomInvites_FinalizeInviteDelegate));
		IntPtr intPtr134 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_RejectRequestToJoin");
		if (intPtr134 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_RejectRequestToJoin");
		}
		EOS_CustomInvites_RejectRequestToJoin = (EOS_CustomInvites_RejectRequestToJoinDelegate)Marshal.GetDelegateForFunctionPointer(intPtr134, typeof(EOS_CustomInvites_RejectRequestToJoinDelegate));
		IntPtr intPtr135 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_RemoveNotifyCustomInviteAccepted");
		if (intPtr135 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_RemoveNotifyCustomInviteAccepted");
		}
		EOS_CustomInvites_RemoveNotifyCustomInviteAccepted = (EOS_CustomInvites_RemoveNotifyCustomInviteAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr135, typeof(EOS_CustomInvites_RemoveNotifyCustomInviteAcceptedDelegate));
		IntPtr intPtr136 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_RemoveNotifyCustomInviteReceived");
		if (intPtr136 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_RemoveNotifyCustomInviteReceived");
		}
		EOS_CustomInvites_RemoveNotifyCustomInviteReceived = (EOS_CustomInvites_RemoveNotifyCustomInviteReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr136, typeof(EOS_CustomInvites_RemoveNotifyCustomInviteReceivedDelegate));
		IntPtr intPtr137 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_RemoveNotifyCustomInviteRejected");
		if (intPtr137 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_RemoveNotifyCustomInviteRejected");
		}
		EOS_CustomInvites_RemoveNotifyCustomInviteRejected = (EOS_CustomInvites_RemoveNotifyCustomInviteRejectedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr137, typeof(EOS_CustomInvites_RemoveNotifyCustomInviteRejectedDelegate));
		IntPtr intPtr138 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_RemoveNotifyRequestToJoinAccepted");
		if (intPtr138 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_RemoveNotifyRequestToJoinAccepted");
		}
		EOS_CustomInvites_RemoveNotifyRequestToJoinAccepted = (EOS_CustomInvites_RemoveNotifyRequestToJoinAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr138, typeof(EOS_CustomInvites_RemoveNotifyRequestToJoinAcceptedDelegate));
		IntPtr intPtr139 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_RemoveNotifyRequestToJoinReceived");
		if (intPtr139 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_RemoveNotifyRequestToJoinReceived");
		}
		EOS_CustomInvites_RemoveNotifyRequestToJoinReceived = (EOS_CustomInvites_RemoveNotifyRequestToJoinReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr139, typeof(EOS_CustomInvites_RemoveNotifyRequestToJoinReceivedDelegate));
		IntPtr intPtr140 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_RemoveNotifyRequestToJoinRejected");
		if (intPtr140 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_RemoveNotifyRequestToJoinRejected");
		}
		EOS_CustomInvites_RemoveNotifyRequestToJoinRejected = (EOS_CustomInvites_RemoveNotifyRequestToJoinRejectedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr140, typeof(EOS_CustomInvites_RemoveNotifyRequestToJoinRejectedDelegate));
		IntPtr intPtr141 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceived");
		if (intPtr141 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceived");
		}
		EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceived = (EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr141, typeof(EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceivedDelegate));
		IntPtr intPtr142 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequested");
		if (intPtr142 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequested");
		}
		EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequested = (EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequestedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr142, typeof(EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequestedDelegate));
		IntPtr intPtr143 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_SendCustomInvite");
		if (intPtr143 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_SendCustomInvite");
		}
		EOS_CustomInvites_SendCustomInvite = (EOS_CustomInvites_SendCustomInviteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr143, typeof(EOS_CustomInvites_SendCustomInviteDelegate));
		IntPtr intPtr144 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_SendRequestToJoin");
		if (intPtr144 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_SendRequestToJoin");
		}
		EOS_CustomInvites_SendRequestToJoin = (EOS_CustomInvites_SendRequestToJoinDelegate)Marshal.GetDelegateForFunctionPointer(intPtr144, typeof(EOS_CustomInvites_SendRequestToJoinDelegate));
		IntPtr intPtr145 = getFunctionPointer(libraryHandle, "EOS_CustomInvites_SetCustomInvite");
		if (intPtr145 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_CustomInvites_SetCustomInvite");
		}
		EOS_CustomInvites_SetCustomInvite = (EOS_CustomInvites_SetCustomInviteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr145, typeof(EOS_CustomInvites_SetCustomInviteDelegate));
		IntPtr intPtr146 = getFunctionPointer(libraryHandle, "EOS_EApplicationStatus_ToString");
		if (intPtr146 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_EApplicationStatus_ToString");
		}
		EOS_EApplicationStatus_ToString = (EOS_EApplicationStatus_ToStringDelegate)Marshal.GetDelegateForFunctionPointer(intPtr146, typeof(EOS_EApplicationStatus_ToStringDelegate));
		IntPtr intPtr147 = getFunctionPointer(libraryHandle, "EOS_ENetworkStatus_ToString");
		if (intPtr147 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ENetworkStatus_ToString");
		}
		EOS_ENetworkStatus_ToString = (EOS_ENetworkStatus_ToStringDelegate)Marshal.GetDelegateForFunctionPointer(intPtr147, typeof(EOS_ENetworkStatus_ToStringDelegate));
		IntPtr intPtr148 = getFunctionPointer(libraryHandle, "EOS_EResult_IsOperationComplete");
		if (intPtr148 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_EResult_IsOperationComplete");
		}
		EOS_EResult_IsOperationComplete = (EOS_EResult_IsOperationCompleteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr148, typeof(EOS_EResult_IsOperationCompleteDelegate));
		IntPtr intPtr149 = getFunctionPointer(libraryHandle, "EOS_EResult_ToString");
		if (intPtr149 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_EResult_ToString");
		}
		EOS_EResult_ToString = (EOS_EResult_ToStringDelegate)Marshal.GetDelegateForFunctionPointer(intPtr149, typeof(EOS_EResult_ToStringDelegate));
		IntPtr intPtr150 = getFunctionPointer(libraryHandle, "EOS_Ecom_CatalogItem_Release");
		if (intPtr150 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CatalogItem_Release");
		}
		EOS_Ecom_CatalogItem_Release = (EOS_Ecom_CatalogItem_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr150, typeof(EOS_Ecom_CatalogItem_ReleaseDelegate));
		IntPtr intPtr151 = getFunctionPointer(libraryHandle, "EOS_Ecom_CatalogOffer_Release");
		if (intPtr151 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CatalogOffer_Release");
		}
		EOS_Ecom_CatalogOffer_Release = (EOS_Ecom_CatalogOffer_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr151, typeof(EOS_Ecom_CatalogOffer_ReleaseDelegate));
		IntPtr intPtr152 = getFunctionPointer(libraryHandle, "EOS_Ecom_CatalogRelease_Release");
		if (intPtr152 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CatalogRelease_Release");
		}
		EOS_Ecom_CatalogRelease_Release = (EOS_Ecom_CatalogRelease_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr152, typeof(EOS_Ecom_CatalogRelease_ReleaseDelegate));
		IntPtr intPtr153 = getFunctionPointer(libraryHandle, "EOS_Ecom_Checkout");
		if (intPtr153 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_Checkout");
		}
		EOS_Ecom_Checkout = (EOS_Ecom_CheckoutDelegate)Marshal.GetDelegateForFunctionPointer(intPtr153, typeof(EOS_Ecom_CheckoutDelegate));
		IntPtr intPtr154 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyEntitlementById");
		if (intPtr154 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyEntitlementById");
		}
		EOS_Ecom_CopyEntitlementById = (EOS_Ecom_CopyEntitlementByIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr154, typeof(EOS_Ecom_CopyEntitlementByIdDelegate));
		IntPtr intPtr155 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyEntitlementByIndex");
		if (intPtr155 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyEntitlementByIndex");
		}
		EOS_Ecom_CopyEntitlementByIndex = (EOS_Ecom_CopyEntitlementByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr155, typeof(EOS_Ecom_CopyEntitlementByIndexDelegate));
		IntPtr intPtr156 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyEntitlementByNameAndIndex");
		if (intPtr156 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyEntitlementByNameAndIndex");
		}
		EOS_Ecom_CopyEntitlementByNameAndIndex = (EOS_Ecom_CopyEntitlementByNameAndIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr156, typeof(EOS_Ecom_CopyEntitlementByNameAndIndexDelegate));
		IntPtr intPtr157 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyItemById");
		if (intPtr157 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyItemById");
		}
		EOS_Ecom_CopyItemById = (EOS_Ecom_CopyItemByIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr157, typeof(EOS_Ecom_CopyItemByIdDelegate));
		IntPtr intPtr158 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyItemImageInfoByIndex");
		if (intPtr158 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyItemImageInfoByIndex");
		}
		EOS_Ecom_CopyItemImageInfoByIndex = (EOS_Ecom_CopyItemImageInfoByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr158, typeof(EOS_Ecom_CopyItemImageInfoByIndexDelegate));
		IntPtr intPtr159 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyItemReleaseByIndex");
		if (intPtr159 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyItemReleaseByIndex");
		}
		EOS_Ecom_CopyItemReleaseByIndex = (EOS_Ecom_CopyItemReleaseByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr159, typeof(EOS_Ecom_CopyItemReleaseByIndexDelegate));
		IntPtr intPtr160 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyLastRedeemedEntitlementByIndex");
		if (intPtr160 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyLastRedeemedEntitlementByIndex");
		}
		EOS_Ecom_CopyLastRedeemedEntitlementByIndex = (EOS_Ecom_CopyLastRedeemedEntitlementByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr160, typeof(EOS_Ecom_CopyLastRedeemedEntitlementByIndexDelegate));
		IntPtr intPtr161 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyOfferById");
		if (intPtr161 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyOfferById");
		}
		EOS_Ecom_CopyOfferById = (EOS_Ecom_CopyOfferByIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr161, typeof(EOS_Ecom_CopyOfferByIdDelegate));
		IntPtr intPtr162 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyOfferByIndex");
		if (intPtr162 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyOfferByIndex");
		}
		EOS_Ecom_CopyOfferByIndex = (EOS_Ecom_CopyOfferByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr162, typeof(EOS_Ecom_CopyOfferByIndexDelegate));
		IntPtr intPtr163 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyOfferImageInfoByIndex");
		if (intPtr163 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyOfferImageInfoByIndex");
		}
		EOS_Ecom_CopyOfferImageInfoByIndex = (EOS_Ecom_CopyOfferImageInfoByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr163, typeof(EOS_Ecom_CopyOfferImageInfoByIndexDelegate));
		IntPtr intPtr164 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyOfferItemByIndex");
		if (intPtr164 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyOfferItemByIndex");
		}
		EOS_Ecom_CopyOfferItemByIndex = (EOS_Ecom_CopyOfferItemByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr164, typeof(EOS_Ecom_CopyOfferItemByIndexDelegate));
		IntPtr intPtr165 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyTransactionById");
		if (intPtr165 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyTransactionById");
		}
		EOS_Ecom_CopyTransactionById = (EOS_Ecom_CopyTransactionByIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr165, typeof(EOS_Ecom_CopyTransactionByIdDelegate));
		IntPtr intPtr166 = getFunctionPointer(libraryHandle, "EOS_Ecom_CopyTransactionByIndex");
		if (intPtr166 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_CopyTransactionByIndex");
		}
		EOS_Ecom_CopyTransactionByIndex = (EOS_Ecom_CopyTransactionByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr166, typeof(EOS_Ecom_CopyTransactionByIndexDelegate));
		IntPtr intPtr167 = getFunctionPointer(libraryHandle, "EOS_Ecom_Entitlement_Release");
		if (intPtr167 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_Entitlement_Release");
		}
		EOS_Ecom_Entitlement_Release = (EOS_Ecom_Entitlement_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr167, typeof(EOS_Ecom_Entitlement_ReleaseDelegate));
		IntPtr intPtr168 = getFunctionPointer(libraryHandle, "EOS_Ecom_GetEntitlementsByNameCount");
		if (intPtr168 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_GetEntitlementsByNameCount");
		}
		EOS_Ecom_GetEntitlementsByNameCount = (EOS_Ecom_GetEntitlementsByNameCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr168, typeof(EOS_Ecom_GetEntitlementsByNameCountDelegate));
		IntPtr intPtr169 = getFunctionPointer(libraryHandle, "EOS_Ecom_GetEntitlementsCount");
		if (intPtr169 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_GetEntitlementsCount");
		}
		EOS_Ecom_GetEntitlementsCount = (EOS_Ecom_GetEntitlementsCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr169, typeof(EOS_Ecom_GetEntitlementsCountDelegate));
		IntPtr intPtr170 = getFunctionPointer(libraryHandle, "EOS_Ecom_GetItemImageInfoCount");
		if (intPtr170 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_GetItemImageInfoCount");
		}
		EOS_Ecom_GetItemImageInfoCount = (EOS_Ecom_GetItemImageInfoCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr170, typeof(EOS_Ecom_GetItemImageInfoCountDelegate));
		IntPtr intPtr171 = getFunctionPointer(libraryHandle, "EOS_Ecom_GetItemReleaseCount");
		if (intPtr171 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_GetItemReleaseCount");
		}
		EOS_Ecom_GetItemReleaseCount = (EOS_Ecom_GetItemReleaseCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr171, typeof(EOS_Ecom_GetItemReleaseCountDelegate));
		IntPtr intPtr172 = getFunctionPointer(libraryHandle, "EOS_Ecom_GetLastRedeemedEntitlementsCount");
		if (intPtr172 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_GetLastRedeemedEntitlementsCount");
		}
		EOS_Ecom_GetLastRedeemedEntitlementsCount = (EOS_Ecom_GetLastRedeemedEntitlementsCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr172, typeof(EOS_Ecom_GetLastRedeemedEntitlementsCountDelegate));
		IntPtr intPtr173 = getFunctionPointer(libraryHandle, "EOS_Ecom_GetOfferCount");
		if (intPtr173 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_GetOfferCount");
		}
		EOS_Ecom_GetOfferCount = (EOS_Ecom_GetOfferCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr173, typeof(EOS_Ecom_GetOfferCountDelegate));
		IntPtr intPtr174 = getFunctionPointer(libraryHandle, "EOS_Ecom_GetOfferImageInfoCount");
		if (intPtr174 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_GetOfferImageInfoCount");
		}
		EOS_Ecom_GetOfferImageInfoCount = (EOS_Ecom_GetOfferImageInfoCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr174, typeof(EOS_Ecom_GetOfferImageInfoCountDelegate));
		IntPtr intPtr175 = getFunctionPointer(libraryHandle, "EOS_Ecom_GetOfferItemCount");
		if (intPtr175 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_GetOfferItemCount");
		}
		EOS_Ecom_GetOfferItemCount = (EOS_Ecom_GetOfferItemCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr175, typeof(EOS_Ecom_GetOfferItemCountDelegate));
		IntPtr intPtr176 = getFunctionPointer(libraryHandle, "EOS_Ecom_GetTransactionCount");
		if (intPtr176 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_GetTransactionCount");
		}
		EOS_Ecom_GetTransactionCount = (EOS_Ecom_GetTransactionCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr176, typeof(EOS_Ecom_GetTransactionCountDelegate));
		IntPtr intPtr177 = getFunctionPointer(libraryHandle, "EOS_Ecom_KeyImageInfo_Release");
		if (intPtr177 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_KeyImageInfo_Release");
		}
		EOS_Ecom_KeyImageInfo_Release = (EOS_Ecom_KeyImageInfo_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr177, typeof(EOS_Ecom_KeyImageInfo_ReleaseDelegate));
		IntPtr intPtr178 = getFunctionPointer(libraryHandle, "EOS_Ecom_QueryEntitlementToken");
		if (intPtr178 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_QueryEntitlementToken");
		}
		EOS_Ecom_QueryEntitlementToken = (EOS_Ecom_QueryEntitlementTokenDelegate)Marshal.GetDelegateForFunctionPointer(intPtr178, typeof(EOS_Ecom_QueryEntitlementTokenDelegate));
		IntPtr intPtr179 = getFunctionPointer(libraryHandle, "EOS_Ecom_QueryEntitlements");
		if (intPtr179 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_QueryEntitlements");
		}
		EOS_Ecom_QueryEntitlements = (EOS_Ecom_QueryEntitlementsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr179, typeof(EOS_Ecom_QueryEntitlementsDelegate));
		IntPtr intPtr180 = getFunctionPointer(libraryHandle, "EOS_Ecom_QueryOffers");
		if (intPtr180 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_QueryOffers");
		}
		EOS_Ecom_QueryOffers = (EOS_Ecom_QueryOffersDelegate)Marshal.GetDelegateForFunctionPointer(intPtr180, typeof(EOS_Ecom_QueryOffersDelegate));
		IntPtr intPtr181 = getFunctionPointer(libraryHandle, "EOS_Ecom_QueryOwnership");
		if (intPtr181 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_QueryOwnership");
		}
		EOS_Ecom_QueryOwnership = (EOS_Ecom_QueryOwnershipDelegate)Marshal.GetDelegateForFunctionPointer(intPtr181, typeof(EOS_Ecom_QueryOwnershipDelegate));
		IntPtr intPtr182 = getFunctionPointer(libraryHandle, "EOS_Ecom_QueryOwnershipBySandboxIds");
		if (intPtr182 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_QueryOwnershipBySandboxIds");
		}
		EOS_Ecom_QueryOwnershipBySandboxIds = (EOS_Ecom_QueryOwnershipBySandboxIdsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr182, typeof(EOS_Ecom_QueryOwnershipBySandboxIdsDelegate));
		IntPtr intPtr183 = getFunctionPointer(libraryHandle, "EOS_Ecom_QueryOwnershipToken");
		if (intPtr183 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_QueryOwnershipToken");
		}
		EOS_Ecom_QueryOwnershipToken = (EOS_Ecom_QueryOwnershipTokenDelegate)Marshal.GetDelegateForFunctionPointer(intPtr183, typeof(EOS_Ecom_QueryOwnershipTokenDelegate));
		IntPtr intPtr184 = getFunctionPointer(libraryHandle, "EOS_Ecom_RedeemEntitlements");
		if (intPtr184 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_RedeemEntitlements");
		}
		EOS_Ecom_RedeemEntitlements = (EOS_Ecom_RedeemEntitlementsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr184, typeof(EOS_Ecom_RedeemEntitlementsDelegate));
		IntPtr intPtr185 = getFunctionPointer(libraryHandle, "EOS_Ecom_Transaction_CopyEntitlementByIndex");
		if (intPtr185 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_Transaction_CopyEntitlementByIndex");
		}
		EOS_Ecom_Transaction_CopyEntitlementByIndex = (EOS_Ecom_Transaction_CopyEntitlementByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr185, typeof(EOS_Ecom_Transaction_CopyEntitlementByIndexDelegate));
		IntPtr intPtr186 = getFunctionPointer(libraryHandle, "EOS_Ecom_Transaction_GetEntitlementsCount");
		if (intPtr186 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_Transaction_GetEntitlementsCount");
		}
		EOS_Ecom_Transaction_GetEntitlementsCount = (EOS_Ecom_Transaction_GetEntitlementsCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr186, typeof(EOS_Ecom_Transaction_GetEntitlementsCountDelegate));
		IntPtr intPtr187 = getFunctionPointer(libraryHandle, "EOS_Ecom_Transaction_GetTransactionId");
		if (intPtr187 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_Transaction_GetTransactionId");
		}
		EOS_Ecom_Transaction_GetTransactionId = (EOS_Ecom_Transaction_GetTransactionIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr187, typeof(EOS_Ecom_Transaction_GetTransactionIdDelegate));
		IntPtr intPtr188 = getFunctionPointer(libraryHandle, "EOS_Ecom_Transaction_Release");
		if (intPtr188 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Ecom_Transaction_Release");
		}
		EOS_Ecom_Transaction_Release = (EOS_Ecom_Transaction_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr188, typeof(EOS_Ecom_Transaction_ReleaseDelegate));
		IntPtr intPtr189 = getFunctionPointer(libraryHandle, "EOS_EpicAccountId_FromString");
		if (intPtr189 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_EpicAccountId_FromString");
		}
		EOS_EpicAccountId_FromString = (EOS_EpicAccountId_FromStringDelegate)Marshal.GetDelegateForFunctionPointer(intPtr189, typeof(EOS_EpicAccountId_FromStringDelegate));
		IntPtr intPtr190 = getFunctionPointer(libraryHandle, "EOS_EpicAccountId_IsValid");
		if (intPtr190 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_EpicAccountId_IsValid");
		}
		EOS_EpicAccountId_IsValid = (EOS_EpicAccountId_IsValidDelegate)Marshal.GetDelegateForFunctionPointer(intPtr190, typeof(EOS_EpicAccountId_IsValidDelegate));
		IntPtr intPtr191 = getFunctionPointer(libraryHandle, "EOS_EpicAccountId_ToString");
		if (intPtr191 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_EpicAccountId_ToString");
		}
		EOS_EpicAccountId_ToString = (EOS_EpicAccountId_ToStringDelegate)Marshal.GetDelegateForFunctionPointer(intPtr191, typeof(EOS_EpicAccountId_ToStringDelegate));
		IntPtr intPtr192 = getFunctionPointer(libraryHandle, "EOS_Friends_AcceptInvite");
		if (intPtr192 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_AcceptInvite");
		}
		EOS_Friends_AcceptInvite = (EOS_Friends_AcceptInviteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr192, typeof(EOS_Friends_AcceptInviteDelegate));
		IntPtr intPtr193 = getFunctionPointer(libraryHandle, "EOS_Friends_AddNotifyBlockedUsersUpdate");
		if (intPtr193 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_AddNotifyBlockedUsersUpdate");
		}
		EOS_Friends_AddNotifyBlockedUsersUpdate = (EOS_Friends_AddNotifyBlockedUsersUpdateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr193, typeof(EOS_Friends_AddNotifyBlockedUsersUpdateDelegate));
		IntPtr intPtr194 = getFunctionPointer(libraryHandle, "EOS_Friends_AddNotifyFriendsUpdate");
		if (intPtr194 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_AddNotifyFriendsUpdate");
		}
		EOS_Friends_AddNotifyFriendsUpdate = (EOS_Friends_AddNotifyFriendsUpdateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr194, typeof(EOS_Friends_AddNotifyFriendsUpdateDelegate));
		IntPtr intPtr195 = getFunctionPointer(libraryHandle, "EOS_Friends_GetBlockedUserAtIndex");
		if (intPtr195 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_GetBlockedUserAtIndex");
		}
		EOS_Friends_GetBlockedUserAtIndex = (EOS_Friends_GetBlockedUserAtIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr195, typeof(EOS_Friends_GetBlockedUserAtIndexDelegate));
		IntPtr intPtr196 = getFunctionPointer(libraryHandle, "EOS_Friends_GetBlockedUsersCount");
		if (intPtr196 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_GetBlockedUsersCount");
		}
		EOS_Friends_GetBlockedUsersCount = (EOS_Friends_GetBlockedUsersCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr196, typeof(EOS_Friends_GetBlockedUsersCountDelegate));
		IntPtr intPtr197 = getFunctionPointer(libraryHandle, "EOS_Friends_GetFriendAtIndex");
		if (intPtr197 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_GetFriendAtIndex");
		}
		EOS_Friends_GetFriendAtIndex = (EOS_Friends_GetFriendAtIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr197, typeof(EOS_Friends_GetFriendAtIndexDelegate));
		IntPtr intPtr198 = getFunctionPointer(libraryHandle, "EOS_Friends_GetFriendsCount");
		if (intPtr198 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_GetFriendsCount");
		}
		EOS_Friends_GetFriendsCount = (EOS_Friends_GetFriendsCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr198, typeof(EOS_Friends_GetFriendsCountDelegate));
		IntPtr intPtr199 = getFunctionPointer(libraryHandle, "EOS_Friends_GetStatus");
		if (intPtr199 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_GetStatus");
		}
		EOS_Friends_GetStatus = (EOS_Friends_GetStatusDelegate)Marshal.GetDelegateForFunctionPointer(intPtr199, typeof(EOS_Friends_GetStatusDelegate));
		IntPtr intPtr200 = getFunctionPointer(libraryHandle, "EOS_Friends_QueryFriends");
		if (intPtr200 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_QueryFriends");
		}
		EOS_Friends_QueryFriends = (EOS_Friends_QueryFriendsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr200, typeof(EOS_Friends_QueryFriendsDelegate));
		IntPtr intPtr201 = getFunctionPointer(libraryHandle, "EOS_Friends_RejectInvite");
		if (intPtr201 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_RejectInvite");
		}
		EOS_Friends_RejectInvite = (EOS_Friends_RejectInviteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr201, typeof(EOS_Friends_RejectInviteDelegate));
		IntPtr intPtr202 = getFunctionPointer(libraryHandle, "EOS_Friends_RemoveNotifyBlockedUsersUpdate");
		if (intPtr202 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_RemoveNotifyBlockedUsersUpdate");
		}
		EOS_Friends_RemoveNotifyBlockedUsersUpdate = (EOS_Friends_RemoveNotifyBlockedUsersUpdateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr202, typeof(EOS_Friends_RemoveNotifyBlockedUsersUpdateDelegate));
		IntPtr intPtr203 = getFunctionPointer(libraryHandle, "EOS_Friends_RemoveNotifyFriendsUpdate");
		if (intPtr203 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_RemoveNotifyFriendsUpdate");
		}
		EOS_Friends_RemoveNotifyFriendsUpdate = (EOS_Friends_RemoveNotifyFriendsUpdateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr203, typeof(EOS_Friends_RemoveNotifyFriendsUpdateDelegate));
		IntPtr intPtr204 = getFunctionPointer(libraryHandle, "EOS_Friends_SendInvite");
		if (intPtr204 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Friends_SendInvite");
		}
		EOS_Friends_SendInvite = (EOS_Friends_SendInviteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr204, typeof(EOS_Friends_SendInviteDelegate));
		IntPtr intPtr205 = getFunctionPointer(libraryHandle, "EOS_GetVersion");
		if (intPtr205 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_GetVersion");
		}
		EOS_GetVersion = (EOS_GetVersionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr205, typeof(EOS_GetVersionDelegate));
		IntPtr intPtr206 = getFunctionPointer(libraryHandle, "EOS_Initialize");
		if (intPtr206 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Initialize");
		}
		EOS_Initialize = (EOS_InitializeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr206, typeof(EOS_InitializeDelegate));
		IntPtr intPtr207 = getFunctionPointer(libraryHandle, "EOS_IntegratedPlatformOptionsContainer_Add");
		if (intPtr207 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_IntegratedPlatformOptionsContainer_Add");
		}
		EOS_IntegratedPlatformOptionsContainer_Add = (EOS_IntegratedPlatformOptionsContainer_AddDelegate)Marshal.GetDelegateForFunctionPointer(intPtr207, typeof(EOS_IntegratedPlatformOptionsContainer_AddDelegate));
		IntPtr intPtr208 = getFunctionPointer(libraryHandle, "EOS_IntegratedPlatformOptionsContainer_Release");
		if (intPtr208 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_IntegratedPlatformOptionsContainer_Release");
		}
		EOS_IntegratedPlatformOptionsContainer_Release = (EOS_IntegratedPlatformOptionsContainer_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr208, typeof(EOS_IntegratedPlatformOptionsContainer_ReleaseDelegate));
		IntPtr intPtr209 = getFunctionPointer(libraryHandle, "EOS_IntegratedPlatform_AddNotifyUserLoginStatusChanged");
		if (intPtr209 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_IntegratedPlatform_AddNotifyUserLoginStatusChanged");
		}
		EOS_IntegratedPlatform_AddNotifyUserLoginStatusChanged = (EOS_IntegratedPlatform_AddNotifyUserLoginStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr209, typeof(EOS_IntegratedPlatform_AddNotifyUserLoginStatusChangedDelegate));
		IntPtr intPtr210 = getFunctionPointer(libraryHandle, "EOS_IntegratedPlatform_ClearUserPreLogoutCallback");
		if (intPtr210 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_IntegratedPlatform_ClearUserPreLogoutCallback");
		}
		EOS_IntegratedPlatform_ClearUserPreLogoutCallback = (EOS_IntegratedPlatform_ClearUserPreLogoutCallbackDelegate)Marshal.GetDelegateForFunctionPointer(intPtr210, typeof(EOS_IntegratedPlatform_ClearUserPreLogoutCallbackDelegate));
		IntPtr intPtr211 = getFunctionPointer(libraryHandle, "EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainer");
		if (intPtr211 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainer");
		}
		EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainer = (EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr211, typeof(EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainerDelegate));
		IntPtr intPtr212 = getFunctionPointer(libraryHandle, "EOS_IntegratedPlatform_FinalizeDeferredUserLogout");
		if (intPtr212 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_IntegratedPlatform_FinalizeDeferredUserLogout");
		}
		EOS_IntegratedPlatform_FinalizeDeferredUserLogout = (EOS_IntegratedPlatform_FinalizeDeferredUserLogoutDelegate)Marshal.GetDelegateForFunctionPointer(intPtr212, typeof(EOS_IntegratedPlatform_FinalizeDeferredUserLogoutDelegate));
		IntPtr intPtr213 = getFunctionPointer(libraryHandle, "EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChanged");
		if (intPtr213 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChanged");
		}
		EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChanged = (EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr213, typeof(EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChangedDelegate));
		IntPtr intPtr214 = getFunctionPointer(libraryHandle, "EOS_IntegratedPlatform_SetUserLoginStatus");
		if (intPtr214 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_IntegratedPlatform_SetUserLoginStatus");
		}
		EOS_IntegratedPlatform_SetUserLoginStatus = (EOS_IntegratedPlatform_SetUserLoginStatusDelegate)Marshal.GetDelegateForFunctionPointer(intPtr214, typeof(EOS_IntegratedPlatform_SetUserLoginStatusDelegate));
		IntPtr intPtr215 = getFunctionPointer(libraryHandle, "EOS_IntegratedPlatform_SetUserPreLogoutCallback");
		if (intPtr215 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_IntegratedPlatform_SetUserPreLogoutCallback");
		}
		EOS_IntegratedPlatform_SetUserPreLogoutCallback = (EOS_IntegratedPlatform_SetUserPreLogoutCallbackDelegate)Marshal.GetDelegateForFunctionPointer(intPtr215, typeof(EOS_IntegratedPlatform_SetUserPreLogoutCallbackDelegate));
		IntPtr intPtr216 = getFunctionPointer(libraryHandle, "EOS_KWS_AddNotifyPermissionsUpdateReceived");
		if (intPtr216 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_KWS_AddNotifyPermissionsUpdateReceived");
		}
		EOS_KWS_AddNotifyPermissionsUpdateReceived = (EOS_KWS_AddNotifyPermissionsUpdateReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr216, typeof(EOS_KWS_AddNotifyPermissionsUpdateReceivedDelegate));
		IntPtr intPtr217 = getFunctionPointer(libraryHandle, "EOS_KWS_CopyPermissionByIndex");
		if (intPtr217 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_KWS_CopyPermissionByIndex");
		}
		EOS_KWS_CopyPermissionByIndex = (EOS_KWS_CopyPermissionByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr217, typeof(EOS_KWS_CopyPermissionByIndexDelegate));
		IntPtr intPtr218 = getFunctionPointer(libraryHandle, "EOS_KWS_CreateUser");
		if (intPtr218 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_KWS_CreateUser");
		}
		EOS_KWS_CreateUser = (EOS_KWS_CreateUserDelegate)Marshal.GetDelegateForFunctionPointer(intPtr218, typeof(EOS_KWS_CreateUserDelegate));
		IntPtr intPtr219 = getFunctionPointer(libraryHandle, "EOS_KWS_GetPermissionByKey");
		if (intPtr219 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_KWS_GetPermissionByKey");
		}
		EOS_KWS_GetPermissionByKey = (EOS_KWS_GetPermissionByKeyDelegate)Marshal.GetDelegateForFunctionPointer(intPtr219, typeof(EOS_KWS_GetPermissionByKeyDelegate));
		IntPtr intPtr220 = getFunctionPointer(libraryHandle, "EOS_KWS_GetPermissionsCount");
		if (intPtr220 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_KWS_GetPermissionsCount");
		}
		EOS_KWS_GetPermissionsCount = (EOS_KWS_GetPermissionsCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr220, typeof(EOS_KWS_GetPermissionsCountDelegate));
		IntPtr intPtr221 = getFunctionPointer(libraryHandle, "EOS_KWS_PermissionStatus_Release");
		if (intPtr221 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_KWS_PermissionStatus_Release");
		}
		EOS_KWS_PermissionStatus_Release = (EOS_KWS_PermissionStatus_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr221, typeof(EOS_KWS_PermissionStatus_ReleaseDelegate));
		IntPtr intPtr222 = getFunctionPointer(libraryHandle, "EOS_KWS_QueryAgeGate");
		if (intPtr222 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_KWS_QueryAgeGate");
		}
		EOS_KWS_QueryAgeGate = (EOS_KWS_QueryAgeGateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr222, typeof(EOS_KWS_QueryAgeGateDelegate));
		IntPtr intPtr223 = getFunctionPointer(libraryHandle, "EOS_KWS_QueryPermissions");
		if (intPtr223 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_KWS_QueryPermissions");
		}
		EOS_KWS_QueryPermissions = (EOS_KWS_QueryPermissionsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr223, typeof(EOS_KWS_QueryPermissionsDelegate));
		IntPtr intPtr224 = getFunctionPointer(libraryHandle, "EOS_KWS_RemoveNotifyPermissionsUpdateReceived");
		if (intPtr224 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_KWS_RemoveNotifyPermissionsUpdateReceived");
		}
		EOS_KWS_RemoveNotifyPermissionsUpdateReceived = (EOS_KWS_RemoveNotifyPermissionsUpdateReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr224, typeof(EOS_KWS_RemoveNotifyPermissionsUpdateReceivedDelegate));
		IntPtr intPtr225 = getFunctionPointer(libraryHandle, "EOS_KWS_RequestPermissions");
		if (intPtr225 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_KWS_RequestPermissions");
		}
		EOS_KWS_RequestPermissions = (EOS_KWS_RequestPermissionsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr225, typeof(EOS_KWS_RequestPermissionsDelegate));
		IntPtr intPtr226 = getFunctionPointer(libraryHandle, "EOS_KWS_UpdateParentEmail");
		if (intPtr226 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_KWS_UpdateParentEmail");
		}
		EOS_KWS_UpdateParentEmail = (EOS_KWS_UpdateParentEmailDelegate)Marshal.GetDelegateForFunctionPointer(intPtr226, typeof(EOS_KWS_UpdateParentEmailDelegate));
		IntPtr intPtr227 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_CopyLeaderboardDefinitionByIndex");
		if (intPtr227 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_CopyLeaderboardDefinitionByIndex");
		}
		EOS_Leaderboards_CopyLeaderboardDefinitionByIndex = (EOS_Leaderboards_CopyLeaderboardDefinitionByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr227, typeof(EOS_Leaderboards_CopyLeaderboardDefinitionByIndexDelegate));
		IntPtr intPtr228 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardId");
		if (intPtr228 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardId");
		}
		EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardId = (EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr228, typeof(EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardIdDelegate));
		IntPtr intPtr229 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_CopyLeaderboardRecordByIndex");
		if (intPtr229 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_CopyLeaderboardRecordByIndex");
		}
		EOS_Leaderboards_CopyLeaderboardRecordByIndex = (EOS_Leaderboards_CopyLeaderboardRecordByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr229, typeof(EOS_Leaderboards_CopyLeaderboardRecordByIndexDelegate));
		IntPtr intPtr230 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_CopyLeaderboardRecordByUserId");
		if (intPtr230 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_CopyLeaderboardRecordByUserId");
		}
		EOS_Leaderboards_CopyLeaderboardRecordByUserId = (EOS_Leaderboards_CopyLeaderboardRecordByUserIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr230, typeof(EOS_Leaderboards_CopyLeaderboardRecordByUserIdDelegate));
		IntPtr intPtr231 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_CopyLeaderboardUserScoreByIndex");
		if (intPtr231 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_CopyLeaderboardUserScoreByIndex");
		}
		EOS_Leaderboards_CopyLeaderboardUserScoreByIndex = (EOS_Leaderboards_CopyLeaderboardUserScoreByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr231, typeof(EOS_Leaderboards_CopyLeaderboardUserScoreByIndexDelegate));
		IntPtr intPtr232 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_CopyLeaderboardUserScoreByUserId");
		if (intPtr232 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_CopyLeaderboardUserScoreByUserId");
		}
		EOS_Leaderboards_CopyLeaderboardUserScoreByUserId = (EOS_Leaderboards_CopyLeaderboardUserScoreByUserIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr232, typeof(EOS_Leaderboards_CopyLeaderboardUserScoreByUserIdDelegate));
		IntPtr intPtr233 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_Definition_Release");
		if (intPtr233 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_Definition_Release");
		}
		EOS_Leaderboards_Definition_Release = (EOS_Leaderboards_Definition_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr233, typeof(EOS_Leaderboards_Definition_ReleaseDelegate));
		IntPtr intPtr234 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_GetLeaderboardDefinitionCount");
		if (intPtr234 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_GetLeaderboardDefinitionCount");
		}
		EOS_Leaderboards_GetLeaderboardDefinitionCount = (EOS_Leaderboards_GetLeaderboardDefinitionCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr234, typeof(EOS_Leaderboards_GetLeaderboardDefinitionCountDelegate));
		IntPtr intPtr235 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_GetLeaderboardRecordCount");
		if (intPtr235 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_GetLeaderboardRecordCount");
		}
		EOS_Leaderboards_GetLeaderboardRecordCount = (EOS_Leaderboards_GetLeaderboardRecordCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr235, typeof(EOS_Leaderboards_GetLeaderboardRecordCountDelegate));
		IntPtr intPtr236 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_GetLeaderboardUserScoreCount");
		if (intPtr236 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_GetLeaderboardUserScoreCount");
		}
		EOS_Leaderboards_GetLeaderboardUserScoreCount = (EOS_Leaderboards_GetLeaderboardUserScoreCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr236, typeof(EOS_Leaderboards_GetLeaderboardUserScoreCountDelegate));
		IntPtr intPtr237 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_LeaderboardDefinition_Release");
		if (intPtr237 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_LeaderboardDefinition_Release");
		}
		EOS_Leaderboards_LeaderboardDefinition_Release = (EOS_Leaderboards_LeaderboardDefinition_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr237, typeof(EOS_Leaderboards_LeaderboardDefinition_ReleaseDelegate));
		IntPtr intPtr238 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_LeaderboardRecord_Release");
		if (intPtr238 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_LeaderboardRecord_Release");
		}
		EOS_Leaderboards_LeaderboardRecord_Release = (EOS_Leaderboards_LeaderboardRecord_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr238, typeof(EOS_Leaderboards_LeaderboardRecord_ReleaseDelegate));
		IntPtr intPtr239 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_LeaderboardUserScore_Release");
		if (intPtr239 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_LeaderboardUserScore_Release");
		}
		EOS_Leaderboards_LeaderboardUserScore_Release = (EOS_Leaderboards_LeaderboardUserScore_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr239, typeof(EOS_Leaderboards_LeaderboardUserScore_ReleaseDelegate));
		IntPtr intPtr240 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_QueryLeaderboardDefinitions");
		if (intPtr240 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_QueryLeaderboardDefinitions");
		}
		EOS_Leaderboards_QueryLeaderboardDefinitions = (EOS_Leaderboards_QueryLeaderboardDefinitionsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr240, typeof(EOS_Leaderboards_QueryLeaderboardDefinitionsDelegate));
		IntPtr intPtr241 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_QueryLeaderboardRanks");
		if (intPtr241 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_QueryLeaderboardRanks");
		}
		EOS_Leaderboards_QueryLeaderboardRanks = (EOS_Leaderboards_QueryLeaderboardRanksDelegate)Marshal.GetDelegateForFunctionPointer(intPtr241, typeof(EOS_Leaderboards_QueryLeaderboardRanksDelegate));
		IntPtr intPtr242 = getFunctionPointer(libraryHandle, "EOS_Leaderboards_QueryLeaderboardUserScores");
		if (intPtr242 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Leaderboards_QueryLeaderboardUserScores");
		}
		EOS_Leaderboards_QueryLeaderboardUserScores = (EOS_Leaderboards_QueryLeaderboardUserScoresDelegate)Marshal.GetDelegateForFunctionPointer(intPtr242, typeof(EOS_Leaderboards_QueryLeaderboardUserScoresDelegate));
		IntPtr intPtr243 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_CopyAttributeByIndex");
		if (intPtr243 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_CopyAttributeByIndex");
		}
		EOS_LobbyDetails_CopyAttributeByIndex = (EOS_LobbyDetails_CopyAttributeByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr243, typeof(EOS_LobbyDetails_CopyAttributeByIndexDelegate));
		IntPtr intPtr244 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_CopyAttributeByKey");
		if (intPtr244 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_CopyAttributeByKey");
		}
		EOS_LobbyDetails_CopyAttributeByKey = (EOS_LobbyDetails_CopyAttributeByKeyDelegate)Marshal.GetDelegateForFunctionPointer(intPtr244, typeof(EOS_LobbyDetails_CopyAttributeByKeyDelegate));
		IntPtr intPtr245 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_CopyInfo");
		if (intPtr245 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_CopyInfo");
		}
		EOS_LobbyDetails_CopyInfo = (EOS_LobbyDetails_CopyInfoDelegate)Marshal.GetDelegateForFunctionPointer(intPtr245, typeof(EOS_LobbyDetails_CopyInfoDelegate));
		IntPtr intPtr246 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_CopyMemberAttributeByIndex");
		if (intPtr246 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_CopyMemberAttributeByIndex");
		}
		EOS_LobbyDetails_CopyMemberAttributeByIndex = (EOS_LobbyDetails_CopyMemberAttributeByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr246, typeof(EOS_LobbyDetails_CopyMemberAttributeByIndexDelegate));
		IntPtr intPtr247 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_CopyMemberAttributeByKey");
		if (intPtr247 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_CopyMemberAttributeByKey");
		}
		EOS_LobbyDetails_CopyMemberAttributeByKey = (EOS_LobbyDetails_CopyMemberAttributeByKeyDelegate)Marshal.GetDelegateForFunctionPointer(intPtr247, typeof(EOS_LobbyDetails_CopyMemberAttributeByKeyDelegate));
		IntPtr intPtr248 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_CopyMemberInfo");
		if (intPtr248 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_CopyMemberInfo");
		}
		EOS_LobbyDetails_CopyMemberInfo = (EOS_LobbyDetails_CopyMemberInfoDelegate)Marshal.GetDelegateForFunctionPointer(intPtr248, typeof(EOS_LobbyDetails_CopyMemberInfoDelegate));
		IntPtr intPtr249 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_GetAttributeCount");
		if (intPtr249 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_GetAttributeCount");
		}
		EOS_LobbyDetails_GetAttributeCount = (EOS_LobbyDetails_GetAttributeCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr249, typeof(EOS_LobbyDetails_GetAttributeCountDelegate));
		IntPtr intPtr250 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_GetLobbyOwner");
		if (intPtr250 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_GetLobbyOwner");
		}
		EOS_LobbyDetails_GetLobbyOwner = (EOS_LobbyDetails_GetLobbyOwnerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr250, typeof(EOS_LobbyDetails_GetLobbyOwnerDelegate));
		IntPtr intPtr251 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_GetMemberAttributeCount");
		if (intPtr251 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_GetMemberAttributeCount");
		}
		EOS_LobbyDetails_GetMemberAttributeCount = (EOS_LobbyDetails_GetMemberAttributeCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr251, typeof(EOS_LobbyDetails_GetMemberAttributeCountDelegate));
		IntPtr intPtr252 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_GetMemberByIndex");
		if (intPtr252 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_GetMemberByIndex");
		}
		EOS_LobbyDetails_GetMemberByIndex = (EOS_LobbyDetails_GetMemberByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr252, typeof(EOS_LobbyDetails_GetMemberByIndexDelegate));
		IntPtr intPtr253 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_GetMemberCount");
		if (intPtr253 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_GetMemberCount");
		}
		EOS_LobbyDetails_GetMemberCount = (EOS_LobbyDetails_GetMemberCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr253, typeof(EOS_LobbyDetails_GetMemberCountDelegate));
		IntPtr intPtr254 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_Info_Release");
		if (intPtr254 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_Info_Release");
		}
		EOS_LobbyDetails_Info_Release = (EOS_LobbyDetails_Info_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr254, typeof(EOS_LobbyDetails_Info_ReleaseDelegate));
		IntPtr intPtr255 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_MemberInfo_Release");
		if (intPtr255 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_MemberInfo_Release");
		}
		EOS_LobbyDetails_MemberInfo_Release = (EOS_LobbyDetails_MemberInfo_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr255, typeof(EOS_LobbyDetails_MemberInfo_ReleaseDelegate));
		IntPtr intPtr256 = getFunctionPointer(libraryHandle, "EOS_LobbyDetails_Release");
		if (intPtr256 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyDetails_Release");
		}
		EOS_LobbyDetails_Release = (EOS_LobbyDetails_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr256, typeof(EOS_LobbyDetails_ReleaseDelegate));
		IntPtr intPtr257 = getFunctionPointer(libraryHandle, "EOS_LobbyModification_AddAttribute");
		if (intPtr257 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyModification_AddAttribute");
		}
		EOS_LobbyModification_AddAttribute = (EOS_LobbyModification_AddAttributeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr257, typeof(EOS_LobbyModification_AddAttributeDelegate));
		IntPtr intPtr258 = getFunctionPointer(libraryHandle, "EOS_LobbyModification_AddMemberAttribute");
		if (intPtr258 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyModification_AddMemberAttribute");
		}
		EOS_LobbyModification_AddMemberAttribute = (EOS_LobbyModification_AddMemberAttributeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr258, typeof(EOS_LobbyModification_AddMemberAttributeDelegate));
		IntPtr intPtr259 = getFunctionPointer(libraryHandle, "EOS_LobbyModification_Release");
		if (intPtr259 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyModification_Release");
		}
		EOS_LobbyModification_Release = (EOS_LobbyModification_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr259, typeof(EOS_LobbyModification_ReleaseDelegate));
		IntPtr intPtr260 = getFunctionPointer(libraryHandle, "EOS_LobbyModification_RemoveAttribute");
		if (intPtr260 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyModification_RemoveAttribute");
		}
		EOS_LobbyModification_RemoveAttribute = (EOS_LobbyModification_RemoveAttributeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr260, typeof(EOS_LobbyModification_RemoveAttributeDelegate));
		IntPtr intPtr261 = getFunctionPointer(libraryHandle, "EOS_LobbyModification_RemoveMemberAttribute");
		if (intPtr261 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyModification_RemoveMemberAttribute");
		}
		EOS_LobbyModification_RemoveMemberAttribute = (EOS_LobbyModification_RemoveMemberAttributeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr261, typeof(EOS_LobbyModification_RemoveMemberAttributeDelegate));
		IntPtr intPtr262 = getFunctionPointer(libraryHandle, "EOS_LobbyModification_SetAllowedPlatformIds");
		if (intPtr262 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyModification_SetAllowedPlatformIds");
		}
		EOS_LobbyModification_SetAllowedPlatformIds = (EOS_LobbyModification_SetAllowedPlatformIdsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr262, typeof(EOS_LobbyModification_SetAllowedPlatformIdsDelegate));
		IntPtr intPtr263 = getFunctionPointer(libraryHandle, "EOS_LobbyModification_SetBucketId");
		if (intPtr263 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyModification_SetBucketId");
		}
		EOS_LobbyModification_SetBucketId = (EOS_LobbyModification_SetBucketIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr263, typeof(EOS_LobbyModification_SetBucketIdDelegate));
		IntPtr intPtr264 = getFunctionPointer(libraryHandle, "EOS_LobbyModification_SetInvitesAllowed");
		if (intPtr264 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyModification_SetInvitesAllowed");
		}
		EOS_LobbyModification_SetInvitesAllowed = (EOS_LobbyModification_SetInvitesAllowedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr264, typeof(EOS_LobbyModification_SetInvitesAllowedDelegate));
		IntPtr intPtr265 = getFunctionPointer(libraryHandle, "EOS_LobbyModification_SetMaxMembers");
		if (intPtr265 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyModification_SetMaxMembers");
		}
		EOS_LobbyModification_SetMaxMembers = (EOS_LobbyModification_SetMaxMembersDelegate)Marshal.GetDelegateForFunctionPointer(intPtr265, typeof(EOS_LobbyModification_SetMaxMembersDelegate));
		IntPtr intPtr266 = getFunctionPointer(libraryHandle, "EOS_LobbyModification_SetPermissionLevel");
		if (intPtr266 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbyModification_SetPermissionLevel");
		}
		EOS_LobbyModification_SetPermissionLevel = (EOS_LobbyModification_SetPermissionLevelDelegate)Marshal.GetDelegateForFunctionPointer(intPtr266, typeof(EOS_LobbyModification_SetPermissionLevelDelegate));
		IntPtr intPtr267 = getFunctionPointer(libraryHandle, "EOS_LobbySearch_CopySearchResultByIndex");
		if (intPtr267 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbySearch_CopySearchResultByIndex");
		}
		EOS_LobbySearch_CopySearchResultByIndex = (EOS_LobbySearch_CopySearchResultByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr267, typeof(EOS_LobbySearch_CopySearchResultByIndexDelegate));
		IntPtr intPtr268 = getFunctionPointer(libraryHandle, "EOS_LobbySearch_Find");
		if (intPtr268 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbySearch_Find");
		}
		EOS_LobbySearch_Find = (EOS_LobbySearch_FindDelegate)Marshal.GetDelegateForFunctionPointer(intPtr268, typeof(EOS_LobbySearch_FindDelegate));
		IntPtr intPtr269 = getFunctionPointer(libraryHandle, "EOS_LobbySearch_GetSearchResultCount");
		if (intPtr269 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbySearch_GetSearchResultCount");
		}
		EOS_LobbySearch_GetSearchResultCount = (EOS_LobbySearch_GetSearchResultCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr269, typeof(EOS_LobbySearch_GetSearchResultCountDelegate));
		IntPtr intPtr270 = getFunctionPointer(libraryHandle, "EOS_LobbySearch_Release");
		if (intPtr270 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbySearch_Release");
		}
		EOS_LobbySearch_Release = (EOS_LobbySearch_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr270, typeof(EOS_LobbySearch_ReleaseDelegate));
		IntPtr intPtr271 = getFunctionPointer(libraryHandle, "EOS_LobbySearch_RemoveParameter");
		if (intPtr271 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbySearch_RemoveParameter");
		}
		EOS_LobbySearch_RemoveParameter = (EOS_LobbySearch_RemoveParameterDelegate)Marshal.GetDelegateForFunctionPointer(intPtr271, typeof(EOS_LobbySearch_RemoveParameterDelegate));
		IntPtr intPtr272 = getFunctionPointer(libraryHandle, "EOS_LobbySearch_SetLobbyId");
		if (intPtr272 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbySearch_SetLobbyId");
		}
		EOS_LobbySearch_SetLobbyId = (EOS_LobbySearch_SetLobbyIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr272, typeof(EOS_LobbySearch_SetLobbyIdDelegate));
		IntPtr intPtr273 = getFunctionPointer(libraryHandle, "EOS_LobbySearch_SetMaxResults");
		if (intPtr273 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbySearch_SetMaxResults");
		}
		EOS_LobbySearch_SetMaxResults = (EOS_LobbySearch_SetMaxResultsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr273, typeof(EOS_LobbySearch_SetMaxResultsDelegate));
		IntPtr intPtr274 = getFunctionPointer(libraryHandle, "EOS_LobbySearch_SetParameter");
		if (intPtr274 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbySearch_SetParameter");
		}
		EOS_LobbySearch_SetParameter = (EOS_LobbySearch_SetParameterDelegate)Marshal.GetDelegateForFunctionPointer(intPtr274, typeof(EOS_LobbySearch_SetParameterDelegate));
		IntPtr intPtr275 = getFunctionPointer(libraryHandle, "EOS_LobbySearch_SetTargetUserId");
		if (intPtr275 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_LobbySearch_SetTargetUserId");
		}
		EOS_LobbySearch_SetTargetUserId = (EOS_LobbySearch_SetTargetUserIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr275, typeof(EOS_LobbySearch_SetTargetUserIdDelegate));
		IntPtr intPtr276 = getFunctionPointer(libraryHandle, "EOS_Lobby_AddNotifyJoinLobbyAccepted");
		if (intPtr276 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_AddNotifyJoinLobbyAccepted");
		}
		EOS_Lobby_AddNotifyJoinLobbyAccepted = (EOS_Lobby_AddNotifyJoinLobbyAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr276, typeof(EOS_Lobby_AddNotifyJoinLobbyAcceptedDelegate));
		IntPtr intPtr277 = getFunctionPointer(libraryHandle, "EOS_Lobby_AddNotifyLeaveLobbyRequested");
		if (intPtr277 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_AddNotifyLeaveLobbyRequested");
		}
		EOS_Lobby_AddNotifyLeaveLobbyRequested = (EOS_Lobby_AddNotifyLeaveLobbyRequestedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr277, typeof(EOS_Lobby_AddNotifyLeaveLobbyRequestedDelegate));
		IntPtr intPtr278 = getFunctionPointer(libraryHandle, "EOS_Lobby_AddNotifyLobbyInviteAccepted");
		if (intPtr278 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_AddNotifyLobbyInviteAccepted");
		}
		EOS_Lobby_AddNotifyLobbyInviteAccepted = (EOS_Lobby_AddNotifyLobbyInviteAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr278, typeof(EOS_Lobby_AddNotifyLobbyInviteAcceptedDelegate));
		IntPtr intPtr279 = getFunctionPointer(libraryHandle, "EOS_Lobby_AddNotifyLobbyInviteReceived");
		if (intPtr279 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_AddNotifyLobbyInviteReceived");
		}
		EOS_Lobby_AddNotifyLobbyInviteReceived = (EOS_Lobby_AddNotifyLobbyInviteReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr279, typeof(EOS_Lobby_AddNotifyLobbyInviteReceivedDelegate));
		IntPtr intPtr280 = getFunctionPointer(libraryHandle, "EOS_Lobby_AddNotifyLobbyInviteRejected");
		if (intPtr280 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_AddNotifyLobbyInviteRejected");
		}
		EOS_Lobby_AddNotifyLobbyInviteRejected = (EOS_Lobby_AddNotifyLobbyInviteRejectedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr280, typeof(EOS_Lobby_AddNotifyLobbyInviteRejectedDelegate));
		IntPtr intPtr281 = getFunctionPointer(libraryHandle, "EOS_Lobby_AddNotifyLobbyMemberStatusReceived");
		if (intPtr281 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_AddNotifyLobbyMemberStatusReceived");
		}
		EOS_Lobby_AddNotifyLobbyMemberStatusReceived = (EOS_Lobby_AddNotifyLobbyMemberStatusReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr281, typeof(EOS_Lobby_AddNotifyLobbyMemberStatusReceivedDelegate));
		IntPtr intPtr282 = getFunctionPointer(libraryHandle, "EOS_Lobby_AddNotifyLobbyMemberUpdateReceived");
		if (intPtr282 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_AddNotifyLobbyMemberUpdateReceived");
		}
		EOS_Lobby_AddNotifyLobbyMemberUpdateReceived = (EOS_Lobby_AddNotifyLobbyMemberUpdateReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr282, typeof(EOS_Lobby_AddNotifyLobbyMemberUpdateReceivedDelegate));
		IntPtr intPtr283 = getFunctionPointer(libraryHandle, "EOS_Lobby_AddNotifyLobbyUpdateReceived");
		if (intPtr283 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_AddNotifyLobbyUpdateReceived");
		}
		EOS_Lobby_AddNotifyLobbyUpdateReceived = (EOS_Lobby_AddNotifyLobbyUpdateReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr283, typeof(EOS_Lobby_AddNotifyLobbyUpdateReceivedDelegate));
		IntPtr intPtr284 = getFunctionPointer(libraryHandle, "EOS_Lobby_AddNotifyRTCRoomConnectionChanged");
		if (intPtr284 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_AddNotifyRTCRoomConnectionChanged");
		}
		EOS_Lobby_AddNotifyRTCRoomConnectionChanged = (EOS_Lobby_AddNotifyRTCRoomConnectionChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr284, typeof(EOS_Lobby_AddNotifyRTCRoomConnectionChangedDelegate));
		IntPtr intPtr285 = getFunctionPointer(libraryHandle, "EOS_Lobby_AddNotifySendLobbyNativeInviteRequested");
		if (intPtr285 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_AddNotifySendLobbyNativeInviteRequested");
		}
		EOS_Lobby_AddNotifySendLobbyNativeInviteRequested = (EOS_Lobby_AddNotifySendLobbyNativeInviteRequestedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr285, typeof(EOS_Lobby_AddNotifySendLobbyNativeInviteRequestedDelegate));
		IntPtr intPtr286 = getFunctionPointer(libraryHandle, "EOS_Lobby_Attribute_Release");
		if (intPtr286 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_Attribute_Release");
		}
		EOS_Lobby_Attribute_Release = (EOS_Lobby_Attribute_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr286, typeof(EOS_Lobby_Attribute_ReleaseDelegate));
		IntPtr intPtr287 = getFunctionPointer(libraryHandle, "EOS_Lobby_CopyLobbyDetailsHandle");
		if (intPtr287 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_CopyLobbyDetailsHandle");
		}
		EOS_Lobby_CopyLobbyDetailsHandle = (EOS_Lobby_CopyLobbyDetailsHandleDelegate)Marshal.GetDelegateForFunctionPointer(intPtr287, typeof(EOS_Lobby_CopyLobbyDetailsHandleDelegate));
		IntPtr intPtr288 = getFunctionPointer(libraryHandle, "EOS_Lobby_CopyLobbyDetailsHandleByInviteId");
		if (intPtr288 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_CopyLobbyDetailsHandleByInviteId");
		}
		EOS_Lobby_CopyLobbyDetailsHandleByInviteId = (EOS_Lobby_CopyLobbyDetailsHandleByInviteIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr288, typeof(EOS_Lobby_CopyLobbyDetailsHandleByInviteIdDelegate));
		IntPtr intPtr289 = getFunctionPointer(libraryHandle, "EOS_Lobby_CopyLobbyDetailsHandleByUiEventId");
		if (intPtr289 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_CopyLobbyDetailsHandleByUiEventId");
		}
		EOS_Lobby_CopyLobbyDetailsHandleByUiEventId = (EOS_Lobby_CopyLobbyDetailsHandleByUiEventIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr289, typeof(EOS_Lobby_CopyLobbyDetailsHandleByUiEventIdDelegate));
		IntPtr intPtr290 = getFunctionPointer(libraryHandle, "EOS_Lobby_CreateLobby");
		if (intPtr290 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_CreateLobby");
		}
		EOS_Lobby_CreateLobby = (EOS_Lobby_CreateLobbyDelegate)Marshal.GetDelegateForFunctionPointer(intPtr290, typeof(EOS_Lobby_CreateLobbyDelegate));
		IntPtr intPtr291 = getFunctionPointer(libraryHandle, "EOS_Lobby_CreateLobbySearch");
		if (intPtr291 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_CreateLobbySearch");
		}
		EOS_Lobby_CreateLobbySearch = (EOS_Lobby_CreateLobbySearchDelegate)Marshal.GetDelegateForFunctionPointer(intPtr291, typeof(EOS_Lobby_CreateLobbySearchDelegate));
		IntPtr intPtr292 = getFunctionPointer(libraryHandle, "EOS_Lobby_DestroyLobby");
		if (intPtr292 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_DestroyLobby");
		}
		EOS_Lobby_DestroyLobby = (EOS_Lobby_DestroyLobbyDelegate)Marshal.GetDelegateForFunctionPointer(intPtr292, typeof(EOS_Lobby_DestroyLobbyDelegate));
		IntPtr intPtr293 = getFunctionPointer(libraryHandle, "EOS_Lobby_GetConnectString");
		if (intPtr293 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_GetConnectString");
		}
		EOS_Lobby_GetConnectString = (EOS_Lobby_GetConnectStringDelegate)Marshal.GetDelegateForFunctionPointer(intPtr293, typeof(EOS_Lobby_GetConnectStringDelegate));
		IntPtr intPtr294 = getFunctionPointer(libraryHandle, "EOS_Lobby_GetInviteCount");
		if (intPtr294 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_GetInviteCount");
		}
		EOS_Lobby_GetInviteCount = (EOS_Lobby_GetInviteCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr294, typeof(EOS_Lobby_GetInviteCountDelegate));
		IntPtr intPtr295 = getFunctionPointer(libraryHandle, "EOS_Lobby_GetInviteIdByIndex");
		if (intPtr295 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_GetInviteIdByIndex");
		}
		EOS_Lobby_GetInviteIdByIndex = (EOS_Lobby_GetInviteIdByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr295, typeof(EOS_Lobby_GetInviteIdByIndexDelegate));
		IntPtr intPtr296 = getFunctionPointer(libraryHandle, "EOS_Lobby_GetRTCRoomName");
		if (intPtr296 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_GetRTCRoomName");
		}
		EOS_Lobby_GetRTCRoomName = (EOS_Lobby_GetRTCRoomNameDelegate)Marshal.GetDelegateForFunctionPointer(intPtr296, typeof(EOS_Lobby_GetRTCRoomNameDelegate));
		IntPtr intPtr297 = getFunctionPointer(libraryHandle, "EOS_Lobby_HardMuteMember");
		if (intPtr297 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_HardMuteMember");
		}
		EOS_Lobby_HardMuteMember = (EOS_Lobby_HardMuteMemberDelegate)Marshal.GetDelegateForFunctionPointer(intPtr297, typeof(EOS_Lobby_HardMuteMemberDelegate));
		IntPtr intPtr298 = getFunctionPointer(libraryHandle, "EOS_Lobby_IsRTCRoomConnected");
		if (intPtr298 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_IsRTCRoomConnected");
		}
		EOS_Lobby_IsRTCRoomConnected = (EOS_Lobby_IsRTCRoomConnectedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr298, typeof(EOS_Lobby_IsRTCRoomConnectedDelegate));
		IntPtr intPtr299 = getFunctionPointer(libraryHandle, "EOS_Lobby_JoinLobby");
		if (intPtr299 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_JoinLobby");
		}
		EOS_Lobby_JoinLobby = (EOS_Lobby_JoinLobbyDelegate)Marshal.GetDelegateForFunctionPointer(intPtr299, typeof(EOS_Lobby_JoinLobbyDelegate));
		IntPtr intPtr300 = getFunctionPointer(libraryHandle, "EOS_Lobby_JoinLobbyById");
		if (intPtr300 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_JoinLobbyById");
		}
		EOS_Lobby_JoinLobbyById = (EOS_Lobby_JoinLobbyByIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr300, typeof(EOS_Lobby_JoinLobbyByIdDelegate));
		IntPtr intPtr301 = getFunctionPointer(libraryHandle, "EOS_Lobby_KickMember");
		if (intPtr301 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_KickMember");
		}
		EOS_Lobby_KickMember = (EOS_Lobby_KickMemberDelegate)Marshal.GetDelegateForFunctionPointer(intPtr301, typeof(EOS_Lobby_KickMemberDelegate));
		IntPtr intPtr302 = getFunctionPointer(libraryHandle, "EOS_Lobby_LeaveLobby");
		if (intPtr302 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_LeaveLobby");
		}
		EOS_Lobby_LeaveLobby = (EOS_Lobby_LeaveLobbyDelegate)Marshal.GetDelegateForFunctionPointer(intPtr302, typeof(EOS_Lobby_LeaveLobbyDelegate));
		IntPtr intPtr303 = getFunctionPointer(libraryHandle, "EOS_Lobby_ParseConnectString");
		if (intPtr303 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_ParseConnectString");
		}
		EOS_Lobby_ParseConnectString = (EOS_Lobby_ParseConnectStringDelegate)Marshal.GetDelegateForFunctionPointer(intPtr303, typeof(EOS_Lobby_ParseConnectStringDelegate));
		IntPtr intPtr304 = getFunctionPointer(libraryHandle, "EOS_Lobby_PromoteMember");
		if (intPtr304 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_PromoteMember");
		}
		EOS_Lobby_PromoteMember = (EOS_Lobby_PromoteMemberDelegate)Marshal.GetDelegateForFunctionPointer(intPtr304, typeof(EOS_Lobby_PromoteMemberDelegate));
		IntPtr intPtr305 = getFunctionPointer(libraryHandle, "EOS_Lobby_QueryInvites");
		if (intPtr305 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_QueryInvites");
		}
		EOS_Lobby_QueryInvites = (EOS_Lobby_QueryInvitesDelegate)Marshal.GetDelegateForFunctionPointer(intPtr305, typeof(EOS_Lobby_QueryInvitesDelegate));
		IntPtr intPtr306 = getFunctionPointer(libraryHandle, "EOS_Lobby_RejectInvite");
		if (intPtr306 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_RejectInvite");
		}
		EOS_Lobby_RejectInvite = (EOS_Lobby_RejectInviteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr306, typeof(EOS_Lobby_RejectInviteDelegate));
		IntPtr intPtr307 = getFunctionPointer(libraryHandle, "EOS_Lobby_RemoveNotifyJoinLobbyAccepted");
		if (intPtr307 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_RemoveNotifyJoinLobbyAccepted");
		}
		EOS_Lobby_RemoveNotifyJoinLobbyAccepted = (EOS_Lobby_RemoveNotifyJoinLobbyAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr307, typeof(EOS_Lobby_RemoveNotifyJoinLobbyAcceptedDelegate));
		IntPtr intPtr308 = getFunctionPointer(libraryHandle, "EOS_Lobby_RemoveNotifyLeaveLobbyRequested");
		if (intPtr308 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_RemoveNotifyLeaveLobbyRequested");
		}
		EOS_Lobby_RemoveNotifyLeaveLobbyRequested = (EOS_Lobby_RemoveNotifyLeaveLobbyRequestedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr308, typeof(EOS_Lobby_RemoveNotifyLeaveLobbyRequestedDelegate));
		IntPtr intPtr309 = getFunctionPointer(libraryHandle, "EOS_Lobby_RemoveNotifyLobbyInviteAccepted");
		if (intPtr309 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_RemoveNotifyLobbyInviteAccepted");
		}
		EOS_Lobby_RemoveNotifyLobbyInviteAccepted = (EOS_Lobby_RemoveNotifyLobbyInviteAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr309, typeof(EOS_Lobby_RemoveNotifyLobbyInviteAcceptedDelegate));
		IntPtr intPtr310 = getFunctionPointer(libraryHandle, "EOS_Lobby_RemoveNotifyLobbyInviteReceived");
		if (intPtr310 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_RemoveNotifyLobbyInviteReceived");
		}
		EOS_Lobby_RemoveNotifyLobbyInviteReceived = (EOS_Lobby_RemoveNotifyLobbyInviteReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr310, typeof(EOS_Lobby_RemoveNotifyLobbyInviteReceivedDelegate));
		IntPtr intPtr311 = getFunctionPointer(libraryHandle, "EOS_Lobby_RemoveNotifyLobbyInviteRejected");
		if (intPtr311 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_RemoveNotifyLobbyInviteRejected");
		}
		EOS_Lobby_RemoveNotifyLobbyInviteRejected = (EOS_Lobby_RemoveNotifyLobbyInviteRejectedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr311, typeof(EOS_Lobby_RemoveNotifyLobbyInviteRejectedDelegate));
		IntPtr intPtr312 = getFunctionPointer(libraryHandle, "EOS_Lobby_RemoveNotifyLobbyMemberStatusReceived");
		if (intPtr312 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_RemoveNotifyLobbyMemberStatusReceived");
		}
		EOS_Lobby_RemoveNotifyLobbyMemberStatusReceived = (EOS_Lobby_RemoveNotifyLobbyMemberStatusReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr312, typeof(EOS_Lobby_RemoveNotifyLobbyMemberStatusReceivedDelegate));
		IntPtr intPtr313 = getFunctionPointer(libraryHandle, "EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceived");
		if (intPtr313 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceived");
		}
		EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceived = (EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr313, typeof(EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceivedDelegate));
		IntPtr intPtr314 = getFunctionPointer(libraryHandle, "EOS_Lobby_RemoveNotifyLobbyUpdateReceived");
		if (intPtr314 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_RemoveNotifyLobbyUpdateReceived");
		}
		EOS_Lobby_RemoveNotifyLobbyUpdateReceived = (EOS_Lobby_RemoveNotifyLobbyUpdateReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr314, typeof(EOS_Lobby_RemoveNotifyLobbyUpdateReceivedDelegate));
		IntPtr intPtr315 = getFunctionPointer(libraryHandle, "EOS_Lobby_RemoveNotifyRTCRoomConnectionChanged");
		if (intPtr315 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_RemoveNotifyRTCRoomConnectionChanged");
		}
		EOS_Lobby_RemoveNotifyRTCRoomConnectionChanged = (EOS_Lobby_RemoveNotifyRTCRoomConnectionChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr315, typeof(EOS_Lobby_RemoveNotifyRTCRoomConnectionChangedDelegate));
		IntPtr intPtr316 = getFunctionPointer(libraryHandle, "EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequested");
		if (intPtr316 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequested");
		}
		EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequested = (EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequestedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr316, typeof(EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequestedDelegate));
		IntPtr intPtr317 = getFunctionPointer(libraryHandle, "EOS_Lobby_SendInvite");
		if (intPtr317 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_SendInvite");
		}
		EOS_Lobby_SendInvite = (EOS_Lobby_SendInviteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr317, typeof(EOS_Lobby_SendInviteDelegate));
		IntPtr intPtr318 = getFunctionPointer(libraryHandle, "EOS_Lobby_UpdateLobby");
		if (intPtr318 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_UpdateLobby");
		}
		EOS_Lobby_UpdateLobby = (EOS_Lobby_UpdateLobbyDelegate)Marshal.GetDelegateForFunctionPointer(intPtr318, typeof(EOS_Lobby_UpdateLobbyDelegate));
		IntPtr intPtr319 = getFunctionPointer(libraryHandle, "EOS_Lobby_UpdateLobbyModification");
		if (intPtr319 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Lobby_UpdateLobbyModification");
		}
		EOS_Lobby_UpdateLobbyModification = (EOS_Lobby_UpdateLobbyModificationDelegate)Marshal.GetDelegateForFunctionPointer(intPtr319, typeof(EOS_Lobby_UpdateLobbyModificationDelegate));
		IntPtr intPtr320 = getFunctionPointer(libraryHandle, "EOS_Logging_SetCallback");
		if (intPtr320 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Logging_SetCallback");
		}
		EOS_Logging_SetCallback = (EOS_Logging_SetCallbackDelegate)Marshal.GetDelegateForFunctionPointer(intPtr320, typeof(EOS_Logging_SetCallbackDelegate));
		IntPtr intPtr321 = getFunctionPointer(libraryHandle, "EOS_Logging_SetLogLevel");
		if (intPtr321 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Logging_SetLogLevel");
		}
		EOS_Logging_SetLogLevel = (EOS_Logging_SetLogLevelDelegate)Marshal.GetDelegateForFunctionPointer(intPtr321, typeof(EOS_Logging_SetLogLevelDelegate));
		IntPtr intPtr322 = getFunctionPointer(libraryHandle, "EOS_Metrics_BeginPlayerSession");
		if (intPtr322 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Metrics_BeginPlayerSession");
		}
		EOS_Metrics_BeginPlayerSession = (EOS_Metrics_BeginPlayerSessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr322, typeof(EOS_Metrics_BeginPlayerSessionDelegate));
		IntPtr intPtr323 = getFunctionPointer(libraryHandle, "EOS_Metrics_EndPlayerSession");
		if (intPtr323 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Metrics_EndPlayerSession");
		}
		EOS_Metrics_EndPlayerSession = (EOS_Metrics_EndPlayerSessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr323, typeof(EOS_Metrics_EndPlayerSessionDelegate));
		IntPtr intPtr324 = getFunctionPointer(libraryHandle, "EOS_Mods_CopyModInfo");
		if (intPtr324 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Mods_CopyModInfo");
		}
		EOS_Mods_CopyModInfo = (EOS_Mods_CopyModInfoDelegate)Marshal.GetDelegateForFunctionPointer(intPtr324, typeof(EOS_Mods_CopyModInfoDelegate));
		IntPtr intPtr325 = getFunctionPointer(libraryHandle, "EOS_Mods_EnumerateMods");
		if (intPtr325 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Mods_EnumerateMods");
		}
		EOS_Mods_EnumerateMods = (EOS_Mods_EnumerateModsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr325, typeof(EOS_Mods_EnumerateModsDelegate));
		IntPtr intPtr326 = getFunctionPointer(libraryHandle, "EOS_Mods_InstallMod");
		if (intPtr326 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Mods_InstallMod");
		}
		EOS_Mods_InstallMod = (EOS_Mods_InstallModDelegate)Marshal.GetDelegateForFunctionPointer(intPtr326, typeof(EOS_Mods_InstallModDelegate));
		IntPtr intPtr327 = getFunctionPointer(libraryHandle, "EOS_Mods_ModInfo_Release");
		if (intPtr327 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Mods_ModInfo_Release");
		}
		EOS_Mods_ModInfo_Release = (EOS_Mods_ModInfo_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr327, typeof(EOS_Mods_ModInfo_ReleaseDelegate));
		IntPtr intPtr328 = getFunctionPointer(libraryHandle, "EOS_Mods_UninstallMod");
		if (intPtr328 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Mods_UninstallMod");
		}
		EOS_Mods_UninstallMod = (EOS_Mods_UninstallModDelegate)Marshal.GetDelegateForFunctionPointer(intPtr328, typeof(EOS_Mods_UninstallModDelegate));
		IntPtr intPtr329 = getFunctionPointer(libraryHandle, "EOS_Mods_UpdateMod");
		if (intPtr329 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Mods_UpdateMod");
		}
		EOS_Mods_UpdateMod = (EOS_Mods_UpdateModDelegate)Marshal.GetDelegateForFunctionPointer(intPtr329, typeof(EOS_Mods_UpdateModDelegate));
		IntPtr intPtr330 = getFunctionPointer(libraryHandle, "EOS_P2P_AcceptConnection");
		if (intPtr330 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_AcceptConnection");
		}
		EOS_P2P_AcceptConnection = (EOS_P2P_AcceptConnectionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr330, typeof(EOS_P2P_AcceptConnectionDelegate));
		IntPtr intPtr331 = getFunctionPointer(libraryHandle, "EOS_P2P_AddNotifyIncomingPacketQueueFull");
		if (intPtr331 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_AddNotifyIncomingPacketQueueFull");
		}
		EOS_P2P_AddNotifyIncomingPacketQueueFull = (EOS_P2P_AddNotifyIncomingPacketQueueFullDelegate)Marshal.GetDelegateForFunctionPointer(intPtr331, typeof(EOS_P2P_AddNotifyIncomingPacketQueueFullDelegate));
		IntPtr intPtr332 = getFunctionPointer(libraryHandle, "EOS_P2P_AddNotifyPeerConnectionClosed");
		if (intPtr332 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_AddNotifyPeerConnectionClosed");
		}
		EOS_P2P_AddNotifyPeerConnectionClosed = (EOS_P2P_AddNotifyPeerConnectionClosedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr332, typeof(EOS_P2P_AddNotifyPeerConnectionClosedDelegate));
		IntPtr intPtr333 = getFunctionPointer(libraryHandle, "EOS_P2P_AddNotifyPeerConnectionEstablished");
		if (intPtr333 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_AddNotifyPeerConnectionEstablished");
		}
		EOS_P2P_AddNotifyPeerConnectionEstablished = (EOS_P2P_AddNotifyPeerConnectionEstablishedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr333, typeof(EOS_P2P_AddNotifyPeerConnectionEstablishedDelegate));
		IntPtr intPtr334 = getFunctionPointer(libraryHandle, "EOS_P2P_AddNotifyPeerConnectionInterrupted");
		if (intPtr334 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_AddNotifyPeerConnectionInterrupted");
		}
		EOS_P2P_AddNotifyPeerConnectionInterrupted = (EOS_P2P_AddNotifyPeerConnectionInterruptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr334, typeof(EOS_P2P_AddNotifyPeerConnectionInterruptedDelegate));
		IntPtr intPtr335 = getFunctionPointer(libraryHandle, "EOS_P2P_AddNotifyPeerConnectionRequest");
		if (intPtr335 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_AddNotifyPeerConnectionRequest");
		}
		EOS_P2P_AddNotifyPeerConnectionRequest = (EOS_P2P_AddNotifyPeerConnectionRequestDelegate)Marshal.GetDelegateForFunctionPointer(intPtr335, typeof(EOS_P2P_AddNotifyPeerConnectionRequestDelegate));
		IntPtr intPtr336 = getFunctionPointer(libraryHandle, "EOS_P2P_ClearPacketQueue");
		if (intPtr336 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_ClearPacketQueue");
		}
		EOS_P2P_ClearPacketQueue = (EOS_P2P_ClearPacketQueueDelegate)Marshal.GetDelegateForFunctionPointer(intPtr336, typeof(EOS_P2P_ClearPacketQueueDelegate));
		IntPtr intPtr337 = getFunctionPointer(libraryHandle, "EOS_P2P_CloseConnection");
		if (intPtr337 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_CloseConnection");
		}
		EOS_P2P_CloseConnection = (EOS_P2P_CloseConnectionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr337, typeof(EOS_P2P_CloseConnectionDelegate));
		IntPtr intPtr338 = getFunctionPointer(libraryHandle, "EOS_P2P_CloseConnections");
		if (intPtr338 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_CloseConnections");
		}
		EOS_P2P_CloseConnections = (EOS_P2P_CloseConnectionsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr338, typeof(EOS_P2P_CloseConnectionsDelegate));
		IntPtr intPtr339 = getFunctionPointer(libraryHandle, "EOS_P2P_GetNATType");
		if (intPtr339 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_GetNATType");
		}
		EOS_P2P_GetNATType = (EOS_P2P_GetNATTypeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr339, typeof(EOS_P2P_GetNATTypeDelegate));
		IntPtr intPtr340 = getFunctionPointer(libraryHandle, "EOS_P2P_GetNextReceivedPacketSize");
		if (intPtr340 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_GetNextReceivedPacketSize");
		}
		EOS_P2P_GetNextReceivedPacketSize = (EOS_P2P_GetNextReceivedPacketSizeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr340, typeof(EOS_P2P_GetNextReceivedPacketSizeDelegate));
		IntPtr intPtr341 = getFunctionPointer(libraryHandle, "EOS_P2P_GetPacketQueueInfo");
		if (intPtr341 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_GetPacketQueueInfo");
		}
		EOS_P2P_GetPacketQueueInfo = (EOS_P2P_GetPacketQueueInfoDelegate)Marshal.GetDelegateForFunctionPointer(intPtr341, typeof(EOS_P2P_GetPacketQueueInfoDelegate));
		IntPtr intPtr342 = getFunctionPointer(libraryHandle, "EOS_P2P_GetPortRange");
		if (intPtr342 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_GetPortRange");
		}
		EOS_P2P_GetPortRange = (EOS_P2P_GetPortRangeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr342, typeof(EOS_P2P_GetPortRangeDelegate));
		IntPtr intPtr343 = getFunctionPointer(libraryHandle, "EOS_P2P_GetRelayControl");
		if (intPtr343 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_GetRelayControl");
		}
		EOS_P2P_GetRelayControl = (EOS_P2P_GetRelayControlDelegate)Marshal.GetDelegateForFunctionPointer(intPtr343, typeof(EOS_P2P_GetRelayControlDelegate));
		IntPtr intPtr344 = getFunctionPointer(libraryHandle, "EOS_P2P_QueryNATType");
		if (intPtr344 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_QueryNATType");
		}
		EOS_P2P_QueryNATType = (EOS_P2P_QueryNATTypeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr344, typeof(EOS_P2P_QueryNATTypeDelegate));
		IntPtr intPtr345 = getFunctionPointer(libraryHandle, "EOS_P2P_ReceivePacket");
		if (intPtr345 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_ReceivePacket");
		}
		EOS_P2P_ReceivePacket = (EOS_P2P_ReceivePacketDelegate)Marshal.GetDelegateForFunctionPointer(intPtr345, typeof(EOS_P2P_ReceivePacketDelegate));
		IntPtr intPtr346 = getFunctionPointer(libraryHandle, "EOS_P2P_RemoveNotifyIncomingPacketQueueFull");
		if (intPtr346 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_RemoveNotifyIncomingPacketQueueFull");
		}
		EOS_P2P_RemoveNotifyIncomingPacketQueueFull = (EOS_P2P_RemoveNotifyIncomingPacketQueueFullDelegate)Marshal.GetDelegateForFunctionPointer(intPtr346, typeof(EOS_P2P_RemoveNotifyIncomingPacketQueueFullDelegate));
		IntPtr intPtr347 = getFunctionPointer(libraryHandle, "EOS_P2P_RemoveNotifyPeerConnectionClosed");
		if (intPtr347 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_RemoveNotifyPeerConnectionClosed");
		}
		EOS_P2P_RemoveNotifyPeerConnectionClosed = (EOS_P2P_RemoveNotifyPeerConnectionClosedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr347, typeof(EOS_P2P_RemoveNotifyPeerConnectionClosedDelegate));
		IntPtr intPtr348 = getFunctionPointer(libraryHandle, "EOS_P2P_RemoveNotifyPeerConnectionEstablished");
		if (intPtr348 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_RemoveNotifyPeerConnectionEstablished");
		}
		EOS_P2P_RemoveNotifyPeerConnectionEstablished = (EOS_P2P_RemoveNotifyPeerConnectionEstablishedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr348, typeof(EOS_P2P_RemoveNotifyPeerConnectionEstablishedDelegate));
		IntPtr intPtr349 = getFunctionPointer(libraryHandle, "EOS_P2P_RemoveNotifyPeerConnectionInterrupted");
		if (intPtr349 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_RemoveNotifyPeerConnectionInterrupted");
		}
		EOS_P2P_RemoveNotifyPeerConnectionInterrupted = (EOS_P2P_RemoveNotifyPeerConnectionInterruptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr349, typeof(EOS_P2P_RemoveNotifyPeerConnectionInterruptedDelegate));
		IntPtr intPtr350 = getFunctionPointer(libraryHandle, "EOS_P2P_RemoveNotifyPeerConnectionRequest");
		if (intPtr350 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_RemoveNotifyPeerConnectionRequest");
		}
		EOS_P2P_RemoveNotifyPeerConnectionRequest = (EOS_P2P_RemoveNotifyPeerConnectionRequestDelegate)Marshal.GetDelegateForFunctionPointer(intPtr350, typeof(EOS_P2P_RemoveNotifyPeerConnectionRequestDelegate));
		IntPtr intPtr351 = getFunctionPointer(libraryHandle, "EOS_P2P_SendPacket");
		if (intPtr351 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_SendPacket");
		}
		EOS_P2P_SendPacket = (EOS_P2P_SendPacketDelegate)Marshal.GetDelegateForFunctionPointer(intPtr351, typeof(EOS_P2P_SendPacketDelegate));
		IntPtr intPtr352 = getFunctionPointer(libraryHandle, "EOS_P2P_SetPacketQueueSize");
		if (intPtr352 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_SetPacketQueueSize");
		}
		EOS_P2P_SetPacketQueueSize = (EOS_P2P_SetPacketQueueSizeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr352, typeof(EOS_P2P_SetPacketQueueSizeDelegate));
		IntPtr intPtr353 = getFunctionPointer(libraryHandle, "EOS_P2P_SetPortRange");
		if (intPtr353 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_SetPortRange");
		}
		EOS_P2P_SetPortRange = (EOS_P2P_SetPortRangeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr353, typeof(EOS_P2P_SetPortRangeDelegate));
		IntPtr intPtr354 = getFunctionPointer(libraryHandle, "EOS_P2P_SetRelayControl");
		if (intPtr354 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_P2P_SetRelayControl");
		}
		EOS_P2P_SetRelayControl = (EOS_P2P_SetRelayControlDelegate)Marshal.GetDelegateForFunctionPointer(intPtr354, typeof(EOS_P2P_SetRelayControlDelegate));
		IntPtr intPtr355 = getFunctionPointer(libraryHandle, "EOS_Platform_CheckForLauncherAndRestart");
		if (intPtr355 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_CheckForLauncherAndRestart");
		}
		EOS_Platform_CheckForLauncherAndRestart = (EOS_Platform_CheckForLauncherAndRestartDelegate)Marshal.GetDelegateForFunctionPointer(intPtr355, typeof(EOS_Platform_CheckForLauncherAndRestartDelegate));
		IntPtr intPtr356 = getFunctionPointer(libraryHandle, "EOS_Platform_Create");
		if (intPtr356 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_Create");
		}
		EOS_Platform_Create = (EOS_Platform_CreateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr356, typeof(EOS_Platform_CreateDelegate));
		IntPtr intPtr357 = getFunctionPointer(libraryHandle, "EOS_Platform_GetAchievementsInterface");
		if (intPtr357 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetAchievementsInterface");
		}
		EOS_Platform_GetAchievementsInterface = (EOS_Platform_GetAchievementsInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr357, typeof(EOS_Platform_GetAchievementsInterfaceDelegate));
		IntPtr intPtr358 = getFunctionPointer(libraryHandle, "EOS_Platform_GetActiveCountryCode");
		if (intPtr358 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetActiveCountryCode");
		}
		EOS_Platform_GetActiveCountryCode = (EOS_Platform_GetActiveCountryCodeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr358, typeof(EOS_Platform_GetActiveCountryCodeDelegate));
		IntPtr intPtr359 = getFunctionPointer(libraryHandle, "EOS_Platform_GetActiveLocaleCode");
		if (intPtr359 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetActiveLocaleCode");
		}
		EOS_Platform_GetActiveLocaleCode = (EOS_Platform_GetActiveLocaleCodeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr359, typeof(EOS_Platform_GetActiveLocaleCodeDelegate));
		IntPtr intPtr360 = getFunctionPointer(libraryHandle, "EOS_Platform_GetAntiCheatClientInterface");
		if (intPtr360 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetAntiCheatClientInterface");
		}
		EOS_Platform_GetAntiCheatClientInterface = (EOS_Platform_GetAntiCheatClientInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr360, typeof(EOS_Platform_GetAntiCheatClientInterfaceDelegate));
		IntPtr intPtr361 = getFunctionPointer(libraryHandle, "EOS_Platform_GetAntiCheatServerInterface");
		if (intPtr361 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetAntiCheatServerInterface");
		}
		EOS_Platform_GetAntiCheatServerInterface = (EOS_Platform_GetAntiCheatServerInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr361, typeof(EOS_Platform_GetAntiCheatServerInterfaceDelegate));
		IntPtr intPtr362 = getFunctionPointer(libraryHandle, "EOS_Platform_GetApplicationStatus");
		if (intPtr362 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetApplicationStatus");
		}
		EOS_Platform_GetApplicationStatus = (EOS_Platform_GetApplicationStatusDelegate)Marshal.GetDelegateForFunctionPointer(intPtr362, typeof(EOS_Platform_GetApplicationStatusDelegate));
		IntPtr intPtr363 = getFunctionPointer(libraryHandle, "EOS_Platform_GetAuthInterface");
		if (intPtr363 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetAuthInterface");
		}
		EOS_Platform_GetAuthInterface = (EOS_Platform_GetAuthInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr363, typeof(EOS_Platform_GetAuthInterfaceDelegate));
		IntPtr intPtr364 = getFunctionPointer(libraryHandle, "EOS_Platform_GetConnectInterface");
		if (intPtr364 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetConnectInterface");
		}
		EOS_Platform_GetConnectInterface = (EOS_Platform_GetConnectInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr364, typeof(EOS_Platform_GetConnectInterfaceDelegate));
		IntPtr intPtr365 = getFunctionPointer(libraryHandle, "EOS_Platform_GetCustomInvitesInterface");
		if (intPtr365 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetCustomInvitesInterface");
		}
		EOS_Platform_GetCustomInvitesInterface = (EOS_Platform_GetCustomInvitesInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr365, typeof(EOS_Platform_GetCustomInvitesInterfaceDelegate));
		IntPtr intPtr366 = getFunctionPointer(libraryHandle, "EOS_Platform_GetDesktopCrossplayStatus");
		if (intPtr366 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetDesktopCrossplayStatus");
		}
		EOS_Platform_GetDesktopCrossplayStatus = (EOS_Platform_GetDesktopCrossplayStatusDelegate)Marshal.GetDelegateForFunctionPointer(intPtr366, typeof(EOS_Platform_GetDesktopCrossplayStatusDelegate));
		IntPtr intPtr367 = getFunctionPointer(libraryHandle, "EOS_Platform_GetEcomInterface");
		if (intPtr367 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetEcomInterface");
		}
		EOS_Platform_GetEcomInterface = (EOS_Platform_GetEcomInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr367, typeof(EOS_Platform_GetEcomInterfaceDelegate));
		IntPtr intPtr368 = getFunctionPointer(libraryHandle, "EOS_Platform_GetFriendsInterface");
		if (intPtr368 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetFriendsInterface");
		}
		EOS_Platform_GetFriendsInterface = (EOS_Platform_GetFriendsInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr368, typeof(EOS_Platform_GetFriendsInterfaceDelegate));
		IntPtr intPtr369 = getFunctionPointer(libraryHandle, "EOS_Platform_GetIntegratedPlatformInterface");
		if (intPtr369 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetIntegratedPlatformInterface");
		}
		EOS_Platform_GetIntegratedPlatformInterface = (EOS_Platform_GetIntegratedPlatformInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr369, typeof(EOS_Platform_GetIntegratedPlatformInterfaceDelegate));
		IntPtr intPtr370 = getFunctionPointer(libraryHandle, "EOS_Platform_GetKWSInterface");
		if (intPtr370 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetKWSInterface");
		}
		EOS_Platform_GetKWSInterface = (EOS_Platform_GetKWSInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr370, typeof(EOS_Platform_GetKWSInterfaceDelegate));
		IntPtr intPtr371 = getFunctionPointer(libraryHandle, "EOS_Platform_GetLeaderboardsInterface");
		if (intPtr371 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetLeaderboardsInterface");
		}
		EOS_Platform_GetLeaderboardsInterface = (EOS_Platform_GetLeaderboardsInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr371, typeof(EOS_Platform_GetLeaderboardsInterfaceDelegate));
		IntPtr intPtr372 = getFunctionPointer(libraryHandle, "EOS_Platform_GetLobbyInterface");
		if (intPtr372 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetLobbyInterface");
		}
		EOS_Platform_GetLobbyInterface = (EOS_Platform_GetLobbyInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr372, typeof(EOS_Platform_GetLobbyInterfaceDelegate));
		IntPtr intPtr373 = getFunctionPointer(libraryHandle, "EOS_Platform_GetMetricsInterface");
		if (intPtr373 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetMetricsInterface");
		}
		EOS_Platform_GetMetricsInterface = (EOS_Platform_GetMetricsInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr373, typeof(EOS_Platform_GetMetricsInterfaceDelegate));
		IntPtr intPtr374 = getFunctionPointer(libraryHandle, "EOS_Platform_GetModsInterface");
		if (intPtr374 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetModsInterface");
		}
		EOS_Platform_GetModsInterface = (EOS_Platform_GetModsInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr374, typeof(EOS_Platform_GetModsInterfaceDelegate));
		IntPtr intPtr375 = getFunctionPointer(libraryHandle, "EOS_Platform_GetNetworkStatus");
		if (intPtr375 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetNetworkStatus");
		}
		EOS_Platform_GetNetworkStatus = (EOS_Platform_GetNetworkStatusDelegate)Marshal.GetDelegateForFunctionPointer(intPtr375, typeof(EOS_Platform_GetNetworkStatusDelegate));
		IntPtr intPtr376 = getFunctionPointer(libraryHandle, "EOS_Platform_GetOverrideCountryCode");
		if (intPtr376 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetOverrideCountryCode");
		}
		EOS_Platform_GetOverrideCountryCode = (EOS_Platform_GetOverrideCountryCodeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr376, typeof(EOS_Platform_GetOverrideCountryCodeDelegate));
		IntPtr intPtr377 = getFunctionPointer(libraryHandle, "EOS_Platform_GetOverrideLocaleCode");
		if (intPtr377 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetOverrideLocaleCode");
		}
		EOS_Platform_GetOverrideLocaleCode = (EOS_Platform_GetOverrideLocaleCodeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr377, typeof(EOS_Platform_GetOverrideLocaleCodeDelegate));
		IntPtr intPtr378 = getFunctionPointer(libraryHandle, "EOS_Platform_GetP2PInterface");
		if (intPtr378 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetP2PInterface");
		}
		EOS_Platform_GetP2PInterface = (EOS_Platform_GetP2PInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr378, typeof(EOS_Platform_GetP2PInterfaceDelegate));
		IntPtr intPtr379 = getFunctionPointer(libraryHandle, "EOS_Platform_GetPlayerDataStorageInterface");
		if (intPtr379 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetPlayerDataStorageInterface");
		}
		EOS_Platform_GetPlayerDataStorageInterface = (EOS_Platform_GetPlayerDataStorageInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr379, typeof(EOS_Platform_GetPlayerDataStorageInterfaceDelegate));
		IntPtr intPtr380 = getFunctionPointer(libraryHandle, "EOS_Platform_GetPresenceInterface");
		if (intPtr380 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetPresenceInterface");
		}
		EOS_Platform_GetPresenceInterface = (EOS_Platform_GetPresenceInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr380, typeof(EOS_Platform_GetPresenceInterfaceDelegate));
		IntPtr intPtr381 = getFunctionPointer(libraryHandle, "EOS_Platform_GetProgressionSnapshotInterface");
		if (intPtr381 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetProgressionSnapshotInterface");
		}
		EOS_Platform_GetProgressionSnapshotInterface = (EOS_Platform_GetProgressionSnapshotInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr381, typeof(EOS_Platform_GetProgressionSnapshotInterfaceDelegate));
		IntPtr intPtr382 = getFunctionPointer(libraryHandle, "EOS_Platform_GetRTCAdminInterface");
		if (intPtr382 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetRTCAdminInterface");
		}
		EOS_Platform_GetRTCAdminInterface = (EOS_Platform_GetRTCAdminInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr382, typeof(EOS_Platform_GetRTCAdminInterfaceDelegate));
		IntPtr intPtr383 = getFunctionPointer(libraryHandle, "EOS_Platform_GetRTCInterface");
		if (intPtr383 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetRTCInterface");
		}
		EOS_Platform_GetRTCInterface = (EOS_Platform_GetRTCInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr383, typeof(EOS_Platform_GetRTCInterfaceDelegate));
		IntPtr intPtr384 = getFunctionPointer(libraryHandle, "EOS_Platform_GetReportsInterface");
		if (intPtr384 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetReportsInterface");
		}
		EOS_Platform_GetReportsInterface = (EOS_Platform_GetReportsInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr384, typeof(EOS_Platform_GetReportsInterfaceDelegate));
		IntPtr intPtr385 = getFunctionPointer(libraryHandle, "EOS_Platform_GetSanctionsInterface");
		if (intPtr385 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetSanctionsInterface");
		}
		EOS_Platform_GetSanctionsInterface = (EOS_Platform_GetSanctionsInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr385, typeof(EOS_Platform_GetSanctionsInterfaceDelegate));
		IntPtr intPtr386 = getFunctionPointer(libraryHandle, "EOS_Platform_GetSessionsInterface");
		if (intPtr386 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetSessionsInterface");
		}
		EOS_Platform_GetSessionsInterface = (EOS_Platform_GetSessionsInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr386, typeof(EOS_Platform_GetSessionsInterfaceDelegate));
		IntPtr intPtr387 = getFunctionPointer(libraryHandle, "EOS_Platform_GetStatsInterface");
		if (intPtr387 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetStatsInterface");
		}
		EOS_Platform_GetStatsInterface = (EOS_Platform_GetStatsInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr387, typeof(EOS_Platform_GetStatsInterfaceDelegate));
		IntPtr intPtr388 = getFunctionPointer(libraryHandle, "EOS_Platform_GetTitleStorageInterface");
		if (intPtr388 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetTitleStorageInterface");
		}
		EOS_Platform_GetTitleStorageInterface = (EOS_Platform_GetTitleStorageInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr388, typeof(EOS_Platform_GetTitleStorageInterfaceDelegate));
		IntPtr intPtr389 = getFunctionPointer(libraryHandle, "EOS_Platform_GetUIInterface");
		if (intPtr389 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetUIInterface");
		}
		EOS_Platform_GetUIInterface = (EOS_Platform_GetUIInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr389, typeof(EOS_Platform_GetUIInterfaceDelegate));
		IntPtr intPtr390 = getFunctionPointer(libraryHandle, "EOS_Platform_GetUserInfoInterface");
		if (intPtr390 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_GetUserInfoInterface");
		}
		EOS_Platform_GetUserInfoInterface = (EOS_Platform_GetUserInfoInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr390, typeof(EOS_Platform_GetUserInfoInterfaceDelegate));
		IntPtr intPtr391 = getFunctionPointer(libraryHandle, "EOS_Platform_Release");
		if (intPtr391 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_Release");
		}
		EOS_Platform_Release = (EOS_Platform_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr391, typeof(EOS_Platform_ReleaseDelegate));
		IntPtr intPtr392 = getFunctionPointer(libraryHandle, "EOS_Platform_SetApplicationStatus");
		if (intPtr392 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_SetApplicationStatus");
		}
		EOS_Platform_SetApplicationStatus = (EOS_Platform_SetApplicationStatusDelegate)Marshal.GetDelegateForFunctionPointer(intPtr392, typeof(EOS_Platform_SetApplicationStatusDelegate));
		IntPtr intPtr393 = getFunctionPointer(libraryHandle, "EOS_Platform_SetNetworkStatus");
		if (intPtr393 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_SetNetworkStatus");
		}
		EOS_Platform_SetNetworkStatus = (EOS_Platform_SetNetworkStatusDelegate)Marshal.GetDelegateForFunctionPointer(intPtr393, typeof(EOS_Platform_SetNetworkStatusDelegate));
		IntPtr intPtr394 = getFunctionPointer(libraryHandle, "EOS_Platform_SetOverrideCountryCode");
		if (intPtr394 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_SetOverrideCountryCode");
		}
		EOS_Platform_SetOverrideCountryCode = (EOS_Platform_SetOverrideCountryCodeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr394, typeof(EOS_Platform_SetOverrideCountryCodeDelegate));
		IntPtr intPtr395 = getFunctionPointer(libraryHandle, "EOS_Platform_SetOverrideLocaleCode");
		if (intPtr395 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_SetOverrideLocaleCode");
		}
		EOS_Platform_SetOverrideLocaleCode = (EOS_Platform_SetOverrideLocaleCodeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr395, typeof(EOS_Platform_SetOverrideLocaleCodeDelegate));
		IntPtr intPtr396 = getFunctionPointer(libraryHandle, "EOS_Platform_Tick");
		if (intPtr396 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Platform_Tick");
		}
		EOS_Platform_Tick = (EOS_Platform_TickDelegate)Marshal.GetDelegateForFunctionPointer(intPtr396, typeof(EOS_Platform_TickDelegate));
		IntPtr intPtr397 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorageFileTransferRequest_CancelRequest");
		if (intPtr397 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorageFileTransferRequest_CancelRequest");
		}
		EOS_PlayerDataStorageFileTransferRequest_CancelRequest = (EOS_PlayerDataStorageFileTransferRequest_CancelRequestDelegate)Marshal.GetDelegateForFunctionPointer(intPtr397, typeof(EOS_PlayerDataStorageFileTransferRequest_CancelRequestDelegate));
		IntPtr intPtr398 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorageFileTransferRequest_GetFileRequestState");
		if (intPtr398 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorageFileTransferRequest_GetFileRequestState");
		}
		EOS_PlayerDataStorageFileTransferRequest_GetFileRequestState = (EOS_PlayerDataStorageFileTransferRequest_GetFileRequestStateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr398, typeof(EOS_PlayerDataStorageFileTransferRequest_GetFileRequestStateDelegate));
		IntPtr intPtr399 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorageFileTransferRequest_GetFilename");
		if (intPtr399 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorageFileTransferRequest_GetFilename");
		}
		EOS_PlayerDataStorageFileTransferRequest_GetFilename = (EOS_PlayerDataStorageFileTransferRequest_GetFilenameDelegate)Marshal.GetDelegateForFunctionPointer(intPtr399, typeof(EOS_PlayerDataStorageFileTransferRequest_GetFilenameDelegate));
		IntPtr intPtr400 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorageFileTransferRequest_Release");
		if (intPtr400 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorageFileTransferRequest_Release");
		}
		EOS_PlayerDataStorageFileTransferRequest_Release = (EOS_PlayerDataStorageFileTransferRequest_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr400, typeof(EOS_PlayerDataStorageFileTransferRequest_ReleaseDelegate));
		IntPtr intPtr401 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorage_CopyFileMetadataAtIndex");
		if (intPtr401 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorage_CopyFileMetadataAtIndex");
		}
		EOS_PlayerDataStorage_CopyFileMetadataAtIndex = (EOS_PlayerDataStorage_CopyFileMetadataAtIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr401, typeof(EOS_PlayerDataStorage_CopyFileMetadataAtIndexDelegate));
		IntPtr intPtr402 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorage_CopyFileMetadataByFilename");
		if (intPtr402 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorage_CopyFileMetadataByFilename");
		}
		EOS_PlayerDataStorage_CopyFileMetadataByFilename = (EOS_PlayerDataStorage_CopyFileMetadataByFilenameDelegate)Marshal.GetDelegateForFunctionPointer(intPtr402, typeof(EOS_PlayerDataStorage_CopyFileMetadataByFilenameDelegate));
		IntPtr intPtr403 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorage_DeleteCache");
		if (intPtr403 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorage_DeleteCache");
		}
		EOS_PlayerDataStorage_DeleteCache = (EOS_PlayerDataStorage_DeleteCacheDelegate)Marshal.GetDelegateForFunctionPointer(intPtr403, typeof(EOS_PlayerDataStorage_DeleteCacheDelegate));
		IntPtr intPtr404 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorage_DeleteFile");
		if (intPtr404 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorage_DeleteFile");
		}
		EOS_PlayerDataStorage_DeleteFile = (EOS_PlayerDataStorage_DeleteFileDelegate)Marshal.GetDelegateForFunctionPointer(intPtr404, typeof(EOS_PlayerDataStorage_DeleteFileDelegate));
		IntPtr intPtr405 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorage_DuplicateFile");
		if (intPtr405 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorage_DuplicateFile");
		}
		EOS_PlayerDataStorage_DuplicateFile = (EOS_PlayerDataStorage_DuplicateFileDelegate)Marshal.GetDelegateForFunctionPointer(intPtr405, typeof(EOS_PlayerDataStorage_DuplicateFileDelegate));
		IntPtr intPtr406 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorage_FileMetadata_Release");
		if (intPtr406 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorage_FileMetadata_Release");
		}
		EOS_PlayerDataStorage_FileMetadata_Release = (EOS_PlayerDataStorage_FileMetadata_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr406, typeof(EOS_PlayerDataStorage_FileMetadata_ReleaseDelegate));
		IntPtr intPtr407 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorage_GetFileMetadataCount");
		if (intPtr407 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorage_GetFileMetadataCount");
		}
		EOS_PlayerDataStorage_GetFileMetadataCount = (EOS_PlayerDataStorage_GetFileMetadataCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr407, typeof(EOS_PlayerDataStorage_GetFileMetadataCountDelegate));
		IntPtr intPtr408 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorage_QueryFile");
		if (intPtr408 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorage_QueryFile");
		}
		EOS_PlayerDataStorage_QueryFile = (EOS_PlayerDataStorage_QueryFileDelegate)Marshal.GetDelegateForFunctionPointer(intPtr408, typeof(EOS_PlayerDataStorage_QueryFileDelegate));
		IntPtr intPtr409 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorage_QueryFileList");
		if (intPtr409 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorage_QueryFileList");
		}
		EOS_PlayerDataStorage_QueryFileList = (EOS_PlayerDataStorage_QueryFileListDelegate)Marshal.GetDelegateForFunctionPointer(intPtr409, typeof(EOS_PlayerDataStorage_QueryFileListDelegate));
		IntPtr intPtr410 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorage_ReadFile");
		if (intPtr410 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorage_ReadFile");
		}
		EOS_PlayerDataStorage_ReadFile = (EOS_PlayerDataStorage_ReadFileDelegate)Marshal.GetDelegateForFunctionPointer(intPtr410, typeof(EOS_PlayerDataStorage_ReadFileDelegate));
		IntPtr intPtr411 = getFunctionPointer(libraryHandle, "EOS_PlayerDataStorage_WriteFile");
		if (intPtr411 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PlayerDataStorage_WriteFile");
		}
		EOS_PlayerDataStorage_WriteFile = (EOS_PlayerDataStorage_WriteFileDelegate)Marshal.GetDelegateForFunctionPointer(intPtr411, typeof(EOS_PlayerDataStorage_WriteFileDelegate));
		IntPtr intPtr412 = getFunctionPointer(libraryHandle, "EOS_PresenceModification_DeleteData");
		if (intPtr412 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PresenceModification_DeleteData");
		}
		EOS_PresenceModification_DeleteData = (EOS_PresenceModification_DeleteDataDelegate)Marshal.GetDelegateForFunctionPointer(intPtr412, typeof(EOS_PresenceModification_DeleteDataDelegate));
		IntPtr intPtr413 = getFunctionPointer(libraryHandle, "EOS_PresenceModification_Release");
		if (intPtr413 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PresenceModification_Release");
		}
		EOS_PresenceModification_Release = (EOS_PresenceModification_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr413, typeof(EOS_PresenceModification_ReleaseDelegate));
		IntPtr intPtr414 = getFunctionPointer(libraryHandle, "EOS_PresenceModification_SetData");
		if (intPtr414 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PresenceModification_SetData");
		}
		EOS_PresenceModification_SetData = (EOS_PresenceModification_SetDataDelegate)Marshal.GetDelegateForFunctionPointer(intPtr414, typeof(EOS_PresenceModification_SetDataDelegate));
		IntPtr intPtr415 = getFunctionPointer(libraryHandle, "EOS_PresenceModification_SetJoinInfo");
		if (intPtr415 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PresenceModification_SetJoinInfo");
		}
		EOS_PresenceModification_SetJoinInfo = (EOS_PresenceModification_SetJoinInfoDelegate)Marshal.GetDelegateForFunctionPointer(intPtr415, typeof(EOS_PresenceModification_SetJoinInfoDelegate));
		IntPtr intPtr416 = getFunctionPointer(libraryHandle, "EOS_PresenceModification_SetRawRichText");
		if (intPtr416 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PresenceModification_SetRawRichText");
		}
		EOS_PresenceModification_SetRawRichText = (EOS_PresenceModification_SetRawRichTextDelegate)Marshal.GetDelegateForFunctionPointer(intPtr416, typeof(EOS_PresenceModification_SetRawRichTextDelegate));
		IntPtr intPtr417 = getFunctionPointer(libraryHandle, "EOS_PresenceModification_SetStatus");
		if (intPtr417 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_PresenceModification_SetStatus");
		}
		EOS_PresenceModification_SetStatus = (EOS_PresenceModification_SetStatusDelegate)Marshal.GetDelegateForFunctionPointer(intPtr417, typeof(EOS_PresenceModification_SetStatusDelegate));
		IntPtr intPtr418 = getFunctionPointer(libraryHandle, "EOS_Presence_AddNotifyJoinGameAccepted");
		if (intPtr418 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Presence_AddNotifyJoinGameAccepted");
		}
		EOS_Presence_AddNotifyJoinGameAccepted = (EOS_Presence_AddNotifyJoinGameAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr418, typeof(EOS_Presence_AddNotifyJoinGameAcceptedDelegate));
		IntPtr intPtr419 = getFunctionPointer(libraryHandle, "EOS_Presence_AddNotifyOnPresenceChanged");
		if (intPtr419 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Presence_AddNotifyOnPresenceChanged");
		}
		EOS_Presence_AddNotifyOnPresenceChanged = (EOS_Presence_AddNotifyOnPresenceChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr419, typeof(EOS_Presence_AddNotifyOnPresenceChangedDelegate));
		IntPtr intPtr420 = getFunctionPointer(libraryHandle, "EOS_Presence_CopyPresence");
		if (intPtr420 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Presence_CopyPresence");
		}
		EOS_Presence_CopyPresence = (EOS_Presence_CopyPresenceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr420, typeof(EOS_Presence_CopyPresenceDelegate));
		IntPtr intPtr421 = getFunctionPointer(libraryHandle, "EOS_Presence_CreatePresenceModification");
		if (intPtr421 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Presence_CreatePresenceModification");
		}
		EOS_Presence_CreatePresenceModification = (EOS_Presence_CreatePresenceModificationDelegate)Marshal.GetDelegateForFunctionPointer(intPtr421, typeof(EOS_Presence_CreatePresenceModificationDelegate));
		IntPtr intPtr422 = getFunctionPointer(libraryHandle, "EOS_Presence_GetJoinInfo");
		if (intPtr422 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Presence_GetJoinInfo");
		}
		EOS_Presence_GetJoinInfo = (EOS_Presence_GetJoinInfoDelegate)Marshal.GetDelegateForFunctionPointer(intPtr422, typeof(EOS_Presence_GetJoinInfoDelegate));
		IntPtr intPtr423 = getFunctionPointer(libraryHandle, "EOS_Presence_HasPresence");
		if (intPtr423 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Presence_HasPresence");
		}
		EOS_Presence_HasPresence = (EOS_Presence_HasPresenceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr423, typeof(EOS_Presence_HasPresenceDelegate));
		IntPtr intPtr424 = getFunctionPointer(libraryHandle, "EOS_Presence_Info_Release");
		if (intPtr424 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Presence_Info_Release");
		}
		EOS_Presence_Info_Release = (EOS_Presence_Info_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr424, typeof(EOS_Presence_Info_ReleaseDelegate));
		IntPtr intPtr425 = getFunctionPointer(libraryHandle, "EOS_Presence_QueryPresence");
		if (intPtr425 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Presence_QueryPresence");
		}
		EOS_Presence_QueryPresence = (EOS_Presence_QueryPresenceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr425, typeof(EOS_Presence_QueryPresenceDelegate));
		IntPtr intPtr426 = getFunctionPointer(libraryHandle, "EOS_Presence_RemoveNotifyJoinGameAccepted");
		if (intPtr426 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Presence_RemoveNotifyJoinGameAccepted");
		}
		EOS_Presence_RemoveNotifyJoinGameAccepted = (EOS_Presence_RemoveNotifyJoinGameAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr426, typeof(EOS_Presence_RemoveNotifyJoinGameAcceptedDelegate));
		IntPtr intPtr427 = getFunctionPointer(libraryHandle, "EOS_Presence_RemoveNotifyOnPresenceChanged");
		if (intPtr427 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Presence_RemoveNotifyOnPresenceChanged");
		}
		EOS_Presence_RemoveNotifyOnPresenceChanged = (EOS_Presence_RemoveNotifyOnPresenceChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr427, typeof(EOS_Presence_RemoveNotifyOnPresenceChangedDelegate));
		IntPtr intPtr428 = getFunctionPointer(libraryHandle, "EOS_Presence_SetPresence");
		if (intPtr428 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Presence_SetPresence");
		}
		EOS_Presence_SetPresence = (EOS_Presence_SetPresenceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr428, typeof(EOS_Presence_SetPresenceDelegate));
		IntPtr intPtr429 = getFunctionPointer(libraryHandle, "EOS_ProductUserId_FromString");
		if (intPtr429 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ProductUserId_FromString");
		}
		EOS_ProductUserId_FromString = (EOS_ProductUserId_FromStringDelegate)Marshal.GetDelegateForFunctionPointer(intPtr429, typeof(EOS_ProductUserId_FromStringDelegate));
		IntPtr intPtr430 = getFunctionPointer(libraryHandle, "EOS_ProductUserId_IsValid");
		if (intPtr430 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ProductUserId_IsValid");
		}
		EOS_ProductUserId_IsValid = (EOS_ProductUserId_IsValidDelegate)Marshal.GetDelegateForFunctionPointer(intPtr430, typeof(EOS_ProductUserId_IsValidDelegate));
		IntPtr intPtr431 = getFunctionPointer(libraryHandle, "EOS_ProductUserId_ToString");
		if (intPtr431 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ProductUserId_ToString");
		}
		EOS_ProductUserId_ToString = (EOS_ProductUserId_ToStringDelegate)Marshal.GetDelegateForFunctionPointer(intPtr431, typeof(EOS_ProductUserId_ToStringDelegate));
		IntPtr intPtr432 = getFunctionPointer(libraryHandle, "EOS_ProgressionSnapshot_AddProgression");
		if (intPtr432 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ProgressionSnapshot_AddProgression");
		}
		EOS_ProgressionSnapshot_AddProgression = (EOS_ProgressionSnapshot_AddProgressionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr432, typeof(EOS_ProgressionSnapshot_AddProgressionDelegate));
		IntPtr intPtr433 = getFunctionPointer(libraryHandle, "EOS_ProgressionSnapshot_BeginSnapshot");
		if (intPtr433 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ProgressionSnapshot_BeginSnapshot");
		}
		EOS_ProgressionSnapshot_BeginSnapshot = (EOS_ProgressionSnapshot_BeginSnapshotDelegate)Marshal.GetDelegateForFunctionPointer(intPtr433, typeof(EOS_ProgressionSnapshot_BeginSnapshotDelegate));
		IntPtr intPtr434 = getFunctionPointer(libraryHandle, "EOS_ProgressionSnapshot_DeleteSnapshot");
		if (intPtr434 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ProgressionSnapshot_DeleteSnapshot");
		}
		EOS_ProgressionSnapshot_DeleteSnapshot = (EOS_ProgressionSnapshot_DeleteSnapshotDelegate)Marshal.GetDelegateForFunctionPointer(intPtr434, typeof(EOS_ProgressionSnapshot_DeleteSnapshotDelegate));
		IntPtr intPtr435 = getFunctionPointer(libraryHandle, "EOS_ProgressionSnapshot_EndSnapshot");
		if (intPtr435 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ProgressionSnapshot_EndSnapshot");
		}
		EOS_ProgressionSnapshot_EndSnapshot = (EOS_ProgressionSnapshot_EndSnapshotDelegate)Marshal.GetDelegateForFunctionPointer(intPtr435, typeof(EOS_ProgressionSnapshot_EndSnapshotDelegate));
		IntPtr intPtr436 = getFunctionPointer(libraryHandle, "EOS_ProgressionSnapshot_SubmitSnapshot");
		if (intPtr436 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_ProgressionSnapshot_SubmitSnapshot");
		}
		EOS_ProgressionSnapshot_SubmitSnapshot = (EOS_ProgressionSnapshot_SubmitSnapshotDelegate)Marshal.GetDelegateForFunctionPointer(intPtr436, typeof(EOS_ProgressionSnapshot_SubmitSnapshotDelegate));
		IntPtr intPtr437 = getFunctionPointer(libraryHandle, "EOS_RTCAdmin_CopyUserTokenByIndex");
		if (intPtr437 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAdmin_CopyUserTokenByIndex");
		}
		EOS_RTCAdmin_CopyUserTokenByIndex = (EOS_RTCAdmin_CopyUserTokenByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr437, typeof(EOS_RTCAdmin_CopyUserTokenByIndexDelegate));
		IntPtr intPtr438 = getFunctionPointer(libraryHandle, "EOS_RTCAdmin_CopyUserTokenByUserId");
		if (intPtr438 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAdmin_CopyUserTokenByUserId");
		}
		EOS_RTCAdmin_CopyUserTokenByUserId = (EOS_RTCAdmin_CopyUserTokenByUserIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr438, typeof(EOS_RTCAdmin_CopyUserTokenByUserIdDelegate));
		IntPtr intPtr439 = getFunctionPointer(libraryHandle, "EOS_RTCAdmin_Kick");
		if (intPtr439 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAdmin_Kick");
		}
		EOS_RTCAdmin_Kick = (EOS_RTCAdmin_KickDelegate)Marshal.GetDelegateForFunctionPointer(intPtr439, typeof(EOS_RTCAdmin_KickDelegate));
		IntPtr intPtr440 = getFunctionPointer(libraryHandle, "EOS_RTCAdmin_QueryJoinRoomToken");
		if (intPtr440 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAdmin_QueryJoinRoomToken");
		}
		EOS_RTCAdmin_QueryJoinRoomToken = (EOS_RTCAdmin_QueryJoinRoomTokenDelegate)Marshal.GetDelegateForFunctionPointer(intPtr440, typeof(EOS_RTCAdmin_QueryJoinRoomTokenDelegate));
		IntPtr intPtr441 = getFunctionPointer(libraryHandle, "EOS_RTCAdmin_SetParticipantHardMute");
		if (intPtr441 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAdmin_SetParticipantHardMute");
		}
		EOS_RTCAdmin_SetParticipantHardMute = (EOS_RTCAdmin_SetParticipantHardMuteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr441, typeof(EOS_RTCAdmin_SetParticipantHardMuteDelegate));
		IntPtr intPtr442 = getFunctionPointer(libraryHandle, "EOS_RTCAdmin_UserToken_Release");
		if (intPtr442 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAdmin_UserToken_Release");
		}
		EOS_RTCAdmin_UserToken_Release = (EOS_RTCAdmin_UserToken_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr442, typeof(EOS_RTCAdmin_UserToken_ReleaseDelegate));
		IntPtr intPtr443 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_AddNotifyAudioBeforeRender");
		if (intPtr443 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_AddNotifyAudioBeforeRender");
		}
		EOS_RTCAudio_AddNotifyAudioBeforeRender = (EOS_RTCAudio_AddNotifyAudioBeforeRenderDelegate)Marshal.GetDelegateForFunctionPointer(intPtr443, typeof(EOS_RTCAudio_AddNotifyAudioBeforeRenderDelegate));
		IntPtr intPtr444 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_AddNotifyAudioBeforeSend");
		if (intPtr444 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_AddNotifyAudioBeforeSend");
		}
		EOS_RTCAudio_AddNotifyAudioBeforeSend = (EOS_RTCAudio_AddNotifyAudioBeforeSendDelegate)Marshal.GetDelegateForFunctionPointer(intPtr444, typeof(EOS_RTCAudio_AddNotifyAudioBeforeSendDelegate));
		IntPtr intPtr445 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_AddNotifyAudioDevicesChanged");
		if (intPtr445 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_AddNotifyAudioDevicesChanged");
		}
		EOS_RTCAudio_AddNotifyAudioDevicesChanged = (EOS_RTCAudio_AddNotifyAudioDevicesChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr445, typeof(EOS_RTCAudio_AddNotifyAudioDevicesChangedDelegate));
		IntPtr intPtr446 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_AddNotifyAudioInputState");
		if (intPtr446 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_AddNotifyAudioInputState");
		}
		EOS_RTCAudio_AddNotifyAudioInputState = (EOS_RTCAudio_AddNotifyAudioInputStateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr446, typeof(EOS_RTCAudio_AddNotifyAudioInputStateDelegate));
		IntPtr intPtr447 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_AddNotifyAudioOutputState");
		if (intPtr447 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_AddNotifyAudioOutputState");
		}
		EOS_RTCAudio_AddNotifyAudioOutputState = (EOS_RTCAudio_AddNotifyAudioOutputStateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr447, typeof(EOS_RTCAudio_AddNotifyAudioOutputStateDelegate));
		IntPtr intPtr448 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_AddNotifyParticipantUpdated");
		if (intPtr448 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_AddNotifyParticipantUpdated");
		}
		EOS_RTCAudio_AddNotifyParticipantUpdated = (EOS_RTCAudio_AddNotifyParticipantUpdatedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr448, typeof(EOS_RTCAudio_AddNotifyParticipantUpdatedDelegate));
		IntPtr intPtr449 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_CopyInputDeviceInformationByIndex");
		if (intPtr449 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_CopyInputDeviceInformationByIndex");
		}
		EOS_RTCAudio_CopyInputDeviceInformationByIndex = (EOS_RTCAudio_CopyInputDeviceInformationByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr449, typeof(EOS_RTCAudio_CopyInputDeviceInformationByIndexDelegate));
		IntPtr intPtr450 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_CopyOutputDeviceInformationByIndex");
		if (intPtr450 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_CopyOutputDeviceInformationByIndex");
		}
		EOS_RTCAudio_CopyOutputDeviceInformationByIndex = (EOS_RTCAudio_CopyOutputDeviceInformationByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr450, typeof(EOS_RTCAudio_CopyOutputDeviceInformationByIndexDelegate));
		IntPtr intPtr451 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_GetAudioInputDeviceByIndex");
		if (intPtr451 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_GetAudioInputDeviceByIndex");
		}
		EOS_RTCAudio_GetAudioInputDeviceByIndex = (EOS_RTCAudio_GetAudioInputDeviceByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr451, typeof(EOS_RTCAudio_GetAudioInputDeviceByIndexDelegate));
		IntPtr intPtr452 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_GetAudioInputDevicesCount");
		if (intPtr452 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_GetAudioInputDevicesCount");
		}
		EOS_RTCAudio_GetAudioInputDevicesCount = (EOS_RTCAudio_GetAudioInputDevicesCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr452, typeof(EOS_RTCAudio_GetAudioInputDevicesCountDelegate));
		IntPtr intPtr453 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_GetAudioOutputDeviceByIndex");
		if (intPtr453 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_GetAudioOutputDeviceByIndex");
		}
		EOS_RTCAudio_GetAudioOutputDeviceByIndex = (EOS_RTCAudio_GetAudioOutputDeviceByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr453, typeof(EOS_RTCAudio_GetAudioOutputDeviceByIndexDelegate));
		IntPtr intPtr454 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_GetAudioOutputDevicesCount");
		if (intPtr454 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_GetAudioOutputDevicesCount");
		}
		EOS_RTCAudio_GetAudioOutputDevicesCount = (EOS_RTCAudio_GetAudioOutputDevicesCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr454, typeof(EOS_RTCAudio_GetAudioOutputDevicesCountDelegate));
		IntPtr intPtr455 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_GetInputDevicesCount");
		if (intPtr455 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_GetInputDevicesCount");
		}
		EOS_RTCAudio_GetInputDevicesCount = (EOS_RTCAudio_GetInputDevicesCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr455, typeof(EOS_RTCAudio_GetInputDevicesCountDelegate));
		IntPtr intPtr456 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_GetOutputDevicesCount");
		if (intPtr456 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_GetOutputDevicesCount");
		}
		EOS_RTCAudio_GetOutputDevicesCount = (EOS_RTCAudio_GetOutputDevicesCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr456, typeof(EOS_RTCAudio_GetOutputDevicesCountDelegate));
		IntPtr intPtr457 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_InputDeviceInformation_Release");
		if (intPtr457 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_InputDeviceInformation_Release");
		}
		EOS_RTCAudio_InputDeviceInformation_Release = (EOS_RTCAudio_InputDeviceInformation_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr457, typeof(EOS_RTCAudio_InputDeviceInformation_ReleaseDelegate));
		IntPtr intPtr458 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_OutputDeviceInformation_Release");
		if (intPtr458 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_OutputDeviceInformation_Release");
		}
		EOS_RTCAudio_OutputDeviceInformation_Release = (EOS_RTCAudio_OutputDeviceInformation_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr458, typeof(EOS_RTCAudio_OutputDeviceInformation_ReleaseDelegate));
		IntPtr intPtr459 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_QueryInputDevicesInformation");
		if (intPtr459 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_QueryInputDevicesInformation");
		}
		EOS_RTCAudio_QueryInputDevicesInformation = (EOS_RTCAudio_QueryInputDevicesInformationDelegate)Marshal.GetDelegateForFunctionPointer(intPtr459, typeof(EOS_RTCAudio_QueryInputDevicesInformationDelegate));
		IntPtr intPtr460 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_QueryOutputDevicesInformation");
		if (intPtr460 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_QueryOutputDevicesInformation");
		}
		EOS_RTCAudio_QueryOutputDevicesInformation = (EOS_RTCAudio_QueryOutputDevicesInformationDelegate)Marshal.GetDelegateForFunctionPointer(intPtr460, typeof(EOS_RTCAudio_QueryOutputDevicesInformationDelegate));
		IntPtr intPtr461 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_RegisterPlatformAudioUser");
		if (intPtr461 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_RegisterPlatformAudioUser");
		}
		EOS_RTCAudio_RegisterPlatformAudioUser = (EOS_RTCAudio_RegisterPlatformAudioUserDelegate)Marshal.GetDelegateForFunctionPointer(intPtr461, typeof(EOS_RTCAudio_RegisterPlatformAudioUserDelegate));
		IntPtr intPtr462 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_RegisterPlatformUser");
		if (intPtr462 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_RegisterPlatformUser");
		}
		EOS_RTCAudio_RegisterPlatformUser = (EOS_RTCAudio_RegisterPlatformUserDelegate)Marshal.GetDelegateForFunctionPointer(intPtr462, typeof(EOS_RTCAudio_RegisterPlatformUserDelegate));
		IntPtr intPtr463 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_RemoveNotifyAudioBeforeRender");
		if (intPtr463 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_RemoveNotifyAudioBeforeRender");
		}
		EOS_RTCAudio_RemoveNotifyAudioBeforeRender = (EOS_RTCAudio_RemoveNotifyAudioBeforeRenderDelegate)Marshal.GetDelegateForFunctionPointer(intPtr463, typeof(EOS_RTCAudio_RemoveNotifyAudioBeforeRenderDelegate));
		IntPtr intPtr464 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_RemoveNotifyAudioBeforeSend");
		if (intPtr464 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_RemoveNotifyAudioBeforeSend");
		}
		EOS_RTCAudio_RemoveNotifyAudioBeforeSend = (EOS_RTCAudio_RemoveNotifyAudioBeforeSendDelegate)Marshal.GetDelegateForFunctionPointer(intPtr464, typeof(EOS_RTCAudio_RemoveNotifyAudioBeforeSendDelegate));
		IntPtr intPtr465 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_RemoveNotifyAudioDevicesChanged");
		if (intPtr465 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_RemoveNotifyAudioDevicesChanged");
		}
		EOS_RTCAudio_RemoveNotifyAudioDevicesChanged = (EOS_RTCAudio_RemoveNotifyAudioDevicesChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr465, typeof(EOS_RTCAudio_RemoveNotifyAudioDevicesChangedDelegate));
		IntPtr intPtr466 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_RemoveNotifyAudioInputState");
		if (intPtr466 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_RemoveNotifyAudioInputState");
		}
		EOS_RTCAudio_RemoveNotifyAudioInputState = (EOS_RTCAudio_RemoveNotifyAudioInputStateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr466, typeof(EOS_RTCAudio_RemoveNotifyAudioInputStateDelegate));
		IntPtr intPtr467 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_RemoveNotifyAudioOutputState");
		if (intPtr467 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_RemoveNotifyAudioOutputState");
		}
		EOS_RTCAudio_RemoveNotifyAudioOutputState = (EOS_RTCAudio_RemoveNotifyAudioOutputStateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr467, typeof(EOS_RTCAudio_RemoveNotifyAudioOutputStateDelegate));
		IntPtr intPtr468 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_RemoveNotifyParticipantUpdated");
		if (intPtr468 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_RemoveNotifyParticipantUpdated");
		}
		EOS_RTCAudio_RemoveNotifyParticipantUpdated = (EOS_RTCAudio_RemoveNotifyParticipantUpdatedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr468, typeof(EOS_RTCAudio_RemoveNotifyParticipantUpdatedDelegate));
		IntPtr intPtr469 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_SendAudio");
		if (intPtr469 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_SendAudio");
		}
		EOS_RTCAudio_SendAudio = (EOS_RTCAudio_SendAudioDelegate)Marshal.GetDelegateForFunctionPointer(intPtr469, typeof(EOS_RTCAudio_SendAudioDelegate));
		IntPtr intPtr470 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_SetAudioInputSettings");
		if (intPtr470 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_SetAudioInputSettings");
		}
		EOS_RTCAudio_SetAudioInputSettings = (EOS_RTCAudio_SetAudioInputSettingsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr470, typeof(EOS_RTCAudio_SetAudioInputSettingsDelegate));
		IntPtr intPtr471 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_SetAudioOutputSettings");
		if (intPtr471 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_SetAudioOutputSettings");
		}
		EOS_RTCAudio_SetAudioOutputSettings = (EOS_RTCAudio_SetAudioOutputSettingsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr471, typeof(EOS_RTCAudio_SetAudioOutputSettingsDelegate));
		IntPtr intPtr472 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_SetInputDeviceSettings");
		if (intPtr472 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_SetInputDeviceSettings");
		}
		EOS_RTCAudio_SetInputDeviceSettings = (EOS_RTCAudio_SetInputDeviceSettingsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr472, typeof(EOS_RTCAudio_SetInputDeviceSettingsDelegate));
		IntPtr intPtr473 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_SetOutputDeviceSettings");
		if (intPtr473 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_SetOutputDeviceSettings");
		}
		EOS_RTCAudio_SetOutputDeviceSettings = (EOS_RTCAudio_SetOutputDeviceSettingsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr473, typeof(EOS_RTCAudio_SetOutputDeviceSettingsDelegate));
		IntPtr intPtr474 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_UnregisterPlatformAudioUser");
		if (intPtr474 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_UnregisterPlatformAudioUser");
		}
		EOS_RTCAudio_UnregisterPlatformAudioUser = (EOS_RTCAudio_UnregisterPlatformAudioUserDelegate)Marshal.GetDelegateForFunctionPointer(intPtr474, typeof(EOS_RTCAudio_UnregisterPlatformAudioUserDelegate));
		IntPtr intPtr475 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_UnregisterPlatformUser");
		if (intPtr475 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_UnregisterPlatformUser");
		}
		EOS_RTCAudio_UnregisterPlatformUser = (EOS_RTCAudio_UnregisterPlatformUserDelegate)Marshal.GetDelegateForFunctionPointer(intPtr475, typeof(EOS_RTCAudio_UnregisterPlatformUserDelegate));
		IntPtr intPtr476 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_UpdateParticipantVolume");
		if (intPtr476 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_UpdateParticipantVolume");
		}
		EOS_RTCAudio_UpdateParticipantVolume = (EOS_RTCAudio_UpdateParticipantVolumeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr476, typeof(EOS_RTCAudio_UpdateParticipantVolumeDelegate));
		IntPtr intPtr477 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_UpdateReceiving");
		if (intPtr477 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_UpdateReceiving");
		}
		EOS_RTCAudio_UpdateReceiving = (EOS_RTCAudio_UpdateReceivingDelegate)Marshal.GetDelegateForFunctionPointer(intPtr477, typeof(EOS_RTCAudio_UpdateReceivingDelegate));
		IntPtr intPtr478 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_UpdateReceivingVolume");
		if (intPtr478 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_UpdateReceivingVolume");
		}
		EOS_RTCAudio_UpdateReceivingVolume = (EOS_RTCAudio_UpdateReceivingVolumeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr478, typeof(EOS_RTCAudio_UpdateReceivingVolumeDelegate));
		IntPtr intPtr479 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_UpdateSending");
		if (intPtr479 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_UpdateSending");
		}
		EOS_RTCAudio_UpdateSending = (EOS_RTCAudio_UpdateSendingDelegate)Marshal.GetDelegateForFunctionPointer(intPtr479, typeof(EOS_RTCAudio_UpdateSendingDelegate));
		IntPtr intPtr480 = getFunctionPointer(libraryHandle, "EOS_RTCAudio_UpdateSendingVolume");
		if (intPtr480 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTCAudio_UpdateSendingVolume");
		}
		EOS_RTCAudio_UpdateSendingVolume = (EOS_RTCAudio_UpdateSendingVolumeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr480, typeof(EOS_RTCAudio_UpdateSendingVolumeDelegate));
		IntPtr intPtr481 = getFunctionPointer(libraryHandle, "EOS_RTC_AddNotifyDisconnected");
		if (intPtr481 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_AddNotifyDisconnected");
		}
		EOS_RTC_AddNotifyDisconnected = (EOS_RTC_AddNotifyDisconnectedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr481, typeof(EOS_RTC_AddNotifyDisconnectedDelegate));
		IntPtr intPtr482 = getFunctionPointer(libraryHandle, "EOS_RTC_AddNotifyParticipantStatusChanged");
		if (intPtr482 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_AddNotifyParticipantStatusChanged");
		}
		EOS_RTC_AddNotifyParticipantStatusChanged = (EOS_RTC_AddNotifyParticipantStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr482, typeof(EOS_RTC_AddNotifyParticipantStatusChangedDelegate));
		IntPtr intPtr483 = getFunctionPointer(libraryHandle, "EOS_RTC_AddNotifyRoomStatisticsUpdated");
		if (intPtr483 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_AddNotifyRoomStatisticsUpdated");
		}
		EOS_RTC_AddNotifyRoomStatisticsUpdated = (EOS_RTC_AddNotifyRoomStatisticsUpdatedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr483, typeof(EOS_RTC_AddNotifyRoomStatisticsUpdatedDelegate));
		IntPtr intPtr484 = getFunctionPointer(libraryHandle, "EOS_RTC_BlockParticipant");
		if (intPtr484 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_BlockParticipant");
		}
		EOS_RTC_BlockParticipant = (EOS_RTC_BlockParticipantDelegate)Marshal.GetDelegateForFunctionPointer(intPtr484, typeof(EOS_RTC_BlockParticipantDelegate));
		IntPtr intPtr485 = getFunctionPointer(libraryHandle, "EOS_RTC_GetAudioInterface");
		if (intPtr485 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_GetAudioInterface");
		}
		EOS_RTC_GetAudioInterface = (EOS_RTC_GetAudioInterfaceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr485, typeof(EOS_RTC_GetAudioInterfaceDelegate));
		IntPtr intPtr486 = getFunctionPointer(libraryHandle, "EOS_RTC_JoinRoom");
		if (intPtr486 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_JoinRoom");
		}
		EOS_RTC_JoinRoom = (EOS_RTC_JoinRoomDelegate)Marshal.GetDelegateForFunctionPointer(intPtr486, typeof(EOS_RTC_JoinRoomDelegate));
		IntPtr intPtr487 = getFunctionPointer(libraryHandle, "EOS_RTC_LeaveRoom");
		if (intPtr487 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_LeaveRoom");
		}
		EOS_RTC_LeaveRoom = (EOS_RTC_LeaveRoomDelegate)Marshal.GetDelegateForFunctionPointer(intPtr487, typeof(EOS_RTC_LeaveRoomDelegate));
		IntPtr intPtr488 = getFunctionPointer(libraryHandle, "EOS_RTC_RemoveNotifyDisconnected");
		if (intPtr488 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_RemoveNotifyDisconnected");
		}
		EOS_RTC_RemoveNotifyDisconnected = (EOS_RTC_RemoveNotifyDisconnectedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr488, typeof(EOS_RTC_RemoveNotifyDisconnectedDelegate));
		IntPtr intPtr489 = getFunctionPointer(libraryHandle, "EOS_RTC_RemoveNotifyParticipantStatusChanged");
		if (intPtr489 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_RemoveNotifyParticipantStatusChanged");
		}
		EOS_RTC_RemoveNotifyParticipantStatusChanged = (EOS_RTC_RemoveNotifyParticipantStatusChangedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr489, typeof(EOS_RTC_RemoveNotifyParticipantStatusChangedDelegate));
		IntPtr intPtr490 = getFunctionPointer(libraryHandle, "EOS_RTC_RemoveNotifyRoomStatisticsUpdated");
		if (intPtr490 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_RemoveNotifyRoomStatisticsUpdated");
		}
		EOS_RTC_RemoveNotifyRoomStatisticsUpdated = (EOS_RTC_RemoveNotifyRoomStatisticsUpdatedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr490, typeof(EOS_RTC_RemoveNotifyRoomStatisticsUpdatedDelegate));
		IntPtr intPtr491 = getFunctionPointer(libraryHandle, "EOS_RTC_SetRoomSetting");
		if (intPtr491 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_SetRoomSetting");
		}
		EOS_RTC_SetRoomSetting = (EOS_RTC_SetRoomSettingDelegate)Marshal.GetDelegateForFunctionPointer(intPtr491, typeof(EOS_RTC_SetRoomSettingDelegate));
		IntPtr intPtr492 = getFunctionPointer(libraryHandle, "EOS_RTC_SetSetting");
		if (intPtr492 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_RTC_SetSetting");
		}
		EOS_RTC_SetSetting = (EOS_RTC_SetSettingDelegate)Marshal.GetDelegateForFunctionPointer(intPtr492, typeof(EOS_RTC_SetSettingDelegate));
		IntPtr intPtr493 = getFunctionPointer(libraryHandle, "EOS_Reports_SendPlayerBehaviorReport");
		if (intPtr493 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Reports_SendPlayerBehaviorReport");
		}
		EOS_Reports_SendPlayerBehaviorReport = (EOS_Reports_SendPlayerBehaviorReportDelegate)Marshal.GetDelegateForFunctionPointer(intPtr493, typeof(EOS_Reports_SendPlayerBehaviorReportDelegate));
		IntPtr intPtr494 = getFunctionPointer(libraryHandle, "EOS_Sanctions_CopyPlayerSanctionByIndex");
		if (intPtr494 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sanctions_CopyPlayerSanctionByIndex");
		}
		EOS_Sanctions_CopyPlayerSanctionByIndex = (EOS_Sanctions_CopyPlayerSanctionByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr494, typeof(EOS_Sanctions_CopyPlayerSanctionByIndexDelegate));
		IntPtr intPtr495 = getFunctionPointer(libraryHandle, "EOS_Sanctions_GetPlayerSanctionCount");
		if (intPtr495 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sanctions_GetPlayerSanctionCount");
		}
		EOS_Sanctions_GetPlayerSanctionCount = (EOS_Sanctions_GetPlayerSanctionCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr495, typeof(EOS_Sanctions_GetPlayerSanctionCountDelegate));
		IntPtr intPtr496 = getFunctionPointer(libraryHandle, "EOS_Sanctions_PlayerSanction_Release");
		if (intPtr496 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sanctions_PlayerSanction_Release");
		}
		EOS_Sanctions_PlayerSanction_Release = (EOS_Sanctions_PlayerSanction_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr496, typeof(EOS_Sanctions_PlayerSanction_ReleaseDelegate));
		IntPtr intPtr497 = getFunctionPointer(libraryHandle, "EOS_Sanctions_QueryActivePlayerSanctions");
		if (intPtr497 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sanctions_QueryActivePlayerSanctions");
		}
		EOS_Sanctions_QueryActivePlayerSanctions = (EOS_Sanctions_QueryActivePlayerSanctionsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr497, typeof(EOS_Sanctions_QueryActivePlayerSanctionsDelegate));
		IntPtr intPtr498 = getFunctionPointer(libraryHandle, "EOS_SessionDetails_Attribute_Release");
		if (intPtr498 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionDetails_Attribute_Release");
		}
		EOS_SessionDetails_Attribute_Release = (EOS_SessionDetails_Attribute_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr498, typeof(EOS_SessionDetails_Attribute_ReleaseDelegate));
		IntPtr intPtr499 = getFunctionPointer(libraryHandle, "EOS_SessionDetails_CopyInfo");
		if (intPtr499 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionDetails_CopyInfo");
		}
		EOS_SessionDetails_CopyInfo = (EOS_SessionDetails_CopyInfoDelegate)Marshal.GetDelegateForFunctionPointer(intPtr499, typeof(EOS_SessionDetails_CopyInfoDelegate));
		IntPtr intPtr500 = getFunctionPointer(libraryHandle, "EOS_SessionDetails_CopySessionAttributeByIndex");
		if (intPtr500 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionDetails_CopySessionAttributeByIndex");
		}
		EOS_SessionDetails_CopySessionAttributeByIndex = (EOS_SessionDetails_CopySessionAttributeByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr500, typeof(EOS_SessionDetails_CopySessionAttributeByIndexDelegate));
		IntPtr intPtr501 = getFunctionPointer(libraryHandle, "EOS_SessionDetails_CopySessionAttributeByKey");
		if (intPtr501 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionDetails_CopySessionAttributeByKey");
		}
		EOS_SessionDetails_CopySessionAttributeByKey = (EOS_SessionDetails_CopySessionAttributeByKeyDelegate)Marshal.GetDelegateForFunctionPointer(intPtr501, typeof(EOS_SessionDetails_CopySessionAttributeByKeyDelegate));
		IntPtr intPtr502 = getFunctionPointer(libraryHandle, "EOS_SessionDetails_GetSessionAttributeCount");
		if (intPtr502 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionDetails_GetSessionAttributeCount");
		}
		EOS_SessionDetails_GetSessionAttributeCount = (EOS_SessionDetails_GetSessionAttributeCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr502, typeof(EOS_SessionDetails_GetSessionAttributeCountDelegate));
		IntPtr intPtr503 = getFunctionPointer(libraryHandle, "EOS_SessionDetails_Info_Release");
		if (intPtr503 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionDetails_Info_Release");
		}
		EOS_SessionDetails_Info_Release = (EOS_SessionDetails_Info_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr503, typeof(EOS_SessionDetails_Info_ReleaseDelegate));
		IntPtr intPtr504 = getFunctionPointer(libraryHandle, "EOS_SessionDetails_Release");
		if (intPtr504 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionDetails_Release");
		}
		EOS_SessionDetails_Release = (EOS_SessionDetails_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr504, typeof(EOS_SessionDetails_ReleaseDelegate));
		IntPtr intPtr505 = getFunctionPointer(libraryHandle, "EOS_SessionModification_AddAttribute");
		if (intPtr505 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionModification_AddAttribute");
		}
		EOS_SessionModification_AddAttribute = (EOS_SessionModification_AddAttributeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr505, typeof(EOS_SessionModification_AddAttributeDelegate));
		IntPtr intPtr506 = getFunctionPointer(libraryHandle, "EOS_SessionModification_Release");
		if (intPtr506 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionModification_Release");
		}
		EOS_SessionModification_Release = (EOS_SessionModification_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr506, typeof(EOS_SessionModification_ReleaseDelegate));
		IntPtr intPtr507 = getFunctionPointer(libraryHandle, "EOS_SessionModification_RemoveAttribute");
		if (intPtr507 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionModification_RemoveAttribute");
		}
		EOS_SessionModification_RemoveAttribute = (EOS_SessionModification_RemoveAttributeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr507, typeof(EOS_SessionModification_RemoveAttributeDelegate));
		IntPtr intPtr508 = getFunctionPointer(libraryHandle, "EOS_SessionModification_SetAllowedPlatformIds");
		if (intPtr508 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionModification_SetAllowedPlatformIds");
		}
		EOS_SessionModification_SetAllowedPlatformIds = (EOS_SessionModification_SetAllowedPlatformIdsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr508, typeof(EOS_SessionModification_SetAllowedPlatformIdsDelegate));
		IntPtr intPtr509 = getFunctionPointer(libraryHandle, "EOS_SessionModification_SetBucketId");
		if (intPtr509 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionModification_SetBucketId");
		}
		EOS_SessionModification_SetBucketId = (EOS_SessionModification_SetBucketIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr509, typeof(EOS_SessionModification_SetBucketIdDelegate));
		IntPtr intPtr510 = getFunctionPointer(libraryHandle, "EOS_SessionModification_SetHostAddress");
		if (intPtr510 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionModification_SetHostAddress");
		}
		EOS_SessionModification_SetHostAddress = (EOS_SessionModification_SetHostAddressDelegate)Marshal.GetDelegateForFunctionPointer(intPtr510, typeof(EOS_SessionModification_SetHostAddressDelegate));
		IntPtr intPtr511 = getFunctionPointer(libraryHandle, "EOS_SessionModification_SetInvitesAllowed");
		if (intPtr511 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionModification_SetInvitesAllowed");
		}
		EOS_SessionModification_SetInvitesAllowed = (EOS_SessionModification_SetInvitesAllowedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr511, typeof(EOS_SessionModification_SetInvitesAllowedDelegate));
		IntPtr intPtr512 = getFunctionPointer(libraryHandle, "EOS_SessionModification_SetJoinInProgressAllowed");
		if (intPtr512 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionModification_SetJoinInProgressAllowed");
		}
		EOS_SessionModification_SetJoinInProgressAllowed = (EOS_SessionModification_SetJoinInProgressAllowedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr512, typeof(EOS_SessionModification_SetJoinInProgressAllowedDelegate));
		IntPtr intPtr513 = getFunctionPointer(libraryHandle, "EOS_SessionModification_SetMaxPlayers");
		if (intPtr513 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionModification_SetMaxPlayers");
		}
		EOS_SessionModification_SetMaxPlayers = (EOS_SessionModification_SetMaxPlayersDelegate)Marshal.GetDelegateForFunctionPointer(intPtr513, typeof(EOS_SessionModification_SetMaxPlayersDelegate));
		IntPtr intPtr514 = getFunctionPointer(libraryHandle, "EOS_SessionModification_SetPermissionLevel");
		if (intPtr514 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionModification_SetPermissionLevel");
		}
		EOS_SessionModification_SetPermissionLevel = (EOS_SessionModification_SetPermissionLevelDelegate)Marshal.GetDelegateForFunctionPointer(intPtr514, typeof(EOS_SessionModification_SetPermissionLevelDelegate));
		IntPtr intPtr515 = getFunctionPointer(libraryHandle, "EOS_SessionSearch_CopySearchResultByIndex");
		if (intPtr515 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionSearch_CopySearchResultByIndex");
		}
		EOS_SessionSearch_CopySearchResultByIndex = (EOS_SessionSearch_CopySearchResultByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr515, typeof(EOS_SessionSearch_CopySearchResultByIndexDelegate));
		IntPtr intPtr516 = getFunctionPointer(libraryHandle, "EOS_SessionSearch_Find");
		if (intPtr516 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionSearch_Find");
		}
		EOS_SessionSearch_Find = (EOS_SessionSearch_FindDelegate)Marshal.GetDelegateForFunctionPointer(intPtr516, typeof(EOS_SessionSearch_FindDelegate));
		IntPtr intPtr517 = getFunctionPointer(libraryHandle, "EOS_SessionSearch_GetSearchResultCount");
		if (intPtr517 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionSearch_GetSearchResultCount");
		}
		EOS_SessionSearch_GetSearchResultCount = (EOS_SessionSearch_GetSearchResultCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr517, typeof(EOS_SessionSearch_GetSearchResultCountDelegate));
		IntPtr intPtr518 = getFunctionPointer(libraryHandle, "EOS_SessionSearch_Release");
		if (intPtr518 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionSearch_Release");
		}
		EOS_SessionSearch_Release = (EOS_SessionSearch_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr518, typeof(EOS_SessionSearch_ReleaseDelegate));
		IntPtr intPtr519 = getFunctionPointer(libraryHandle, "EOS_SessionSearch_RemoveParameter");
		if (intPtr519 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionSearch_RemoveParameter");
		}
		EOS_SessionSearch_RemoveParameter = (EOS_SessionSearch_RemoveParameterDelegate)Marshal.GetDelegateForFunctionPointer(intPtr519, typeof(EOS_SessionSearch_RemoveParameterDelegate));
		IntPtr intPtr520 = getFunctionPointer(libraryHandle, "EOS_SessionSearch_SetMaxResults");
		if (intPtr520 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionSearch_SetMaxResults");
		}
		EOS_SessionSearch_SetMaxResults = (EOS_SessionSearch_SetMaxResultsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr520, typeof(EOS_SessionSearch_SetMaxResultsDelegate));
		IntPtr intPtr521 = getFunctionPointer(libraryHandle, "EOS_SessionSearch_SetParameter");
		if (intPtr521 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionSearch_SetParameter");
		}
		EOS_SessionSearch_SetParameter = (EOS_SessionSearch_SetParameterDelegate)Marshal.GetDelegateForFunctionPointer(intPtr521, typeof(EOS_SessionSearch_SetParameterDelegate));
		IntPtr intPtr522 = getFunctionPointer(libraryHandle, "EOS_SessionSearch_SetSessionId");
		if (intPtr522 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionSearch_SetSessionId");
		}
		EOS_SessionSearch_SetSessionId = (EOS_SessionSearch_SetSessionIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr522, typeof(EOS_SessionSearch_SetSessionIdDelegate));
		IntPtr intPtr523 = getFunctionPointer(libraryHandle, "EOS_SessionSearch_SetTargetUserId");
		if (intPtr523 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_SessionSearch_SetTargetUserId");
		}
		EOS_SessionSearch_SetTargetUserId = (EOS_SessionSearch_SetTargetUserIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr523, typeof(EOS_SessionSearch_SetTargetUserIdDelegate));
		IntPtr intPtr524 = getFunctionPointer(libraryHandle, "EOS_Sessions_AddNotifyJoinSessionAccepted");
		if (intPtr524 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_AddNotifyJoinSessionAccepted");
		}
		EOS_Sessions_AddNotifyJoinSessionAccepted = (EOS_Sessions_AddNotifyJoinSessionAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr524, typeof(EOS_Sessions_AddNotifyJoinSessionAcceptedDelegate));
		IntPtr intPtr525 = getFunctionPointer(libraryHandle, "EOS_Sessions_AddNotifyLeaveSessionRequested");
		if (intPtr525 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_AddNotifyLeaveSessionRequested");
		}
		EOS_Sessions_AddNotifyLeaveSessionRequested = (EOS_Sessions_AddNotifyLeaveSessionRequestedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr525, typeof(EOS_Sessions_AddNotifyLeaveSessionRequestedDelegate));
		IntPtr intPtr526 = getFunctionPointer(libraryHandle, "EOS_Sessions_AddNotifySendSessionNativeInviteRequested");
		if (intPtr526 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_AddNotifySendSessionNativeInviteRequested");
		}
		EOS_Sessions_AddNotifySendSessionNativeInviteRequested = (EOS_Sessions_AddNotifySendSessionNativeInviteRequestedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr526, typeof(EOS_Sessions_AddNotifySendSessionNativeInviteRequestedDelegate));
		IntPtr intPtr527 = getFunctionPointer(libraryHandle, "EOS_Sessions_AddNotifySessionInviteAccepted");
		if (intPtr527 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_AddNotifySessionInviteAccepted");
		}
		EOS_Sessions_AddNotifySessionInviteAccepted = (EOS_Sessions_AddNotifySessionInviteAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr527, typeof(EOS_Sessions_AddNotifySessionInviteAcceptedDelegate));
		IntPtr intPtr528 = getFunctionPointer(libraryHandle, "EOS_Sessions_AddNotifySessionInviteReceived");
		if (intPtr528 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_AddNotifySessionInviteReceived");
		}
		EOS_Sessions_AddNotifySessionInviteReceived = (EOS_Sessions_AddNotifySessionInviteReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr528, typeof(EOS_Sessions_AddNotifySessionInviteReceivedDelegate));
		IntPtr intPtr529 = getFunctionPointer(libraryHandle, "EOS_Sessions_AddNotifySessionInviteRejected");
		if (intPtr529 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_AddNotifySessionInviteRejected");
		}
		EOS_Sessions_AddNotifySessionInviteRejected = (EOS_Sessions_AddNotifySessionInviteRejectedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr529, typeof(EOS_Sessions_AddNotifySessionInviteRejectedDelegate));
		IntPtr intPtr530 = getFunctionPointer(libraryHandle, "EOS_Sessions_CopyActiveSessionHandle");
		if (intPtr530 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_CopyActiveSessionHandle");
		}
		EOS_Sessions_CopyActiveSessionHandle = (EOS_Sessions_CopyActiveSessionHandleDelegate)Marshal.GetDelegateForFunctionPointer(intPtr530, typeof(EOS_Sessions_CopyActiveSessionHandleDelegate));
		IntPtr intPtr531 = getFunctionPointer(libraryHandle, "EOS_Sessions_CopySessionHandleByInviteId");
		if (intPtr531 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_CopySessionHandleByInviteId");
		}
		EOS_Sessions_CopySessionHandleByInviteId = (EOS_Sessions_CopySessionHandleByInviteIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr531, typeof(EOS_Sessions_CopySessionHandleByInviteIdDelegate));
		IntPtr intPtr532 = getFunctionPointer(libraryHandle, "EOS_Sessions_CopySessionHandleByUiEventId");
		if (intPtr532 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_CopySessionHandleByUiEventId");
		}
		EOS_Sessions_CopySessionHandleByUiEventId = (EOS_Sessions_CopySessionHandleByUiEventIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr532, typeof(EOS_Sessions_CopySessionHandleByUiEventIdDelegate));
		IntPtr intPtr533 = getFunctionPointer(libraryHandle, "EOS_Sessions_CopySessionHandleForPresence");
		if (intPtr533 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_CopySessionHandleForPresence");
		}
		EOS_Sessions_CopySessionHandleForPresence = (EOS_Sessions_CopySessionHandleForPresenceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr533, typeof(EOS_Sessions_CopySessionHandleForPresenceDelegate));
		IntPtr intPtr534 = getFunctionPointer(libraryHandle, "EOS_Sessions_CreateSessionModification");
		if (intPtr534 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_CreateSessionModification");
		}
		EOS_Sessions_CreateSessionModification = (EOS_Sessions_CreateSessionModificationDelegate)Marshal.GetDelegateForFunctionPointer(intPtr534, typeof(EOS_Sessions_CreateSessionModificationDelegate));
		IntPtr intPtr535 = getFunctionPointer(libraryHandle, "EOS_Sessions_CreateSessionSearch");
		if (intPtr535 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_CreateSessionSearch");
		}
		EOS_Sessions_CreateSessionSearch = (EOS_Sessions_CreateSessionSearchDelegate)Marshal.GetDelegateForFunctionPointer(intPtr535, typeof(EOS_Sessions_CreateSessionSearchDelegate));
		IntPtr intPtr536 = getFunctionPointer(libraryHandle, "EOS_Sessions_DestroySession");
		if (intPtr536 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_DestroySession");
		}
		EOS_Sessions_DestroySession = (EOS_Sessions_DestroySessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr536, typeof(EOS_Sessions_DestroySessionDelegate));
		IntPtr intPtr537 = getFunctionPointer(libraryHandle, "EOS_Sessions_DumpSessionState");
		if (intPtr537 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_DumpSessionState");
		}
		EOS_Sessions_DumpSessionState = (EOS_Sessions_DumpSessionStateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr537, typeof(EOS_Sessions_DumpSessionStateDelegate));
		IntPtr intPtr538 = getFunctionPointer(libraryHandle, "EOS_Sessions_EndSession");
		if (intPtr538 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_EndSession");
		}
		EOS_Sessions_EndSession = (EOS_Sessions_EndSessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr538, typeof(EOS_Sessions_EndSessionDelegate));
		IntPtr intPtr539 = getFunctionPointer(libraryHandle, "EOS_Sessions_GetInviteCount");
		if (intPtr539 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_GetInviteCount");
		}
		EOS_Sessions_GetInviteCount = (EOS_Sessions_GetInviteCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr539, typeof(EOS_Sessions_GetInviteCountDelegate));
		IntPtr intPtr540 = getFunctionPointer(libraryHandle, "EOS_Sessions_GetInviteIdByIndex");
		if (intPtr540 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_GetInviteIdByIndex");
		}
		EOS_Sessions_GetInviteIdByIndex = (EOS_Sessions_GetInviteIdByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr540, typeof(EOS_Sessions_GetInviteIdByIndexDelegate));
		IntPtr intPtr541 = getFunctionPointer(libraryHandle, "EOS_Sessions_IsUserInSession");
		if (intPtr541 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_IsUserInSession");
		}
		EOS_Sessions_IsUserInSession = (EOS_Sessions_IsUserInSessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr541, typeof(EOS_Sessions_IsUserInSessionDelegate));
		IntPtr intPtr542 = getFunctionPointer(libraryHandle, "EOS_Sessions_JoinSession");
		if (intPtr542 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_JoinSession");
		}
		EOS_Sessions_JoinSession = (EOS_Sessions_JoinSessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr542, typeof(EOS_Sessions_JoinSessionDelegate));
		IntPtr intPtr543 = getFunctionPointer(libraryHandle, "EOS_Sessions_QueryInvites");
		if (intPtr543 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_QueryInvites");
		}
		EOS_Sessions_QueryInvites = (EOS_Sessions_QueryInvitesDelegate)Marshal.GetDelegateForFunctionPointer(intPtr543, typeof(EOS_Sessions_QueryInvitesDelegate));
		IntPtr intPtr544 = getFunctionPointer(libraryHandle, "EOS_Sessions_RegisterPlayers");
		if (intPtr544 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_RegisterPlayers");
		}
		EOS_Sessions_RegisterPlayers = (EOS_Sessions_RegisterPlayersDelegate)Marshal.GetDelegateForFunctionPointer(intPtr544, typeof(EOS_Sessions_RegisterPlayersDelegate));
		IntPtr intPtr545 = getFunctionPointer(libraryHandle, "EOS_Sessions_RejectInvite");
		if (intPtr545 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_RejectInvite");
		}
		EOS_Sessions_RejectInvite = (EOS_Sessions_RejectInviteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr545, typeof(EOS_Sessions_RejectInviteDelegate));
		IntPtr intPtr546 = getFunctionPointer(libraryHandle, "EOS_Sessions_RemoveNotifyJoinSessionAccepted");
		if (intPtr546 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_RemoveNotifyJoinSessionAccepted");
		}
		EOS_Sessions_RemoveNotifyJoinSessionAccepted = (EOS_Sessions_RemoveNotifyJoinSessionAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr546, typeof(EOS_Sessions_RemoveNotifyJoinSessionAcceptedDelegate));
		IntPtr intPtr547 = getFunctionPointer(libraryHandle, "EOS_Sessions_RemoveNotifyLeaveSessionRequested");
		if (intPtr547 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_RemoveNotifyLeaveSessionRequested");
		}
		EOS_Sessions_RemoveNotifyLeaveSessionRequested = (EOS_Sessions_RemoveNotifyLeaveSessionRequestedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr547, typeof(EOS_Sessions_RemoveNotifyLeaveSessionRequestedDelegate));
		IntPtr intPtr548 = getFunctionPointer(libraryHandle, "EOS_Sessions_RemoveNotifySendSessionNativeInviteRequested");
		if (intPtr548 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_RemoveNotifySendSessionNativeInviteRequested");
		}
		EOS_Sessions_RemoveNotifySendSessionNativeInviteRequested = (EOS_Sessions_RemoveNotifySendSessionNativeInviteRequestedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr548, typeof(EOS_Sessions_RemoveNotifySendSessionNativeInviteRequestedDelegate));
		IntPtr intPtr549 = getFunctionPointer(libraryHandle, "EOS_Sessions_RemoveNotifySessionInviteAccepted");
		if (intPtr549 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_RemoveNotifySessionInviteAccepted");
		}
		EOS_Sessions_RemoveNotifySessionInviteAccepted = (EOS_Sessions_RemoveNotifySessionInviteAcceptedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr549, typeof(EOS_Sessions_RemoveNotifySessionInviteAcceptedDelegate));
		IntPtr intPtr550 = getFunctionPointer(libraryHandle, "EOS_Sessions_RemoveNotifySessionInviteReceived");
		if (intPtr550 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_RemoveNotifySessionInviteReceived");
		}
		EOS_Sessions_RemoveNotifySessionInviteReceived = (EOS_Sessions_RemoveNotifySessionInviteReceivedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr550, typeof(EOS_Sessions_RemoveNotifySessionInviteReceivedDelegate));
		IntPtr intPtr551 = getFunctionPointer(libraryHandle, "EOS_Sessions_RemoveNotifySessionInviteRejected");
		if (intPtr551 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_RemoveNotifySessionInviteRejected");
		}
		EOS_Sessions_RemoveNotifySessionInviteRejected = (EOS_Sessions_RemoveNotifySessionInviteRejectedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr551, typeof(EOS_Sessions_RemoveNotifySessionInviteRejectedDelegate));
		IntPtr intPtr552 = getFunctionPointer(libraryHandle, "EOS_Sessions_SendInvite");
		if (intPtr552 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_SendInvite");
		}
		EOS_Sessions_SendInvite = (EOS_Sessions_SendInviteDelegate)Marshal.GetDelegateForFunctionPointer(intPtr552, typeof(EOS_Sessions_SendInviteDelegate));
		IntPtr intPtr553 = getFunctionPointer(libraryHandle, "EOS_Sessions_StartSession");
		if (intPtr553 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_StartSession");
		}
		EOS_Sessions_StartSession = (EOS_Sessions_StartSessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr553, typeof(EOS_Sessions_StartSessionDelegate));
		IntPtr intPtr554 = getFunctionPointer(libraryHandle, "EOS_Sessions_UnregisterPlayers");
		if (intPtr554 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_UnregisterPlayers");
		}
		EOS_Sessions_UnregisterPlayers = (EOS_Sessions_UnregisterPlayersDelegate)Marshal.GetDelegateForFunctionPointer(intPtr554, typeof(EOS_Sessions_UnregisterPlayersDelegate));
		IntPtr intPtr555 = getFunctionPointer(libraryHandle, "EOS_Sessions_UpdateSession");
		if (intPtr555 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_UpdateSession");
		}
		EOS_Sessions_UpdateSession = (EOS_Sessions_UpdateSessionDelegate)Marshal.GetDelegateForFunctionPointer(intPtr555, typeof(EOS_Sessions_UpdateSessionDelegate));
		IntPtr intPtr556 = getFunctionPointer(libraryHandle, "EOS_Sessions_UpdateSessionModification");
		if (intPtr556 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Sessions_UpdateSessionModification");
		}
		EOS_Sessions_UpdateSessionModification = (EOS_Sessions_UpdateSessionModificationDelegate)Marshal.GetDelegateForFunctionPointer(intPtr556, typeof(EOS_Sessions_UpdateSessionModificationDelegate));
		IntPtr intPtr557 = getFunctionPointer(libraryHandle, "EOS_Shutdown");
		if (intPtr557 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Shutdown");
		}
		EOS_Shutdown = (EOS_ShutdownDelegate)Marshal.GetDelegateForFunctionPointer(intPtr557, typeof(EOS_ShutdownDelegate));
		IntPtr intPtr558 = getFunctionPointer(libraryHandle, "EOS_Stats_CopyStatByIndex");
		if (intPtr558 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Stats_CopyStatByIndex");
		}
		EOS_Stats_CopyStatByIndex = (EOS_Stats_CopyStatByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr558, typeof(EOS_Stats_CopyStatByIndexDelegate));
		IntPtr intPtr559 = getFunctionPointer(libraryHandle, "EOS_Stats_CopyStatByName");
		if (intPtr559 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Stats_CopyStatByName");
		}
		EOS_Stats_CopyStatByName = (EOS_Stats_CopyStatByNameDelegate)Marshal.GetDelegateForFunctionPointer(intPtr559, typeof(EOS_Stats_CopyStatByNameDelegate));
		IntPtr intPtr560 = getFunctionPointer(libraryHandle, "EOS_Stats_GetStatsCount");
		if (intPtr560 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Stats_GetStatsCount");
		}
		EOS_Stats_GetStatsCount = (EOS_Stats_GetStatsCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr560, typeof(EOS_Stats_GetStatsCountDelegate));
		IntPtr intPtr561 = getFunctionPointer(libraryHandle, "EOS_Stats_IngestStat");
		if (intPtr561 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Stats_IngestStat");
		}
		EOS_Stats_IngestStat = (EOS_Stats_IngestStatDelegate)Marshal.GetDelegateForFunctionPointer(intPtr561, typeof(EOS_Stats_IngestStatDelegate));
		IntPtr intPtr562 = getFunctionPointer(libraryHandle, "EOS_Stats_QueryStats");
		if (intPtr562 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Stats_QueryStats");
		}
		EOS_Stats_QueryStats = (EOS_Stats_QueryStatsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr562, typeof(EOS_Stats_QueryStatsDelegate));
		IntPtr intPtr563 = getFunctionPointer(libraryHandle, "EOS_Stats_Stat_Release");
		if (intPtr563 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_Stats_Stat_Release");
		}
		EOS_Stats_Stat_Release = (EOS_Stats_Stat_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr563, typeof(EOS_Stats_Stat_ReleaseDelegate));
		IntPtr intPtr564 = getFunctionPointer(libraryHandle, "EOS_TitleStorageFileTransferRequest_CancelRequest");
		if (intPtr564 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorageFileTransferRequest_CancelRequest");
		}
		EOS_TitleStorageFileTransferRequest_CancelRequest = (EOS_TitleStorageFileTransferRequest_CancelRequestDelegate)Marshal.GetDelegateForFunctionPointer(intPtr564, typeof(EOS_TitleStorageFileTransferRequest_CancelRequestDelegate));
		IntPtr intPtr565 = getFunctionPointer(libraryHandle, "EOS_TitleStorageFileTransferRequest_GetFileRequestState");
		if (intPtr565 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorageFileTransferRequest_GetFileRequestState");
		}
		EOS_TitleStorageFileTransferRequest_GetFileRequestState = (EOS_TitleStorageFileTransferRequest_GetFileRequestStateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr565, typeof(EOS_TitleStorageFileTransferRequest_GetFileRequestStateDelegate));
		IntPtr intPtr566 = getFunctionPointer(libraryHandle, "EOS_TitleStorageFileTransferRequest_GetFilename");
		if (intPtr566 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorageFileTransferRequest_GetFilename");
		}
		EOS_TitleStorageFileTransferRequest_GetFilename = (EOS_TitleStorageFileTransferRequest_GetFilenameDelegate)Marshal.GetDelegateForFunctionPointer(intPtr566, typeof(EOS_TitleStorageFileTransferRequest_GetFilenameDelegate));
		IntPtr intPtr567 = getFunctionPointer(libraryHandle, "EOS_TitleStorageFileTransferRequest_Release");
		if (intPtr567 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorageFileTransferRequest_Release");
		}
		EOS_TitleStorageFileTransferRequest_Release = (EOS_TitleStorageFileTransferRequest_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr567, typeof(EOS_TitleStorageFileTransferRequest_ReleaseDelegate));
		IntPtr intPtr568 = getFunctionPointer(libraryHandle, "EOS_TitleStorage_CopyFileMetadataAtIndex");
		if (intPtr568 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorage_CopyFileMetadataAtIndex");
		}
		EOS_TitleStorage_CopyFileMetadataAtIndex = (EOS_TitleStorage_CopyFileMetadataAtIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr568, typeof(EOS_TitleStorage_CopyFileMetadataAtIndexDelegate));
		IntPtr intPtr569 = getFunctionPointer(libraryHandle, "EOS_TitleStorage_CopyFileMetadataByFilename");
		if (intPtr569 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorage_CopyFileMetadataByFilename");
		}
		EOS_TitleStorage_CopyFileMetadataByFilename = (EOS_TitleStorage_CopyFileMetadataByFilenameDelegate)Marshal.GetDelegateForFunctionPointer(intPtr569, typeof(EOS_TitleStorage_CopyFileMetadataByFilenameDelegate));
		IntPtr intPtr570 = getFunctionPointer(libraryHandle, "EOS_TitleStorage_DeleteCache");
		if (intPtr570 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorage_DeleteCache");
		}
		EOS_TitleStorage_DeleteCache = (EOS_TitleStorage_DeleteCacheDelegate)Marshal.GetDelegateForFunctionPointer(intPtr570, typeof(EOS_TitleStorage_DeleteCacheDelegate));
		IntPtr intPtr571 = getFunctionPointer(libraryHandle, "EOS_TitleStorage_FileMetadata_Release");
		if (intPtr571 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorage_FileMetadata_Release");
		}
		EOS_TitleStorage_FileMetadata_Release = (EOS_TitleStorage_FileMetadata_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr571, typeof(EOS_TitleStorage_FileMetadata_ReleaseDelegate));
		IntPtr intPtr572 = getFunctionPointer(libraryHandle, "EOS_TitleStorage_GetFileMetadataCount");
		if (intPtr572 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorage_GetFileMetadataCount");
		}
		EOS_TitleStorage_GetFileMetadataCount = (EOS_TitleStorage_GetFileMetadataCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr572, typeof(EOS_TitleStorage_GetFileMetadataCountDelegate));
		IntPtr intPtr573 = getFunctionPointer(libraryHandle, "EOS_TitleStorage_QueryFile");
		if (intPtr573 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorage_QueryFile");
		}
		EOS_TitleStorage_QueryFile = (EOS_TitleStorage_QueryFileDelegate)Marshal.GetDelegateForFunctionPointer(intPtr573, typeof(EOS_TitleStorage_QueryFileDelegate));
		IntPtr intPtr574 = getFunctionPointer(libraryHandle, "EOS_TitleStorage_QueryFileList");
		if (intPtr574 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorage_QueryFileList");
		}
		EOS_TitleStorage_QueryFileList = (EOS_TitleStorage_QueryFileListDelegate)Marshal.GetDelegateForFunctionPointer(intPtr574, typeof(EOS_TitleStorage_QueryFileListDelegate));
		IntPtr intPtr575 = getFunctionPointer(libraryHandle, "EOS_TitleStorage_ReadFile");
		if (intPtr575 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_TitleStorage_ReadFile");
		}
		EOS_TitleStorage_ReadFile = (EOS_TitleStorage_ReadFileDelegate)Marshal.GetDelegateForFunctionPointer(intPtr575, typeof(EOS_TitleStorage_ReadFileDelegate));
		IntPtr intPtr576 = getFunctionPointer(libraryHandle, "EOS_UI_AcknowledgeEventId");
		if (intPtr576 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_AcknowledgeEventId");
		}
		EOS_UI_AcknowledgeEventId = (EOS_UI_AcknowledgeEventIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr576, typeof(EOS_UI_AcknowledgeEventIdDelegate));
		IntPtr intPtr577 = getFunctionPointer(libraryHandle, "EOS_UI_AddNotifyDisplaySettingsUpdated");
		if (intPtr577 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_AddNotifyDisplaySettingsUpdated");
		}
		EOS_UI_AddNotifyDisplaySettingsUpdated = (EOS_UI_AddNotifyDisplaySettingsUpdatedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr577, typeof(EOS_UI_AddNotifyDisplaySettingsUpdatedDelegate));
		IntPtr intPtr578 = getFunctionPointer(libraryHandle, "EOS_UI_AddNotifyMemoryMonitor");
		if (intPtr578 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_AddNotifyMemoryMonitor");
		}
		EOS_UI_AddNotifyMemoryMonitor = (EOS_UI_AddNotifyMemoryMonitorDelegate)Marshal.GetDelegateForFunctionPointer(intPtr578, typeof(EOS_UI_AddNotifyMemoryMonitorDelegate));
		IntPtr intPtr579 = getFunctionPointer(libraryHandle, "EOS_UI_GetFriendsExclusiveInput");
		if (intPtr579 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_GetFriendsExclusiveInput");
		}
		EOS_UI_GetFriendsExclusiveInput = (EOS_UI_GetFriendsExclusiveInputDelegate)Marshal.GetDelegateForFunctionPointer(intPtr579, typeof(EOS_UI_GetFriendsExclusiveInputDelegate));
		IntPtr intPtr580 = getFunctionPointer(libraryHandle, "EOS_UI_GetFriendsVisible");
		if (intPtr580 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_GetFriendsVisible");
		}
		EOS_UI_GetFriendsVisible = (EOS_UI_GetFriendsVisibleDelegate)Marshal.GetDelegateForFunctionPointer(intPtr580, typeof(EOS_UI_GetFriendsVisibleDelegate));
		IntPtr intPtr581 = getFunctionPointer(libraryHandle, "EOS_UI_GetNotificationLocationPreference");
		if (intPtr581 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_GetNotificationLocationPreference");
		}
		EOS_UI_GetNotificationLocationPreference = (EOS_UI_GetNotificationLocationPreferenceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr581, typeof(EOS_UI_GetNotificationLocationPreferenceDelegate));
		IntPtr intPtr582 = getFunctionPointer(libraryHandle, "EOS_UI_GetToggleFriendsButton");
		if (intPtr582 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_GetToggleFriendsButton");
		}
		EOS_UI_GetToggleFriendsButton = (EOS_UI_GetToggleFriendsButtonDelegate)Marshal.GetDelegateForFunctionPointer(intPtr582, typeof(EOS_UI_GetToggleFriendsButtonDelegate));
		IntPtr intPtr583 = getFunctionPointer(libraryHandle, "EOS_UI_GetToggleFriendsKey");
		if (intPtr583 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_GetToggleFriendsKey");
		}
		EOS_UI_GetToggleFriendsKey = (EOS_UI_GetToggleFriendsKeyDelegate)Marshal.GetDelegateForFunctionPointer(intPtr583, typeof(EOS_UI_GetToggleFriendsKeyDelegate));
		IntPtr intPtr584 = getFunctionPointer(libraryHandle, "EOS_UI_HideFriends");
		if (intPtr584 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_HideFriends");
		}
		EOS_UI_HideFriends = (EOS_UI_HideFriendsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr584, typeof(EOS_UI_HideFriendsDelegate));
		IntPtr intPtr585 = getFunctionPointer(libraryHandle, "EOS_UI_IsSocialOverlayPaused");
		if (intPtr585 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_IsSocialOverlayPaused");
		}
		EOS_UI_IsSocialOverlayPaused = (EOS_UI_IsSocialOverlayPausedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr585, typeof(EOS_UI_IsSocialOverlayPausedDelegate));
		IntPtr intPtr586 = getFunctionPointer(libraryHandle, "EOS_UI_IsValidButtonCombination");
		if (intPtr586 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_IsValidButtonCombination");
		}
		EOS_UI_IsValidButtonCombination = (EOS_UI_IsValidButtonCombinationDelegate)Marshal.GetDelegateForFunctionPointer(intPtr586, typeof(EOS_UI_IsValidButtonCombinationDelegate));
		IntPtr intPtr587 = getFunctionPointer(libraryHandle, "EOS_UI_IsValidKeyCombination");
		if (intPtr587 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_IsValidKeyCombination");
		}
		EOS_UI_IsValidKeyCombination = (EOS_UI_IsValidKeyCombinationDelegate)Marshal.GetDelegateForFunctionPointer(intPtr587, typeof(EOS_UI_IsValidKeyCombinationDelegate));
		IntPtr intPtr588 = getFunctionPointer(libraryHandle, "EOS_UI_PauseSocialOverlay");
		if (intPtr588 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_PauseSocialOverlay");
		}
		EOS_UI_PauseSocialOverlay = (EOS_UI_PauseSocialOverlayDelegate)Marshal.GetDelegateForFunctionPointer(intPtr588, typeof(EOS_UI_PauseSocialOverlayDelegate));
		IntPtr intPtr589 = getFunctionPointer(libraryHandle, "EOS_UI_PrePresent");
		if (intPtr589 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_PrePresent");
		}
		EOS_UI_PrePresent = (EOS_UI_PrePresentDelegate)Marshal.GetDelegateForFunctionPointer(intPtr589, typeof(EOS_UI_PrePresentDelegate));
		IntPtr intPtr590 = getFunctionPointer(libraryHandle, "EOS_UI_RemoveNotifyDisplaySettingsUpdated");
		if (intPtr590 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_RemoveNotifyDisplaySettingsUpdated");
		}
		EOS_UI_RemoveNotifyDisplaySettingsUpdated = (EOS_UI_RemoveNotifyDisplaySettingsUpdatedDelegate)Marshal.GetDelegateForFunctionPointer(intPtr590, typeof(EOS_UI_RemoveNotifyDisplaySettingsUpdatedDelegate));
		IntPtr intPtr591 = getFunctionPointer(libraryHandle, "EOS_UI_RemoveNotifyMemoryMonitor");
		if (intPtr591 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_RemoveNotifyMemoryMonitor");
		}
		EOS_UI_RemoveNotifyMemoryMonitor = (EOS_UI_RemoveNotifyMemoryMonitorDelegate)Marshal.GetDelegateForFunctionPointer(intPtr591, typeof(EOS_UI_RemoveNotifyMemoryMonitorDelegate));
		IntPtr intPtr592 = getFunctionPointer(libraryHandle, "EOS_UI_ReportInputState");
		if (intPtr592 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_ReportInputState");
		}
		EOS_UI_ReportInputState = (EOS_UI_ReportInputStateDelegate)Marshal.GetDelegateForFunctionPointer(intPtr592, typeof(EOS_UI_ReportInputStateDelegate));
		IntPtr intPtr593 = getFunctionPointer(libraryHandle, "EOS_UI_SetDisplayPreference");
		if (intPtr593 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_SetDisplayPreference");
		}
		EOS_UI_SetDisplayPreference = (EOS_UI_SetDisplayPreferenceDelegate)Marshal.GetDelegateForFunctionPointer(intPtr593, typeof(EOS_UI_SetDisplayPreferenceDelegate));
		IntPtr intPtr594 = getFunctionPointer(libraryHandle, "EOS_UI_SetToggleFriendsButton");
		if (intPtr594 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_SetToggleFriendsButton");
		}
		EOS_UI_SetToggleFriendsButton = (EOS_UI_SetToggleFriendsButtonDelegate)Marshal.GetDelegateForFunctionPointer(intPtr594, typeof(EOS_UI_SetToggleFriendsButtonDelegate));
		IntPtr intPtr595 = getFunctionPointer(libraryHandle, "EOS_UI_SetToggleFriendsKey");
		if (intPtr595 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_SetToggleFriendsKey");
		}
		EOS_UI_SetToggleFriendsKey = (EOS_UI_SetToggleFriendsKeyDelegate)Marshal.GetDelegateForFunctionPointer(intPtr595, typeof(EOS_UI_SetToggleFriendsKeyDelegate));
		IntPtr intPtr596 = getFunctionPointer(libraryHandle, "EOS_UI_ShowBlockPlayer");
		if (intPtr596 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_ShowBlockPlayer");
		}
		EOS_UI_ShowBlockPlayer = (EOS_UI_ShowBlockPlayerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr596, typeof(EOS_UI_ShowBlockPlayerDelegate));
		IntPtr intPtr597 = getFunctionPointer(libraryHandle, "EOS_UI_ShowFriends");
		if (intPtr597 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_ShowFriends");
		}
		EOS_UI_ShowFriends = (EOS_UI_ShowFriendsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr597, typeof(EOS_UI_ShowFriendsDelegate));
		IntPtr intPtr598 = getFunctionPointer(libraryHandle, "EOS_UI_ShowNativeProfile");
		if (intPtr598 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_ShowNativeProfile");
		}
		EOS_UI_ShowNativeProfile = (EOS_UI_ShowNativeProfileDelegate)Marshal.GetDelegateForFunctionPointer(intPtr598, typeof(EOS_UI_ShowNativeProfileDelegate));
		IntPtr intPtr599 = getFunctionPointer(libraryHandle, "EOS_UI_ShowReportPlayer");
		if (intPtr599 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UI_ShowReportPlayer");
		}
		EOS_UI_ShowReportPlayer = (EOS_UI_ShowReportPlayerDelegate)Marshal.GetDelegateForFunctionPointer(intPtr599, typeof(EOS_UI_ShowReportPlayerDelegate));
		IntPtr intPtr600 = getFunctionPointer(libraryHandle, "EOS_UserInfo_BestDisplayName_Release");
		if (intPtr600 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_BestDisplayName_Release");
		}
		EOS_UserInfo_BestDisplayName_Release = (EOS_UserInfo_BestDisplayName_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr600, typeof(EOS_UserInfo_BestDisplayName_ReleaseDelegate));
		IntPtr intPtr601 = getFunctionPointer(libraryHandle, "EOS_UserInfo_CopyBestDisplayName");
		if (intPtr601 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_CopyBestDisplayName");
		}
		EOS_UserInfo_CopyBestDisplayName = (EOS_UserInfo_CopyBestDisplayNameDelegate)Marshal.GetDelegateForFunctionPointer(intPtr601, typeof(EOS_UserInfo_CopyBestDisplayNameDelegate));
		IntPtr intPtr602 = getFunctionPointer(libraryHandle, "EOS_UserInfo_CopyBestDisplayNameWithPlatform");
		if (intPtr602 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_CopyBestDisplayNameWithPlatform");
		}
		EOS_UserInfo_CopyBestDisplayNameWithPlatform = (EOS_UserInfo_CopyBestDisplayNameWithPlatformDelegate)Marshal.GetDelegateForFunctionPointer(intPtr602, typeof(EOS_UserInfo_CopyBestDisplayNameWithPlatformDelegate));
		IntPtr intPtr603 = getFunctionPointer(libraryHandle, "EOS_UserInfo_CopyExternalUserInfoByAccountId");
		if (intPtr603 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_CopyExternalUserInfoByAccountId");
		}
		EOS_UserInfo_CopyExternalUserInfoByAccountId = (EOS_UserInfo_CopyExternalUserInfoByAccountIdDelegate)Marshal.GetDelegateForFunctionPointer(intPtr603, typeof(EOS_UserInfo_CopyExternalUserInfoByAccountIdDelegate));
		IntPtr intPtr604 = getFunctionPointer(libraryHandle, "EOS_UserInfo_CopyExternalUserInfoByAccountType");
		if (intPtr604 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_CopyExternalUserInfoByAccountType");
		}
		EOS_UserInfo_CopyExternalUserInfoByAccountType = (EOS_UserInfo_CopyExternalUserInfoByAccountTypeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr604, typeof(EOS_UserInfo_CopyExternalUserInfoByAccountTypeDelegate));
		IntPtr intPtr605 = getFunctionPointer(libraryHandle, "EOS_UserInfo_CopyExternalUserInfoByIndex");
		if (intPtr605 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_CopyExternalUserInfoByIndex");
		}
		EOS_UserInfo_CopyExternalUserInfoByIndex = (EOS_UserInfo_CopyExternalUserInfoByIndexDelegate)Marshal.GetDelegateForFunctionPointer(intPtr605, typeof(EOS_UserInfo_CopyExternalUserInfoByIndexDelegate));
		IntPtr intPtr606 = getFunctionPointer(libraryHandle, "EOS_UserInfo_CopyUserInfo");
		if (intPtr606 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_CopyUserInfo");
		}
		EOS_UserInfo_CopyUserInfo = (EOS_UserInfo_CopyUserInfoDelegate)Marshal.GetDelegateForFunctionPointer(intPtr606, typeof(EOS_UserInfo_CopyUserInfoDelegate));
		IntPtr intPtr607 = getFunctionPointer(libraryHandle, "EOS_UserInfo_ExternalUserInfo_Release");
		if (intPtr607 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_ExternalUserInfo_Release");
		}
		EOS_UserInfo_ExternalUserInfo_Release = (EOS_UserInfo_ExternalUserInfo_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr607, typeof(EOS_UserInfo_ExternalUserInfo_ReleaseDelegate));
		IntPtr intPtr608 = getFunctionPointer(libraryHandle, "EOS_UserInfo_GetExternalUserInfoCount");
		if (intPtr608 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_GetExternalUserInfoCount");
		}
		EOS_UserInfo_GetExternalUserInfoCount = (EOS_UserInfo_GetExternalUserInfoCountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr608, typeof(EOS_UserInfo_GetExternalUserInfoCountDelegate));
		IntPtr intPtr609 = getFunctionPointer(libraryHandle, "EOS_UserInfo_GetLocalPlatformType");
		if (intPtr609 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_GetLocalPlatformType");
		}
		EOS_UserInfo_GetLocalPlatformType = (EOS_UserInfo_GetLocalPlatformTypeDelegate)Marshal.GetDelegateForFunctionPointer(intPtr609, typeof(EOS_UserInfo_GetLocalPlatformTypeDelegate));
		IntPtr intPtr610 = getFunctionPointer(libraryHandle, "EOS_UserInfo_QueryUserInfo");
		if (intPtr610 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_QueryUserInfo");
		}
		EOS_UserInfo_QueryUserInfo = (EOS_UserInfo_QueryUserInfoDelegate)Marshal.GetDelegateForFunctionPointer(intPtr610, typeof(EOS_UserInfo_QueryUserInfoDelegate));
		IntPtr intPtr611 = getFunctionPointer(libraryHandle, "EOS_UserInfo_QueryUserInfoByDisplayName");
		if (intPtr611 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_QueryUserInfoByDisplayName");
		}
		EOS_UserInfo_QueryUserInfoByDisplayName = (EOS_UserInfo_QueryUserInfoByDisplayNameDelegate)Marshal.GetDelegateForFunctionPointer(intPtr611, typeof(EOS_UserInfo_QueryUserInfoByDisplayNameDelegate));
		IntPtr intPtr612 = getFunctionPointer(libraryHandle, "EOS_UserInfo_QueryUserInfoByExternalAccount");
		if (intPtr612 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_QueryUserInfoByExternalAccount");
		}
		EOS_UserInfo_QueryUserInfoByExternalAccount = (EOS_UserInfo_QueryUserInfoByExternalAccountDelegate)Marshal.GetDelegateForFunctionPointer(intPtr612, typeof(EOS_UserInfo_QueryUserInfoByExternalAccountDelegate));
		IntPtr intPtr613 = getFunctionPointer(libraryHandle, "EOS_UserInfo_Release");
		if (intPtr613 == IntPtr.Zero)
		{
			throw new DynamicBindingException("EOS_UserInfo_Release");
		}
		EOS_UserInfo_Release = (EOS_UserInfo_ReleaseDelegate)Marshal.GetDelegateForFunctionPointer(intPtr613, typeof(EOS_UserInfo_ReleaseDelegate));
	}

	public static void Unhook()
	{
		EOS_Achievements_AddNotifyAchievementsUnlocked = null;
		EOS_Achievements_AddNotifyAchievementsUnlockedV2 = null;
		EOS_Achievements_CopyAchievementDefinitionByAchievementId = null;
		EOS_Achievements_CopyAchievementDefinitionByIndex = null;
		EOS_Achievements_CopyAchievementDefinitionV2ByAchievementId = null;
		EOS_Achievements_CopyAchievementDefinitionV2ByIndex = null;
		EOS_Achievements_CopyPlayerAchievementByAchievementId = null;
		EOS_Achievements_CopyPlayerAchievementByIndex = null;
		EOS_Achievements_CopyUnlockedAchievementByAchievementId = null;
		EOS_Achievements_CopyUnlockedAchievementByIndex = null;
		EOS_Achievements_DefinitionV2_Release = null;
		EOS_Achievements_Definition_Release = null;
		EOS_Achievements_GetAchievementDefinitionCount = null;
		EOS_Achievements_GetPlayerAchievementCount = null;
		EOS_Achievements_GetUnlockedAchievementCount = null;
		EOS_Achievements_PlayerAchievement_Release = null;
		EOS_Achievements_QueryDefinitions = null;
		EOS_Achievements_QueryPlayerAchievements = null;
		EOS_Achievements_RemoveNotifyAchievementsUnlocked = null;
		EOS_Achievements_UnlockAchievements = null;
		EOS_Achievements_UnlockedAchievement_Release = null;
		EOS_ActiveSession_CopyInfo = null;
		EOS_ActiveSession_GetRegisteredPlayerByIndex = null;
		EOS_ActiveSession_GetRegisteredPlayerCount = null;
		EOS_ActiveSession_Info_Release = null;
		EOS_ActiveSession_Release = null;
		EOS_AntiCheatClient_AddExternalIntegrityCatalog = null;
		EOS_AntiCheatClient_AddNotifyClientIntegrityViolated = null;
		EOS_AntiCheatClient_AddNotifyMessageToPeer = null;
		EOS_AntiCheatClient_AddNotifyMessageToServer = null;
		EOS_AntiCheatClient_AddNotifyPeerActionRequired = null;
		EOS_AntiCheatClient_AddNotifyPeerAuthStatusChanged = null;
		EOS_AntiCheatClient_BeginSession = null;
		EOS_AntiCheatClient_EndSession = null;
		EOS_AntiCheatClient_GetProtectMessageOutputLength = null;
		EOS_AntiCheatClient_PollStatus = null;
		EOS_AntiCheatClient_ProtectMessage = null;
		EOS_AntiCheatClient_ReceiveMessageFromPeer = null;
		EOS_AntiCheatClient_ReceiveMessageFromServer = null;
		EOS_AntiCheatClient_RegisterPeer = null;
		EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolated = null;
		EOS_AntiCheatClient_RemoveNotifyMessageToPeer = null;
		EOS_AntiCheatClient_RemoveNotifyMessageToServer = null;
		EOS_AntiCheatClient_RemoveNotifyPeerActionRequired = null;
		EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChanged = null;
		EOS_AntiCheatClient_UnprotectMessage = null;
		EOS_AntiCheatClient_UnregisterPeer = null;
		EOS_AntiCheatServer_AddNotifyClientActionRequired = null;
		EOS_AntiCheatServer_AddNotifyClientAuthStatusChanged = null;
		EOS_AntiCheatServer_AddNotifyMessageToClient = null;
		EOS_AntiCheatServer_BeginSession = null;
		EOS_AntiCheatServer_EndSession = null;
		EOS_AntiCheatServer_GetProtectMessageOutputLength = null;
		EOS_AntiCheatServer_LogEvent = null;
		EOS_AntiCheatServer_LogGameRoundEnd = null;
		EOS_AntiCheatServer_LogGameRoundStart = null;
		EOS_AntiCheatServer_LogPlayerDespawn = null;
		EOS_AntiCheatServer_LogPlayerRevive = null;
		EOS_AntiCheatServer_LogPlayerSpawn = null;
		EOS_AntiCheatServer_LogPlayerTakeDamage = null;
		EOS_AntiCheatServer_LogPlayerTick = null;
		EOS_AntiCheatServer_LogPlayerUseAbility = null;
		EOS_AntiCheatServer_LogPlayerUseWeapon = null;
		EOS_AntiCheatServer_ProtectMessage = null;
		EOS_AntiCheatServer_ReceiveMessageFromClient = null;
		EOS_AntiCheatServer_RegisterClient = null;
		EOS_AntiCheatServer_RegisterEvent = null;
		EOS_AntiCheatServer_RemoveNotifyClientActionRequired = null;
		EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChanged = null;
		EOS_AntiCheatServer_RemoveNotifyMessageToClient = null;
		EOS_AntiCheatServer_SetClientDetails = null;
		EOS_AntiCheatServer_SetClientNetworkState = null;
		EOS_AntiCheatServer_SetGameSessionId = null;
		EOS_AntiCheatServer_UnprotectMessage = null;
		EOS_AntiCheatServer_UnregisterClient = null;
		EOS_Auth_AddNotifyLoginStatusChanged = null;
		EOS_Auth_CopyIdToken = null;
		EOS_Auth_CopyUserAuthToken = null;
		EOS_Auth_DeletePersistentAuth = null;
		EOS_Auth_GetLoggedInAccountByIndex = null;
		EOS_Auth_GetLoggedInAccountsCount = null;
		EOS_Auth_GetLoginStatus = null;
		EOS_Auth_GetMergedAccountByIndex = null;
		EOS_Auth_GetMergedAccountsCount = null;
		EOS_Auth_GetSelectedAccountId = null;
		EOS_Auth_IdToken_Release = null;
		EOS_Auth_LinkAccount = null;
		EOS_Auth_Login = null;
		EOS_Auth_Logout = null;
		EOS_Auth_QueryIdToken = null;
		EOS_Auth_RemoveNotifyLoginStatusChanged = null;
		EOS_Auth_Token_Release = null;
		EOS_Auth_VerifyIdToken = null;
		EOS_Auth_VerifyUserAuth = null;
		EOS_ByteArray_ToString = null;
		EOS_Connect_AddNotifyAuthExpiration = null;
		EOS_Connect_AddNotifyLoginStatusChanged = null;
		EOS_Connect_CopyIdToken = null;
		EOS_Connect_CopyProductUserExternalAccountByAccountId = null;
		EOS_Connect_CopyProductUserExternalAccountByAccountType = null;
		EOS_Connect_CopyProductUserExternalAccountByIndex = null;
		EOS_Connect_CopyProductUserInfo = null;
		EOS_Connect_CreateDeviceId = null;
		EOS_Connect_CreateUser = null;
		EOS_Connect_DeleteDeviceId = null;
		EOS_Connect_ExternalAccountInfo_Release = null;
		EOS_Connect_GetExternalAccountMapping = null;
		EOS_Connect_GetLoggedInUserByIndex = null;
		EOS_Connect_GetLoggedInUsersCount = null;
		EOS_Connect_GetLoginStatus = null;
		EOS_Connect_GetProductUserExternalAccountCount = null;
		EOS_Connect_GetProductUserIdMapping = null;
		EOS_Connect_IdToken_Release = null;
		EOS_Connect_LinkAccount = null;
		EOS_Connect_Login = null;
		EOS_Connect_QueryExternalAccountMappings = null;
		EOS_Connect_QueryProductUserIdMappings = null;
		EOS_Connect_RemoveNotifyAuthExpiration = null;
		EOS_Connect_RemoveNotifyLoginStatusChanged = null;
		EOS_Connect_TransferDeviceIdAccount = null;
		EOS_Connect_UnlinkAccount = null;
		EOS_Connect_VerifyIdToken = null;
		EOS_ContinuanceToken_ToString = null;
		EOS_CustomInvites_AcceptRequestToJoin = null;
		EOS_CustomInvites_AddNotifyCustomInviteAccepted = null;
		EOS_CustomInvites_AddNotifyCustomInviteReceived = null;
		EOS_CustomInvites_AddNotifyCustomInviteRejected = null;
		EOS_CustomInvites_AddNotifyRequestToJoinAccepted = null;
		EOS_CustomInvites_AddNotifyRequestToJoinReceived = null;
		EOS_CustomInvites_AddNotifyRequestToJoinRejected = null;
		EOS_CustomInvites_AddNotifyRequestToJoinResponseReceived = null;
		EOS_CustomInvites_AddNotifySendCustomNativeInviteRequested = null;
		EOS_CustomInvites_FinalizeInvite = null;
		EOS_CustomInvites_RejectRequestToJoin = null;
		EOS_CustomInvites_RemoveNotifyCustomInviteAccepted = null;
		EOS_CustomInvites_RemoveNotifyCustomInviteReceived = null;
		EOS_CustomInvites_RemoveNotifyCustomInviteRejected = null;
		EOS_CustomInvites_RemoveNotifyRequestToJoinAccepted = null;
		EOS_CustomInvites_RemoveNotifyRequestToJoinReceived = null;
		EOS_CustomInvites_RemoveNotifyRequestToJoinRejected = null;
		EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceived = null;
		EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequested = null;
		EOS_CustomInvites_SendCustomInvite = null;
		EOS_CustomInvites_SendRequestToJoin = null;
		EOS_CustomInvites_SetCustomInvite = null;
		EOS_EApplicationStatus_ToString = null;
		EOS_ENetworkStatus_ToString = null;
		EOS_EResult_IsOperationComplete = null;
		EOS_EResult_ToString = null;
		EOS_Ecom_CatalogItem_Release = null;
		EOS_Ecom_CatalogOffer_Release = null;
		EOS_Ecom_CatalogRelease_Release = null;
		EOS_Ecom_Checkout = null;
		EOS_Ecom_CopyEntitlementById = null;
		EOS_Ecom_CopyEntitlementByIndex = null;
		EOS_Ecom_CopyEntitlementByNameAndIndex = null;
		EOS_Ecom_CopyItemById = null;
		EOS_Ecom_CopyItemImageInfoByIndex = null;
		EOS_Ecom_CopyItemReleaseByIndex = null;
		EOS_Ecom_CopyLastRedeemedEntitlementByIndex = null;
		EOS_Ecom_CopyOfferById = null;
		EOS_Ecom_CopyOfferByIndex = null;
		EOS_Ecom_CopyOfferImageInfoByIndex = null;
		EOS_Ecom_CopyOfferItemByIndex = null;
		EOS_Ecom_CopyTransactionById = null;
		EOS_Ecom_CopyTransactionByIndex = null;
		EOS_Ecom_Entitlement_Release = null;
		EOS_Ecom_GetEntitlementsByNameCount = null;
		EOS_Ecom_GetEntitlementsCount = null;
		EOS_Ecom_GetItemImageInfoCount = null;
		EOS_Ecom_GetItemReleaseCount = null;
		EOS_Ecom_GetLastRedeemedEntitlementsCount = null;
		EOS_Ecom_GetOfferCount = null;
		EOS_Ecom_GetOfferImageInfoCount = null;
		EOS_Ecom_GetOfferItemCount = null;
		EOS_Ecom_GetTransactionCount = null;
		EOS_Ecom_KeyImageInfo_Release = null;
		EOS_Ecom_QueryEntitlementToken = null;
		EOS_Ecom_QueryEntitlements = null;
		EOS_Ecom_QueryOffers = null;
		EOS_Ecom_QueryOwnership = null;
		EOS_Ecom_QueryOwnershipBySandboxIds = null;
		EOS_Ecom_QueryOwnershipToken = null;
		EOS_Ecom_RedeemEntitlements = null;
		EOS_Ecom_Transaction_CopyEntitlementByIndex = null;
		EOS_Ecom_Transaction_GetEntitlementsCount = null;
		EOS_Ecom_Transaction_GetTransactionId = null;
		EOS_Ecom_Transaction_Release = null;
		EOS_EpicAccountId_FromString = null;
		EOS_EpicAccountId_IsValid = null;
		EOS_EpicAccountId_ToString = null;
		EOS_Friends_AcceptInvite = null;
		EOS_Friends_AddNotifyBlockedUsersUpdate = null;
		EOS_Friends_AddNotifyFriendsUpdate = null;
		EOS_Friends_GetBlockedUserAtIndex = null;
		EOS_Friends_GetBlockedUsersCount = null;
		EOS_Friends_GetFriendAtIndex = null;
		EOS_Friends_GetFriendsCount = null;
		EOS_Friends_GetStatus = null;
		EOS_Friends_QueryFriends = null;
		EOS_Friends_RejectInvite = null;
		EOS_Friends_RemoveNotifyBlockedUsersUpdate = null;
		EOS_Friends_RemoveNotifyFriendsUpdate = null;
		EOS_Friends_SendInvite = null;
		EOS_GetVersion = null;
		EOS_Initialize = null;
		EOS_IntegratedPlatformOptionsContainer_Add = null;
		EOS_IntegratedPlatformOptionsContainer_Release = null;
		EOS_IntegratedPlatform_AddNotifyUserLoginStatusChanged = null;
		EOS_IntegratedPlatform_ClearUserPreLogoutCallback = null;
		EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainer = null;
		EOS_IntegratedPlatform_FinalizeDeferredUserLogout = null;
		EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChanged = null;
		EOS_IntegratedPlatform_SetUserLoginStatus = null;
		EOS_IntegratedPlatform_SetUserPreLogoutCallback = null;
		EOS_KWS_AddNotifyPermissionsUpdateReceived = null;
		EOS_KWS_CopyPermissionByIndex = null;
		EOS_KWS_CreateUser = null;
		EOS_KWS_GetPermissionByKey = null;
		EOS_KWS_GetPermissionsCount = null;
		EOS_KWS_PermissionStatus_Release = null;
		EOS_KWS_QueryAgeGate = null;
		EOS_KWS_QueryPermissions = null;
		EOS_KWS_RemoveNotifyPermissionsUpdateReceived = null;
		EOS_KWS_RequestPermissions = null;
		EOS_KWS_UpdateParentEmail = null;
		EOS_Leaderboards_CopyLeaderboardDefinitionByIndex = null;
		EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardId = null;
		EOS_Leaderboards_CopyLeaderboardRecordByIndex = null;
		EOS_Leaderboards_CopyLeaderboardRecordByUserId = null;
		EOS_Leaderboards_CopyLeaderboardUserScoreByIndex = null;
		EOS_Leaderboards_CopyLeaderboardUserScoreByUserId = null;
		EOS_Leaderboards_Definition_Release = null;
		EOS_Leaderboards_GetLeaderboardDefinitionCount = null;
		EOS_Leaderboards_GetLeaderboardRecordCount = null;
		EOS_Leaderboards_GetLeaderboardUserScoreCount = null;
		EOS_Leaderboards_LeaderboardDefinition_Release = null;
		EOS_Leaderboards_LeaderboardRecord_Release = null;
		EOS_Leaderboards_LeaderboardUserScore_Release = null;
		EOS_Leaderboards_QueryLeaderboardDefinitions = null;
		EOS_Leaderboards_QueryLeaderboardRanks = null;
		EOS_Leaderboards_QueryLeaderboardUserScores = null;
		EOS_LobbyDetails_CopyAttributeByIndex = null;
		EOS_LobbyDetails_CopyAttributeByKey = null;
		EOS_LobbyDetails_CopyInfo = null;
		EOS_LobbyDetails_CopyMemberAttributeByIndex = null;
		EOS_LobbyDetails_CopyMemberAttributeByKey = null;
		EOS_LobbyDetails_CopyMemberInfo = null;
		EOS_LobbyDetails_GetAttributeCount = null;
		EOS_LobbyDetails_GetLobbyOwner = null;
		EOS_LobbyDetails_GetMemberAttributeCount = null;
		EOS_LobbyDetails_GetMemberByIndex = null;
		EOS_LobbyDetails_GetMemberCount = null;
		EOS_LobbyDetails_Info_Release = null;
		EOS_LobbyDetails_MemberInfo_Release = null;
		EOS_LobbyDetails_Release = null;
		EOS_LobbyModification_AddAttribute = null;
		EOS_LobbyModification_AddMemberAttribute = null;
		EOS_LobbyModification_Release = null;
		EOS_LobbyModification_RemoveAttribute = null;
		EOS_LobbyModification_RemoveMemberAttribute = null;
		EOS_LobbyModification_SetAllowedPlatformIds = null;
		EOS_LobbyModification_SetBucketId = null;
		EOS_LobbyModification_SetInvitesAllowed = null;
		EOS_LobbyModification_SetMaxMembers = null;
		EOS_LobbyModification_SetPermissionLevel = null;
		EOS_LobbySearch_CopySearchResultByIndex = null;
		EOS_LobbySearch_Find = null;
		EOS_LobbySearch_GetSearchResultCount = null;
		EOS_LobbySearch_Release = null;
		EOS_LobbySearch_RemoveParameter = null;
		EOS_LobbySearch_SetLobbyId = null;
		EOS_LobbySearch_SetMaxResults = null;
		EOS_LobbySearch_SetParameter = null;
		EOS_LobbySearch_SetTargetUserId = null;
		EOS_Lobby_AddNotifyJoinLobbyAccepted = null;
		EOS_Lobby_AddNotifyLeaveLobbyRequested = null;
		EOS_Lobby_AddNotifyLobbyInviteAccepted = null;
		EOS_Lobby_AddNotifyLobbyInviteReceived = null;
		EOS_Lobby_AddNotifyLobbyInviteRejected = null;
		EOS_Lobby_AddNotifyLobbyMemberStatusReceived = null;
		EOS_Lobby_AddNotifyLobbyMemberUpdateReceived = null;
		EOS_Lobby_AddNotifyLobbyUpdateReceived = null;
		EOS_Lobby_AddNotifyRTCRoomConnectionChanged = null;
		EOS_Lobby_AddNotifySendLobbyNativeInviteRequested = null;
		EOS_Lobby_Attribute_Release = null;
		EOS_Lobby_CopyLobbyDetailsHandle = null;
		EOS_Lobby_CopyLobbyDetailsHandleByInviteId = null;
		EOS_Lobby_CopyLobbyDetailsHandleByUiEventId = null;
		EOS_Lobby_CreateLobby = null;
		EOS_Lobby_CreateLobbySearch = null;
		EOS_Lobby_DestroyLobby = null;
		EOS_Lobby_GetConnectString = null;
		EOS_Lobby_GetInviteCount = null;
		EOS_Lobby_GetInviteIdByIndex = null;
		EOS_Lobby_GetRTCRoomName = null;
		EOS_Lobby_HardMuteMember = null;
		EOS_Lobby_IsRTCRoomConnected = null;
		EOS_Lobby_JoinLobby = null;
		EOS_Lobby_JoinLobbyById = null;
		EOS_Lobby_KickMember = null;
		EOS_Lobby_LeaveLobby = null;
		EOS_Lobby_ParseConnectString = null;
		EOS_Lobby_PromoteMember = null;
		EOS_Lobby_QueryInvites = null;
		EOS_Lobby_RejectInvite = null;
		EOS_Lobby_RemoveNotifyJoinLobbyAccepted = null;
		EOS_Lobby_RemoveNotifyLeaveLobbyRequested = null;
		EOS_Lobby_RemoveNotifyLobbyInviteAccepted = null;
		EOS_Lobby_RemoveNotifyLobbyInviteReceived = null;
		EOS_Lobby_RemoveNotifyLobbyInviteRejected = null;
		EOS_Lobby_RemoveNotifyLobbyMemberStatusReceived = null;
		EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceived = null;
		EOS_Lobby_RemoveNotifyLobbyUpdateReceived = null;
		EOS_Lobby_RemoveNotifyRTCRoomConnectionChanged = null;
		EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequested = null;
		EOS_Lobby_SendInvite = null;
		EOS_Lobby_UpdateLobby = null;
		EOS_Lobby_UpdateLobbyModification = null;
		EOS_Logging_SetCallback = null;
		EOS_Logging_SetLogLevel = null;
		EOS_Metrics_BeginPlayerSession = null;
		EOS_Metrics_EndPlayerSession = null;
		EOS_Mods_CopyModInfo = null;
		EOS_Mods_EnumerateMods = null;
		EOS_Mods_InstallMod = null;
		EOS_Mods_ModInfo_Release = null;
		EOS_Mods_UninstallMod = null;
		EOS_Mods_UpdateMod = null;
		EOS_P2P_AcceptConnection = null;
		EOS_P2P_AddNotifyIncomingPacketQueueFull = null;
		EOS_P2P_AddNotifyPeerConnectionClosed = null;
		EOS_P2P_AddNotifyPeerConnectionEstablished = null;
		EOS_P2P_AddNotifyPeerConnectionInterrupted = null;
		EOS_P2P_AddNotifyPeerConnectionRequest = null;
		EOS_P2P_ClearPacketQueue = null;
		EOS_P2P_CloseConnection = null;
		EOS_P2P_CloseConnections = null;
		EOS_P2P_GetNATType = null;
		EOS_P2P_GetNextReceivedPacketSize = null;
		EOS_P2P_GetPacketQueueInfo = null;
		EOS_P2P_GetPortRange = null;
		EOS_P2P_GetRelayControl = null;
		EOS_P2P_QueryNATType = null;
		EOS_P2P_ReceivePacket = null;
		EOS_P2P_RemoveNotifyIncomingPacketQueueFull = null;
		EOS_P2P_RemoveNotifyPeerConnectionClosed = null;
		EOS_P2P_RemoveNotifyPeerConnectionEstablished = null;
		EOS_P2P_RemoveNotifyPeerConnectionInterrupted = null;
		EOS_P2P_RemoveNotifyPeerConnectionRequest = null;
		EOS_P2P_SendPacket = null;
		EOS_P2P_SetPacketQueueSize = null;
		EOS_P2P_SetPortRange = null;
		EOS_P2P_SetRelayControl = null;
		EOS_Platform_CheckForLauncherAndRestart = null;
		EOS_Platform_Create = null;
		EOS_Platform_GetAchievementsInterface = null;
		EOS_Platform_GetActiveCountryCode = null;
		EOS_Platform_GetActiveLocaleCode = null;
		EOS_Platform_GetAntiCheatClientInterface = null;
		EOS_Platform_GetAntiCheatServerInterface = null;
		EOS_Platform_GetApplicationStatus = null;
		EOS_Platform_GetAuthInterface = null;
		EOS_Platform_GetConnectInterface = null;
		EOS_Platform_GetCustomInvitesInterface = null;
		EOS_Platform_GetDesktopCrossplayStatus = null;
		EOS_Platform_GetEcomInterface = null;
		EOS_Platform_GetFriendsInterface = null;
		EOS_Platform_GetIntegratedPlatformInterface = null;
		EOS_Platform_GetKWSInterface = null;
		EOS_Platform_GetLeaderboardsInterface = null;
		EOS_Platform_GetLobbyInterface = null;
		EOS_Platform_GetMetricsInterface = null;
		EOS_Platform_GetModsInterface = null;
		EOS_Platform_GetNetworkStatus = null;
		EOS_Platform_GetOverrideCountryCode = null;
		EOS_Platform_GetOverrideLocaleCode = null;
		EOS_Platform_GetP2PInterface = null;
		EOS_Platform_GetPlayerDataStorageInterface = null;
		EOS_Platform_GetPresenceInterface = null;
		EOS_Platform_GetProgressionSnapshotInterface = null;
		EOS_Platform_GetRTCAdminInterface = null;
		EOS_Platform_GetRTCInterface = null;
		EOS_Platform_GetReportsInterface = null;
		EOS_Platform_GetSanctionsInterface = null;
		EOS_Platform_GetSessionsInterface = null;
		EOS_Platform_GetStatsInterface = null;
		EOS_Platform_GetTitleStorageInterface = null;
		EOS_Platform_GetUIInterface = null;
		EOS_Platform_GetUserInfoInterface = null;
		EOS_Platform_Release = null;
		EOS_Platform_SetApplicationStatus = null;
		EOS_Platform_SetNetworkStatus = null;
		EOS_Platform_SetOverrideCountryCode = null;
		EOS_Platform_SetOverrideLocaleCode = null;
		EOS_Platform_Tick = null;
		EOS_PlayerDataStorageFileTransferRequest_CancelRequest = null;
		EOS_PlayerDataStorageFileTransferRequest_GetFileRequestState = null;
		EOS_PlayerDataStorageFileTransferRequest_GetFilename = null;
		EOS_PlayerDataStorageFileTransferRequest_Release = null;
		EOS_PlayerDataStorage_CopyFileMetadataAtIndex = null;
		EOS_PlayerDataStorage_CopyFileMetadataByFilename = null;
		EOS_PlayerDataStorage_DeleteCache = null;
		EOS_PlayerDataStorage_DeleteFile = null;
		EOS_PlayerDataStorage_DuplicateFile = null;
		EOS_PlayerDataStorage_FileMetadata_Release = null;
		EOS_PlayerDataStorage_GetFileMetadataCount = null;
		EOS_PlayerDataStorage_QueryFile = null;
		EOS_PlayerDataStorage_QueryFileList = null;
		EOS_PlayerDataStorage_ReadFile = null;
		EOS_PlayerDataStorage_WriteFile = null;
		EOS_PresenceModification_DeleteData = null;
		EOS_PresenceModification_Release = null;
		EOS_PresenceModification_SetData = null;
		EOS_PresenceModification_SetJoinInfo = null;
		EOS_PresenceModification_SetRawRichText = null;
		EOS_PresenceModification_SetStatus = null;
		EOS_Presence_AddNotifyJoinGameAccepted = null;
		EOS_Presence_AddNotifyOnPresenceChanged = null;
		EOS_Presence_CopyPresence = null;
		EOS_Presence_CreatePresenceModification = null;
		EOS_Presence_GetJoinInfo = null;
		EOS_Presence_HasPresence = null;
		EOS_Presence_Info_Release = null;
		EOS_Presence_QueryPresence = null;
		EOS_Presence_RemoveNotifyJoinGameAccepted = null;
		EOS_Presence_RemoveNotifyOnPresenceChanged = null;
		EOS_Presence_SetPresence = null;
		EOS_ProductUserId_FromString = null;
		EOS_ProductUserId_IsValid = null;
		EOS_ProductUserId_ToString = null;
		EOS_ProgressionSnapshot_AddProgression = null;
		EOS_ProgressionSnapshot_BeginSnapshot = null;
		EOS_ProgressionSnapshot_DeleteSnapshot = null;
		EOS_ProgressionSnapshot_EndSnapshot = null;
		EOS_ProgressionSnapshot_SubmitSnapshot = null;
		EOS_RTCAdmin_CopyUserTokenByIndex = null;
		EOS_RTCAdmin_CopyUserTokenByUserId = null;
		EOS_RTCAdmin_Kick = null;
		EOS_RTCAdmin_QueryJoinRoomToken = null;
		EOS_RTCAdmin_SetParticipantHardMute = null;
		EOS_RTCAdmin_UserToken_Release = null;
		EOS_RTCAudio_AddNotifyAudioBeforeRender = null;
		EOS_RTCAudio_AddNotifyAudioBeforeSend = null;
		EOS_RTCAudio_AddNotifyAudioDevicesChanged = null;
		EOS_RTCAudio_AddNotifyAudioInputState = null;
		EOS_RTCAudio_AddNotifyAudioOutputState = null;
		EOS_RTCAudio_AddNotifyParticipantUpdated = null;
		EOS_RTCAudio_CopyInputDeviceInformationByIndex = null;
		EOS_RTCAudio_CopyOutputDeviceInformationByIndex = null;
		EOS_RTCAudio_GetAudioInputDeviceByIndex = null;
		EOS_RTCAudio_GetAudioInputDevicesCount = null;
		EOS_RTCAudio_GetAudioOutputDeviceByIndex = null;
		EOS_RTCAudio_GetAudioOutputDevicesCount = null;
		EOS_RTCAudio_GetInputDevicesCount = null;
		EOS_RTCAudio_GetOutputDevicesCount = null;
		EOS_RTCAudio_InputDeviceInformation_Release = null;
		EOS_RTCAudio_OutputDeviceInformation_Release = null;
		EOS_RTCAudio_QueryInputDevicesInformation = null;
		EOS_RTCAudio_QueryOutputDevicesInformation = null;
		EOS_RTCAudio_RegisterPlatformAudioUser = null;
		EOS_RTCAudio_RegisterPlatformUser = null;
		EOS_RTCAudio_RemoveNotifyAudioBeforeRender = null;
		EOS_RTCAudio_RemoveNotifyAudioBeforeSend = null;
		EOS_RTCAudio_RemoveNotifyAudioDevicesChanged = null;
		EOS_RTCAudio_RemoveNotifyAudioInputState = null;
		EOS_RTCAudio_RemoveNotifyAudioOutputState = null;
		EOS_RTCAudio_RemoveNotifyParticipantUpdated = null;
		EOS_RTCAudio_SendAudio = null;
		EOS_RTCAudio_SetAudioInputSettings = null;
		EOS_RTCAudio_SetAudioOutputSettings = null;
		EOS_RTCAudio_SetInputDeviceSettings = null;
		EOS_RTCAudio_SetOutputDeviceSettings = null;
		EOS_RTCAudio_UnregisterPlatformAudioUser = null;
		EOS_RTCAudio_UnregisterPlatformUser = null;
		EOS_RTCAudio_UpdateParticipantVolume = null;
		EOS_RTCAudio_UpdateReceiving = null;
		EOS_RTCAudio_UpdateReceivingVolume = null;
		EOS_RTCAudio_UpdateSending = null;
		EOS_RTCAudio_UpdateSendingVolume = null;
		EOS_RTC_AddNotifyDisconnected = null;
		EOS_RTC_AddNotifyParticipantStatusChanged = null;
		EOS_RTC_AddNotifyRoomStatisticsUpdated = null;
		EOS_RTC_BlockParticipant = null;
		EOS_RTC_GetAudioInterface = null;
		EOS_RTC_JoinRoom = null;
		EOS_RTC_LeaveRoom = null;
		EOS_RTC_RemoveNotifyDisconnected = null;
		EOS_RTC_RemoveNotifyParticipantStatusChanged = null;
		EOS_RTC_RemoveNotifyRoomStatisticsUpdated = null;
		EOS_RTC_SetRoomSetting = null;
		EOS_RTC_SetSetting = null;
		EOS_Reports_SendPlayerBehaviorReport = null;
		EOS_Sanctions_CopyPlayerSanctionByIndex = null;
		EOS_Sanctions_GetPlayerSanctionCount = null;
		EOS_Sanctions_PlayerSanction_Release = null;
		EOS_Sanctions_QueryActivePlayerSanctions = null;
		EOS_SessionDetails_Attribute_Release = null;
		EOS_SessionDetails_CopyInfo = null;
		EOS_SessionDetails_CopySessionAttributeByIndex = null;
		EOS_SessionDetails_CopySessionAttributeByKey = null;
		EOS_SessionDetails_GetSessionAttributeCount = null;
		EOS_SessionDetails_Info_Release = null;
		EOS_SessionDetails_Release = null;
		EOS_SessionModification_AddAttribute = null;
		EOS_SessionModification_Release = null;
		EOS_SessionModification_RemoveAttribute = null;
		EOS_SessionModification_SetAllowedPlatformIds = null;
		EOS_SessionModification_SetBucketId = null;
		EOS_SessionModification_SetHostAddress = null;
		EOS_SessionModification_SetInvitesAllowed = null;
		EOS_SessionModification_SetJoinInProgressAllowed = null;
		EOS_SessionModification_SetMaxPlayers = null;
		EOS_SessionModification_SetPermissionLevel = null;
		EOS_SessionSearch_CopySearchResultByIndex = null;
		EOS_SessionSearch_Find = null;
		EOS_SessionSearch_GetSearchResultCount = null;
		EOS_SessionSearch_Release = null;
		EOS_SessionSearch_RemoveParameter = null;
		EOS_SessionSearch_SetMaxResults = null;
		EOS_SessionSearch_SetParameter = null;
		EOS_SessionSearch_SetSessionId = null;
		EOS_SessionSearch_SetTargetUserId = null;
		EOS_Sessions_AddNotifyJoinSessionAccepted = null;
		EOS_Sessions_AddNotifyLeaveSessionRequested = null;
		EOS_Sessions_AddNotifySendSessionNativeInviteRequested = null;
		EOS_Sessions_AddNotifySessionInviteAccepted = null;
		EOS_Sessions_AddNotifySessionInviteReceived = null;
		EOS_Sessions_AddNotifySessionInviteRejected = null;
		EOS_Sessions_CopyActiveSessionHandle = null;
		EOS_Sessions_CopySessionHandleByInviteId = null;
		EOS_Sessions_CopySessionHandleByUiEventId = null;
		EOS_Sessions_CopySessionHandleForPresence = null;
		EOS_Sessions_CreateSessionModification = null;
		EOS_Sessions_CreateSessionSearch = null;
		EOS_Sessions_DestroySession = null;
		EOS_Sessions_DumpSessionState = null;
		EOS_Sessions_EndSession = null;
		EOS_Sessions_GetInviteCount = null;
		EOS_Sessions_GetInviteIdByIndex = null;
		EOS_Sessions_IsUserInSession = null;
		EOS_Sessions_JoinSession = null;
		EOS_Sessions_QueryInvites = null;
		EOS_Sessions_RegisterPlayers = null;
		EOS_Sessions_RejectInvite = null;
		EOS_Sessions_RemoveNotifyJoinSessionAccepted = null;
		EOS_Sessions_RemoveNotifyLeaveSessionRequested = null;
		EOS_Sessions_RemoveNotifySendSessionNativeInviteRequested = null;
		EOS_Sessions_RemoveNotifySessionInviteAccepted = null;
		EOS_Sessions_RemoveNotifySessionInviteReceived = null;
		EOS_Sessions_RemoveNotifySessionInviteRejected = null;
		EOS_Sessions_SendInvite = null;
		EOS_Sessions_StartSession = null;
		EOS_Sessions_UnregisterPlayers = null;
		EOS_Sessions_UpdateSession = null;
		EOS_Sessions_UpdateSessionModification = null;
		EOS_Shutdown = null;
		EOS_Stats_CopyStatByIndex = null;
		EOS_Stats_CopyStatByName = null;
		EOS_Stats_GetStatsCount = null;
		EOS_Stats_IngestStat = null;
		EOS_Stats_QueryStats = null;
		EOS_Stats_Stat_Release = null;
		EOS_TitleStorageFileTransferRequest_CancelRequest = null;
		EOS_TitleStorageFileTransferRequest_GetFileRequestState = null;
		EOS_TitleStorageFileTransferRequest_GetFilename = null;
		EOS_TitleStorageFileTransferRequest_Release = null;
		EOS_TitleStorage_CopyFileMetadataAtIndex = null;
		EOS_TitleStorage_CopyFileMetadataByFilename = null;
		EOS_TitleStorage_DeleteCache = null;
		EOS_TitleStorage_FileMetadata_Release = null;
		EOS_TitleStorage_GetFileMetadataCount = null;
		EOS_TitleStorage_QueryFile = null;
		EOS_TitleStorage_QueryFileList = null;
		EOS_TitleStorage_ReadFile = null;
		EOS_UI_AcknowledgeEventId = null;
		EOS_UI_AddNotifyDisplaySettingsUpdated = null;
		EOS_UI_AddNotifyMemoryMonitor = null;
		EOS_UI_GetFriendsExclusiveInput = null;
		EOS_UI_GetFriendsVisible = null;
		EOS_UI_GetNotificationLocationPreference = null;
		EOS_UI_GetToggleFriendsButton = null;
		EOS_UI_GetToggleFriendsKey = null;
		EOS_UI_HideFriends = null;
		EOS_UI_IsSocialOverlayPaused = null;
		EOS_UI_IsValidButtonCombination = null;
		EOS_UI_IsValidKeyCombination = null;
		EOS_UI_PauseSocialOverlay = null;
		EOS_UI_PrePresent = null;
		EOS_UI_RemoveNotifyDisplaySettingsUpdated = null;
		EOS_UI_RemoveNotifyMemoryMonitor = null;
		EOS_UI_ReportInputState = null;
		EOS_UI_SetDisplayPreference = null;
		EOS_UI_SetToggleFriendsButton = null;
		EOS_UI_SetToggleFriendsKey = null;
		EOS_UI_ShowBlockPlayer = null;
		EOS_UI_ShowFriends = null;
		EOS_UI_ShowNativeProfile = null;
		EOS_UI_ShowReportPlayer = null;
		EOS_UserInfo_BestDisplayName_Release = null;
		EOS_UserInfo_CopyBestDisplayName = null;
		EOS_UserInfo_CopyBestDisplayNameWithPlatform = null;
		EOS_UserInfo_CopyExternalUserInfoByAccountId = null;
		EOS_UserInfo_CopyExternalUserInfoByAccountType = null;
		EOS_UserInfo_CopyExternalUserInfoByIndex = null;
		EOS_UserInfo_CopyUserInfo = null;
		EOS_UserInfo_ExternalUserInfo_Release = null;
		EOS_UserInfo_GetExternalUserInfoCount = null;
		EOS_UserInfo_GetLocalPlatformType = null;
		EOS_UserInfo_QueryUserInfo = null;
		EOS_UserInfo_QueryUserInfoByDisplayName = null;
		EOS_UserInfo_QueryUserInfoByExternalAccount = null;
		EOS_UserInfo_Release = null;
	}
}
