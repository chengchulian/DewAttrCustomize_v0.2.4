using System;
using TMPro;
using UnityEngine;

public class UI_InGame_WorldMessage : MonoBehaviour
{
	public float lifeTime = 2f;

	public Vector2 popOffset;

	public AnimationCurve offsetCurve;

	public TextMeshProUGUI text;

	[NonSerialized]
	public WorldMessageSetting message;

	private float _startTime;

	private void Start()
	{
		_startTime = Time.time;
		global::UnityEngine.Object.Destroy(base.gameObject, lifeTime);
		UpdatePosition();
		GetComponent<Animation>().Play("DamageNumberAnim");
		text.text = message.rawText;
		text.color = message.color;
		if (message.popOffset.HasValue)
		{
			popOffset = message.popOffset.Value;
		}
	}

	private void LateUpdate()
	{
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		Vector3 pos = ((message.worldPosGetter != null) ? message.worldPosGetter() : message.worldPos);
		base.transform.position = Dew.mainCamera.WorldToScreenPoint(pos) + (Vector3)popOffset * offsetCurve.Evaluate(Time.time - _startTime);
	}
}
