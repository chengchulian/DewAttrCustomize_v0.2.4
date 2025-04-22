using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnDisplaySettingsUpdatedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnDisplaySettingsUpdatedCallbackInfo>, ISettable<OnDisplaySettingsUpdatedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private int m_IsVisible;

	private int m_IsExclusiveInput;

	public object ClientData
	{
		get
		{
			Helper.Get(m_ClientData, out object value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientData);
		}
	}

	public IntPtr ClientDataAddress => m_ClientData;

	public bool IsVisible
	{
		get
		{
			Helper.Get(m_IsVisible, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_IsVisible);
		}
	}

	public bool IsExclusiveInput
	{
		get
		{
			Helper.Get(m_IsExclusiveInput, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_IsExclusiveInput);
		}
	}

	public void Set(ref OnDisplaySettingsUpdatedCallbackInfo other)
	{
		ClientData = other.ClientData;
		IsVisible = other.IsVisible;
		IsExclusiveInput = other.IsExclusiveInput;
	}

	public void Set(ref OnDisplaySettingsUpdatedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			IsVisible = other.Value.IsVisible;
			IsExclusiveInput = other.Value.IsExclusiveInput;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
	}

	public void Get(out OnDisplaySettingsUpdatedCallbackInfo output)
	{
		output = default(OnDisplaySettingsUpdatedCallbackInfo);
		output.Set(ref this);
	}
}
