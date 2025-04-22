using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionModificationSetPermissionLevelOptionsInternal : ISettable<SessionModificationSetPermissionLevelOptions>, IDisposable
{
	private int m_ApiVersion;

	private OnlineSessionPermissionLevel m_PermissionLevel;

	public OnlineSessionPermissionLevel PermissionLevel
	{
		set
		{
			m_PermissionLevel = value;
		}
	}

	public void Set(ref SessionModificationSetPermissionLevelOptions other)
	{
		m_ApiVersion = 1;
		PermissionLevel = other.PermissionLevel;
	}

	public void Set(ref SessionModificationSetPermissionLevelOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PermissionLevel = other.Value.PermissionLevel;
		}
	}

	public void Dispose()
	{
	}
}
