using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyPlayerSanctionByIndexOptionsInternal : ISettable<CopyPlayerSanctionByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private uint m_SanctionIndex;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public uint SanctionIndex
	{
		set
		{
			m_SanctionIndex = value;
		}
	}

	public void Set(ref CopyPlayerSanctionByIndexOptions other)
	{
		m_ApiVersion = 1;
		TargetUserId = other.TargetUserId;
		SanctionIndex = other.SanctionIndex;
	}

	public void Set(ref CopyPlayerSanctionByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			TargetUserId = other.Value.TargetUserId;
			SanctionIndex = other.Value.SanctionIndex;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
	}
}
