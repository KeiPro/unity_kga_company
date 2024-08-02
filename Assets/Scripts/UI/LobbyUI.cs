using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject _mainUI;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private GameObject _lobbyItem;

    private List<GameObject> _lobbyItemList = new List<GameObject>();
    private Coroutine _timerCoroutine = null;
    
    public void Init()
    {
        if (_timerCoroutine != null)
            return;
        
        ShowLobbyList();
    }
    
    public void OnClickRefreshButton()
    {
        if (_timerCoroutine != null)
            return;
        
        ShowLobbyList();
    }
    
    private async void ShowLobbyList() {
        try
        {
            const float timer = 5f;
            _timerCoroutine = StartCoroutine(TimerCoroutine(timer));
            
            var lobbyList = await UGSServiceManager.Instance.LobbyService.GetLobbyList();
            if (lobbyList.Count < 1)
                return;

            InitLobbyItem(lobbyList);
            
            for (int i = 0; i < lobbyList.Count; i++)
            {
                Debug.Log($"lobby : " + lobbyList[i]);
                var lobbyItem = _lobbyItemList[i];
                lobbyItem.SetActive(true);
                lobbyItem.GetComponent<LobbyItem>().Init(lobbyList[i]);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void InitLobbyItem(List<Lobby> lobbyList)
    {
        foreach (var lobbyItemObj in _lobbyItemList)
        {
            lobbyItemObj.SetActive(false);
        }
            
        while (_lobbyItemList.Count < lobbyList.Count)
        {
            _lobbyItemList.Add(Instantiate(_lobbyItem, _scrollRect.content));
        }
    }

    private IEnumerator TimerCoroutine(float timer)
    {
        yield return new WaitForSeconds(timer);
        _timerCoroutine = null;
    }

    //TODO : 게임 종료 시에 LobbyService.Instance.DeleteLobbyAsync() 이 코드가 실행 되도록 해야 됨.
}
