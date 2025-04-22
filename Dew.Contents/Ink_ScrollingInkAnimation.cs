using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[LogicUpdatePriority(1600)]
public class Ink_ScrollingInkAnimation : LogicBehaviour
{
	private struct InkMaterailItem
	{
		public Material mat;

		public float[] noiseWeights;

		public Vector4[] noiseSTs;

		public Vector4[] noiseScales;
	}

	private static readonly int L1NoiseWeight = Shader.PropertyToID("_L1NoiseWeight");

	private static readonly int L2NoiseWeight = Shader.PropertyToID("_L2NoiseWeight");

	private static readonly int L3NoiseWeight = Shader.PropertyToID("_L3NoiseWeight");

	private static readonly int L4NoiseWeight = Shader.PropertyToID("_L4NoiseWeight");

	private static readonly int L5NoiseWeight = Shader.PropertyToID("_L5NoiseWeight");

	private static readonly int L6NoiseWeight = Shader.PropertyToID("_L6NoiseWeight");

	private static readonly int L7NoiseWeight = Shader.PropertyToID("_L7NoiseWeight");

	private static readonly int[] NoiseWeight = new int[7] { L1NoiseWeight, L2NoiseWeight, L3NoiseWeight, L4NoiseWeight, L5NoiseWeight, L6NoiseWeight, L7NoiseWeight };

	private static readonly int L0Noise_ST = Shader.PropertyToID("_L0Noise_ST");

	private static readonly int L1Noise_ST = Shader.PropertyToID("_L1Noise_ST");

	private static readonly int L2Noise_ST = Shader.PropertyToID("_L2Noise_ST");

	private static readonly int L3Noise_ST = Shader.PropertyToID("_L3Noise_ST");

	private static readonly int L4Noise_ST = Shader.PropertyToID("_L4Noise_ST");

	private static readonly int L5Noise_ST = Shader.PropertyToID("_L5Noise_ST");

	private static readonly int L6Noise_ST = Shader.PropertyToID("_L6Noise_ST");

	private static readonly int L7Noise_ST = Shader.PropertyToID("_L7Noise_ST");

	private static readonly int[] NoiseST = new int[8] { L0Noise_ST, L1Noise_ST, L2Noise_ST, L3Noise_ST, L4Noise_ST, L5Noise_ST, L6Noise_ST, L7Noise_ST };

	private static readonly int L0NoiseScales = Shader.PropertyToID("_L0NoiseScales");

	private static readonly int L1NoiseScales = Shader.PropertyToID("_L1NoiseScales");

	private static readonly int L2NoiseScales = Shader.PropertyToID("_L2NoiseScales");

	private static readonly int L3NoiseScales = Shader.PropertyToID("_L3NoiseScales");

	private static readonly int L4NoiseScales = Shader.PropertyToID("_L4NoiseScales");

	private static readonly int L5NoiseScales = Shader.PropertyToID("_L5NoiseScales");

	private static readonly int L6NoiseScales = Shader.PropertyToID("_L6NoiseScales");

	private static readonly int L7NoiseScales = Shader.PropertyToID("_L7NoiseScales");

	private static readonly int[] NoiseScales = new int[8] { L0NoiseScales, L1NoiseScales, L2NoiseScales, L3NoiseScales, L4NoiseScales, L5NoiseScales, L6NoiseScales, L7NoiseScales };

	public bool doTerrain;

	public bool l1NoiseWeight;

	public Vector2 l1SpeedAndMagnitude = new Vector2(1.25f, 0.5f);

	[Space(20f)]
	public bool l2NoiseWeight;

	public Vector2 l2SpeedAndMagnitude = new Vector2(1.25f, 0.5f);

	[Space(20f)]
	public bool l3NoiseWeight;

	public Vector2 l3SpeedAndMagnitude = new Vector2(1.25f, 0.5f);

	[Space(20f)]
	public bool l4NoiseWeight;

	public Vector2 l4SpeedAndMagnitude = new Vector2(1.25f, 0.5f);

	[Space(20f)]
	public bool l5NoiseWeight;

	public Vector2 l5SpeedAndMagnitude = new Vector2(1.25f, 0.5f);

	[Space(20f)]
	public bool l6NoiseWeight;

	public Vector2 l6SpeedAndMagnitude = new Vector2(1.25f, 0.5f);

	[Space(20f)]
	public bool l7NoiseWeight;

	public Vector2 l7SpeedAndMagnitude = new Vector2(1.25f, 0.5f);

	[Space(20f)]
	public bool l0NoiseScroll;

