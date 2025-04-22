using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_PlayRewardAnnouncer : SingletonBehaviour<UI_Lobby_PlayRewardAnnouncer>
{
	public Button buttonObject;

	public GameObject fxNext;

	public GameObject fxEnd;

	public GameObject achObject;

	public UI_Achievement_Icon achIcon;

	public UI_Achievement_AddedToCollectablesText achAddedToCollectablesText;

	public TextMeshProUGUI achNameText;

	public TextMeshProUGUI achDescriptionText;

	public GameObject artObject;

	public UI_Achievement_Icon artIcon;

	public TextMeshProUGUI artNameText;

	public TextMeshProUGUI artDescriptionText;

	public GameObject discoverObject;

	public UI_Achievement_Icon discoverItemTemplate;

	public Transform discoverItemsParent;

	public GameObject masteryObject;

	public UI_HeroIcon masteryHeroIcon;

	public TextMeshProUGUI masteryHeroName;

	public TextMeshProUGUI masteryLevelText;

	public Image masteryPointsFill;

	public Image masteryPointsDelta;

	public GameObject fxMasteryLevelUp;

	public DewAudioSource fxMasteryTick;

	public GameObject fxMasteryFinish;

	public GameObject masteryFlasher;

	public TextMeshProUGUI masteryGainedPointsText;

	private List<UI_Achievement_Icon> _discoverItemsInstances = new List<UI_Achievement_Icon>();

	private bool _clickedNext;

	private CanvasGroup _cg;

	private LastGamePlayReward _reward;

	private float _cv;

	protected override void Awake()
	{
		base.Awake();
		_cg = GetComponent<CanvasGroup>();
		_cg.SetActivationState(value: false);
		buttonObject.onClick.AddListener(ClickNext);
		discoverItemTemplate.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		LobbyUIManager lobbyUIManager = LobbyUIManager.instance;
		lobbyUIManager.onStateChanged = (Action<string, string>)Delegate.Combine(lobbyUIManager.onStateChanged, new Action<string, string>(OnStateChanged));
		OnStateChanged(null, null);
	}

	private void OnStateChanged(string _, string __)
	{
		if (!(LobbyUIManager.instance.state != "Lobby") && AchievementManager.lastGamePlayReward != null)
		{
			_reward = AchievementManager.lastGamePlayReward;
			AchievementManager.lastGamePlayReward = null;
			if (_reward.unlockedAchievements.Count != 0 || _reward.discoveredSkills.Count != 0 || _reward.discoveredGems.Count != 0 || _reward.heroMasteryPoints > 0)
			{
				_cg.SetActivationState(value: true);
				achObject.SetActive(value: false);
				artObject.SetActive(value: false);
				discoverObject.SetActive(value: false);
				masteryObject.SetActive(value: false);
				buttonObject.gameObject.SetActive(value: false);
				StartCoroutine(Routine());
			}
		}
		IEnumerator Routine()
		{
			for (int i = 0; i < _reward.unlockedAchievements.Count; i++)
			{
				achObject.SetActive(value: true);
				achObject.transform.DOKill(complete: true);
				achObject.transform.localScale = Vector3.one * 0.9f;
				achObject.transform.DOScale(Vector3.one, 0.15f);
				Type ach = _reward.unlockedAchievements[i];
				achIcon.Setup(ach);
				achNameText.text = DewLocalization.GetAchievementName(ach.Name);
				achNameText.color = Dew.GetAchievementColor(ach);
				achDescriptionText.text = DewLocalization.GetAchievementDescription(ach.Name);
				achAddedToCollectablesText.Setup(Dew.GetUnlockedTargetsOfAchievement(ach)[0]);
				DewEffect.Play(fxNext);
				yield return WaitForClickNext();
				achObject.SetActive(value: false);
			}
			for (int i = 0; i < _reward.unlockedArtifacts.Count; i++)
			{
				artObject.SetActive(value: true);
				artObject.transform.DOKill(complete: true);
				artObject.transform.localScale = Vector3.one * 0.9f;
				artObject.transform.DOScale(Vector3.one, 0.15f);
				string artName = _reward.unlockedArtifacts[i];
				Artifact art = DewResources.GetByShortTypeName<Artifact>(artName);
				artIcon.SetupByItem(art);
				artNameText.text = DewLocalization.GetArtifactName(DewLocalization.GetArtifactKey(artName));
				artNameText.color = art.mainColor.WithA(1f);
				artDescriptionText.text = DewLocalization.GetArtifactShortStory(DewLocalization.GetArtifactKey(artName))[0];
				DewEffect.Play(fxNext);
				yield return WaitForClickNext();
				artObject.SetActive(value: false);
			}
			if (_reward.discoveredSkills.Count > 0 || _reward.discoveredGems.Count > 0)
			{
				discoverObject.SetActive(value: true);
				discoverObject.transform.DOKill(complete: true);
				discoverObject.transform.localScale = Vector3.one * 0.9f;
				discoverObject.transform.DOScale(Vector3.one, 0.15f);
				int count = _reward.discoveredSkills.Count + _reward.discoveredGems.Count;
				if (count > 20)
				{
					count = 20;
				}
				while (_discoverItemsInstances.Count > count)
				{
					global::UnityEngine.Object.Destroy(_discoverItemsInstances[0].gameObject);
					_discoverItemsInstances.RemoveAt(0);
				}
				while (_discoverItemsInstances.Count < count)
				{
					UI_Achievement_Icon newItem = global::UnityEngine.Object.Instantiate(discoverItemTemplate, discoverItemsParent);
					newItem.gameObject.SetActive(value: true);
					_discoverItemsInstances.Add(newItem);
				}
				for (int j = 0; j < _reward.discoveredSkills.Count && j < _discoverItemsInstances.Count; j++)
				{
					_discoverItemsInstances[j].SetupByItem(DewResources.GetByType(_reward.discoveredSkills[j]));
				}
				for (int k = 0; k < _reward.discoveredGems.Count && k + _reward.discoveredSkills.Count < _discoverItemsInstances.Count; k++)
				{
					_discoverItemsInstances[_reward.discoveredSkills.Count + k].SetupByItem(DewResources.GetByType(_reward.discoveredGems[k]));
				}
				DewEffect.Play(fxNext);
				yield return WaitForClickNext();
				discoverObject.SetActive(value: false);
			}
			if (_reward.heroMasteryPoints > 0 && DewSave.profile.heroMasteries.TryGetValue(_reward.heroType, out var mastery))
			{
				masteryGainedPointsText.text = "";
				masteryFlasher.SetActive(value: false);
				masteryObject.SetActive(value: true);
				masteryObject.transform.DOKill(complete: true);
				masteryObject.transform.localScale = Vector3.one * 0.9f;
				masteryObject.transform.DOScale(Vector3.one, 0.15f);
				int startLevel = mastery.currentLevel;
				long startPoints;
				for (startPoints = mastery.currentPoints - _reward.heroMasteryPoints; startPoints < 0; startPoints += Dew.GetRequiredMasteryPointsToLevelUp(startLevel))
				{
					startLevel--;
				}
				if (startLevel < 1)
				{
					startLevel = 1;
				}
				masteryHeroIcon.Setup(_reward.heroType);
				masteryHeroName.text = DewLocalization.GetUIValue(_reward.heroType + "_Name");
				masteryHeroName.color = DewResources.GetByShortTypeName<Hero>(_reward.heroType).mainColor;
				int i = startLevel;
				float currentPoints = startPoints;
				masteryLevelText.text = i.ToString();
				masteryPointsFill.fillAmount = currentPoints / (float)Dew.GetRequiredMasteryPointsToLevelUp(i);
				masteryPointsDelta.fillAmount = masteryPointsFill.fillAmount;
				float duration = Mathf.Clamp((float)_reward.heroMasteryPoints / 13000f, 1f, 4f);
				float startTime = Time.time;
				float lastTickTime = Time.time;
				while (!(Time.time - startTime > 0.4f) || (!DewInput.GetButtonDown(GamepadButtonEx.A) && !DewInput.GetButtonDownAnyMouse()))
				{
					if (Time.time - lastTickTime > 0.075f)
					{
						lastTickTime += 0.075f;
						fxMasteryTick.pitchMultiplier = 0.85f + (Time.time - startTime) * 0.25f;
						fxMasteryTick.volumeMultiplier = Mathf.Clamp01(0.6f + (Time.time - startTime) / duration);
						DewEffect.PlayNew(fxMasteryTick.gameObject);
					}
					if (i == mastery.currentLevel && currentPoints > (float)mastery.currentPoints)
					{
						break;
					}
					float speed = Mathf.Max(DewEase.EaseInOutQuad.GetDelta((Time.time - startTime) / duration), (Time.time - startTime) / duration * 0.05f) * (float)_reward.heroMasteryPoints / duration * 2f;
					currentPoints += speed * Time.deltaTime;
					if (currentPoints > (float)Dew.GetRequiredMasteryPointsToLevelUp(i))
					{
						masteryFlasher.SetActive(value: false);
						masteryFlasher.SetActive(value: true);
						currentPoints -= (float)Dew.GetRequiredMasteryPointsToLevelUp(i);
						i++;
						masteryLevelText.text = i.ToString();
						DewEffect.PlayNew(fxMasteryLevelUp);
						masteryPointsFill.fillAmount = 0f;
					}
					masteryPointsDelta.fillAmount = currentPoints / (float)Dew.GetRequiredMasteryPointsToLevelUp(i);
					yield return null;
				}
				if (mastery.currentLevel.ToString() != masteryLevelText.text)
				{
					masteryLevelText.text = mastery.currentLevel.ToString();
					DewEffect.PlayNew(fxMasteryLevelUp);
					masteryPointsFill.fillAmount = 0f;
				}
				masteryPointsDelta.fillAmount = (float)mastery.currentPoints / (float)Dew.GetRequiredMasteryPointsToLevelUp(mastery.currentLevel);
				DewEffect.PlayNew(fxMasteryFinish);
				masteryGainedPointsText.text = string.Format(DewLocalization.GetUIValue("TravelerMastery_AddedPoints"), _reward.heroMasteryPoints.ToString("#,##0"));
				yield return WaitForClickNext();
				masteryObject.SetActive(value: false);
			}
			DewEffect.Play(fxEnd);
			_cg.alpha = 0f;
			yield return new WaitForSeconds(0.4f);
			_cg.SetActivationState(value: false);
		}
	}

	private void Update()
	{
		if (masteryObject.activeInHierarchy)
		{
			masteryPointsFill.fillAmount = Mathf.SmoothDamp(masteryPointsFill.fillAmount, masteryPointsDelta.fillAmount, ref _cv, 0.2f);
		}
	}

	public IEnumerator WaitForClickNext()
	{
		_clickedNext = false;
		yield return new WaitForSeconds(0.65f);
		buttonObject.gameObject.SetActive(value: true);
		yield return new WaitWhile(() => !_clickedNext);
	}

	private void ClickNext()
	{
		_clickedNext = true;
		buttonObject.gameObject.SetActive(value: false);
	}
}
