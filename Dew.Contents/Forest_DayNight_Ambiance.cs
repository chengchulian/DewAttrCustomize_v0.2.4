using UnityEngine;

public class Forest_DayNight_Ambiance : Forest_DayNight_Base
{
	public AudioSource day;

	public float dayVolume;

	public AudioSource night;

	public float nightVolume;

	public override void Update()
	{
		base.Update();
		if (!Application.IsPlaying(this) || Time.timeScale <= 0.001f)
		{
			return;
		}
		float num = Mathf.PingPong(base.animatedValue, 1f);
		day.volume = (1f - num) * dayVolume;
		night.volume = num * nightVolume;
		if (day.clip != null)
		{
			if (day.volume > 0.01f && !day.isPlaying)
			{
				day.Play();
			}
			else if (day.volume < 0.01f && day.isPlaying)
			{
				day.Stop();
			}
		}
		if (night.clip != null)
		{
			if (night.volume > 0.01f && !night.isPlaying)
			{
				night.Play();
			}
			else if (night.volume < 0.01f && night.isPlaying)
			{
				night.Stop();
			}
		}
	}
}
