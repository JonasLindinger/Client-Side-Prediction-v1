using UnityEngine;
using Unity.Netcode;

namespace LindoNoxStudio.Network.Input
{
    public class ClientInputState : INetworkSerializable
    {
        public uint Tick;
        public Vector2 Cycle;

        public void SetUp(uint tick, Vector2 cycle)
        {
            Tick = tick;
            Cycle = cycle;
        }
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Tick);
            serializer.SerializeValue(ref Cycle);
        }
    }
}