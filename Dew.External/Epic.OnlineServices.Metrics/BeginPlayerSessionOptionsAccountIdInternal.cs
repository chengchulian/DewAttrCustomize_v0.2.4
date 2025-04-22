using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Metrics;

[StructLayout(LayoutKind.Explicit, Pack = 4)]
internal struct BeginPlayerSessionOptionsAccountIdInternal : IGettable<BeginPlayerSessionOptionsAccountId>, ISettable<BeginPlayerSessionOptionsAccountId>, IDisposable
{
	[FieldOffset(0)]
	private MetricsAccountIdType m_AccountIdType;

	[FieldOffset(4)]
	private IntPtr m_Epic;

	[FieldOffset(4)]
	private IntPtr m_External;

	public EpicAccountId Epic
	{
		get
		{
			Helper.Get(m_Epic, out EpicAccountId value, m_AccountIdType, MetricsAccountIdType.Epic);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Epic, MetricsAccountIdType.Epic, ref m_AccountIdType, this);
		}
	}

	public Utf8String External
	{
		get
		{
			Helper.Get(m_External, out Utf8String value, m_AccountIdType, MetricsAccountIdType.External);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_External, MetricsAccountIdType.External, ref m_AccountIdType, this);
		}
	}

	public void Set(ref BeginPlayerSessionOptionsAccountId other)
	{
		Epic = other.Epic;
		External = other.External;
	}

	public void Set(ref BeginPlayerSessionOptionsAccountId? other)
	{
		if (other.HasValue)
		{
			Epic = other.Value.Epic;
			External = other.Value.External;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Epic);
		Helper.Dispose(ref m_External, m_AccountIdType, MetricsAccountIdType.External);
	}

	public void Get(out BeginPlayerSessionOptionsAccountId output)
	{
		output = default(BeginPlayerSessionOptionsAccountId);
		output.Set(ref this);
	}
}
