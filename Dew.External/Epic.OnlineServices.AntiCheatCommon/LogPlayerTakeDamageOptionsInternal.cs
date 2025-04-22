using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogPlayerTakeDamageOptionsInternal : ISettable<LogPlayerTakeDamageOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_VictimPlayerHandle;

	private IntPtr m_VictimPlayerPosition;

	private IntPtr m_VictimPlayerViewRotation;

	private IntPtr m_AttackerPlayerHandle;

	private IntPtr m_AttackerPlayerPosition;

	private IntPtr m_AttackerPlayerViewRotation;

	private int m_IsHitscanAttack;

	private int m_HasLineOfSight;

	private int m_IsCriticalHit;

	private uint m_HitBoneId_DEPRECATED;

	private float m_DamageTaken;

	private float m_HealthRemaining;

	private AntiCheatCommonPlayerTakeDamageSource m_DamageSource;

	private AntiCheatCommonPlayerTakeDamageType m_DamageType;

	private AntiCheatCommonPlayerTakeDamageResult m_DamageResult;

	private IntPtr m_PlayerUseWeaponData;

	private uint m_TimeSincePlayerUseWeaponMs;

	private IntPtr m_DamagePosition;

	public IntPtr VictimPlayerHandle
	{
		set
		{
			m_VictimPlayerHandle = value;
		}
	}

	public Vec3f? VictimPlayerPosition
	{
		set
		{
			Helper.Set<Vec3f, Vec3fInternal>(ref value, ref m_VictimPlayerPosition);
		}
	}

	public Quat? VictimPlayerViewRotation
	{
		set
		{
			Helper.Set<Quat, QuatInternal>(ref value, ref m_VictimPlayerViewRotation);
		}
	}

	public IntPtr AttackerPlayerHandle
	{
		set
		{
			m_AttackerPlayerHandle = value;
		}
	}

	public Vec3f? AttackerPlayerPosition
	{
		set
		{
			Helper.Set<Vec3f, Vec3fInternal>(ref value, ref m_AttackerPlayerPosition);
		}
	}

	public Quat? AttackerPlayerViewRotation
	{
		set
		{
			Helper.Set<Quat, QuatInternal>(ref value, ref m_AttackerPlayerViewRotation);
		}
	}

	public bool IsHitscanAttack
	{
		set
		{
			Helper.Set(value, ref m_IsHitscanAttack);
		}
	}

	public bool HasLineOfSight
	{
		set
		{
			Helper.Set(value, ref m_HasLineOfSight);
		}
	}

	public bool IsCriticalHit
	{
		set
		{
			Helper.Set(value, ref m_IsCriticalHit);
		}
	}

	public uint HitBoneId_DEPRECATED
	{
		set
		{
			m_HitBoneId_DEPRECATED = value;
		}
	}

	public float DamageTaken
	{
		set
		{
			m_DamageTaken = value;
		}
	}

	public float HealthRemaining
	{
		set
		{
			m_HealthRemaining = value;
		}
	}

	public AntiCheatCommonPlayerTakeDamageSource DamageSource
	{
		set
		{
			m_DamageSource = value;
		}
	}

	public AntiCheatCommonPlayerTakeDamageType DamageType
	{
		set
		{
			m_DamageType = value;
		}
	}

	public AntiCheatCommonPlayerTakeDamageResult DamageResult
	{
		set
		{
			m_DamageResult = value;
		}
	}

	public LogPlayerUseWeaponData? PlayerUseWeaponData
	{
		set
		{
			Helper.Set<LogPlayerUseWeaponData, LogPlayerUseWeaponDataInternal>(ref value, ref m_PlayerUseWeaponData);
		}
	}

	public uint TimeSincePlayerUseWeaponMs
	{
		set
		{
			m_TimeSincePlayerUseWeaponMs = value;
		}
	}

	public Vec3f? DamagePosition
	{
		set
		{
			Helper.Set<Vec3f, Vec3fInternal>(ref value, ref m_DamagePosition);
		}
	}

	public void Set(ref LogPlayerTakeDamageOptions other)
	{
		m_ApiVersion = 3;
		VictimPlayerHandle = other.VictimPlayerHandle;
		VictimPlayerPosition = other.VictimPlayerPosition;
		VictimPlayerViewRotation = other.VictimPlayerViewRotation;
		AttackerPlayerHandle = other.AttackerPlayerHandle;
		AttackerPlayerPosition = other.AttackerPlayerPosition;
		AttackerPlayerViewRotation = other.AttackerPlayerViewRotation;
		IsHitscanAttack = other.IsHitscanAttack;
		HasLineOfSight = other.HasLineOfSight;
		IsCriticalHit = other.IsCriticalHit;
		HitBoneId_DEPRECATED = other.HitBoneId_DEPRECATED;
		DamageTaken = other.DamageTaken;
		HealthRemaining = other.HealthRemaining;
		DamageSource = other.DamageSource;
		DamageType = other.DamageType;
		DamageResult = other.DamageResult;
		PlayerUseWeaponData = other.PlayerUseWeaponData;
		TimeSincePlayerUseWeaponMs = other.TimeSincePlayerUseWeaponMs;
		DamagePosition = other.DamagePosition;
	}

	public void Set(ref LogPlayerTakeDamageOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			VictimPlayerHandle = other.Value.VictimPlayerHandle;
			VictimPlayerPosition = other.Value.VictimPlayerPosition;
			VictimPlayerViewRotation = other.Value.VictimPlayerViewRotation;
			AttackerPlayerHandle = other.Value.AttackerPlayerHandle;
			AttackerPlayerPosition = other.Value.AttackerPlayerPosition;
			AttackerPlayerViewRotation = other.Value.AttackerPlayerViewRotation;
			IsHitscanAttack = other.Value.IsHitscanAttack;
			HasLineOfSight = other.Value.HasLineOfSight;
			IsCriticalHit = other.Value.IsCriticalHit;
			HitBoneId_DEPRECATED = other.Value.HitBoneId_DEPRECATED;
			DamageTaken = other.Value.DamageTaken;
			HealthRemaining = other.Value.HealthRemaining;
			DamageSource = other.Value.DamageSource;
			DamageType = other.Value.DamageType;
			DamageResult = other.Value.DamageResult;
			PlayerUseWeaponData = other.Value.PlayerUseWeaponData;
			TimeSincePlayerUseWeaponMs = other.Value.TimeSincePlayerUseWeaponMs;
			DamagePosition = other.Value.DamagePosition;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_VictimPlayerHandle);
		Helper.Dispose(ref m_VictimPlayerPosition);
		Helper.Dispose(ref m_VictimPlayerViewRotation);
		Helper.Dispose(ref m_AttackerPlayerHandle);
		Helper.Dispose(ref m_AttackerPlayerPosition);
		Helper.Dispose(ref m_AttackerPlayerViewRotation);
		Helper.Dispose(ref m_PlayerUseWeaponData);
		Helper.Dispose(ref m_DamagePosition);
	}
}
