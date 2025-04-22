using System;

namespace Epic.OnlineServices.Sessions;

public sealed class SessionDetails : Handle
{
	public const int SessiondetailsAttributeApiLatest = 1;

	public const int SessiondetailsCopyinfoApiLatest = 1;

	public const int SessiondetailsCopysessionattributebyindexApiLatest = 1;

	public const int SessiondetailsCopysessionattributebykeyApiLatest = 1;

	public const int SessiondetailsGetsessionattributecountApiLatest = 1;

	public const int SessiondetailsInfoApiLatest = 2;

	public const int SessiondetailsSettingsApiLatest = 4;

	public SessionDetails()
	{
	}

	public SessionDetails(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyInfo(ref SessionDetailsCopyInfoOptions options, out SessionDetailsInfo? outSessionInfo)
	{
		SessionDetailsCopyInfoOptionsInternal optionsInternal = default(SessionDetailsCopyInfoOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSessionInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_SessionDetails_CopyInfo(base.InnerHandle, ref optionsInternal, ref outSessionInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<SessionDetailsInfoInternal, SessionDetailsInfo>(outSessionInfoAddress, out outSessionInfo);
		if (outSessionInfo.HasValue)
		{
			Bindings.EOS_SessionDetails_Info_Release(outSessionInfoAddress);
		}
		return result;
	}

	public Result CopySessionAttributeByIndex(ref SessionDetailsCopySessionAttributeByIndexOptions options, out SessionDetailsAttribute? outSessionAttribute)
	{
		SessionDetailsCopySessionAttributeByIndexOptionsInternal optionsInternal = default(SessionDetailsCopySessionAttributeByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSessionAttributeAddress = IntPtr.Zero;
		Result result = Bindings.EOS_SessionDetails_CopySessionAttributeByIndex(base.InnerHandle, ref optionsInternal, ref outSessionAttributeAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<SessionDetailsAttributeInternal, SessionDetailsAttribute>(outSessionAttributeAddress, out outSessionAttribute);
		if (outSessionAttribute.HasValue)
		{
			Bindings.EOS_SessionDetails_Attribute_Release(outSessionAttributeAddress);
		}
		return result;
	}

	public Result CopySessionAttributeByKey(ref SessionDetailsCopySessionAttributeByKeyOptions options, out SessionDetailsAttribute? outSessionAttribute)
	{
		SessionDetailsCopySessionAttributeByKeyOptionsInternal optionsInternal = default(SessionDetailsCopySessionAttributeByKeyOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSessionAttributeAddress = IntPtr.Zero;
		Result result = Bindings.EOS_SessionDetails_CopySessionAttributeByKey(base.InnerHandle, ref optionsInternal, ref outSessionAttributeAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<SessionDetailsAttributeInternal, SessionDetailsAttribute>(outSessionAttributeAddress, out outSessionAttribute);
		if (outSessionAttribute.HasValue)
		{
			Bindings.EOS_SessionDetails_Attribute_Release(outSessionAttributeAddress);
		}
		return result;
	}

	public uint GetSessionAttributeCount(ref SessionDetailsGetSessionAttributeCountOptions options)
	{
		SessionDetailsGetSessionAttributeCountOptionsInternal optionsInternal = default(SessionDetailsGetSessionAttributeCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_SessionDetails_GetSessionAttributeCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_SessionDetails_Release(base.InnerHandle);
	}
}
