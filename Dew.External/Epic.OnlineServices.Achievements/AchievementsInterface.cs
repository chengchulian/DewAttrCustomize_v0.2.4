using System;

namespace Epic.OnlineServices.Achievements;

public sealed class AchievementsInterface : Handle
{
	public const int AchievementUnlocktimeUndefined = -1;

	public const int AddnotifyachievementsunlockedApiLatest = 1;

	public const int Addnotifyachievementsunlockedv2ApiLatest = 2;

	public const int Copyachievementdefinitionv2ByachievementidApiLatest = 2;

	public const int Copyachievementdefinitionv2ByindexApiLatest = 2;

	public const int CopydefinitionbyachievementidApiLatest = 1;

	public const int CopydefinitionbyindexApiLatest = 1;

	public const int Copydefinitionv2ByachievementidApiLatest = 2;

	public const int Copydefinitionv2ByindexApiLatest = 2;

	public const int CopyplayerachievementbyachievementidApiLatest = 2;

	public const int CopyplayerachievementbyindexApiLatest = 2;

	public const int CopyunlockedachievementbyachievementidApiLatest = 1;

	public const int CopyunlockedachievementbyindexApiLatest = 1;

	public const int DefinitionApiLatest = 1;

	public const int Definitionv2ApiLatest = 2;

	public const int GetachievementdefinitioncountApiLatest = 1;

	public const int GetplayerachievementcountApiLatest = 1;

	public const int GetunlockedachievementcountApiLatest = 1;

	public const int PlayerachievementApiLatest = 2;

	public const int PlayerstatinfoApiLatest = 1;

	public const int QuerydefinitionsApiLatest = 3;

	public const int QueryplayerachievementsApiLatest = 2;

	public const int StatthresholdApiLatest = 1;

	public const int StatthresholdsApiLatest = 1;

	public const int UnlockachievementsApiLatest = 1;

	public const int UnlockedachievementApiLatest = 1;

	public AchievementsInterface()
	{
	}

