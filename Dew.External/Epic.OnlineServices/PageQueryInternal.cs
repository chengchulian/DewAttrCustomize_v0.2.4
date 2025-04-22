using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PageQueryInternal : IGettable<PageQuery>, ISettable<PageQuery>, IDisposable
{
	private int m_ApiVersion;

	private int m_StartIndex;

	private int m_MaxCount;

	public int StartIndex
	{
		get
		{
			return m_StartIndex;
		}
		set
		{
			m_StartIndex = value;
		}
	}

	public int MaxCount
	{
		get
		{
			return m_MaxCount;
		}
		set
		{
			m_MaxCount = value;
		}
	}

	public void Set(ref PageQuery other)
	{
		m_ApiVersion = 1;
		StartIndex = other.StartIndex;
		MaxCount = other.MaxCount;
	}

	public void Set(ref PageQuery? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			StartIndex = other.Value.StartIndex;
			MaxCount = other.Value.MaxCount;
		}
	}

	public void Dispose()
	{
	}

	public void Get(out PageQuery output)
	{
		output = default(PageQuery);
		output.Set(ref this);
	}
}
