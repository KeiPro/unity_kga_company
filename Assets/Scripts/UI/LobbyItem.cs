using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;

    private Lobby _lobby;
    
    public void Init(Lobby lobby)
    {
        _lobby = lobby;
        title.text = $"{lobby.Name} ({lobby.Players.Count}/{lobby.MaxPlayers})";

        if (lobby.Data.TryGetValue("JoinCode", out var joinCodeData))
        {
            UGSServiceManager.Instance.RelayService.SetJoinCode(joinCodeData.Value);
        }
        else
        {
            Debug.LogError("not exist JoinCode.");
        }
    }
    
    public async void OnClickJoinButton()
    {
        try
        {
            UGSServiceManager.Instance.LobbyService.SetLobby(_lobby);
            await UGSServiceManager.Instance.LobbyService.JoinLobbyById(_lobby.Id);
            SceneManager.LoadScene("GameScene");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to join lobby: {e.Message}");
        }
    }
}