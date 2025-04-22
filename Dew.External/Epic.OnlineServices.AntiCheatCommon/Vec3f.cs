namespace Epic.OnlineServices.AntiCheatCommon;

public struct Vec3f
{
	public float x { get; set; }

	public float y { get; set; }

	public float z { get; set; }

	internal void Set(ref Vec3fInternal other)
	{
		x = other.x;
		y = other.y;
		z = other.z;
	}
}
