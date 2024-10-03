using UnityEngine;

namespace LindoNoxStudio.Network.Simulation
{
    public static class SimulationManager
    {
        public const int PhysicsTickRate = 60;
        public const int StateTickRate = 60;
        #if Server
        public const int AdjustmentTickRate = 2;
        #endif
        
        public static uint CurrentTick => PhysicsTickSystem.CurrentTick;
        
        public static TickSystem PhysicsTickSystem { get; private set; }
        public static TickSystem StateTickSystem { get; private set; }
        #if Server
        public static TickSystem AdjustmentTickSystem { get; private set; }
        #endif

        public static void StartTickSystem(uint startingTick = 0)
        {
            // Freezing physics, so that we can run it manually
            Physics.simulationMode = SimulationMode.Script;
            
            // Startting tick systems
            PhysicsTickSystem = new TickSystem(PhysicsTickRate, startingTick);
            PhysicsTickSystem.OnTick += HandlePhysicsTick;
            
            StateTickSystem = new TickSystem(StateTickRate);
            StateTickSystem.OnTick += HandleStateTick;
            
            #if Server
            AdjustmentTickSystem = new TickSystem(AdjustmentTickRate);
            AdjustmentTickSystem.OnTick += HandleAdjustmentTick;
            #endif
        }

        public static void Update(float deltaTime)
        {
            if (PhysicsTickSystem != null)
                PhysicsTickSystem.Update(deltaTime);
            if (StateTickSystem != null)
                StateTickSystem.Update(deltaTime);
            #if Server
            if (AdjustmentTickSystem != null)
                AdjustmentTickSystem.Update(deltaTime);
            #endif
        }

        private static void HandlePhysicsTick(uint tick)
        {
            Debug.Log("Physics tick: ");
            // Simulating physics for the time between ticks
            Physics.Simulate(PhysicsTickSystem.TimeBetweenTicks);

            #if Client
            // Todo: Move local player and save inputs
            #elif Server
            // Todo: Move all players
            #endif
        }
        
        private static void HandleStateTick(uint tick)
        {
            Debug.Log("State tick: ");
            #if Client
            // Todo: Send inputs to server
            #elif Server
            // Todo: Send states to server
            #endif
        }
        
        #if Server
        private static void HandleAdjustmentTick(uint tick)
        {
            Debug.Log("Adjustment tick: ");
            // Todo: Send tick adjustments to clients
        }
        #endif
    }
}