	public AchievementsInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public ulong AddNotifyAchievementsUnlocked(ref AddNotifyAchievementsUnlockedOptions options, object clientData, OnAchievementsUnlockedCallback notificationFn)
	{
		AddNotifyAchievementsUnlockedOptionsInternal optionsInternal = default(AddNotifyAchievementsUnlockedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnAchievementsUnlockedCallbackInternal notificationFnInternal = OnAchievementsUnlockedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Achievements_AddNotifyAchievementsUnlocked(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyAchievementsUnlockedV2(ref AddNotifyAchievementsUnlockedV2Options options, object clientData, OnAchievementsUnlockedCallbackV2 notificationFn)
	{
		AddNotifyAchievementsUnlockedV2OptionsInternal optionsInternal = default(AddNotifyAchievementsUnlockedV2OptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnAchievementsUnlockedCallbackV2Internal notificationFnInternal = OnAchievementsUnlockedCallbackV2InternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Achievements_AddNotifyAchievementsUnlockedV2(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result CopyAchievementDefinitionByAchievementId(ref CopyAchievementDefinitionByAchievementIdOptions options, out Definition? outDefinition)
	{
		CopyAchievementDefinitionByAchievementIdOptionsInternal optionsInternal = default(CopyAchievementDefinitionByAchievementIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outDefinitionAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Achievements_CopyAchievementDefinitionByAchievementId(base.InnerHandle, ref optionsInternal, ref outDefinitionAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<DefinitionInternal, Definition>(outDefinitionAddress, out outDefinition);
		if (outDefinition.HasValue)
		{
			Bindings.EOS_Achievements_Definition_Release(outDefinitionAddress);
		}
		return result;
	}

	public Result CopyAchievementDefinitionByIndex(ref CopyAchievementDefinitionByIndexOptions options, out Definition? outDefinition)
	{
		CopyAchievementDefinitionByIndexOptionsInternal optionsInternal = default(CopyAchievementDefinitionByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outDefinitionAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Achievements_CopyAchievementDefinitionByIndex(base.InnerHandle, ref optionsInternal, ref outDefinitionAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<DefinitionInternal, Definition>(outDefinitionAddress, out outDefinition);
		if (outDefinition.HasValue)
		{
			Bindings.EOS_Achievements_Definition_Release(outDefinitionAddress);
		}
		return result;
	}

	public Result CopyAchievementDefinitionV2ByAchievementId(ref CopyAchievementDefinitionV2ByAchievementIdOptions options, out DefinitionV2? outDefinition)
	{
		CopyAchievementDefinitionV2ByAchievementIdOptionsInternal optionsInternal = default(CopyAchievementDefinitionV2ByAchievementIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outDefinitionAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Achievements_CopyAchievementDefinitionV2ByAchievementId(base.InnerHandle, ref optionsInternal, ref outDefinitionAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<DefinitionV2Internal, DefinitionV2>(outDefinitionAddress, out outDefinition);
		if (outDefinition.HasValue)
		{
			Bindings.EOS_Achievements_DefinitionV2_Release(outDefinitionAddress);
		}
		return result;
	}

	public Result CopyAchievementDefinitionV2ByIndex(ref CopyAchievementDefinitionV2ByIndexOptions options, out DefinitionV2? outDefinition)
	{
		CopyAchievementDefinitionV2ByIndexOptionsInternal optionsInternal = default(CopyAchievementDefinitionV2ByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outDefinitionAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Achievements_CopyAchievementDefinitionV2ByIndex(base.InnerHandle, ref optionsInternal, ref outDefinitionAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<DefinitionV2Internal, DefinitionV2>(outDefinitionAddress, out outDefinition);
		if (outDefinition.HasValue)
		{
			Bindings.EOS_Achievements_DefinitionV2_Release(outDefinitionAddress);
		}
		return result;
	}

	public Result CopyPlayerAchievementByAchievementId(ref CopyPlayerAchievementByAchievementIdOptions options, out PlayerAchievement? outAchievement)
	{
		CopyPlayerAchievementByAchievementIdOptionsInternal optionsInternal = default(CopyPlayerAchievementByAchievementIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outAchievementAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Achievements_CopyPlayerAchievementByAchievementId(base.InnerHandle, ref optionsInternal, ref outAchievementAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<PlayerAchievementInternal, PlayerAchievement>(outAchievementAddress, out outAchievement);
		if (outAchievement.HasValue)
		{
			Bindings.EOS_Achievements_PlayerAchievement_Release(outAchievementAddress);
		}
		return result;
	}

	public Result CopyPlayerAchievementByIndex(ref CopyPlayerAchievementByIndexOptions options, out PlayerAchievement? outAchievement)
	{
		CopyPlayerAchievementByIndexOptionsInternal optionsInternal = default(CopyPlayerAchievementByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outAchievementAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Achievements_CopyPlayerAchievementByIndex(base.InnerHandle, ref optionsInternal, ref outAchievementAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<PlayerAchievementInternal, PlayerAchievement>(outAchievementAddress, out outAchievement);
		if (outAchievement.HasValue)
		{
			Bindings.EOS_Achievements_PlayerAchievement_Release(outAchievementAddress);
		}
		return result;
	}

	public Result CopyUnlockedAchievementByAchievementId(ref CopyUnlockedAchievementByAchievementIdOptions options, out UnlockedAchievement? outAchievement)
	{
		CopyUnlockedAchievementByAchievementIdOptionsInternal optionsInternal = default(CopyUnlockedAchievementByAchievementIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outAchievementAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Achievements_CopyUnlockedAchievementByAchievementId(base.InnerHandle, ref optionsInternal, ref outAchievementAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<UnlockedAchievementInternal, UnlockedAchievement>(outAchievementAddress, out outAchievement);
		if (outAchievement.HasValue)
		{
			Bindings.EOS_Achievements_UnlockedAchievement_Release(outAchievementAddress);
		}
		return result;
	}

	public Result CopyUnlockedAchievementByIndex(ref CopyUnlockedAchievementByIndexOptions options, out UnlockedAchievement? outAchievement)
	{
		CopyUnlockedAchievementByIndexOptionsInternal optionsInternal = default(CopyUnlockedAchievementByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outAchievementAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Achievements_CopyUnlockedAchievementByIndex(base.InnerHandle, ref optionsInternal, ref outAchievementAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<UnlockedAchievementInternal, UnlockedAchievement>(outAchievementAddress, out outAchievement);
		if (outAchievement.HasValue)
		{
			Bindings.EOS_Achievements_UnlockedAchievement_Release(outAchievementAddress);
		}
		return result;
	}

	public uint GetAchievementDefinitionCount(ref GetAchievementDefinitionCountOptions options)
	{
		GetAchievementDefinitionCountOptionsInternal optionsInternal = default(GetAchievementDefinitionCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Achievements_GetAchievementDefinitionCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetPlayerAchievementCount(ref GetPlayerAchievementCountOptions options)
	{
		GetPlayerAchievementCountOptionsInternal optionsInternal = default(GetPlayerAchievementCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Achievements_GetPlayerAchievementCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetUnlockedAchievementCount(ref GetUnlockedAchievementCountOptions options)
	{
		GetUnlockedAchievementCountOptionsInternal optionsInternal = default(GetUnlockedAchievementCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Achievements_GetUnlockedAchievementCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void QueryDefinitions(ref QueryDefinitionsOptions options, object clientData, OnQueryDefinitionsCompleteCallback completionDelegate)
	{
		QueryDefinitionsOptionsInternal optionsInternal = default(QueryDefinitionsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryDefinitionsCompleteCallbackInternal completionDelegateInternal = OnQueryDefinitionsCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Achievements_QueryDefinitions(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryPlayerAchievements(ref QueryPlayerAchievementsOptions options, object clientData, OnQueryPlayerAchievementsCompleteCallback completionDelegate)
	{
		QueryPlayerAchievementsOptionsInternal optionsInternal = default(QueryPlayerAchievementsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryPlayerAchievementsCompleteCallbackInternal completionDelegateInternal = OnQueryPlayerAchievementsCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Achievements_QueryPlayerAchievements(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RemoveNotifyAchievementsUnlocked(ulong inId)
	{
		Bindings.EOS_Achievements_RemoveNotifyAchievementsUnlocked(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void UnlockAchievements(ref UnlockAchievementsOptions options, object clientData, OnUnlockAchievementsCompleteCallback completionDelegate)
	{
		UnlockAchievementsOptionsInternal optionsInternal = default(UnlockAchievementsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUnlockAchievementsCompleteCallbackInternal completionDelegateInternal = OnUnlockAchievementsCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Achievements_UnlockAchievements(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnAchievementsUnlockedCallbackInternal))]
	internal static void OnAchievementsUnlockedCallbackInternalImplementation(ref OnAchievementsUnlockedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnAchievementsUnlockedCallbackInfoInternal, OnAchievementsUnlockedCallback, OnAchievementsUnlockedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnAchievementsUnlockedCallbackV2Internal))]
	internal static void OnAchievementsUnlockedCallbackV2InternalImplementation(ref OnAchievementsUnlockedCallbackV2InfoInternal data)
	{
		if (Helper.TryGetCallback<OnAchievementsUnlockedCallbackV2InfoInternal, OnAchievementsUnlockedCallbackV2, OnAchievementsUnlockedCallbackV2Info>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryDefinitionsCompleteCallbackInternal))]
	internal static void OnQueryDefinitionsCompleteCallbackInternalImplementation(ref OnQueryDefinitionsCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnQueryDefinitionsCompleteCallbackInfoInternal, OnQueryDefinitionsCompleteCallback, OnQueryDefinitionsCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryPlayerAchievementsCompleteCallbackInternal))]
	internal static void OnQueryPlayerAchievementsCompleteCallbackInternalImplementation(ref OnQueryPlayerAchievementsCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnQueryPlayerAchievementsCompleteCallbackInfoInternal, OnQueryPlayerAchievementsCompleteCallback, OnQueryPlayerAchievementsCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUnlockAchievementsCompleteCallbackInternal))]
	internal static void OnUnlockAchievementsCompleteCallbackInternalImplementation(ref OnUnlockAchievementsCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnUnlockAchievementsCompleteCallbackInfoInternal, OnUnlockAchievementsCompleteCallback, OnUnlockAchievementsCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
