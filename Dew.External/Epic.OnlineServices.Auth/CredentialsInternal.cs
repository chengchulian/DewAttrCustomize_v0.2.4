using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CredentialsInternal : IGettable<Credentials>, ISettable<Credentials>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Id;

	private IntPtr m_Token;

	private LoginCredentialType m_Type;

	private IntPtr m_SystemAuthCredentialsOptions;

	private ExternalCredentialType m_ExternalType;

	public Utf8String Id
	{
		get
		{
			Helper.Get(m_Id, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Id);
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

	public LoginCredentialType Type
	{
		get
		{
			return m_Type;
		}
		set
		{
			m_Type = value;
		}
	}

	public IntPtr SystemAuthCredentialsOptions
	{
		get
		{
			return m_SystemAuthCredentialsOptions;
		}
		set
		{
			m_SystemAuthCredentialsOptions = value;
		}
	}

	public ExternalCredentialType ExternalType
	{
		get
		{
			return m_ExternalType;
		}
		set
		{
			m_ExternalType = value;
		}
	}

	public void Set(ref Credentials other)
	{
		m_ApiVersion = 4;
		Id = other.Id;
		Token = other.Token;
		Type = other.Type;
		SystemAuthCredentialsOptions = other.SystemAuthCredentialsOptions;
		ExternalType = other.ExternalType;
	}

	public void Set(ref Credentials? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 4;
			Id = other.Value.Id;
			Token = other.Value.Token;
			Type = other.Value.Type;
			SystemAuthCredentialsOptions = other.Value.SystemAuthCredentialsOptions;
			ExternalType = other.Value.ExternalType;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Id);
		Helper.Dispose(ref m_Token);
		Helper.Dispose(ref m_SystemAuthCredentialsOptions);
	}

	public void Get(out Credentials output)
	{
		output = default(Credentials);
		output.Set(ref this);
	}
}
