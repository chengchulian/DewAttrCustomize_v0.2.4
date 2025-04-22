using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using EpicTransport;
using UnityEngine;

public class LobbyServiceEOS : LobbyServiceProvider
{
	public const int ShortLobbyIdLength = 8;

	public readonly string[] AttributeKeys = new string[7] { "version", "hasGameStarted", "isInviteOnly", "name", "difficulty", "hostAddress", "shortCode" };

	private LobbyInstanceEpic _currentLobby;

	private ulong _lobbyMemberStatusNotifyId;

	private ulong _lobbyAttributeUpdateNotifyId;

	private bool _didWaitForSteam;

	public override LobbyInstance currentLobby => _currentLobby;

	private async void Start()
	{
		await UniTask.WaitWhile(() => !EOSSDKComponent.Initialized);
		AddNotifyLobbyMemberStatusReceivedOptions addNotifyLobbyMemberStatusReceivedOptions = default(AddNotifyLobbyMemberStatusReceivedOptions);
		_lobbyMemberStatusNotifyId = EOSSDKComponent.GetLobbyInterface().AddNotifyLobbyMemberStatusReceived(ref addNotifyLobbyMemberStatusReceivedOptions, null, delegate(ref LobbyMemberStatusReceivedCallbackInfo callback)
		{
			if (_currentLobby != null)
			{
				_currentLobby.UpdateMemberList();
			}
			if (callback.CurrentStatus == LobbyMemberStatus.Closed)
			{
				LeaveLobby();
			}
		});
		AddNotifyLobbyUpdateReceivedOptions addNotifyLobbyUpdateReceivedOptions = default(AddNotifyLobbyUpdateReceivedOptions);
		_lobbyAttributeUpdateNotifyId = EOSSDKComponent.GetLobbyInterface().AddNotifyLobbyUpdateReceived(ref addNotifyLobbyUpdateReceivedOptions, null, delegate
		{
			if (_currentLobby != null)
			{
				SetCurrentLobbyData(new LobbyInstanceEpic().ApplyFromHandle(_currentLobby.details));
			}
		});
		Debug.Log("Added notifications to EOS LobbyInterface");
	}

	private void OnDestroy()
	{
		if (!(EOSSDKComponent.Instance == null) && !(EOSSDKComponent.Instance.EOS == null))
		{
			EOSSDKComponent.GetLobbyInterface().RemoveNotifyLobbyMemberStatusReceived(_lobbyMemberStatusNotifyId);
			EOSSDKComponent.GetLobbyInterface().RemoveNotifyLobbyUpdateReceived(_lobbyAttributeUpdateNotifyId);
		}
	}

	private async UniTask EnsureInitialized()
	{
		if (EOSSDKComponent.IsConnecting || !EOSSDKComponent.Initialized)
		{
			Debug.Log("Waiting for EOS");
			await UniTask.WaitWhile(() => EOSSDKComponent.IsConnecting).Timeout(TimeSpan.FromSeconds(10.0));
			if (!EOSSDKComponent.Initialized)
			{
				throw new Exception("EOS is not available");
			}
		}
	}

