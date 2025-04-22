using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionDetailsAttributeInternal : IGettable<SessionDetailsAttribute>, ISettable<SessionDetailsAttribute>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Data;

	private SessionAttributeAdvertisementType m_AdvertisementType;

	public AttributeData? Data
	{
		get
		{
			Helper.Get<AttributeDataInternal, AttributeData>(m_Data, out AttributeData? value);
			return value;
		}
		set
		{
			Helper.Set<AttributeData, AttributeDataInternal>(ref value, ref m_Data);
		}
	}

	public SessionAttributeAdvertisementType AdvertisementType
	{
		get
		{
			return m_AdvertisementType;
		}
		set
		{
			m_AdvertisementType = value;
		}
	}

	public void Set(ref SessionDetailsAttribute other)
	{
		m_ApiVersion = 1;
		Data = other.Data;
		AdvertisementType = other.AdvertisementType;
	}

	public void Set(ref SessionDetailsAttribute? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Data = other.Value.Data;
			AdvertisementType = other.Value.AdvertisementType;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Data);
	}

	public void Get(out SessionDetailsAttribute output)
	{
		output = default(SessionDetailsAttribute);
		output.Set(ref this);
	}
}
