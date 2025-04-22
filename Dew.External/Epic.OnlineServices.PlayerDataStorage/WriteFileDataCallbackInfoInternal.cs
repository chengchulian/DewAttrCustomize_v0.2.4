using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct WriteFileDataCallbackInfoInternal : ICallbackInfoInternal, IGettable<WriteFileDataCallbackInfo>, ISettable<WriteFileDataCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_Filename;

	private uint m_DataBufferLengthBytes;

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

	public ProductUserId LocalUserId
	{
		get
		{
			Helper.Get(m_LocalUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String Filename
	{
		get
		{
			Helper.Get(m_Filename, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Filename);
		}
	}

	public uint DataBufferLengthBytes
	{
		get
		{
			return m_DataBufferLengthBytes;
		}
		set
		{
			m_DataBufferLengthBytes = value;
		}
	}

	public void Set(ref WriteFileDataCallbackInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		Filename = other.Filename;
		DataBufferLengthBytes = other.DataBufferLengthBytes;
	}

	public void Set(ref WriteFileDataCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			Filename = other.Value.Filename;
			DataBufferLengthBytes = other.Value.DataBufferLengthBytes;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_Filename);
	}

	public void Get(out WriteFileDataCallbackInfo output)
	{
		output = default(WriteFileDataCallbackInfo);
		output.Set(ref this);
	}
}
