using System;
using LindoNoxStudio.Network.Simulation;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LindoNoxStudio.Network.Connection
{
    public class NetworkStarter : MonoBehaviour
    {
        public static ConnectionData ConnectionData = new ConnectionData()
        {
            IP = "127.0.0.1",
            Port = 7777
        };
        
        private UnityTransport _unityTransport; 
      
        
        private void Start()
        {
            SetConnectionData();
            #if Client
            StartClient();
            #elif Server
            StartServer();
            #endif
        }

        private void Update()
        {
            SimulationManager.Update(Time.deltaTime);
        }

        private void SetConnectionData()
        {
            _unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            _unityTransport.ConnectionData = new UnityTransport.ConnectionAddressData()
            {
                Address = ConnectionData.IP,
                Port = ConnectionData.Port
            };
        }
        
        #if Client
        private void StartClient()
        {
            ulong clientId = (ulong) Random.Range(11111, 99999);
            NetworkManager.Singleton.NetworkConfig.ConnectionData = ConnectionPayload.Encode(clientId, "Client " + clientId);
            NetworkManager.Singleton.StartClient();
        }
        #elif Server
        private void StartServer()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ConnectionManager.OnClientJoinRequest;
            NetworkManager.Singleton.OnConnectionEvent +=
                (NetworkManager networkManager, ConnectionEventData connectionEvent) =>
                {
                    switch (connectionEvent.EventType)
                    {
                        case ConnectionEvent.ClientConnected:
                            ConnectionManager.OnClientJoined(Client.GetClientByClientId(connectionEvent.ClientId));
                            break;
                        case ConnectionEvent.ClientDisconnected:
                            Client leftClient = Client.GetClientByClientId(connectionEvent.ClientId);
                            
                            if (leftClient == null) return;
                            ConnectionManager.OnClientLeft(leftClient);
                            break;
                    }
                };
            
            NetworkManager.Singleton.StartServer();
            SimulationManager.StartTickSystem();
        } 
        #endif
    }
}