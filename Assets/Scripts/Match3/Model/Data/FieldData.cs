using UnityEngine;

namespace Match3.Model
{
    [CreateAssetMenu(fileName = "NewField", menuName = "Match3/Field", order = 0)]
    public class FieldData : ScriptableObject
    {
        [field: Range(1, 10)]
        [field: SerializeField] public int Height { get; private set; }
        [field: Range(1, 10)]
        [field: SerializeField] public int Width { get; private set; }
    }
}