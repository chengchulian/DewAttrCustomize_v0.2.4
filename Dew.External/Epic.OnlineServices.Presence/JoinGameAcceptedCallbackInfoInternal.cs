using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct JoinGameAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinGameAcceptedCallbackInfo>, ISettable<JoinGameAcceptedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_JoinInfo;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetUserId;

	private ulong m_UiEventId;

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

	public Utf8String JoinInfo
	{
		get
		{
			Helper.Get(m_JoinInfo, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_JoinInfo);
		}
	}

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

	public ulong UiEventId
	{
		get
		{
			return m_UiEventId;
		}
		set
		{
			m_UiEventId = value;
		}
	}

	public void Set(ref JoinGameAcceptedCallbackInfo other)
	{
		ClientData = other.ClientData;
		JoinInfo = other.JoinInfo;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		UiEventId = other.UiEventId;
	}

	public void Set(ref JoinGameAcceptedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			JoinInfo = other.Value.JoinInfo;
			LocalUserId = other.Value.LocalUserId;
			TargetUserId = other.Value.TargetUserId;
			UiEventId = other.Value.UiEventId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_JoinInfo);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetUserId);
	}

	public void Get(out JoinGameAcceptedCallbackInfo output)
	{
		output = default(JoinGameAcceptedCallbackInfo);
		output.Set(ref this);
	}
}
