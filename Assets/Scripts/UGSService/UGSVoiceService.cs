using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Vivox;
using UnityEngine;

public class UGSVoiceService
{
    [SerializeField]
    private string _key;
    [SerializeField]
    private string _issuer;
    [SerializeField]
    private string _domain;
    [SerializeField]
    private string _server;

    private Action _endCallback;
    
    public InitializationOptions GetVivoxCredentials() 
    {
        var options = new InitializationOptions();
        if (CheckManualCredentials())
        {
            options.SetVivoxCredentials(_server, _domain, _issuer, _key);
        }

        return options;
    }

    public async Task LoginToVivoxService()
    {
        VivoxService.Instance.LoggedIn += OnUserLoggedIn;
        
        if (IsMicAvailable())
        {
            await LoginToVivox();
        }
        else
        {
            Debug.LogError("Microphone access is required for voice features. Please enable microphone access in your system settings.");
        }
    }

    private bool IsMicAvailable()
    {
        return Microphone.devices.Length > 0;
    }

    // VivoxService 초기화 및 로그인 처리.
    private async Task LoginToVivox()
    {
        await VivoxService.Instance.InitializeAsync();

        var loginOptions = new LoginOptions()
        {
            DisplayName = UGSServiceManager.Instance.Nickname,
            ParticipantUpdateFrequency = ParticipantPropertyUpdateFrequency.FivePerSecond
        };
        await VivoxService.Instance.LoginAsync(loginOptions);
    }

    private async void OnUserLoggedIn()
    {
        await VivoxService.Instance.JoinGroupChannelAsync(UGSServiceManager.Instance.LobbyService.CurrentLobby.Name, ChatCapability.AudioOnly);
        _endCallback?.Invoke();
    }

    public bool CheckManualCredentials()
    {
        return !(string.IsNullOrEmpty(_issuer) && string.IsNullOrEmpty(_domain) && string.IsNullOrEmpty(_server));
    }

    public void SetEndCallback(Action endCallback)
    {
        _endCallback = endCallback;
    }
}