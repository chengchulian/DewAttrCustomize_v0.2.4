using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using EpicTransport;
using UnityEngine;

public class EOSLobby : MonoBehaviour
{
	public delegate void CreateLobbySuccess(List<Attribute> attributes);

	public delegate void CreateLobbyFailure(string errorMessage);

	public delegate void JoinLobbySuccess(List<Attribute> attributes);

	public delegate void JoinLobbyFailure(string errorMessage);

	public delegate void FindLobbiesSuccess(List<LobbyDetails> foundLobbies);

	public delegate void FindLobbiesFailure(string errorMessage);

	public delegate void LeaveLobbySuccess();

	public delegate void LeaveLobbyFailure(string errorMessage);

	public delegate void UpdateAttributeSuccess(string key);

	public delegate void UpdateAttributeFailure(string key, string errorMessage);

	public delegate void LobbyMemberStatusUpdate(LobbyMemberStatusReceivedCallbackInfo callback);

	public delegate void LobbyAttributeUpdate(LobbyUpdateReceivedCallbackInfo callback);

	[SerializeField]
	public string[] AttributeKeys = new string[1] { "lobby_name" };

	private const string DefaultAttributeKey = "default";

	public const string hostAddressKey = "host_address";

	private string currentLobbyId = string.Empty;

	private bool isLobbyOwner;

	private List<LobbyDetails> foundLobbies = new List<LobbyDetails>();

	private List<Attribute> lobbyData = new List<Attribute>();

	private ulong lobbyMemberStatusNotifyId;

	private ulong lobbyAttributeUpdateNotifyId;

	[HideInInspector]
	public bool ConnectedToLobby { get; private set; }

	public LobbyDetails ConnectedLobbyDetails { get; private set; }

	public event CreateLobbySuccess CreateLobbySucceeded;

	public event CreateLobbyFailure CreateLobbyFailed;

	public event JoinLobbySuccess JoinLobbySucceeded;

	public event JoinLobbyFailure JoinLobbyFailed;

	public event FindLobbiesSuccess FindLobbiesSucceeded;

	public event FindLobbiesFailure FindLobbiesFailed;

	public event LeaveLobbySuccess LeaveLobbySucceeded;

	public event LeaveLobbyFailure LeaveLobbyFailed;

	public event UpdateAttributeSuccess AttributeUpdateSucceeded;

	public event UpdateAttributeFailure AttributeUpdateFailed;

	public event LobbyMemberStatusUpdate LobbyMemberStatusUpdated;

	public event LobbyAttributeUpdate LobbyAttributeUpdated;

	public virtual void Start()
	{
		AddNotifyLobbyMemberStatusReceivedOptions addNotifyLobbyMemberStatusReceivedOptions = default(AddNotifyLobbyMemberStatusReceivedOptions);
		lobbyMemberStatusNotifyId = EOSSDKComponent.GetLobbyInterface().AddNotifyLobbyMemberStatusReceived(ref addNotifyLobbyMemberStatusReceivedOptions, null, delegate(ref LobbyMemberStatusReceivedCallbackInfo callback)
		{
			this.LobbyMemberStatusUpdated?.Invoke(callback);
			if (callback.CurrentStatus == LobbyMemberStatus.Closed)
			{
				LeaveLobby();
			}
		});
		AddNotifyLobbyUpdateReceivedOptions addNotifyLobbyUpdateReceivedOptions = default(AddNotifyLobbyUpdateReceivedOptions);
		lobbyAttributeUpdateNotifyId = EOSSDKComponent.GetLobbyInterface().AddNotifyLobbyUpdateReceived(ref addNotifyLobbyUpdateReceivedOptions, null, delegate(ref LobbyUpdateReceivedCallbackInfo callback)
		{
			this.LobbyAttributeUpdated?.Invoke(callback);
		});
	}

