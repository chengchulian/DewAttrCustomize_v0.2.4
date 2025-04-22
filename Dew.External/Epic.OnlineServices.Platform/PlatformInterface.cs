using System;
using Epic.OnlineServices.Achievements;
using Epic.OnlineServices.AntiCheatClient;
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
using Epic.OnlineServices.Metrics;
using Epic.OnlineServices.Mods;
using Epic.OnlineServices.P2P;
using Epic.OnlineServices.PlayerDataStorage;
using Epic.OnlineServices.Presence;
using Epic.OnlineServices.ProgressionSnapshot;
using Epic.OnlineServices.Reports;
using Epic.OnlineServices.RTC;
using Epic.OnlineServices.RTCAdmin;
using Epic.OnlineServices.Sanctions;
using Epic.OnlineServices.Sessions;
using Epic.OnlineServices.Stats;
using Epic.OnlineServices.TitleStorage;
using Epic.OnlineServices.UI;
using Epic.OnlineServices.UserInfo;

namespace Epic.OnlineServices.Platform;

public sealed class PlatformInterface : Handle
{
	public const int AndroidInitializeoptionssysteminitializeoptionsApiLatest = 2;

	public static readonly Utf8String CheckforlauncherandrestartEnvVar = "EOS_LAUNCHED_BY_EPIC";

	public const int ClientcredentialsClientidMaxLength = 64;

	public const int ClientcredentialsClientsecretMaxLength = 64;

	public const int CountrycodeMaxBufferLen = 5;

	public const int CountrycodeMaxLength = 4;

	public const int GetdesktopcrossplaystatusApiLatest = 1;

	public const int InitializeApiLatest = 4;

	public const int InitializeThreadaffinityApiLatest = 2;

	public const int InitializeoptionsProductnameMaxLength = 64;

	public const int InitializeoptionsProductversionMaxLength = 64;

	public const int LocalecodeMaxBufferLen = 10;

	public const int LocalecodeMaxLength = 9;

	public const int OptionsApiLatest = 13;

	public const int OptionsDeploymentidMaxLength = 64;

	public const int OptionsEncryptionkeyLength = 64;

	public const int OptionsProductidMaxLength = 64;

	public const int OptionsSandboxidMaxLength = 64;

	public const int RtcoptionsApiLatest = 2;

	public const int WindowsRtcoptionsplatformspecificoptionsApiLatest = 1;

	public static Result Initialize(ref AndroidInitializeOptions options)
	{
		AndroidInitializeOptionsInternal optionsInternal = default(AndroidInitializeOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = AndroidBindings.EOS_Initialize(ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public PlatformInterface()
	{
	}

	public PlatformInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CheckForLauncherAndRestart()
	{
		return Bindings.EOS_Platform_CheckForLauncherAndRestart(base.InnerHandle);
	}

	public static PlatformInterface Create(ref Options options)
	{
		OptionsInternal optionsInternal = default(OptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr from = Bindings.EOS_Platform_Create(ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out PlatformInterface funcResultReturn);
		return funcResultReturn;
	}

	public AchievementsInterface GetAchievementsInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetAchievementsInterface(base.InnerHandle), out AchievementsInterface funcResultReturn);
		return funcResultReturn;
	}

	public Result GetActiveCountryCode(EpicAccountId localUserId, out Utf8String outBuffer)
	{
		IntPtr localUserIdInnerHandle = IntPtr.Zero;
		Helper.Set(localUserId, ref localUserIdInnerHandle);
		int inOutBufferLength = 5;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Platform_GetActiveCountryCode(base.InnerHandle, localUserIdInnerHandle, outBufferAddress, ref inOutBufferLength);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public Result GetActiveLocaleCode(EpicAccountId localUserId, out Utf8String outBuffer)
	{
		IntPtr localUserIdInnerHandle = IntPtr.Zero;
		Helper.Set(localUserId, ref localUserIdInnerHandle);
		int inOutBufferLength = 10;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Platform_GetActiveLocaleCode(base.InnerHandle, localUserIdInnerHandle, outBufferAddress, ref inOutBufferLength);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public AntiCheatClientInterface GetAntiCheatClientInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetAntiCheatClientInterface(base.InnerHandle), out AntiCheatClientInterface funcResultReturn);
		return funcResultReturn;
	}

	public AntiCheatServerInterface GetAntiCheatServerInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetAntiCheatServerInterface(base.InnerHandle), out AntiCheatServerInterface funcResultReturn);
		return funcResultReturn;
	}

