using System.Collections.Generic;
using Match3.Controller;
using UnityEngine;

namespace Match3.Model
{
    // Most of items are CellContents, but there can be smth like disposable on-use items later
    // TODO: Maybe this actually shouldn't be Monobeh
    public abstract class Item : MonoBehaviour
    {
        public List<ICellSelector> Selectors { get; private set; }
        
        public abstract void Apply(CellContent targetCell);
    }
}