using LindoNoxStudio.Network.Game;
using Unity.Netcode;
using UnityEngine;

namespace LindoNoxStudio.Network.Connection
{
    public static class ConnectionManager
    {
        #if Server
        public const int WantedPlayerCount = 2;
        public static int CurrentPlayerCount;
        
        public static void OnClientJoinRequest(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            var payload = ConnectionPayload.Decode(request.Payload);

            Client newClient = new Client()
            {
                Uuid = payload.uuid,
                ClientId = request.ClientNetworkId,
                DisplayName = payload.displayName,
            };

            if (Client.GetClientByUuid(newClient.Uuid) == null)
            {
                // New player
                if (GameManager.GameState == GameState.WaitingForPlayers)
                {
                    // Game hasn't started yet
                    response.Approved = true;
                    
                    // Adding new client
                    newClient.Add();
                    NetworkClientSpawner.Instance.Spawn(newClient.ClientId);
                }
                else
                {
                    // Game already started
                    response.Reason = "Game already started";
                    response.Approved = false;
                }
            }
            else
            {
                // Player already exists
                Client existingClient = Client.GetClientByUuid(newClient.Uuid);
                if (!existingClient.IsOnline)
                {
                    // Reconnect
                    response.Approved = true;
                    
                    Debug.Log(existingClient.UniqueName + "Client Reconnected");

                    existingClient.ClientId = newClient.ClientId;
                    Debug.Log("Worked: " + (existingClient.ClientId == newClient.ClientId).ToString());

                    // Todo: Handle ownership
                }
                else
                {
                    // Player tries to join, but he is already in the game and online
                    response.Reason = "You are already in the game and online!";
                    response.Approved = false;
                }
            }
        }

        public static void OnClientJoined(Client newClient)
        {
            newClient.Joined();
            CurrentPlayerCount++;
            
            // Todo: Check player limit and start game condition
            if (CurrentPlayerCount == WantedPlayerCount)
                GameManager.StartGame();
            
            Debug.Log(newClient.UniqueName + " Joined");
        }

        public static void OnClientLeft(Client leftClient)
        {
            if (GameManager.GameState == GameState.WaitingForPlayers)
            {
                leftClient.Remove();
                CurrentPlayerCount--;
                
                Debug.Log(leftClient.UniqueName + " Left");
            }
            else
            {
                leftClient.Left();

                Debug.Log(leftClient.UniqueName + " Disconnected");
            }
        }
        
        #endif 
    }
}