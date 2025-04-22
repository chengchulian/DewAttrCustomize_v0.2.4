using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LoginCallbackInfoInternal : ICallbackInfoInternal, IGettable<LoginCallbackInfo>, ISettable<LoginCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_ContinuanceToken;

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

	public ContinuanceToken ContinuanceToken
	{
		get
		{
			Helper.Get(m_ContinuanceToken, out ContinuanceToken value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ContinuanceToken);
		}
	}

	public void Set(ref LoginCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		ContinuanceToken = other.ContinuanceToken;
	}

	public void Set(ref LoginCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			ContinuanceToken = other.Value.ContinuanceToken;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_ContinuanceToken);
	}

	public void Get(out LoginCallbackInfo output)
	{
		output = default(LoginCallbackInfo);
		output.Set(ref this);
	}
}
