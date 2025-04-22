namespace Epic.OnlineServices.Sessions;

public struct SessionDetailsAttribute
{
	public AttributeData? Data { get; set; }

	public SessionAttributeAdvertisementType AdvertisementType { get; set; }

	internal void Set(ref SessionDetailsAttributeInternal other)
	{
		Data = other.Data;
		AdvertisementType = other.AdvertisementType;
	}
}
