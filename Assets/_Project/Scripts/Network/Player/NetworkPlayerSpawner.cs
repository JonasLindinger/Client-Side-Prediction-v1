using LindoNoxStudio.Network.Connection;
using UnityEngine;

namespace LindoNoxStudio.Network.Player
{
    public class NetworkPlayerSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private PlayerController _playerPrefab;
        
        #if Server
        public static NetworkPlayerSpawner Instance { get; private set; }
        
        private void Start()
        {
            if (Instance != null)
            {
                Debug.LogError("Duplicate found");
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
        
        private void OnDestroy()
        {
            if (!Instance) return;
            if (Instance != this) return;
            
            Instance = null;
        }
        
        public void Spawn(ulong clientId)
        {
            // Instantiate the player object on the server
            PlayerController player = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);

            // Spawn the object on every client
            player.NetworkObject.Spawn(false);
            player.NetworkObject.ChangeOwnership(clientId);
        }
        #endif
    }
}