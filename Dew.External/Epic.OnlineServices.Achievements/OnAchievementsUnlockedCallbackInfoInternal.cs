using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnAchievementsUnlockedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnAchievementsUnlockedCallbackInfo>, ISettable<OnAchievementsUnlockedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_UserId;

	private uint m_AchievementsCount;

	private IntPtr m_AchievementIds;

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

	public Utf8String[] AchievementIds
	{
		get
		{
			Helper.Get<Utf8String>(m_AchievementIds, out var value, m_AchievementsCount, isArrayItemAllocated: true);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AchievementIds, isArrayItemAllocated: true, out m_AchievementsCount);
		}
	}

	public void Set(ref OnAchievementsUnlockedCallbackInfo other)
	{
		ClientData = other.ClientData;
		UserId = other.UserId;
		AchievementIds = other.AchievementIds;
	}

	public void Set(ref OnAchievementsUnlockedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			UserId = other.Value.UserId;
			AchievementIds = other.Value.AchievementIds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_UserId);
		Helper.Dispose(ref m_AchievementIds);
	}

	public void Get(out OnAchievementsUnlockedCallbackInfo output)
	{
		output = default(OnAchievementsUnlockedCallbackInfo);
		output.Set(ref this);
	}
}
