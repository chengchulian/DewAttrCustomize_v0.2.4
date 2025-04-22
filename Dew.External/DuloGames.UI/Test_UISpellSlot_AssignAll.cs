using UnityEngine;

namespace DuloGames.UI;

public class Test_UISpellSlot_AssignAll : MonoBehaviour
{
	[SerializeField]
	private Transform m_Container;

	private void Start()
	{
		if (m_Container == null || UISpellDatabase.Instance == null)
		{
			Destruct();
			return;
		}
		UISpellSlot[] slots = m_Container.gameObject.GetComponentsInChildren<UISpellSlot>();
		UISpellInfo[] spells = UISpellDatabase.Instance.spells;
		if (slots.Length != 0 && spells.Length != 0)
		{
			UISpellSlot[] array = slots;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Assign(spells[Random.Range(0, spells.Length)]);
			}
		}
		Destruct();
	}

	private void Destruct()
	{
		Object.DestroyImmediate(this);
	}
}
