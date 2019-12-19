using Library.SignalBusSystem;

namespace Signals
{
    public class SimulationStartedSignal : ISignal
    {
        public readonly int DestructedBrickCount;
        public readonly int TotalBrickCount;

        public SimulationStartedSignal(int destructedBrickCount, int totalBrickCount)
        {
            DestructedBrickCount = destructedBrickCount;
            TotalBrickCount = totalBrickCount;
        }
    }
}