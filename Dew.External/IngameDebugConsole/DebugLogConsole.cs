using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Mirror;
using UnityEngine;

namespace IngameDebugConsole;

public static class DebugLogConsole
{
	public delegate bool ParseFunction(string input, out object output);

	public static Action<string> ServerCommandHandler;

	public static Func<bool> GameContextValidator;

	public static Func<bool> CheatContextValidator;

	private static readonly List<ConsoleMethodInfo> methods;

	private static readonly List<ConsoleMethodInfo> matchingMethods;

	private static readonly Dictionary<Type, ParseFunction> parseFunctions;

	private static readonly Dictionary<Type, string> typeReadableNames;

	private static readonly List<string> commandArguments;

	private static readonly string[] inputDelimiters;

	internal static readonly CompareInfo caseInsensitiveComparer;

	static DebugLogConsole()
	{
		methods = new List<ConsoleMethodInfo>();
		matchingMethods = new List<ConsoleMethodInfo>(4);
		parseFunctions = new Dictionary<Type, ParseFunction>
		{
			{
				typeof(string),
				ParseString
			},
			{
				typeof(bool),
				ParseBool
			},
			{
				typeof(int),
				ParseInt
			},
			{
				typeof(uint),
				ParseUInt
			},
			{
				typeof(long),
				ParseLong
			},
			{
				typeof(ulong),
				ParseULong
			},
			{
				typeof(byte),
				ParseByte
			},
			{
				typeof(sbyte),
				ParseSByte
			},
			{
				typeof(short),
				ParseShort
			},
			{
				typeof(ushort),
				ParseUShort
			},
			{
				typeof(char),
				ParseChar
			},
			{
				typeof(float),
				ParseFloat
			},
			{
				typeof(double),
				ParseDouble
			},
			{
				typeof(decimal),
				ParseDecimal
			},
			{
				typeof(Vector2),
				ParseVector2
			},
			{
				typeof(Vector3),
				ParseVector3
			},
			{
				typeof(Vector4),
				ParseVector4
			},
			{
				typeof(Quaternion),
				ParseQuaternion
			},
			{
				typeof(Color),
				ParseColor
			},
			{
				typeof(Color32),
				ParseColor32
			},
			{
				typeof(Rect),
				ParseRect
			},
			{
				typeof(RectOffset),
				ParseRectOffset
			},
			{
				typeof(Bounds),
				ParseBounds
			},
			{
				typeof(GameObject),
				ParseGameObject
			},
			{
				typeof(Vector2Int),
				ParseVector2Int
			},
			{
				typeof(Vector3Int),
				ParseVector3Int
			},
			{
				typeof(RectInt),
				ParseRectInt
			},
			{
				typeof(BoundsInt),
				ParseBoundsInt
			}
		};
		typeReadableNames = new Dictionary<Type, string>
		{
			{
				typeof(string),
				"String"
			},
			{
				typeof(bool),
				"Boolean"
			},
			{
				typeof(int),
				"Integer"
			},
			{
				typeof(uint),
				"Unsigned Integer"
			},
			{
				typeof(long),
				"Long"
			},
			{
				typeof(ulong),
				"Unsigned Long"
			},
			{
				typeof(byte),
				"Byte"
			},
			{
				typeof(sbyte),
				"Short Byte"
			},
			{
				typeof(short),
				"Short"
			},
			{
				typeof(ushort),
				"Unsigned Short"
			},
			{
				typeof(char),
				"Char"
			},
			{
				typeof(float),
				"Float"
			},
			{
				typeof(double),
				"Double"
			},
			{
				typeof(decimal),
				"Decimal"
			}
		};
		commandArguments = new List<string>(8);
		inputDelimiters = new string[5] { "\"\"", "''", "{}", "()", "[]" };
		caseInsensitiveComparer = new CultureInfo("en-US").CompareInfo;
		AddCommand("help", "Prints all commands", LogAllCommands);
		AddCommand<string>("help", "Prints all matching commands", LogAllCommandsWithName);
		AddCommand("sysinfo", "Prints system information", LogSystemInfo);
		string[] ignoredAssemblies = new string[12]
		{
			"Unity", "System", "Mono.", "mscorlib", "netstandard", "TextMeshPro", "Microsoft.GeneratedCode", "I18N", "Boo.", "UnityScript.",
			"ICSharpCode.", "ExCSS.Unity"
		};
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		foreach (Assembly assembly in assemblies)
		{
			if (assembly.IsDynamic)
			{
				continue;
			}
			string assemblyName = assembly.GetName().Name;
			bool ignoreAssembly = false;
			for (int j = 0; j < ignoredAssemblies.Length; j++)
			{
				if (caseInsensitiveComparer.IsPrefix(assemblyName, ignoredAssemblies[j], CompareOptions.IgnoreCase))
				{
					ignoreAssembly = true;
					break;
				}
			}
			if (ignoreAssembly)
			{
				continue;
			}
			try
			{
				Type[] exportedTypes = assembly.GetExportedTypes();
				for (int k = 0; k < exportedTypes.Length; k++)
				{
					MethodInfo[] array = exportedTypes[k].GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
					foreach (MethodInfo method in array)
					{
						object[] customAttributes = method.GetCustomAttributes(typeof(ConsoleMethodAttribute), inherit: false);
						for (int m = 0; m < customAttributes.Length; m++)
						{
							if (customAttributes[m] is ConsoleMethodAttribute consoleMethod)
							{
								AddCommand(consoleMethod.Command, consoleMethod.Description, method, null, consoleMethod.ParameterNames);
							}
						}
						customAttributes = method.GetCustomAttributes(typeof(DewConsoleMethodAttribute), inherit: false);
						for (int m = 0; m < customAttributes.Length; m++)
						{
							if (customAttributes[m] is DewConsoleMethodAttribute consoleMethod2)
							{
								AddCommand(consoleMethod2.CustomName ?? method.Name, consoleMethod2.Description, method, null, null, consoleMethod2.Type);
							}
						}
					}
				}
			}
			catch (NotSupportedException)
			{
			}
			catch (FileNotFoundException)
			{
			}
			catch (ReflectionTypeLoadException)
			{
			}
			catch (Exception ex4)
			{
				Debug.LogError("Couldn't search assembly for [ConsoleMethod] attributes: " + assemblyName + "\n" + ex4.ToString());
			}
		}
	}

