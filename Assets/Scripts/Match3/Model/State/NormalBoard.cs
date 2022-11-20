using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Match3.Model
{
    public class NormalBoard : GameBoard, IPointerDownHandler
    {
        [SerializeField] private Tilemap _tilemap;
        //[SerializeField] private BoardView _boardView;

        public void Initialize(FieldData data)
        {
            Cells = new CellContent[data.Height, data.Width];
            //_boardView.Initialize(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("Click on field detected");
            Vector2 screenPos = eventData.position;
            Vector3 worldPos = eventData.pointerCurrentRaycast.worldPosition;
            Vector3Int coord = _tilemap.WorldToCell(worldPos);
            //Debug.Log(coord);
        }

        public override bool CellExists(Vector2Int position)
        {
            return Cells.GetLength(0) > position.x && position.x >= 0 &&
                   Cells.GetLength(1) > position.y && position.y >= 0;
        }
    }
}