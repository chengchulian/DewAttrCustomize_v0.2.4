using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct FileTransferProgressCallbackInfoInternal : ICallbackInfoInternal, IGettable<FileTransferProgressCallbackInfo>, ISettable<FileTransferProgressCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_Filename;

	private uint m_BytesTransferred;

	private uint m_TotalFileSizeBytes;

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

	public uint BytesTransferred
	{
		get
		{
			return m_BytesTransferred;
		}
		set
		{
			m_BytesTransferred = value;
		}
	}

	public uint TotalFileSizeBytes
	{
		get
		{
			return m_TotalFileSizeBytes;
		}
		set
		{
			m_TotalFileSizeBytes = value;
		}
	}

	public void Set(ref FileTransferProgressCallbackInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		Filename = other.Filename;
		BytesTransferred = other.BytesTransferred;
		TotalFileSizeBytes = other.TotalFileSizeBytes;
	}

	public void Set(ref FileTransferProgressCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			Filename = other.Value.Filename;
			BytesTransferred = other.Value.BytesTransferred;
			TotalFileSizeBytes = other.Value.TotalFileSizeBytes;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_Filename);
	}

	public void Get(out FileTransferProgressCallbackInfo output)
	{
		output = default(FileTransferProgressCallbackInfo);
		output.Set(ref this);
	}
}
