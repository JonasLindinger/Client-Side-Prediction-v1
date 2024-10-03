using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace LindoNoxStudio.Network.Connection
{
    public class NetworkClient : NetworkBehaviour
    {
        #if Client
        public static NetworkClient LocalClient { get; private set; }
        #elif Server
            
        public static List<NetworkClient> Clients = new List<NetworkClient>();
            
        #endif       

        public override void OnNetworkSpawn()
        {
            #if Client
            
            LocalClient = this;
            Debug.Log("Local client spawned");
            
            #elif Server
            
            Clients.Add(this);
            Debug.Log("Client spawned");
            
            #endif
        }

        public override void OnNetworkDespawn()
        {
            #if Client
            
            LocalClient = null;
            Debug.Log("Local despawned");
            
            #elif Server
            
            Clients.Remove(this);
            Debug.Log("Client despawned");
            
            #endif
        } 
    }
}