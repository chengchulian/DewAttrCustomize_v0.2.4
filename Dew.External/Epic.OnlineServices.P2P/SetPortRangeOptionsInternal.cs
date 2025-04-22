using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetPortRangeOptionsInternal : ISettable<SetPortRangeOptions>, IDisposable
{
	private int m_ApiVersion;

	private ushort m_Port;

	private ushort m_MaxAdditionalPortsToTry;

	public ushort Port
	{
		set
		{
			m_Port = value;
		}
	}

	public ushort MaxAdditionalPortsToTry
	{
		set
		{
			m_MaxAdditionalPortsToTry = value;
		}
	}

	public void Set(ref SetPortRangeOptions other)
	{
		m_ApiVersion = 1;
		Port = other.Port;
		MaxAdditionalPortsToTry = other.MaxAdditionalPortsToTry;
	}

	public void Set(ref SetPortRangeOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Port = other.Value.Port;
			MaxAdditionalPortsToTry = other.Value.MaxAdditionalPortsToTry;
		}
	}

	public void Dispose()
	{
	}
}
