using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyDetailsCopyMemberAttributeByKeyOptionsInternal : ISettable<LobbyDetailsCopyMemberAttributeByKeyOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private IntPtr m_AttrKey;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public Utf8String AttrKey
	{
		set
		{
			Helper.Set(value, ref m_AttrKey);
		}
	}

	public void Set(ref LobbyDetailsCopyMemberAttributeByKeyOptions other)
	{
		m_ApiVersion = 1;
		TargetUserId = other.TargetUserId;
		AttrKey = other.AttrKey;
	}

	public void Set(ref LobbyDetailsCopyMemberAttributeByKeyOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			TargetUserId = other.Value.TargetUserId;
			AttrKey = other.Value.AttrKey;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
		Helper.Dispose(ref m_AttrKey);
	}
}
