using System;

namespace Epic.OnlineServices.Reports;

public sealed class ReportsInterface : Handle
{
	public const int ReportcontextMaxLength = 4096;

	public const int ReportmessageMaxLength = 512;

	public const int SendplayerbehaviorreportApiLatest = 2;

	public ReportsInterface()
	{
	}

	public ReportsInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public void SendPlayerBehaviorReport(ref SendPlayerBehaviorReportOptions options, object clientData, OnSendPlayerBehaviorReportCompleteCallback completionDelegate)
	{
		SendPlayerBehaviorReportOptionsInternal optionsInternal = default(SendPlayerBehaviorReportOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSendPlayerBehaviorReportCompleteCallbackInternal completionDelegateInternal = OnSendPlayerBehaviorReportCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Reports_SendPlayerBehaviorReport(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnSendPlayerBehaviorReportCompleteCallbackInternal))]
	internal static void OnSendPlayerBehaviorReportCompleteCallbackInternalImplementation(ref SendPlayerBehaviorReportCompleteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<SendPlayerBehaviorReportCompleteCallbackInfoInternal, OnSendPlayerBehaviorReportCompleteCallback, SendPlayerBehaviorReportCompleteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