	public override async UniTask CreateLobby()
	{
		await EnsureInitialized();
		await LeaveLobby();
		LobbyPermissionLevel vis = LobbyPermissionLevel.Publicadvertised;
		CreateLobbyOptions createLobbyOptions = default(CreateLobbyOptions);
		createLobbyOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		createLobbyOptions.MaxLobbyMembers = (uint)GetInitialAttr_maxPlayers();
		createLobbyOptions.PermissionLevel = vis;
		createLobbyOptions.PresenceEnabled = false;
		createLobbyOptions.BucketId = "default";
		CreateLobbyOptions createLobbyOptions2 = createLobbyOptions;
		UniTaskCompletionSource task = new UniTaskCompletionSource();
		ManagerBase<TransitionManager>.instance.UpdateLoadingStatus(LoadingStatus.CreatingLobby);
		EOSSDKComponent.GetLobbyInterface().CreateLobby(ref createLobbyOptions2, null, delegate(ref CreateLobbyCallbackInfo callback)
		{
			try
			{
				List<global::Epic.OnlineServices.Lobby.Attribute> lobbyReturnData;
				LobbyModification modHandle;
				if (callback.ResultCode != 0)
				{
					task.TrySetException(new EOSResultException(callback.ResultCode));
				}
				else
				{
					lobbyReturnData = new List<global::Epic.OnlineServices.Lobby.Attribute>();
					modHandle = new LobbyModification();
					AttributeData attributeData = default(AttributeData);
					attributeData.Key = "default";
					attributeData.Value = "default";
					AttributeData value2 = attributeData;
					UpdateLobbyModificationOptions updateLobbyModificationOptions = default(UpdateLobbyModificationOptions);
					updateLobbyModificationOptions.LobbyId = callback.LobbyId;
					updateLobbyModificationOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
					UpdateLobbyModificationOptions options = updateLobbyModificationOptions;
					EOSSDKComponent.GetLobbyInterface().UpdateLobbyModification(ref options, out modHandle);
					LobbyModificationAddAttributeOptions lobbyModificationAddAttributeOptions = default(LobbyModificationAddAttributeOptions);
					lobbyModificationAddAttributeOptions.Attribute = value2;
					lobbyModificationAddAttributeOptions.Visibility = LobbyAttributeVisibility.Public;
					LobbyModificationAddAttributeOptions options2 = lobbyModificationAddAttributeOptions;
					modHandle.AddAttribute(ref options2);
					AddAttr("version", GetInitialAttr_version());
					AddAttr("hostAddress", GetInitialAttr_hostAddress());
					AddAttr("hasGameStarted", GetInitialAttr_hasGameStarted());
					AddAttr("difficulty", GetInitialAttr_difficulty());
					AddAttr("maxPlayers", GetInitialAttr_maxPlayers());
					AddAttr("name", GetInitialAttr_name());
					AddAttr("isInviteOnly", GetInitialAttr_isInviteOnly());
					Utf8String lobbyId = callback.LobbyId;
					ManagerBase<TransitionManager>.instance.UpdateLoadingStatus(LoadingStatus.PreparingLobby);
					UpdateLobbyOptions updateLobbyOptions = default(UpdateLobbyOptions);
					updateLobbyOptions.LobbyModificationHandle = modHandle;
					UpdateLobbyOptions options3 = updateLobbyOptions;
					EOSSDKComponent.GetLobbyInterface().UpdateLobby(ref options3, null, delegate(ref UpdateLobbyCallbackInfo updateCallback)
					{
						try
						{
							if (updateCallback.ResultCode != 0)
							{
								task.TrySetException(new EOSResultException(updateCallback.ResultCode));
							}
							else
							{
								CopyLobbyDetailsHandleOptions copyLobbyDetailsHandleOptions = default(CopyLobbyDetailsHandleOptions);
								copyLobbyDetailsHandleOptions.LobbyId = lobbyId;
								copyLobbyDetailsHandleOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
								CopyLobbyDetailsHandleOptions options4 = copyLobbyDetailsHandleOptions;
								EOSSDKComponent.GetLobbyInterface().CopyLobbyDetailsHandle(ref options4, out var outLobbyDetailsHandle);
								LobbyInstanceEpic lobbyInstanceEpic = new LobbyInstanceEpic();
								lobbyInstanceEpic.ApplyFromHandle(outLobbyDetailsHandle);
								lobbyInstanceEpic.isLobbyLeader = true;
								SetCurrentLobbyData(lobbyInstanceEpic);
								SetLobbyShortId();
								task.TrySetResult();
							}
						}
						catch (Exception exception)
						{
							task.TrySetException(exception);
						}
					});
				}
				void AddAttr(string key, object value)
				{
					LobbyModificationAddAttributeOptions options5 = default(LobbyModificationAddAttributeOptions);
					options5.Attribute = new AttributeData
					{
						Key = key,
						Value = value.ToAttrDataValue()
					};
					options5.Visibility = LobbyAttributeVisibility.Public;
					modHandle.AddAttribute(ref options5);
					lobbyReturnData.Add(new global::Epic.OnlineServices.Lobby.Attribute
					{
						Data = options5.Attribute,
						Visibility = LobbyAttributeVisibility.Public
					});
				}
			}
			catch (Exception exception2)
			{
				task.TrySetException(exception2);
			}
		});
		await task.Task;
		Debug.Log($"Created EOS Lobby: {currentLobby}");
		StopAllCoroutines();
		StartCoroutine(Heartbeat());
		IEnumerator Heartbeat()
		{
			while (currentLobby != null && currentLobby.isLobbyLeader)
			{
				SetLobbyAttribute("heartbeat", DateTimeOffset.UtcNow.ToUnixTimeSeconds());
				yield return new WaitForSecondsRealtime(15f);
			}
		}
	}

