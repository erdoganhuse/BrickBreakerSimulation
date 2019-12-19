using System.Collections.Generic;
using UnityEngine;
using Utilities.Enums;

namespace Core.Data.Board
{
    public struct GameArea
    {
        public readonly Vector2 Center;
        public readonly Vector2 Size;

        public GameArea(Vector2 center, Vector2 size)
        {
            Center = center;
            Size = size;
        }

        public float GetSingleSquareSize(int columnCount, float gapBetweenItems)
        {
            return (Size.x - (columnCount + 1) * gapBetweenItems) / columnCount;
        }
        
        public Vector2[] GetGridPointsFromUpperLeft(int amount, int columnCount, Vector2 singleGridSize, float gapBetweenItems)
        {            
            Vector2 upperLeftPosition = GetPosition(Anchor.UpperLeft);
            Vector2[] gridPoints = new Vector2[amount];
            for (int i = 0; i < amount; i++)
            {
                int columnIndex = i % columnCount;
                int rowIndex = (i / columnCount);
                
                float posX = upperLeftPosition.x + singleGridSize.x * ((2 * columnIndex + 1) / 2f) + gapBetweenItems * (columnIndex + 1);
                float posY = upperLeftPosition.y - singleGridSize.y * ((2 * rowIndex + 1) / 2f) - gapBetweenItems * (rowIndex + 1);
                
                gridPoints[i] = new Vector2(posX, posY);
            }

            return gridPoints;
        }
        
        public Vector2 GetPosition(Anchor anchor)
        {
            switch (anchor)
            {
                    case Anchor.LowerCenter:
                        return Center + new Vector2(0f, -Size.y / 2f);
                    case Anchor.LowerLeft:
                        return Center + new Vector2(-Size.x / 2f, -Size.y / 2f);
                    case Anchor.LowerRight:
                        return Center + new Vector2(Size.x / 2f, -Size.y / 2f);
                    case Anchor.MiddleCenter:
                        return Center;
                    case Anchor.MiddleLeft:
                        return Center + new Vector2(-Size.x / 2f, 0f);
                    case Anchor.MiddleRight:
                        return Center + new Vector2(Size.x / 2f, 0f);
                    case Anchor.UpperCenter:
                        return Center + new Vector2(0f, Size.y / 2f);
                    case Anchor.UpperLeft:
                        return Center + new Vector2(-Size.x / 2f, Size.y / 2f);
                    case Anchor.UpperRight:
                        return Center + new Vector2(Size.x / 2f, Size.y / 2f);
                    
                    default:
                        return Center;
            }
        }
    }
}