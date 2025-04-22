public class UI_EmoteList_Item_GamepadFocusable : UI_GamepadFocusable
{
	public override void OnFocusStateChanged(bool state)
	{
		base.OnFocusStateChanged(state);
		if (state)
		{
			GetComponent<UI_Toggle>().isChecked = true;
		}
	}
}
