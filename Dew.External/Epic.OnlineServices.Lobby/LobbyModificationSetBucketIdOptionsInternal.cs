using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyModificationSetBucketIdOptionsInternal : ISettable<LobbyModificationSetBucketIdOptions>, IDisposable
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

	public void Set(ref LobbyModificationSetBucketIdOptions other)
	{
		m_ApiVersion = 1;
		BucketId = other.BucketId;
	}

	public void Set(ref LobbyModificationSetBucketIdOptions? other)
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
