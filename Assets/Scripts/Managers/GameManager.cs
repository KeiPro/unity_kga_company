using Unity.Services.Vivox;
using UnityEngine;
using VContainer.Unity;

public class GameManager : Singleton<GameManager>
{
    // GameScene 시작 시 Relay서비스 및 Lobby서비스, Vivox음성 서비스 초기화.
    protected override async void Awake()
    {
        if (UGSServiceManager.Instance.IsHost)
        {
            await UGSServiceManager.Instance.RelayService.CreateRelayAndLobbyAsync(maxConnection:3);
        }
        else
        {
            await UGSServiceManager.Instance.RelayService.JoinRelayAsync(UGSServiceManager.Instance.RelayService.JoinCode);
        }
        
        await UGSServiceManager.Instance.VivoxService.LoginToVivoxService();
        await VivoxService.Instance.InitializeAsync();
    }
    
    public void ShowGameResult(bool isSuccess)
    {
        if (isSuccess)
        {
            Debug.Log("게임 승리!");
        }
        else
        {
            Debug.Log("게임 실패!");
        }
    }
}
