using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PermissionStatusInternal : IGettable<PermissionStatus>, ISettable<PermissionStatus>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Name;

	private KWSPermissionStatus m_Status;

	public Utf8String Name
	{
		get
		{
			Helper.Get(m_Name, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Name);
		}
	}

	public KWSPermissionStatus Status
	{
		get
		{
			return m_Status;
		}
		set
		{
			m_Status = value;
		}
	}

	public void Set(ref PermissionStatus other)
	{
		m_ApiVersion = 1;
		Name = other.Name;
		Status = other.Status;
	}

	public void Set(ref PermissionStatus? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Name = other.Value.Name;
			Status = other.Value.Status;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Name);
	}

	public void Get(out PermissionStatus output)
	{
		output = default(PermissionStatus);
		output.Set(ref this);
	}
}
