using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionModificationSetBucketIdOptionsInternal : ISettable<SessionModificationSetBucketIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_BucketId;

	public Utf8String BucketId
	{
		set
		{
			Helper.Set(value, ref m_BucketId);
		}
	}

	public void Set(ref SessionModificationSetBucketIdOptions other)
	{
		m_ApiVersion = 1;
		BucketId = other.BucketId;
	}

	public void Set(ref SessionModificationSetBucketIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			BucketId = other.Value.BucketId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_BucketId);
	}
}
