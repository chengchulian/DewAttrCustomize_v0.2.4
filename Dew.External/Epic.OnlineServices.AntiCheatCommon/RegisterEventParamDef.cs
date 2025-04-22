namespace Epic.OnlineServices.AntiCheatCommon;

public struct RegisterEventParamDef
{
	public Utf8String ParamName { get; set; }

	public AntiCheatCommonEventParamType ParamType { get; set; }

	internal void Set(ref RegisterEventParamDefInternal other)
	{
		ParamName = other.ParamName;
		ParamType = other.ParamType;
	}
}
