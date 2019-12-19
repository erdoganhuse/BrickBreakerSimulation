using Library.SignalBusSystem;

namespace Signals
{
    public class SimulationEndedSignal : ISignal
    {
        public readonly bool IsCompleted;
        public readonly float DestructionPercentage;
        public readonly float SimulationDuration;

        public SimulationEndedSignal(
            bool isCompleted,
            float destructionPercentage, 
            float simulationDuration)
        {
            IsCompleted = isCompleted;
            DestructionPercentage = destructionPercentage;
            SimulationDuration = simulationDuration;
        }
    }
}