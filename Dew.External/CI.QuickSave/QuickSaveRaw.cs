using System;
using System.Collections.Generic;
using CI.QuickSave.Core.Settings;
using CI.QuickSave.Core.Storage;
using UnityEngine;

namespace CI.QuickSave;

public class QuickSaveRaw
{
	public static void SaveString(string filename, string content)
	{
		SaveString(filename, content, new QuickSaveSettings());
	}

	public static void SaveString(string filename, string content, QuickSaveSettings settings)
	{
		string contentToWrite;
		try
		{
			contentToWrite = Compression.Compress(content, settings.CompressionMode);
		}
		catch (Exception innerException)
		{
			throw new QuickSaveException("Compression failed", innerException);
		}
		if (settings.CompressionMode != CompressionMode.Gzip || settings.SecurityMode != SecurityMode.Base64)
		{
			try
			{
				contentToWrite = Cryptography.Encrypt(contentToWrite, settings.SecurityMode, settings.Password);
			}
			catch (Exception innerException2)
			{
				throw new QuickSaveException("Encryption failed", innerException2);
			}
		}
		if (!FileAccess.SaveString(filename, includesExtension: true, contentToWrite))
		{
			throw new QuickSaveException("Failed to write to file");
		}
	}

	public static void SaveBytes(string filename, byte[] content)
	{
		if (!FileAccess.SaveBytes(filename, includesExtension: true, content))
		{
			throw new QuickSaveException("Failed to write to file");
		}
	}

	public static string LoadString(string filename)
	{
		return LoadString(filename, new QuickSaveSettings());
	}

	public static string LoadString(string filename, QuickSaveSettings settings)
	{
		string content = FileAccess.LoadString(filename, includesExtension: true);
		if (content == null)
		{
			throw new QuickSaveException("Failed to load file");
		}
		if (settings.CompressionMode != CompressionMode.Gzip || settings.SecurityMode != SecurityMode.Base64)
		{
			try
			{
				content = Cryptography.Decrypt(content, settings.SecurityMode, settings.Password);
			}
			catch (Exception innerException)
			{
				throw new QuickSaveException("Decryption failed", innerException);
			}
		}
		try
		{
			return Compression.Decompress(content, settings.CompressionMode);
		}
		catch (Exception innerException2)
		{
			throw new QuickSaveException("Decompression failed", innerException2);
		}
	}

	public static byte[] LoadBytes(string filename)
	{
		return FileAccess.LoadBytes(filename, includesExtension: true) ?? throw new QuickSaveException("Failed to load file");
	}

	public static T LoadResource<T>(string filename) where T : global::UnityEngine.Object
	{
		return Resources.Load<T>(filename);
	}

	public static void Delete(string filename)
	{
		FileAccess.Delete(filename, includesExtension: true);
	}

	public static bool Exists(string filename)
	{
		return FileAccess.Exists(filename, includesExtension: true);
	}

	public static IEnumerable<string> GetAllFiles()
	{
		return FileAccess.Files(includeExtensions: false);
	}
}
