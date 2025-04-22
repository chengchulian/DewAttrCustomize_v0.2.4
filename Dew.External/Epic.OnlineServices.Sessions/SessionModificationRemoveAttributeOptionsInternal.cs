using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionModificationRemoveAttributeOptionsInternal : ISettable<SessionModificationRemoveAttributeOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Key;

	public Utf8String Key
	{
		set
		{
			Helper.Set(value, ref m_Key);
		}
	}

	public void Set(ref SessionModificationRemoveAttributeOptions other)
	{
		m_ApiVersion = 1;
		Key = other.Key;
	}

	public void Set(ref SessionModificationRemoveAttributeOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Key = other.Value.Key;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Key);
	}
}
