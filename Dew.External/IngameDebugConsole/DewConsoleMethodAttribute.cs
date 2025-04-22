using System;

namespace IngameDebugConsole;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class DewConsoleMethodAttribute : Attribute
{
	public readonly string Description;

	public readonly CommandType Type;

	public readonly string CustomName;

	public DewConsoleMethodAttribute(string description)
	{
		Description = description;
		Type = CommandType.Anywhere;
		CustomName = null;
	}

	public DewConsoleMethodAttribute(string description, CommandType type)
	{
		Description = description;
		Type = type;
		CustomName = null;
	}

	public DewConsoleMethodAttribute(string description, string customName)
	{
		Description = description;
		Type = CommandType.Anywhere;
		CustomName = customName;
	}

	public DewConsoleMethodAttribute(string description, string customName, CommandType type)
	{
		Description = description;
		Type = type;
		CustomName = customName;
	}
}
