using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyAudioBeforeRenderOptionsInternal : ISettable<AddNotifyAudioBeforeRenderOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

	private int m_UnmixedAudio;

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

	public bool UnmixedAudio
	{
		set
		{
			Helper.Set(value, ref m_UnmixedAudio);
		}
	}

	public void Set(ref AddNotifyAudioBeforeRenderOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		UnmixedAudio = other.UnmixedAudio;
	}

	public void Set(ref AddNotifyAudioBeforeRenderOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
			UnmixedAudio = other.Value.UnmixedAudio;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
	}
}
