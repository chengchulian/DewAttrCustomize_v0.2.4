using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QuatInternal : IGettable<Quat>, ISettable<Quat>, IDisposable
{
	private float m_w;

	private float m_x;

	private float m_y;

	private float m_z;

	public float w
	{
		get
		{
			return m_w;
		}
		set
		{
			m_w = value;
		}
	}

	public float x
	{
		get
		{
			return m_x;
		}
		set
		{
			m_x = value;
		}
	}

	public float y
	{
		get
		{
			return m_y;
		}
		set
		{
			m_y = value;
		}
	}

	public float z
	{
		get
		{
			return m_z;
		}
		set
		{
			m_z = value;
		}
	}

	public void Set(ref Quat other)
	{
		w = other.w;
		x = other.x;
		y = other.y;
		z = other.z;
	}

	public void Set(ref Quat? other)
	{
		if (other.HasValue)
		{
			w = other.Value.w;
			x = other.Value.x;
			y = other.Value.y;
			z = other.Value.z;
		}
	}

	public void Dispose()
	{
	}

	public void Get(out Quat output)
	{
		output = default(Quat);
		output.Set(ref this);
	}
}
