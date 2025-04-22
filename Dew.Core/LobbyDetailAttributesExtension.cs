using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;

public static class LobbyDetailAttributesExtension
{
	public static AttributeDataValue ToAttrDataValue(this object o)
	{
		if (o is string s)
		{
			AttributeDataValue result = default(AttributeDataValue);
			result.AsUtf8 = s;
			return result;
		}
		if (o is double d)
		{
			AttributeDataValue result = default(AttributeDataValue);
			result.AsDouble = d;
			return result;
		}
		if (o is float f)
		{
			AttributeDataValue result = default(AttributeDataValue);
			result.AsDouble = f;
			return result;
		}
		if (o is int i)
		{
			AttributeDataValue result = default(AttributeDataValue);
			result.AsInt64 = i;
			return result;
		}
		if (o is long l)
		{
			AttributeDataValue result = default(AttributeDataValue);
			result.AsInt64 = l;
			return result;
		}
		if (o is bool b)
		{
			AttributeDataValue result = default(AttributeDataValue);
			result.AsBool = b;
			return result;
		}
		throw new ArgumentException("o");
	}

	public static string GetAttributeString(this LobbyDetails lobby, string key)
	{
		LobbyDetailsCopyAttributeByKeyOptions lobbyDetailsCopyAttributeByKeyOptions = default(LobbyDetailsCopyAttributeByKeyOptions);
		lobbyDetailsCopyAttributeByKeyOptions.AttrKey = key;
		LobbyDetailsCopyAttributeByKeyOptions opt = lobbyDetailsCopyAttributeByKeyOptions;
		if (lobby.CopyAttributeByKey(ref opt, out var attrName) == Result.Success && attrName.HasValue && attrName.Value.Data.HasValue)
		{
			return attrName.Value.Data.Value.Value.AsUtf8;
		}
		return "";
	}

	public static long GetAttributeLong(this LobbyDetails lobby, string key)
	{
		LobbyDetailsCopyAttributeByKeyOptions lobbyDetailsCopyAttributeByKeyOptions = default(LobbyDetailsCopyAttributeByKeyOptions);
		lobbyDetailsCopyAttributeByKeyOptions.AttrKey = key;
		LobbyDetailsCopyAttributeByKeyOptions opt = lobbyDetailsCopyAttributeByKeyOptions;
		if (lobby.CopyAttributeByKey(ref opt, out var attrName) == Result.Success && attrName.HasValue && attrName.Value.Data.HasValue)
		{
			return attrName.Value.Data.Value.Value.AsInt64.GetValueOrDefault();
		}
		return 0L;
	}

	public static bool GetAttributeBool(this LobbyDetails lobby, string key)
	{
		LobbyDetailsCopyAttributeByKeyOptions lobbyDetailsCopyAttributeByKeyOptions = default(LobbyDetailsCopyAttributeByKeyOptions);
		lobbyDetailsCopyAttributeByKeyOptions.AttrKey = key;
		LobbyDetailsCopyAttributeByKeyOptions opt = lobbyDetailsCopyAttributeByKeyOptions;
		if (lobby.CopyAttributeByKey(ref opt, out var attrName) == Result.Success && attrName.HasValue && attrName.Value.Data.HasValue)
		{
			return attrName.Value.Data.Value.Value.AsBool == true;
		}
		return false;
	}
}
