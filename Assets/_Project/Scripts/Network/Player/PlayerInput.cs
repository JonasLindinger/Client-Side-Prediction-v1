using Unity.Netcode;
using UnityEngine;

namespace LindoNoxStudio.Network.Player
{
    [RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))]
    public class PlayerInput : NetworkBehaviour
    {
        #if Client
        private UnityEngine.InputSystem.PlayerInput _playerInput;
        
        public override void OnNetworkSpawn()
        {
            _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        }
        #endif
        
        [Rpc(SendTo.Server, Delivery = RpcDelivery.Unreliable)]
        private void OnClientInputRPC()
        {
            
        }
    }
}