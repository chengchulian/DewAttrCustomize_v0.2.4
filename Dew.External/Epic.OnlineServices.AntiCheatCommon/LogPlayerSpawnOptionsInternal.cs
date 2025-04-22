using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogPlayerSpawnOptionsInternal : ISettable<LogPlayerSpawnOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_SpawnedPlayerHandle;

	private uint m_TeamId;

	private uint m_CharacterId;

	public IntPtr SpawnedPlayerHandle
	{
		set
		{
			m_SpawnedPlayerHandle = value;
		}
	}

	public uint TeamId
	{
		set
		{
			m_TeamId = value;
		}
	}

	public uint CharacterId
	{
		set
		{
			m_CharacterId = value;
		}
	}

	public void Set(ref LogPlayerSpawnOptions other)
	{
		m_ApiVersion = 1;
		SpawnedPlayerHandle = other.SpawnedPlayerHandle;
		TeamId = other.TeamId;
		CharacterId = other.CharacterId;
	}

	public void Set(ref LogPlayerSpawnOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			SpawnedPlayerHandle = other.Value.SpawnedPlayerHandle;
			TeamId = other.Value.TeamId;
			CharacterId = other.Value.CharacterId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SpawnedPlayerHandle);
	}
}
