using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyModificationAddMemberAttributeOptionsInternal : ISettable<LobbyModificationAddMemberAttributeOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Attribute;

	private LobbyAttributeVisibility m_Visibility;

	public AttributeData? Attribute
	{
		set
		{
			Helper.Set<AttributeData, AttributeDataInternal>(ref value, ref m_Attribute);
		}
	}

	public LobbyAttributeVisibility Visibility
	{
		set
		{
			m_Visibility = value;
		}
	}

	public void Set(ref LobbyModificationAddMemberAttributeOptions other)
	{
		m_ApiVersion = 2;
		Attribute = other.Attribute;
		Visibility = other.Visibility;
	}

	public void Set(ref LobbyModificationAddMemberAttributeOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			Attribute = other.Value.Attribute;
			Visibility = other.Value.Visibility;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Attribute);
	}
}
