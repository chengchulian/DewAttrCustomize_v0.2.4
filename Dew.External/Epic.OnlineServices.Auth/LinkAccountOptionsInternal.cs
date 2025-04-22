using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LinkAccountOptionsInternal : ISettable<LinkAccountOptions>, IDisposable
{
	private int m_ApiVersion;

	private LinkAccountFlags m_LinkAccountFlags;

	private IntPtr m_ContinuanceToken;

	private IntPtr m_LocalUserId;

	public LinkAccountFlags LinkAccountFlags
	{
		set
		{
			m_LinkAccountFlags = value;
		}
	}

	public ContinuanceToken ContinuanceToken
	{
		set
		{
			Helper.Set(value, ref m_ContinuanceToken);
		}
	}

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public void Set(ref LinkAccountOptions other)
	{
		m_ApiVersion = 1;
		LinkAccountFlags = other.LinkAccountFlags;
		ContinuanceToken = other.ContinuanceToken;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref LinkAccountOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LinkAccountFlags = other.Value.LinkAccountFlags;
			ContinuanceToken = other.Value.ContinuanceToken;
			LocalUserId = other.Value.LocalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ContinuanceToken);
		Helper.Dispose(ref m_LocalUserId);
	}
}
