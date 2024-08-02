using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UGSLobbyService
{
    public Lobby CurrentLobby { get; private set; } = null;
    public string LobbyName { get; private set; }
    private bool _isPublicLobby = false;

    // 비동기 작업에 전달하여 작업 취소를 가능하게 해주는 변수
    private CancellationTokenSource _heartbeatCts = null;

    public async Task CreateLobbyAsync(string lobbyName) {
        try
        {
            const float heartbeatInterval = 15f;
            
            // TODO : maxPlayer도 설정하도록 수정 필요.
            int maxPlayers = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                Player = GetPlayer(UGSServiceManager.Instance.Nickname),
                Data = new Dictionary<string, DataObject>()
                {
                    {"JoinCode", 
                        new DataObject(_isPublicLobby ? DataObject.VisibilityOptions.Public : DataObject.VisibilityOptions.Private, 
                        UGSServiceManager.Instance.RelayService.JoinCode)} 
                }
            };
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            SetLobby(lobby);
            
            _ = StartHeartbeat(heartbeatInterval);
        }
        catch (LobbyServiceException e) {
            Debug.Log($"Exception: {e.Message}\nStackTrace: {e.StackTrace}");
            throw;
        }
    }

    public async Task<List<Lobby>> GetLobbyList()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            return queryResponse.Results.Count > 0 ? queryResponse.Results : new List<Lobby>(); 
        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"LobbyServiceException: {e.Message}\nStackTrace: {e.StackTrace}");
            return new List<Lobby>();
        }
    }
    
    private Task StartHeartbeat(float interval)
    {
        StopHeartbeat();
        
        //비동기 작업을 취소할 수 있는 기능을 제공
        _heartbeatCts = new CancellationTokenSource();
        Debug.Log("Starting heartbeat...");
        // HeartbeatLobbyAsync를 백그라운드 작업으로 실행합니다.
        var heartbeatTask = HeartbeatLobbyAsync(CurrentLobby.Id, interval, _heartbeatCts.Token);

        // Task를 반환하여 비동기로 실행되도록 합니다.
        return heartbeatTask;
    }
    
    private async Task HeartbeatLobbyAsync(string lobbyId, float waitTimeSeconds, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                await Task.Delay(TimeSpan.FromSeconds(waitTimeSeconds), token);
            }
            catch (OperationCanceledException)
            {
                // 예외를 다시 던지지 않고 루프를 종료
                Debug.Log("Heartbeat was cancelled.");
                return;
            }
            catch (Exception e)
            {
                Debug.Log("Failed to send heartbeat: " + e.Message);
            }
        }
    } 

    //TODO 게임 시작 시 호출되어야 할 메소드.
    public void StopHeartbeat()
    {
        if (_heartbeatCts != null)
        {
            _heartbeatCts.Cancel();
            _heartbeatCts.Dispose();  // 취소 후 리소스 해제
            _heartbeatCts = null;
        }
    }
    
    public async Task JoinLobbyById(string lobbyId)
    {
        Debug.Log($"[lobbyId] : {lobbyId}");
        try
        {
            JoinLobbyByIdOptions joinLobbyByCodeOptions = new JoinLobbyByIdOptions
            {
                Player = GetPlayer(UGSServiceManager.Instance.Nickname)
            };

            await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId, joinLobbyByCodeOptions);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    
    private Player GetPlayer(string nickname) {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject> {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, nickname) }
            }
        };
    }
    
    public void SetLobby(Lobby lobby) { CurrentLobby = lobby; }
    public void SetLobbyName(string lobbyName) {LobbyName = lobbyName; }
    public void SetIsPublicLobby(bool isPublic) { _isPublicLobby = isPublic; } 
}