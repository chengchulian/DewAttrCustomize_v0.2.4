using UnityEngine;

namespace DuloGames.UI;

public class Demo_ResolutionSelect : MonoBehaviour
{
	[SerializeField]
	private UISelectField m_SelectField;

	protected void Start()
	{
		if (!(m_SelectField == null))
		{
			m_SelectField.ClearOptions();
			Resolution[] resolutions = Screen.resolutions;
			for (int i = 0; i < resolutions.Length; i++)
			{
				Resolution res = resolutions[i];
				m_SelectField.AddOption(res.width + "x" + res.height + " @ " + res.refreshRate + "Hz");
			}
			Resolution currentResolution = Screen.currentResolution;
			m_SelectField.SelectOption(currentResolution.width + "x" + currentResolution.height + " @ " + currentResolution.refreshRate + "Hz");
		}
	}

	protected void OnEnable()
	{
		if (!(m_SelectField == null))
		{
			m_SelectField.onChange.AddListener(OnSelectedOption);
		}
	}

	protected void OnDisable()
	{
		if (!(m_SelectField == null))
		{
			m_SelectField.onChange.RemoveListener(OnSelectedOption);
		}
	}

	protected void OnSelectedOption(int index, string option)
	{
		Resolution res = Screen.resolutions[index];
		if (!res.Equals(Screen.currentResolution))
		{
			Screen.SetResolution(res.width, res.height, fullscreen: true, res.refreshRate);
		}
	}
}
