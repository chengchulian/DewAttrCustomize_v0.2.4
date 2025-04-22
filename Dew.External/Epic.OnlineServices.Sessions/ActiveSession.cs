using System;

namespace Epic.OnlineServices.Sessions;

public sealed class ActiveSession : Handle
{
	public const int ActivesessionCopyinfoApiLatest = 1;

	public const int ActivesessionGetregisteredplayerbyindexApiLatest = 1;

	public const int ActivesessionGetregisteredplayercountApiLatest = 1;

	public const int ActivesessionInfoApiLatest = 1;

	public ActiveSession()
	{
	}

	public ActiveSession(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyInfo(ref ActiveSessionCopyInfoOptions options, out ActiveSessionInfo? outActiveSessionInfo)
	{
		ActiveSessionCopyInfoOptionsInternal optionsInternal = default(ActiveSessionCopyInfoOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outActiveSessionInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_ActiveSession_CopyInfo(base.InnerHandle, ref optionsInternal, ref outActiveSessionInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<ActiveSessionInfoInternal, ActiveSessionInfo>(outActiveSessionInfoAddress, out outActiveSessionInfo);
		if (outActiveSessionInfo.HasValue)
		{
			Bindings.EOS_ActiveSession_Info_Release(outActiveSessionInfoAddress);
		}
		return result;
	}

	public ProductUserId GetRegisteredPlayerByIndex(ref ActiveSessionGetRegisteredPlayerByIndexOptions options)
	{
		ActiveSessionGetRegisteredPlayerByIndexOptionsInternal optionsInternal = default(ActiveSessionGetRegisteredPlayerByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr from = Bindings.EOS_ActiveSession_GetRegisteredPlayerByIndex(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out ProductUserId funcResultReturn);
		return funcResultReturn;
	}

	public uint GetRegisteredPlayerCount(ref ActiveSessionGetRegisteredPlayerCountOptions options)
	{
		ActiveSessionGetRegisteredPlayerCountOptionsInternal optionsInternal = default(ActiveSessionGetRegisteredPlayerCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_ActiveSession_GetRegisteredPlayerCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_ActiveSession_Release(base.InnerHandle);
	}
}
