using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetPermissionByKeyOptionsInternal : ISettable<GetPermissionByKeyOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_Key;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String Key
	{
		set
		{
			Helper.Set(value, ref m_Key);
		}
	}

	public void Set(ref GetPermissionByKeyOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		Key = other.Key;
	}

	public void Set(ref GetPermissionByKeyOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			Key = other.Value.Key;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_Key);
	}
}
