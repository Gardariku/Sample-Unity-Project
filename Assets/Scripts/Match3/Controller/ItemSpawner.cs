using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Model;
using Match3.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3.Controller
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private GameBoard _board;
        [SerializeField] private BoardView _boardView;
        [SerializeField] private Transform[] _spawnPoints;
        
        [Header("Factories")]
        [SerializeField] private PieceFactory _pieceFactory;
        private HashSet<PieceType> _pieceTypes = new HashSet<PieceType>();

        private int _totalPieces;

        public void Initialize(GameBoard board, BoardView boardView)
        {
            _pieceTypes.UnionWith((PieceType[])Enum.GetValues(typeof(PieceType)));
            _board = board;
            _boardView = boardView;
            _totalPieces = Enum.GetNames(typeof(PieceType)).Length;

            //FillBoard();
        }

        private void FillBoard()
        {
            foreach (var cell in _board.Cells)
            {
                SpawnContent(cell.Position);
            }
        }

        public CellContent SpawnContent(Vector2Int position, bool trackMatches = false)
        {
            Piece newPiece = _pieceFactory.Create(position, 
                trackMatches ? GetCorrectPiece(position) : GetRandomPiece());
            newPiece.transform.position = _spawnPoints[position.y].position;
            newPiece.transform.SetParent(_boardView.CellTransforms[position.x, position.y]);
            _board.Cells[position.x, position.y] = newPiece;

            return newPiece;
        }

        // TODO: refactor this boilerplate
        private PieceType GetCorrectPiece(Vector2Int position)
        {
            var excludedTypes = new HashSet<PieceType>();
            var cells = _board.Cells;
            if (_board.CellExists(new Vector2Int(position.x, position.y - 1))
                && _board.CellExists(new Vector2Int(position.x, position.y - 2))
                && cells[position.x, position.y - 1] is Piece piece111
                && cells[position.x, position.y - 2] is Piece piece112
                && piece111.PieceType == piece112.PieceType)
                excludedTypes.Add(piece111.PieceType);
            if (_board.CellExists(new Vector2Int(position.x, position.y + 1))
                && _board.CellExists(new Vector2Int(position.x, position.y + 2))
                && cells[position.x, position.y + 1] is Piece piece121
                && cells[position.x, position.y + 2] is Piece piece122
                && piece121.PieceType == piece122.PieceType)
                excludedTypes.Add(piece121.PieceType);
            if (_board.CellExists(new Vector2Int(position.x - 1, position.y))
                && _board.CellExists(new Vector2Int(position.x - 2, position.y))
                && cells[position.x - 1, position.y] is Piece piece211
                && cells[position.x - 2, position.y] is Piece piece212
                && piece211.PieceType == piece212.PieceType)
                excludedTypes.Add(piece211.PieceType);
            if (_board.CellExists(new Vector2Int(position.x + 1, position.y))
                && _board.CellExists(new Vector2Int(position.x + 2, position.y))
                && cells[position.x + 1, position.y] is Piece piece221
                && cells[position.x + 2, position.y] is Piece piece222
                && piece221.PieceType == piece222.PieceType)
                excludedTypes.Add(piece221.PieceType);

            var correctTypes = new HashSet<PieceType>(_pieceTypes);
            correctTypes.ExceptWith(excludedTypes);

            return correctTypes.ElementAt(Random.Range(0, correctTypes.Count));
        }

        private void SpawnPiece(Vector2Int position, PieceType pieceType)
        {
            var piece = _pieceFactory.Create(position, pieceType);
            piece.transform.position = _spawnPoints[position.y].position;
            piece.transform.SetParent(_boardView.CellTransforms[piece.Position.x, piece.Position.y]);
        }

        private PieceType GetRandomPiece() => (PieceType) Random.Range(0, _totalPieces);
    }
}