using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LocalRTCOptionsInternal : IGettable<LocalRTCOptions>, ISettable<LocalRTCOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_Flags;

	private int m_UseManualAudioInput;

	private int m_UseManualAudioOutput;

	private int m_LocalAudioDeviceInputStartsMuted;

	public uint Flags
	{
		get
		{
			return m_Flags;
		}
		set
		{
			m_Flags = value;
		}
	}

	public bool UseManualAudioInput
	{
		get
		{
			Helper.Get(m_UseManualAudioInput, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UseManualAudioInput);
		}
	}

	public bool UseManualAudioOutput
	{
		get
		{
			Helper.Get(m_UseManualAudioOutput, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UseManualAudioOutput);
		}
	}

	public bool LocalAudioDeviceInputStartsMuted
	{
		get
		{
			Helper.Get(m_LocalAudioDeviceInputStartsMuted, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LocalAudioDeviceInputStartsMuted);
		}
	}

	public void Set(ref LocalRTCOptions other)
	{
		m_ApiVersion = 1;
		Flags = other.Flags;
		UseManualAudioInput = other.UseManualAudioInput;
		UseManualAudioOutput = other.UseManualAudioOutput;
		LocalAudioDeviceInputStartsMuted = other.LocalAudioDeviceInputStartsMuted;
	}

	public void Set(ref LocalRTCOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Flags = other.Value.Flags;
			UseManualAudioInput = other.Value.UseManualAudioInput;
			UseManualAudioOutput = other.Value.UseManualAudioOutput;
			LocalAudioDeviceInputStartsMuted = other.Value.LocalAudioDeviceInputStartsMuted;
		}
	}

	public void Dispose()
	{
	}

	public void Get(out LocalRTCOptions output)
	{
		output = default(LocalRTCOptions);
		output.Set(ref this);
	}
}
