using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct BestDisplayNameInternal : IGettable<BestDisplayName>, ISettable<BestDisplayName>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserId;

	private IntPtr m_DisplayName;

	private IntPtr m_DisplayNameSanitized;

	private IntPtr m_Nickname;

	private uint m_PlatformType;

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

	public uint PlatformType
	{
		get
		{
			return m_PlatformType;
		}
		set
		{
			m_PlatformType = value;
		}
	}

	public void Set(ref BestDisplayName other)
	{
		m_ApiVersion = 1;
		UserId = other.UserId;
		DisplayName = other.DisplayName;
		DisplayNameSanitized = other.DisplayNameSanitized;
		Nickname = other.Nickname;
		PlatformType = other.PlatformType;
	}

	public void Set(ref BestDisplayName? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			UserId = other.Value.UserId;
			DisplayName = other.Value.DisplayName;
			DisplayNameSanitized = other.Value.DisplayNameSanitized;
			Nickname = other.Value.Nickname;
			PlatformType = other.Value.PlatformType;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
		Helper.Dispose(ref m_DisplayName);
		Helper.Dispose(ref m_DisplayNameSanitized);
		Helper.Dispose(ref m_Nickname);
	}

	public void Get(out BestDisplayName output)
	{
		output = default(BestDisplayName);
		output.Set(ref this);
	}
}
