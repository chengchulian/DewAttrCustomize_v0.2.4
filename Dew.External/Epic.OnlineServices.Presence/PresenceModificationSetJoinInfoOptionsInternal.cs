using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PresenceModificationSetJoinInfoOptionsInternal : ISettable<PresenceModificationSetJoinInfoOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_JoinInfo;

	public Utf8String JoinInfo
	{
		set
		{
			Helper.Set(value, ref m_JoinInfo);
		}
	}

	public void Set(ref PresenceModificationSetJoinInfoOptions other)
	{
		m_ApiVersion = 1;
		JoinInfo = other.JoinInfo;
	}

	public void Set(ref PresenceModificationSetJoinInfoOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			JoinInfo = other.Value.JoinInfo;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_JoinInfo);
	}
}
