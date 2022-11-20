using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Model;
using UnityEngine;

namespace Match3.Controller
{
    public class AdjacentSelection : MonoBehaviour, ICellSelector
    {
        public List<Vector2Int> SelectCells(Vector2Int position, GameBoard board)
        {
            if (!board.CellExists(position))
                throw new ArgumentException($"Playing filed does not have cell at {position} position.");

            var selection = new List<Vector2Int>();
            for (int i = position.x - 1; i <= position.x + 1; i++)
            {
                for (int j = position.y - 1; j <= position.y + 1; j++)
                {
                    var selectionPosition = new Vector2Int(i, j);
                    if (board.CellExists(selectionPosition) && selectionPosition != position)
                        selection.Add(selectionPosition);
                }
            }
            
            return selection;
        }
    }
}