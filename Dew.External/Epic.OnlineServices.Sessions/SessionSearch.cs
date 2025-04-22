using System;

namespace Epic.OnlineServices.Sessions;

public sealed class SessionSearch : Handle
{
	public const int SessionsearchCopysearchresultbyindexApiLatest = 1;

	public const int SessionsearchFindApiLatest = 2;

	public const int SessionsearchGetsearchresultcountApiLatest = 1;

	public const int SessionsearchRemoveparameterApiLatest = 1;

	public const int SessionsearchSetmaxsearchresultsApiLatest = 1;

	public const int SessionsearchSetparameterApiLatest = 1;

	public const int SessionsearchSetsessionidApiLatest = 1;

	public const int SessionsearchSettargetuseridApiLatest = 1;

	public SessionSearch()
	{
	}

	public SessionSearch(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopySearchResultByIndex(ref SessionSearchCopySearchResultByIndexOptions options, out SessionDetails outSessionHandle)
	{
		SessionSearchCopySearchResultByIndexOptionsInternal optionsInternal = default(SessionSearchCopySearchResultByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSessionHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_SessionSearch_CopySearchResultByIndex(base.InnerHandle, ref optionsInternal, ref outSessionHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outSessionHandleAddress, out outSessionHandle);
		return result;
	}

	public void Find(ref SessionSearchFindOptions options, object clientData, SessionSearchOnFindCallback completionDelegate)
	{
		SessionSearchFindOptionsInternal optionsInternal = default(SessionSearchFindOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		SessionSearchOnFindCallbackInternal completionDelegateInternal = OnFindCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_SessionSearch_Find(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public uint GetSearchResultCount(ref SessionSearchGetSearchResultCountOptions options)
	{
		SessionSearchGetSearchResultCountOptionsInternal optionsInternal = default(SessionSearchGetSearchResultCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_SessionSearch_GetSearchResultCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_SessionSearch_Release(base.InnerHandle);
	}

	public Result RemoveParameter(ref SessionSearchRemoveParameterOptions options)
	{
		SessionSearchRemoveParameterOptionsInternal optionsInternal = default(SessionSearchRemoveParameterOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionSearch_RemoveParameter(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetMaxResults(ref SessionSearchSetMaxResultsOptions options)
	{
		SessionSearchSetMaxResultsOptionsInternal optionsInternal = default(SessionSearchSetMaxResultsOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionSearch_SetMaxResults(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetParameter(ref SessionSearchSetParameterOptions options)
	{
		SessionSearchSetParameterOptionsInternal optionsInternal = default(SessionSearchSetParameterOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionSearch_SetParameter(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetSessionId(ref SessionSearchSetSessionIdOptions options)
	{
		SessionSearchSetSessionIdOptionsInternal optionsInternal = default(SessionSearchSetSessionIdOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionSearch_SetSessionId(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetTargetUserId(ref SessionSearchSetTargetUserIdOptions options)
	{
		SessionSearchSetTargetUserIdOptionsInternal optionsInternal = default(SessionSearchSetTargetUserIdOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_SessionSearch_SetTargetUserId(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	[MonoPInvokeCallback(typeof(SessionSearchOnFindCallbackInternal))]
	internal static void OnFindCallbackInternalImplementation(ref SessionSearchFindCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<SessionSearchFindCallbackInfoInternal, SessionSearchOnFindCallback, SessionSearchFindCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
