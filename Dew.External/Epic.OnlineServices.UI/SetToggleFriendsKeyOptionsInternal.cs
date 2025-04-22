using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetToggleFriendsKeyOptionsInternal : ISettable<SetToggleFriendsKeyOptions>, IDisposable
{
	private int m_ApiVersion;

	private KeyCombination m_KeyCombination;

	public KeyCombination KeyCombination
	{
		set
		{
			m_KeyCombination = value;
		}
	}

	public void Set(ref SetToggleFriendsKeyOptions other)
	{
		m_ApiVersion = 1;
		KeyCombination = other.KeyCombination;
	}

	public void Set(ref SetToggleFriendsKeyOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			KeyCombination = other.Value.KeyCombination;
		}
	}

	public void Dispose()
	{
	}
}
