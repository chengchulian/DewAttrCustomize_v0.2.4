using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnQueryPlayerAchievementsCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnQueryPlayerAchievementsCompleteCallbackInfo>, ISettable<OnQueryPlayerAchievementsCompleteCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_UserId;

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

	public void Set(ref OnQueryPlayerAchievementsCompleteCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		UserId = other.UserId;
	}

	public void Set(ref OnQueryPlayerAchievementsCompleteCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			UserId = other.Value.UserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_UserId);
	}

	public void Get(out OnQueryPlayerAchievementsCompleteCallbackInfo output)
	{
		output = default(OnQueryPlayerAchievementsCompleteCallbackInfo);
		output.Set(ref this);
	}
}
