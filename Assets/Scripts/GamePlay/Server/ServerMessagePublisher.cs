using Infrastructure;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class ServerMessagePublisher : NetworkBehaviour
{
    [Inject]
    private IPublisher<TributeSubmittedEventMessage> m_publisher;

    public void Publish(TributeSubmittedEventMessage message)
    {
        m_publisher.Publish(message);
        Debug.Log("Tribute submitted with price " + message.GetType().Name);
    }
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            var gameState = FindObjectOfType<ServerGameState>();
            if (gameState != null)
            {
                gameState.Container.Inject(this);
            }
        }
    }
}