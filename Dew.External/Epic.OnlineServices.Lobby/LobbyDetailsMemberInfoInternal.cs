using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyDetailsMemberInfoInternal : IGettable<LobbyDetailsMemberInfo>, ISettable<LobbyDetailsMemberInfo>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserId;

	private uint m_Platform;

	private int m_AllowsCrossplay;

	public ProductUserId UserId
	{
		get
		{
			Helper.Get(m_UserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

	public uint Platform
	{
		get
		{
			return m_Platform;
		}
		set
		{
			m_Platform = value;
		}
	}

	public bool AllowsCrossplay
	{
		get
		{
			Helper.Get(m_AllowsCrossplay, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AllowsCrossplay);
		}
	}

	public void Set(ref LobbyDetailsMemberInfo other)
	{
		m_ApiVersion = 1;
		UserId = other.UserId;
		Platform = other.Platform;
		AllowsCrossplay = other.AllowsCrossplay;
	}

	public void Set(ref LobbyDetailsMemberInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			UserId = other.Value.UserId;
			Platform = other.Value.Platform;
			AllowsCrossplay = other.Value.AllowsCrossplay;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
	}

	public void Get(out LobbyDetailsMemberInfo output)
	{
		output = default(LobbyDetailsMemberInfo);
		output.Set(ref this);
	}
}
