using System.IO;
using UnityEngine;

public class RampGeneratorTDE : MonoBehaviour
{
	public enum Mode
	{
		CreateAtStart,
		UpdateEveryFrame,
		BakeAndSaveAsTexture
	}

	public Gradient procedrualGradientRamp;

	public string pathForPNG = "/SineVFX/TopDownEffects/Recources/Textures/ForVFX/RampsGenerated/";

	public Renderer[] renderers;

	public Mode mode;

	private Texture2D rampTexture;

	private Texture2D tempTexture;

	private float width = 256f;

	private float height = 64f;

	private void Start()
	{
		switch (mode)
		{
		case Mode.CreateAtStart:
			UpdateRampTexture();
			break;
		case Mode.UpdateEveryFrame:
			UpdateRampTexture();
			break;
		case Mode.BakeAndSaveAsTexture:
			break;
		}
	}

	private void Update()
	{
		switch (mode)
		{
		case Mode.UpdateEveryFrame:
			UpdateRampTexture();
			break;
		case Mode.CreateAtStart:
		case Mode.BakeAndSaveAsTexture:
			break;
		}
	}

	private Texture2D GenerateTextureFromGradient(Gradient grad, float textureheight)
	{
		if (tempTexture == null)
		{
			tempTexture = new Texture2D((int)width, (int)textureheight);
		}
		for (int x = 0; (float)x < width; x++)
		{
			for (int y = 0; (float)y < textureheight; y++)
			{
				Color col = grad.Evaluate(0f + (float)x / width);
				tempTexture.SetPixel(x, y, col);
			}
		}
		tempTexture.wrapMode = TextureWrapMode.Clamp;
		tempTexture.Apply();
		return tempTexture;
	}

	public void UpdateRampTexture()
	{
		rampTexture = GenerateTextureFromGradient(procedrualGradientRamp, height);
		Renderer[] array = renderers;
		for (int i = 0; i < array.Length; i++)
		{
			Material[] materials = array[i].materials;
			for (int j = 0; j < materials.Length; j++)
			{
				materials[j].SetTexture("_Ramp", rampTexture);
			}
		}
	}

	public void BakeGradient()
	{
		rampTexture = GenerateTextureFromGradient(procedrualGradientRamp, 64f);
		byte[] _bytes = rampTexture.EncodeToPNG();
		File.WriteAllBytes(Application.dataPath + pathForPNG + "GeneratedRamp_" + Random.Range(0, 99999) + ".png", _bytes);
	}
}