	public Vector2 l0ScrollSpeed;

	[Space(20f)]
	public bool l1NoiseScroll;

	public Vector2 l1ScrollSpeed;

	[Space(20f)]
	public bool l2NoiseScroll;

	public Vector2 l2ScrollSpeed;

	[Space(20f)]
	public bool l3NoiseScroll;

	public Vector2 l3ScrollSpeed;

	[Space(20f)]
	public bool l4NoiseScroll;

	public Vector2 l4ScrollSpeed;

	[Space(20f)]
	public bool l5NoiseScroll;

	public Vector2 l5ScrollSpeed;

	[Space(20f)]
	public bool l6NoiseScroll;

	public Vector2 l6ScrollSpeed;

	[Space(20f)]
	public bool l7NoiseScroll;

	public Vector2 l7ScrollSpeed;

	[Space(20f)]
	public bool l0DetailScalePulse;

	public Vector2 l0DetailPulseParams = new Vector2(0.5f, 0.5f);

	[Space(20f)]
	public bool l1DetailScalePulse;

	public Vector2 l1DetailPulseParams = new Vector2(0.5f, 0.5f);

	[Space(20f)]
	public bool l2DetailScalePulse;

	public Vector2 l2DetailPulseParams = new Vector2(0.5f, 0.5f);

	[Space(20f)]
	public bool l3DetailScalePulse;

	public Vector2 l3DetailPulseParams = new Vector2(0.5f, 0.5f);

	[Space(20f)]
	public bool l4DetailScalePulse;

	public Vector2 l4DetailPulseParams = new Vector2(0.5f, 0.5f);

	[Space(20f)]
	public bool l5DetailScalePulse;

	public Vector2 l5DetailPulseParams = new Vector2(0.5f, 0.5f);

	[Space(20f)]
	public bool l6DetailScalePulse;

	public Vector2 l6DetailPulseParams = new Vector2(0.5f, 0.5f);

	[Space(20f)]
	public bool l7DetailScalePulse;

	public Vector2 l7DetailPulseParams = new Vector2(0.5f, 0.5f);

	private List<InkMaterailItem> _targets = new List<InkMaterailItem>();

