using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices;
using Epic.OnlineServices.Achievements;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.Ecom;
using Epic.OnlineServices.Friends;
using Epic.OnlineServices.Leaderboards;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Metrics;
using Epic.OnlineServices.Mods;
using Epic.OnlineServices.P2P;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.PlayerDataStorage;
using Epic.OnlineServices.Presence;
using Epic.OnlineServices.Sessions;
using Epic.OnlineServices.TitleStorage;
using Epic.OnlineServices.UI;
using Epic.OnlineServices.UserInfo;
using UnityEngine;

namespace EpicTransport;

[DefaultExecutionOrder(-32000)]
public class EOSSDKComponent : MonoBehaviour
{
	[SerializeField]
	private EosApiKey apiKeys;

	[Header("User Login")]
	public bool authInterfaceLogin;

	public LoginCredentialType authInterfaceCredentialType = LoginCredentialType.AccountPortal;

	public uint devAuthToolPort = 7878u;

	public string devAuthToolCredentialName = "";

	public ExternalCredentialType connectInterfaceCredentialType = ExternalCredentialType.DeviceidAccessToken;

	public string deviceModel = "PC Windows 64bit";

	[SerializeField]
	private string displayName = "User";

	[Header("Misc")]
	public LogLevel epicLoggerLevel = LogLevel.Error;

	[SerializeField]
	public bool collectPlayerMetrics = true;

	public bool checkForEpicLauncherAndRestart;

	public bool delayedInitialization;

	public float platformTickIntervalInSeconds;

	private float platformTickTimer;

	public uint tickBudgetInMilliseconds;

	private ulong authExpirationHandle;

	private string authInterfaceLoginCredentialId;

	private string authInterfaceCredentialToken;

	private string connectInterfaceCredentialToken;

	public PlatformInterface EOS;

	protected EpicAccountId localUserAccountId;

	protected string localUserAccountIdString;

	protected ProductUserId localUserProductId;

	protected string localUserProductIdString;

	protected bool initialized;

	protected bool isConnecting;

	protected static EOSSDKComponent instance;

	private IntPtr libraryPointer;

	public static string DisplayName
	{
		get
		{
			return Instance.displayName;
		}
		set
		{
			Instance.displayName = value;
		}
	}

	public static bool CollectPlayerMetrics => Instance.collectPlayerMetrics;

	public static EpicAccountId LocalUserAccountId => Instance.localUserAccountId;

	public static string LocalUserAccountIdString => Instance.localUserAccountIdString;

	public static ProductUserId LocalUserProductId => Instance.localUserProductId;

	public static string LocalUserProductIdString => Instance.localUserProductIdString;

	public static bool Initialized => Instance.initialized;

	public static bool IsConnecting => Instance.isConnecting;

	public static EOSSDKComponent Instance => instance;

	public static void SetAuthInterfaceLoginCredentialId(string credentialId)
	{
		Instance.authInterfaceLoginCredentialId = credentialId;
	}

	public static void SetAuthInterfaceCredentialToken(string credentialToken)
	{
		Instance.authInterfaceCredentialToken = credentialToken;
	}

	public static void SetConnectInterfaceCredentialToken(string credentialToken)
	{
		Instance.connectInterfaceCredentialToken = credentialToken;
	}

	public static AchievementsInterface GetAchievementsInterface()
	{
		return Instance.EOS.GetAchievementsInterface();
	}

	public static AuthInterface GetAuthInterface()
	{
		return Instance.EOS.GetAuthInterface();
	}

	public static ConnectInterface GetConnectInterface()
	{
		return Instance.EOS.GetConnectInterface();
	}

	public static EcomInterface GetEcomInterface()
	{
		return Instance.EOS.GetEcomInterface();
	}

	public static FriendsInterface GetFriendsInterface()
	{
		return Instance.EOS.GetFriendsInterface();
	}

	public static LeaderboardsInterface GetLeaderboardsInterface()
	{
		return Instance.EOS.GetLeaderboardsInterface();
	}

