using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct IOSCredentialsSystemAuthCredentialsOptionsInternal : IGettable<IOSCredentialsSystemAuthCredentialsOptions>, ISettable<IOSCredentialsSystemAuthCredentialsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PresentationContextProviding;

	public IntPtr PresentationContextProviding
	{
		get
		{
			return m_PresentationContextProviding;
		}
		set
		{
			m_PresentationContextProviding = value;
		}
	}

	public void Set(ref IOSCredentialsSystemAuthCredentialsOptions other)
	{
		m_ApiVersion = 1;
		PresentationContextProviding = other.PresentationContextProviding;
	}

	public void Set(ref IOSCredentialsSystemAuthCredentialsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PresentationContextProviding = other.Value.PresentationContextProviding;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PresentationContextProviding);
	}

	public void Get(out IOSCredentialsSystemAuthCredentialsOptions output)
	{
		output = default(IOSCredentialsSystemAuthCredentialsOptions);
		output.Set(ref this);
	}
}