	public override async UniTask JoinLobby(object lobby)
	{
		LobbyDetails details;
		if (lobby is LobbyDetails d)
		{
			details = d;
		}
		else
		{
			if (!(lobby is string id))
			{
				throw new DewException(DewExceptionType.LobbyNotFound);
			}
			details = await GetLobbyById(id);
		}
		await EnsureInitialized();
		await LeaveLobby();
		JoinLobbyOptions joinLobbyOptions = default(JoinLobbyOptions);
		joinLobbyOptions.LobbyDetailsHandle = details;
		joinLobbyOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		joinLobbyOptions.PresenceEnabled = false;
		JoinLobbyOptions joinLobbyOptions2 = joinLobbyOptions;
		ManagerBase<TransitionManager>.instance.UpdateLoadingStatus(LoadingStatus.ConnectingToLobby);
		UniTaskCompletionSource task = new UniTaskCompletionSource();
		EOSSDKComponent.GetLobbyInterface().JoinLobby(ref joinLobbyOptions2, null, delegate(ref JoinLobbyCallbackInfo callback)
		{
			try
			{
				if (callback.ResultCode != 0)
				{
					task.TrySetException(new EOSResultException(callback.ResultCode));
				}
				else
				{
					CopyLobbyDetailsHandleOptions copyLobbyDetailsHandleOptions = default(CopyLobbyDetailsHandleOptions);
					copyLobbyDetailsHandleOptions.LobbyId = callback.LobbyId;
					copyLobbyDetailsHandleOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
					CopyLobbyDetailsHandleOptions options = copyLobbyDetailsHandleOptions;
					EOSSDKComponent.GetLobbyInterface().CopyLobbyDetailsHandle(ref options, out var outLobbyDetailsHandle);
					LobbyInstanceEpic lobbyInstanceEpic = new LobbyInstanceEpic();
					lobbyInstanceEpic.ApplyFromHandle(outLobbyDetailsHandle);
					lobbyInstanceEpic.isLobbyLeader = false;
					SetCurrentLobbyData(lobbyInstanceEpic);
					task.TrySetResult();
				}
			}
			catch (Exception exception)
			{
				task.TrySetException(exception);
			}
		});
		await task.Task;
	}

