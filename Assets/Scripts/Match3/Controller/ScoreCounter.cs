using System;
using Match3.Model;
using UnityEngine;

namespace Match3.Controller
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private GameBoard _board;
        [SerializeField] private int _matchScore;
        [SerializeField] private int _comboBonus;
        
        private int _combo;

        private void Start()
        {
            //_board.RemoveStarted += OnRemoveStarted;
            //_board.RemoveEnded += OnRemoveEnded;
            //_board.ContentRemoved += OnContentRemoved;
        }

        private void OnContentRemoved(CellContent content)
        {
            throw new NotImplementedException();
        }

        private void OnRemoveStarted()
        {
            throw new NotImplementedException();
        }

        private void OnRemoveEnded()
        {
            throw new NotImplementedException();
        }
    }
}