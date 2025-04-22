using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RequestPermissionsOptionsInternal : ISettable<RequestPermissionsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private uint m_PermissionKeyCount;

	private IntPtr m_PermissionKeys;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String[] PermissionKeys
	{
		set
		{
			Helper.Set(value, ref m_PermissionKeys, isArrayItemAllocated: true, out m_PermissionKeyCount);
		}
	}

	public void Set(ref RequestPermissionsOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		PermissionKeys = other.PermissionKeys;
	}

	public void Set(ref RequestPermissionsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			PermissionKeys = other.Value.PermissionKeys;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_PermissionKeys);
	}
}
