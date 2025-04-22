using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PresenceModificationDataRecordIdInternal : IGettable<PresenceModificationDataRecordId>, ISettable<PresenceModificationDataRecordId>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Key;

	public Utf8String Key
	{
		get
		{
			Helper.Get(m_Key, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Key);
		}
	}

	public void Set(ref PresenceModificationDataRecordId other)
	{
		m_ApiVersion = 1;
		Key = other.Key;
	}

	public void Set(ref PresenceModificationDataRecordId? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Key = other.Value.Key;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Key);
	}

	public void Get(out PresenceModificationDataRecordId output)
	{
		output = default(PresenceModificationDataRecordId);
		output.Set(ref this);
	}
}
