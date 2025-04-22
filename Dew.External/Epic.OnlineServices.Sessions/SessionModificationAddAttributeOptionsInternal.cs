using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionModificationAddAttributeOptionsInternal : ISettable<SessionModificationAddAttributeOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_SessionAttribute;

	private SessionAttributeAdvertisementType m_AdvertisementType;

	public AttributeData? SessionAttribute
	{
		set
		{
			Helper.Set<AttributeData, AttributeDataInternal>(ref value, ref m_SessionAttribute);
		}
	}

	public SessionAttributeAdvertisementType AdvertisementType
	{
		set
		{
			m_AdvertisementType = value;
		}
	}

	public void Set(ref SessionModificationAddAttributeOptions other)
	{
		m_ApiVersion = 2;
		SessionAttribute = other.SessionAttribute;
		AdvertisementType = other.AdvertisementType;
	}

	public void Set(ref SessionModificationAddAttributeOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			SessionAttribute = other.Value.SessionAttribute;
			AdvertisementType = other.Value.AdvertisementType;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SessionAttribute);
	}
}
