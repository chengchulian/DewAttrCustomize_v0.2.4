using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PresenceModificationSetDataOptionsInternal : ISettable<PresenceModificationSetDataOptions>, IDisposable
{
	private int m_ApiVersion;

	private int m_RecordsCount;

	private IntPtr m_Records;

	public DataRecord[] Records
	{
		set
		{
			Helper.Set<DataRecord, DataRecordInternal>(ref value, ref m_Records, out m_RecordsCount);
		}
	}

	public void Set(ref PresenceModificationSetDataOptions other)
	{
		m_ApiVersion = 1;
		Records = other.Records;
	}

	public void Set(ref PresenceModificationSetDataOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Records = other.Value.Records;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Records);
	}
}
