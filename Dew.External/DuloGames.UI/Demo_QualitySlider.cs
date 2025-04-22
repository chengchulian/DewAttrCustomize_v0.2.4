using System.Collections.Generic;
using UnityEngine;

namespace DuloGames.UI;

public class Demo_QualitySlider : MonoBehaviour
{
	[SerializeField]
	private UISliderExtended m_Slider;

	private void Start()
	{
		List<string> graphicsQualityList = new List<string>(QualitySettings.names.Length);
		string[] names = QualitySettings.names;
		foreach (string name in names)
		{
			graphicsQualityList.Add(name);
		}
		m_Slider.options = graphicsQualityList;
		m_Slider.value = QualitySettings.GetQualityLevel();
	}
}