	public override async UniTask LeaveLobby()
	{
		if (currentLobby == null)
		{
			return;
		}
		await EnsureInitialized();
		UniTaskCompletionSource task = new UniTaskCompletionSource();
		if (currentLobby.isLobbyLeader)
		{
			Debug.Log("Deleting EOS lobby");
			ManagerBase<TransitionManager>.instance.UpdateLoadingStatus(LoadingStatus.CleaningUpPreviousLobby);
			DestroyLobbyOptions destroyLobbyOptions = default(DestroyLobbyOptions);
			destroyLobbyOptions.LobbyId = currentLobby.id;
			destroyLobbyOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
			DestroyLobbyOptions destroyLobbyOptions2 = destroyLobbyOptions;
			EOSSDKComponent.GetLobbyInterface().DestroyLobby(ref destroyLobbyOptions2, null, delegate(ref DestroyLobbyCallbackInfo callback)
			{
				if (callback.ResultCode != 0)
				{
					task.TrySetException(new EOSResultException(callback.ResultCode));
				}
				else
				{
					task.TrySetResult();
				}
			});
			SetCurrentLobbyData(null);
		}
		else
		{
			Debug.Log("Leaving EOS lobby");
			ManagerBase<TransitionManager>.instance.UpdateLoadingStatus(LoadingStatus.LeavingPreviousLobby);
			LeaveLobbyOptions leaveLobbyOptions = default(LeaveLobbyOptions);
			leaveLobbyOptions.LobbyId = currentLobby.id;
			leaveLobbyOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
			LeaveLobbyOptions leaveLobbyOptions2 = leaveLobbyOptions;
			EOSSDKComponent.GetLobbyInterface().LeaveLobby(ref leaveLobbyOptions2, null, delegate(ref LeaveLobbyCallbackInfo callback)
			{
				if (callback.ResultCode != 0 && callback.ResultCode != Result.NotFound)
				{
					task.TrySetException(new EOSResultException(callback.ResultCode));
				}
				else
				{
					task.TrySetResult();
				}
			});
			SetCurrentLobbyData(null);
		}
		try
		{
			await task.Task;
		}
		catch (Exception message)
		{
			Debug.Log("Leave EOS lobby failed");
			Debug.Log(message);
		}
		Debug.Log("EOS Lobby left; LeaveLobby()");
	}

	public override async UniTask HandleUserLeavingGame(string id)
	{
		if (_currentLobby == null || _currentLobby.lobbyMembers.Find((EpicLobbyMember e) => e.handle.ToString() == id) == null)
		{
			return;
		}
		KickMemberOptions kickMemberOptions = default(KickMemberOptions);
		kickMemberOptions.LobbyId = _currentLobby.id;
		kickMemberOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		kickMemberOptions.TargetUserId = ProductUserId.FromString(id);
		KickMemberOptions opt = kickMemberOptions;
		Debug.Log("Kicking user " + id);
		EOSSDKComponent.GetLobbyInterface().KickMember(ref opt, null, delegate(ref KickMemberCallbackInfo data)
		{
			if (data.ResultCode != 0)
			{
				Debug.Log("Failed to kick user " + id + " from EOS lobby: " + data.ResultCode);
			}
			else
			{
				Debug.Log("Kicked user " + id + " from EOS lobby");
			}
		});
	}

