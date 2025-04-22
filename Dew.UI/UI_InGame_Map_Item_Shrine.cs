using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_Map_Item_Shrine : UI_InGame_Map_Item_Actor
{
	public Image markerImage;

	public Outline outlineForColor;

	public Color outlineTint;

	public Color unavailableColor;

	public new Shrine target => (Shrine)base.target;

	public override bool IsSupported(Actor a)
	{
		return a is Shrine;
	}

	public override MapItemOrder OnSetup(object t)
	{
		base.OnSetup(t);
		visibility = target.mapVisibility;
		if (visibility == MapItemVisibility.Hidden)
		{
			Object.Destroy(base.gameObject);
			return MapItemOrder.Default;
		}
		if (target.mapIcon != null)
		{
			markerImage.sprite = target.mapIcon;
		}
		UpdateColor();
		return MapItemOrder.Default;
	}

	private void UpdateColor()
	{
		markerImage.color = (target.isAvailable ? target.mapIconColor : unavailableColor);
		if (outlineForColor != null)
		{
			outlineForColor.effectColor = markerImage.color * outlineTint;
		}
	}

	protected override void OnUpdateItem()
	{
		base.OnUpdateItem();
		UpdateColor();
	}
}
