using System.Collections.Generic;
using UnityEngine;

namespace LindoNoxStudio.Network.Connection
{
    public class Client
    {
        private static List<Client> _clients = new List<Client>();
        
        public string UniqueName => DisplayName + "(" + Uuid + ")";
        
        public ulong Uuid;
        public ulong ClientId;
        
        public string DisplayName;
        public bool IsOnline;

        public static Client GetClientByUuid(ulong uuid)
        {
            return _clients.Find(c => c.Uuid == uuid);
        }
        
        public static Client GetClientByClientId(ulong clientId)
        {
            return _clients.Find(c => c.ClientId == clientId);
        }

        public void Add()
        {
            _clients.Add(this);
        }
        
        public void Remove()
        {
            _clients.Remove(this);
        }
        
        public void Joined()
        {
            IsOnline = true;
        }

        public void Left()
        {
            IsOnline = false;
        }
    }
}