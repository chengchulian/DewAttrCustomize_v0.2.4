using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct MemoryMonitorCallbackInfoInternal : ICallbackInfoInternal, IGettable<MemoryMonitorCallbackInfo>, ISettable<MemoryMonitorCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_SystemMemoryMonitorReport;

	public object ClientData
	{
		get
		{
			Helper.Get(m_ClientData, out object value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientData);
		}
	}

	public IntPtr ClientDataAddress => m_ClientData;

	public IntPtr SystemMemoryMonitorReport
	{
		get
		{
			return m_SystemMemoryMonitorReport;
		}
		set
		{
			m_SystemMemoryMonitorReport = value;
		}
	}

	public void Set(ref MemoryMonitorCallbackInfo other)
	{
		ClientData = other.ClientData;
		SystemMemoryMonitorReport = other.SystemMemoryMonitorReport;
	}

	public void Set(ref MemoryMonitorCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			SystemMemoryMonitorReport = other.Value.SystemMemoryMonitorReport;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_SystemMemoryMonitorReport);
	}

	public void Get(out MemoryMonitorCallbackInfo output)
	{
		output = default(MemoryMonitorCallbackInfo);
		output.Set(ref this);
	}
}
