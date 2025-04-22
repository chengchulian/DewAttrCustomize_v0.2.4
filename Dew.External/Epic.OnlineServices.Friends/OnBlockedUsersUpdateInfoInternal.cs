using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnBlockedUsersUpdateInfoInternal : ICallbackInfoInternal, IGettable<OnBlockedUsersUpdateInfo>, ISettable<OnBlockedUsersUpdateInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetUserId;

	private int m_Blocked;

	public object ClientData
	{
		get
		{
			Helper.Get(m_ClientData, out object value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientData);
		}
	}

	public IntPtr ClientDataAddress => m_ClientData;

	public EpicAccountId LocalUserId
	{
		get
		{
			Helper.Get(m_LocalUserId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public EpicAccountId TargetUserId
	{
		get
		{
			Helper.Get(m_TargetUserId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public bool Blocked
	{
		get
		{
			Helper.Get(m_Blocked, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Blocked);
		}
	}

	public void Set(ref OnBlockedUsersUpdateInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		Blocked = other.Blocked;
	}

	public void Set(ref OnBlockedUsersUpdateInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			TargetUserId = other.Value.TargetUserId;
			Blocked = other.Value.Blocked;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetUserId);
	}

	public void Get(out OnBlockedUsersUpdateInfo output)
	{
		output = default(OnBlockedUsersUpdateInfo);
		output.Set(ref this);
	}
}
