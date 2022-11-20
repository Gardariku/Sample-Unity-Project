using System;
using DG.Tweening;
using Match3.Controller;
using Match3.Model;
using UnityEngine;

namespace Match3.View
{
    public class PieceView : MonoBehaviour
    {
        public Piece Piece { get; private set; }

        [field: SerializeField] public Collider Collider { get; private set; }
        [SerializeField] [Min(0f)] private float _moveTime = 0.7f;
        [SerializeField] [Min(0f)] private float _removeTime = 0.5f;
        private ItemController _controller;

        public void Initialize(Piece piece, ItemController itemController)
        {
            Piece = piece;
            _controller = itemController;
            
            piece.Moved += OnMoved;
            piece.Removed += OnRemoved;
        }

        private void OnRemoved()
        {
            _controller.Animations++;
            transform.DOScale(0f, _removeTime).OnComplete(() =>
            {
                _controller.Animations--;
                Destroy(this);
            });
        }

        private void OnDestroy()
        {
            Piece.Moved -= OnMoved;
            Piece.Removed -= OnRemoved;
        }

        private void OnMoved(Vector2Int movePosition)
        {
            _controller.Animations++;
            transform.position = new Vector3(transform.position.x, transform.position.y, -9f);
            transform.DOMove(_controller.BoardView.CellTransforms[movePosition.x, movePosition.y].transform.position, _moveTime);
            transform.DOScale(Vector3.one, _moveTime).OnComplete(() =>
            {
                _controller.Animations--;
                Collider.enabled = true;
            });
        }
    }
}