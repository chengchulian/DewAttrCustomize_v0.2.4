using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave.Core.Serialisers;
using CI.QuickSave.Core.Settings;
using CI.QuickSave.Core.Storage;
using Newtonsoft.Json.Linq;

namespace CI.QuickSave;

public abstract class QuickSaveBase
{
	protected JObject _items;

	protected string _root;

	protected QuickSaveSettings _settings;

	public bool Exists(string key)
	{
		return _items[key] != null;
	}

	public IEnumerable<string> GetAllKeys()
	{
		return (from x in _items.Properties()
			select x.Name).ToList();
	}

	protected void Load(bool rootMightNotExist)
	{
		string json = FileAccess.LoadString(_root, includesExtension: false);
		if (string.IsNullOrEmpty(json))
		{
			if (rootMightNotExist)
			{
				_items = new JObject();
				return;
			}
			throw new QuickSaveException("Root does not exist");
		}
		if (_settings.CompressionMode != CompressionMode.Gzip || _settings.SecurityMode != SecurityMode.Base64)
		{
			try
			{
				json = Cryptography.Decrypt(json, _settings.SecurityMode, _settings.Password);
			}
			catch (Exception innerException)
			{
				throw new QuickSaveException("Decryption failed", innerException);
			}
		}
		try
		{
			json = Compression.Decompress(json, _settings.CompressionMode);
		}
		catch (Exception innerException2)
		{
			throw new QuickSaveException("Decompression failed", innerException2);
		}
		try
		{
			_items = JObject.Parse(json);
		}
		catch (Exception innerException3)
		{
			throw new QuickSaveException("Deserialisation failed", innerException3);
		}
	}

	protected void Save()
	{
		string json;
		try
		{
			json = JsonSerialiser.Serialise(_items);
		}
		catch (Exception innerException)
		{
			throw new QuickSaveException("Serialisation failed", innerException);
		}
		try
		{
			json = Compression.Compress(json, _settings.CompressionMode);
		}
		catch (Exception innerException2)
		{
			throw new QuickSaveException("Compression failed", innerException2);
		}
		if (_settings.CompressionMode != CompressionMode.Gzip || _settings.SecurityMode != SecurityMode.Base64)
		{
			try
			{
				json = Cryptography.Encrypt(json, _settings.SecurityMode, _settings.Password);
			}
			catch (Exception innerException3)
			{
				throw new QuickSaveException("Encryption failed", innerException3);
			}
		}
		if (!FileAccess.SaveString(_root, includesExtension: false, json))
		{
			throw new QuickSaveException("Failed to write to file");
		}
	}
}
