using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct EntitlementInternal : IGettable<Entitlement>, ISettable<Entitlement>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_EntitlementName;

	private IntPtr m_EntitlementId;

	private IntPtr m_CatalogItemId;

	private int m_ServerIndex;

	private int m_Redeemed;

	private long m_EndTimestamp;

	public Utf8String EntitlementName
	{
		get
		{
			Helper.Get(m_EntitlementName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_EntitlementName);
		}
	}

	public Utf8String EntitlementId
	{
		get
		{
			Helper.Get(m_EntitlementId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_EntitlementId);
		}
	}

	public Utf8String CatalogItemId
	{
		get
		{
			Helper.Get(m_CatalogItemId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_CatalogItemId);
		}
	}

	public int ServerIndex
	{
		get
		{
			return m_ServerIndex;
		}
		set
		{
			m_ServerIndex = value;
		}
	}

	public bool Redeemed
	{
		get
		{
			Helper.Get(m_Redeemed, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Redeemed);
		}
	}

	public long EndTimestamp
	{
		get
		{
			return m_EndTimestamp;
		}
		set
		{
			m_EndTimestamp = value;
		}
	}

	public void Set(ref Entitlement other)
	{
		m_ApiVersion = 2;
		EntitlementName = other.EntitlementName;
		EntitlementId = other.EntitlementId;
		CatalogItemId = other.CatalogItemId;
		ServerIndex = other.ServerIndex;
		Redeemed = other.Redeemed;
		EndTimestamp = other.EndTimestamp;
	}

	public void Set(ref Entitlement? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			EntitlementName = other.Value.EntitlementName;
			EntitlementId = other.Value.EntitlementId;
			CatalogItemId = other.Value.CatalogItemId;
			ServerIndex = other.Value.ServerIndex;
			Redeemed = other.Value.Redeemed;
			EndTimestamp = other.Value.EndTimestamp;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_EntitlementName);
		Helper.Dispose(ref m_EntitlementId);
		Helper.Dispose(ref m_CatalogItemId);
	}

	public void Get(out Entitlement output)
	{
		output = default(Entitlement);
		output.Set(ref this);
	}
}
