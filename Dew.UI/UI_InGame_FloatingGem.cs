using UnityEngine;

public class UI_InGame_FloatingGem : UI_InGame_FloatingSkill
{
	protected override void OnDraggingObjectChanged(Object obj)
	{
		if (obj is Gem gem)
		{
			_cg.alpha = 1f;
			image.sprite = gem.icon;
			UpdatePosition(immediately: true);
		}
		else
		{
			_cg.alpha = 0f;
		}
	}

	protected override void UpdatePosition(bool immediately)
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
			targetPos = skillButtons[index].transform.position;
		}
		else if (ManagerBase<EditSkillManager>.instance.selectedGemSlot.HasValue)
		{
			UI_InGame_SkillButton[] skillButtons2 = ManagerBase<UI_InGame_SkillButtons>.instance.skillButtons;
			GemLocation sel = ManagerBase<EditSkillManager>.instance.selectedGemSlot.Value;
			int skill = (int)sel.skill;
			targetPos = skillButtons2[skill].GetGemSlot(sel.index).transform.position + Vector3.up * 20f;
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
