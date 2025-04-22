using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetToggleFriendsButtonOptionsInternal : ISettable<SetToggleFriendsButtonOptions>, IDisposable
{
	private int m_ApiVersion;

	private InputStateButtonFlags m_ButtonCombination;

	public InputStateButtonFlags ButtonCombination
	{
		set
		{
			m_ButtonCombination = value;
		}
	}

	public void Set(ref SetToggleFriendsButtonOptions other)
	{
		m_ApiVersion = 1;
		ButtonCombination = other.ButtonCombination;
	}

	public void Set(ref SetToggleFriendsButtonOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			ButtonCombination = other.Value.ButtonCombination;
		}
	}

	public void Dispose()
	{
	}
}
