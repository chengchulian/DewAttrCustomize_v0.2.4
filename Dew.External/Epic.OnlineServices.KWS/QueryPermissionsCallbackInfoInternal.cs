using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryPermissionsCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryPermissionsCallbackInfo>, ISettable<QueryPermissionsCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_KWSUserId;

	private IntPtr m_DateOfBirth;

	private int m_IsMinor;

	private IntPtr m_ParentEmail;

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

	public Utf8String KWSUserId
	{
		get
		{
			Helper.Get(m_KWSUserId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_KWSUserId);
		}
	}

	public Utf8String DateOfBirth
	{
		get
		{
			Helper.Get(m_DateOfBirth, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DateOfBirth);
		}
	}

	public bool IsMinor
	{
		get
		{
			Helper.Get(m_IsMinor, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_IsMinor);
		}
	}

	public Utf8String ParentEmail
	{
		get
		{
			Helper.Get(m_ParentEmail, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ParentEmail);
		}
	}

	public void Set(ref QueryPermissionsCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		KWSUserId = other.KWSUserId;
		DateOfBirth = other.DateOfBirth;
		IsMinor = other.IsMinor;
		ParentEmail = other.ParentEmail;
	}

	public void Set(ref QueryPermissionsCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			KWSUserId = other.Value.KWSUserId;
			DateOfBirth = other.Value.DateOfBirth;
			IsMinor = other.Value.IsMinor;
			ParentEmail = other.Value.ParentEmail;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_KWSUserId);
		Helper.Dispose(ref m_DateOfBirth);
		Helper.Dispose(ref m_ParentEmail);
	}

	public void Get(out QueryPermissionsCallbackInfo output)
	{
		output = default(QueryPermissionsCallbackInfo);
		output.Set(ref this);
	}
}