	public virtual void CreateLobby(uint maxConnections, LobbyPermissionLevel permissionLevel, bool presenceEnabled, AttributeData[] lobbyData = null)
	{
		CreateLobbyOptions createLobbyOptions = default(CreateLobbyOptions);
		createLobbyOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		createLobbyOptions.MaxLobbyMembers = maxConnections;
		createLobbyOptions.PermissionLevel = permissionLevel;
		createLobbyOptions.PresenceEnabled = presenceEnabled;
		createLobbyOptions.BucketId = "default";
		CreateLobbyOptions createLobbyOptions2 = createLobbyOptions;
		EOSSDKComponent.GetLobbyInterface().CreateLobby(ref createLobbyOptions2, null, delegate(ref CreateLobbyCallbackInfo callback)
		{
			List<Attribute> lobbyReturnData = new List<Attribute>();
			if (callback.ResultCode != 0)
			{
				this.CreateLobbyFailed?.Invoke("There was an error while creating a lobby. Error: " + callback.ResultCode);
			}
			else
			{
				LobbyModification outLobbyModificationHandle = new LobbyModification();
				AttributeData attributeData = default(AttributeData);
				attributeData.Key = "default";
				attributeData.Value = "default";
				AttributeData value = attributeData;
				attributeData = default(AttributeData);
				attributeData.Key = "host_address";
				attributeData.Value = EOSSDKComponent.LocalUserProductIdString;
				AttributeData value2 = attributeData;
				UpdateLobbyModificationOptions updateLobbyModificationOptions = default(UpdateLobbyModificationOptions);
				updateLobbyModificationOptions.LobbyId = callback.LobbyId;
				updateLobbyModificationOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
				UpdateLobbyModificationOptions options = updateLobbyModificationOptions;
				EOSSDKComponent.GetLobbyInterface().UpdateLobbyModification(ref options, out outLobbyModificationHandle);
				LobbyModificationAddAttributeOptions lobbyModificationAddAttributeOptions = default(LobbyModificationAddAttributeOptions);
				lobbyModificationAddAttributeOptions.Attribute = value;
				lobbyModificationAddAttributeOptions.Visibility = LobbyAttributeVisibility.Public;
				LobbyModificationAddAttributeOptions options2 = lobbyModificationAddAttributeOptions;
				lobbyModificationAddAttributeOptions = default(LobbyModificationAddAttributeOptions);
				lobbyModificationAddAttributeOptions.Attribute = value2;
				lobbyModificationAddAttributeOptions.Visibility = LobbyAttributeVisibility.Public;
				LobbyModificationAddAttributeOptions options3 = lobbyModificationAddAttributeOptions;
				outLobbyModificationHandle.AddAttribute(ref options2);
				outLobbyModificationHandle.AddAttribute(ref options3);
				if (lobbyData != null)
				{
					AttributeData[] array = lobbyData;
					foreach (AttributeData value3 in array)
					{
						LobbyModificationAddAttributeOptions options4 = default(LobbyModificationAddAttributeOptions);
						options4.Attribute = value3;
						options4.Visibility = LobbyAttributeVisibility.Public;
						outLobbyModificationHandle.AddAttribute(ref options4);
						lobbyReturnData.Add(new Attribute
						{
							Data = value3,
							Visibility = LobbyAttributeVisibility.Public
						});
					}
				}
				Utf8String lobbyId = callback.LobbyId;
				UpdateLobbyOptions updateLobbyOptions = default(UpdateLobbyOptions);
				updateLobbyOptions.LobbyModificationHandle = outLobbyModificationHandle;
				UpdateLobbyOptions options5 = updateLobbyOptions;
				EOSSDKComponent.GetLobbyInterface().UpdateLobby(ref options5, null, delegate(ref UpdateLobbyCallbackInfo updateCallback)
				{
					if (updateCallback.ResultCode != 0)
					{
						this.CreateLobbyFailed?.Invoke("There was an error while updating the lobby. Error: " + updateCallback.ResultCode);
					}
					else
					{
						CopyLobbyDetailsHandleOptions copyLobbyDetailsHandleOptions = default(CopyLobbyDetailsHandleOptions);
						copyLobbyDetailsHandleOptions.LobbyId = lobbyId;
						copyLobbyDetailsHandleOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
						CopyLobbyDetailsHandleOptions options6 = copyLobbyDetailsHandleOptions;
						EOSSDKComponent.GetLobbyInterface().CopyLobbyDetailsHandle(ref options6, out var outLobbyDetailsHandle);
						ConnectedLobbyDetails = outLobbyDetailsHandle;
						isLobbyOwner = true;
						ConnectedToLobby = true;
						currentLobbyId = lobbyId;
						this.CreateLobbySucceeded?.Invoke(lobbyReturnData);
					}
				});
			}
		});
	}

