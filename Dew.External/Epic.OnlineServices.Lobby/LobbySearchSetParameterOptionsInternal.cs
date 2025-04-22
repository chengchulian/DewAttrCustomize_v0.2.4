using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbySearchSetParameterOptionsInternal : ISettable<LobbySearchSetParameterOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Parameter;

	private ComparisonOp m_ComparisonOp;

	public AttributeData? Parameter
	{
		set
		{
			Helper.Set<AttributeData, AttributeDataInternal>(ref value, ref m_Parameter);
		}
	}

	public ComparisonOp ComparisonOp
	{
		set
		{
			m_ComparisonOp = value;
		}
	}

	public void Set(ref LobbySearchSetParameterOptions other)
	{
		m_ApiVersion = 1;
		Parameter = other.Parameter;
		ComparisonOp = other.ComparisonOp;
	}

	public void Set(ref LobbySearchSetParameterOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Parameter = other.Value.Parameter;
			ComparisonOp = other.Value.ComparisonOp;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Parameter);
	}
}
