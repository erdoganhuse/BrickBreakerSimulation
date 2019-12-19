using Core.Logic.Brick;
using Library.SignalBusSystem;

namespace Signals
{
    public class BrickDestructedSignal : ISignal
    {
        public readonly Brick Brick;

        public BrickDestructedSignal(Brick brick)
        {
            Brick = brick;
        }
    }
}