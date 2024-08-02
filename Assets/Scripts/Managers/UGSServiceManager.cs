using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class UGSServiceManager : Singleton<UGSServiceManager>
{
    #region UGS_Service
    private UGSVoiceService _vivox = new UGSVoiceService();
    private UGSRelayService _relay = new UGSRelayService();
    private UGSLobbyService _lobby = new UGSLobbyService();
    
    public UGSVoiceService VivoxService => _vivox;
    public UGSRelayService RelayService => _relay;
    public UGSLobbyService LobbyService => _lobby;
    #endregion

    public bool IsHost { get; private set; }
    public string Nickname { get; private set; }
    

    protected override async void Awake()
    {
        base.Awake();
        
        var options = _vivox.GetVivoxCredentials();
        await UnityServices.InitializeAsync(options);
        Debug.Log("Completed initializing Unity Services.");
    }

    protected override bool ShouldDontDestroyOnLoad() 
	{
        return true;
    }
    
    public void SetIsHost(bool isHost) { IsHost = isHost; }
    public void SetNickname(string nickname) { Nickname = nickname; }
    
    public async Task StartAuthentication()
    {
        try
        {
            AuthenticationService.Instance.SwitchProfile(Nickname);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to sign in: " + e.Message);
            throw;
        }
    }
}
