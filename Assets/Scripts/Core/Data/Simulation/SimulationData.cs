using System.Collections.Generic;
using UnityEngine;

namespace Core.Data.Simulation
{
    public struct SimulationData
    {
        public Vector2 BallPosition;
        public Vector2 BallSpeed;

        public Vector2 BoardCenter;
        public Vector2 BoardSize;

        public int TotalBrickCount;
        public List<BrickData> Bricks;
    }
}