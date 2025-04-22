using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[LogicUpdatePriority(600)]
public class UI_InGame_HeroInfoBar : LogicBehaviour
{
	public Image heroIcon;

	public TextMeshProUGUI healthText;

	public TextMeshProUGUI levelText;

	public TextMeshProUGUI manaText;

	public GameObject manaBarObject;

	public Transform levelObject;

	private UI_EntityProvider _entityProvider;

	private Canvas _canvas;

	private DewInputTrigger it_selectInfoBar;

	private void Awake()
	{
		GetComponent(out _entityProvider);
		GetComponent(out _canvas);
	}

	private void Start()
	{
		CameraManager instance = ManagerBase<CameraManager>.instance;
		instance.onIsSpectatingChanged = (Action<bool>)Delegate.Combine(instance.onIsSpectatingChanged, new Action<bool>(OnIsSpectatingChanged));
		it_selectInfoBar = new DewInputTrigger
		{
			owner = this,
			binding = () => DewSave.profile.controls.selectInfoBar,
			priority = -1,
			isValidCheck = () => ManagerBase<GlobalUIManager>.instance.focused == null
		};
	}

	private void OnIsSpectatingChanged(bool obj)
	{
		_canvas.enabled = !obj;
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (DewInput.currentMode == InputMode.Gamepad && ManagerBase<GlobalUIManager>.instance.focused == null && it_selectInfoBar.down)
		{
			ManagerBase<GlobalUIManager>.instance.SetFocus(healthText.GetComponentInParent<UI_GamepadFocusable>());
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (DewPlayer.local == null)
		{
			return;
		}
		Hero target = DewPlayer.local.hero;
		if (target == null)
		{
			return;
		}
		heroIcon.sprite = target.icon;
		_entityProvider.target = target;
		bool hasMana = target.Status.manaTypeKey != "";
		manaBarObject.SetActive(hasMana);
		if (!(target == null))
		{
			float healthVal = target.currentHealth;
			if (target.isKnockedOut)
			{
				healthVal = 0f;
			}
			else if (healthVal < 1f)
			{
				healthVal = 1f;
			}
			if (target.Status.isHealthHidden)
			{
				healthText.text = $"???/{target.maxHealth:#,##0}";
			}
			else if (target.Status.currentShield > 0.01f)
			{
				healthText.text = $"{healthVal:#,##0}/{target.maxHealth:#,##0} (+{target.Status.currentShield:#,##0})";
			}
			else
			{
				healthText.text = $"{healthVal:#,##0}/{target.maxHealth:#,##0}";
			}
			levelText.text = $"{target.level}";
			manaText.text = $"{target.currentMana:#,##0}/{target.maxMana:#,##0}";
		}
	}
}
