using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct HardMuteMemberOptionsInternal : ISettable<HardMuteMemberOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LobbyId;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetUserId;

	private int m_HardMute;

	public Utf8String LobbyId
	{
		set
		{
			Helper.Set(value, ref m_LobbyId);
		}
	}

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public bool HardMute
	{
		set
		{
			Helper.Set(value, ref m_HardMute);
		}
	}

	public void Set(ref HardMuteMemberOptions other)
	{
		m_ApiVersion = 1;
		LobbyId = other.LobbyId;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		HardMute = other.HardMute;
	}

	public void Set(ref HardMuteMemberOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LobbyId = other.Value.LobbyId;
			LocalUserId = other.Value.LocalUserId;
			TargetUserId = other.Value.TargetUserId;
			HardMute = other.Value.HardMute;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LobbyId);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetUserId);
	}
}
