using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct KeyImageInfoInternal : IGettable<KeyImageInfo>, ISettable<KeyImageInfo>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Type;

	private IntPtr m_Url;

	private uint m_Width;

	private uint m_Height;

	public Utf8String Type
	{
		get
		{
			Helper.Get(m_Type, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Type);
		}
	}

	public Utf8String Url
	{
		get
		{
			Helper.Get(m_Url, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Url);
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

	public void Set(ref KeyImageInfo other)
	{
		m_ApiVersion = 1;
		Type = other.Type;
		Url = other.Url;
		Width = other.Width;
		Height = other.Height;
	}

	public void Set(ref KeyImageInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Type = other.Value.Type;
			Url = other.Value.Url;
			Width = other.Value.Width;
			Height = other.Value.Height;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Type);
		Helper.Dispose(ref m_Url);
	}

	public void Get(out KeyImageInfo output)
	{
		output = default(KeyImageInfo);
		output.Set(ref this);
	}
}
