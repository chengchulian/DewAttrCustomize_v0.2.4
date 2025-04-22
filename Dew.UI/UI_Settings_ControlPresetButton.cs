using UnityEngine;
using UnityEngine.UI;

public class UI_Settings_ControlPresetButton : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(delegate
		{
			SingletonBehaviour<UI_SelectControlPresetWindow>.instance.Show(showCancel: true);
		});
	}
}
