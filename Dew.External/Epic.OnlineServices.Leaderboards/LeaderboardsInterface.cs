using System;

namespace Epic.OnlineServices.Leaderboards;

public sealed class LeaderboardsInterface : Handle
{
	public const int CopyleaderboarddefinitionbyindexApiLatest = 1;

	public const int CopyleaderboarddefinitionbyleaderboardidApiLatest = 1;

	public const int CopyleaderboardrecordbyindexApiLatest = 2;

	public const int CopyleaderboardrecordbyuseridApiLatest = 2;

	public const int CopyleaderboarduserscorebyindexApiLatest = 1;

	public const int CopyleaderboarduserscorebyuseridApiLatest = 1;

	public const int DefinitionApiLatest = 1;

	public const int GetleaderboarddefinitioncountApiLatest = 1;

	public const int GetleaderboardrecordcountApiLatest = 1;

	public const int GetleaderboarduserscorecountApiLatest = 1;

	public const int LeaderboardrecordApiLatest = 2;

	public const int LeaderboarduserscoreApiLatest = 1;

	public const int QueryleaderboarddefinitionsApiLatest = 2;

	public const int QueryleaderboardranksApiLatest = 2;

	public const int QueryleaderboarduserscoresApiLatest = 2;

	public const int TimeUndefined = -1;

	public const int UserscoresquerystatinfoApiLatest = 1;

	public LeaderboardsInterface()
	{
	}

