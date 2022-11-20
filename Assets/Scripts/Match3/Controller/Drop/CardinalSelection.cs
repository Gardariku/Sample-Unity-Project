using System;
using System.Collections.Generic;
using Match3.Model;
using UnityEngine;

namespace Match3.Controller
{
    public class CardinalSelection : MonoBehaviour, ICellSelector
    {
        public List<Vector2Int> SelectCells(Vector2Int position, GameBoard board)
        {
            if (!board.CellExists(position))
                throw new ArgumentException($"Playing filed does not have cell at {position} position.");
            var selection = new List<Vector2Int>();
            
            for (int i = 0; i <= board.Height; i++)
            {
                if (i != position.x)
                    selection.Add(new Vector2Int(i, position.y));
            }
            for (int j = 0; j <= board.Width; j++)
            {
                if (j != position.y)
                    selection.Add(new Vector2Int(position.x, j));
            }

            return selection;
        }
    }
}