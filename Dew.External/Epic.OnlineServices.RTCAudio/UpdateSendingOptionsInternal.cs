using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UpdateSendingOptionsInternal : ISettable<UpdateSendingOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

	private RTCAudioStatus m_AudioStatus;

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

	public RTCAudioStatus AudioStatus
	{
		set
		{
			m_AudioStatus = value;
		}
	}

	public void Set(ref UpdateSendingOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		AudioStatus = other.AudioStatus;
	}

	public void Set(ref UpdateSendingOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
			AudioStatus = other.Value.AudioStatus;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
	}
}
