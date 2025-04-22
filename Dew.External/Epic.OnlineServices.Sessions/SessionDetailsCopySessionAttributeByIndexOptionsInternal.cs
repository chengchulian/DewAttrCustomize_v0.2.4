using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionDetailsCopySessionAttributeByIndexOptionsInternal : ISettable<SessionDetailsCopySessionAttributeByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_AttrIndex;

	public uint AttrIndex
	{
		set
		{
			m_AttrIndex = value;
		}
	}

	public void Set(ref SessionDetailsCopySessionAttributeByIndexOptions other)
	{
		m_ApiVersion = 1;
		AttrIndex = other.AttrIndex;
	}

	public void Set(ref SessionDetailsCopySessionAttributeByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AttrIndex = other.Value.AttrIndex;
		}
	}

	public void Dispose()
	{
	}
}
