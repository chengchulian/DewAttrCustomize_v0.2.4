using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct FinalizeInviteOptionsInternal : ISettable<FinalizeInviteOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private IntPtr m_LocalUserId;

	private IntPtr m_CustomInviteId;

	private Result m_ProcessingResult;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String CustomInviteId
	{
		set
		{
			Helper.Set(value, ref m_CustomInviteId);
		}
	}

	public Result ProcessingResult
	{
		set
		{
			m_ProcessingResult = value;
		}
	}

	public void Set(ref FinalizeInviteOptions other)
	{
		m_ApiVersion = 1;
		TargetUserId = other.TargetUserId;
		LocalUserId = other.LocalUserId;
		CustomInviteId = other.CustomInviteId;
		ProcessingResult = other.ProcessingResult;
	}

	public void Set(ref FinalizeInviteOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			TargetUserId = other.Value.TargetUserId;
			LocalUserId = other.Value.LocalUserId;
			CustomInviteId = other.Value.CustomInviteId;
			ProcessingResult = other.Value.ProcessingResult;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_CustomInviteId);
	}
}