	public override async UniTask GetLobbies(Action<LobbySearchResult> onUpdated, object continuationToken = null)
	{
		await EnsureInitialized();
		uint maxResults = 100u;
		LobbySearchSetParameterOptions[] obj = new LobbySearchSetParameterOptions[4]
		{
			new LobbySearchSetParameterOptions
			{
				Parameter = new AttributeData
				{
					Key = "version",
					Value = Dew.GetCurrentMultiplayerCompatibilityVersion()
				},
				ComparisonOp = ComparisonOp.Equal
			},
			new LobbySearchSetParameterOptions
			{
				Parameter = new AttributeData
				{
					Key = "hasGameStarted",
					Value = false
				},
				ComparisonOp = ComparisonOp.Equal
			},
			new LobbySearchSetParameterOptions
			{
				Parameter = new AttributeData
				{
					Key = "isInviteOnly",
					Value = false
				},
				ComparisonOp = ComparisonOp.Equal
			},
			new LobbySearchSetParameterOptions
			{
				Parameter = new AttributeData
				{
					Key = "heartbeat",
					Value = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 30
				},
				ComparisonOp = ComparisonOp.Greaterthan
			}
		};
		LobbySearch search = new LobbySearch();
		CreateLobbySearchOptions createLobbySearchOptions = new CreateLobbySearchOptions
		{
			MaxResults = maxResults
		};
		EOSSDKComponent.GetLobbyInterface().CreateLobbySearch(ref createLobbySearchOptions, out search);
		LobbySearchSetParameterOptions[] array = obj;
		for (int i = 0; i < array.Length; i++)
		{
			LobbySearchSetParameterOptions option = array[i];
			search.SetParameter(ref option);
		}
		LobbySearchFindOptions findOptions = default(LobbySearchFindOptions);
		findOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		UniTaskCompletionSource<LobbySearchResult> task = new UniTaskCompletionSource<LobbySearchResult>();
		search.Find(ref findOptions, null, delegate(ref LobbySearchFindCallbackInfo callback)
		{
			try
			{
				if (callback.ResultCode != 0)
				{
					task.TrySetException(new EOSResultException(callback.ResultCode));
				}
				else
				{
					LobbySearchResult lobbySearchResult = new LobbySearchResult();
					LobbySearchGetSearchResultCountOptions options = default(LobbySearchGetSearchResultCountOptions);
					for (int j = 0; j < search.GetSearchResultCount(ref options); j++)
					{
						LobbySearchCopySearchResultByIndexOptions options2 = default(LobbySearchCopySearchResultByIndexOptions);
						options2.LobbyIndex = (uint)j;
						search.CopySearchResultByIndex(ref options2, out var outLobbyDetailsHandle);
						LobbyInstanceEpic lobbyInstanceEpic = new LobbyInstanceEpic().ApplyFromHandle(outLobbyDetailsHandle);
						if (lobbyInstanceEpic.currentPlayers < lobbyInstanceEpic.maxPlayers && lobbyInstanceEpic.currentPlayers > 0 && lobbyInstanceEpic.name.Length > 0)
						{
							LobbyDetailsGetLobbyOwnerOptions options3 = default(LobbyDetailsGetLobbyOwnerOptions);
							if (!(outLobbyDetailsHandle.GetLobbyOwner(ref options3) == EOSSDKComponent.LocalUserProductId))
							{
								lobbySearchResult.lobbies.Add(lobbyInstanceEpic);
							}
						}
					}
					task.TrySetResult(lobbySearchResult);
				}
			}
			catch (Exception exception)
			{
				task.TrySetException(exception);
			}
		});
		LobbySearchResult res = await task.Task.Timeout(TimeSpan.FromSeconds(15.0));
		onUpdated?.Invoke(res);
	}

	public override async UniTask SetLobbyAttribute(string key, object value)
	{
		if (currentLobby == null)
		{
			return;
		}
		UniTaskCompletionSource task = new UniTaskCompletionSource();
		LobbyModification modHandle = new LobbyModification();
		UpdateLobbyModificationOptions updateLobbyModificationOptions = default(UpdateLobbyModificationOptions);
		updateLobbyModificationOptions.LobbyId = currentLobby.id;
		updateLobbyModificationOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		UpdateLobbyModificationOptions updateLobbyModificationOptions2 = updateLobbyModificationOptions;
		EOSSDKComponent.GetLobbyInterface().UpdateLobbyModification(ref updateLobbyModificationOptions2, out modHandle);
		LobbyModificationAddAttributeOptions options = default(LobbyModificationAddAttributeOptions);
		options.Attribute = new AttributeData
		{
			Key = key,
			Value = value.ToAttrDataValue()
		};
		options.Visibility = LobbyAttributeVisibility.Public;
		modHandle.AddAttribute(ref options);
		UpdateLobbyOptions updateLobbyOptions = default(UpdateLobbyOptions);
		updateLobbyOptions.LobbyModificationHandle = modHandle;
		UpdateLobbyOptions updateLobbyOptions2 = updateLobbyOptions;
		EOSSDKComponent.GetLobbyInterface().UpdateLobby(ref updateLobbyOptions2, null, delegate(ref UpdateLobbyCallbackInfo callback)
		{
			if (callback.ResultCode != 0)
			{
				task.TrySetException(new EOSResultException(callback.ResultCode));
			}
			else
			{
				SetCurrentLobbyData(new LobbyInstanceEpic().ApplyFromHandle(_currentLobby.details));
				task.TrySetResult();
			}
		});
		await task.Task;
	}

