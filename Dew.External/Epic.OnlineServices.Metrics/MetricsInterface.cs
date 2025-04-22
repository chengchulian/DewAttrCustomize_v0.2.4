using System;

namespace Epic.OnlineServices.Metrics;

public sealed class MetricsInterface : Handle
{
	public const int BeginplayersessionApiLatest = 1;

	public const int EndplayersessionApiLatest = 1;

	public MetricsInterface()
	{
	}

	public MetricsInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result BeginPlayerSession(ref BeginPlayerSessionOptions options)
	{
		BeginPlayerSessionOptionsInternal optionsInternal = default(BeginPlayerSessionOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_Metrics_BeginPlayerSession(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result EndPlayerSession(ref EndPlayerSessionOptions options)
	{
		EndPlayerSessionOptionsInternal optionsInternal = default(EndPlayerSessionOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_Metrics_EndPlayerSession(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}
}
