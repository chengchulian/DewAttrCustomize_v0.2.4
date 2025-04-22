using System;

namespace DuloGames.UI;

[Flags]
public enum UISpellInfo_Flags
{
	Passive = 1,
	InstantCast = 2,
	PowerCostInPct = 4
}
