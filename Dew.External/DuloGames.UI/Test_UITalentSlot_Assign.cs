using UnityEngine;

namespace DuloGames.UI;

public class Test_UITalentSlot_Assign : MonoBehaviour
{
	[SerializeField]
	private UITalentSlot slot;

	[SerializeField]
	private int assignTalent;

	[SerializeField]
	private int addPoints;

	private void Start()
	{
		if (slot == null)
		{
			slot = GetComponent<UITalentSlot>();
		}
		if (slot == null || UITalentDatabase.Instance == null || UISpellDatabase.Instance == null)
		{
			Destruct();
			return;
		}
		UITalentInfo info = UITalentDatabase.Instance.GetByID(assignTalent);
		if (info != null)
		{
			slot.Assign(info, UISpellDatabase.Instance.GetByID(info.spellEntry));
			slot.AddPoints(addPoints);
		}
		Destruct();
	}

	private void Destruct()
	{
		Object.DestroyImmediate(this);
	}
}
