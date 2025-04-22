using UnityEngine;

public class UI_RegisterTooltipCallbacks : MonoBehaviour
{
	private void Start()
	{
		SingletonBehaviour<UI_TooltipManager>.instance.RegisterInGameCallbacks();
	}
}
