using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyDetailsCopyMemberAttributeByIndexOptionsInternal : ISettable<LobbyDetailsCopyMemberAttributeByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private uint m_AttrIndex;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public uint AttrIndex
	{
		set
		{
			m_AttrIndex = value;
		}
	}

	public void Set(ref LobbyDetailsCopyMemberAttributeByIndexOptions other)
	{
		m_ApiVersion = 1;
		TargetUserId = other.TargetUserId;
		AttrIndex = other.AttrIndex;
	}

	public void Set(ref LobbyDetailsCopyMemberAttributeByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			TargetUserId = other.Value.TargetUserId;
			AttrIndex = other.Value.AttrIndex;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
	}
}
