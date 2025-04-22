using UnityEngine;

[RequireComponent(typeof(Camera))]
public class wireframe : MonoBehaviour
{
	public KeyCode wireFrameKey;

	public bool wireFrameMode;

	public void Start()
	{
	}

	private void OnPreRender()
	{
		if (wireFrameMode)
		{
			GL.wireframe = true;
		}
		else
		{
			GL.wireframe = false;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(wireFrameKey))
		{
			wireFrameMode = !wireFrameMode;
		}
	}
}
