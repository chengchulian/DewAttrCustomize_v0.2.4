namespace Epic.OnlineServices.Mods;

public struct ModInfo
{
	public ModIdentifier[] Mods { get; set; }

	public ModEnumerationType Type { get; set; }

	internal void Set(ref ModInfoInternal other)
	{
		Mods = other.Mods;
		Type = other.Type;
	}
}
