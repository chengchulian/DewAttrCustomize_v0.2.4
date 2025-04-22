using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AndroidInitializeOptionsInternal : ISettable<AndroidInitializeOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_AllocateMemoryFunction;

	private IntPtr m_ReallocateMemoryFunction;

	private IntPtr m_ReleaseMemoryFunction;

	private IntPtr m_ProductName;

	private IntPtr m_ProductVersion;

	private IntPtr m_Reserved;

	private IntPtr m_SystemInitializeOptions;

	private IntPtr m_OverrideThreadAffinity;

	public IntPtr AllocateMemoryFunction
	{
		set
		{
			m_AllocateMemoryFunction = value;
		}
	}

	public IntPtr ReallocateMemoryFunction
	{
		set
		{
			m_ReallocateMemoryFunction = value;
		}
	}

	public IntPtr ReleaseMemoryFunction
	{
		set
		{
			m_ReleaseMemoryFunction = value;
		}
	}

	public Utf8String ProductName
	{
		set
		{
			Helper.Set(value, ref m_ProductName);
		}
	}

	public Utf8String ProductVersion
	{
		set
		{
			Helper.Set(value, ref m_ProductVersion);
		}
	}

	public IntPtr Reserved
	{
		set
		{
			m_Reserved = value;
		}
	}

	public AndroidInitializeOptionsSystemInitializeOptions? SystemInitializeOptions
	{
		set
		{
			Helper.Set<AndroidInitializeOptionsSystemInitializeOptions, AndroidInitializeOptionsSystemInitializeOptionsInternal>(ref value, ref m_SystemInitializeOptions);
		}
	}

	public InitializeThreadAffinity? OverrideThreadAffinity
	{
		set
		{
			Helper.Set<InitializeThreadAffinity, InitializeThreadAffinityInternal>(ref value, ref m_OverrideThreadAffinity);
		}
	}

	public void Set(ref AndroidInitializeOptions other)
	{
		m_ApiVersion = 4;
		AllocateMemoryFunction = other.AllocateMemoryFunction;
		ReallocateMemoryFunction = other.ReallocateMemoryFunction;
		ReleaseMemoryFunction = other.ReleaseMemoryFunction;
		ProductName = other.ProductName;
		ProductVersion = other.ProductVersion;
		m_Reserved = other.Reserved;
		if (m_Reserved == IntPtr.Zero)
		{
			Helper.Set(new int[2] { 1, 1 }, ref m_Reserved);
		}
		SystemInitializeOptions = other.SystemInitializeOptions;
		OverrideThreadAffinity = other.OverrideThreadAffinity;
	}

	public void Set(ref AndroidInitializeOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 4;
			AllocateMemoryFunction = other.Value.AllocateMemoryFunction;
			ReallocateMemoryFunction = other.Value.ReallocateMemoryFunction;
			ReleaseMemoryFunction = other.Value.ReleaseMemoryFunction;
			ProductName = other.Value.ProductName;
			ProductVersion = other.Value.ProductVersion;
			m_Reserved = other.Value.Reserved;
			if (m_Reserved == IntPtr.Zero)
			{
				Helper.Set(new int[2] { 1, 1 }, ref m_Reserved);
			}
			SystemInitializeOptions = other.Value.SystemInitializeOptions;
			OverrideThreadAffinity = other.Value.OverrideThreadAffinity;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AllocateMemoryFunction);
		Helper.Dispose(ref m_ReallocateMemoryFunction);
		Helper.Dispose(ref m_ReleaseMemoryFunction);
		Helper.Dispose(ref m_ProductName);
		Helper.Dispose(ref m_ProductVersion);
		Helper.Dispose(ref m_Reserved);
		Helper.Dispose(ref m_SystemInitializeOptions);
		Helper.Dispose(ref m_OverrideThreadAffinity);
	}
}
