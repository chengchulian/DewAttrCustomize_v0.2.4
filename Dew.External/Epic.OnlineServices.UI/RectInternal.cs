using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RectInternal : IGettable<Rect>, ISettable<Rect>, IDisposable
{
	private int m_ApiVersion;

	private int m_X;

	private int m_Y;

	private uint m_Width;

	private uint m_Height;

	public int X
	{
		get
		{
			return m_X;
		}
		set
		{
			m_X = value;
		}
	}

	public int Y
	{
		get
		{
			return m_Y;
		}
		set
		{
			m_Y = value;
		}
	}

	public uint Width
	{
		get
		{
			return m_Width;
		}
		set
		{
			m_Width = value;
		}
	}

	public uint Height
	{
		get
		{
			return m_Height;
		}
		set
		{
			m_Height = value;
		}
	}

	public void Set(ref Rect other)
	{
		m_ApiVersion = 1;
		X = other.X;
		Y = other.Y;
		Width = other.Width;
		Height = other.Height;
	}

	public void Set(ref Rect? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			X = other.Value.X;
			Y = other.Value.Y;
			Width = other.Value.Width;
			Height = other.Value.Height;
		}
	}

	public void Dispose()
	{
	}

	public void Get(out Rect output)
	{
		output = default(Rect);
		output.Set(ref this);
	}
}
