using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class UGSRelayService
{
    public string JoinCode { get; private set; }
    
    public async Task CreateRelayAndLobbyAsync(int maxConnection = 1)
    {
        try
        {
            // 첫 번째 인자는 maxConnection인데, 4인 플레이라면 3을 입력해준다. ( 호스트는 자동으로 포함되어 있기에 3을 입력해준다. )
            // Relay 서비스를 만들어주고,
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);

            // joincode생성.
            JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            
            // JoinCode가 생성되고 로비를 생성해야한다.
            await UGSServiceManager.Instance.LobbyService.CreateLobbyAsync(UGSServiceManager.Instance.LobbyService.LobbyName);
            
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    
    public async Task JoinRelayAsync(string joinCode) 
    {
        try
        {
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void SetJoinCode(string joinCode)
    {
        JoinCode = joinCode;
    }
}
