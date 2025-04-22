using System;
using System.Collections;

using UnityEngine;

public class UI_Lobby_HideIfSingleplayer : MonoBehaviour
{
    private void Start()
    {
        
        if (DewNetworkManager.networkMode == DewNetworkManager.Mode.Singleplayer)
        {
            base.gameObject.SetActive(value: false);
        }
        else
        {
            StartCoroutine(waitLobbyLoadEndCoroutine());
        }
    }

    private IEnumerator waitLobbyLoadEndCoroutine()
    {


        int count = -1;

        while (count < 0)
        {
            yield return new WaitForSeconds(0.1f);
            try
            {
                count = ManagerBase<LobbyManager>.instance.service.currentLobby.maxPlayers;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        Transform playList = base.gameObject.transform.GetChild(7);



        var playListGameObject = playList.gameObject;
        UI_Lobby_PlayerListItem[] uiLobbyPlayerListItems =
            playListGameObject.GetComponentsInChildren<UI_Lobby_PlayerListItem>();
        int addCount = count - uiLobbyPlayerListItems.Length;

        for (int i = 0; i < addCount; i++)
        {
            GameObject addGameObject =
                Instantiate(uiLobbyPlayerListItems[0].gameObject, playListGameObject.transform);

            addGameObject.name = $"UI_Lobby_PlayerListItem ({uiLobbyPlayerListItems.Length + i})";
            addGameObject.transform.SetParent(playListGameObject.transform, false);
            addGameObject.GetComponent<UI_Lobby_PlayerListItem>().index = playListGameObject.transform.childCount - 1;
        }
        yield return null;
    }
}