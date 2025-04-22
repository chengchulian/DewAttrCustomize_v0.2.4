using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetToggleFriendsKeyOptionsInternal : ISettable<GetToggleFriendsKeyOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref GetToggleFriendsKeyOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref GetToggleFriendsKeyOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
		}
	}

	public void Dispose()
	{
	}
}