	private async UniTask SetLobbyShortId()
	{
		if (_currentLobby != null)
		{
			string hash = GenerateHash(currentLobby.id);
			await SetLobbyAttribute("shortCode", hash);
		}
		static string GenerateHash(string input)
		{
			char[] availableCharacters = "23456789abcdefghjklmnpqrstuvwxyz".ToCharArray();
			using MD5 md5 = MD5.Create();
			byte[] inputBytes = Encoding.UTF8.GetBytes(input);
			byte[] hashBytes = md5.ComputeHash(inputBytes);
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < 8; i++)
			{
				int index = hashBytes[i] % availableCharacters.Length;
				char character = availableCharacters[index];
				sb.Append(character);
			}
			return sb.ToString();
		}
	}

	private async UniTask<LobbyDetails> GetLobbyById(string id)
	{
		await EnsureInitialized();
		ManagerBase<TransitionManager>.instance.UpdateLoadingStatus(LoadingStatus.GettingLobbyInformation);
		CreateLobbySearchOptions createLobbySearchOptions = default(CreateLobbySearchOptions);
		createLobbySearchOptions.MaxResults = 1u;
		CreateLobbySearchOptions opt = createLobbySearchOptions;
		EOSSDKComponent.GetLobbyInterface().CreateLobbySearch(ref opt, out var search);
		string shortId = id.Trim().Replace(" ", "").ToLower();
		if (shortId.Length == 8)
		{
			Debug.Log("Starting lobby search via short code: " + shortId);
			LobbySearchSetParameterOptions lobbySearchSetParameterOptions = default(LobbySearchSetParameterOptions);
			lobbySearchSetParameterOptions.Parameter = new AttributeData
			{
				Key = "shortCode",
				Value = shortId
			};
			lobbySearchSetParameterOptions.ComparisonOp = ComparisonOp.Equal;
			LobbySearchSetParameterOptions param = lobbySearchSetParameterOptions;
			search.SetParameter(ref param);
		}
		else
		{
			Debug.Log("Starting lobby search via id: " + id);
			LobbySearchSetLobbyIdOptions lobbySearchSetLobbyIdOptions = default(LobbySearchSetLobbyIdOptions);
			lobbySearchSetLobbyIdOptions.LobbyId = id;
			LobbySearchSetLobbyIdOptions param2 = lobbySearchSetLobbyIdOptions;
			search.SetLobbyId(ref param2);
		}
		UniTaskCompletionSource<LobbyDetails> task = new UniTaskCompletionSource<LobbyDetails>();
		LobbySearchFindOptions lobbySearchFindOptions = default(LobbySearchFindOptions);
		lobbySearchFindOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		LobbySearchFindOptions opt2 = lobbySearchFindOptions;
		search.Find(ref opt2, null, delegate(ref LobbySearchFindCallbackInfo callback)
		{
			if (callback.ResultCode != 0)
			{
				task.TrySetException(new EOSResultException(callback.ResultCode));
			}
			LobbySearchGetSearchResultCountOptions options = default(LobbySearchGetSearchResultCountOptions);
			if (search.GetSearchResultCount(ref options) == 0)
			{
				task.TrySetException(new EOSResultException(Result.NotFound));
			}
			else
			{
				LobbySearchCopySearchResultByIndexOptions lobbySearchCopySearchResultByIndexOptions = default(LobbySearchCopySearchResultByIndexOptions);
				lobbySearchCopySearchResultByIndexOptions.LobbyIndex = 0u;
				LobbySearchCopySearchResultByIndexOptions options2 = lobbySearchCopySearchResultByIndexOptions;
				search.CopySearchResultByIndex(ref options2, out var outLobbyDetailsHandle);
				task.TrySetResult(outLobbyDetailsHandle);
			}
		});
		return await task.Task;
	}

	private bool TryGetAttribute(List<global::Epic.OnlineServices.Lobby.Attribute> list, string key, out global::Epic.OnlineServices.Lobby.Attribute attr)
	{
		attr = list.Find((global::Epic.OnlineServices.Lobby.Attribute x) => x.Data.HasValue && x.Data.Value.Key == (Utf8String)key);
		return attr.Data.HasValue;
	}

	private void SetCurrentLobbyData(LobbyInstanceEpic data)
	{
		if (_currentLobby != data)
		{
			_currentLobby = data;
			InvokeOnCurrentLobbyChanged();
		}
	}
}
