using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyUserTokenByUserIdOptionsInternal : ISettable<CopyUserTokenByUserIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private uint m_QueryId;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public uint QueryId
	{
		set
		{
			m_QueryId = value;
		}
	}

	public void Set(ref CopyUserTokenByUserIdOptions other)
	{
		m_ApiVersion = 2;
		TargetUserId = other.TargetUserId;
		QueryId = other.QueryId;
	}

	public void Set(ref CopyUserTokenByUserIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			TargetUserId = other.Value.TargetUserId;
			QueryId = other.Value.QueryId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
	}
}
