using UnityEngine;

namespace Match3.Model
{
    [CreateAssetMenu(fileName = "NewFigure", menuName = "Match3/Figure", order = 0)]
    public class PieceData : ScriptableObject
    {
        [field: SerializeField] public PieceType Type { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: TextArea]
        [field: SerializeField] public string Description { get; private set; }
    }
}