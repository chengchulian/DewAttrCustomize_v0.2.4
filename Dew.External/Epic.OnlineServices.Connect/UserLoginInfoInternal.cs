using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UserLoginInfoInternal : IGettable<UserLoginInfo>, ISettable<UserLoginInfo>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_DisplayName;

	private IntPtr m_NsaIdToken;

	public Utf8String DisplayName
	{
		get
		{
			Helper.Get(m_DisplayName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DisplayName);
		}
	}

	public Utf8String NsaIdToken
	{
		get
		{
			Helper.Get(m_NsaIdToken, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_NsaIdToken);
		}
	}

	public void Set(ref UserLoginInfo other)
	{
		m_ApiVersion = 2;
		DisplayName = other.DisplayName;
		NsaIdToken = other.NsaIdToken;
	}

	public void Set(ref UserLoginInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			DisplayName = other.Value.DisplayName;
			NsaIdToken = other.Value.NsaIdToken;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_DisplayName);
		Helper.Dispose(ref m_NsaIdToken);
	}

	public void Get(out UserLoginInfo output)
	{
		output = default(UserLoginInfo);
		output.Set(ref this);
	}
}
