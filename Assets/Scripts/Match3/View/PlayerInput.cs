using System;
using Match3.Controller;
using Match3.Model;
using UnityEngine;

namespace Match3.View
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private ItemController _itemController;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                switch (_itemController.State.Value)
                {
                    case MatchThreeState.ChoosingItem when TryGetItem<Item>(out var item):
                        _itemController.PickItem(item);
                        break;
                    case MatchThreeState.DraggingItem:
                        if (TryGetItem<Piece>(out var piece))
                            _itemController.UseItem(piece);
                        else _itemController.CancelDrag();
                        break;
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (_itemController.State == MatchThreeState.DraggingItem)
                    _itemController.CancelDrag();
            }
        }

        // TODO: Raycasting shouldn't be there
        private bool TryGetItem<T>(out T item) where T : Item
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitData, 100) 
                && hitData.transform.TryGetComponent(out item))
            {
                Debug.Log($"Chosen item {item}");
                return true;
            }

            item = null;
            return false;
        }
    }
}