	public static LobbyInterface GetLobbyInterface()
	{
		return Instance.EOS.GetLobbyInterface();
	}

	public static MetricsInterface GetMetricsInterface()
	{
		return Instance.EOS.GetMetricsInterface();
	}

	public static ModsInterface GetModsInterface()
	{
		return Instance.EOS.GetModsInterface();
	}

	public static P2PInterface GetP2PInterface()
	{
		return Instance.EOS.GetP2PInterface();
	}

	public static PlayerDataStorageInterface GetPlayerDataStorageInterface()
	{
		return Instance.EOS.GetPlayerDataStorageInterface();
	}

	public static PresenceInterface GetPresenceInterface()
	{
		return Instance.EOS.GetPresenceInterface();
	}

	public static SessionsInterface GetSessionsInterface()
	{
		return Instance.EOS.GetSessionsInterface();
	}

	public static TitleStorageInterface GetTitleStorageInterface()
	{
		return Instance.EOS.GetTitleStorageInterface();
	}

	public static UIInterface GetUIInterface()
	{
		return Instance.EOS.GetUIInterface();
	}

	public static UserInfoInterface GetUserInfoInterface()
	{
		return Instance.EOS.GetUserInfoInterface();
	}

	public static void Tick()
	{
		instance.platformTickTimer -= Time.deltaTime;
		instance.EOS.Tick();
	}

	[DllImport("Kernel32.dll")]
	private static extern IntPtr LoadLibrary(string lpLibFileName);

	[DllImport("Kernel32.dll")]
	private static extern int FreeLibrary(IntPtr hLibModule);

	[DllImport("Kernel32.dll")]
	private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

