using System;

namespace Epic.OnlineServices.AntiCheatCommon;

public struct LogPlayerUseWeaponData
{
	public IntPtr PlayerHandle { get; set; }

	public Vec3f? PlayerPosition { get; set; }

	public Quat? PlayerViewRotation { get; set; }

	public bool IsPlayerViewZoomed { get; set; }

	public bool IsMeleeAttack { get; set; }

	public Utf8String WeaponName { get; set; }

	internal void Set(ref LogPlayerUseWeaponDataInternal other)
	{
		PlayerHandle = other.PlayerHandle;
		PlayerPosition = other.PlayerPosition;
		PlayerViewRotation = other.PlayerViewRotation;
		IsPlayerViewZoomed = other.IsPlayerViewZoomed;
		IsMeleeAttack = other.IsMeleeAttack;
		WeaponName = other.WeaponName;
	}
}
