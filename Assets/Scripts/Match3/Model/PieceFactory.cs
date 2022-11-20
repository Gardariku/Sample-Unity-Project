using System;
using Match3.Controller;
using Match3.View;
using UnityEngine;

namespace Match3.Model
{
    public class PieceFactory : MonoBehaviour
    {
        [SerializeField] private GameBoard _board;
        [SerializeField] private ItemController _itemController;
        [SerializeField] private PieceView[] _piecePrefabs;
        [SerializeField] private PieceTypeData[] _pieces;
        
        public Piece Create(Vector2Int position, PieceType pieceType)
        {
            var pieceView = Instantiate(_piecePrefabs[(int)pieceType]);
            //var piece = pieceView.gameObject.AddComponent<Piece>();
            var piece = pieceView.GetComponent<Piece>();
            piece.Initialize(position, GetPieceData(pieceType), _board);
            pieceView.Initialize(piece, _itemController);

            return piece;
        }

        private PieceData GetPieceData(PieceType type)
        {
            foreach (var piece in _pieces)
            {
                if (piece.Type == type)
                    return piece.Data;
            }

            throw new ArgumentException($"Cannot find PieceData for {type} PieceType");
        }
        
        [Serializable]
        private struct PieceTypeData
        {
            [field: SerializeField] public PieceType Type { get; private set; }
            [field: SerializeField] public PieceData Data { get; private set; }
        }
    }
}