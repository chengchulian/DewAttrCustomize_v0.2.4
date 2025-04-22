using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetCustomInviteOptionsInternal : ISettable<SetCustomInviteOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_Payload;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String Payload
	{
		set
		{
			Helper.Set(value, ref m_Payload);
		}
	}

	public void Set(ref SetCustomInviteOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		Payload = other.Payload;
	}

	public void Set(ref SetCustomInviteOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			Payload = other.Value.Payload;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_Payload);
	}
}
