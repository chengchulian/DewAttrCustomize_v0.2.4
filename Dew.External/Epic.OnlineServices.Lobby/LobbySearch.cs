using System;

namespace Epic.OnlineServices.Lobby;

public sealed class LobbySearch : Handle
{
	public const int LobbysearchCopysearchresultbyindexApiLatest = 1;

	public const int LobbysearchFindApiLatest = 1;

	public const int LobbysearchGetsearchresultcountApiLatest = 1;

	public const int LobbysearchRemoveparameterApiLatest = 1;

	public const int LobbysearchSetlobbyidApiLatest = 1;

	public const int LobbysearchSetmaxresultsApiLatest = 1;

	public const int LobbysearchSetparameterApiLatest = 1;

	public const int LobbysearchSettargetuseridApiLatest = 1;

	public LobbySearch()
	{
	}

	public LobbySearch(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopySearchResultByIndex(ref LobbySearchCopySearchResultByIndexOptions options, out LobbyDetails outLobbyDetailsHandle)
	{
		LobbySearchCopySearchResultByIndexOptionsInternal optionsInternal = default(LobbySearchCopySearchResultByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLobbyDetailsHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_LobbySearch_CopySearchResultByIndex(base.InnerHandle, ref optionsInternal, ref outLobbyDetailsHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outLobbyDetailsHandleAddress, out outLobbyDetailsHandle);
		return result;
	}

	public void Find(ref LobbySearchFindOptions options, object clientData, LobbySearchOnFindCallback completionDelegate)
	{
		LobbySearchFindOptionsInternal optionsInternal = default(LobbySearchFindOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		LobbySearchOnFindCallbackInternal completionDelegateInternal = OnFindCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_LobbySearch_Find(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public uint GetSearchResultCount(ref LobbySearchGetSearchResultCountOptions options)
	{
		LobbySearchGetSearchResultCountOptionsInternal optionsInternal = default(LobbySearchGetSearchResultCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_LobbySearch_GetSearchResultCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_LobbySearch_Release(base.InnerHandle);
	}

	public Result RemoveParameter(ref LobbySearchRemoveParameterOptions options)
	{
		LobbySearchRemoveParameterOptionsInternal optionsInternal = default(LobbySearchRemoveParameterOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbySearch_RemoveParameter(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetLobbyId(ref LobbySearchSetLobbyIdOptions options)
	{
		LobbySearchSetLobbyIdOptionsInternal optionsInternal = default(LobbySearchSetLobbyIdOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbySearch_SetLobbyId(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetMaxResults(ref LobbySearchSetMaxResultsOptions options)
	{
		LobbySearchSetMaxResultsOptionsInternal optionsInternal = default(LobbySearchSetMaxResultsOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbySearch_SetMaxResults(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetParameter(ref LobbySearchSetParameterOptions options)
	{
		LobbySearchSetParameterOptionsInternal optionsInternal = default(LobbySearchSetParameterOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbySearch_SetParameter(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetTargetUserId(ref LobbySearchSetTargetUserIdOptions options)
	{
		LobbySearchSetTargetUserIdOptionsInternal optionsInternal = default(LobbySearchSetTargetUserIdOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_LobbySearch_SetTargetUserId(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	[MonoPInvokeCallback(typeof(LobbySearchOnFindCallbackInternal))]
	internal static void OnFindCallbackInternalImplementation(ref LobbySearchFindCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<LobbySearchFindCallbackInfoInternal, LobbySearchOnFindCallback, LobbySearchFindCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
