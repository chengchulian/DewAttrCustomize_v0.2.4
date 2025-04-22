using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyBestDisplayNameWithPlatformOptionsInternal : ISettable<CopyBestDisplayNameWithPlatformOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetUserId;

	private uint m_TargetPlatformType;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public EpicAccountId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public uint TargetPlatformType
	{
		set
		{
			m_TargetPlatformType = value;
		}
	}

	public void Set(ref CopyBestDisplayNameWithPlatformOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		TargetPlatformType = other.TargetPlatformType;
	}

	public void Set(ref CopyBestDisplayNameWithPlatformOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			TargetUserId = other.Value.TargetUserId;
			TargetPlatformType = other.Value.TargetPlatformType;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetUserId);
	}
}
