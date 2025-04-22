using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryOwnershipBySandboxIdsCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryOwnershipBySandboxIdsCallbackInfo>, ISettable<QueryOwnershipBySandboxIdsCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_SandboxIdItemOwnerships;

	private uint m_SandboxIdItemOwnershipsCount;

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

	public SandboxIdItemOwnership[] SandboxIdItemOwnerships
	{
		get
		{
			Helper.Get<SandboxIdItemOwnershipInternal, SandboxIdItemOwnership>(m_SandboxIdItemOwnerships, out var value, m_SandboxIdItemOwnershipsCount);
			return value;
		}
		set
		{
			Helper.Set<SandboxIdItemOwnership, SandboxIdItemOwnershipInternal>(ref value, ref m_SandboxIdItemOwnerships, out m_SandboxIdItemOwnershipsCount);
		}
	}

	public void Set(ref QueryOwnershipBySandboxIdsCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		SandboxIdItemOwnerships = other.SandboxIdItemOwnerships;
	}

	public void Set(ref QueryOwnershipBySandboxIdsCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			SandboxIdItemOwnerships = other.Value.SandboxIdItemOwnerships;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_SandboxIdItemOwnerships);
	}

	public void Get(out QueryOwnershipBySandboxIdsCallbackInfo output)
	{
		output = default(QueryOwnershipBySandboxIdsCallbackInfo);
		output.Set(ref this);
	}
}
