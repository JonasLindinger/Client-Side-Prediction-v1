using Unity.Netcode;

namespace LindoNoxStudio.Network.Simulation
{
    public class TickSyncronisation : NetworkBehaviour
    {
        #if Client
        public const int StartingTickOffset = 5;
        #endif
        
        #if Server
        public override void OnNetworkSpawn()
        {
            OnServerTickRPC(SimulationManager.CurrentTick);
        }
        #endif

        [Rpc(SendTo.Owner, Delivery = RpcDelivery.Reliable)]
        private void OnServerTickRPC(uint serverTick)
        {
            #if Client
            SimulationManager.StartTickSystem(serverTick + StartingTickOffset);
            #endif
        }
    }
}