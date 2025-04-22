using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogPlayerUseAbilityOptionsInternal : ISettable<LogPlayerUseAbilityOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PlayerHandle;

	private uint m_AbilityId;

	private uint m_AbilityDurationMs;

	private uint m_AbilityCooldownMs;

	public IntPtr PlayerHandle
	{
		set
		{
			m_PlayerHandle = value;
		}
	}

	public uint AbilityId
	{
		set
		{
			m_AbilityId = value;
		}
	}

	public uint AbilityDurationMs
	{
		set
		{
			m_AbilityDurationMs = value;
		}
	}

	public uint AbilityCooldownMs
	{
		set
		{
			m_AbilityCooldownMs = value;
		}
	}

	public void Set(ref LogPlayerUseAbilityOptions other)
	{
		m_ApiVersion = 1;
		PlayerHandle = other.PlayerHandle;
		AbilityId = other.AbilityId;
		AbilityDurationMs = other.AbilityDurationMs;
		AbilityCooldownMs = other.AbilityCooldownMs;
	}

	public void Set(ref LogPlayerUseAbilityOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PlayerHandle = other.Value.PlayerHandle;
			AbilityId = other.Value.AbilityId;
			AbilityDurationMs = other.Value.AbilityDurationMs;
			AbilityCooldownMs = other.Value.AbilityCooldownMs;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PlayerHandle);
	}
}
