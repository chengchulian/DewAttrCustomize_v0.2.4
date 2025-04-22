using System;

namespace IngameDebugConsole;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class ConsoleMethodAttribute : Attribute
{
	private string m_command;

	private string m_description;

	private string[] m_parameterNames;

	public string Command => m_command;

	public string Description => m_description;

	public string[] ParameterNames => m_parameterNames;

	public ConsoleMethodAttribute(string command, string description, params string[] parameterNames)
	{
		m_command = command;
		m_description = description;
		m_parameterNames = parameterNames;
	}
}
