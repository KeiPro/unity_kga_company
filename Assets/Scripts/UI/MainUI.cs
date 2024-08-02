using System;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nicknameInputField;
    [SerializeField] private GameObject _lobbyUI;
    [SerializeField] private GameObject _hostPopupUI;
    
    public void OnClickHostButton()
    {
        if (string.IsNullOrEmpty(_nicknameInputField.text))
        {
            Debug.Log("Input field cannot be empty.");
            return;
        }
        
        UGSServiceManager.Instance.SetNickname(_nicknameInputField.text);
        _hostPopupUI.SetActive(true);
    }
    
    public async void OnClickJoinButton()
    {
        try
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                gameObject.SetActive(false);
                _lobbyUI.SetActive(true);
                _lobbyUI.GetComponent<LobbyUI>().Init();
            };
            
            UGSServiceManager.Instance.SetNickname(_nicknameInputField.text);
            await UGSServiceManager.Instance.StartAuthentication();
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to sign in: " + e.Message);
        }
    }

    public async void OnClickSingleButton()
    {
        UGSServiceManager.Instance.SetNickname("KeiPro");
        
        await UGSServiceManager.Instance.StartAuthentication();
        UGSServiceManager.Instance.LobbyService.SetLobbyName("TestLobby");
        UGSServiceManager.Instance.SetIsHost(true);
        UGSServiceManager.Instance.LobbyService.SetIsPublicLobby(true);
            
        SceneManager.LoadScene("GameScene");
    }
}
