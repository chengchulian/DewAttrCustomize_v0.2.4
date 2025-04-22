using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Reports;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SendPlayerBehaviorReportOptionsInternal : ISettable<SendPlayerBehaviorReportOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_ReporterUserId;

	private IntPtr m_ReportedUserId;

	private PlayerReportsCategory m_Category;

	private IntPtr m_Message;

	private IntPtr m_Context;

	public ProductUserId ReporterUserId
	{
		set
		{
			Helper.Set(value, ref m_ReporterUserId);
		}
	}

	public ProductUserId ReportedUserId
	{
		set
		{
			Helper.Set(value, ref m_ReportedUserId);
		}
	}

	public PlayerReportsCategory Category
	{
		set
		{
			m_Category = value;
		}
	}

	public Utf8String Message
	{
		set
		{
			Helper.Set(value, ref m_Message);
		}
	}

	public Utf8String Context
	{
		set
		{
			Helper.Set(value, ref m_Context);
		}
	}

	public void Set(ref SendPlayerBehaviorReportOptions other)
	{
		m_ApiVersion = 2;
		ReporterUserId = other.ReporterUserId;
		ReportedUserId = other.ReportedUserId;
		Category = other.Category;
		Message = other.Message;
		Context = other.Context;
	}

	public void Set(ref SendPlayerBehaviorReportOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			ReporterUserId = other.Value.ReporterUserId;
			ReportedUserId = other.Value.ReportedUserId;
			Category = other.Value.Category;
			Message = other.Value.Message;
			Context = other.Value.Context;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ReporterUserId);
		Helper.Dispose(ref m_ReportedUserId);
		Helper.Dispose(ref m_Message);
		Helper.Dispose(ref m_Context);
	}
}
