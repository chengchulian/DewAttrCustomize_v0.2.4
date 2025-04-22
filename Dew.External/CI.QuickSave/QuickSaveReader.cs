using System;
using CI.QuickSave.Core.Serialisers;

namespace CI.QuickSave;

public class QuickSaveReader : QuickSaveBase
{
	private QuickSaveReader(string root, QuickSaveSettings settings)
	{
		_root = root;
		_settings = settings;
	}

	public static QuickSaveReader Create(string root)
	{
		return Create(root, new QuickSaveSettings());
	}

	public static QuickSaveReader Create(string root, QuickSaveSettings settings)
	{
		QuickSaveReader quickSaveReader = new QuickSaveReader(root, settings);
		quickSaveReader.Load(rootMightNotExist: false);
		return quickSaveReader;
	}

	public T Read<T>(string key)
	{
		if (!Exists(key))
		{
			throw new QuickSaveException("Key does not exists");
		}
		try
		{
			return JsonSerialiser.DeserialiseKey<T>(key, _items);
		}
		catch
		{
			throw new QuickSaveException("Deserialisation failed");
		}
	}

	public QuickSaveReader Read<T>(string key, Action<T> result)
	{
		if (!Exists(key))
		{
			throw new QuickSaveException("Key does not exists");
		}
		try
		{
			result(JsonSerialiser.DeserialiseKey<T>(key, _items));
			return this;
		}
		catch
		{
			throw new QuickSaveException("Deserialisation failed");
		}
	}

	public bool TryRead<T>(string key, out T result)
	{
		result = default(T);
		if (!Exists(key))
		{
			return false;
		}
		try
		{
			result = JsonSerialiser.DeserialiseKey<T>(key, _items);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public void Reload()
	{
		Load(rootMightNotExist: false);
	}
}
