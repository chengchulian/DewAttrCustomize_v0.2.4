using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UserTokenInternal : IGettable<UserToken>, ISettable<UserToken>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_ProductUserId;

	private IntPtr m_Token;

	public ProductUserId ProductUserId
	{
		get
		{
			Helper.Get(m_ProductUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ProductUserId);
		}
	}

	public Utf8String Token
	{
		get
		{
			Helper.Get(m_Token, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Token);
		}
	}

	public void Set(ref UserToken other)
	{
		m_ApiVersion = 1;
		ProductUserId = other.ProductUserId;
		Token = other.Token;
	}

	public void Set(ref UserToken? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			ProductUserId = other.Value.ProductUserId;
			Token = other.Value.Token;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ProductUserId);
		Helper.Dispose(ref m_Token);
	}

	public void Get(out UserToken output)
	{
		output = default(UserToken);
		output.Set(ref this);
	}
}
