using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyProductUserExternalAccountByIndexOptionsInternal : ISettable<CopyProductUserExternalAccountByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private uint m_ExternalAccountInfoIndex;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public uint ExternalAccountInfoIndex
	{
		set
		{
			m_ExternalAccountInfoIndex = value;
		}
	}

	public void Set(ref CopyProductUserExternalAccountByIndexOptions other)
	{
		m_ApiVersion = 1;
		TargetUserId = other.TargetUserId;
		ExternalAccountInfoIndex = other.ExternalAccountInfoIndex;
	}

	public void Set(ref CopyProductUserExternalAccountByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			TargetUserId = other.Value.TargetUserId;
			ExternalAccountInfoIndex = other.Value.ExternalAccountInfoIndex;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
	}
}
