using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ParticipantUpdatedCallbackInfoInternal : ICallbackInfoInternal, IGettable<ParticipantUpdatedCallbackInfo>, ISettable<ParticipantUpdatedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

	private IntPtr m_ParticipantId;

	private int m_Speaking;

	private RTCAudioStatus m_AudioStatus;

	public object ClientData
	{
		get
		{
			Helper.Get(m_ClientData, out object value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientData);
		}
	}

	public IntPtr ClientDataAddress => m_ClientData;

	public ProductUserId LocalUserId
	{
		get
		{
			Helper.Get(m_LocalUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String RoomName
	{
		get
		{
			Helper.Get(m_RoomName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_RoomName);
		}
	}

	public ProductUserId ParticipantId
	{
		get
		{
			Helper.Get(m_ParticipantId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ParticipantId);
		}
	}

	public bool Speaking
	{
		get
		{
			Helper.Get(m_Speaking, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Speaking);
		}
	}

	public RTCAudioStatus AudioStatus
	{
		get
		{
			return m_AudioStatus;
		}
		set
		{
			m_AudioStatus = value;
		}
	}

	public void Set(ref ParticipantUpdatedCallbackInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		ParticipantId = other.ParticipantId;
		Speaking = other.Speaking;
		AudioStatus = other.AudioStatus;
	}

	public void Set(ref ParticipantUpdatedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
			ParticipantId = other.Value.ParticipantId;
			Speaking = other.Value.Speaking;
			AudioStatus = other.Value.AudioStatus;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
		Helper.Dispose(ref m_ParticipantId);
	}

	public void Get(out ParticipantUpdatedCallbackInfo output)
	{
		output = default(ParticipantUpdatedCallbackInfo);
		output.Set(ref this);
	}
}
