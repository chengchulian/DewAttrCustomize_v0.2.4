namespace UnityJSON;

public static class OptionsExtensions
{
	public static bool IsSerialized(this NodeOptions options)
	{
		return (options & NodeOptions.DontSerialize) == 0;
	}

	public static bool IsDeserialized(this NodeOptions options)
	{
		return (options & NodeOptions.DontDeserialize) == 0;
	}

	public static bool ShouldSerializeNull(this NodeOptions options)
	{
		return (options & NodeOptions.SerializeNull) == NodeOptions.SerializeNull;
	}

	public static bool ShouldIgnoreTypeMismatch(this NodeOptions options)
	{
		return (options & NodeOptions.IgnoreTypeMismatch) == NodeOptions.IgnoreTypeMismatch;
	}

	public static bool ShouldIgnoreUnknownType(this NodeOptions options)
	{
		return (options & NodeOptions.IgnoreInstantiationError) == NodeOptions.IgnoreInstantiationError;
	}

	public static bool ShouldAssignNull(this NodeOptions options)
	{
		return (options & NodeOptions.DontAssignNull) == 0;
	}

	public static bool ShouldReplaceWithDeserialized(this NodeOptions options)
	{
		return (options & NodeOptions.ReplaceDeserialized) == NodeOptions.ReplaceDeserialized;
	}

	public static bool ShouldIgnoreProperties(this ObjectOptions options)
	{
		return (options & ObjectOptions.IgnoreProperties) == ObjectOptions.IgnoreProperties;
	}

	public static bool ShouldIgnoreStatic(this ObjectOptions options)
	{
		return (options & ObjectOptions.IncludeStatic) == 0;
	}

	public static bool ShouldThrowAtUnknownKey(this ObjectOptions options)
	{
		return (options & ObjectOptions.IgnoreUnknownKey) == 0;
	}

	public static bool ShouldUseTupleFormat(this ObjectOptions options)
	{
		return (options & ObjectOptions.TupleFormat) == ObjectOptions.TupleFormat;
	}

	public static bool SupportsString(this ObjectTypes types)
	{
		return (types & ObjectTypes.String) == ObjectTypes.String;
	}

	public static bool SupportsBool(this ObjectTypes types)
	{
		return (types & ObjectTypes.Bool) == ObjectTypes.Bool;
	}

	public static bool SupportsNumber(this ObjectTypes types)
	{
		return (types & ObjectTypes.Number) == ObjectTypes.Number;
	}

	public static bool SupportsArray(this ObjectTypes types)
	{
		return (types & ObjectTypes.Array) == ObjectTypes.Array;
	}

	public static bool SupportsDictionary(this ObjectTypes types)
	{
		return (types & ObjectTypes.Dictionary) == ObjectTypes.Dictionary;
	}

	public static bool SupportsCustom(this ObjectTypes types)
	{
		return (types & ObjectTypes.Custom) == ObjectTypes.Custom;
	}
}
