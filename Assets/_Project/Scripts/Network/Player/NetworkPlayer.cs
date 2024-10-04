using LindoNoxStudio.Network.Input;
using Unity.Netcode;
using UnityEngine;
using Client = LindoNoxStudio.Network.Connection.Client;
using NetworkClient = LindoNoxStudio.Network.Connection.NetworkClient;

namespace LindoNoxStudio.Network.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class NetworkPlayer : NetworkBehaviour
    {
        #if Client
        public static NetworkPlayer LocalNetworkPlayer { get; private set; }
        #elif Server
        private Client _networkClient;
        #endif
        
        private PlayerController _playerController;
        
        public override void OnNetworkSpawn()
        {
            #if Client
            if (IsOwner)
                LocalNetworkPlayer = this;
            #elif Server
            _networkClient = Client.GetClientByClientId(OwnerClientId);
            _networkClient.NetworkPlayer = this;
            #endif
            
            _playerController = GetComponent<PlayerController>();
        }
        
        public override void OnNetworkDespawn()
        {
            #if Client
            if (IsOwner)
                LocalNetworkPlayer = null;
            #endif
        }

        #if Client
        public void PredictLocalState(uint tick)
        {
            ClientInputState input = NetworkClient.LocalClient._input.GetClientInputState(tick);
            _playerController.OnInput(input);
            
            // Todo: Save state for later use
        }
        #elif Server
        public void HandleState(uint tick) 
        {
            ClientInputState input = _networkClient.NetworkClient._input.GetClientInputState(tick);
            _playerController.OnInput(input);
            
            // Todo: Save state for later use, but don't send the states. We do that in the state tick
        }
        #endif
    }
}