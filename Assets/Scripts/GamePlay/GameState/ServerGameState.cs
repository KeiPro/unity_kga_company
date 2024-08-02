using Infrastructure;
using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[RequireComponent(typeof(NetcodeHooks))]
public class ServerGameState : LifetimeScope
{
    [SerializeField]
    NetcodeHooks m_netcodeHooks;

    [Inject]
    public void InjectDependencies(ISubscriber<TributeSubmittedEventMessage> lifeState)
    {
        m_tributeSubmittedEventMessageSubscriber = lifeState;
    }
    
    private ISubscriber<TributeSubmittedEventMessage> m_tributeSubmittedEventMessageSubscriber;

    // 게임 시작 시 0으로 초기화하는 과정 필요함.
    private long _totalOfferingValue = 0; 
    
    // TODO : 미션 관련 코드에서 관리되어야 함.
    private const int TARGET_OFFERING_VALUE = 1000; 
    
    protected override void Awake()
    {
        base.Awake();
        
        if (Parent != null)
        {
            Parent.Container.Inject(this);
        }
        
        if (m_netcodeHooks == null)
        {
            m_netcodeHooks = GetComponent<NetcodeHooks>();
        }

        m_netcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
        m_netcodeHooks.OnNetworkDespawnHook += OnNetworkDespawn;
    }
    
    void OnNetworkSpawn()
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            enabled = false;
            return;
        }

        if (m_tributeSubmittedEventMessageSubscriber == null)
        {
            Debug.LogError("_tributeSubmittedEventMessageSubscriber is null during OnNetworkSpawn");
            return;
        }
        
        m_tributeSubmittedEventMessageSubscriber.Subscribe(OnTributeSubmitted);
    }

    void OnNetworkDespawn()
    {
        if (m_tributeSubmittedEventMessageSubscriber != null)
        {
            m_tributeSubmittedEventMessageSubscriber.Unsubscribe(OnTributeSubmitted);
        }
    }
    
    protected override void OnDestroy()
    {
        if (m_tributeSubmittedEventMessageSubscriber != null)
        {
            m_tributeSubmittedEventMessageSubscriber.Unsubscribe(OnTributeSubmitted);
        }

        if (m_netcodeHooks)
        {
            m_netcodeHooks.OnNetworkSpawnHook -= OnNetworkSpawn;
            m_netcodeHooks.OnNetworkDespawnHook -= OnNetworkDespawn;
        }

        base.OnDestroy();
    }

    void OnTributeSubmitted(TributeSubmittedEventMessage message)
    {
        _totalOfferingValue += message.price;
        
        ClientRpcManager.Instance.UpdateOfferingValueClientRpc(_totalOfferingValue, TARGET_OFFERING_VALUE);
        
        // 목표치를 달성했는지 검사
        if (_totalOfferingValue >= TARGET_OFFERING_VALUE)
        {
            ClientRpcManager.Instance.GameResultClientRpc(true);
        }
    }
}
