using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Logging;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogMessageInternal : IGettable<LogMessage>, ISettable<LogMessage>, IDisposable
{
	private IntPtr m_Category;

	private IntPtr m_Message;

	private LogLevel m_Level;

	public Utf8String Category
	{
		get
		{
			Helper.Get(m_Category, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Category);
		}
	}

	public Utf8String Message
	{
		get
		{
			Helper.Get(m_Message, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Message);
		}
	}

	public LogLevel Level
	{
		get
		{
			return m_Level;
		}
		set
		{
			m_Level = value;
		}
	}

	public void Set(ref LogMessage other)
	{
		Category = other.Category;
		Message = other.Message;
		Level = other.Level;
	}

	public void Set(ref LogMessage? other)
	{
		if (other.HasValue)
		{
			Category = other.Value.Category;
			Message = other.Value.Message;
			Level = other.Value.Level;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Category);
		Helper.Dispose(ref m_Message);
	}

	public void Get(out LogMessage output)
	{
		output = default(LogMessage);
		output.Set(ref this);
	}
}
