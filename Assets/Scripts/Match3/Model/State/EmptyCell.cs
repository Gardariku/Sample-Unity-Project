using System;
using UnityEngine;

namespace Match3.Model
{
    // TODO: Should this inherit CellContent? It cannot be Applied
    public class EmptyCell : CellContent
    {
        public EmptyCell()
        {
            ContentType = CellContentType.None;
        }

        public override void Apply(CellContent content)
        {
            Debug.LogWarning("Moving an empty cell");
        }
    }
}