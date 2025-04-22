using System;
using DG.Tweening;
using UnityEngine;

public class UI_InGame_EditSkillStateIndicator : MonoBehaviour
{
	public EditSkillManager.ModeType type;

	public bool doScale = true;

	public Vector3 startScale = Vector3.one * 0.25f;

	public float scaleDuration = 0.25f;

	public GameObject cancelObject;

	private Vector3 _originalScale;

	private void Start()
	{
		EditSkillManager instance = ManagerBase<EditSkillManager>.instance;
		instance.onModeChanged = (Action<EditSkillManager.ModeType>)Delegate.Combine(instance.onModeChanged, new Action<EditSkillManager.ModeType>(OnStateChanged));
		base.gameObject.SetActive(value: false);
	}

	private void OnEnable()
	{
		base.transform.DOKill();
		if (_originalScale == default(Vector3))
		{
			_originalScale = base.transform.localScale;
		}
		if (doScale)
		{
			base.transform.localScale = startScale;
			base.transform.DOScale(_originalScale, scaleDuration).SetUpdate(isIndependentUpdate: true);
		}
		if (cancelObject != null)
		{
			cancelObject.SetActive(!ManagerBase<ControlManager>.instance.isEditSkillDisabled);
		}
	}

	private void OnStateChanged(EditSkillManager.ModeType obj)
	{
		base.gameObject.SetActive(obj == type);
	}
}
