using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct InstallModCallbackInfoInternal : ICallbackInfoInternal, IGettable<InstallModCallbackInfo>, ISettable<InstallModCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_LocalUserId;

	private IntPtr m_ClientData;

	private IntPtr m_Mod;

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

	public ModIdentifier? Mod
	{
		get
		{
			Helper.Get<ModIdentifierInternal, ModIdentifier>(m_Mod, out ModIdentifier? value);
			return value;
		}
		set
		{
			Helper.Set<ModIdentifier, ModIdentifierInternal>(ref value, ref m_Mod);
		}
	}

	public void Set(ref InstallModCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		LocalUserId = other.LocalUserId;
		ClientData = other.ClientData;
		Mod = other.Mod;
	}

	public void Set(ref InstallModCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			LocalUserId = other.Value.LocalUserId;
			ClientData = other.Value.ClientData;
			Mod = other.Value.Mod;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_Mod);
	}

	public void Get(out InstallModCallbackInfo output)
	{
		output = default(InstallModCallbackInfo);
		output.Set(ref this);
	}
}
