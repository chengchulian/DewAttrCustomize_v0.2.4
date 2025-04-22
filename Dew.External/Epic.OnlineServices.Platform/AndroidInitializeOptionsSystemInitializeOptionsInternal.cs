using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AndroidInitializeOptionsSystemInitializeOptionsInternal : IGettable<AndroidInitializeOptionsSystemInitializeOptions>, ISettable<AndroidInitializeOptionsSystemInitializeOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Reserved;

	private IntPtr m_OptionalInternalDirectory;

	private IntPtr m_OptionalExternalDirectory;

	public IntPtr Reserved
	{
		get
		{
			return m_Reserved;
		}
		set
		{
			m_Reserved = value;
		}
	}

	public Utf8String OptionalInternalDirectory
	{
		get
		{
			Helper.Get(m_OptionalInternalDirectory, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_OptionalInternalDirectory);
		}
	}

	public Utf8String OptionalExternalDirectory
	{
		get
		{
			Helper.Get(m_OptionalExternalDirectory, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_OptionalExternalDirectory);
		}
	}

	public void Set(ref AndroidInitializeOptionsSystemInitializeOptions other)
	{
		m_ApiVersion = 2;
		Reserved = other.Reserved;
		OptionalInternalDirectory = other.OptionalInternalDirectory;
		OptionalExternalDirectory = other.OptionalExternalDirectory;
	}

	public void Set(ref AndroidInitializeOptionsSystemInitializeOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			m_Reserved = other.Value.Reserved;
			if (m_Reserved == IntPtr.Zero)
			{
				Helper.Set(new int[2] { 1, 1 }, ref m_Reserved);
			}
			OptionalInternalDirectory = other.Value.OptionalInternalDirectory;
			OptionalExternalDirectory = other.Value.OptionalExternalDirectory;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Reserved);
		Helper.Dispose(ref m_OptionalInternalDirectory);
		Helper.Dispose(ref m_OptionalExternalDirectory);
	}

	public void Get(out AndroidInitializeOptionsSystemInitializeOptions output)
	{
		output = default(AndroidInitializeOptionsSystemInitializeOptions);
		output.Set(ref this);
	}
}
