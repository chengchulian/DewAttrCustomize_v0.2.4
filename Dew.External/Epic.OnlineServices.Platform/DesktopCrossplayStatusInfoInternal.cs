using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct DesktopCrossplayStatusInfoInternal : IGettable<DesktopCrossplayStatusInfo>, ISettable<DesktopCrossplayStatusInfo>, IDisposable
{
	private DesktopCrossplayStatus m_Status;

	private int m_ServiceInitResult;

	public DesktopCrossplayStatus Status
	{
		get
		{
			return m_Status;
		}
		set
		{
			m_Status = value;
		}
	}

	public int ServiceInitResult
	{
		get
		{
			return m_ServiceInitResult;
		}
		set
		{
			m_ServiceInitResult = value;
		}
	}

	public void Set(ref DesktopCrossplayStatusInfo other)
	{
		Status = other.Status;
		ServiceInitResult = other.ServiceInitResult;
	}

	public void Set(ref DesktopCrossplayStatusInfo? other)
	{
		if (other.HasValue)
		{
			Status = other.Value.Status;
			ServiceInitResult = other.Value.ServiceInitResult;
		}
	}

	public void Dispose()
	{
	}

	public void Get(out DesktopCrossplayStatusInfo output)
	{
		output = default(DesktopCrossplayStatusInfo);
		output.Set(ref this);
	}
}
