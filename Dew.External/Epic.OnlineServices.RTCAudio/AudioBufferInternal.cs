using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AudioBufferInternal : IGettable<AudioBuffer>, ISettable<AudioBuffer>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Frames;

	private uint m_FramesCount;

	private uint m_SampleRate;

	private uint m_Channels;

	public short[] Frames
	{
		get
		{
			Helper.Get(m_Frames, out short[] value, m_FramesCount);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Frames, out m_FramesCount);
		}
	}

	public uint SampleRate
	{
		get
		{
			return m_SampleRate;
		}
		set
		{
			m_SampleRate = value;
		}
	}

	public uint Channels
	{
		get
		{
			return m_Channels;
		}
		set
		{
			m_Channels = value;
		}
	}

	public void Set(ref AudioBuffer other)
	{
		m_ApiVersion = 1;
		Frames = other.Frames;
		SampleRate = other.SampleRate;
		Channels = other.Channels;
	}

	public void Set(ref AudioBuffer? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Frames = other.Value.Frames;
			SampleRate = other.Value.SampleRate;
			Channels = other.Value.Channels;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Frames);
	}

	public void Get(out AudioBuffer output)
	{
		output = default(AudioBuffer);
		output.Set(ref this);
	}
}
