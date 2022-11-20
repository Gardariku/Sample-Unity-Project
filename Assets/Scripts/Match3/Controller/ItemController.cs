using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Cysharp.Threading.Tasks;
using Match3.Model;
using Match3.View;
using ModestTree;
using UnityEngine;

namespace Match3.Controller
{
    public class ItemController : MonoBehaviour
    {
        [field: SerializeField] public GameBoard Board { get; private set; }
        [field: SerializeField] public BoardView BoardView { get; private set; }
        [SerializeField] private ItemDragger _itemDragger;
        [SerializeField] private CellHighlighter _cellHighlighter;
        
        [field: SerializeField] public Observable<MatchThreeState> State { get; private set; }
        [SerializeField] private int _animations;
        public int Animations
        {
            get => _animations;
            set => _animations = Mathf.Max(0, value);
        }

        private Item _currentItem;
        private HashSet<Vector2Int> _possiblePositions = new HashSet<Vector2Int>();
        private Queue<ItemCommand> _actionQueue = new Queue<ItemCommand>();

        private void Awake()
        {
            State = new Observable<MatchThreeState>();
        }

        private void Start()
        {
            _itemDragger.DragCanceled += ItemDraggerOnDragCanceled;
            Board.ContentRemoved += OnContentRemoved;
            Board.ContentMoved += OnContentMoved;
        }

        private void Update()
        {
            if (State != MatchThreeState.ChoosingItem || _actionQueue.IsEmpty()) return;
            var nextAction = _actionQueue.Dequeue();
            switch (nextAction.Command)
            {
                case MatchThreeState.MovingCells:
                    StartMovingContent(nextAction.Cells).Forget();
                    break;
                case MatchThreeState.RemovingCells:
                    StartRemovingContent(nextAction.Cells).Forget();
                    break;
                default:
                    Debug.Log($"{nextAction.Command} isn't supposed to be in the action queue.");
                    break;
            }
        }

        private void OnContentMoved(IEnumerable<CellMove> cells)
        {
            var cellMoves = cells.ToArray();
            _actionQueue.Enqueue(new ItemCommand(cellMoves, MatchThreeState.MovingCells));
        }
        // TODO: Is awaiting event somehow(Observable.Changed) a good idea here?
        // Or just think about how to reduce boilerplate, mb implementing state machine
        private async UniTaskVoid StartMovingContent(IEnumerable<CellMove> cells)
        {
            State = MatchThreeState.MovingCells;
            foreach (var move in cells)
            {
                move.Cell.InvokeMoved(move.Position);
            }
            
            while (_animations > 0)
                await UniTask.WaitForEndOfFrame(this);
            
            State = MatchThreeState.ChoosingItem;
        }

        private void OnContentRemoved(IEnumerable<CellContent> cells)
        {
            var moves = new List<CellMove>();
            foreach (var cell in cells)
                moves.Add(new CellMove(cell, new Vector2Int(-1, -1)));
            _actionQueue.Enqueue(new ItemCommand(moves, MatchThreeState.RemovingCells));
        }
        private async UniTaskVoid StartRemovingContent(IEnumerable<CellMove> moves)
        {
            State = MatchThreeState.RemovingCells;
            foreach (var move in moves)
            {
                move.Cell.InvokeRemoved();
            }
            
            while (_animations > 0)
                await UniTask.WaitForEndOfFrame(this);
            
            State = MatchThreeState.ChoosingItem;
        }

        private void ItemDraggerOnDragCanceled()
        {
            if (State == MatchThreeState.DraggingItem)
                State = MatchThreeState.ChoosingItem;
            else Debug.LogError($"Drag canceled during controller state: {State.Value}");
        }

        public void PickItem(Item item)
        {
            _possiblePositions.Clear();
            if (item is CellContent cellContent)
            {
                foreach (var cellSelector in item.GetComponents<ICellSelector>())
                    _possiblePositions.UnionWith(cellSelector.SelectCells(cellContent.Position, Board));
            }
            else
            {
                foreach (var cellSelector in item.GetComponents<ICellSelector>())
                    _possiblePositions.UnionWith(cellSelector.SelectCells(-Vector2Int.one, Board));
            }

            if (_possiblePositions.Count == 0)
                return;

            State = MatchThreeState.DraggingItem;
            _currentItem = item;
            _cellHighlighter.ShowCells(_possiblePositions);
            _itemDragger.StartDragging(_currentItem);
        }
        
        public void UseItem(Piece targetItem)
        {
            Debug.Log($"Tried using item {_currentItem} on {targetItem}");
            if (_possiblePositions.Contains(targetItem.Position))
            {
                State = MatchThreeState.ChoosingItem;
                _itemDragger.Drop(_currentItem);
                _currentItem.Apply(targetItem);
            }
            else
                CancelDrag();
        }
        
        public void CancelDrag()
        {
            _itemDragger.Cancel();
            _currentItem = null;
        }
    }

    public struct ItemCommand
    {
        public IEnumerable<CellMove> Cells { get; }
        public MatchThreeState Command { get; }

        public ItemCommand(IEnumerable<CellMove> cells, MatchThreeState command)
        {
            Cells = cells;
            Command = command;
        }
    }
    
    public enum MatchThreeState
    {
        ChoosingItem,
        DraggingItem,
        MovingCells,
        RemovingCells,
        Pause
    }
}