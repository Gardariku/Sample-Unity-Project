using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Model;
using UnityEngine;
//using Test = System.Func<UnityEngine.Vector2Int, UnityEngine.Vector2Int>;

namespace Match3.Controller
{
    public class MatchChecker
    {
        private GameBoard _board;

        private CellContent _currentCell;
        private CellContent _targetCell;
        private HashSet<CellContent> _matches;
        private HashSet<CellContent> _axisMatches;

        private int matchNumber = 3;
        private static readonly (Func<Vector2Int, Vector2Int>, Func<Vector2Int, Vector2Int>) Horizontal = (Right, Left);
        private static readonly (Func<Vector2Int, Vector2Int>, Func<Vector2Int, Vector2Int>) Vertical = (Up, Down);
        

        public MatchChecker(GameBoard board)
        {
            _board = board;
            _matches = new HashSet<CellContent>();
            _axisMatches = new HashSet<CellContent>();
        }
        
        public CellContent[] CheckCardinal(CellContent content)
        {
            _matches.Clear();
            bool matched = CheckAxis(content, Horizontal) ||
                           CheckAxis(content, Vertical);
            
            return matched ? _matches.ToArray() : Array.Empty<CellContent>();
        }
        
        public CellContent[] CheckCardinal(CellContent content1, CellContent content2)
        {
            _matches.Clear();
            bool matched = CheckAxis(content1, Horizontal); 
            matched = CheckAxis(content1, Vertical) || matched;
            matched = CheckAxis(content2, Horizontal) || matched; 
            matched = CheckAxis(content2, Vertical) || matched;

            return matched ? _matches.ToArray() : Array.Empty<CellContent>();
        }
        
        public CellContent[] CheckCardinal(IEnumerable<CellContent> contents)
        {
            _matches.Clear();
            bool matched = false;

            foreach (var content in contents)
            {
                matched = CheckAxis(content, Horizontal) || matched;
                matched = CheckAxis(content, Vertical) || matched;
            }
            
            return matched ? _matches.ToArray() : Array.Empty<CellContent>();
        }

        public CellContent[] CheckCardinal(IEnumerable<CellMove> contents)
        {
            var cells = new List<CellContent>();
            foreach (var move in contents)
                cells.Add(move.Cell);
            return CheckCardinal(cells);
        }

        private bool CheckAxis(CellContent content, 
            (Func<Vector2Int, Vector2Int>, Func<Vector2Int, Vector2Int>) directions)
        {
            _axisMatches.Clear();
            _axisMatches.Add(content);
            if (content is Piece piece)
            {
                CheckDirection(piece, directions.Item1);
                CheckDirection(piece, directions.Item2);
            }

            if (_axisMatches.Count < matchNumber)
                return false;

            _matches.UnionWith(_axisMatches);
            return true;
        }

        private void CheckDirection(Piece piece, Func<Vector2Int, Vector2Int> nextCell)
        {
            var nextPosition = nextCell(piece.Position);
            while (_board.CellExists(nextPosition))
            {
                if (_board.GetCellAt(nextPosition) is Piece nextPiece && nextPiece.Data.Type == piece.Data.Type)
                    _axisMatches.Add(nextPiece);
                else
                    break;
                nextPosition = nextCell(nextPosition);
            }
        }

        private static Vector2Int Up(Vector2Int position) => new Vector2Int(position.x - 1, position.y);
        private static Vector2Int Down(Vector2Int position) => new Vector2Int(position.x + 1, position.y);
        private static Vector2Int Right(Vector2Int position) => new Vector2Int(position.x, position.y + 1);
        private static Vector2Int Left(Vector2Int position) => new Vector2Int(position.x, position.y - 1);
    }
}