	public LeaderboardsInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyLeaderboardDefinitionByIndex(ref CopyLeaderboardDefinitionByIndexOptions options, out Definition? outLeaderboardDefinition)
	{
		CopyLeaderboardDefinitionByIndexOptionsInternal optionsInternal = default(CopyLeaderboardDefinitionByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLeaderboardDefinitionAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Leaderboards_CopyLeaderboardDefinitionByIndex(base.InnerHandle, ref optionsInternal, ref outLeaderboardDefinitionAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<DefinitionInternal, Definition>(outLeaderboardDefinitionAddress, out outLeaderboardDefinition);
		if (outLeaderboardDefinition.HasValue)
		{
			Bindings.EOS_Leaderboards_Definition_Release(outLeaderboardDefinitionAddress);
		}
		return result;
	}

	public Result CopyLeaderboardDefinitionByLeaderboardId(ref CopyLeaderboardDefinitionByLeaderboardIdOptions options, out Definition? outLeaderboardDefinition)
	{
		CopyLeaderboardDefinitionByLeaderboardIdOptionsInternal optionsInternal = default(CopyLeaderboardDefinitionByLeaderboardIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLeaderboardDefinitionAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardId(base.InnerHandle, ref optionsInternal, ref outLeaderboardDefinitionAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<DefinitionInternal, Definition>(outLeaderboardDefinitionAddress, out outLeaderboardDefinition);
		if (outLeaderboardDefinition.HasValue)
		{
			Bindings.EOS_Leaderboards_Definition_Release(outLeaderboardDefinitionAddress);
		}
		return result;
	}

	public Result CopyLeaderboardRecordByIndex(ref CopyLeaderboardRecordByIndexOptions options, out LeaderboardRecord? outLeaderboardRecord)
	{
		CopyLeaderboardRecordByIndexOptionsInternal optionsInternal = default(CopyLeaderboardRecordByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLeaderboardRecordAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Leaderboards_CopyLeaderboardRecordByIndex(base.InnerHandle, ref optionsInternal, ref outLeaderboardRecordAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<LeaderboardRecordInternal, LeaderboardRecord>(outLeaderboardRecordAddress, out outLeaderboardRecord);
		if (outLeaderboardRecord.HasValue)
		{
			Bindings.EOS_Leaderboards_LeaderboardRecord_Release(outLeaderboardRecordAddress);
		}
		return result;
	}

	public Result CopyLeaderboardRecordByUserId(ref CopyLeaderboardRecordByUserIdOptions options, out LeaderboardRecord? outLeaderboardRecord)
	{
		CopyLeaderboardRecordByUserIdOptionsInternal optionsInternal = default(CopyLeaderboardRecordByUserIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLeaderboardRecordAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Leaderboards_CopyLeaderboardRecordByUserId(base.InnerHandle, ref optionsInternal, ref outLeaderboardRecordAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<LeaderboardRecordInternal, LeaderboardRecord>(outLeaderboardRecordAddress, out outLeaderboardRecord);
		if (outLeaderboardRecord.HasValue)
		{
			Bindings.EOS_Leaderboards_LeaderboardRecord_Release(outLeaderboardRecordAddress);
		}
		return result;
	}

	public Result CopyLeaderboardUserScoreByIndex(ref CopyLeaderboardUserScoreByIndexOptions options, out LeaderboardUserScore? outLeaderboardUserScore)
	{
		CopyLeaderboardUserScoreByIndexOptionsInternal optionsInternal = default(CopyLeaderboardUserScoreByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLeaderboardUserScoreAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Leaderboards_CopyLeaderboardUserScoreByIndex(base.InnerHandle, ref optionsInternal, ref outLeaderboardUserScoreAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<LeaderboardUserScoreInternal, LeaderboardUserScore>(outLeaderboardUserScoreAddress, out outLeaderboardUserScore);
		if (outLeaderboardUserScore.HasValue)
		{
			Bindings.EOS_Leaderboards_LeaderboardUserScore_Release(outLeaderboardUserScoreAddress);
		}
		return result;
	}

	public Result CopyLeaderboardUserScoreByUserId(ref CopyLeaderboardUserScoreByUserIdOptions options, out LeaderboardUserScore? outLeaderboardUserScore)
	{
		CopyLeaderboardUserScoreByUserIdOptionsInternal optionsInternal = default(CopyLeaderboardUserScoreByUserIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLeaderboardUserScoreAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Leaderboards_CopyLeaderboardUserScoreByUserId(base.InnerHandle, ref optionsInternal, ref outLeaderboardUserScoreAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<LeaderboardUserScoreInternal, LeaderboardUserScore>(outLeaderboardUserScoreAddress, out outLeaderboardUserScore);
		if (outLeaderboardUserScore.HasValue)
		{
			Bindings.EOS_Leaderboards_LeaderboardUserScore_Release(outLeaderboardUserScoreAddress);
		}
		return result;
	}

	public uint GetLeaderboardDefinitionCount(ref GetLeaderboardDefinitionCountOptions options)
	{
		GetLeaderboardDefinitionCountOptionsInternal optionsInternal = default(GetLeaderboardDefinitionCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Leaderboards_GetLeaderboardDefinitionCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetLeaderboardRecordCount(ref GetLeaderboardRecordCountOptions options)
	{
		GetLeaderboardRecordCountOptionsInternal optionsInternal = default(GetLeaderboardRecordCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Leaderboards_GetLeaderboardRecordCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetLeaderboardUserScoreCount(ref GetLeaderboardUserScoreCountOptions options)
	{
		GetLeaderboardUserScoreCountOptionsInternal optionsInternal = default(GetLeaderboardUserScoreCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Leaderboards_GetLeaderboardUserScoreCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void QueryLeaderboardDefinitions(ref QueryLeaderboardDefinitionsOptions options, object clientData, OnQueryLeaderboardDefinitionsCompleteCallback completionDelegate)
	{
		QueryLeaderboardDefinitionsOptionsInternal optionsInternal = default(QueryLeaderboardDefinitionsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryLeaderboardDefinitionsCompleteCallbackInternal completionDelegateInternal = OnQueryLeaderboardDefinitionsCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Leaderboards_QueryLeaderboardDefinitions(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryLeaderboardRanks(ref QueryLeaderboardRanksOptions options, object clientData, OnQueryLeaderboardRanksCompleteCallback completionDelegate)
	{
		QueryLeaderboardRanksOptionsInternal optionsInternal = default(QueryLeaderboardRanksOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryLeaderboardRanksCompleteCallbackInternal completionDelegateInternal = OnQueryLeaderboardRanksCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Leaderboards_QueryLeaderboardRanks(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryLeaderboardUserScores(ref QueryLeaderboardUserScoresOptions options, object clientData, OnQueryLeaderboardUserScoresCompleteCallback completionDelegate)
	{
		QueryLeaderboardUserScoresOptionsInternal optionsInternal = default(QueryLeaderboardUserScoresOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryLeaderboardUserScoresCompleteCallbackInternal completionDelegateInternal = OnQueryLeaderboardUserScoresCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Leaderboards_QueryLeaderboardUserScores(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnQueryLeaderboardDefinitionsCompleteCallbackInternal))]
	internal static void OnQueryLeaderboardDefinitionsCompleteCallbackInternalImplementation(ref OnQueryLeaderboardDefinitionsCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnQueryLeaderboardDefinitionsCompleteCallbackInfoInternal, OnQueryLeaderboardDefinitionsCompleteCallback, OnQueryLeaderboardDefinitionsCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryLeaderboardRanksCompleteCallbackInternal))]
	internal static void OnQueryLeaderboardRanksCompleteCallbackInternalImplementation(ref OnQueryLeaderboardRanksCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnQueryLeaderboardRanksCompleteCallbackInfoInternal, OnQueryLeaderboardRanksCompleteCallback, OnQueryLeaderboardRanksCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryLeaderboardUserScoresCompleteCallbackInternal))]
	internal static void OnQueryLeaderboardUserScoresCompleteCallbackInternalImplementation(ref OnQueryLeaderboardUserScoresCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnQueryLeaderboardUserScoresCompleteCallbackInfoInternal, OnQueryLeaderboardUserScoresCompleteCallback, OnQueryLeaderboardUserScoresCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
