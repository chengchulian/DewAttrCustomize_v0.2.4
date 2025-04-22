using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnQueryNATTypeCompleteInfoInternal : ICallbackInfoInternal, IGettable<OnQueryNATTypeCompleteInfo>, ISettable<OnQueryNATTypeCompleteInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private NATType m_NATType;

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

	public NATType NATType
	{
		get
		{
			return m_NATType;
		}
		set
		{
			m_NATType = value;
		}
	}

	public void Set(ref OnQueryNATTypeCompleteInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		NATType = other.NATType;
	}

	public void Set(ref OnQueryNATTypeCompleteInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			NATType = other.Value.NATType;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
	}

	public void Get(out OnQueryNATTypeCompleteInfo output)
	{
		output = default(OnQueryNATTypeCompleteInfo);
		output.Set(ref this);
	}
}
