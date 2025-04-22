using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_DamageNumberGroup : SingletonBehaviour<UI_DamageNumberGroup>
{
	[Serializable]
	public class NumberPool
	{
		public UI_DamageNumberText prefab;

		public int count = 15;

		private int _cursor;

		private UI_DamageNumberText[] _pool;

		private List<UI_DamageNumberText> _active;

		public void Init(Transform parent)
		{
			if (_pool == null)
			{
				_pool = new UI_DamageNumberText[count];
			}
			for (int i = 0; i < _pool.Length; i++)
			{
				if (_pool[i] == null)
				{
					_pool[i] = global::UnityEngine.Object.Instantiate(prefab, parent);
				}
				_pool[i].gameObject.SetActive(value: false);
			}
			_active = new List<UI_DamageNumberText>(count);
		}

		public void Cleanup()
		{
			for (int i = 0; i < _pool.Length; i++)
			{
				_pool[i].gameObject.SetActive(value: false);
			}
			_active.Clear();
		}

		public void Handle(EventInfoDamage info)
		{
			for (int i = 0; i < _active.Count; i++)
			{
				if (_active[i].CanMerge(info))
				{
					_active[i].Merge(info);
					return;
				}
			}
			if (!_active.Contains(_pool[_cursor]))
			{
				_active.Add(_pool[_cursor]);
			}
			UI_DamageNumberText curr = _pool[_cursor];
			_pool[_cursor].Setup(info, delegate
			{
				_active.Remove(curr);
			});
			_cursor++;
			if (_cursor >= _pool.Length)
			{
				_cursor = 0;
			}
		}

		public void Handle(EventInfoHeal info)
		{
			for (int i = 0; i < _active.Count; i++)
			{
				if (_active[i].CanMerge(info))
				{
					_active[i].Merge(info);
					return;
				}
			}
			if (!_active.Contains(_pool[_cursor]))
			{
				_active.Add(_pool[_cursor]);
			}
			UI_DamageNumberText curr = _pool[_cursor];
			_pool[_cursor].Setup(info, delegate
			{
				_active.Remove(curr);
			});
			_cursor++;
			if (_cursor >= _pool.Length)
			{
				_cursor = 0;
			}
		}
	}

	private class Ad_IgnoreCCText
	{
		public float lastShowTime;
	}

	public NumberPool damagePrefab;

	public NumberPool damageCritPrefab;

	public NumberPool healPrefab;

	public NumberPool healCritPrefab;

	public NumberPool invulnerablePrefab;

	public NumberPool ccImmunityPrefab;

	public UI_InGame_WorldMessage worldMessagePrefab;

	private void OnEnable()
	{
		NetworkedManagerBase<ClientEventManager>.instance.OnTakeDamage += new Action<EventInfoDamage>(HandleTakeDamage);
		NetworkedManagerBase<ClientEventManager>.instance.OnTakeHeal += new Action<EventInfoHeal>(HandleTakeHeal);
		NetworkedManagerBase<ClientEventManager>.instance.OnDamageNegated += new Action<EventInfoDamageNegatedByImmunity>(HandleDamageNegated);
		NetworkedManagerBase<ClientEventManager>.instance.OnIgnoreCC += new Action<Entity>(HandleIgnoreCC);
		InGameUIManager inGameUIManager = InGameUIManager.instance;
		inGameUIManager.onShowWorldMessage = (Action<WorldMessageSetting>)Delegate.Combine(inGameUIManager.onShowWorldMessage, new Action<WorldMessageSetting>(OnShowWorldMessage));
		damagePrefab.Init(base.transform);
		damageCritPrefab.Init(base.transform);
		healPrefab.Init(base.transform);
		healCritPrefab.Init(base.transform);
		invulnerablePrefab.Init(base.transform);
		ccImmunityPrefab.Init(base.transform);
	}

	private void OnDisable()
	{
		if (NetworkedManagerBase<ClientEventManager>.instance != null)
		{
			NetworkedManagerBase<ClientEventManager>.instance.OnTakeDamage -= new Action<EventInfoDamage>(HandleTakeDamage);
			NetworkedManagerBase<ClientEventManager>.instance.OnTakeHeal -= new Action<EventInfoHeal>(HandleTakeHeal);
			NetworkedManagerBase<ClientEventManager>.instance.OnDamageNegated -= new Action<EventInfoDamageNegatedByImmunity>(HandleDamageNegated);
			NetworkedManagerBase<ClientEventManager>.instance.OnIgnoreCC -= new Action<Entity>(HandleIgnoreCC);
		}
		if (InGameUIManager.instance != null)
		{
			InGameUIManager inGameUIManager = InGameUIManager.instance;
			inGameUIManager.onShowWorldMessage = (Action<WorldMessageSetting>)Delegate.Remove(inGameUIManager.onShowWorldMessage, new Action<WorldMessageSetting>(OnShowWorldMessage));
		}
		damagePrefab.Cleanup();
		damageCritPrefab.Cleanup();
		healPrefab.Cleanup();
		healCritPrefab.Cleanup();
		invulnerablePrefab.Cleanup();
		ccImmunityPrefab.Cleanup();
	}

	private void OnShowWorldMessage(WorldMessageSetting obj)
	{
		global::UnityEngine.Object.Instantiate(worldMessagePrefab, base.transform).message = obj;
	}

	private void HandleTakeDamage(EventInfoDamage info)
	{
		switch (DewSave.profile.gameplay.damageNumberVisibility)
		{
		case DamageNumberVisibility.Off:
			return;
		case DamageNumberVisibility.MineOnly:
		{
			Entity fe = ManagerBase<CameraManager>.instance.focusedEntity;
			if (fe == null || (info.actor != fe && !info.actor.IsDescendantOf(fe) && info.victim != fe && !info.victim.IsDescendantOf(fe)))
			{
				return;
			}
			break;
		}
		}
		if (info.damage.HasAttr(DamageAttribute.IsCrit))
		{
			damageCritPrefab.Handle(info);
		}
		else
		{
			damagePrefab.Handle(info);
		}
	}

	private void HandleTakeHeal(EventInfoHeal info)
	{
		switch (DewSave.profile.gameplay.damageNumberVisibility)
		{
		case DamageNumberVisibility.Off:
			return;
		case DamageNumberVisibility.MineOnly:
		{
			Entity fe = ManagerBase<CameraManager>.instance.focusedEntity;
			if (fe == null || (info.actor != fe && !info.actor.IsDescendantOf(fe) && info.target != fe && !info.target.IsDescendantOf(fe)))
			{
				return;
			}
			break;
		}
		}
		if (info.isCrit)
		{
			healCritPrefab.Handle(info);
		}
		else
		{
			healPrefab.Handle(info);
		}
	}

	private void HandleDamageNegated(EventInfoDamageNegatedByImmunity obj)
	{
		invulnerablePrefab.Handle(new EventInfoDamage
		{
			victim = obj.victim,
			damage = new FinalDamageData
			{
				amount = -1f
			}
		});
	}

	private void HandleIgnoreCC(Entity victim)
	{
		if (!victim.IsNullOrInactive() && victim.Status.hasMirageSkin)
		{
			if (!victim.TryGetData<Ad_IgnoreCCText>(out var ccText))
			{
				ccText = new Ad_IgnoreCCText();
				victim.AddData(ccText);
			}
			if (!(Time.time - ccText.lastShowTime < 3f))
			{
				ccText.lastShowTime = Time.time;
				ccImmunityPrefab.Handle(new EventInfoDamage
				{
					victim = victim,
					damage = new FinalDamageData
					{
						amount = -2f
					}
				});
			}
		}
	}
}
