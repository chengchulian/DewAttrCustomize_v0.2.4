using UnityEngine;

namespace DuloGames.UI;

public class Test_UISlotBase_Assign : MonoBehaviour
{
	[SerializeField]
	private UISlotBase slot;

	[SerializeField]
	private Texture texture;

	[SerializeField]
	private Sprite sprite;

	private void Start()
	{
		if (slot != null)
		{
			if (texture != null)
			{
				slot.Assign(texture);
			}
			else if (sprite != null)
			{
				slot.Assign(sprite);
			}
		}
	}
}
