using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PresenceModificationSetRawRichTextOptionsInternal : ISettable<PresenceModificationSetRawRichTextOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_RichText;

	public Utf8String RichText
	{
		set
		{
			Helper.Set(value, ref m_RichText);
		}
	}

	public void Set(ref PresenceModificationSetRawRichTextOptions other)
	{
		m_ApiVersion = 1;
		RichText = other.RichText;
	}

	public void Set(ref PresenceModificationSetRawRichTextOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			RichText = other.Value.RichText;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_RichText);
	}
}
