using LindoNoxStudio.Network.Game;
using Unity.Netcode;
using UnityEngine;

namespace LindoNoxStudio.Network.Connection
{
    public static class ConnectionManager
    {
        #if Server
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
                    // Todo: Check player limit and start game conditions
                    
                    // Game hasn't started yet
                    response.Approved = true;
                    
                    // Adding new client
                    newClient.Add();
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
            Debug.Log(newClient.UniqueName + " Joined");
        }

        public static void OnClientLeft(Client leftClient)
        {
            leftClient.Left();
            Debug.Log(leftClient.UniqueName + " Left");
        }
        
        #endif 
    }
}