using System;
using System.Collections.Generic;
using Match3.Model;
using UnityEngine;

namespace Match3.Controller
{
    public class DiagonalSelection : MonoBehaviour, ICellSelector
    {
        public List<Vector2Int> SelectCells(Vector2Int position, GameBoard board)
        {
            if (!board.CellExists(position))
                throw new ArgumentException($"Playing filed does not have cell at {position} position.");
            var selection = new List<Vector2Int>();

            foreach (var direction in Diagonals)
            {
                Vector2Int newPos = position;
                while (board.CellExists(direction(newPos)))
                {
                    newPos = direction(newPos);
                    selection.Add(newPos);
                }
            }

            return selection;
        }

        public static readonly Func<Vector2Int, Vector2Int>[] Diagonals = { UpRight, UpLeft, DownRight, DownLeft };

        public static Vector2Int UpRight(Vector2Int pos) => new Vector2Int(pos.x - 1, pos.y + 1);
        public static Vector2Int UpLeft(Vector2Int pos) => new Vector2Int(pos.x - 1, pos.y - 1);
        public static Vector2Int DownRight(Vector2Int pos) => new Vector2Int(pos.x + 1, pos.y + 1);
        public static Vector2Int DownLeft(Vector2Int pos) => new Vector2Int(pos.x + 1, pos.y - 1);
    }
}