using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnClientIntegrityViolatedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnClientIntegrityViolatedCallbackInfo>, ISettable<OnClientIntegrityViolatedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private AntiCheatClientViolationType m_ViolationType;

	private IntPtr m_ViolationMessage;

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

	public AntiCheatClientViolationType ViolationType
	{
		get
		{
			return m_ViolationType;
		}
		set
		{
			m_ViolationType = value;
		}
	}

	public Utf8String ViolationMessage
	{
		get
		{
			Helper.Get(m_ViolationMessage, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ViolationMessage);
		}
	}

	public void Set(ref OnClientIntegrityViolatedCallbackInfo other)
	{
		ClientData = other.ClientData;
		ViolationType = other.ViolationType;
		ViolationMessage = other.ViolationMessage;
	}

	public void Set(ref OnClientIntegrityViolatedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			ViolationType = other.Value.ViolationType;
			ViolationMessage = other.Value.ViolationMessage;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_ViolationMessage);
	}

	public void Get(out OnClientIntegrityViolatedCallbackInfo output)
	{
		output = default(OnClientIntegrityViolatedCallbackInfo);
		output.Set(ref this);
	}
}
