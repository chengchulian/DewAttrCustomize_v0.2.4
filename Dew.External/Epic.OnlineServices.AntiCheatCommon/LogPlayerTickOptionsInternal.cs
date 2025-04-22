using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogPlayerTickOptionsInternal : ISettable<LogPlayerTickOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PlayerHandle;

	private IntPtr m_PlayerPosition;

	private IntPtr m_PlayerViewRotation;

	private int m_IsPlayerViewZoomed;

	private float m_PlayerHealth;

	private AntiCheatCommonPlayerMovementState m_PlayerMovementState;

	public IntPtr PlayerHandle
	{
		set
		{
			m_PlayerHandle = value;
		}
	}

	public Vec3f? PlayerPosition
	{
		set
		{
			Helper.Set<Vec3f, Vec3fInternal>(ref value, ref m_PlayerPosition);
		}
	}

	public Quat? PlayerViewRotation
	{
		set
		{
			Helper.Set<Quat, QuatInternal>(ref value, ref m_PlayerViewRotation);
		}
	}

	public bool IsPlayerViewZoomed
	{
		set
		{
			Helper.Set(value, ref m_IsPlayerViewZoomed);
		}
	}

	public float PlayerHealth
	{
		set
		{
			m_PlayerHealth = value;
		}
	}

	public AntiCheatCommonPlayerMovementState PlayerMovementState
	{
		set
		{
			m_PlayerMovementState = value;
		}
	}

	public void Set(ref LogPlayerTickOptions other)
	{
		m_ApiVersion = 2;
		PlayerHandle = other.PlayerHandle;
		PlayerPosition = other.PlayerPosition;
		PlayerViewRotation = other.PlayerViewRotation;
		IsPlayerViewZoomed = other.IsPlayerViewZoomed;
		PlayerHealth = other.PlayerHealth;
		PlayerMovementState = other.PlayerMovementState;
	}

	public void Set(ref LogPlayerTickOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			PlayerHandle = other.Value.PlayerHandle;
			PlayerPosition = other.Value.PlayerPosition;
			PlayerViewRotation = other.Value.PlayerViewRotation;
			IsPlayerViewZoomed = other.Value.IsPlayerViewZoomed;
			PlayerHealth = other.Value.PlayerHealth;
			PlayerMovementState = other.Value.PlayerMovementState;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PlayerHandle);
		Helper.Dispose(ref m_PlayerPosition);
		Helper.Dispose(ref m_PlayerViewRotation);
	}
}
