using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyProductUserInfoOptionsInternal : ISettable<CopyProductUserInfoOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public void Set(ref CopyProductUserInfoOptions other)
	{
		m_ApiVersion = 1;
		TargetUserId = other.TargetUserId;
	}

	public void Set(ref CopyProductUserInfoOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			TargetUserId = other.Value.TargetUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
	}
}
