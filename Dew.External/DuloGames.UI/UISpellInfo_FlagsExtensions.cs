using System;

namespace DuloGames.UI;

public static class UISpellInfo_FlagsExtensions
{
	public static bool Has(this UISpellInfo_Flags type, UISpellInfo_Flags value)
	{
		try
		{
			return (type & value) == value;
		}
		catch
		{
			return false;
		}
	}

	public static bool Is(this UISpellInfo_Flags type, UISpellInfo_Flags value)
	{
		try
		{
			return type == value;
		}
		catch
		{
			return false;
		}
	}

	public static UISpellInfo_Flags Add(this UISpellInfo_Flags type, UISpellInfo_Flags value)
	{
		try
		{
			return type | value;
		}
		catch (Exception innerException)
		{
			throw new ArgumentException($"Could not append value from enumerated type '{typeof(UISpellInfo_Flags).Name}'.", innerException);
		}
	}

	public static UISpellInfo_Flags Remove(this UISpellInfo_Flags type, UISpellInfo_Flags value)
	{
		try
		{
			return type & ~value;
		}
		catch (Exception innerException)
		{
			throw new ArgumentException($"Could not remove value from enumerated type '{typeof(UISpellInfo_Flags).Name}'.", innerException);
		}
	}
}