	public virtual void FindLobbies(uint maxResults = 100u, LobbySearchSetParameterOptions[] lobbySearchSetParameterOptions = null)
	{
		LobbySearch search = new LobbySearch();
		CreateLobbySearchOptions createLobbySearchOptions = default(CreateLobbySearchOptions);
		createLobbySearchOptions.MaxResults = maxResults;
		CreateLobbySearchOptions createLobbySearchOptions2 = createLobbySearchOptions;
		EOSSDKComponent.GetLobbyInterface().CreateLobbySearch(ref createLobbySearchOptions2, out search);
		if (lobbySearchSetParameterOptions != null)
		{
			for (int i = 0; i < lobbySearchSetParameterOptions.Length; i++)
			{
				LobbySearchSetParameterOptions option = lobbySearchSetParameterOptions[i];
				search.SetParameter(ref option);
			}
		}
		else
		{
			LobbySearchSetParameterOptions options = default(LobbySearchSetParameterOptions);
			options.ComparisonOp = ComparisonOp.Equal;
			options.Parameter = new AttributeData
			{
				Key = "default",
				Value = "default"
			};
			search.SetParameter(ref options);
		}
		LobbySearchFindOptions findOptions = default(LobbySearchFindOptions);
		findOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		search.Find(ref findOptions, null, delegate(ref LobbySearchFindCallbackInfo callback)
		{
			if (callback.ResultCode != 0)
			{
				this.FindLobbiesFailed?.Invoke("There was an error while finding lobbies. Error: " + callback.ResultCode);
			}
			else
			{
				foundLobbies.Clear();
				LobbySearchGetSearchResultCountOptions options2 = default(LobbySearchGetSearchResultCountOptions);
				for (int j = 0; j < search.GetSearchResultCount(ref options2); j++)
				{
					LobbySearchCopySearchResultByIndexOptions options3 = default(LobbySearchCopySearchResultByIndexOptions);
					options3.LobbyIndex = (uint)j;
					search.CopySearchResultByIndex(ref options3, out var outLobbyDetailsHandle);
					foundLobbies.Add(outLobbyDetailsHandle);
				}
				this.FindLobbiesSucceeded?.Invoke(foundLobbies);
			}
		});
	}

	public virtual void JoinLobby(LobbyDetails lobbyToJoin, string[] attributeKeys = null, bool presenceEnabled = false)
	{
		JoinLobbyOptions joinLobbyOptions = default(JoinLobbyOptions);
		joinLobbyOptions.LobbyDetailsHandle = lobbyToJoin;
		joinLobbyOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		joinLobbyOptions.PresenceEnabled = presenceEnabled;
		JoinLobbyOptions joinLobbyOptions2 = joinLobbyOptions;
		EOSSDKComponent.GetLobbyInterface().JoinLobby(ref joinLobbyOptions2, null, delegate(ref JoinLobbyCallbackInfo callback)
		{
			if (callback.ResultCode != 0)
			{
				this.JoinLobbyFailed?.Invoke("There was an error while joining a lobby. Error: " + callback.ResultCode);
			}
			else
			{
				lobbyData.Clear();
				Attribute? outAttribute = default(Attribute);
				LobbyDetailsCopyAttributeByKeyOptions options = default(LobbyDetailsCopyAttributeByKeyOptions);
				options.AttrKey = "host_address";
				lobbyToJoin.CopyAttributeByKey(ref options, out outAttribute);
				lobbyData.Add(outAttribute.Value);
				if (attributeKeys != null)
				{
					string[] array = attributeKeys;
					foreach (string text in array)
					{
						Attribute? outAttribute2 = default(Attribute);
						LobbyDetailsCopyAttributeByKeyOptions options2 = default(LobbyDetailsCopyAttributeByKeyOptions);
						options2.AttrKey = text;
						lobbyToJoin.CopyAttributeByKey(ref options2, out outAttribute2);
						lobbyData.Add(outAttribute2.Value);
					}
				}
				CopyLobbyDetailsHandleOptions copyLobbyDetailsHandleOptions = default(CopyLobbyDetailsHandleOptions);
				copyLobbyDetailsHandleOptions.LobbyId = callback.LobbyId;
				copyLobbyDetailsHandleOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
				CopyLobbyDetailsHandleOptions options3 = copyLobbyDetailsHandleOptions;
				EOSSDKComponent.GetLobbyInterface().CopyLobbyDetailsHandle(ref options3, out var outLobbyDetailsHandle);
				ConnectedLobbyDetails = outLobbyDetailsHandle;
				isLobbyOwner = false;
				ConnectedToLobby = true;
				currentLobbyId = callback.LobbyId;
				this.JoinLobbySucceeded?.Invoke(lobbyData);
			}
		});
	}

