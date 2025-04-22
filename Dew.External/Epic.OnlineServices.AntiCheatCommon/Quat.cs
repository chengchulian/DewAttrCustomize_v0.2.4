namespace Epic.OnlineServices.AntiCheatCommon;

public struct Quat
{
	public float w { get; set; }

	public float x { get; set; }

	public float y { get; set; }

	public float z { get; set; }

	internal void Set(ref QuatInternal other)
	{
		w = other.w;
		x = other.x;
		y = other.y;
		z = other.z;
	}
}
