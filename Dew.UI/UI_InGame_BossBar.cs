using System;
using System.Collections;
using TMPro;
using UnityEngine;

[LogicUpdatePriority(600)]
public class UI_InGame_BossBar : LogicBehaviour
{
	public TextMeshProUGUI healthText;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI subtitleText;

	private UI_EntityProvider _provider;

	private Canvas _canvas;

	public bool isShown => _canvas.enabled;

	private void Awake()
	{
		GetComponent(out _canvas);
		GetComponent(out _provider);
		_canvas.enabled = false;
	}

	private void Start()
	{
		NetworkedManagerBase<ActorManager>.instance.onEntityAdd += new Action<Entity>(OnEntityAdd);
	}

	private void OnEntityAdd(Entity obj)
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			if (obj is Monster { type: Monster.MonsterType.Boss })
			{
				do
				{
					yield return new WaitForSeconds(0.1f);
					if (obj.IsNullOrInactive())
					{
						yield break;
					}
				}
				while (obj.Visual.isSpawning || ManagerBase<CameraManager>.instance.isPlayingCutscene);
				_provider.target = obj;
				nameText.text = DewLocalization.GetUIValue(obj.GetType().Name + "_Name");
				subtitleText.text = DewLocalization.GetUIValue(obj.GetType().Name + "_Subtitle");
			}
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		_canvas.enabled = _provider.target != null && !_provider.target.IsNullOrInactive();
		if (_canvas.enabled)
		{
			Entity target = _provider.target;
			if (target.Status.currentShield > 0.01f)
			{
				healthText.text = $"{target.currentHealth:#,##0}/{target.maxHealth:#,##0} (+{target.Status.currentShield:#,##0})";
			}
			else
			{
				healthText.text = $"{target.currentHealth:#,##0}/{target.maxHealth:#,##0}";
			}
		}
	}
}
