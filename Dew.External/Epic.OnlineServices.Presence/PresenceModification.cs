using System;

namespace Epic.OnlineServices.Presence;

public sealed class PresenceModification : Handle
{
	public const int PresencemodificationDatarecordidApiLatest = 1;

	public const int PresencemodificationDeletedataApiLatest = 1;

	public const int PresencemodificationJoininfoMaxLength = 255;

	public const int PresencemodificationSetdataApiLatest = 1;

	public const int PresencemodificationSetjoininfoApiLatest = 1;

	public const int PresencemodificationSetrawrichtextApiLatest = 1;

	public const int PresencemodificationSetstatusApiLatest = 1;

	public PresenceModification()
	{
	}

	public PresenceModification(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result DeleteData(ref PresenceModificationDeleteDataOptions options)
	{
		PresenceModificationDeleteDataOptionsInternal optionsInternal = default(PresenceModificationDeleteDataOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_PresenceModification_DeleteData(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_PresenceModification_Release(base.InnerHandle);
	}

	public Result SetData(ref PresenceModificationSetDataOptions options)
	{
		PresenceModificationSetDataOptionsInternal optionsInternal = default(PresenceModificationSetDataOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_PresenceModification_SetData(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetJoinInfo(ref PresenceModificationSetJoinInfoOptions options)
	{
		PresenceModificationSetJoinInfoOptionsInternal optionsInternal = default(PresenceModificationSetJoinInfoOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_PresenceModification_SetJoinInfo(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetRawRichText(ref PresenceModificationSetRawRichTextOptions options)
	{
		PresenceModificationSetRawRichTextOptionsInternal optionsInternal = default(PresenceModificationSetRawRichTextOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_PresenceModification_SetRawRichText(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetStatus(ref PresenceModificationSetStatusOptions options)
	{
		PresenceModificationSetStatusOptionsInternal optionsInternal = default(PresenceModificationSetStatusOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_PresenceModification_SetStatus(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}
}