	public static void LogAllCommands()
	{
		int length = 25;
		for (int i = 0; i < methods.Count; i++)
		{
			if (methods[i].IsValid())
			{
				length += methods[i].signature.Length + 7;
			}
		}
		StringBuilder stringBuilder = new StringBuilder(length);
		stringBuilder.Append("Available commands:");
		for (int j = 0; j < methods.Count; j++)
		{
			if (methods[j].IsValid())
			{
				stringBuilder.Append("\n    - ").Append(methods[j].signature);
			}
		}
		Debug.Log(stringBuilder.ToString());
		if ((bool)DebugLogManager.Instance)
		{
			DebugLogManager.Instance.AdjustLatestPendingLog(autoExpand: true, stripStackTrace: true);
		}
	}

	public static void LogAllCommandsWithName(string commandName)
	{
		matchingMethods.Clear();
		FindCommands(commandName, allowSubstringMatching: false, matchingMethods);
		if (matchingMethods.Count == 0)
		{
			FindCommands(commandName, allowSubstringMatching: true, matchingMethods);
		}
		if (matchingMethods.Count == 0)
		{
			Debug.LogWarning("ERROR: can't find command '" + commandName + "'");
			return;
		}
		int commandsLength = 25;
		for (int i = 0; i < matchingMethods.Count; i++)
		{
			commandsLength += matchingMethods[i].signature.Length + 7;
		}
		StringBuilder stringBuilder = new StringBuilder(commandsLength);
		stringBuilder.Append("Matching commands:");
		for (int j = 0; j < matchingMethods.Count; j++)
		{
			stringBuilder.Append("\n    - ").Append(matchingMethods[j].signature);
		}
		Debug.Log(stringBuilder.ToString());
		if ((bool)DebugLogManager.Instance)
		{
			DebugLogManager.Instance.AdjustLatestPendingLog(autoExpand: true, stripStackTrace: true);
		}
	}

