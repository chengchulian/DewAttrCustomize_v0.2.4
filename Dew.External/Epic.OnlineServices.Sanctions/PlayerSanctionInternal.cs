using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PlayerSanctionInternal : IGettable<PlayerSanction>, ISettable<PlayerSanction>, IDisposable
{
	private int m_ApiVersion;

	private long m_TimePlaced;

	private IntPtr m_Action;

	private long m_TimeExpires;

	private IntPtr m_ReferenceId;

	public long TimePlaced
	{
		get
		{
			return m_TimePlaced;
		}
		set
		{
			m_TimePlaced = value;
		}
	}

	public Utf8String Action
	{
		get
		{
			Helper.Get(m_Action, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Action);
		}
	}

	public long TimeExpires
	{
		get
		{
			return m_TimeExpires;
		}
		set
		{
			m_TimeExpires = value;
		}
	}

	public Utf8String ReferenceId
	{
		get
		{
			Helper.Get(m_ReferenceId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ReferenceId);
		}
	}

	public void Set(ref PlayerSanction other)
	{
		m_ApiVersion = 2;
		TimePlaced = other.TimePlaced;
		Action = other.Action;
		TimeExpires = other.TimeExpires;
		ReferenceId = other.ReferenceId;
	}

	public void Set(ref PlayerSanction? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			TimePlaced = other.Value.TimePlaced;
			Action = other.Value.Action;
			TimeExpires = other.Value.TimeExpires;
			ReferenceId = other.Value.ReferenceId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Action);
		Helper.Dispose(ref m_ReferenceId);
	}

	public void Get(out PlayerSanction output)
	{
		output = default(PlayerSanction);
		output.Set(ref this);
	}
}
