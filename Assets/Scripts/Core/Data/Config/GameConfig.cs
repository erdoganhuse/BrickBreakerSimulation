using System;

namespace Core.Data.Config
{
    [Serializable]
    public struct GameConfig
    {
        public int gameAreaWidth;
        public int gameAreaHeight;
        public int numBricksToSpawn;
        public int minBrickLife;
        public int maxBrickLife;
        public int ballSpeed;
        public int noHitsTimeout;
    }
}