	public virtual void JoinLobbyByID(string lobbyID)
	{
		LobbySearch search = new LobbySearch();
		CreateLobbySearchOptions createLobbySearchOptions = default(CreateLobbySearchOptions);
		createLobbySearchOptions.MaxResults = 1u;
		CreateLobbySearchOptions createLobbySearchOptions2 = createLobbySearchOptions;
		EOSSDKComponent.GetLobbyInterface().CreateLobbySearch(ref createLobbySearchOptions2, out search);
		LobbySearchSetLobbyIdOptions lobbySearchSetLobbyIdOptions = default(LobbySearchSetLobbyIdOptions);
		lobbySearchSetLobbyIdOptions.LobbyId = lobbyID;
		LobbySearchSetLobbyIdOptions lobbySearchSetLobbyOptions = lobbySearchSetLobbyIdOptions;
		search.SetLobbyId(ref lobbySearchSetLobbyOptions);
		LobbySearchFindOptions lobbySearchFindOptions = default(LobbySearchFindOptions);
		lobbySearchFindOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		LobbySearchFindOptions lobbySearchFindOptions2 = lobbySearchFindOptions;
		search.Find(ref lobbySearchFindOptions2, null, delegate(ref LobbySearchFindCallbackInfo callback)
		{
			if (callback.ResultCode != 0)
			{
				this.FindLobbiesFailed?.Invoke("There was an error while finding lobbies. Error: " + callback.ResultCode);
			}
			else
			{
				foundLobbies.Clear();
				LobbySearchGetSearchResultCountOptions options = default(LobbySearchGetSearchResultCountOptions);
				for (int i = 0; i < search.GetSearchResultCount(ref options); i++)
				{
					LobbySearchCopySearchResultByIndexOptions options2 = default(LobbySearchCopySearchResultByIndexOptions);
					options2.LobbyIndex = (uint)i;
					search.CopySearchResultByIndex(ref options2, out var outLobbyDetailsHandle);
					foundLobbies.Add(outLobbyDetailsHandle);
				}
				if (foundLobbies.Count > 0)
				{
					JoinLobby(foundLobbies[0]);
				}
			}
		});
	}

	public virtual void LeaveLobby()
	{
		if (isLobbyOwner)
		{
			DestroyLobbyOptions destroyLobbyOptions = default(DestroyLobbyOptions);
			destroyLobbyOptions.LobbyId = currentLobbyId;
			destroyLobbyOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
			DestroyLobbyOptions destroyLobbyOptions2 = destroyLobbyOptions;
			EOSSDKComponent.GetLobbyInterface().DestroyLobby(ref destroyLobbyOptions2, null, delegate(ref DestroyLobbyCallbackInfo callback)
			{
				if (callback.ResultCode != 0)
				{
					this.LeaveLobbyFailed?.Invoke("There was an error while destroying the lobby. Error: " + callback.ResultCode);
				}
				else
				{
					ConnectedToLobby = false;
					this.LeaveLobbySucceeded?.Invoke();
				}
			});
		}
		else
		{
			LeaveLobbyOptions leaveLobbyOptions = default(LeaveLobbyOptions);
			leaveLobbyOptions.LobbyId = currentLobbyId;
			leaveLobbyOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
			LeaveLobbyOptions leaveLobbyOptions2 = leaveLobbyOptions;
			EOSSDKComponent.GetLobbyInterface().LeaveLobby(ref leaveLobbyOptions2, null, delegate(ref LeaveLobbyCallbackInfo callback)
			{
				if (callback.ResultCode != 0 && callback.ResultCode != Result.NotFound)
				{
					this.LeaveLobbyFailed?.Invoke("There was an error while leaving the lobby. Error: " + callback.ResultCode);
				}
				else
				{
					ConnectedToLobby = false;
					this.LeaveLobbySucceeded?.Invoke();
				}
			});
		}
		EOSSDKComponent.GetLobbyInterface().RemoveNotifyLobbyMemberStatusReceived(lobbyMemberStatusNotifyId);
		EOSSDKComponent.GetLobbyInterface().RemoveNotifyLobbyUpdateReceived(lobbyAttributeUpdateNotifyId);
	}

