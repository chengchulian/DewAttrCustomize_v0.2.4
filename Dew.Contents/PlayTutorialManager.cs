using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayTutorialManager : GameManager
{
	public PlayTutorial_Tutorial[] tutorials;

	public RectTransform[] qwerArrowPivots;

	public RectTransform[] qwerButtons;

	public GameObject editSkillIndicator;

	public RectTransform huntDisplay;

	public RectTransform skillButtons;

	public float skillButtonsOffset;

	public float huntDisplayOffset;

	public float skillButtonsAnimateTime;

	private Vector3? _originalHuntPos;

	private Vector3? _originalSkillAnchorPos;

	private Dictionary<HeroSkillLocation, SkillTrigger> _unequipped = new Dictionary<HeroSkillLocation, SkillTrigger>();

	public new static PlayTutorialManager instance => (PlayTutorialManager)NetworkedManagerBase<GameManager>.instance;

	public RectTransform GetSkillArrowPivot(HeroSkillLocation type)
	{
		return type switch
		{
			HeroSkillLocation.Q => qwerArrowPivots[0], 
			HeroSkillLocation.W => qwerArrowPivots[1], 
			HeroSkillLocation.E => qwerArrowPivots[2], 
			HeroSkillLocation.R => qwerArrowPivots[3], 
			HeroSkillLocation.Identity => qwerArrowPivots[4], 
			_ => qwerArrowPivots[0], 
		};
	}

	public RectTransform GetSkillButton(HeroSkillLocation type)
	{
		return type switch
		{
			HeroSkillLocation.Q => qwerButtons[0], 
			HeroSkillLocation.W => qwerButtons[1], 
			HeroSkillLocation.E => qwerButtons[2], 
			HeroSkillLocation.R => qwerButtons[3], 
			HeroSkillLocation.Identity => qwerButtons[4], 
			_ => qwerButtons[0], 
		};
	}

	protected override void Awake()
	{
		base.Awake();
		base.difficulty = GetDifficulty();
	}

	public override void OnStart()
	{
		base.OnStart();
		ManagerBase<CameraManager>.instance.DoGenericFadeOut(immediately: true);
		base.isGameOverEnabled = false;
	}

	public override void OnLateStartServer()
	{
		base.OnLateStartServer();
		StartCoroutine(SetupRoutine());
		IEnumerator SetupRoutine()
		{
			DewSave.profile.didPlayTutorial = true;
			DewSave.SaveProfile();
			ManagerBase<ControlManager>.instance.DisableCharacterControls();
			ManagerBase<ControlManager>.instance.isWorldMapDisabled = true;
			yield return Dew.WaitForClientsReadyRoutine();
			NetworkedManagerBase<ZoneManager>.instance.currentZone = Object.FindObjectOfType<Zone>();
			NetworkedManagerBase<ZoneManager>.instance.currentRoomIndex = 0;
			NetworkedManagerBase<ZoneManager>.instance.currentZoneClearedNodes = 0;
			NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex = 0;
			NetworkedManagerBase<ZoneManager>.instance.currentRoom = SingletonDewNetworkBehaviour<Room>.instance;
			NetworkedManagerBase<ZoneManager>.instance.nodes.Clear();
			Vector2 vector = new Vector2(ZoneManager.WorldWidth / 2f, ZoneManager.WorldHeight / 2f);
			NetworkedManagerBase<ZoneManager>.instance.nodes.Add(new WorldNodeData
			{
				type = WorldNodeType.Start,
				position = vector + new Vector2(-17f, -17f),
				modifiers = new List<ModifierData>(),
				room = "",
				status = WorldNodeStatus.Revealed
			});
			NetworkedManagerBase<ZoneManager>.instance.nodes.Add(new WorldNodeData
			{
				type = WorldNodeType.Combat,
				position = vector + new Vector2(17f, 17f),
				modifiers = new List<ModifierData>
				{
					new ModifierData("RoomMod_Tutorial_Voice")
				},
				room = "",
				status = WorldNodeStatus.Revealed
			});
			NetworkedManagerBase<ZoneManager>.instance.hunterStartNodeIndex = -1;
			NetworkedManagerBase<ZoneManager>.instance.hunterStatuses.Add(HunterStatus.None);
			NetworkedManagerBase<ZoneManager>.instance.hunterStatuses.Add(HunterStatus.None);
			NetworkedManagerBase<ZoneManager>.instance.nodeDistanceMatrix.AddRange(new int[4] { 0, 1, 1, 0 });
			NetworkedManagerBase<ZoneManager>.instance.visitedNodesSaveData = new List<WorldNodeSaveData>();
			for (int i = 0; i < 2; i++)
			{
				NetworkedManagerBase<ZoneManager>.instance.visitedNodesSaveData.Add(null);
			}
			NetworkedManagerBase<ZoneManager>.instance.SetCurrentNodeIndexAndRevealAdjacent(0);
			Hero_Lacerta byType = DewResources.GetByType<Hero_Lacerta>();
			Vector3 position = Object.FindObjectOfType<Room_HeroSpawnPos>().transform.position;
			Quaternion heroSpawnRotation = SingletonDewNetworkBehaviour<Room>.instance.GetHeroSpawnRotation();
			Hero_Lacerta newHero = Dew.SpawnHero(byType, position, heroSpawnRotation, DewPlayer.local, 1, new HeroLoadoutData());
			DewPlayer.local.hero = newHero;
			SingletonDewNetworkBehaviour<Room>.instance.StartRoom(null);
			newHero.CreateStatusEffect<Se_PlayTutorialHeroProtection>(newHero, default(CastInfo));
			DewPlayer.local.hero = newHero;
			DewPlayer.local.controllingEntity = newHero;
			yield return new WaitForSeconds(0.5f);
			newHero.Skill.Q.configs[0].cooldownTime = 3f;
			newHero.Skill.R.configs[0].cooldownTime = 6f;
			LockSkills();
			LockEditSkill();
			ManagerBase<TransitionManager>.instance.FadeIn();
			yield return new WaitForSeconds(0.5f);
			InGameUIManager.instance.SetState("Playing");
			SetTutorial(0);
			ManagerBase<ControlManager>.instance.EnableCharacterControls();
		}
	}

	public override void OnStopClient()
	{
		base.OnStopClient();
		if (InGameUIManager.instance != null)
		{
			InGameUIManager.instance.SetState("Loading");
		}
	}

	protected override DewDifficultySettings GetDifficulty()
	{
		return DewResources.GetByName<DewDifficultySettings>("diffTutorial");
	}

	public void AdvanceTutorial()
	{
		int num = -1;
		for (int i = 0; i < tutorials.Length; i++)
		{
			if (tutorials[i].gameObject.activeSelf)
			{
				num = i;
			}
		}
		SetTutorial(num + 1);
	}

	public void SetTutorial(int index)
	{
		for (int i = 0; i < tutorials.Length; i++)
		{
			if (index != i)
			{
				tutorials[i].gameObject.SetActive(value: false);
			}
		}
		for (int j = 0; j < tutorials.Length; j++)
		{
			if (index == j)
			{
				tutorials[j].gameObject.SetActive(value: true);
			}
		}
	}

	public void LockSkills()
	{
		ManagerBase<ControlManager>.instance.isMainSkillDisabled = true;
		if (!_originalSkillAnchorPos.HasValue)
		{
			_originalSkillAnchorPos = skillButtons.anchoredPosition;
		}
		skillButtons.anchoredPosition = _originalSkillAnchorPos.Value + Vector3.down * skillButtonsOffset;
		HeroSkillLocation[] heroSkillTypes = ControlManager.HeroSkillTypes;
		foreach (HeroSkillLocation heroSkillLocation in heroSkillTypes)
		{
			if (heroSkillLocation != HeroSkillLocation.Movement)
			{
				SkillTrigger skill = DewPlayer.local.hero.Skill.GetSkill(heroSkillLocation);
				if (!(skill == null))
				{
					_unequipped.Add(heroSkillLocation, skill);
					DewPlayer.local.hero.Skill.UnequipSkill(heroSkillLocation, default(Vector3), ignoreCanReplace: true).gameObject.SetActive(value: false);
				}
			}
		}
	}

	public void UnlockSkills()
	{
		if (ManagerBase<ControlManager>.instance != null)
		{
			ManagerBase<ControlManager>.instance.isMainSkillDisabled = false;
		}
		if (!_originalSkillAnchorPos.HasValue)
		{
			return;
		}
		skillButtons.DOAnchorPos(_originalSkillAnchorPos.Value, skillButtonsAnimateTime, snapping: true);
		foreach (KeyValuePair<HeroSkillLocation, SkillTrigger> item in _unequipped)
		{
			if (!(item.Value == null))
			{
				item.Value.gameObject.SetActive(value: true);
				if (DewPlayer.local != null && DewPlayer.local.hero != null)
				{
					DewPlayer.local.hero.Skill.EquipSkill(item.Key, item.Value, ignoreCanReplace: true);
				}
			}
		}
		_unequipped.Clear();
	}

	public void LockHunt()
	{
		if (!_originalHuntPos.HasValue)
		{
			_originalHuntPos = huntDisplay.position;
		}
		huntDisplay.position = _originalHuntPos.Value + Vector3.up * huntDisplayOffset;
	}

	public void UnlockHunt()
	{
		if (_originalHuntPos.HasValue)
		{
			huntDisplay.DOMove(_originalHuntPos.Value, skillButtonsAnimateTime, snapping: true);
		}
	}

	public void LockDodge()
	{
		ManagerBase<ControlManager>.instance.isDodgeDisabled = true;
	}

	public void UnlockDodge()
	{
		ManagerBase<ControlManager>.instance.isDodgeDisabled = false;
	}

	public void LockEditSkill()
	{
		editSkillIndicator.SetActive(value: false);
		ManagerBase<ControlManager>.instance.isEditSkillDisabled = true;
	}

	public void UnlockEditSkill()
	{
		editSkillIndicator.SetActive(value: true);
		ManagerBase<ControlManager>.instance.isEditSkillDisabled = false;
	}

	public void LockDismantle()
	{
		ManagerBase<ControlManager>.instance.isDismantleDisabled = true;
	}

	public void UnlockDismantle()
	{
		ManagerBase<ControlManager>.instance.isDismantleDisabled = false;
	}

	private void MirrorProcessed()
	{
	}
}
