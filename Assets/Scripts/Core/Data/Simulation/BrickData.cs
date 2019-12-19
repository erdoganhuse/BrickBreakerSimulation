using System;
using Core.Logic.Brick;
using UnityEngine;

namespace Core.Data.Simulation
{
    [Serializable]
    public struct BrickData
    {
        public Vector2 Position;
        public Vector2 Size;
        public int Life;
        
        public static explicit operator BrickData(Brick brick) => new BrickData
        {
            Life = brick.LifeAmount, 
            Position = brick.transform.position,
            Size = brick.transform.localScale
        };
    }
}