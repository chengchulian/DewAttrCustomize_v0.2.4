using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Title_LizardSmoothie : LogicBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public Action<bool> onClicked;

	public Transform punchTransform;

	public Vector3 punch;

	public float punchDuration;

	public GameObject normalEffect;

	public GameObject flipEffect;

	public GameObject thanksForPlaying;

	private Vector3 _originalScale;

	private void Awake()
	{
		_originalScale = punchTransform.localScale;
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (DewInput.GetButtonDown(GamepadButtonEx.RightTrigger))
		{
			OnPointerClick(null);
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (thanksForPlaying != null)
		{
			thanksForPlaying.SetActive(DewSave.profile.didReadLoopNotice);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		bool isFlip = global::UnityEngine.Random.value < 0.1f;
		DewEffect.Play(isFlip ? flipEffect : normalEffect);
		punchTransform.localScale = _originalScale * ((!isFlip) ? 1 : (-1));
		punchTransform.DOKill();
		punchTransform.DOPunchScale(punch, punchDuration);
		onClicked?.Invoke(isFlip);
	}

	public void OpenEmail()
	{
		Application.OpenURL("mailto:lizardsmoothie.dev@gmail.com");
	}
}
