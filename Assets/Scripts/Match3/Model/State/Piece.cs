using UnityEngine;

namespace Match3.Model
{
    public class Piece : CellContent
    {
        [field: SerializeField] public PieceData Data { get; private set; }
        public PieceType PieceType => Data.Type;

        public void Initialize(Vector2Int position, PieceData data, GameBoard board)
        {
            base.Initialize(position, board);
            Data = data;
        }

        public override void Apply(CellContent targetCell)
        {
            Board.MovePiece(this, targetCell);
        }
    }

    public enum PieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }
}