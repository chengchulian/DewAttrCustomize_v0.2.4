using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyStatByNameOptionsInternal : ISettable<CopyStatByNameOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private IntPtr m_Name;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public Utf8String Name
	{
		set
		{
			Helper.Set(value, ref m_Name);
		}
	}

	public void Set(ref CopyStatByNameOptions other)
	{
		m_ApiVersion = 1;
		TargetUserId = other.TargetUserId;
		Name = other.Name;
	}

	public void Set(ref CopyStatByNameOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			TargetUserId = other.Value.TargetUserId;
			Name = other.Value.Name;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
		Helper.Dispose(ref m_Name);
	}
}
