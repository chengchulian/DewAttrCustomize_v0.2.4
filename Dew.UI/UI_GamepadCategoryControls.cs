using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_GamepadCategoryControls : MonoBehaviour
{
	public UnityEvent onPrev;

	public UnityEvent onNext;

	private DewInputTrigger it_prevCategory;

	private DewInputTrigger it_nextCategory;

	private UI_ToggleGroup _group;

	private void Awake()
	{
		_group = GetComponent<UI_ToggleGroup>();
	}

	private void Start()
	{
		it_prevCategory = new DewInputTrigger
		{
			owner = this,
			binding = () => DewSave.profile.controls.prevCategory,
			isValidCheck = () => base.isActiveAndEnabled,
			canConsume = false,
			priority = -10
		};
		it_nextCategory = new DewInputTrigger
		{
			owner = this,
			binding = () => DewSave.profile.controls.nextCategory,
			isValidCheck = () => base.isActiveAndEnabled,
			canConsume = false,
			priority = -10
		};
	}

	private void Update()
	{
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			if (it_prevCategory.downRepeated)
			{
				Advance(next: false);
				DewEffect.Play(ManagerBase<GlobalUIManager>.instance.fxTick);
			}
			if (it_nextCategory.downRepeated)
			{
				Advance(next: true);
				DewEffect.Play(ManagerBase<GlobalUIManager>.instance.fxTick);
			}
		}
	}

	private void Advance(bool next)
	{
		if (next)
		{
			onNext?.Invoke();
		}
		else
		{
			onPrev?.Invoke();
		}
		if (_group == null)
		{
			return;
		}
		ListReturnHandle<UI_Toggle> handle;
		List<UI_Toggle> toggles = ((Component)this).GetComponentsInChildrenNonAlloc(out handle);
		toggles.Sort((UI_Toggle x, UI_Toggle y) => x.index.CompareTo(y.index));
		if (next)
		{
			for (int i = 0; i < toggles.Count; i++)
			{
				if (toggles[i].isActiveAndEnabled && toggles[i].index > _group.currentIndex)
				{
					_group.currentIndex = toggles[i].index;
					return;
				}
			}
		}
		else
		{
			for (int i2 = toggles.Count - 1; i2 >= 0; i2--)
			{
				if (toggles[i2].isActiveAndEnabled && toggles[i2].index < _group.currentIndex)
				{
					_group.currentIndex = toggles[i2].index;
					return;
				}
			}
		}
		handle.Return();
	}
}
