using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.Model;
using Match3.View;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Controller
{
    //TODO: Perhaps there should be a single object at the scene responsible for this behaviour, instead of multiple components?
    [RequireComponent(typeof(ICellSelector))]
    public class ItemDragger : MonoBehaviour
    {
        public event Action DragStarted;
        public event Action DragCanceled;
        
        [SerializeField] private GameBoard _board;
        [SerializeField] private float _highlightMoveHeight = -5f;
        [SerializeField] private float _highlightMoveScale = 1.5f;
        [SerializeField] private float _highlightMoveTime = 0.5f;

        //TODO: Consider just replacing Piece with PieceView (and maybe removing Monobeh from Items)
        private Item _item;
        private PieceView _currentView;
        private bool _isDragged;
        private Vector3 _startPosition;
        private float _startScale;

        public void StartDragging(Item item)
        {
            Debug.Log($"Started dragging {item}");
            _item = item;
            _currentView = item.GetComponent<PieceView>();
            _isDragged = true;
            _startPosition = _item.transform.position;
            _startScale = _item.transform.localScale.x;
            _currentView.Collider.enabled = false;
            
            _item.transform.DOScale(_highlightMoveScale, _highlightMoveTime).SetEase(Ease.OutBack);

            DragStarted?.Invoke();
            Drag().Forget();
        }

        //TODO: Cancellation token isn't needed here, riiiight?
        private async UniTaskVoid Drag()
        {
            var mainCamera = Camera.main;
            while (_isDragged)
            {
                //Debug.Log($"Dragging {_item}");
                
                // if (Input.touchCount == 0)
                //     Debug.LogError("No touches were detected during drag task");
                // transform.position = mainCamera.ScreenToWorldPoint(Input.touches[0].position);
                
                Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                _item.transform.position = new Vector3(position.x, position.y, _highlightMoveHeight);

                await UniTask.WaitForEndOfFrame(this);
            }
        }
        
        //TODO: Hhmmmmm
        public void Drop(Item item)
        {
            Debug.Log($"Dropped {item}");
            _isDragged = false;
            _currentView = null;
            _item = null;
        }

        public void Cancel()
        {
            Debug.Log($"Stopped dragging {_item}");
            _isDragged = false;
            _currentView.Collider.enabled = true;

            _item.transform.DOMove(_startPosition, _highlightMoveTime).SetEase(Ease.OutCubic);
            _item.transform.DOScale(_startScale, _highlightMoveTime).SetEase(Ease.OutBack)
                .onComplete += () => DragCanceled?.Invoke();
            _currentView = null;
            _item = null;
        }
        
        private List<Vector2> GetSuitableCells()
        {
            throw new NotImplementedException();
        }
    }
}