using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using TMPro;
using UnityEngine;

public class UI_InGame_PartyDisplay_Item : LogicBehaviour
{
	private enum ChangeType : byte
	{
		PickedUp,
		Dropped,
		Sell,
		Buy,
		Upgrade,
		MergeGem,
		Dismantled,
		Cleansed
	}

	private const string LevelQualityDisplayTemplate = "<alpha=#AA><voffset=3.5><size=80%>{0}</size></voffset><alpha=#FF>";

	public TextMeshProUGUI nameText;

	public GameObject lowHealthObject;

	public GameObject deadObject;

	public float lowHealthRatio = 0.35f;

	public TextMeshProUGUI gemCountText;

	public RectTransform notiRt;

	public CanvasGroup notiCg;

	public TextMeshProUGUI notiText;

	public Color notiSkillColor;

	public Color notiGemColor;

	public float notiStartTime;

	public float notiSustainTime;

	public float notiDecayTime;

	public Vector3 appearOffset;

	public Vector3 disappearOffset;

	public CanvasGroup[] skillGroups;

	public float disabledSkillGroupAlpha;

	public RectTransform emoteParent;

	private UI_EntityProvider _provider;

	private Vector3 _notiOriginalPos;

	private Emote _currentEmote;

	private Dictionary<Actor, ChangeType> _change = new Dictionary<Actor, ChangeType>();

	public Entity target
	{
		get
		{
			if (!(_provider != null))
			{
				return null;
			}
			return _provider.target;
		}
	}

	protected void Awake()
	{
		_provider = GetComponentInParent<UI_EntityProvider>();
		notiCg.alpha = 0f;
		_notiOriginalPos = notiRt.anchoredPosition;
	}

	private void Start()
	{
		Hero obj = (Hero)target;
		obj.Skill.ClientHeroEvent_OnSkillPickup += new Action<SkillTrigger>(OnSkillPickup);
		obj.Skill.ClientHeroEvent_OnSkillDrop += new Action<SkillTrigger>(OnSkillDrop);
		obj.Skill.ClientHeroEvent_OnGemPickup += new Action<Gem>(OnGemPickup);
		obj.Skill.ClientHeroEvent_OnGemDrop += new Action<Gem>(OnGemDrop);
		obj.Skill.ClientHeroEvent_OnSkillSwap += new Action<HeroSkillLocation, HeroSkillLocation>(OnSkillSwap);
		NetworkedManagerBase<ClientEventManager>.instance.OnItemBought += new Action<Hero, NetworkBehaviour>(OnItemBought);
		NetworkedManagerBase<ClientEventManager>.instance.OnItemSold += new Action<Hero, NetworkBehaviour>(OnItemSold);
		NetworkedManagerBase<ClientEventManager>.instance.OnItemUpgraded += new Action<Hero, NetworkBehaviour>(OnItemUpgraded);
		NetworkedManagerBase<ClientEventManager>.instance.OnGemMergeUpgraded += new Action<Hero, Gem>(OnGemMergeUpgraded);
		NetworkedManagerBase<ClientEventManager>.instance.OnDismantled += new Action<Hero, NetworkBehaviour>(OnDismantled);
		NetworkedManagerBase<ClientEventManager>.instance.OnItemCleansed += new Action<Hero, NetworkBehaviour>(OnItemCleansed);
		nameText.text = target.owner.playerName;
		NetworkedManagerBase<ChatManager>.instance.ClientEvent_OnEmoteReceived += new Action<DewPlayer, string>(ClientEventOnEmoteReceived);
		StartCoroutine(DelayedUpdate());
		IEnumerator DelayedUpdate()
		{
			yield return new WaitForSeconds(0.5f);
			UpdateSkillGemCount();
		}
	}

