using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LinkAccountCallbackInfoInternal : ICallbackInfoInternal, IGettable<LinkAccountCallbackInfo>, ISettable<LinkAccountCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_PinGrantInfo;

	private IntPtr m_SelectedAccountId;

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

	public PinGrantInfo? PinGrantInfo
	{
		get
		{
			Helper.Get<PinGrantInfoInternal, PinGrantInfo>(m_PinGrantInfo, out PinGrantInfo? value);
			return value;
		}
		set
		{
			Helper.Set<PinGrantInfo, PinGrantInfoInternal>(ref value, ref m_PinGrantInfo);
		}
	}

	public EpicAccountId SelectedAccountId
	{
		get
		{
			Helper.Get(m_SelectedAccountId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SelectedAccountId);
		}
	}

	public void Set(ref LinkAccountCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		PinGrantInfo = other.PinGrantInfo;
		SelectedAccountId = other.SelectedAccountId;
	}

	public void Set(ref LinkAccountCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			PinGrantInfo = other.Value.PinGrantInfo;
			SelectedAccountId = other.Value.SelectedAccountId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_PinGrantInfo);
		Helper.Dispose(ref m_SelectedAccountId);
	}

	public void Get(out LinkAccountCallbackInfo output)
	{
		output = default(LinkAccountCallbackInfo);
		output.Set(ref this);
	}
}
