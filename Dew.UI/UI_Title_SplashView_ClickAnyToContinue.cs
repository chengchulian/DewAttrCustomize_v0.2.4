using UnityEngine;

public class UI_Title_SplashView_ClickAnyToContinue : MonoBehaviour
{
	private void Update()
	{
		if (DewInput.GetButtonDownAnyGamepad() || DewInput.GetButtonDownAnyMouse())
		{
			GetComponentInParent<UI_Title_SplashView>().Click();
		}
	}
}
