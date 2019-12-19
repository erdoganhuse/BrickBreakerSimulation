using Core.Data.Simulation;
using Library.SignalBusSystem;

namespace Signals
{
    public class SimulationLoadedSignal : ISignal
    {
        public readonly SimulationData Data;

        public SimulationLoadedSignal(SimulationData data)
        {
            Data = data;
        }
    }
}