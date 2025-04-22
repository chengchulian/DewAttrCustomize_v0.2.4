using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

public class FlexibleWidthGridLayout : GridLayoutGroup
{
	public override void SetLayoutHorizontal()
	{
		UpdateCellSize();
		base.SetLayoutHorizontal();
	}

	public override void SetLayoutVertical()
	{
		UpdateCellSize();
		base.SetLayoutVertical();
	}

	private void UpdateCellSize()
	{
		float x = (base.rectTransform.rect.size.x - (float)base.padding.horizontal - base.spacing.x * (float)(base.constraintCount - 1)) / (float)base.constraintCount;
		base.constraint = Constraint.FixedColumnCount;
		base.cellSize = new Vector2(x, base.cellSize.y);
	}
}
