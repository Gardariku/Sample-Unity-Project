using System;
using UnityEngine;

namespace Match3.Model
{
    // This abstraction is supposed to generalize any object, that theoretically can occupy a cell
    // Such as figure, obstacle, bomb or smth else
    public abstract class CellContent : Item
    {
        public event Action<Vector2Int> Moved;
        public event Action Removed;
        
        public static CellContent Empty { get; }
        static CellContent()
        {
            Empty = new EmptyCell();
            //Empty = Instantiate(new GameObject()).AddComponent<EmptyCell>();
        }
        
        public GameBoard Board { get; private set; }
        [field: SerializeField] public CellContentType ContentType { get; protected set; }
        [field: SerializeField] public Vector2Int Position { get; protected set; }
        [field: SerializeField] public bool IsRemoved { get; private set; }
        private static readonly Vector2Int OutPosition = new Vector2Int(-1, -1);

        protected void Initialize(Vector2Int position, GameBoard board)
        {
            Position = position;
            Board = board;
        }

        // TODO: Consider baking cell array link change in board into this method
        public void Move(Vector2Int newPosition)
        {
            Position = newPosition;
        }
        
        public void Remove()
        {
            Position = OutPosition;
            IsRemoved = true;
        }

        public void InvokeMoved(Vector2Int movePosition) => Moved?.Invoke(movePosition);
        public void InvokeRemoved() => Removed?.Invoke();
    }

    // TODO: is this excessive? All these types are gonna have its own class
    public enum CellContentType
    {
        None,
        Piece,
        Block,
        Object
    }
}