using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UpdateSessionOptionsInternal : ISettable<UpdateSessionOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_SessionModificationHandle;

	public SessionModification SessionModificationHandle
	{
		set
		{
			Helper.Set(value, ref m_SessionModificationHandle);
		}
	}

	public void Set(ref UpdateSessionOptions other)
	{
		m_ApiVersion = 1;
		SessionModificationHandle = other.SessionModificationHandle;
	}

	public void Set(ref UpdateSessionOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			SessionModificationHandle = other.Value.SessionModificationHandle;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SessionModificationHandle);
	}
}
