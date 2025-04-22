using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryOwnershipCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryOwnershipCallbackInfo>, ISettable<QueryOwnershipCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_ItemOwnership;

	private uint m_ItemOwnershipCount;

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

	public ItemOwnership[] ItemOwnership
	{
		get
		{
			Helper.Get<ItemOwnershipInternal, ItemOwnership>(m_ItemOwnership, out var value, m_ItemOwnershipCount);
			return value;
		}
		set
		{
			Helper.Set<ItemOwnership, ItemOwnershipInternal>(ref value, ref m_ItemOwnership, out m_ItemOwnershipCount);
		}
	}

	public void Set(ref QueryOwnershipCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		ItemOwnership = other.ItemOwnership;
	}

	public void Set(ref QueryOwnershipCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			ItemOwnership = other.Value.ItemOwnership;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_ItemOwnership);
	}

	public void Get(out QueryOwnershipCallbackInfo output)
	{
		output = default(QueryOwnershipCallbackInfo);
		output.Set(ref this);
	}
}
