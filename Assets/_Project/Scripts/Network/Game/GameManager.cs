using System.Threading.Tasks;
using UnityEngine;

namespace LindoNoxStudio.Network.Game
{
    public static class GameManager
    {
        public static GameState GameState { get; private set; } = GameState.WaitingForPlayers;

        public static async Task StartGame()
        {
            GameState = GameState.Starting;

            await Task.Delay(3000);
            
            GameState = GameState.Started;
            Debug.Log("Game Started");
        }
    }
}