	public static void LogSystemInfo()
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		stringBuilder.Append("Rig: ").AppendSysInfoIfPresent(SystemInfo.deviceModel).AppendSysInfoIfPresent(SystemInfo.processorType)
			.AppendSysInfoIfPresent(SystemInfo.systemMemorySize, "MB RAM")
			.Append(SystemInfo.processorCount)
			.Append(" cores\n");
		stringBuilder.Append("OS: ").Append(SystemInfo.operatingSystem).Append("\n");
		stringBuilder.Append("GPU: ").Append(SystemInfo.graphicsDeviceName).Append(" ")
			.Append(SystemInfo.graphicsMemorySize)
			.Append("MB ")
			.Append(SystemInfo.graphicsDeviceVersion)
			.Append(SystemInfo.graphicsMultiThreaded ? " multi-threaded\n" : "\n");
		stringBuilder.Append("Data Path: ").Append(Application.dataPath).Append("\n");
		stringBuilder.Append("Persistent Data Path: ").Append(Application.persistentDataPath).Append("\n");
		stringBuilder.Append("StreamingAssets Path: ").Append(Application.streamingAssetsPath).Append("\n");
		stringBuilder.Append("Temporary Cache Path: ").Append(Application.temporaryCachePath).Append("\n");
		stringBuilder.Append("Device ID: ").Append(SystemInfo.deviceUniqueIdentifier).Append("\n");
		stringBuilder.Append("Max Texture Size: ").Append(SystemInfo.maxTextureSize).Append("\n");
		stringBuilder.Append("Max Cubemap Size: ").Append(SystemInfo.maxCubemapSize).Append("\n");
		stringBuilder.Append("Accelerometer: ").Append(SystemInfo.supportsAccelerometer ? "supported\n" : "not supported\n");
		stringBuilder.Append("Gyro: ").Append(SystemInfo.supportsGyroscope ? "supported\n" : "not supported\n");
		stringBuilder.Append("Location Service: ").Append(SystemInfo.supportsLocationService ? "supported\n" : "not supported\n");
		stringBuilder.Append("Compute Shaders: ").Append(SystemInfo.supportsComputeShaders ? "supported\n" : "not supported\n");
		stringBuilder.Append("Shadows: ").Append(SystemInfo.supportsShadows ? "supported\n" : "not supported\n");
		stringBuilder.Append("Instancing: ").Append(SystemInfo.supportsInstancing ? "supported\n" : "not supported\n");
		stringBuilder.Append("Motion Vectors: ").Append(SystemInfo.supportsMotionVectors ? "supported\n" : "not supported\n");
		stringBuilder.Append("3D Textures: ").Append(SystemInfo.supports3DTextures ? "supported\n" : "not supported\n");
		stringBuilder.Append("3D Render Textures: ").Append(SystemInfo.supports3DRenderTextures ? "supported\n" : "not supported\n");
		stringBuilder.Append("2D Array Textures: ").Append(SystemInfo.supports2DArrayTextures ? "supported\n" : "not supported\n");
		stringBuilder.Append("Cubemap Array Textures: ").Append(SystemInfo.supportsCubemapArrayTextures ? "supported" : "not supported");
		Debug.Log(stringBuilder.ToString());
		if ((bool)DebugLogManager.Instance)
		{
			DebugLogManager.Instance.AdjustLatestPendingLog(autoExpand: true, stripStackTrace: true);
		}
	}

	private static StringBuilder AppendSysInfoIfPresent(this StringBuilder sb, string info, string postfix = null)
	{
		if (info != "n/a")
		{
			sb.Append(info);
			if (postfix != null)
			{
				sb.Append(postfix);
			}
			sb.Append(" ");
		}
		return sb;
	}

	private static StringBuilder AppendSysInfoIfPresent(this StringBuilder sb, int info, string postfix = null)
	{
		if (info > 0)
		{
			sb.Append(info);
			if (postfix != null)
			{
				sb.Append(postfix);
			}
			sb.Append(" ");
		}
		return sb;
	}

	public static void AddCustomParameterType(Type type, ParseFunction parseFunction, string typeReadableName = null)
	{
		if (type == null)
		{
			Debug.LogError("Parameter type can't be null!");
			return;
		}
		if (parseFunction == null)
		{
			Debug.LogError("Parameter parseFunction can't be null!");
			return;
		}
		parseFunctions[type] = parseFunction;
		if (!string.IsNullOrEmpty(typeReadableName))
		{
			typeReadableNames[type] = typeReadableName;
		}
	}

	public static void RemoveCustomParameterType(Type type)
	{
		parseFunctions.Remove(type);
		typeReadableNames.Remove(type);
	}

	public static void AddCommandInstance(string command, string description, string methodName, object instance, params string[] parameterNames)
	{
		if (instance == null)
		{
			Debug.LogError("Instance can't be null!");
		}
		else
		{
			AddCommand(command, description, methodName, instance.GetType(), instance, parameterNames);
		}
	}

	public static void AddCommandStatic(string command, string description, string methodName, Type ownerType, params string[] parameterNames)
	{
		AddCommand(command, description, methodName, ownerType, null, parameterNames);
	}

	public static void AddCommand(string command, string description, Action method)
	{
		AddCommand(command, description, method.Method, method.Target, null);
	}

	public static void AddCommand<T1>(string command, string description, Action<T1> method)
	{
		AddCommand(command, description, method.Method, method.Target, null);
	}

	public static void AddCommand<T1>(string command, string description, Func<T1> method)
	{
		AddCommand(command, description, method.Method, method.Target, null);
	}

	public static void AddCommand<T1, T2>(string command, string description, Action<T1, T2> method)
	{
		AddCommand(command, description, method.Method, method.Target, null);
	}

	public static void AddCommand<T1, T2>(string command, string description, Func<T1, T2> method)
	{
		AddCommand(command, description, method.Method, method.Target, null);
	}

	public static void AddCommand<T1, T2, T3>(string command, string description, Action<T1, T2, T3> method)
	{
		AddCommand(command, description, method.Method, method.Target, null);
	}

	public static void AddCommand<T1, T2, T3>(string command, string description, Func<T1, T2, T3> method)
	{
		AddCommand(command, description, method.Method, method.Target, null);
	}

	public static void AddCommand<T1, T2, T3, T4>(string command, string description, Action<T1, T2, T3, T4> method)
	{
		AddCommand(command, description, method.Method, method.Target, null);
	}

	public static void AddCommand<T1, T2, T3, T4>(string command, string description, Func<T1, T2, T3, T4> method)
	{
		AddCommand(command, description, method.Method, method.Target, null);
	}

	public static void AddCommand<T1, T2, T3, T4, T5>(string command, string description, Func<T1, T2, T3, T4, T5> method)
	{
		AddCommand(command, description, method.Method, method.Target, null);
	}

	public static void AddCommand(string command, string description, Delegate method)
	{
		AddCommand(command, description, method.Method, method.Target, null);
	}

	public static void AddCommand<T1>(string command, string description, Action<T1> method, string parameterName)
	{
		AddCommand(command, description, method.Method, method.Target, new string[1] { parameterName });
	}

	public static void AddCommand<T1, T2>(string command, string description, Action<T1, T2> method, string parameterName1, string parameterName2)
	{
		AddCommand(command, description, method.Method, method.Target, new string[2] { parameterName1, parameterName2 });
	}

	public static void AddCommand<T1, T2>(string command, string description, Func<T1, T2> method, string parameterName)
	{
		AddCommand(command, description, method.Method, method.Target, new string[1] { parameterName });
	}

	public static void AddCommand<T1, T2, T3>(string command, string description, Action<T1, T2, T3> method, string parameterName1, string parameterName2, string parameterName3)
	{
		AddCommand(command, description, method.Method, method.Target, new string[3] { parameterName1, parameterName2, parameterName3 });
	}

	public static void AddCommand<T1, T2, T3>(string command, string description, Func<T1, T2, T3> method, string parameterName1, string parameterName2)
	{
		AddCommand(command, description, method.Method, method.Target, new string[2] { parameterName1, parameterName2 });
	}

	public static void AddCommand<T1, T2, T3, T4>(string command, string description, Action<T1, T2, T3, T4> method, string parameterName1, string parameterName2, string parameterName3, string parameterName4)
	{
		AddCommand(command, description, method.Method, method.Target, new string[4] { parameterName1, parameterName2, parameterName3, parameterName4 });
	}

	public static void AddCommand<T1, T2, T3, T4>(string command, string description, Func<T1, T2, T3, T4> method, string parameterName1, string parameterName2, string parameterName3)
	{
		AddCommand(command, description, method.Method, method.Target, new string[3] { parameterName1, parameterName2, parameterName3 });
	}

	public static void AddCommand<T1, T2, T3, T4, T5>(string command, string description, Func<T1, T2, T3, T4, T5> method, string parameterName1, string parameterName2, string parameterName3, string parameterName4)
	{
		AddCommand(command, description, method.Method, method.Target, new string[4] { parameterName1, parameterName2, parameterName3, parameterName4 });
	}

	public static void AddCommand(string command, string description, Delegate method, params string[] parameterNames)
	{
		AddCommand(command, description, method.Method, method.Target, parameterNames);
	}

	private static void AddCommand(string command, string description, string methodName, Type ownerType, object instance, string[] parameterNames)
	{
		MethodInfo method = ownerType.GetMethod(methodName, (BindingFlags)(0x30 | ((instance != null) ? 4 : 8)));
		if (method == null)
		{
			Debug.LogError(methodName + " does not exist in " + ownerType);
		}
		else
		{
			AddCommand(command, description, method, instance, parameterNames);
		}
	}

	private static void AddCommand(string command, string description, MethodInfo method, object instance, string[] parameterNames, CommandType tags = CommandType.Anywhere)
	{
		if (string.IsNullOrEmpty(command))
		{
			Debug.LogError("Command name can't be empty!");
			return;
		}
		command = command.Trim();
		if (command.IndexOf(' ') >= 0)
		{
			Debug.LogError("Command name can't contain whitespace: " + command);
			return;
		}
		ParameterInfo[] parameters = method.GetParameters();
		if (parameters == null)
		{
			parameters = new ParameterInfo[0];
		}
		Type[] parameterTypes = new Type[parameters.Length];
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].ParameterType.IsByRef)
			{
				Debug.LogError("Command can't have 'out' or 'ref' parameters");
				return;
			}
			Type parameterType = parameters[i].ParameterType;
			if (parseFunctions.ContainsKey(parameterType) || typeof(Component).IsAssignableFrom(parameterType) || parameterType.IsEnum || IsSupportedArrayType(parameterType))
			{
				parameterTypes[i] = parameterType;
				continue;
			}
			Debug.LogError(string.Concat("Parameter ", parameters[i].Name, "'s Type ", parameterType, " isn't supported"));
			return;
		}
		int commandIndex = FindCommandIndex(command);
		if (commandIndex < 0)
		{
			commandIndex = ~commandIndex;
		}
		else
		{
			int commandFirstIndex = commandIndex;
			int commandLastIndex = commandIndex;
			while (commandFirstIndex > 0 && caseInsensitiveComparer.Compare(methods[commandFirstIndex - 1].command, command, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0)
			{
				commandFirstIndex--;
			}
			for (; commandLastIndex < methods.Count - 1 && caseInsensitiveComparer.Compare(methods[commandLastIndex + 1].command, command, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0; commandLastIndex++)
			{
			}
			commandIndex = commandFirstIndex;
			for (int j = commandFirstIndex; j <= commandLastIndex; j++)
			{
				int parameterCountDiff = methods[j].parameterTypes.Length - parameterTypes.Length;
				if (parameterCountDiff > 0)
				{
					continue;
				}
				commandIndex = j + 1;
				if (parameterCountDiff == 0)
				{
					int k;
					for (k = 0; k < parameterTypes.Length && parameterTypes[k] == methods[j].parameterTypes[k]; k++)
					{
					}
					if (k >= parameterTypes.Length)
					{
						commandIndex = j;
						commandLastIndex--;
						methods.RemoveAt(j--);
					}
				}
			}
		}
		StringBuilder methodSignature = new StringBuilder(256);
		string[] parameterSignatures = new string[parameterTypes.Length];
		methodSignature.Append("<b>");
		methodSignature.Append(command);
		if (parameterTypes.Length != 0)
		{
			methodSignature.Append(" ");
			for (int l = 0; l < parameterTypes.Length; l++)
			{
				int parameterSignatureStartIndex = methodSignature.Length;
				methodSignature.Append("[").Append(GetTypeReadableName(parameterTypes[l])).Append(" ")
					.Append((parameterNames != null && l < parameterNames.Length && !string.IsNullOrEmpty(parameterNames[l])) ? parameterNames[l] : parameters[l].Name)
					.Append("]");
				if (l < parameterTypes.Length - 1)
				{
					methodSignature.Append(" ");
				}
				parameterSignatures[l] = methodSignature.ToString(parameterSignatureStartIndex, methodSignature.Length - parameterSignatureStartIndex);
			}
		}
		methodSignature.Append("</b>");
		if (!string.IsNullOrEmpty(description))
		{
			methodSignature.Append(": ").Append(description);
		}
		methods.Insert(commandIndex, new ConsoleMethodInfo(method, parameterTypes, instance, command, methodSignature.ToString(), parameterSignatures, tags));
	}

	public static void RemoveCommand(string command)
	{
		if (string.IsNullOrEmpty(command))
		{
			return;
		}
		for (int i = methods.Count - 1; i >= 0; i--)
		{
			if (caseInsensitiveComparer.Compare(methods[i].command, command, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0)
			{
				methods.RemoveAt(i);
			}
		}
	}

	public static void RemoveCommand(Action method)
	{
		RemoveCommand(method.Method);
	}

	public static void RemoveCommand<T1>(Action<T1> method)
	{
		RemoveCommand(method.Method);
	}

	public static void RemoveCommand<T1>(Func<T1> method)
	{
		RemoveCommand(method.Method);
	}

	public static void RemoveCommand<T1, T2>(Action<T1, T2> method)
	{
		RemoveCommand(method.Method);
	}

	public static void RemoveCommand<T1, T2>(Func<T1, T2> method)
	{
		RemoveCommand(method.Method);
	}

	public static void RemoveCommand<T1, T2, T3>(Action<T1, T2, T3> method)
	{
		RemoveCommand(method.Method);
	}

	public static void RemoveCommand<T1, T2, T3>(Func<T1, T2, T3> method)
	{
		RemoveCommand(method.Method);
	}

	public static void RemoveCommand<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method)
	{
		RemoveCommand(method.Method);
	}

	public static void RemoveCommand<T1, T2, T3, T4>(Func<T1, T2, T3, T4> method)
	{
		RemoveCommand(method.Method);
	}

	public static void RemoveCommand<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> method)
	{
		RemoveCommand(method.Method);
	}

	public static void RemoveCommand(Delegate method)
	{
		RemoveCommand(method.Method);
	}

	public static void RemoveCommand(MethodInfo method)
	{
		if (!(method != null))
		{
			return;
		}
		for (int i = methods.Count - 1; i >= 0; i--)
		{
			if (methods[i].method == method)
			{
				methods.RemoveAt(i);
			}
		}
	}

	public static string GetAutoCompleteCommand(string commandStart, string previousSuggestion)
	{
		int commandIndex = FindCommandIndex((!string.IsNullOrEmpty(previousSuggestion)) ? previousSuggestion : commandStart);
		if (commandIndex < 0)
		{
			commandIndex = ~commandIndex;
			if (commandIndex >= methods.Count || !caseInsensitiveComparer.IsPrefix(methods[commandIndex].command, commandStart, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace))
			{
				return null;
			}
			return methods[commandIndex].command;
		}
		for (int i = commandIndex + 1; i < methods.Count; i++)
		{
			if (caseInsensitiveComparer.Compare(methods[i].command, previousSuggestion, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) != 0)
			{
				if (!caseInsensitiveComparer.IsPrefix(methods[i].command, commandStart, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace))
				{
					break;
				}
				return methods[i].command;
			}
		}
		string result = null;
		int i2 = commandIndex - 1;
		while (i2 >= 0 && caseInsensitiveComparer.IsPrefix(methods[i2].command, commandStart, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace))
		{
			result = methods[i2].command;
			i2--;
		}
		return result;
	}

	public static string[] SplitByDelimiterWithQuotes(string input)
	{
		List<string> result = new List<string>();
		string pattern = ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
		string[] array = Regex.Split(input, pattern);
		for (int i = 0; i < array.Length; i++)
		{
			string trimmedItem = array[i].Trim();
			result.Add(trimmedItem);
		}
		return result.ToArray();
	}

	public static void ExecuteCommand(string command, bool isComingFromRemote = false)
	{
		if (command == null)
		{
			return;
		}
		command = command.Trim();
		if (command.Length == 0)
		{
			return;
		}
		string[] split = SplitByDelimiterWithQuotes(command);
		GameObject coroutiner;
		if (split.Length > 1)
		{
			if (isComingFromRemote)
			{
				throw new InvalidOperationException();
			}
			coroutiner = new GameObject("Runner - " + command);
			global::UnityEngine.Object.DontDestroyOnLoad(coroutiner);
			coroutiner.AddComponent<CoroutineRunner>().StartCoroutine(Routine());
			return;
		}
		commandArguments.Clear();
		FetchArgumentsFromCommand(command, commandArguments);
		matchingMethods.Clear();
		bool parameterCountMismatch = false;
		int commandIndex = FindCommandIndex(commandArguments[0]);
		if (commandIndex >= 0)
		{
			string _command = commandArguments[0];
			int commandLastIndex = commandIndex;
			while (commandIndex > 0 && caseInsensitiveComparer.Compare(methods[commandIndex - 1].command, _command, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0)
			{
				commandIndex--;
			}
			for (; commandLastIndex < methods.Count - 1 && caseInsensitiveComparer.Compare(methods[commandLastIndex + 1].command, _command, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0; commandLastIndex++)
			{
			}
			while (commandIndex <= commandLastIndex)
			{
				if (!methods[commandIndex].IsValid())
				{
					methods.RemoveAt(commandIndex);
					commandLastIndex--;
					continue;
				}
				if (methods[commandIndex].parameterTypes.Length == commandArguments.Count - 1)
				{
					matchingMethods.Add(methods[commandIndex]);
				}
				else
				{
					parameterCountMismatch = true;
				}
				commandIndex++;
			}
		}
		if (matchingMethods.Count == 0)
		{
			string _command2 = commandArguments[0];
			FindCommands(_command2, !parameterCountMismatch, matchingMethods);
			if (matchingMethods.Count == 0)
			{
				Debug.LogWarning("ERROR: can't find command '" + _command2 + "'");
				return;
			}
			int commandsLength = _command2.Length + 75;
			for (int i = 0; i < matchingMethods.Count; i++)
			{
				commandsLength += matchingMethods[i].signature.Length + 7;
			}
			StringBuilder stringBuilder = new StringBuilder(commandsLength);
			if (parameterCountMismatch)
			{
				stringBuilder.Append("ERROR: '").Append(_command2).Append("' doesn't take ")
					.Append(commandArguments.Count - 1)
					.Append(" parameter(s). Available command(s):");
			}
			else
			{
				stringBuilder.Append("ERROR: can't find command '").Append(_command2).Append("'. Did you mean:");
			}
			for (int j = 0; j < matchingMethods.Count; j++)
			{
				stringBuilder.Append("\n    - ").Append(matchingMethods[j].signature);
			}
			Debug.LogWarning(stringBuilder.ToString());
			if ((bool)DebugLogManager.Instance)
			{
				DebugLogManager.Instance.AdjustLatestPendingLog(autoExpand: true, stripStackTrace: true);
			}
			return;
		}
		ConsoleMethodInfo methodToExecute = null;
		object[] parameters = new object[commandArguments.Count - 1];
		string errorMessage = null;
		for (int k = 0; k < matchingMethods.Count; k++)
		{
			if (methodToExecute != null)
			{
				break;
			}
			ConsoleMethodInfo methodInfo = matchingMethods[k];
			bool success = true;
			for (int l = 0; l < methodInfo.parameterTypes.Length && success; l++)
			{
				try
				{
					string argument = commandArguments[l + 1];
					Type parameterType = methodInfo.parameterTypes[l];
					if (ParseArgument(argument, parameterType, out var val))
					{
						parameters[l] = val;
						continue;
					}
					success = false;
					errorMessage = "ERROR: couldn't parse " + argument + " to " + GetTypeReadableName(parameterType);
				}
				catch (Exception ex)
				{
					success = false;
					errorMessage = "ERROR: " + ex.ToString();
				}
			}
			if (success)
			{
				methodToExecute = methodInfo;
			}
		}
		if (methodToExecute == null)
		{
			Debug.LogWarning((!string.IsNullOrEmpty(errorMessage)) ? errorMessage : "ERROR: something went wrong");
		}
		else if (methodToExecute.type.NeedNetwork() && !NetworkServer.active && !NetworkClient.active)
		{
			if (!isComingFromRemote)
			{
				Debug.Log("You need to be in networked context to execute " + methodToExecute.command);
			}
		}
		else if (methodToExecute.type.NeedGame() && (GameContextValidator == null || !GameContextValidator()))
		{
			if (!isComingFromRemote)
			{
				Debug.Log("You need to be in a game to execute " + methodToExecute.command);
			}
		}
		else if (methodToExecute.type.IsCheat() && (CheatContextValidator == null || !CheatContextValidator()))
		{
			if (!isComingFromRemote)
			{
				Debug.Log("Cheat needs to be enabled on server to execute " + methodToExecute.command);
			}
		}
		else
		{
			if (isComingFromRemote && !methodToExecute.type.IsServer())
			{
				return;
			}
			if (!isComingFromRemote && methodToExecute.type.IsServer())
			{
				ServerCommandHandler?.Invoke(command);
				return;
			}
			object result = methodToExecute.method.Invoke(methodToExecute.instance, parameters);
			if (methodToExecute.method.ReturnType != typeof(void))
			{
				if (result == null || result.Equals(null))
				{
					Debug.Log("Returned: null");
				}
				else
				{
					Debug.Log("Returned: " + result.ToString());
				}
			}
		}
		IEnumerator Routine()
		{
			string[] array = split;
			for (int m = 0; m < array.Length; m++)
			{
				string cmd = array[m].Trim();
				float dur;
				if (cmd == "delay")
				{
					yield return null;
				}
				else if (cmd.StartsWith("delay ") && cmd.Split(" ").Length == 2 && float.TryParse(cmd.Split(" ")[1], out dur))
				{
					yield return new WaitForSecondsRealtime(dur);
				}
				else
				{
					try
					{
						ExecuteCommand(cmd);
					}
					catch (Exception)
					{
						global::UnityEngine.Object.Destroy(coroutiner);
						throw;
					}
				}
			}
			global::UnityEngine.Object.Destroy(coroutiner);
		}
	}

	public static void FetchArgumentsFromCommand(string command, List<string> commandArguments)
	{
		for (int i = 0; i < command.Length; i++)
		{
			if (!char.IsWhiteSpace(command[i]))
			{
				int delimiterIndex = IndexOfDelimiterGroup(command[i]);
				if (delimiterIndex >= 0)
				{
					int endIndex = IndexOfDelimiterGroupEnd(command, delimiterIndex, i + 1);
					commandArguments.Add(command.Substring(i + 1, endIndex - i - 1));
					i = ((endIndex < command.Length - 1 && command[endIndex + 1] == ',') ? (endIndex + 1) : endIndex);
				}
				else
				{
					int endIndex2 = IndexOfChar(command, ' ', i + 1);
					commandArguments.Add(command.Substring(i, (command[endIndex2 - 1] == ',') ? (endIndex2 - 1 - i) : (endIndex2 - i)));
					i = endIndex2;
				}
			}
		}
	}

	public static void FindCommands(string commandName, bool allowSubstringMatching, List<ConsoleMethodInfo> matchingCommands)
	{
		if (allowSubstringMatching)
		{
			for (int i = 0; i < methods.Count; i++)
			{
				if (methods[i].IsValid() && caseInsensitiveComparer.IndexOf(methods[i].command, commandName, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0)
				{
					matchingCommands.Add(methods[i]);
				}
			}
			return;
		}
		for (int j = 0; j < methods.Count; j++)
		{
			if (methods[j].IsValid() && caseInsensitiveComparer.Compare(methods[j].command, commandName, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0)
			{
				matchingCommands.Add(methods[j]);
			}
		}
	}

	internal static void GetCommandSuggestions(string command, List<ConsoleMethodInfo> matchingCommands, List<int> caretIndexIncrements, ref string commandName, out int numberOfParameters)
	{
		bool commandNameCalculated = false;
		bool commandNameFullyTyped = false;
		numberOfParameters = -1;
		for (int i = 0; i < command.Length; i++)
		{
			if (char.IsWhiteSpace(command[i]))
			{
				continue;
			}
			int delimiterIndex = IndexOfDelimiterGroup(command[i]);
			if (delimiterIndex >= 0)
			{
				int endIndex = IndexOfDelimiterGroupEnd(command, delimiterIndex, i + 1);
				if (!commandNameCalculated)
				{
					commandNameCalculated = true;
					commandNameFullyTyped = command.Length > endIndex;
					int commandNameLength = endIndex - i - 1;
					if (commandName == null || commandNameLength == 0 || commandName.Length != commandNameLength || caseInsensitiveComparer.IndexOf(command, commandName, i + 1, commandNameLength, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) != i + 1)
					{
						commandName = command.Substring(i + 1, commandNameLength);
					}
				}
				i = ((endIndex < command.Length - 1 && command[endIndex + 1] == ',') ? (endIndex + 1) : endIndex);
				caretIndexIncrements.Add(i + 1);
			}
			else
			{
				int endIndex2 = IndexOfChar(command, ' ', i + 1);
				if (!commandNameCalculated)
				{
					commandNameCalculated = true;
					commandNameFullyTyped = command.Length > endIndex2;
					int commandNameLength2 = ((command[endIndex2 - 1] == ',') ? (endIndex2 - 1 - i) : (endIndex2 - i));
					if (commandName == null || commandNameLength2 == 0 || commandName.Length != commandNameLength2 || caseInsensitiveComparer.IndexOf(command, commandName, i, commandNameLength2, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) != i)
					{
						commandName = command.Substring(i, commandNameLength2);
					}
				}
				i = endIndex2;
				caretIndexIncrements.Add(i);
			}
			numberOfParameters++;
		}
		if (!commandNameCalculated)
		{
			commandName = string.Empty;
		}
		if (string.IsNullOrEmpty(commandName))
		{
			return;
		}
		int commandIndex = FindCommandIndex(commandName);
		if (commandIndex < 0)
		{
			commandIndex = ~commandIndex;
		}
		int commandLastIndex = commandIndex;
		if (!commandNameFullyTyped)
		{
			if (commandIndex < methods.Count && caseInsensitiveComparer.IsPrefix(methods[commandIndex].command, commandName, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace))
			{
				while (commandIndex > 0 && caseInsensitiveComparer.IsPrefix(methods[commandIndex - 1].command, commandName, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace))
				{
					commandIndex--;
				}
				for (; commandLastIndex < methods.Count - 1 && caseInsensitiveComparer.IsPrefix(methods[commandLastIndex + 1].command, commandName, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace); commandLastIndex++)
				{
				}
			}
			else
			{
				commandLastIndex = -1;
			}
		}
		else if (commandIndex < methods.Count && caseInsensitiveComparer.Compare(methods[commandIndex].command, commandName, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0)
		{
			while (commandIndex > 0 && caseInsensitiveComparer.Compare(methods[commandIndex - 1].command, commandName, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0)
			{
				commandIndex--;
			}
			for (; commandLastIndex < methods.Count - 1 && caseInsensitiveComparer.Compare(methods[commandLastIndex + 1].command, commandName, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0; commandLastIndex++)
			{
			}
		}
		else
		{
			commandLastIndex = -1;
		}
		for (; commandIndex <= commandLastIndex; commandIndex++)
		{
			if (methods[commandIndex].parameterTypes.Length >= numberOfParameters)
			{
				matchingCommands.Add(methods[commandIndex]);
			}
		}
	}

	private static int IndexOfDelimiterGroup(char c)
	{
		for (int i = 0; i < inputDelimiters.Length; i++)
		{
			if (c == inputDelimiters[i][0])
			{
				return i;
			}
		}
		return -1;
	}

	private static int IndexOfDelimiterGroupEnd(string command, int delimiterIndex, int startIndex)
	{
		char startChar = inputDelimiters[delimiterIndex][0];
		char endChar = inputDelimiters[delimiterIndex][1];
		int depth = 1;
		for (int i = startIndex; i < command.Length; i++)
		{
			char c = command[i];
			if (c == endChar && --depth <= 0)
			{
				return i;
			}
			if (c == startChar)
			{
				depth++;
			}
		}
		return command.Length;
	}

	private static int IndexOfChar(string command, char c, int startIndex)
	{
		int result = command.IndexOf(c, startIndex);
		if (result < 0)
		{
			result = command.Length;
		}
		return result;
	}

	private static int FindCommandIndex(string command)
	{
		int min = 0;
		int max = methods.Count - 1;
		while (min <= max)
		{
			int mid = (min + max) / 2;
			int comparison = caseInsensitiveComparer.Compare(command, methods[mid].command, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
			if (comparison == 0)
			{
				return mid;
			}
			if (comparison < 0)
			{
				max = mid - 1;
			}
			else
			{
				min = mid + 1;
			}
		}
		return ~min;
	}

	public static bool IsSupportedArrayType(Type type)
	{
		if (type.IsArray)
		{
			if (type.GetArrayRank() != 1)
			{
				return false;
			}
			type = type.GetElementType();
		}
		else
		{
			if (!type.IsGenericType)
			{
				return false;
			}
			if (type.GetGenericTypeDefinition() != typeof(List<>))
			{
				return false;
			}
			type = type.GetGenericArguments()[0];
		}
		if (!parseFunctions.ContainsKey(type) && !typeof(Component).IsAssignableFrom(type))
		{
			return type.IsEnum;
		}
		return true;
	}

	public static string GetTypeReadableName(Type type)
	{
		if (typeReadableNames.TryGetValue(type, out var result))
		{
			return result;
		}
		if (IsSupportedArrayType(type))
		{
			Type elementType = (type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0]);
			if (typeReadableNames.TryGetValue(elementType, out result))
			{
				return result + "[]";
			}
			return elementType.Name + "[]";
		}
		return type.Name;
	}

	public static bool ParseArgument(string input, Type argumentType, out object output)
	{
		if (parseFunctions.TryGetValue(argumentType, out var parseFunction))
		{
			return parseFunction(input, out output);
		}
		if (typeof(Component).IsAssignableFrom(argumentType))
		{
			return ParseComponent(input, argumentType, out output);
		}
		if (argumentType.IsEnum)
		{
			return ParseEnum(input, argumentType, out output);
		}
		if (IsSupportedArrayType(argumentType))
		{
			return ParseArray(input, argumentType, out output);
		}
		output = null;
		return false;
	}

	public static bool ParseString(string input, out object output)
	{
		output = input;
		return true;
	}

	public static bool ParseBool(string input, out object output)
	{
		if (input == "1" || input.ToLowerInvariant() == "true")
		{
			output = true;
			return true;
		}
		if (input == "0" || input.ToLowerInvariant() == "false")
		{
			output = false;
			return true;
		}
		output = false;
		return false;
	}

	public static bool ParseInt(string input, out object output)
	{
		int value;
		bool result = int.TryParse(input, out value);
		output = value;
		return result;
	}

	public static bool ParseUInt(string input, out object output)
	{
		uint value;
		bool result = uint.TryParse(input, out value);
		output = value;
		return result;
	}

	public static bool ParseLong(string input, out object output)
	{
		long value;
		bool result = long.TryParse((!input.EndsWith("L", StringComparison.OrdinalIgnoreCase)) ? input : input.Substring(0, input.Length - 1), out value);
		output = value;
		return result;
	}

	public static bool ParseULong(string input, out object output)
	{
		ulong value;
		bool result = ulong.TryParse((!input.EndsWith("L", StringComparison.OrdinalIgnoreCase)) ? input : input.Substring(0, input.Length - 1), out value);
		output = value;
		return result;
	}

	public static bool ParseByte(string input, out object output)
	{
		byte value;
		bool result = byte.TryParse(input, out value);
		output = value;
		return result;
	}

	public static bool ParseSByte(string input, out object output)
	{
		sbyte value;
		bool result = sbyte.TryParse(input, out value);
		output = value;
		return result;
	}

	public static bool ParseShort(string input, out object output)
	{
		short value;
		bool result = short.TryParse(input, out value);
		output = value;
		return result;
	}

	public static bool ParseUShort(string input, out object output)
	{
		ushort value;
		bool result = ushort.TryParse(input, out value);
		output = value;
		return result;
	}

	public static bool ParseChar(string input, out object output)
	{
		char value;
		bool result = char.TryParse(input, out value);
		output = value;
		return result;
	}

	public static bool ParseFloat(string input, out object output)
	{
		float value;
		bool result = float.TryParse((!input.EndsWith("f", StringComparison.OrdinalIgnoreCase)) ? input : input.Substring(0, input.Length - 1), out value);
		output = value;
		return result;
	}

	public static bool ParseDouble(string input, out object output)
	{
		double value;
		bool result = double.TryParse((!input.EndsWith("f", StringComparison.OrdinalIgnoreCase)) ? input : input.Substring(0, input.Length - 1), out value);
		output = value;
		return result;
	}

	public static bool ParseDecimal(string input, out object output)
	{
		decimal value;
		bool result = decimal.TryParse((!input.EndsWith("f", StringComparison.OrdinalIgnoreCase)) ? input : input.Substring(0, input.Length - 1), out value);
		output = value;
		return result;
	}

	public static bool ParseVector2(string input, out object output)
	{
		return ParseVector(input, typeof(Vector2), out output);
	}

	public static bool ParseVector3(string input, out object output)
	{
		return ParseVector(input, typeof(Vector3), out output);
	}

	public static bool ParseVector4(string input, out object output)
	{
		return ParseVector(input, typeof(Vector4), out output);
	}

	public static bool ParseQuaternion(string input, out object output)
	{
		return ParseVector(input, typeof(Quaternion), out output);
	}

	public static bool ParseColor(string input, out object output)
	{
		return ParseVector(input, typeof(Color), out output);
	}

	public static bool ParseColor32(string input, out object output)
	{
		return ParseVector(input, typeof(Color32), out output);
	}

	public static bool ParseRect(string input, out object output)
	{
		return ParseVector(input, typeof(Rect), out output);
	}

	public static bool ParseRectOffset(string input, out object output)
	{
		return ParseVector(input, typeof(RectOffset), out output);
	}

	public static bool ParseBounds(string input, out object output)
	{
		return ParseVector(input, typeof(Bounds), out output);
	}

	public static bool ParseVector2Int(string input, out object output)
	{
		return ParseVector(input, typeof(Vector2Int), out output);
	}

	public static bool ParseVector3Int(string input, out object output)
	{
		return ParseVector(input, typeof(Vector3Int), out output);
	}

	public static bool ParseRectInt(string input, out object output)
	{
		return ParseVector(input, typeof(RectInt), out output);
	}

	public static bool ParseBoundsInt(string input, out object output)
	{
		return ParseVector(input, typeof(BoundsInt), out output);
	}

	public static bool ParseGameObject(string input, out object output)
	{
		output = ((input == "null") ? null : GameObject.Find(input));
		return true;
	}

	public static bool ParseComponent(string input, Type componentType, out object output)
	{
		GameObject gameObject = ((input == "null") ? null : GameObject.Find(input));
		output = (gameObject ? gameObject.GetComponent(componentType) : null);
		return true;
	}

	public static bool ParseEnum(string input, Type enumType, out object output)
	{
		int outputInt = 0;
		int operation = 0;
		int i;
		for (i = 0; i < input.Length; i++)
		{
			int orIndex = input.IndexOf('|', i);
			int andIndex = input.IndexOf('&', i);
			string enumStr = ((orIndex >= 0) ? input.Substring(i, ((andIndex < 0) ? orIndex : Mathf.Min(andIndex, orIndex)) - i).Trim() : input.Substring(i, ((andIndex < 0) ? input.Length : andIndex) - i).Trim());
			if (!int.TryParse(enumStr, out var value))
			{
				try
				{
					value = Convert.ToInt32(Enum.Parse(enumType, enumStr, ignoreCase: true));
				}
				catch
				{
					output = null;
					return false;
				}
			}
			outputInt = operation switch
			{
				0 => value, 
				1 => outputInt | value, 
				_ => outputInt & value, 
			};
			if (orIndex >= 0)
			{
				if (andIndex > orIndex)
				{
					operation = 2;
					i = andIndex;
				}
				else
				{
					operation = 1;
					i = orIndex;
				}
			}
			else if (andIndex >= 0)
			{
				operation = 2;
				i = andIndex;
			}
			else
			{
				i = input.Length;
			}
		}
		output = Enum.ToObject(enumType, outputInt);
		return true;
	}

	public static bool ParseArray(string input, Type arrayType, out object output)
	{
		List<string> valuesToParse = new List<string>(2);
		FetchArgumentsFromCommand(input, valuesToParse);
		IList result = (IList)(output = (IList)Activator.CreateInstance(arrayType, valuesToParse.Count));
		if (arrayType.IsArray)
		{
			Type elementType = arrayType.GetElementType();
			for (int i = 0; i < valuesToParse.Count; i++)
			{
				if (!ParseArgument(valuesToParse[i], elementType, out var obj))
				{
					return false;
				}
				result[i] = obj;
			}
		}
		else
		{
			Type elementType2 = arrayType.GetGenericArguments()[0];
			for (int j = 0; j < valuesToParse.Count; j++)
			{
				if (!ParseArgument(valuesToParse[j], elementType2, out var obj2))
				{
					return false;
				}
				result.Add(obj2);
			}
		}
		return true;
	}

	private static bool ParseVector(string input, Type vectorType, out object output)
	{
		List<string> tokens = new List<string>(input.Replace(',', ' ').Trim().Split(' '));
		for (int i = tokens.Count - 1; i >= 0; i--)
		{
			tokens[i] = tokens[i].Trim();
			if (tokens[i].Length == 0)
			{
				tokens.RemoveAt(i);
			}
		}
		float[] tokenValues = new float[tokens.Count];
		for (int j = 0; j < tokens.Count; j++)
		{
			if (!ParseFloat(tokens[j], out var val))
			{
				if (vectorType == typeof(Vector3))
				{
					output = Vector3.zero;
				}
				else if (vectorType == typeof(Vector2))
				{
					output = Vector2.zero;
				}
				else
				{
					output = Vector4.zero;
				}
				return false;
			}
			tokenValues[j] = (float)val;
		}
		if (vectorType == typeof(Vector3))
		{
			Vector3 result = Vector3.zero;
			for (int k = 0; k < tokenValues.Length && k < 3; k++)
			{
				result[k] = tokenValues[k];
			}
			output = result;
		}
		else if (vectorType == typeof(Vector2))
		{
			Vector2 result2 = Vector2.zero;
			for (int l = 0; l < tokenValues.Length && l < 2; l++)
			{
				result2[l] = tokenValues[l];
			}
			output = result2;
		}
		else if (vectorType == typeof(Vector4))
		{
			Vector4 result3 = Vector4.zero;
			for (int m = 0; m < tokenValues.Length && m < 4; m++)
			{
				result3[m] = tokenValues[m];
			}
			output = result3;
		}
		else if (vectorType == typeof(Quaternion))
		{
			Quaternion result4 = Quaternion.identity;
			for (int n = 0; n < tokenValues.Length && n < 4; n++)
			{
				result4[n] = tokenValues[n];
			}
			output = result4;
		}
		else if (vectorType == typeof(Color))
		{
			Color result5 = Color.black;
			for (int num = 0; num < tokenValues.Length && num < 4; num++)
			{
				result5[num] = tokenValues[num];
			}
			output = result5;
		}
		else if (vectorType == typeof(Color32))
		{
			Color32 result6 = new Color32(0, 0, 0, byte.MaxValue);
			if (tokenValues.Length != 0)
			{
				result6.r = (byte)Mathf.RoundToInt(tokenValues[0]);
			}
			if (tokenValues.Length > 1)
			{
				result6.g = (byte)Mathf.RoundToInt(tokenValues[1]);
			}
			if (tokenValues.Length > 2)
			{
				result6.b = (byte)Mathf.RoundToInt(tokenValues[2]);
			}
			if (tokenValues.Length > 3)
			{
				result6.a = (byte)Mathf.RoundToInt(tokenValues[3]);
			}
			output = result6;
		}
		else if (vectorType == typeof(Rect))
		{
			Rect result7 = Rect.zero;
			if (tokenValues.Length != 0)
			{
				result7.x = tokenValues[0];
			}
			if (tokenValues.Length > 1)
			{
				result7.y = tokenValues[1];
			}
			if (tokenValues.Length > 2)
			{
				result7.width = tokenValues[2];
			}
			if (tokenValues.Length > 3)
			{
				result7.height = tokenValues[3];
			}
			output = result7;
		}
		else if (vectorType == typeof(RectOffset))
		{
			RectOffset result8 = new RectOffset();
			if (tokenValues.Length != 0)
			{
				result8.left = Mathf.RoundToInt(tokenValues[0]);
			}
			if (tokenValues.Length > 1)
			{
				result8.right = Mathf.RoundToInt(tokenValues[1]);
			}
			if (tokenValues.Length > 2)
			{
				result8.top = Mathf.RoundToInt(tokenValues[2]);
			}
			if (tokenValues.Length > 3)
			{
				result8.bottom = Mathf.RoundToInt(tokenValues[3]);
			}
			output = result8;
		}
		else if (vectorType == typeof(Bounds))
		{
			Vector3 center = Vector3.zero;
			for (int num2 = 0; num2 < tokenValues.Length && num2 < 3; num2++)
			{
				center[num2] = tokenValues[num2];
			}
			Vector3 size = Vector3.zero;
			for (int num3 = 3; num3 < tokenValues.Length && num3 < 6; num3++)
			{
				size[num3 - 3] = tokenValues[num3];
			}
			output = new Bounds(center, size);
		}
		else if (vectorType == typeof(Vector3Int))
		{
			Vector3Int result9 = Vector3Int.zero;
			for (int num4 = 0; num4 < tokenValues.Length && num4 < 3; num4++)
			{
				result9[num4] = Mathf.RoundToInt(tokenValues[num4]);
			}
			output = result9;
		}
		else if (vectorType == typeof(Vector2Int))
		{
			Vector2Int result10 = Vector2Int.zero;
			for (int num5 = 0; num5 < tokenValues.Length && num5 < 2; num5++)
			{
				result10[num5] = Mathf.RoundToInt(tokenValues[num5]);
			}
			output = result10;
		}
		else if (vectorType == typeof(RectInt))
		{
			RectInt result11 = default(RectInt);
			if (tokenValues.Length != 0)
			{
				result11.x = Mathf.RoundToInt(tokenValues[0]);
			}
			if (tokenValues.Length > 1)
			{
				result11.y = Mathf.RoundToInt(tokenValues[1]);
			}
			if (tokenValues.Length > 2)
			{
				result11.width = Mathf.RoundToInt(tokenValues[2]);
			}
			if (tokenValues.Length > 3)
			{
				result11.height = Mathf.RoundToInt(tokenValues[3]);
			}
			output = result11;
		}
		else
		{
			if (!(vectorType == typeof(BoundsInt)))
			{
				output = null;
				return false;
			}
			Vector3Int center2 = Vector3Int.zero;
			for (int num6 = 0; num6 < tokenValues.Length && num6 < 3; num6++)
			{
				center2[num6] = Mathf.RoundToInt(tokenValues[num6]);
			}
			Vector3Int size2 = Vector3Int.zero;
			for (int num7 = 3; num7 < tokenValues.Length && num7 < 6; num7++)
			{
				size2[num7 - 3] = Mathf.RoundToInt(tokenValues[num7]);
			}
			output = new BoundsInt(center2, size2);
		}
		return true;
	}
}
