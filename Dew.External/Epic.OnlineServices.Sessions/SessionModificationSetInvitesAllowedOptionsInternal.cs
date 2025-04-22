using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionModificationSetInvitesAllowedOptionsInternal : ISettable<SessionModificationSetInvitesAllowedOptions>, IDisposable
{
	private int m_ApiVersion;

	private int m_InvitesAllowed;

	public bool InvitesAllowed
	{
		set
		{
			Helper.Set(value, ref m_InvitesAllowed);
		}
	}

	public void Set(ref SessionModificationSetInvitesAllowedOptions other)
	{
		m_ApiVersion = 1;
		InvitesAllowed = other.InvitesAllowed;
	}

	public void Set(ref SessionModificationSetInvitesAllowedOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			InvitesAllowed = other.Value.InvitesAllowed;
		}
	}

	public void Dispose()
	{
	}
}
