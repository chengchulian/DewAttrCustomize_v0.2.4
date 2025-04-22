using CI.QuickSave.Core.Serialisers;

namespace CI.QuickSave;

public class QuickSaveWriter : QuickSaveBase
{
	private QuickSaveWriter(string root, QuickSaveSettings settings)
	{
		_root = root;
		_settings = settings;
	}

	public static QuickSaveWriter Create(string root)
	{
		return Create(root, new QuickSaveSettings());
	}

	public static QuickSaveWriter Create(string root, QuickSaveSettings settings)
	{
		QuickSaveWriter quickSaveWriter = new QuickSaveWriter(root, settings);
		quickSaveWriter.Load(rootMightNotExist: true);
		return quickSaveWriter;
	}

	public QuickSaveWriter Write<T>(string key, T value)
	{
		if (Exists(key))
		{
			_items.Remove(key);
		}
		_items.Add(key, JsonSerialiser.SerialiseKey(value));
		return this;
	}

	public void Delete(string key)
	{
		if (Exists(key))
		{
			_items.Remove(key);
		}
	}

	public void Commit()
	{
		Save();
	}

	public bool TryCommit()
	{
		try
		{
			Save();
			return true;
		}
		catch
		{
			return false;
		}
	}
}
