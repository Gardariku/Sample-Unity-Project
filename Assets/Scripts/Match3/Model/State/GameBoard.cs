using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Cysharp.Threading.Tasks;
using Match3.Controller;
using Match3.View;
using UnityEngine;

namespace Match3.Model
{
    //TODO: Is this abstraction really needed? Can't imagine alternative field concept at the moment
    public abstract class GameBoard : MonoBehaviour
    {
        public event Action<IEnumerable<CellContent>> ContentAdded;
        public event Action<IEnumerable<CellContent>> ContentRemoved;
        public event Action<IEnumerable<CellMove>> ContentMoved;
        public event Action<CellContent, CellContent> MoveFailed;

        public CellContent[,] Cells { get; protected set; }
        public int Height => Cells.GetLength(0);
        public int Width => Cells.GetLength(1);
        
        [SerializeField] private ItemSpawner _itemSpawner;
        [SerializeField] private BoardView _boardView;
        private MatchChecker _matchChecker;
        HashSet<CellMove> _movedCells = new HashSet<CellMove>();

        private void Awake()
        {
            Cells = new CellContent[8, 8];
            Utility.Populate2DArray(Cells, CellContent.Empty);

            _matchChecker = new MatchChecker(this);
            _boardView.Initialize(this);
        }

        private void Start()
        {
            Debug.Log(FindObjectOfType<BoardView>());
            _itemSpawner.Initialize(this, FindObjectOfType<BoardView>());
            LateStart().Forget();
        }

        private async UniTaskVoid LateStart()
        {
            await UniTask.WaitForEndOfFrame(this);
            FillEmptyCells(true);
        }

        public void AddCellContent(CellContent content, Vector2Int position)
        {
            if (Cells[position.x, position.y].ContentType != CellContentType.None)
                throw new ArgumentException($"Playing field position {position} is already taken.");

            Cells[position.x, position.y] = content;
            //ContentAdded?.Invoke(content);
        }

        public void MovePiece(CellContent initiator, CellContent target)
        {
            SwapCells(initiator, target);
            ContentMoved?.Invoke(new []{new CellMove(initiator, initiator.Position), new CellMove(target, target.Position)});
            CellContent[] matches = _matchChecker.CheckCardinal(initiator, target);
            Debug.Log($"Detected matches: {matches.Length}");
            if (matches.Length > 0)
            {
                Debug.Log($"Moving {initiator} on {target}");
                RemoveMatches(matches);
            }
            else
            {
                SwapCells(initiator, target);
                ContentMoved?.Invoke(new []{new CellMove(initiator, initiator.Position), new CellMove(target, target.Position)});
                MoveFailed?.Invoke(initiator, target);
            }
        }

        private void RemoveMatches(CellContent[] matches)
        {
            _movedCells.Clear();
            ContentRemoved?.Invoke(matches);
            
            foreach (var cell in matches)
            {
                int x = cell.Position.x, y = cell.Position.y;
                cell.Remove();
                Cells[x, y] = CellContent.Empty;

                for (int i = x; i > 0; i--)
                {
                    Cells[i, y] = Cells[i - 1, y];
                    Cells[i - 1, y] = CellContent.Empty;
                    Cells[i, y].Move(new Vector2Int(i, y));
                    _movedCells.Remove(new CellMove(Cells[i, y], new Vector2Int(i, y)));
                    _movedCells.Add(new CellMove(Cells[i, y], new Vector2Int(i, y)));
                }
            }
            ContentMoved?.Invoke(_movedCells);
            
            FillEmptyCells();
        }

        //TODO: how to fix this list duplication?
        private void FillEmptyCells(bool firstTime = false)
        {
            var moves = new HashSet<CellMove>();
            for (int i = Cells.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = Cells.GetLength(1) - 1; j >= 0; j--)
                {
                    if (Cells[i, j] == CellContent.Empty)
                    {
                        var pos = new Vector2Int(i, j);
                        Cells[i, j] = _itemSpawner.SpawnContent(pos, firstTime);
                        Cells[i, j].Move(pos);
                        moves.Add(new CellMove(Cells[i, j], pos));
                    }
                }
            }
            ContentMoved?.Invoke(moves);
            moves.UnionWith(_movedCells);
            var matches = _matchChecker.CheckCardinal(moves);
            if (matches.Length > 0) RemoveMatches(matches);
        }
        
        private void SwapCells(CellContent cell1, CellContent cell2)
        {
            var temp = cell1.Position;
            cell1.Move(cell2.Position);
            cell2.Move(temp);

            Cells[temp.x, temp.y] = cell2;
            Cells[cell1.Position.x, cell1.Position.y] = cell1;
        }

        public CellContent GetCellAt(Vector2Int position) => Cells[position.x, position.y];

        public abstract bool CellExists(Vector2Int position);
    }

    public struct CellMove
    {
        public CellContent Cell { get; }
        public Vector2Int Position { get; }

        // TODO: think twice about should this be a structure or not
        public CellMove(CellContent cell, Vector2Int position)
        {
            Cell = cell;
            Position = position;
        }
        
        public override bool Equals(object obj) 
        {
            if (!(obj is CellMove other))
                return false;

            return Cell == other.Cell;
        }

        public override int GetHashCode()
        {
            return Cell.GetHashCode();
        }
    }
}