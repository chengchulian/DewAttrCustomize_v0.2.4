using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct TokenInternal : IGettable<Token>, ISettable<Token>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_App;

	private IntPtr m_ClientId;

	private IntPtr m_AccountId;

	private IntPtr m_AccessToken;

	private double m_ExpiresIn;

	private IntPtr m_ExpiresAt;

	private AuthTokenType m_AuthType;

	private IntPtr m_RefreshToken;

	private double m_RefreshExpiresIn;

	private IntPtr m_RefreshExpiresAt;

	public Utf8String App
	{
		get
		{
			Helper.Get(m_App, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_App);
		}
	}

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

	public EpicAccountId AccountId
	{
		get
		{
			Helper.Get(m_AccountId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AccountId);
		}
	}

	public Utf8String AccessToken
	{
		get
		{
			Helper.Get(m_AccessToken, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AccessToken);
		}
	}

	public double ExpiresIn
	{
		get
		{
			return m_ExpiresIn;
		}
		set
		{
			m_ExpiresIn = value;
		}
	}

	public Utf8String ExpiresAt
	{
		get
		{
			Helper.Get(m_ExpiresAt, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ExpiresAt);
		}
	}

	public AuthTokenType AuthType
	{
		get
		{
			return m_AuthType;
		}
		set
		{
			m_AuthType = value;
		}
	}

	public Utf8String RefreshToken
	{
		get
		{
			Helper.Get(m_RefreshToken, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_RefreshToken);
		}
	}

	public double RefreshExpiresIn
	{
		get
		{
			return m_RefreshExpiresIn;
		}
		set
		{
			m_RefreshExpiresIn = value;
		}
	}

	public Utf8String RefreshExpiresAt
	{
		get
		{
			Helper.Get(m_RefreshExpiresAt, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_RefreshExpiresAt);
		}
	}

	public void Set(ref Token other)
	{
		m_ApiVersion = 2;
		App = other.App;
		ClientId = other.ClientId;
		AccountId = other.AccountId;
		AccessToken = other.AccessToken;
		ExpiresIn = other.ExpiresIn;
		ExpiresAt = other.ExpiresAt;
		AuthType = other.AuthType;
		RefreshToken = other.RefreshToken;
		RefreshExpiresIn = other.RefreshExpiresIn;
		RefreshExpiresAt = other.RefreshExpiresAt;
	}

	public void Set(ref Token? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			App = other.Value.App;
			ClientId = other.Value.ClientId;
			AccountId = other.Value.AccountId;
			AccessToken = other.Value.AccessToken;
			ExpiresIn = other.Value.ExpiresIn;
			ExpiresAt = other.Value.ExpiresAt;
			AuthType = other.Value.AuthType;
			RefreshToken = other.Value.RefreshToken;
			RefreshExpiresIn = other.Value.RefreshExpiresIn;
			RefreshExpiresAt = other.Value.RefreshExpiresAt;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_App);
		Helper.Dispose(ref m_ClientId);
		Helper.Dispose(ref m_AccountId);
		Helper.Dispose(ref m_AccessToken);
		Helper.Dispose(ref m_ExpiresAt);
		Helper.Dispose(ref m_RefreshToken);
		Helper.Dispose(ref m_RefreshExpiresAt);
	}

	public void Get(out Token output)
	{
		output = default(Token);
		output.Set(ref this);
	}
}
