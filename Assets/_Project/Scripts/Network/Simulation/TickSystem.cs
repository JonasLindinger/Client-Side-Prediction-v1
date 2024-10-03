using System;

namespace LindoNoxStudio.Network.Simulation
{
    public class TickSystem
    {
        public Action<uint> OnTick = delegate {  };
        
        public uint CurrentTick { get; private set; }
        public int TickRate { get; private set; }
        public float TimeBetweenTicks { get; private set; }
        
        private float _time;
        
        public TickSystem(int tickRate, uint startingTick = 0)
        {
            TickRate = tickRate;
            TimeBetweenTicks = 1f / tickRate;
            
            CurrentTick = startingTick;
        }

        public void Update(float deltaTime)
        {
            _time += deltaTime;

            if (_time >= TimeBetweenTicks)
            {
                _time -= TimeBetweenTicks;
                
                CurrentTick++;
                OnTick?.Invoke(CurrentTick);
            }
        }
    }
}