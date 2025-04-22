using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyMemberUpdateReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyMemberUpdateReceivedCallbackInfo>, ISettable<LobbyMemberUpdateReceivedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LobbyId;

	private IntPtr m_TargetUserId;

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

	public void Set(ref LobbyMemberUpdateReceivedCallbackInfo other)
	{
		ClientData = other.ClientData;
		LobbyId = other.LobbyId;
		TargetUserId = other.TargetUserId;
	}

	public void Set(ref LobbyMemberUpdateReceivedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LobbyId = other.Value.LobbyId;
			TargetUserId = other.Value.TargetUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LobbyId);
		Helper.Dispose(ref m_TargetUserId);
	}

	public void Get(out LobbyMemberUpdateReceivedCallbackInfo output)
	{
		output = default(LobbyMemberUpdateReceivedCallbackInfo);
		output.Set(ref this);
	}
}
