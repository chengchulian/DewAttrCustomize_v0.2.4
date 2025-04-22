using UnityEngine;

namespace DuloGames.UI;

public interface IUIItemSlot
{
	UIItemInfo GetItemInfo();

	bool Assign(UIItemInfo itemInfo, Object source);

	void Unassign();
}
