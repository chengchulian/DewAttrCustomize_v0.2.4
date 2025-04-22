using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ModInfoInternal : IGettable<ModInfo>, ISettable<ModInfo>, IDisposable
{
	private int m_ApiVersion;

	private int m_ModsCount;

	private IntPtr m_Mods;

	private ModEnumerationType m_Type;

	public ModIdentifier[] Mods
	{
		get
		{
			Helper.Get<ModIdentifierInternal, ModIdentifier>(m_Mods, out var value, m_ModsCount);
			return value;
		}
		set
		{
			Helper.Set<ModIdentifier, ModIdentifierInternal>(ref value, ref m_Mods, out m_ModsCount);
		}
	}

	public ModEnumerationType Type
	{
		get
		{
			return m_Type;
		}
		set
		{
			m_Type = value;
		}
	}

	public void Set(ref ModInfo other)
	{
		m_ApiVersion = 1;
		Mods = other.Mods;
		Type = other.Type;
	}

	public void Set(ref ModInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Mods = other.Value.Mods;
			Type = other.Value.Type;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Mods);
	}

	public void Get(out ModInfo output)
	{
		output = default(ModInfo);
		output.Set(ref this);
	}
}
