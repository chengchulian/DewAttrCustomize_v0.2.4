using UnityEngine;

namespace DuloGames.UI;

public class Test_UIEquipSlot_Assign : MonoBehaviour
{
	[SerializeField]
	private UIEquipSlot slot;

	[SerializeField]
	private int assignItem;

	private void Awake()
	{
		if (slot == null)
		{
			slot = GetComponent<UIEquipSlot>();
		}
	}

	private void Start()
	{
		if (slot == null || UIItemDatabase.Instance == null)
		{
			Destruct();
			return;
		}
		slot.Assign(UIItemDatabase.Instance.GetByID(assignItem));
		Destruct();
	}

	private void Destruct()
	{
		Object.DestroyImmediate(this);
	}
}
