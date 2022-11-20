using System;
using System.Collections.Generic;
using Match3.Model;
using UnityEngine;

namespace Match3.Controller
{
    public class NeumannSelection : MonoBehaviour, ICellSelector
    {
        public List<Vector2Int> SelectCells(Vector2Int position, GameBoard board)
        {
            if (!board.CellExists(position))
                throw new ArgumentException($"Playing filed does not have cell at {position} position.");
            var selection = new List<Vector2Int>();
            
            if (board.CellExists(new Vector2Int(position.x + 1, position.y)))
                selection.Add(new Vector2Int(position.x + 1, position.y));
            if (board.CellExists(new Vector2Int(position.x - 1, position.y)))
                selection.Add(new Vector2Int(position.x - 1, position.y));
            if (board.CellExists(new Vector2Int(position.x, position.y + 1)))
                selection.Add(new Vector2Int(position.x, position.y + 1));
            if (board.CellExists(new Vector2Int(position.x, position.y - 1)))
                selection.Add(new Vector2Int(position.x, position.y - 1));
            
            return selection;
        }
    }
}