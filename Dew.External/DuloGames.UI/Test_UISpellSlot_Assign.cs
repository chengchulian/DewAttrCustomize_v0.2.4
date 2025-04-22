using UnityEngine;

namespace DuloGames.UI;

public class Test_UISpellSlot_Assign : MonoBehaviour
{
	[SerializeField]
	private UISpellSlot slot;

	[SerializeField]
	private int assignSpell;

	private void Awake()
	{
		if (slot == null)
		{
			slot = GetComponent<UISpellSlot>();
		}
	}

	private void Start()
	{
		if (slot == null || UISpellDatabase.Instance == null)
		{
			Destruct();
			return;
		}
		slot.Assign(UISpellDatabase.Instance.GetByID(assignSpell));
		Destruct();
	}

	private void Destruct()
	{
		Object.DestroyImmediate(this);
	}
}
