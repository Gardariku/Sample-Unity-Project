using System.Collections.Generic;
using Match3.Model;
using UnityEngine;

namespace Match3.Controller
{
    public interface ICellSelector
    {
        public List<Vector2Int> SelectCells(Vector2Int position, GameBoard board);
    }
}