	private void ClientEventOnEmoteReceived(DewPlayer arg1, string arg2)
	{
		if (arg1 != target.owner)
		{
			return;
		}
		Emote prefab = DewResources.GetByName<Emote>(arg2);
		if (!(prefab == null))
		{
			if (_currentEmote != null)
			{
				global::UnityEngine.Object.Destroy(_currentEmote.gameObject);
				_currentEmote = null;
			}
			_currentEmote = global::UnityEngine.Object.Instantiate(prefab, emoteParent);
			float startUnscaledTime = Time.unscaledTime;
			_currentEmote.posGetter = () => emoteParent.position + Vector3.right * ((Time.unscaledTime - startUnscaledTime) * 6f - 20f);
			_currentEmote.transform.localScale *= 0.7f;
			_currentEmote.customDuration = 2.5f;
			DewAudioSource[] componentsInChildren = _currentEmote.GetComponentsInChildren<DewAudioSource>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				global::UnityEngine.Object.Destroy(componentsInChildren[i]);
			}
		}
	}

	private void OnDestroy()
	{
		Hero h = target as Hero;
		if (h != null)
		{
			h.Skill.ClientHeroEvent_OnSkillPickup -= new Action<SkillTrigger>(OnSkillPickup);
			h.Skill.ClientHeroEvent_OnSkillDrop -= new Action<SkillTrigger>(OnSkillDrop);
			h.Skill.ClientHeroEvent_OnGemPickup -= new Action<Gem>(OnGemPickup);
			h.Skill.ClientHeroEvent_OnGemDrop -= new Action<Gem>(OnGemDrop);
			h.Skill.ClientHeroEvent_OnSkillSwap -= new Action<HeroSkillLocation, HeroSkillLocation>(OnSkillSwap);
		}
		if (NetworkedManagerBase<ClientEventManager>.instance != null)
		{
			NetworkedManagerBase<ClientEventManager>.instance.OnItemBought -= new Action<Hero, NetworkBehaviour>(OnItemBought);
			NetworkedManagerBase<ClientEventManager>.instance.OnItemSold -= new Action<Hero, NetworkBehaviour>(OnItemSold);
			NetworkedManagerBase<ClientEventManager>.instance.OnItemUpgraded -= new Action<Hero, NetworkBehaviour>(OnItemUpgraded);
			NetworkedManagerBase<ClientEventManager>.instance.OnGemMergeUpgraded -= new Action<Hero, Gem>(OnGemMergeUpgraded);
			NetworkedManagerBase<ClientEventManager>.instance.OnDismantled -= new Action<Hero, NetworkBehaviour>(OnDismantled);
		}
		if (NetworkedManagerBase<ChatManager>.instance != null)
		{
			NetworkedManagerBase<ChatManager>.instance.ClientEvent_OnEmoteReceived -= new Action<DewPlayer, string>(ClientEventOnEmoteReceived);
		}
	}

	private void OnSkillSwap(HeroSkillLocation arg1, HeroSkillLocation arg2)
	{
		UpdateSkillGemCount();
	}

	private void OnGemDrop(Gem obj)
	{
		_change[obj] = ChangeType.Dropped;
	}

	private void OnGemPickup(Gem obj)
	{
		_change[obj] = ChangeType.PickedUp;
	}

	private void OnSkillDrop(SkillTrigger obj)
	{
		_change[obj] = ChangeType.Dropped;
	}

	private void OnSkillPickup(SkillTrigger obj)
	{
		_change[obj] = ChangeType.PickedUp;
	}

	private void OnDismantled(Hero h, NetworkBehaviour obj)
	{
		if (!(h != target))
		{
			_change[(Actor)obj] = ChangeType.Dismantled;
		}
	}

	private void OnGemMergeUpgraded(Hero h, Gem obj)
	{
		if (!(h != target))
		{
			_change[obj] = ChangeType.MergeGem;
		}
	}

	private void OnItemUpgraded(Hero h, NetworkBehaviour obj)
	{
		if (!(h != target))
		{
			_change[(Actor)obj] = ChangeType.Upgrade;
		}
	}

	private void OnItemSold(Hero h, NetworkBehaviour obj)
	{
		if (!(h != target))
		{
			_change[(Actor)obj] = ChangeType.Sell;
		}
	}

	private void OnItemCleansed(Hero h, NetworkBehaviour obj)
	{
		if (!(h != target))
		{
			_change[(Actor)obj] = ChangeType.Cleansed;
		}
	}

	private void OnItemBought(Hero h, NetworkBehaviour obj)
	{
		if (!(h != target))
		{
			_change[(Actor)obj] = ChangeType.Buy;
		}
	}

	private void ShowNotification(string rawText, Color color)
	{
		notiText.text = rawText;
		notiText.color = color;
		notiRt.anchoredPosition = _notiOriginalPos + appearOffset;
		notiRt.DOKill();
		DOTween.Sequence().Append(notiRt.DOAnchorPos(_notiOriginalPos, notiStartTime)).AppendInterval(notiSustainTime)
			.Append(notiRt.DOAnchorPos(_notiOriginalPos + disappearOffset, notiDecayTime))
			.SetId(notiRt);
		notiCg.alpha = 0f;
		notiCg.DOKill();
		DOTween.Sequence().Append(notiCg.DOFade(1f, notiStartTime)).AppendInterval(notiSustainTime)
			.Append(notiCg.DOFade(0f, notiDecayTime))
			.SetId(notiCg);
	}

	private void UpdateSkillGemCount()
	{
		Hero h = target as Hero;
		if (!(h == null))
		{
			skillGroups[0].alpha = ((h.Skill.Q != null) ? 1f : disabledSkillGroupAlpha);
			skillGroups[1].alpha = ((h.Skill.W != null) ? 1f : disabledSkillGroupAlpha);
			skillGroups[2].alpha = ((h.Skill.E != null) ? 1f : disabledSkillGroupAlpha);
			skillGroups[3].alpha = ((h.Skill.R != null) ? 1f : disabledSkillGroupAlpha);
			int gems = h.Skill.gems.Count;
			if (h.Skill.holdingObject is Gem)
			{
				gems++;
			}
			gemCountText.text = $"{gems:#,##0}";
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (target == null)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (target is Hero { isKnockedOut: not false })
		{
			deadObject.SetActive(value: true);
			lowHealthObject.SetActive(value: false);
		}
		else if (target.normalizedHealth < lowHealthRatio)
		{
			deadObject.SetActive(value: false);
			lowHealthObject.SetActive(value: true);
		}
		else
		{
			deadObject.SetActive(value: false);
			lowHealthObject.SetActive(value: false);
		}
		DoNotifications();
	}

	private void DoNotifications()
	{
		if (_change.Count == 0)
		{
			return;
		}
		UpdateSkillGemCount();
		Color notiColor = default(Color);
		string text = null;
		SkillTrigger removedSkill = null;
		SkillTrigger addedSkill = null;
		Gem removedGem = null;
		Gem addedGem = null;
		foreach (KeyValuePair<Actor, ChangeType> p in _change)
		{
			notiColor = ((p.Key is SkillTrigger) ? notiSkillColor : notiGemColor);
			string objName = "";
			if (p.Key is SkillTrigger st)
			{
				objName = DewLocalization.GetSkillName(DewLocalization.GetSkillKey(p.Key.GetType()), 0);
				objName += $"<alpha=#AA><voffset=3.5><size=80%>{DewLocalization.GetSkillLevelSuffix(st.level)}</size></voffset><alpha=#FF>";
			}
			else if (p.Key is Gem g)
			{
				objName = DewLocalization.GetGemName(DewLocalization.GetGemKey(p.Key.GetType()));
				objName += $"<alpha=#AA><voffset=3.5><size=80%>{$" {g.quality:#,##0}%"}</size></voffset><alpha=#FF>";
			}
			string template;
			switch (p.Value)
			{
			case ChangeType.PickedUp:
				template = DewLocalization.GetUIValue("Party_PickedUp");
				if (p.Key is SkillTrigger s3)
				{
					addedSkill = s3;
				}
				else if (p.Key is Gem g3)
				{
					addedGem = g3;
				}
				break;
			case ChangeType.Dropped:
				template = DewLocalization.GetUIValue("Party_Dropped");
				if (p.Key is SkillTrigger s2)
				{
					removedSkill = s2;
				}
				else if (p.Key is Gem g2)
				{
					removedGem = g2;
				}
				break;
			case ChangeType.Sell:
				template = DewLocalization.GetUIValue("Party_Sold");
				break;
			case ChangeType.Buy:
				template = DewLocalization.GetUIValue("Party_Bought");
				break;
			case ChangeType.Upgrade:
				template = DewLocalization.GetUIValue("Party_Upgraded");
				break;
			case ChangeType.MergeGem:
				template = DewLocalization.GetUIValue("Party_Merged");
				break;
			case ChangeType.Dismantled:
				template = DewLocalization.GetUIValue("Party_Dismantled");
				break;
			case ChangeType.Cleansed:
				template = DewLocalization.GetUIValue("Party_Cleansed");
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			text = string.Format(template, objName);
		}
		if (text != null)
		{
			string replacedTemplate = DewLocalization.GetUIValue("Party_Replaced");
			if (removedSkill != null && addedSkill != null)
			{
				string oldName = DewLocalization.GetSkillName(DewLocalization.GetSkillKey(removedSkill.GetType()), 0);
				string newName = DewLocalization.GetSkillName(DewLocalization.GetSkillKey(addedSkill.GetType()), 0);
				text = string.Format(replacedTemplate, oldName, newName);
				notiColor = notiSkillColor;
			}
			else if (removedGem != null && addedGem != null)
			{
				string oldName2 = DewLocalization.GetGemName(DewLocalization.GetGemKey(removedGem.GetType()));
				string newName2 = DewLocalization.GetGemName(DewLocalization.GetGemKey(addedGem.GetType()));
				text = string.Format(replacedTemplate, oldName2, newName2);
				notiColor = notiGemColor;
			}
			ShowNotification(text, notiColor);
		}
		_change.Clear();
	}
}
