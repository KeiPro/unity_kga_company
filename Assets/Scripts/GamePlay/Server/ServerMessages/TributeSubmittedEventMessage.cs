using Unity.Netcode;

public struct TributeSubmittedEventMessage : INetworkSerializeByMemcpy
{
    public long price;
}