	private void Awake()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext", Array.Empty<object>());
			new AndroidJavaClass("com.epicgames.mobile.eossdk.EOSSDK").CallStatic("init", context);
		}
		if (instance != null)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		instance = this;
		string libraryPath = "Shape of Dreams_Data/Plugins/x86_64/EOSSDK-Win64-Shipping";
		libraryPointer = LoadLibrary(libraryPath);
		if (libraryPointer == IntPtr.Zero)
		{
			throw new Exception("Failed to load library" + libraryPath);
		}
		Bindings.Hook(libraryPointer, GetProcAddress);
		if (!delayedInitialization)
		{
			Initialize();
		}
	}

	protected void InitializeImplementation()
	{
		isConnecting = true;
		InitializeOptions initializeOptions = default(InitializeOptions);
		initializeOptions.ProductName = apiKeys.epicProductName;
		initializeOptions.ProductVersion = apiKeys.epicProductVersion;
		InitializeOptions initializeOptions2 = initializeOptions;
		Result initializeResult = PlatformInterface.Initialize(ref initializeOptions2);
		bool isAlreadyConfiguredInEditor = Application.isEditor && initializeResult == Result.AlreadyConfigured;
		if (initializeResult != 0 && !isAlreadyConfiguredInEditor)
		{
			throw new Exception("Failed to initialize platform: " + initializeResult);
		}
		LoggingInterface.SetLogLevel(LogCategory.AllCategories, epicLoggerLevel);
		LoggingInterface.SetCallback(delegate(ref LogMessage message)
		{
			Logger.EpicDebugLog(message);
		});
		Options options = default(Options);
		options.ProductId = apiKeys.epicProductId;
		options.SandboxId = apiKeys.epicSandboxId;
		options.DeploymentId = apiKeys.epicDeploymentId;
		options.ClientCredentials = new ClientCredentials
		{
			ClientId = apiKeys.epicClientId,
			ClientSecret = apiKeys.epicClientSecret
		};
		options.TickBudgetInMilliseconds = tickBudgetInMilliseconds;
		Options options2 = options;
		EOS = PlatformInterface.Create(ref options2);
		if (EOS == null)
		{
			throw new Exception("Failed to create platform");
		}
		if (checkForEpicLauncherAndRestart)
		{
			Result result = EOS.CheckForLauncherAndRestart();
			if (result != Result.NoChange)
			{
				if (result == Result.UnexpectedError)
				{
					Debug.LogError("Unexpected Error while checking if app was started through epic launcher");
				}
				Application.Quit();
			}
		}
		if (authInterfaceLogin)
		{
			if (authInterfaceCredentialType == LoginCredentialType.Developer)
			{
				authInterfaceLoginCredentialId = "localhost:" + devAuthToolPort;
				authInterfaceCredentialToken = devAuthToolCredentialName;
			}
			global::Epic.OnlineServices.Auth.LoginOptions loginOptions = default(global::Epic.OnlineServices.Auth.LoginOptions);
			loginOptions.Credentials = new global::Epic.OnlineServices.Auth.Credentials
			{
				Type = authInterfaceCredentialType,
				Id = authInterfaceLoginCredentialId,
				Token = authInterfaceCredentialToken
			};
			loginOptions.ScopeFlags = AuthScopeFlags.BasicProfile | AuthScopeFlags.FriendsList | AuthScopeFlags.Presence;
			global::Epic.OnlineServices.Auth.LoginOptions loginOptions2 = loginOptions;
			EOS.GetAuthInterface().Login(ref loginOptions2, null, OnAuthInterfaceLogin);
		}
		else if (connectInterfaceCredentialType == ExternalCredentialType.DeviceidAccessToken)
		{
			CreateDeviceIdOptions createDeviceIdOptions = default(CreateDeviceIdOptions);
			createDeviceIdOptions.DeviceModel = deviceModel;
			EOS.GetConnectInterface().CreateDeviceId(ref createDeviceIdOptions, null, OnCreateDeviceId);
		}
		else
		{
			ConnectInterfaceLogin();
		}
	}

	public static void Initialize()
	{
		if (!Instance.initialized && !Instance.isConnecting)
		{
			Instance.InitializeImplementation();
		}
	}

	private void OnAuthInterfaceLogin(ref global::Epic.OnlineServices.Auth.LoginCallbackInfo loginCallbackInfo)
	{
		if (loginCallbackInfo.ResultCode == Result.Success)
		{
			Debug.Log("Auth Interface Login succeeded");
			if (loginCallbackInfo.LocalUserId.ToString(out var accountIdString) == Result.Success)
			{
				Debug.Log("EOS User ID:" + accountIdString);
				localUserAccountIdString = accountIdString;
				localUserAccountId = loginCallbackInfo.LocalUserId;
			}
			ConnectInterfaceLogin();
		}
		else if (global::Epic.OnlineServices.Common.IsOperationComplete(loginCallbackInfo.ResultCode))
		{
			Debug.Log("Login returned " + loginCallbackInfo.ResultCode);
		}
	}

	public void OnCreateDeviceId(ref CreateDeviceIdCallbackInfo createDeviceIdCallbackInfo)
	{
		if (createDeviceIdCallbackInfo.ResultCode == Result.Success || createDeviceIdCallbackInfo.ResultCode == Result.DuplicateNotAllowed)
		{
			ConnectInterfaceLogin();
		}
		else if (global::Epic.OnlineServices.Common.IsOperationComplete(createDeviceIdCallbackInfo.ResultCode))
		{
			Debug.Log("Device ID creation returned " + createDeviceIdCallbackInfo.ResultCode);
		}
	}

	private void ConnectInterfaceLogin()
	{
		global::Epic.OnlineServices.Connect.LoginOptions loginOptions = default(global::Epic.OnlineServices.Connect.LoginOptions);
		if (connectInterfaceCredentialType == ExternalCredentialType.Epic)
		{
			CopyUserAuthTokenOptions copyUserAuthTokenOptions = default(CopyUserAuthTokenOptions);
			if (EOS.GetAuthInterface().CopyUserAuthToken(ref copyUserAuthTokenOptions, localUserAccountId, out var token) == Result.Success)
			{
				connectInterfaceCredentialToken = token?.AccessToken;
			}
			else
			{
				Debug.LogError("Failed to retrieve User Auth Token");
			}
		}
		else if (connectInterfaceCredentialType == ExternalCredentialType.DeviceidAccessToken)
		{
			loginOptions.UserLoginInfo = new UserLoginInfo
			{
				DisplayName = displayName
			};
		}
		loginOptions.Credentials = new global::Epic.OnlineServices.Connect.Credentials
		{
			Type = connectInterfaceCredentialType,
			Token = connectInterfaceCredentialToken
		};
		EOS.GetConnectInterface().Login(ref loginOptions, null, OnConnectInterfaceLogin);
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		SetApplicationStatus(hasFocus);
	}

	private void SetApplicationStatus(bool focus)
	{
		if (initialized && !isConnecting)
		{
			ApplicationStatus status = ApplicationStatus.Foreground;
			if (!focus)
			{
				status = (Application.runInBackground ? ApplicationStatus.BackgroundUnconstrained : ApplicationStatus.BackgroundSuspended);
			}
			EOS.SetApplicationStatus(status);
		}
	}

	private void OnConnectInterfaceLogin(ref global::Epic.OnlineServices.Connect.LoginCallbackInfo loginCallbackInfo)
	{
		if (loginCallbackInfo.ResultCode == Result.Success)
		{
			Debug.Log("Connect Interface Login succeeded");
			if (loginCallbackInfo.LocalUserId.ToString(out var productIdString) == Result.Success)
			{
				Debug.Log("EOS User Product ID:" + productIdString);
				localUserProductIdString = productIdString;
				localUserProductId = loginCallbackInfo.LocalUserId;
			}
			initialized = true;
			isConnecting = false;
			SetApplicationStatus(focus: true);
			EOS.SetNetworkStatus(NetworkStatus.Online);
			AddNotifyAuthExpirationOptions authExpirationOptions = default(AddNotifyAuthExpirationOptions);
			authExpirationHandle = EOS.GetConnectInterface().AddNotifyAuthExpiration(ref authExpirationOptions, null, OnAuthExpiration);
		}
		else
		{
			if (!global::Epic.OnlineServices.Common.IsOperationComplete(loginCallbackInfo.ResultCode))
			{
				return;
			}
			Debug.Log("Login returned " + loginCallbackInfo.ResultCode.ToString() + "\nRetrying...");
			CreateUserOptions createUserOptions = default(CreateUserOptions);
			createUserOptions.ContinuanceToken = loginCallbackInfo.ContinuanceToken;
			CreateUserOptions createUserOptions2 = createUserOptions;
			EOS.GetConnectInterface().CreateUser(ref createUserOptions2, null, delegate(ref CreateUserCallbackInfo cb)
			{
				if (cb.ResultCode != 0)
				{
					Debug.Log(cb.ResultCode);
				}
				else
				{
					localUserProductId = cb.LocalUserId;
					ConnectInterfaceLogin();
				}
			});
		}
	}

	public void OnAuthExpiration(ref AuthExpirationCallbackInfo authExpirationCallbackInfo)
	{
		Debug.Log("AuthExpiration callback");
		EOS.GetConnectInterface().RemoveNotifyAuthExpiration(authExpirationHandle);
		ConnectInterfaceLogin();
	}

	private void LateUpdate()
	{
		if (EOS != null)
		{
			platformTickTimer += Time.deltaTime;
			if (platformTickTimer >= platformTickIntervalInSeconds)
			{
				platformTickTimer = 0f;
				EOS.Tick();
			}
		}
	}

	private void OnDestroy()
	{
		if (EOS != null)
		{
			EOS.Release();
			EOS = null;
			PlatformInterface.Shutdown();
		}
		if (libraryPointer != IntPtr.Zero)
		{
			Bindings.Unhook();
			while (FreeLibrary(libraryPointer) != 0)
			{
			}
			libraryPointer = IntPtr.Zero;
		}
	}
}
