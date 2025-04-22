using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetPlayerSanctionCountOptionsInternal : ISettable<GetPlayerSanctionCountOptions>, IDisposable
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

	public void Set(ref GetPlayerSanctionCountOptions other)
	{
		m_ApiVersion = 1;
		TargetUserId = other.TargetUserId;
	}

	public void Set(ref GetPlayerSanctionCountOptions? other)
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
