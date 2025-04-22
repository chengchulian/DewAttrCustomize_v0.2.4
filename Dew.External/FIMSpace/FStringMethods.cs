namespace FIMSpace;

public static class FStringMethods
{
	public static string IntToString(this int value, int signs)
	{
		string output = value.ToString();
		int missingZeros = signs - output.Length;
		if (missingZeros > 0)
		{
			string missing = "0";
			for (int i = 1; i < missingZeros; i++)
			{
				missing += 0;
			}
			output = missing + output;
		}
		return output;
	}

	public static string CapitalizeOnlyFirstLetter(this string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		return text[0].ToString().ToUpper() + ((text.Length > 1) ? text.Substring(1) : "");
	}

	public static string CapitalizeFirstLetter(this string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		return text[0].ToString().ToUpper() + text.Substring(1);
	}

	public static string ReplaceSpacesWithUnderline(this string text)
	{
		if (text.Contains(" "))
		{
			text = text.Replace(" ", "_");
		}
		return text;
	}

	public static string GetEndOfStringFromSeparator(this string source, char[] separators, int which = 1, bool fromEnd = false)
	{
		bool separated = false;
		int counter = 0;
		int steps = 0;
		int i = 0;
		for (i = source.Length - 1; i >= 0; i--)
		{
			steps++;
			for (int c = 0; c < separators.Length; c++)
			{
				if (source[i] == separators[c])
				{
					counter++;
					if (counter == which)
					{
						i++;
						separated = true;
						break;
					}
				}
			}
			if (separated)
			{
				break;
			}
		}
		if (separated)
		{
			if (!fromEnd)
			{
				return source.Substring(0, source.Length - steps);
			}
			return source.Substring(i, source.Length - i);
		}
		return "";
	}

	public static string GetEndOfStringFromStringSeparator(this string source, string[] separators, int which = 1, bool rest = false)
	{
		bool separated = false;
		int counter = 0;
		int steps = 0;
		int i = 0;
		for (i = 0; i < source.Length; i++)
		{
			steps++;
			for (int c = 0; c < separators.Length && i + separators[c].Length <= source.Length; c++)
			{
				if (source.Substring(i, separators[c].Length) == separators[c])
				{
					counter++;
					if (counter == which)
					{
						i++;
						i += separators[c].Length - 1;
						separated = true;
						break;
					}
				}
			}
			if (separated)
			{
				break;
			}
		}
		if (separated)
		{
			if (rest)
			{
				return source.Substring(0, source.Length - steps);
			}
			return source.Substring(i, source.Length - i);
		}
		return "";
	}
}
