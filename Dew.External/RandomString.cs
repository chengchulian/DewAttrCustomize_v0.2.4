using System;
using System.Text;

public class RandomString
{
	public static string Generate(int size)
	{
		StringBuilder builder = new StringBuilder(size);
		Random random = new Random();
		char offsetLowerCase = 'a';
		char offsetUpperCase = 'A';
		for (int i = 0; i < size; i++)
		{
			char offset = ((random.Next(0, 2) != 0) ? offsetUpperCase : offsetLowerCase);
			char @char = (char)random.Next(offset, offset + 26);
			builder.Append(@char);
		}
		return builder.ToString();
	}
}
