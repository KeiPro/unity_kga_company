
using Unity.Netcode;
using UnityEngine;

public class QuestManager : NetworkBehaviour
{
    // 총 금액을 네트워크에서 동기화
    public NetworkVariable<int> totalValue = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public int targetValue = 1000;
}
