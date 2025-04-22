using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyMemberStatusReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyMemberStatusReceivedCallbackInfo>, ISettable<LobbyMemberStatusReceivedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LobbyId;

	private IntPtr m_TargetUserId;

	private LobbyMemberStatus m_CurrentStatus;

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

	public Utf8String LobbyId
	{
		get
		{
			Helper.Get(m_LobbyId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LobbyId);
		}
	}

	public ProductUserId TargetUserId
	{
		get
		{
			Helper.Get(m_TargetUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public LobbyMemberStatus CurrentStatus
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

	public void Set(ref LobbyMemberStatusReceivedCallbackInfo other)
	{
		ClientData = other.ClientData;
		LobbyId = other.LobbyId;
		TargetUserId = other.TargetUserId;
		CurrentStatus = other.CurrentStatus;
	}

	public void Set(ref LobbyMemberStatusReceivedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LobbyId = other.Value.LobbyId;
			TargetUserId = other.Value.TargetUserId;
			CurrentStatus = other.Value.CurrentStatus;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LobbyId);
		Helper.Dispose(ref m_TargetUserId);
	}

	public void Get(out LobbyMemberStatusReceivedCallbackInfo output)
	{
		output = default(LobbyMemberStatusReceivedCallbackInfo);
		output.Set(ref this);
	}
}
