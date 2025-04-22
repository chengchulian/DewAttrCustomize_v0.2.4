using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UserInfoDataInternal : IGettable<UserInfoData>, ISettable<UserInfoData>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserId;

	private IntPtr m_Country;

	private IntPtr m_DisplayName;

	private IntPtr m_PreferredLanguage;

	private IntPtr m_Nickname;

	private IntPtr m_DisplayNameSanitized;

	public EpicAccountId UserId
	{
		get
		{
			Helper.Get(m_UserId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

	public Utf8String Country
	{
		get
		{
			Helper.Get(m_Country, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Country);
		}
	}

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

	public Utf8String PreferredLanguage
	{
		get
		{
			Helper.Get(m_PreferredLanguage, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_PreferredLanguage);
		}
	}

	public Utf8String Nickname
	{
		get
		{
			Helper.Get(m_Nickname, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Nickname);
		}
	}

	public Utf8String DisplayNameSanitized
	{
		get
		{
			Helper.Get(m_DisplayNameSanitized, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DisplayNameSanitized);
		}
	}

	public void Set(ref UserInfoData other)
	{
		m_ApiVersion = 3;
		UserId = other.UserId;
		Country = other.Country;
		DisplayName = other.DisplayName;
		PreferredLanguage = other.PreferredLanguage;
		Nickname = other.Nickname;
		DisplayNameSanitized = other.DisplayNameSanitized;
	}

	public void Set(ref UserInfoData? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			UserId = other.Value.UserId;
			Country = other.Value.Country;
			DisplayName = other.Value.DisplayName;
			PreferredLanguage = other.Value.PreferredLanguage;
			Nickname = other.Value.Nickname;
			DisplayNameSanitized = other.Value.DisplayNameSanitized;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
		Helper.Dispose(ref m_Country);
		Helper.Dispose(ref m_DisplayName);
		Helper.Dispose(ref m_PreferredLanguage);
		Helper.Dispose(ref m_Nickname);
		Helper.Dispose(ref m_DisplayNameSanitized);
	}

	public void Get(out UserInfoData output)
	{
		output = default(UserInfoData);
		output.Set(ref this);
	}
}
