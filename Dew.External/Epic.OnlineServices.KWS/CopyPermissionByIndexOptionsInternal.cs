using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyPermissionByIndexOptionsInternal : ISettable<CopyPermissionByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private uint m_Index;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public uint Index
	{
		set
		{
			m_Index = value;
		}
	}

	public void Set(ref CopyPermissionByIndexOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		Index = other.Index;
	}

	public void Set(ref CopyPermissionByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			Index = other.Value.Index;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
	}
}
