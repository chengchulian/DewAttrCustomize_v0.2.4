using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetParticipantHardMuteOptionsInternal : ISettable<SetParticipantHardMuteOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_RoomName;

	private IntPtr m_TargetUserId;

	private int m_Mute;

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

	public bool Mute
	{
		set
		{
			Helper.Set(value, ref m_Mute);
		}
	}

	public void Set(ref SetParticipantHardMuteOptions other)
	{
		m_ApiVersion = 1;
		RoomName = other.RoomName;
		TargetUserId = other.TargetUserId;
		Mute = other.Mute;
	}

	public void Set(ref SetParticipantHardMuteOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			RoomName = other.Value.RoomName;
			TargetUserId = other.Value.TargetUserId;
			Mute = other.Value.Mute;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_RoomName);
		Helper.Dispose(ref m_TargetUserId);
	}
}
