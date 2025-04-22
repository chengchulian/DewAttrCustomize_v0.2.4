using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ParticipantStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<ParticipantStatusChangedCallbackInfo>, ISettable<ParticipantStatusChangedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

	private IntPtr m_ParticipantId;

	private RTCParticipantStatus m_ParticipantStatus;

	private uint m_ParticipantMetadataCount;

	private IntPtr m_ParticipantMetadata;

	private int m_ParticipantInBlocklist;

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

	public RTCParticipantStatus ParticipantStatus
	{
		get
		{
			return m_ParticipantStatus;
		}
		set
		{
			m_ParticipantStatus = value;
		}
	}

	public ParticipantMetadata[] ParticipantMetadata
	{
		get
		{
			Helper.Get<ParticipantMetadataInternal, ParticipantMetadata>(m_ParticipantMetadata, out var value, m_ParticipantMetadataCount);
			return value;
		}
		set
		{
			Helper.Set<ParticipantMetadata, ParticipantMetadataInternal>(ref value, ref m_ParticipantMetadata, out m_ParticipantMetadataCount);
		}
	}

	public bool ParticipantInBlocklist
	{
		get
		{
			Helper.Get(m_ParticipantInBlocklist, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ParticipantInBlocklist);
		}
	}

	public void Set(ref ParticipantStatusChangedCallbackInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		ParticipantId = other.ParticipantId;
		ParticipantStatus = other.ParticipantStatus;
		ParticipantMetadata = other.ParticipantMetadata;
		ParticipantInBlocklist = other.ParticipantInBlocklist;
	}

	public void Set(ref ParticipantStatusChangedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
			ParticipantId = other.Value.ParticipantId;
			ParticipantStatus = other.Value.ParticipantStatus;
			ParticipantMetadata = other.Value.ParticipantMetadata;
			ParticipantInBlocklist = other.Value.ParticipantInBlocklist;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
		Helper.Dispose(ref m_ParticipantId);
		Helper.Dispose(ref m_ParticipantMetadata);
	}

	public void Get(out ParticipantStatusChangedCallbackInfo output)
	{
		output = default(ParticipantStatusChangedCallbackInfo);
		output.Set(ref this);
	}
}
