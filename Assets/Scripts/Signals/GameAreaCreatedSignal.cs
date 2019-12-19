using Core.Data.Board;
using Library.SignalBusSystem;

namespace Signals
{
    public class GameAreaCreatedSignal : ISignal
    {
        public readonly GameArea GameArea;

        public GameAreaCreatedSignal(GameArea gameArea)
        {
            GameArea = gameArea;
        }
    }
}