using UnityEngine;
using UnityEngine.UI;

namespace commanastationwww.eternaltemple;

public class FPScounter : MonoBehaviour
{
	public float updateInterval = 0.5f;

	private float accum;

	private int frames;

	private float timeleft;

	private Text textFpsCounter;

	private void Start()
	{
		textFpsCounter = GetComponent<Text>();
	}

	private void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;
		if ((double)timeleft <= 0.0)
		{
			float fps = accum / (float)frames;
			string format = $"{fps:F2} FPS";
			textFpsCounter.text = format;
			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}
}
