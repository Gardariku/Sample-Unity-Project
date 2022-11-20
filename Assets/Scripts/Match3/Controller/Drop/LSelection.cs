using System;
using System.Collections.Generic;
using Match3.Model;
using UnityEngine;

namespace Match3.Controller
{
    public class LSelection : MonoBehaviour, ICellSelector
    {
        public List<Vector2Int> SelectCells(Vector2Int position, GameBoard board)
        {
            if (!board.CellExists(position))
                throw new ArgumentException($"Playing filed does not have cell at {position} position.");
            var selection = new List<Vector2Int>();

            var newPos = new Vector2Int(position.x + 2, position.y + 1);
            if (board.CellExists(newPos))
                selection.Add(newPos);
            newPos = new Vector2Int(position.x + 2, position.y - 1);
            if (board.CellExists(newPos))
                selection.Add(newPos);
            newPos = new Vector2Int(position.x - 2, position.y + 1);
            if (board.CellExists(newPos))
                selection.Add(newPos);
            newPos = new Vector2Int(position.x - 2, position.y - 1);
            if (board.CellExists(newPos))
                selection.Add(newPos);
            newPos = new Vector2Int(position.x + 1, position.y + 2);
            if (board.CellExists(newPos))
                selection.Add(newPos);
            newPos = new Vector2Int(position.x + 1, position.y - 2);
            if (board.CellExists(newPos))
                selection.Add(newPos);
            newPos = new Vector2Int(position.x - 1, position.y + 2);
            if (board.CellExists(newPos))
                selection.Add(newPos);
            newPos = new Vector2Int(position.x - 1, position.y - 2);
            if (board.CellExists(newPos))
                selection.Add(newPos);

            return selection;
        }
    }
}