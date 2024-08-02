using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ApplicationController : LifetimeScope
{
    [SerializeField] NetworkManager m_NetworkManager;
    
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        builder.RegisterComponent(m_NetworkManager);
        builder.RegisterComponent(new NetworkedMessageChannel<TributeSubmittedEventMessage>()).AsImplementedInterfaces();
    }
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 120;
    }
}
