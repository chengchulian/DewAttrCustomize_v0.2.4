using CI.QuickSave.Core.Serialisers;
using Newtonsoft.Json;
using UnityEngine;

namespace CI.QuickSave;

public static class QuickSaveGlobalSettings
{
	public static string StorageLocation { get; set; } = Application.persistentDataPath;

	public static void RegisterConverter(JsonConverter converter)
	{
		JsonSerialiser.RegisterConverter(converter);
	}
}
