using UnityEngine;

namespace DuloGames.UI;

public class Test_UIItemSlot_AssignAll : MonoBehaviour
{
	[SerializeField]
	private Transform m_Container;

	private void Start()
	{
		if (m_Container == null || UIItemDatabase.Instance == null)
		{
			Destruct();
			return;
		}
		UIItemSlot[] slots = m_Container.gameObject.GetComponentsInChildren<UIItemSlot>();
		UIItemInfo[] items = UIItemDatabase.Instance.items;
		if (slots.Length != 0 && items.Length != 0)
		{
			UIItemSlot[] array = slots;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Assign(items[Random.Range(0, items.Length)]);
			}
		}
		Destruct();
	}

	private void Destruct()
	{
		Object.DestroyImmediate(this);
	}
}
