using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using EpicTransport;
using Mirror;
using UnityEngine;

public class EOSLobbyUI : EOSLobby
{
	private string lobbyName = "My Lobby";

	private bool showLobbyList;

	private bool showPlayerList;

	private List<LobbyDetails> foundLobbies = new List<LobbyDetails>();

	private List<Attribute> lobbyData = new List<Attribute>();

	private void OnEnable()
	{
		base.CreateLobbySucceeded += OnCreateLobbySuccess;
		base.JoinLobbySucceeded += OnJoinLobbySuccess;
		base.FindLobbiesSucceeded += OnFindLobbiesSuccess;
		base.LeaveLobbySucceeded += OnLeaveLobbySuccess;
	}

	private void OnDisable()
	{
		base.CreateLobbySucceeded -= OnCreateLobbySuccess;
		base.JoinLobbySucceeded -= OnJoinLobbySuccess;
		base.FindLobbiesSucceeded -= OnFindLobbiesSuccess;
		base.LeaveLobbySucceeded -= OnLeaveLobbySuccess;
	}

	private void OnCreateLobbySuccess(List<Attribute> attributes)
	{
		lobbyData = attributes;
		showPlayerList = true;
		showLobbyList = false;
		GetComponent<NetworkManager>().StartHost();
	}

	private void OnJoinLobbySuccess(List<Attribute> attributes)
	{
		lobbyData = attributes;
		showPlayerList = true;
		showLobbyList = false;
		NetworkManager netManager = GetComponent<NetworkManager>();
		Attribute hostAddressAttribute = attributes.Find((Attribute x) => x.Data.HasValue && x.Data.Value.Key == (Utf8String)"host_address");
		if (!hostAddressAttribute.Data.HasValue)
		{
			Debug.LogError("Host address not found in lobby attributes. Cannot connect to host.");
			return;
		}
		netManager.networkAddress = hostAddressAttribute.Data.Value.Value.AsUtf8;
		netManager.StartClient();
	}

	private void OnFindLobbiesSuccess(List<LobbyDetails> lobbiesFound)
	{
		foundLobbies = lobbiesFound;
		showPlayerList = false;
		showLobbyList = true;
	}

	private void OnLeaveLobbySuccess()
	{
		NetworkManager component = GetComponent<NetworkManager>();
		component.StopHost();
		component.StopClient();
	}

	private void OnGUI()
	{
		if (EOSSDKComponent.Initialized)
		{
			GUILayout.BeginHorizontal();
			DrawMenuButtons();
			GUILayout.BeginScrollView(Vector2.zero, GUILayout.MaxHeight(400f));
			if (showLobbyList && !showPlayerList)
			{
				DrawLobbyList();
			}
			else if (!showLobbyList && showPlayerList && base.ConnectedToLobby)
			{
				DrawLobbyMenu();
			}
			GUILayout.EndScrollView();
			GUILayout.EndHorizontal();
		}
	}

	private void DrawMenuButtons()
	{
		GUILayout.BeginVertical();
		GUI.enabled = !base.ConnectedToLobby;
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Create Lobby"))
		{
			CreateLobby(4u, LobbyPermissionLevel.Publicadvertised, presenceEnabled: false, new AttributeData[1]
			{
				new AttributeData
				{
					Key = AttributeKeys[0],
					Value = lobbyName
				}
			});
		}
		lobbyName = GUILayout.TextField(lobbyName, 40, GUILayout.Width(200f));
		GUILayout.EndHorizontal();
		if (GUILayout.Button("Find Lobbies"))
		{
			FindLobbies();
		}
		GUI.enabled = base.ConnectedToLobby;
		if (GUILayout.Button("Leave Lobby"))
		{
			LeaveLobby();
		}
		GUI.enabled = true;
		GUILayout.EndVertical();
	}

	private void DrawLobbyList()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Lobby Name", GUILayout.Width(220f));
		GUILayout.Label("Player Count");
		GUILayout.EndHorizontal();
		foreach (LobbyDetails lobby in foundLobbies)
		{
			Attribute? lobbyNameAttribute = default(Attribute);
			LobbyDetailsCopyAttributeByKeyOptions lobbyDetailsCopyAttributeByKeyOptions = default(LobbyDetailsCopyAttributeByKeyOptions);
			lobbyDetailsCopyAttributeByKeyOptions.AttrKey = AttributeKeys[0];
			LobbyDetailsCopyAttributeByKeyOptions copyOptions = lobbyDetailsCopyAttributeByKeyOptions;
			lobby.CopyAttributeByKey(ref copyOptions, out lobbyNameAttribute);
			GUILayout.BeginHorizontal(GUILayout.Width(400f), GUILayout.MaxWidth(400f));
			if (lobbyNameAttribute.HasValue && lobbyNameAttribute.Value.Data.HasValue)
			{
				AttributeData data = lobbyNameAttribute.Value.Data.Value;
				GUILayout.Label((data.Value.AsUtf8.Length > 30) ? (data.Value.ToString().Substring(0, 27).Trim() + "...") : ((string)data.Value.AsUtf8), GUILayout.Width(175f));
				GUILayout.Space(75f);
			}
			LobbyDetailsGetMemberCountOptions memberCountOptions = default(LobbyDetailsGetMemberCountOptions);
			GUILayout.Label(lobby.GetMemberCount(ref memberCountOptions).ToString());
			GUILayout.Space(75f);
			if (GUILayout.Button("Join", GUILayout.ExpandWidth(expand: false)))
			{
				JoinLobby(lobby, AttributeKeys);
			}
			GUILayout.EndHorizontal();
		}
	}

	private void DrawLobbyMenu()
	{
		Attribute lobbyNameAttribute = lobbyData.Find((Attribute x) => x.Data.HasValue && x.Data.Value.Key == (Utf8String)AttributeKeys[0]);
		if (lobbyNameAttribute.Data.HasValue)
		{
			GUILayout.Label("Name: " + lobbyNameAttribute.Data.Value.Value.AsUtf8);
			LobbyDetailsGetMemberCountOptions memberCountOptions = default(LobbyDetailsGetMemberCountOptions);
			uint playerCount = base.ConnectedLobbyDetails.GetMemberCount(ref memberCountOptions);
			for (int i = 0; i < playerCount; i++)
			{
				GUILayout.Label("Player " + i);
			}
		}
	}
}