	public ApplicationStatus GetApplicationStatus()
	{
		return Bindings.EOS_Platform_GetApplicationStatus(base.InnerHandle);
	}

	public AuthInterface GetAuthInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetAuthInterface(base.InnerHandle), out AuthInterface funcResultReturn);
		return funcResultReturn;
	}

	public ConnectInterface GetConnectInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetConnectInterface(base.InnerHandle), out ConnectInterface funcResultReturn);
		return funcResultReturn;
	}

	public CustomInvitesInterface GetCustomInvitesInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetCustomInvitesInterface(base.InnerHandle), out CustomInvitesInterface funcResultReturn);
		return funcResultReturn;
	}

	public Result GetDesktopCrossplayStatus(ref GetDesktopCrossplayStatusOptions options, out DesktopCrossplayStatusInfo outDesktopCrossplayStatusInfo)
	{
		GetDesktopCrossplayStatusOptionsInternal optionsInternal = default(GetDesktopCrossplayStatusOptionsInternal);
		optionsInternal.Set(ref options);
		DesktopCrossplayStatusInfoInternal outDesktopCrossplayStatusInfoInternal = Helper.GetDefault<DesktopCrossplayStatusInfoInternal>();
		Result result = Bindings.EOS_Platform_GetDesktopCrossplayStatus(base.InnerHandle, ref optionsInternal, ref outDesktopCrossplayStatusInfoInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<DesktopCrossplayStatusInfoInternal, DesktopCrossplayStatusInfo>(ref outDesktopCrossplayStatusInfoInternal, out outDesktopCrossplayStatusInfo);
		return result;
	}

	public EcomInterface GetEcomInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetEcomInterface(base.InnerHandle), out EcomInterface funcResultReturn);
		return funcResultReturn;
	}

	public FriendsInterface GetFriendsInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetFriendsInterface(base.InnerHandle), out FriendsInterface funcResultReturn);
		return funcResultReturn;
	}

	public IntegratedPlatformInterface GetIntegratedPlatformInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetIntegratedPlatformInterface(base.InnerHandle), out IntegratedPlatformInterface funcResultReturn);
		return funcResultReturn;
	}

	public KWSInterface GetKWSInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetKWSInterface(base.InnerHandle), out KWSInterface funcResultReturn);
		return funcResultReturn;
	}

	public LeaderboardsInterface GetLeaderboardsInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetLeaderboardsInterface(base.InnerHandle), out LeaderboardsInterface funcResultReturn);
		return funcResultReturn;
	}

	public LobbyInterface GetLobbyInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetLobbyInterface(base.InnerHandle), out LobbyInterface funcResultReturn);
		return funcResultReturn;
	}

	public MetricsInterface GetMetricsInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetMetricsInterface(base.InnerHandle), out MetricsInterface funcResultReturn);
		return funcResultReturn;
	}

	public ModsInterface GetModsInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetModsInterface(base.InnerHandle), out ModsInterface funcResultReturn);
		return funcResultReturn;
	}

	public NetworkStatus GetNetworkStatus()
	{
		return Bindings.EOS_Platform_GetNetworkStatus(base.InnerHandle);
	}

	public Result GetOverrideCountryCode(out Utf8String outBuffer)
	{
		int inOutBufferLength = 5;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Platform_GetOverrideCountryCode(base.InnerHandle, outBufferAddress, ref inOutBufferLength);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public Result GetOverrideLocaleCode(out Utf8String outBuffer)
	{
		int inOutBufferLength = 10;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Platform_GetOverrideLocaleCode(base.InnerHandle, outBufferAddress, ref inOutBufferLength);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public P2PInterface GetP2PInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetP2PInterface(base.InnerHandle), out P2PInterface funcResultReturn);
		return funcResultReturn;
	}

	public PlayerDataStorageInterface GetPlayerDataStorageInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetPlayerDataStorageInterface(base.InnerHandle), out PlayerDataStorageInterface funcResultReturn);
		return funcResultReturn;
	}

	public PresenceInterface GetPresenceInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetPresenceInterface(base.InnerHandle), out PresenceInterface funcResultReturn);
		return funcResultReturn;
	}

	public ProgressionSnapshotInterface GetProgressionSnapshotInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetProgressionSnapshotInterface(base.InnerHandle), out ProgressionSnapshotInterface funcResultReturn);
		return funcResultReturn;
	}

	public RTCAdminInterface GetRTCAdminInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetRTCAdminInterface(base.InnerHandle), out RTCAdminInterface funcResultReturn);
		return funcResultReturn;
	}

	public RTCInterface GetRTCInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetRTCInterface(base.InnerHandle), out RTCInterface funcResultReturn);
		return funcResultReturn;
	}

	public ReportsInterface GetReportsInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetReportsInterface(base.InnerHandle), out ReportsInterface funcResultReturn);
		return funcResultReturn;
	}

	public SanctionsInterface GetSanctionsInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetSanctionsInterface(base.InnerHandle), out SanctionsInterface funcResultReturn);
		return funcResultReturn;
	}

	public SessionsInterface GetSessionsInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetSessionsInterface(base.InnerHandle), out SessionsInterface funcResultReturn);
		return funcResultReturn;
	}

	public StatsInterface GetStatsInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetStatsInterface(base.InnerHandle), out StatsInterface funcResultReturn);
		return funcResultReturn;
	}

	public TitleStorageInterface GetTitleStorageInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetTitleStorageInterface(base.InnerHandle), out TitleStorageInterface funcResultReturn);
		return funcResultReturn;
	}

	public UIInterface GetUIInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetUIInterface(base.InnerHandle), out UIInterface funcResultReturn);
		return funcResultReturn;
	}

	public UserInfoInterface GetUserInfoInterface()
	{
		Helper.Get(Bindings.EOS_Platform_GetUserInfoInterface(base.InnerHandle), out UserInfoInterface funcResultReturn);
		return funcResultReturn;
	}

	public static Result Initialize(ref InitializeOptions options)
	{
		InitializeOptionsInternal optionsInternal = default(InitializeOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_Initialize(ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_Platform_Release(base.InnerHandle);
	}

	public Result SetApplicationStatus(ApplicationStatus newStatus)
	{
		return Bindings.EOS_Platform_SetApplicationStatus(base.InnerHandle, newStatus);
	}

	public Result SetNetworkStatus(NetworkStatus newStatus)
	{
		return Bindings.EOS_Platform_SetNetworkStatus(base.InnerHandle, newStatus);
	}

	public Result SetOverrideCountryCode(Utf8String newCountryCode)
	{
		IntPtr newCountryCodeAddress = IntPtr.Zero;
		Helper.Set(newCountryCode, ref newCountryCodeAddress);
		Result result = Bindings.EOS_Platform_SetOverrideCountryCode(base.InnerHandle, newCountryCodeAddress);
		Helper.Dispose(ref newCountryCodeAddress);
		return result;
	}

	public Result SetOverrideLocaleCode(Utf8String newLocaleCode)
	{
		IntPtr newLocaleCodeAddress = IntPtr.Zero;
		Helper.Set(newLocaleCode, ref newLocaleCodeAddress);
		Result result = Bindings.EOS_Platform_SetOverrideLocaleCode(base.InnerHandle, newLocaleCodeAddress);
		Helper.Dispose(ref newLocaleCodeAddress);
		return result;
	}

	public static Result Shutdown()
	{
		return Bindings.EOS_Shutdown();
	}

	public void Tick()
	{
		Bindings.EOS_Platform_Tick(base.InnerHandle);
	}

	public static Utf8String ToString(ApplicationStatus applicationStatus)
	{
		Helper.Get(Bindings.EOS_EApplicationStatus_ToString(applicationStatus), out Utf8String funcResultReturn);
		return funcResultReturn;
	}

	public static Utf8String ToString(NetworkStatus networkStatus)
	{
		Helper.Get(Bindings.EOS_ENetworkStatus_ToString(networkStatus), out Utf8String funcResultReturn);
		return funcResultReturn;
	}

	public static PlatformInterface Create(ref WindowsOptions options)
	{
		WindowsOptionsInternal optionsInternal = default(WindowsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr from = WindowsBindings.EOS_Platform_Create(ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out PlatformInterface funcResultReturn);
		return funcResultReturn;
	}
}
