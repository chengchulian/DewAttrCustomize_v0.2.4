using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CI.QuickSave.Core.Storage;

public static class FileAccess
{
	private const string _defaultExtension = ".json";

	public static string BasePath => Path.Combine(QuickSaveGlobalSettings.StorageLocation, "QuickSave");

	public static bool SaveString(string filename, bool includesExtension, string value)
	{
		filename = GetFilenameWithExtension(filename, includesExtension);
		try
		{
			CreateRootFolder();
			using (StreamWriter writer = new StreamWriter(Path.Combine(BasePath, filename)))
			{
				writer.Write(value);
			}
			return true;
		}
		catch
		{
		}
		return false;
	}

	public static bool SaveBytes(string filename, bool includesExtension, byte[] value)
	{
		filename = GetFilenameWithExtension(filename, includesExtension);
		try
		{
			CreateRootFolder();
			using (FileStream fileStream = new FileStream(Path.Combine(BasePath, filename), FileMode.Create))
			{
				fileStream.Write(value, 0, value.Length);
			}
			return true;
		}
		catch
		{
		}
		return false;
	}

	public static string LoadString(string filename, bool includesExtension)
	{
		filename = GetFilenameWithExtension(filename, includesExtension);
		try
		{
			CreateRootFolder();
			if (Exists(filename, includesExtension: true))
			{
				using (StreamReader reader = new StreamReader(Path.Combine(BasePath, filename)))
				{
					return reader.ReadToEnd();
				}
			}
		}
		catch
		{
		}
		return null;
	}

	public static byte[] LoadBytes(string filename, bool includesExtension)
	{
		filename = GetFilenameWithExtension(filename, includesExtension);
		try
		{
			CreateRootFolder();
			if (Exists(filename, includesExtension: true))
			{
				using (FileStream fileStream = new FileStream(Path.Combine(BasePath, filename), FileMode.Open))
				{
					byte[] buffer = new byte[fileStream.Length];
					fileStream.Read(buffer, 0, buffer.Length);
					return buffer;
				}
			}
		}
		catch
		{
		}
		return null;
	}

	public static void Delete(string filename, bool includesExtension)
	{
		filename = GetFilenameWithExtension(filename, includesExtension);
		try
		{
			CreateRootFolder();
			File.Delete(Path.Combine(BasePath, filename));
		}
		catch
		{
		}
	}

	public static bool Exists(string filename, bool includesExtension)
	{
		filename = GetFilenameWithExtension(filename, includesExtension);
		return File.Exists(Path.Combine(BasePath, filename));
	}

	public static IEnumerable<string> Files(bool includeExtensions)
	{
		try
		{
			CreateRootFolder();
			if (includeExtensions)
			{
				return from x in Directory.GetFiles(BasePath, "*.json")
					select Path.GetFileName(x);
			}
			return from x in Directory.GetFiles(BasePath, "*.json")
				select Path.GetFileNameWithoutExtension(x);
		}
		catch
		{
		}
		return new List<string>();
	}

	private static string GetFilenameWithExtension(string filename, bool includesExtension)
	{
		if (!includesExtension)
		{
			return filename + ".json";
		}
		return filename;
	}

	private static void CreateRootFolder()
	{
		if (!Directory.Exists(BasePath))
		{
			Directory.CreateDirectory(BasePath);
		}
	}
}
