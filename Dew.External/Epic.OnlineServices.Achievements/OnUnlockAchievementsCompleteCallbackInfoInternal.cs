using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnUnlockAchievementsCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnUnlockAchievementsCompleteCallbackInfo>, ISettable<OnUnlockAchievementsCompleteCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_UserId;

	private uint m_AchievementsCount;

	public Result ResultCode
	{
		get
		{
			return m_ResultCode;
		}
		set
		{
			m_ResultCode = value;
		}
	}

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

	public ProductUserId UserId
	{
		get
		{
			Helper.Get(m_UserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

	public uint AchievementsCount
	{
		get
		{
			return m_AchievementsCount;
		}
		set
		{
			m_AchievementsCount = value;
		}
	}

	public void Set(ref OnUnlockAchievementsCompleteCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		UserId = other.UserId;
		AchievementsCount = other.AchievementsCount;
	}

	public void Set(ref OnUnlockAchievementsCompleteCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			UserId = other.Value.UserId;
			AchievementsCount = other.Value.AchievementsCount;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_UserId);
	}

	public void Get(out OnUnlockAchievementsCompleteCallbackInfo output)
	{
		output = default(OnUnlockAchievementsCompleteCallbackInfo);
		output.Set(ref this);
	}
}
