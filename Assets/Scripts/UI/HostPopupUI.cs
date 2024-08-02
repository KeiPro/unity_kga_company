using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HostPopupUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _lobbyNameInputField;

    [SerializeField] private Toggle _publicToggle;
    [SerializeField] private Toggle _friendToggle;

    private readonly Color _unSelectedBackgroundColor = new Color(75f / 255f, 75f / 255f, 75f / 255f, 1.0f);
    private readonly Color _unSelectedTextColor =new Color(210f / 255f, 210f / 255f, 210f / 255f, 1.0f); 
    private readonly Color _selectedBackgroundColor = new Color(240f / 255f, 240f / 255f, 240f / 255f, 1.0f);
    private readonly Color _selectedTextColor = new Color(0, 0, 0, 1.0f);

    private void Awake()
    {
        _publicToggle.onValueChanged.AddListener(delegate { OnChangeToggleButton(_publicToggle); });
        _friendToggle.onValueChanged.AddListener(delegate { OnChangeToggleButton(_friendToggle); });
    }

    private void OnChangeToggleButton(Toggle toggle)
    {
        var backgroundImage = toggle.gameObject.GetComponent<Image>();
        backgroundImage.color = toggle.isOn ? _selectedBackgroundColor : _unSelectedBackgroundColor;

        var text = toggle.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = toggle.isOn ? _selectedTextColor : _unSelectedTextColor;
    }

    public async void OnClickOKButton()
    {
        try
        {
            if (string.IsNullOrEmpty(_lobbyNameInputField.text))
            {
                Debug.Log("Lobby name input field cannot be empty.");
                return;
            }
            
            await UGSServiceManager.Instance.StartAuthentication();
            UGSServiceManager.Instance.LobbyService.SetLobbyName(_lobbyNameInputField.text);
            UGSServiceManager.Instance.SetIsHost(true);
            UGSServiceManager.Instance.LobbyService.SetIsPublicLobby(_publicToggle.isOn);
            
            SceneManager.LoadScene("GameScene");
        }
        catch (Exception e)
        {
            Debug.LogError("Authentication failed: " + e.Message);
        }
    }
    
    public void OnClickBackButton()
    {
        gameObject.SetActive(false);
    }
}
