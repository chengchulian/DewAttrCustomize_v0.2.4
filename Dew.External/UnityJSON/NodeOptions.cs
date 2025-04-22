namespace UnityJSON;

public enum NodeOptions
{
	Default = 0,
	DontSerialize = 1,
	DontDeserialize = 2,
	SerializeNull = 4,
	IgnoreTypeMismatch = 8,
	IgnoreInstantiationError = 16,
	IgnoreDeserializationTypeErrors = 24,
	DontAssignNull = 32,
	ReplaceDeserialized = 64
}
