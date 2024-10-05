using LindoNoxStudio.Network.Input;
using Unity.Netcode;
using UnityEngine;

namespace LindoNoxStudio.Network.Player
{
    public class PlayerState : INetworkSerializable
    {
        public uint Tick;
        
        public ClientInputState InputUsedForNextTick;

        public void SetUp(uint tick, ClientInputState inputUsedForNextTick)
        {
            Tick = tick;
            InputUsedForNextTick = inputUsedForNextTick;
        }
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Tick);
        }
    }
}