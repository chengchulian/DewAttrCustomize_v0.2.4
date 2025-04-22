using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnFriendsUpdateInfoInternal : ICallbackInfoInternal, IGettable<OnFriendsUpdateInfo>, ISettable<OnFriendsUpdateInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetUserId;

	private FriendsStatus m_PreviousStatus;

	private FriendsStatus m_CurrentStatus;

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

	public FriendsStatus PreviousStatus
	{
		get
		{
			return m_PreviousStatus;
		}
		set
		{
			m_PreviousStatus = value;
		}
	}

	public FriendsStatus CurrentStatus
	{
		get
		{
			return m_CurrentStatus;
		}
		set
		{
			m_CurrentStatus = value;
		}
	}

	public void Set(ref OnFriendsUpdateInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		PreviousStatus = other.PreviousStatus;
		CurrentStatus = other.CurrentStatus;
	}

	public void Set(ref OnFriendsUpdateInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			TargetUserId = other.Value.TargetUserId;
			PreviousStatus = other.Value.PreviousStatus;
			CurrentStatus = other.Value.CurrentStatus;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetUserId);
	}

	public void Get(out OnFriendsUpdateInfo output)
	{
		output = default(OnFriendsUpdateInfo);
		output.Set(ref this);
	}
}
