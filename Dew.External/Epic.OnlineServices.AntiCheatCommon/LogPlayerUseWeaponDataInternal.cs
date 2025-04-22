using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogPlayerUseWeaponDataInternal : IGettable<LogPlayerUseWeaponData>, ISettable<LogPlayerUseWeaponData>, IDisposable
{
	private IntPtr m_PlayerHandle;

	private IntPtr m_PlayerPosition;

	private IntPtr m_PlayerViewRotation;

	private int m_IsPlayerViewZoomed;

	private int m_IsMeleeAttack;

	private IntPtr m_WeaponName;

	public IntPtr PlayerHandle
	{
		get
		{
			return m_PlayerHandle;
		}
		set
		{
			m_PlayerHandle = value;
		}
	}

	public Vec3f? PlayerPosition
	{
		get
		{
			Helper.Get<Vec3fInternal, Vec3f>(m_PlayerPosition, out Vec3f? value);
			return value;
		}
		set
		{
			Helper.Set<Vec3f, Vec3fInternal>(ref value, ref m_PlayerPosition);
		}
	}

	public Quat? PlayerViewRotation
	{
		get
		{
			Helper.Get<QuatInternal, Quat>(m_PlayerViewRotation, out Quat? value);
			return value;
		}
		set
		{
			Helper.Set<Quat, QuatInternal>(ref value, ref m_PlayerViewRotation);
		}
	}

	public bool IsPlayerViewZoomed
	{
		get
		{
			Helper.Get(m_IsPlayerViewZoomed, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_IsPlayerViewZoomed);
		}
	}

	public bool IsMeleeAttack
	{
		get
		{
			Helper.Get(m_IsMeleeAttack, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_IsMeleeAttack);
		}
	}

	public Utf8String WeaponName
	{
		get
		{
			Helper.Get(m_WeaponName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_WeaponName);
		}
	}

	public void Set(ref LogPlayerUseWeaponData other)
	{
		PlayerHandle = other.PlayerHandle;
		PlayerPosition = other.PlayerPosition;
		PlayerViewRotation = other.PlayerViewRotation;
		IsPlayerViewZoomed = other.IsPlayerViewZoomed;
		IsMeleeAttack = other.IsMeleeAttack;
		WeaponName = other.WeaponName;
	}

	public void Set(ref LogPlayerUseWeaponData? other)
	{
		if (other.HasValue)
		{
			PlayerHandle = other.Value.PlayerHandle;
			PlayerPosition = other.Value.PlayerPosition;
			PlayerViewRotation = other.Value.PlayerViewRotation;
			IsPlayerViewZoomed = other.Value.IsPlayerViewZoomed;
			IsMeleeAttack = other.Value.IsMeleeAttack;
			WeaponName = other.Value.WeaponName;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PlayerHandle);
		Helper.Dispose(ref m_PlayerPosition);
		Helper.Dispose(ref m_PlayerViewRotation);
		Helper.Dispose(ref m_WeaponName);
	}

	public void Get(out LogPlayerUseWeaponData output)
	{
		output = default(LogPlayerUseWeaponData);
		output.Set(ref this);
	}
}
