using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ShowFriendsOptionsInternal : ISettable<ShowFriendsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public void Set(ref ShowFriendsOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref ShowFriendsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
	}
}
