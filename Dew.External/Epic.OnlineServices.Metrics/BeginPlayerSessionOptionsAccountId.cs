namespace Epic.OnlineServices.Metrics;

public struct BeginPlayerSessionOptionsAccountId
{
	private MetricsAccountIdType m_AccountIdType;

	private EpicAccountId m_Epic;

	private Utf8String m_External;

	public MetricsAccountIdType AccountIdType
	{
		get
		{
			return m_AccountIdType;
		}
		private set
		{
			m_AccountIdType = value;
		}
	}

	public EpicAccountId Epic
	{
		get
		{
			Helper.Get(m_Epic, out var value, m_AccountIdType, MetricsAccountIdType.Epic);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Epic, MetricsAccountIdType.Epic, ref m_AccountIdType);
		}
	}

	public Utf8String External
	{
		get
		{
			Helper.Get(m_External, out var value, m_AccountIdType, MetricsAccountIdType.External);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_External, MetricsAccountIdType.External, ref m_AccountIdType);
		}
	}

	public static implicit operator BeginPlayerSessionOptionsAccountId(EpicAccountId value)
	{
		BeginPlayerSessionOptionsAccountId result = default(BeginPlayerSessionOptionsAccountId);
		result.Epic = value;
		return result;
	}

	public static implicit operator BeginPlayerSessionOptionsAccountId(Utf8String value)
	{
		BeginPlayerSessionOptionsAccountId result = default(BeginPlayerSessionOptionsAccountId);
		result.External = value;
		return result;
	}

	public static implicit operator BeginPlayerSessionOptionsAccountId(string value)
	{
		BeginPlayerSessionOptionsAccountId result = default(BeginPlayerSessionOptionsAccountId);
		result.External = value;
		return result;
	}

	internal void Set(ref BeginPlayerSessionOptionsAccountIdInternal other)
	{
		Epic = other.Epic;
		External = other.External;
	}
}
