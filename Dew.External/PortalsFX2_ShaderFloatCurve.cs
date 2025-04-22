using UnityEngine;

public class PortalsFX2_ShaderFloatCurve : MonoBehaviour
{
	public PortalsFX2_ShaderProperties ShaderFloatProperty = PortalsFX2_ShaderProperties._Cutout;

	public AnimationCurve FloatCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	public float GraphTimeMultiplier = 1f;

	public float GraphIntensityMultiplier = 1f;

	public bool IsLoop;

	private bool canUpdate;

	private float startTime;

	private int propertyID;

	private string shaderProperty;

	private bool isInitialized;

	private MaterialPropertyBlock props;

	private Renderer rend;

	private void Awake()
	{
		if (props == null)
		{
			props = new MaterialPropertyBlock();
		}
		if (rend == null)
		{
			rend = GetComponent<Renderer>();
		}
		shaderProperty = ShaderFloatProperty.ToString();
		propertyID = Shader.PropertyToID(shaderProperty);
	}

	private void OnEnable()
	{
		startTime = Time.time;
		canUpdate = true;
		rend.GetPropertyBlock(props);
		float eval = FloatCurve.Evaluate(0f) * GraphIntensityMultiplier;
		props.SetFloat(propertyID, eval);
		rend.SetPropertyBlock(props);
	}

	private void Update()
	{
		rend.GetPropertyBlock(props);
		float time = Time.time - startTime;
		if (canUpdate)
		{
			float eval = FloatCurve.Evaluate(time / GraphTimeMultiplier) * GraphIntensityMultiplier;
			props.SetFloat(propertyID, eval);
		}
		if (time >= GraphTimeMultiplier)
		{
			if (IsLoop)
			{
				startTime = Time.time;
			}
			else
			{
				canUpdate = false;
			}
		}
		rend.SetPropertyBlock(props);
	}
}
