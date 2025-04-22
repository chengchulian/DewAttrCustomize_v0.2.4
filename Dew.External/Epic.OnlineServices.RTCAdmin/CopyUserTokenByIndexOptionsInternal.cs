using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyUserTokenByIndexOptionsInternal : ISettable<CopyUserTokenByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_UserTokenIndex;

	private uint m_QueryId;

	public uint UserTokenIndex
	{
		set
		{
			m_UserTokenIndex = value;
		}
	}

	public uint QueryId
	{
		set
		{
			m_QueryId = value;
		}
	}

	public void Set(ref CopyUserTokenByIndexOptions other)
	{
		m_ApiVersion = 2;
		UserTokenIndex = other.UserTokenIndex;
		QueryId = other.QueryId;
	}

	public void Set(ref CopyUserTokenByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			UserTokenIndex = other.Value.UserTokenIndex;
			QueryId = other.Value.QueryId;
		}
	}

	public void Dispose()
	{
	}
}
