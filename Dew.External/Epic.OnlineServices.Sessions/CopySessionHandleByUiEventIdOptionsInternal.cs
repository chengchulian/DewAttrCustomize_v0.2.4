using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopySessionHandleByUiEventIdOptionsInternal : ISettable<CopySessionHandleByUiEventIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private ulong m_UiEventId;

	public ulong UiEventId
	{
		set
		{
			m_UiEventId = value;
		}
	}

	public void Set(ref CopySessionHandleByUiEventIdOptions other)
	{
		m_ApiVersion = 1;
		UiEventId = other.UiEventId;
	}

	public void Set(ref CopySessionHandleByUiEventIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			UiEventId = other.Value.UiEventId;
		}
	}

	public void Dispose()
	{
	}
}
