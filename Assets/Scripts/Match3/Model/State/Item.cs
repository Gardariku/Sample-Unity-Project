using System.Collections.Generic;
using Match3.Controller;
using UnityEngine;

namespace Match3.Model
{
    // TODO: Maybe this actually shouldn't be Monobeh
    public abstract class Item : MonoBehaviour
    {
        public List<ICellSelector> Selectors { get; private set; }
        
        public abstract void Apply(CellContent targetCell);
    }
}