using System;

namespace Epic.OnlineServices.ProgressionSnapshot;

public sealed class ProgressionSnapshotInterface : Handle
{
	public const int AddprogressionApiLatest = 1;

	public const int BeginsnapshotApiLatest = 1;

	public const int DeletesnapshotApiLatest = 1;

	public const int EndsnapshotApiLatest = 1;

	public const int InvalidProgressionsnapshotid = 0;

	public const int SubmitsnapshotApiLatest = 1;

	public ProgressionSnapshotInterface()
	{
	}

	public ProgressionSnapshotInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result AddProgression(ref AddProgressionOptions options)
	{
		AddProgressionOptionsInternal optionsInternal = default(AddProgressionOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_ProgressionSnapshot_AddProgression(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result BeginSnapshot(ref BeginSnapshotOptions options, out uint outSnapshotId)
	{
		BeginSnapshotOptionsInternal optionsInternal = default(BeginSnapshotOptionsInternal);
		optionsInternal.Set(ref options);
		outSnapshotId = Helper.GetDefault<uint>();
		Result result = Bindings.EOS_ProgressionSnapshot_BeginSnapshot(base.InnerHandle, ref optionsInternal, ref outSnapshotId);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void DeleteSnapshot(ref DeleteSnapshotOptions options, object clientData, OnDeleteSnapshotCallback completionDelegate)
	{
		DeleteSnapshotOptionsInternal optionsInternal = default(DeleteSnapshotOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnDeleteSnapshotCallbackInternal completionDelegateInternal = OnDeleteSnapshotCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_ProgressionSnapshot_DeleteSnapshot(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result EndSnapshot(ref EndSnapshotOptions options)
	{
		EndSnapshotOptionsInternal optionsInternal = default(EndSnapshotOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_ProgressionSnapshot_EndSnapshot(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void SubmitSnapshot(ref SubmitSnapshotOptions options, object clientData, OnSubmitSnapshotCallback completionDelegate)
	{
		SubmitSnapshotOptionsInternal optionsInternal = default(SubmitSnapshotOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSubmitSnapshotCallbackInternal completionDelegateInternal = OnSubmitSnapshotCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_ProgressionSnapshot_SubmitSnapshot(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnDeleteSnapshotCallbackInternal))]
	internal static void OnDeleteSnapshotCallbackInternalImplementation(ref DeleteSnapshotCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<DeleteSnapshotCallbackInfoInternal, OnDeleteSnapshotCallback, DeleteSnapshotCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSubmitSnapshotCallbackInternal))]
	internal static void OnSubmitSnapshotCallbackInternalImplementation(ref SubmitSnapshotCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<SubmitSnapshotCallbackInfoInternal, OnSubmitSnapshotCallback, SubmitSnapshotCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
