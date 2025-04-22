using UnityEngine;
using UnityEngine.UI;

public class ChangeSizeColor : MonoBehaviour
{
	public Gradient color;

	public Color m_changeColor;

	public GameObject m_obj;

	private Renderer[] m_rnds;

	private float color_Value;

	private bool isChangeColor;

	public Image m_ColorHandler;

	public Text m_intensityfactor;

	private float intensity = 2f;

	private void Update()
	{
		m_changeColor = color.Evaluate(color_Value);
		m_ColorHandler.color = m_changeColor;
		if (!isChangeColor || !(m_obj != null))
		{
			return;
		}
		m_rnds = m_obj.GetComponentsInChildren<Renderer>(includeInactive: true);
		Renderer[] rnds = m_rnds;
		foreach (Renderer rend in rnds)
		{
			for (int j = 0; j < rend.materials.Length; j++)
			{
				rend.materials[j].SetColor("_TintColor", m_changeColor * intensity);
				rend.materials[j].SetColor("_Color", m_changeColor * intensity);
				rend.materials[j].SetColor("_RimColor", m_changeColor * intensity);
			}
		}
	}

	public void ChangeEffectColor(float value)
	{
		color_Value = value;
	}

	public void CheckIsColorChange(bool value)
	{
		isChangeColor = value;
	}

	public void CheckColorState()
	{
		if (isChangeColor)
		{
			isChangeColor = false;
		}
		else
		{
			isChangeColor = true;
		}
	}

	public void GetIntensityFactor()
	{
		float m_intensity = float.Parse(m_intensityfactor.text.ToString());
		if (m_intensity > 0f)
		{
			intensity = m_intensity;
		}
		else
		{
			intensity = 0f;
		}
	}
}
