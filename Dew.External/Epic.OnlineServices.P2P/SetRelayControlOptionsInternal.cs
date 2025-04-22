using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetRelayControlOptionsInternal : ISettable<SetRelayControlOptions>, IDisposable
{
	private int m_ApiVersion;

	private RelayControl m_RelayControl;

	public RelayControl RelayControl
	{
		set
		{
			m_RelayControl = value;
		}
	}

	public void Set(ref SetRelayControlOptions other)
	{
		m_ApiVersion = 1;
		RelayControl = other.RelayControl;
	}

	public void Set(ref SetRelayControlOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			RelayControl = other.Value.RelayControl;
		}
	}

	public void Dispose()
	{
	}
}
