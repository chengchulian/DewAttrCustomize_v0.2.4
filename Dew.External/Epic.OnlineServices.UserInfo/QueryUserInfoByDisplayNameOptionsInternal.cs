using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryUserInfoByDisplayNameOptionsInternal : ISettable<QueryUserInfoByDisplayNameOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_DisplayName;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String DisplayName
	{
		set
		{
			Helper.Set(value, ref m_DisplayName);
		}
	}

	public void Set(ref QueryUserInfoByDisplayNameOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		DisplayName = other.DisplayName;
	}

	public void Set(ref QueryUserInfoByDisplayNameOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			DisplayName = other.Value.DisplayName;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_DisplayName);
	}
}
