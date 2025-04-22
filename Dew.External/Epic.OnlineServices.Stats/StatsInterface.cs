using System;

namespace Epic.OnlineServices.Stats;

public sealed class StatsInterface : Handle
{
	public const int CopystatbyindexApiLatest = 1;

	public const int CopystatbynameApiLatest = 1;

	public const int GetstatcountApiLatest = 1;

	public const int GetstatscountApiLatest = 1;

	public const int IngestdataApiLatest = 1;

	public const int IngeststatApiLatest = 3;

	public const int MaxIngestStats = 3000;

	public const int MaxQueryStats = 1000;

	public const int QuerystatsApiLatest = 3;

	public const int StatApiLatest = 1;

	public const int TimeUndefined = -1;

	public StatsInterface()
	{
	}

	public StatsInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyStatByIndex(ref CopyStatByIndexOptions options, out Stat? outStat)
	{
		CopyStatByIndexOptionsInternal optionsInternal = default(CopyStatByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outStatAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Stats_CopyStatByIndex(base.InnerHandle, ref optionsInternal, ref outStatAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<StatInternal, Stat>(outStatAddress, out outStat);
		if (outStat.HasValue)
		{
			Bindings.EOS_Stats_Stat_Release(outStatAddress);
		}
		return result;
	}

	public Result CopyStatByName(ref CopyStatByNameOptions options, out Stat? outStat)
	{
		CopyStatByNameOptionsInternal optionsInternal = default(CopyStatByNameOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outStatAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Stats_CopyStatByName(base.InnerHandle, ref optionsInternal, ref outStatAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<StatInternal, Stat>(outStatAddress, out outStat);
		if (outStat.HasValue)
		{
			Bindings.EOS_Stats_Stat_Release(outStatAddress);
		}
		return result;
	}

	public uint GetStatsCount(ref GetStatCountOptions options)
	{
		GetStatCountOptionsInternal optionsInternal = default(GetStatCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Stats_GetStatsCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void IngestStat(ref IngestStatOptions options, object clientData, OnIngestStatCompleteCallback completionDelegate)
	{
		IngestStatOptionsInternal optionsInternal = default(IngestStatOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnIngestStatCompleteCallbackInternal completionDelegateInternal = OnIngestStatCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Stats_IngestStat(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryStats(ref QueryStatsOptions options, object clientData, OnQueryStatsCompleteCallback completionDelegate)
	{
		QueryStatsOptionsInternal optionsInternal = default(QueryStatsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryStatsCompleteCallbackInternal completionDelegateInternal = OnQueryStatsCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Stats_QueryStats(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnIngestStatCompleteCallbackInternal))]
	internal static void OnIngestStatCompleteCallbackInternalImplementation(ref IngestStatCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<IngestStatCompleteCallbackInfoInternal, OnIngestStatCompleteCallback, IngestStatCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryStatsCompleteCallbackInternal))]
	internal static void OnQueryStatsCompleteCallbackInternalImplementation(ref OnQueryStatsCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnQueryStatsCompleteCallbackInfoInternal, OnQueryStatsCompleteCallback, OnQueryStatsCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
