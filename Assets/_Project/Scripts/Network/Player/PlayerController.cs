using Unity.Netcode;
using UnityEngine;

namespace LindoNoxStudio.Network.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : NetworkBehaviour
    {
        #if Client
        public static PlayerController LocalPlayer { get; private set; }
        #endif
        
        private PlayerInput _playerInput;
        
        public override void OnNetworkSpawn()
        {
            _playerInput = GetComponent<PlayerInput>();
            
            #if Client
            if (IsOwner)
                LocalPlayer = this;
            #endif
        }
    }
}