using UnityEngine;

namespace DuloGames.UI;

public class UIItemQualityColor
{
	public const string Poor = "9d9d9dff";

	public const string Common = "ffffffff";

	public const string Uncommon = "1eff00ff";

	public const string Rare = "0070ffff";

	public const string Epic = "a335eeff";

	public const string Legendary = "ff8000ff";

	public static string GetHexColor(UIItemQuality quality)
	{
		return quality switch
		{
			UIItemQuality.Poor => "9d9d9dff", 
			UIItemQuality.Common => "ffffffff", 
			UIItemQuality.Uncommon => "1eff00ff", 
			UIItemQuality.Rare => "0070ffff", 
			UIItemQuality.Epic => "a335eeff", 
			UIItemQuality.Legendary => "ff8000ff", 
			_ => "9d9d9dff", 
		};
	}

	public static Color GetColor(UIItemQuality quality)
	{
		return CommonColorBuffer.StringToColor(GetHexColor(quality));
	}
}
