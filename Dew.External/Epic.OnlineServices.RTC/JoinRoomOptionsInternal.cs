using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct JoinRoomOptionsInternal : ISettable<JoinRoomOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

	private IntPtr m_ClientBaseUrl;

	private IntPtr m_ParticipantToken;

	private IntPtr m_ParticipantId;

	private JoinRoomFlags m_Flags;

	private int m_ManualAudioInputEnabled;

	private int m_ManualAudioOutputEnabled;

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

	public Utf8String ClientBaseUrl
	{
		set
		{
			Helper.Set(value, ref m_ClientBaseUrl);
		}
	}

	public Utf8String ParticipantToken
	{
		set
		{
			Helper.Set(value, ref m_ParticipantToken);
		}
	}

	public ProductUserId ParticipantId
	{
		set
		{
			Helper.Set(value, ref m_ParticipantId);
		}
	}

	public JoinRoomFlags Flags
	{
		set
		{
			m_Flags = value;
		}
	}

	public bool ManualAudioInputEnabled
	{
		set
		{
			Helper.Set(value, ref m_ManualAudioInputEnabled);
		}
	}

	public bool ManualAudioOutputEnabled
	{
		set
		{
			Helper.Set(value, ref m_ManualAudioOutputEnabled);
		}
	}

	public void Set(ref JoinRoomOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		ClientBaseUrl = other.ClientBaseUrl;
		ParticipantToken = other.ParticipantToken;
		ParticipantId = other.ParticipantId;
		Flags = other.Flags;
		ManualAudioInputEnabled = other.ManualAudioInputEnabled;
		ManualAudioOutputEnabled = other.ManualAudioOutputEnabled;
	}

	public void Set(ref JoinRoomOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
			ClientBaseUrl = other.Value.ClientBaseUrl;
			ParticipantToken = other.Value.ParticipantToken;
			ParticipantId = other.Value.ParticipantId;
			Flags = other.Value.Flags;
			ManualAudioInputEnabled = other.Value.ManualAudioInputEnabled;
			ManualAudioOutputEnabled = other.Value.ManualAudioOutputEnabled;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
		Helper.Dispose(ref m_ClientBaseUrl);
		Helper.Dispose(ref m_ParticipantToken);
		Helper.Dispose(ref m_ParticipantId);
	}
}
