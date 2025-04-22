using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyBlockedUsersUpdateOptionsInternal : ISettable<AddNotifyBlockedUsersUpdateOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyBlockedUsersUpdateOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyBlockedUsersUpdateOptions? other)
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
