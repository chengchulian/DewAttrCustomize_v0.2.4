using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryJoinRoomTokenOptionsInternal : ISettable<QueryJoinRoomTokenOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

	private IntPtr m_TargetUserIds;

	private uint m_TargetUserIdsCount;

	private IntPtr m_TargetUserIpAddresses;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String RoomName
	{
		set
		{
			Helper.Set(value, ref m_RoomName);
		}
	}

	public ProductUserId[] TargetUserIds
	{
		set
		{
			Helper.Set(value, ref m_TargetUserIds, out m_TargetUserIdsCount);
		}
	}

	public Utf8String TargetUserIpAddresses
	{
		set
		{
			Helper.Set(value, ref m_TargetUserIpAddresses);
		}
	}

	public void Set(ref QueryJoinRoomTokenOptions other)
	{
		m_ApiVersion = 2;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		TargetUserIds = other.TargetUserIds;
		TargetUserIpAddresses = other.TargetUserIpAddresses;
	}

	public void Set(ref QueryJoinRoomTokenOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
			TargetUserIds = other.Value.TargetUserIds;
			TargetUserIpAddresses = other.Value.TargetUserIpAddresses;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
		Helper.Dispose(ref m_TargetUserIds);
		Helper.Dispose(ref m_TargetUserIpAddresses);
	}
}
