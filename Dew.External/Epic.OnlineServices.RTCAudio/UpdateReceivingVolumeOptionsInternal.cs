using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UpdateReceivingVolumeOptionsInternal : ISettable<UpdateReceivingVolumeOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

	private float m_Volume;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String RoomName
	{
		set
		{
			Helper.Set(value, ref m_RoomName);
		}
	}

	public float Volume
	{
		set
		{
			m_Volume = value;
		}
	}

	public void Set(ref UpdateReceivingVolumeOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		Volume = other.Volume;
	}

	public void Set(ref UpdateReceivingVolumeOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
			Volume = other.Value.Volume;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
	}
}
