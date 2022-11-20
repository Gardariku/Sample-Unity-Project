using System;
using Match3.Model;
using UnityEngine;

namespace Match3.View
{
    public class BoardView : MonoBehaviour
    {
        public Transform[,] CellTransforms { get; private set; }
        
        [SerializeField] private GameBoard _gameBoard;
        [SerializeField] private Transform[] _transforms;

        public void Initialize(GameBoard board)
        {
            _gameBoard = board;
            CellTransforms = new Transform[board.Height, board.Width];

            for (int i = 0; i < board.Height; i++)
            {
                for (int j = 0; j < board.Width; j++)
                    CellTransforms[i, j] = _transforms[i * board.Width + j];
            }
        }
    }
}