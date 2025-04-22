using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SubmitSnapshotCallbackInfoInternal : ICallbackInfoInternal, IGettable<SubmitSnapshotCallbackInfo>, ISettable<SubmitSnapshotCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private uint m_SnapshotId;

	private IntPtr m_ClientData;

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

	public uint SnapshotId
	{
		get
		{
			return m_SnapshotId;
		}
		set
		{
			m_SnapshotId = value;
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

	public void Set(ref SubmitSnapshotCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		SnapshotId = other.SnapshotId;
		ClientData = other.ClientData;
	}

	public void Set(ref SubmitSnapshotCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			SnapshotId = other.Value.SnapshotId;
			ClientData = other.Value.ClientData;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
	}

	public void Get(out SubmitSnapshotCallbackInfo output)
	{
		output = default(SubmitSnapshotCallbackInfo);
		output.Set(ref this);
	}
}