	private void Start()
	{
		ListReturnHandle<GameObject> handle;
		List<GameObject> list = DewPool.GetList(out handle);
		SceneManager.GetActiveScene().GetRootGameObjects(list);
		foreach (GameObject item3 in list)
		{
			ListReturnHandle<Renderer> handle2;
			foreach (Renderer item4 in item3.GetComponentsInChildrenNonAlloc(out handle2))
			{
				Material[] sharedMaterials = item4.sharedMaterials;
				for (int i = 0; i < sharedMaterials.Length; i++)
				{
					if (!(sharedMaterials[i] == null) && !(sharedMaterials[i].shader == null))
					{
						string text = sharedMaterials[i].shader.name;
						if (!(text != "Dew/Dew New Terrain") || !(text != "Dew/Dew New Ground"))
						{
							InkMaterailItem item = CreateItem(sharedMaterials[i]);
							_targets.Add(item);
							sharedMaterials[i] = item.mat;
						}
					}
				}
				item4.sharedMaterials = sharedMaterials;
			}
			handle2.Return();
		}
		handle.Return();
		if (!doTerrain)
		{
			return;
		}
		Terrain[] array = Object.FindObjectsOfType<Terrain>();
		foreach (Terrain terrain in array)
		{
			if (!(terrain == null))
			{
				InkMaterailItem item2 = CreateItem(terrain.materialTemplate);
				_targets.Add(item2);
				terrain.materialTemplate = item2.mat;
			}
		}
		static InkMaterailItem CreateItem(Material m)
		{
			InkMaterailItem inkMaterailItem = default(InkMaterailItem);
			inkMaterailItem.mat = Object.Instantiate(m);
			inkMaterailItem.noiseWeights = new float[7];
			inkMaterailItem.noiseSTs = new Vector4[8];
			inkMaterailItem.noiseScales = new Vector4[8];
			InkMaterailItem result = inkMaterailItem;
			for (int k = 0; k < 7; k++)
			{
				result.noiseWeights[k] = m.GetFloat(NoiseWeight[k]);
			}
			for (int l = 0; l < 8; l++)
			{
				result.noiseSTs[l] = m.GetVector(NoiseST[l]);
				result.noiseScales[l] = m.GetVector(NoiseScales[l]);
			}
			return result;
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		int t;
		for (t = 0; t < _targets.Count; t++)
		{
			Material m = _targets[t].mat;
			AnimateNoiseWeight(l1NoiseWeight, 0, l1SpeedAndMagnitude.x, l1SpeedAndMagnitude.y);
			AnimateNoiseWeight(l2NoiseWeight, 1, l2SpeedAndMagnitude.x, l2SpeedAndMagnitude.y);
			AnimateNoiseWeight(l3NoiseWeight, 2, l3SpeedAndMagnitude.x, l3SpeedAndMagnitude.y);
			AnimateNoiseWeight(l4NoiseWeight, 3, l4SpeedAndMagnitude.x, l4SpeedAndMagnitude.y);
			AnimateNoiseWeight(l5NoiseWeight, 4, l5SpeedAndMagnitude.x, l5SpeedAndMagnitude.y);
			AnimateNoiseWeight(l6NoiseWeight, 5, l6SpeedAndMagnitude.x, l6SpeedAndMagnitude.y);
			AnimateNoiseWeight(l7NoiseWeight, 6, l7SpeedAndMagnitude.x, l7SpeedAndMagnitude.y);
			AnimateNoiseScroll(l0NoiseScroll, 0, l0ScrollSpeed);
			AnimateNoiseScroll(l1NoiseScroll, 1, l1ScrollSpeed);
			AnimateNoiseScroll(l2NoiseScroll, 2, l2ScrollSpeed);
			AnimateNoiseScroll(l3NoiseScroll, 3, l3ScrollSpeed);
			AnimateNoiseScroll(l4NoiseScroll, 4, l4ScrollSpeed);
			AnimateNoiseScroll(l5NoiseScroll, 5, l5ScrollSpeed);
			AnimateNoiseScroll(l6NoiseScroll, 6, l6ScrollSpeed);
			AnimateNoiseScroll(l7NoiseScroll, 7, l7ScrollSpeed);
			AnimateDetailScale(l0DetailScalePulse, 0, l0DetailPulseParams.x, l0DetailPulseParams.y);
			AnimateDetailScale(l1DetailScalePulse, 1, l1DetailPulseParams.x, l1DetailPulseParams.y);
			AnimateDetailScale(l2DetailScalePulse, 2, l2DetailPulseParams.x, l2DetailPulseParams.y);
			AnimateDetailScale(l3DetailScalePulse, 3, l3DetailPulseParams.x, l3DetailPulseParams.y);
			AnimateDetailScale(l4DetailScalePulse, 4, l4DetailPulseParams.x, l4DetailPulseParams.y);
			AnimateDetailScale(l5DetailScalePulse, 5, l5DetailPulseParams.x, l5DetailPulseParams.y);
			AnimateDetailScale(l6DetailScalePulse, 6, l6DetailPulseParams.x, l6DetailPulseParams.y);
			AnimateDetailScale(l7DetailScalePulse, 7, l7DetailPulseParams.x, l7DetailPulseParams.y);
			void AnimateDetailScale(bool value, int index, float speed, float magnitude)
			{
				if (value)
				{
					float num3 = Time.time + (float)index * 4.73f + 2.1f;
					Vector4 vector = _targets[t].noiseScales[index];
					float num4 = Mathf.Sin(num3 * speed) * magnitude;
					Vector4 value3 = new Vector4(vector.x, vector.y, vector.z + num4, vector.w);
					m.SetVector(NoiseScales[index], value3);
					Vector4 vector2 = _targets[t].noiseSTs[index];
					float num5 = (vector.z + num4) / vector.z;
					float x = vector2.x;
					float num6 = 0.5f / x * (1f - num5);
					Vector4 value4 = new Vector4(vector2.x, vector2.y, vector2.z + num6, vector2.w);
					m.SetVector(NoiseST[index], value4);
				}
			}
			void AnimateNoiseScroll(bool value, int index, Vector2 speed)
			{
				if (value)
				{
					float num2 = Time.time + (float)index * 8.051f + 4.7f;
					Vector4 value2 = _targets[t].noiseSTs[index];
					value2.z += speed.x * num2;
					value2.w += speed.y * num2;
					m.SetVector(NoiseST[index], value2);
				}
			}
			void AnimateNoiseWeight(bool value, int index, float speed, float magnitude)
			{
				if (value)
				{
					float num = Time.time + (float)index * 12.152f;
					m.SetFloat(NoiseWeight[index], _targets[t].noiseWeights[index] + Mathf.Sin(num * speed) * magnitude);
				}
			}
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < _targets.Count; i++)
		{
			if (!(_targets[i].mat == null))
			{
				Object.Destroy(_targets[i].mat);
			}
		}
		_targets.Clear();
	}
}
