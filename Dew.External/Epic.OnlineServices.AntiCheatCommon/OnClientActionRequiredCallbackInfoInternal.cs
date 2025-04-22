using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnClientActionRequiredCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnClientActionRequiredCallbackInfo>, ISettable<OnClientActionRequiredCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_ClientHandle;

	private AntiCheatCommonClientAction m_ClientAction;

	private AntiCheatCommonClientActionReason m_ActionReasonCode;

	private IntPtr m_ActionReasonDetailsString;

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

	public IntPtr ClientHandle
	{
		get
		{
			return m_ClientHandle;
		}
		set
		{
			m_ClientHandle = value;
		}
	}

	public AntiCheatCommonClientAction ClientAction
	{
		get
		{
			return m_ClientAction;
		}
		set
		{
			m_ClientAction = value;
		}
	}

	public AntiCheatCommonClientActionReason ActionReasonCode
	{
		get
		{
			return m_ActionReasonCode;
		}
		set
		{
			m_ActionReasonCode = value;
		}
	}

	public Utf8String ActionReasonDetailsString
	{
		get
		{
			Helper.Get(m_ActionReasonDetailsString, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ActionReasonDetailsString);
		}
	}

	public void Set(ref OnClientActionRequiredCallbackInfo other)
	{
		ClientData = other.ClientData;
		ClientHandle = other.ClientHandle;
		ClientAction = other.ClientAction;
		ActionReasonCode = other.ActionReasonCode;
		ActionReasonDetailsString = other.ActionReasonDetailsString;
	}

	public void Set(ref OnClientActionRequiredCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			ClientHandle = other.Value.ClientHandle;
			ClientAction = other.Value.ClientAction;
			ActionReasonCode = other.Value.ActionReasonCode;
			ActionReasonDetailsString = other.Value.ActionReasonDetailsString;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_ClientHandle);
		Helper.Dispose(ref m_ActionReasonDetailsString);
	}

	public void Get(out OnClientActionRequiredCallbackInfo output)
	{
		output = default(OnClientActionRequiredCallbackInfo);
		output.Set(ref this);
	}
}
