using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_InGame_CenterMessage : MonoBehaviour
{
	public Vector3 velocity;

	public float lifeTime;

	public float fadeInTime;

	public float fadeOutTime;

	public float fadeInVelocityMultiplier = 3f;

	private TextMeshProUGUI _text;

	private CanvasGroup _cg;

	private void Start()
	{
		_cg = GetComponent<CanvasGroup>();
		_text = GetComponent<TextMeshProUGUI>();
		_cg.alpha = 0f;
		velocity *= fadeInVelocityMultiplier;
		DOTween.Sequence().Append(_cg.DOFade(1f, fadeInTime)).AppendCallback(delegate
		{
			velocity /= fadeInVelocityMultiplier;
		})
			.AppendInterval(lifeTime)
			.Append(_cg.DOFade(0f, fadeOutTime))
			.AppendCallback(delegate
			{
				Object.Destroy(base.gameObject);
			})
			.SetUpdate(isIndependentUpdate: true);
	}

	private void Update()
	{
		base.transform.position += velocity * Time.deltaTime;
	}
}
