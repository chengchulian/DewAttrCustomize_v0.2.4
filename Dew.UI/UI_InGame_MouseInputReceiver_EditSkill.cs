using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InGame_MouseInputReceiver_EditSkill : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		EditSkillManager.ModeType mode = ManagerBase<EditSkillManager>.instance.mode;
		if (!DewInput.GetButton(DewSave.profile.controls.editSkillHold, checkGameAreaForMouse: false) && mode != 0 && mode != EditSkillManager.ModeType.EquipGem && mode != EditSkillManager.ModeType.EquipSkill && mode != EditSkillManager.ModeType.Regular)
		{
			ManagerBase<EditSkillManager>.instance.EndEdit();
		}
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			ManagerBase<EditSkillManager>.instance.EndDrag(isCancel: false);
		}
		else
		{
			ManagerBase<EditSkillManager>.instance.EndDrag(isCancel: true);
		}
	}
}
