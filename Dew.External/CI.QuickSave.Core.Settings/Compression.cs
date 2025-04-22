using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CI.QuickSave.Core.Settings;

public class Compression
{
	public static string Compress(string value, CompressionMode compressionMode)
	{
		if (compressionMode == CompressionMode.None)
		{
			return value;
		}
		using MemoryStream streamIn = new MemoryStream(Encoding.UTF8.GetBytes(value));
		using MemoryStream streamOut = new MemoryStream();
		using (GZipStream compressionStream = new GZipStream(streamOut, global::System.IO.Compression.CompressionMode.Compress))
		{
			streamIn.CopyTo(compressionStream);
		}
		return Convert.ToBase64String(streamOut.ToArray());
	}

	public static string Decompress(string value, CompressionMode compressionMode)
	{
		if (compressionMode == CompressionMode.None)
		{
			return value;
		}
		using MemoryStream streamIn = new MemoryStream(Convert.FromBase64String(value));
		using MemoryStream streamOut = new MemoryStream();
		using (GZipStream compressionStream = new GZipStream(streamIn, global::System.IO.Compression.CompressionMode.Decompress))
		{
			compressionStream.CopyTo(streamOut);
		}
		return Encoding.UTF8.GetString(streamOut.ToArray());
	}
}
