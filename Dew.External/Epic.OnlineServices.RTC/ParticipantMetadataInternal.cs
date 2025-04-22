using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ParticipantMetadataInternal : IGettable<ParticipantMetadata>, ISettable<ParticipantMetadata>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Key;

	private IntPtr m_Value;

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

	public Utf8String Value
	{
		get
		{
			Helper.Get(m_Value, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Value);
		}
	}

	public void Set(ref ParticipantMetadata other)
	{
		m_ApiVersion = 1;
		Key = other.Key;
		Value = other.Value;
	}

	public void Set(ref ParticipantMetadata? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Key = other.Value.Key;
			Value = other.Value.Value;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Key);
		Helper.Dispose(ref m_Value);
	}

	public void Get(out ParticipantMetadata output)
	{
		output = default(ParticipantMetadata);
		output.Set(ref this);
	}
}
