using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PageResultInternal : IGettable<PageResult>, ISettable<PageResult>, IDisposable
{
	private int m_StartIndex;

	private int m_Count;

	private int m_TotalCount;

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

	public int Count
	{
		get
		{
			return m_Count;
		}
		set
		{
			m_Count = value;
		}
	}

	public int TotalCount
	{
		get
		{
			return m_TotalCount;
		}
		set
		{
			m_TotalCount = value;
		}
	}

	public void Set(ref PageResult other)
	{
		StartIndex = other.StartIndex;
		Count = other.Count;
		TotalCount = other.TotalCount;
	}

	public void Set(ref PageResult? other)
	{
		if (other.HasValue)
		{
			StartIndex = other.Value.StartIndex;
			Count = other.Value.Count;
			TotalCount = other.Value.TotalCount;
		}
	}

	public void Dispose()
	{
	}

	public void Get(out PageResult output)
	{
		output = default(PageResult);
		output.Set(ref this);
	}
}
