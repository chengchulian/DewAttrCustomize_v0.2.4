using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_FloatingSkill : MonoBehaviour
{
	public Image image;

	public GameObject discardObject;

	protected CanvasGroup _cg;

	protected Vector3 _cv;

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
		_cg.alpha = 0f;
	}

	private void Start()
	{
		EditSkillManager instance = ManagerBase<EditSkillManager>.instance;
		instance.onDraggingObjectChanged = (Action<global::UnityEngine.Object>)Delegate.Combine(instance.onDraggingObjectChanged, new Action<global::UnityEngine.Object>(OnDraggingObjectChanged));
	}

	protected virtual void OnDraggingObjectChanged(global::UnityEngine.Object obj)
	{
		if (obj is SkillTrigger skill)
		{
			_cg.alpha = 1f;
			image.sprite = skill.configs[0].triggerIcon;
			UpdatePosition(immediately: true);
		}
		else
		{
			_cg.alpha = 0f;
		}
	}

	private void Update()
	{
		if (_cg.alpha > 0.5f)
		{
			UpdatePosition(immediately: false);
			if (ManagerBase<ControlManager>.instance.dropConstraint != null && !ManagerBase<ControlManager>.instance.dropConstraint(ManagerBase<EditSkillManager>.instance.draggingObject))
			{
				discardObject.gameObject.SetActive(value: false);
			}
			else
			{
				discardObject.gameObject.SetActive(ManagerBase<EditSkillManager>.instance.isSelectingGround || (DewInput.currentMode == InputMode.KeyboardAndMouse && !ManagerBase<EditSkillManager>.instance.dropToGroundBlocker.GetScreenSpaceRect().Contains(Input.mousePosition)));
			}
		}
		else
		{
			discardObject.gameObject.SetActive(value: false);
		}
	}

	protected virtual void UpdatePosition(bool immediately)
	{
		Vector3 targetPos = base.transform.position;
		if (DewInput.currentMode == InputMode.KeyboardAndMouse)
		{
			targetPos = Input.mousePosition;
		}
		else if (ManagerBase<EditSkillManager>.instance.isSelectingGround)
		{
			int skillIndex = 0;
			if (ManagerBase<EditSkillManager>.instance.selectedSkillSlot.HasValue)
			{
				skillIndex = (int)ManagerBase<EditSkillManager>.instance.selectedSkillSlot.Value;
			}
			else if (ManagerBase<EditSkillManager>.instance.selectedGemSlot.HasValue)
			{
				skillIndex = (int)ManagerBase<EditSkillManager>.instance.selectedGemSlot.Value.skill;
			}
			targetPos = ManagerBase<UI_InGame_SkillButtons>.instance.skillButtons[skillIndex].tooltipPivot.position + Vector3.up * 100f;
		}
		else if (ManagerBase<EditSkillManager>.instance.selectedSkillSlot.HasValue)
		{
			UI_InGame_SkillButton[] skillButtons = ManagerBase<UI_InGame_SkillButtons>.instance.skillButtons;
			int index = (int)ManagerBase<EditSkillManager>.instance.selectedSkillSlot.Value;
			targetPos = skillButtons[index].transform.position + Vector3.up * 50f;
		}
		else if (ManagerBase<EditSkillManager>.instance.selectedGemSlot.HasValue)
		{
			UI_InGame_SkillButton[] skillButtons2 = ManagerBase<UI_InGame_SkillButtons>.instance.skillButtons;
			GemLocation sel = ManagerBase<EditSkillManager>.instance.selectedGemSlot.Value;
			int skill = (int)sel.skill;
			targetPos = skillButtons2[skill].GetGemSlot(sel.index).transform.position;
		}
		if (immediately || DewInput.currentMode == InputMode.KeyboardAndMouse)
		{
			base.transform.position = targetPos;
			_cv = Vector3.zero;
		}
		else
		{
			base.transform.position = Vector3.SmoothDamp(base.transform.position, targetPos, ref _cv, 0.05f, float.PositiveInfinity, Time.unscaledDeltaTime);
		}
	}
}
