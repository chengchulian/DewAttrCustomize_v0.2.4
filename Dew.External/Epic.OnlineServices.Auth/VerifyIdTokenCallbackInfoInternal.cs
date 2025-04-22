using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct VerifyIdTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<VerifyIdTokenCallbackInfo>, ISettable<VerifyIdTokenCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_ApplicationId;

	private IntPtr m_ClientId;

	private IntPtr m_ProductId;

	private IntPtr m_SandboxId;

	private IntPtr m_DeploymentId;

	private IntPtr m_DisplayName;

	private int m_IsExternalAccountInfoPresent;

	private ExternalAccountType m_ExternalAccountIdType;

	private IntPtr m_ExternalAccountId;

	private IntPtr m_ExternalAccountDisplayName;

	private IntPtr m_Platform;

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

	public Utf8String ApplicationId
	{
		get
		{
			Helper.Get(m_ApplicationId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ApplicationId);
		}
	}

	public Utf8String ClientId
	{
		get
		{
			Helper.Get(m_ClientId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientId);
		}
	}

	public Utf8String ProductId
	{
		get
		{
			Helper.Get(m_ProductId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ProductId);
		}
	}

	public Utf8String SandboxId
	{
		get
		{
			Helper.Get(m_SandboxId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SandboxId);
		}
	}

	public Utf8String DeploymentId
	{
		get
		{
			Helper.Get(m_DeploymentId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DeploymentId);
		}
	}

	public Utf8String DisplayName
	{
		get
		{
			Helper.Get(m_DisplayName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DisplayName);
		}
	}

	public bool IsExternalAccountInfoPresent
	{
		get
		{
			Helper.Get(m_IsExternalAccountInfoPresent, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_IsExternalAccountInfoPresent);
		}
	}

	public ExternalAccountType ExternalAccountIdType
	{
		get
		{
			return m_ExternalAccountIdType;
		}
		set
		{
			m_ExternalAccountIdType = value;
		}
	}

	public Utf8String ExternalAccountId
	{
		get
		{
			Helper.Get(m_ExternalAccountId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ExternalAccountId);
		}
	}

	public Utf8String ExternalAccountDisplayName
	{
		get
		{
			Helper.Get(m_ExternalAccountDisplayName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ExternalAccountDisplayName);
		}
	}

	public Utf8String Platform
	{
		get
		{
			Helper.Get(m_Platform, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Platform);
		}
	}

	public void Set(ref VerifyIdTokenCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		ApplicationId = other.ApplicationId;
		ClientId = other.ClientId;
		ProductId = other.ProductId;
		SandboxId = other.SandboxId;
		DeploymentId = other.DeploymentId;
		DisplayName = other.DisplayName;
		IsExternalAccountInfoPresent = other.IsExternalAccountInfoPresent;
		ExternalAccountIdType = other.ExternalAccountIdType;
		ExternalAccountId = other.ExternalAccountId;
		ExternalAccountDisplayName = other.ExternalAccountDisplayName;
		Platform = other.Platform;
	}

	public void Set(ref VerifyIdTokenCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			ApplicationId = other.Value.ApplicationId;
			ClientId = other.Value.ClientId;
			ProductId = other.Value.ProductId;
			SandboxId = other.Value.SandboxId;
			DeploymentId = other.Value.DeploymentId;
			DisplayName = other.Value.DisplayName;
			IsExternalAccountInfoPresent = other.Value.IsExternalAccountInfoPresent;
			ExternalAccountIdType = other.Value.ExternalAccountIdType;
			ExternalAccountId = other.Value.ExternalAccountId;
			ExternalAccountDisplayName = other.Value.ExternalAccountDisplayName;
			Platform = other.Value.Platform;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_ApplicationId);
		Helper.Dispose(ref m_ClientId);
		Helper.Dispose(ref m_ProductId);
		Helper.Dispose(ref m_SandboxId);
		Helper.Dispose(ref m_DeploymentId);
		Helper.Dispose(ref m_DisplayName);
		Helper.Dispose(ref m_ExternalAccountId);
		Helper.Dispose(ref m_ExternalAccountDisplayName);
		Helper.Dispose(ref m_Platform);
	}

	public void Get(out VerifyIdTokenCallbackInfo output)
	{
		output = default(VerifyIdTokenCallbackInfo);
		output.Set(ref this);
	}
}
