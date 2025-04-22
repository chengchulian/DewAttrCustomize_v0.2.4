using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTutorial_Tutorial_Hunt : PlayTutorial_Tutorial
{
	public Shrine[] shrines;

	public Mon_Polaris polaris;

	public GameObject fxImminentHunt;

	public Transform[] hunterSpawnPoses;

	public DewMusicItem huntMusic;

	public Transform polarisRiftPos;

	public Transform playerRiftPos;

	public Rift finalRift;

	public GameObject[] deactivatedObjectsAfterEverything;

	protected override IEnumerator OnStart()
	{
		Shrine[] array = shrines;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].MakeUnavailable();
		}
		ManagerBase<MusicManager>.instance.Stop();
		yield return PauseRoutine(0.5f);
		DewEffect.Play(fxImminentHunt);
		yield return PauseRoutine(2f);
		yield return SayRoutine("PlayTutorial.SomethingIsComing");
		yield return new WaitForSeconds(1.5f);
		ManagerBase<MusicManager>.instance.Play(huntMusic);
		for (int j = 0; j < 8; j++)
		{
			Vector3 end = AbilityTrigger.PredictPoint_Simple(Random.value, base.h, 1f) + Random.insideUnitSphere.Flattened() * 6f;
			end = Dew.GetValidAgentDestination_Closest(base.h.agentPosition, end);
			NetworkedManagerBase<ActorManager>.instance.serverActor.CreateAbilityInstance<Ai_HunterArtillery_Small>(end, null, default(CastInfo));
			yield return new WaitForSeconds(Random.Range(0.025f, 0.1f));
		}
		NetworkedManagerBase<ActorManager>.instance.serverActor.CreateAbilityInstance<Ai_HunterArtillery_Big>(base.h.agentPosition, null, default(CastInfo)).dealtDamageProcessor.Add(delegate(ref DamageData data, Actor actor, Entity target)
		{
			data.ApplyRawMultiplier(0.2f);
		});
		yield return new WaitForSeconds(2f);
		yield return SayRoutine("PlayTutorial.WhatsTheMeaningOfThis");
		At_Mon_Polaris_Smite smite = polaris.Ability.GetAbility<At_Mon_Polaris_Smite>();
		for (int j = 0; j < 2; j++)
		{
			for (int k = 0; k < hunterSpawnPoses.Length; k++)
			{
				Transform transform = hunterSpawnPoses[k];
				Entity entity = ((k % 3 != 0) ? ((k % 3 != 1) ? ((Entity)DewResources.GetByType<Mon_Forest_Hound>()) : ((Entity)DewResources.GetByType<Mon_Forest_SpiderWarrior>())) : DewResources.GetByType<Mon_Forest_Treant>());
				Quaternion value = Quaternion.LookRotation(base.h.position - transform.position).Flattened();
				Entity entity2 = Dew.SpawnEntity(entity, transform.position, value, null, DewPlayer.creep, 1);
				entity2.CreateStatusEffect<Se_HunterBuff>(entity2, new CastInfo(entity2, entity2));
				entity2.dealtDamageProcessor.Add(delegate(ref DamageData data, Actor actor, Entity target)
				{
					data.ApplyRawMultiplier(0.33f);
				});
				entity2.AI.Aggro(base.h);
				AddToSpawnedEntity(entity2);
			}
			bool allDead;
			do
			{
				allDead = true;
				foreach (Entity spawnedEntity in spawnedEntities)
				{
					if (!spawnedEntity.IsNullInactiveDeadOrKnockedOut())
					{
						allDead = false;
						smite.ResetCooldown();
						polaris.Control.Cast(smite, spawnedEntity);
						yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));
						break;
					}
				}
			}
			while (!allDead);
			spawnedEntities.Clear();
		}
		yield return PauseRoutine(1.5f);
		Vector3 goodRewardPosition = Dew.GetGoodRewardPosition(base.h.position);
		yield return BlinkPolarisRoutine(goodRewardPosition);
		yield return SayRoutine("PlayTutorial.Hunters");
		yield return PauseRoutine(1f);
		yield return SayRoutine("PlayTutorial.YouAreAboutToDie");
		yield return PauseRoutine(0.5f);
		yield return BlinkPolarisRoutine(polarisRiftPos.position);
		ManagerBase<ControlManager>.instance.DisableCharacterControls();
		base.h.Control.MoveToDestination(playerRiftPos.position, immediately: true);
		finalRift.Open();
		yield return new WaitForSeconds(1f);
		base.h.Control.MoveToDestination(playerRiftPos.position, immediately: true);
		yield return WaitForPlayerPrescenceRoutine();
		base.h.Control.RotateTowards(polaris, immediately: true, float.PositiveInfinity);
		ManagerBase<ControlManager>.instance.EnableCharacterControls();
		yield return PauseRoutine(1.5f);
		yield return SayRoutine("PlayTutorial.FacePrimusAeron");
		ManagerBase<ControlManager>.instance.DisableCharacterControls();
		polaris.CreateStatusEffect<Se_Mon_Polaris_Disappear>(polaris, new CastInfo(polaris));
		GameObject[] array2 = deactivatedObjectsAfterEverything;
		foreach (GameObject gameObject in array2)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(value: false);
			}
		}
		yield return PauseRoutine(2f);
		ManagerBase<CameraManager>.instance.DoGenericFadeOut();
		yield return PauseRoutine(1.5f);
		ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
		{
			buttons = DewMessageSettings.ButtonType.Ok,
			rawContent = DewLocalization.GetUIValue("PlayTutorial_Message_EndTutorialGuidebook")
		});
		yield return new WaitWhile(() => ManagerBase<MessageManager>.instance.isShowingMessage);
		GoToPlayLobby();
	}

	public static async UniTaskVoid GoToPlayLobby()
	{
		DewNetworkManager.networkMode = DewNetworkManager.Mode.Singleplayer;
		PlayLobbyManager.isFirstTimePlayFlow = false;
		ManagerBase<TransitionManager>.instance.FadeOut(showTips: false);
		if (ManagerBase<AchievementManager>.instance.isTrackingAchievements)
		{
			ManagerBase<AchievementManager>.instance.StopTrackingAchievements();
		}
		await UniTask.WaitForSeconds(1f, ignoreTimeScale: true);
		NetworkServer.Shutdown();
		NetworkClient.Shutdown();
		if (ManagerBase<GameLogicPackage>.instance != null)
		{
			Object.Destroy(ManagerBase<GameLogicPackage>.instance.gameObject);
		}
		Object.Destroy(ManagerBase<NetworkLogicPackage>.instance.gameObject);
		await UniTask.WaitForSeconds(0.25f, ignoreTimeScale: true);
		SceneManager.LoadScene("PlayLobby");
	}

	private IEnumerator SayRoutine(string key)
	{
		yield return NetworkedManagerBase<ConversationManager>.instance.StartConversationRoutine(new DewConversationSettings
		{
			player = DewPlayer.local,
			rotateTowardsCenter = true,
			speakers = new Entity[2] { base.h, polaris },
			startConversationKey = key,
			visibility = ConversationVisibility.Everyone
		});
	}

	private IEnumerator PauseRoutine(float duration)
	{
		ManagerBase<ControlManager>.instance.DisableCharacterControls();
		yield return new WaitForSeconds(duration);
		ManagerBase<ControlManager>.instance.EnableCharacterControls();
	}

	private IEnumerator WaitForPlayerPrescenceRoutine()
	{
		ManagerBase<ObjectiveArrowManager>.instance.objectivePosition = polaris.position;
		yield return new WaitWhile(() => Vector2.Distance(polaris.agentPosition.ToXY(), base.h.agentPosition.ToXY()) > 4.65f);
		ManagerBase<ObjectiveArrowManager>.instance.objectivePosition = null;
	}

	private IEnumerator BlinkPolarisRoutine(Vector3 destination)
	{
		polaris.Control.RotateTowards(base.h, immediately: false);
		Se_Mon_Polaris_Blink se = polaris.CreateStatusEffect<Se_Mon_Polaris_Blink>(polaris, new CastInfo(polaris, destination));
		yield return new WaitWhile(() => !se.IsNullOrInactive());
		polaris.Control.RotateTowards(base.h, immediately: false, float.PositiveInfinity);
	}

	protected override void OnCleanup()
	{
		Shrine[] array = shrines;
		foreach (Shrine shrine in array)
		{
			if (shrine != null)
			{
				shrine.MakeAvailable();
			}
		}
		if (ManagerBase<CameraManager>.instance != null)
		{
			ManagerBase<CameraManager>.instance.DoGenericFadeIn(immediately: true);
		}
		DewEffect.Stop(fxImminentHunt);
		if (polaris != null && polaris.Status.TryGetStatusEffect<Se_Mon_Polaris_Disappear>(out var effect))
		{
			effect.Destroy();
		}
		if (ManagerBase<MusicManager>.instance != null)
		{
			ManagerBase<MusicManager>.instance.Play(SingletonDewNetworkBehaviour<Room>.instance.music);
		}
		if (ManagerBase<ControlManager>.instance != null)
		{
			while (!ManagerBase<ControlManager>.instance.isCharacterControlEnabled)
			{
				ManagerBase<ControlManager>.instance.EnableCharacterControls();
			}
		}
		uint[] array2 = NetworkedManagerBase<ConversationManager>.instance.convSettings.Keys.ToArray();
		foreach (uint id in array2)
		{
			NetworkedManagerBase<ConversationManager>.instance.StopConversation(id);
		}
		GameObject[] array3 = deactivatedObjectsAfterEverything;
		foreach (GameObject gameObject in array3)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(value: true);
			}
		}
		if (finalRift != null)
		{
			finalRift.Close();
		}
	}
}