	public virtual void RemoveAttribute(string key)
	{
		LobbyModification modHandle = new LobbyModification();
		UpdateLobbyModificationOptions updateLobbyModificationOptions = default(UpdateLobbyModificationOptions);
		updateLobbyModificationOptions.LobbyId = currentLobbyId;
		updateLobbyModificationOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		UpdateLobbyModificationOptions updateLobbyModificationOptions2 = updateLobbyModificationOptions;
		EOSSDKComponent.GetLobbyInterface().UpdateLobbyModification(ref updateLobbyModificationOptions2, out modHandle);
		LobbyModificationRemoveAttributeOptions options = default(LobbyModificationRemoveAttributeOptions);
		options.Key = key;
		modHandle.RemoveAttribute(ref options);
		UpdateLobbyOptions updateLobbyOptions = default(UpdateLobbyOptions);
		updateLobbyOptions.LobbyModificationHandle = modHandle;
		UpdateLobbyOptions updateLobbyOptions2 = updateLobbyOptions;
		EOSSDKComponent.GetLobbyInterface().UpdateLobby(ref updateLobbyOptions2, null, delegate(ref UpdateLobbyCallbackInfo callback)
		{
			if (callback.ResultCode != 0)
			{
				this.AttributeUpdateFailed?.Invoke(key, "There was an error while removing attribute \"" + key + "\". Error: " + callback.ResultCode);
			}
			else
			{
				this.AttributeUpdateSucceeded?.Invoke(key);
			}
		});
	}

	private void UpdateAttribute(AttributeData attribute)
	{
		LobbyModification modHandle = new LobbyModification();
		UpdateLobbyModificationOptions updateLobbyModificationOptions = default(UpdateLobbyModificationOptions);
		updateLobbyModificationOptions.LobbyId = currentLobbyId;
		updateLobbyModificationOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		UpdateLobbyModificationOptions updateLobbyModificationOptions2 = updateLobbyModificationOptions;
		EOSSDKComponent.GetLobbyInterface().UpdateLobbyModification(ref updateLobbyModificationOptions2, out modHandle);
		LobbyModificationAddAttributeOptions options = default(LobbyModificationAddAttributeOptions);
		options.Attribute = attribute;
		options.Visibility = LobbyAttributeVisibility.Public;
		modHandle.AddAttribute(ref options);
		UpdateLobbyOptions updateLobbyOptions = default(UpdateLobbyOptions);
		updateLobbyOptions.LobbyModificationHandle = modHandle;
		UpdateLobbyOptions updateLobbyOptions2 = updateLobbyOptions;
		EOSSDKComponent.GetLobbyInterface().UpdateLobby(ref updateLobbyOptions2, null, delegate(ref UpdateLobbyCallbackInfo callback)
		{
			if (callback.ResultCode != 0)
			{
				this.AttributeUpdateFailed?.Invoke(attribute.Key, $"There was an error while updating attribute \"{attribute.Key}\". Error: " + callback.ResultCode);
			}
			else
			{
				this.AttributeUpdateSucceeded?.Invoke(attribute.Key);
			}
		});
	}

	public void UpdateLobbyAttribute(string key, bool newValue)
	{
		AttributeData attributeData = default(AttributeData);
		attributeData.Key = key;
		attributeData.Value = newValue;
		AttributeData data = attributeData;
		UpdateAttribute(data);
	}

	public void UpdateLobbyAttribute(string key, int newValue)
	{
		AttributeData attributeData = default(AttributeData);
		attributeData.Key = key;
		attributeData.Value = newValue;
		AttributeData data = attributeData;
		UpdateAttribute(data);
	}

	public void UpdateLobbyAttribute(string key, double newValue)
	{
		AttributeData attributeData = default(AttributeData);
		attributeData.Key = key;
		attributeData.Value = newValue;
		AttributeData data = attributeData;
		UpdateAttribute(data);
	}

	public void UpdateLobbyAttribute(string key, string newValue)
	{
		AttributeData attributeData = default(AttributeData);
		attributeData.Key = key;
		attributeData.Value = newValue;
		AttributeData data = attributeData;
		UpdateAttribute(data);
	}

	public string GetCurrentLobbyId()
	{
		return currentLobbyId;
	}
}
