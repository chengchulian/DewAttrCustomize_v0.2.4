using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ClientCredentialsInternal : IGettable<ClientCredentials>, ISettable<ClientCredentials>, IDisposable
{
	private IntPtr m_ClientId;

	private IntPtr m_ClientSecret;

	public Utf8String ClientId
	{
		get
		{
			Helper.Get(m_ClientId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientId);
		}
	}

	public Utf8String ClientSecret
	{
		get
		{
			Helper.Get(m_ClientSecret, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientSecret);
		}
	}

	public void Set(ref ClientCredentials other)
	{
		ClientId = other.ClientId;
		ClientSecret = other.ClientSecret;
	}

	public void Set(ref ClientCredentials? other)
	{
		if (other.HasValue)
		{
			ClientId = other.Value.ClientId;
			ClientSecret = other.Value.ClientSecret;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientId);
		Helper.Dispose(ref m_ClientSecret);
	}

	public void Get(out ClientCredentials output)
	{
		output = default(ClientCredentials);
		output.Set(ref this);
	}
}
