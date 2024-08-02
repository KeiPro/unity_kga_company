using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ClientRpcManager : NetworkSingleton<ClientRpcManager>
{
    // TODO : MissionUI쪽으로 옮겨져야함.
    [SerializeField] private TextMeshProUGUI priceText;
    
    [ClientRpc]
    public void UpdateOfferingValueClientRpc(long totalValue, long targetValue)
    {
        // TODO : UIManager.GetUI<MissionUI>().Refreseh(totalValue);
        
        priceText.text = $"{totalValue} / {targetValue}";
    }
    
    [ClientRpc]
    public void GameResultClientRpc(bool isSuccess)
    {
        GameManager.Instance.ShowGameResult(isSuccess);
    }
}
