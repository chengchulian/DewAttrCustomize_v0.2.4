using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct KickOptionsInternal : ISettable<KickOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_RoomName;

	private IntPtr m_TargetUserId;

	public Utf8String RoomName
	{
		set
		{
			Helper.Set(value, ref m_RoomName);
		}
	}

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public void Set(ref KickOptions other)
	{
		m_ApiVersion = 1;
		RoomName = other.RoomName;
		TargetUserId = other.TargetUserId;
	}

	public void Set(ref KickOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			RoomName = other.Value.RoomName;
			TargetUserId = other.Value.TargetUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_RoomName);
		Helper.Dispose(ref m_TargetUserId);
	}
}
