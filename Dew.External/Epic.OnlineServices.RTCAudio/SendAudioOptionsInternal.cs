using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SendAudioOptionsInternal : ISettable<SendAudioOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

	private IntPtr m_Buffer;

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

	public AudioBuffer? Buffer
	{
		set
		{
			Helper.Set<AudioBuffer, AudioBufferInternal>(ref value, ref m_Buffer);
		}
	}

	public void Set(ref SendAudioOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		Buffer = other.Buffer;
	}

	public void Set(ref SendAudioOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
			Buffer = other.Value.Buffer;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
		Helper.Dispose(ref m_Buffer);
	}
}
