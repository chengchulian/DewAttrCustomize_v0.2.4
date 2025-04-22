using UnityEngine;

public class UI_Title_ShowGRACGradeButton : MonoBehaviour
{
	private void Start()
	{
		if (!UI_Title_SplashView.IsUserInSouthKorea())
		{
			Object.Destroy(base.gameObject);
		}
	}
}
