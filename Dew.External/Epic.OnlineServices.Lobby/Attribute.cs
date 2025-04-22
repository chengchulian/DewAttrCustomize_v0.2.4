namespace Epic.OnlineServices.Lobby;

public struct Attribute
{
	public AttributeData? Data { get; set; }

	public LobbyAttributeVisibility Visibility { get; set; }

	internal void Set(ref AttributeInternal other)
	{
		Data = other.Data;
		Visibility = other.Visibility;
	}
}
