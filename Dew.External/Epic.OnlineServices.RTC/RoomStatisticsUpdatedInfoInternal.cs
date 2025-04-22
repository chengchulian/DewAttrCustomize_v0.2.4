using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RoomStatisticsUpdatedInfoInternal : ICallbackInfoInternal, IGettable<RoomStatisticsUpdatedInfo>, ISettable<RoomStatisticsUpdatedInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

	private IntPtr m_Statistic;

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

	public ProductUserId LocalUserId
	{
		get
		{
			Helper.Get(m_LocalUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String RoomName
	{
		get
		{
			Helper.Get(m_RoomName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_RoomName);
		}
	}

	public Utf8String Statistic
	{
		get
		{
			Helper.Get(m_Statistic, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Statistic);
		}
	}

	public void Set(ref RoomStatisticsUpdatedInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		Statistic = other.Statistic;
	}

	public void Set(ref RoomStatisticsUpdatedInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
			Statistic = other.Value.Statistic;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
		Helper.Dispose(ref m_Statistic);
	}

	public void Get(out RoomStatisticsUpdatedInfo output)
	{
		output = default(RoomStatisticsUpdatedInfo);
		output.Set(ref this);
	}
}
