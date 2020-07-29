using ExplainingEveryString.Data.Level;
using System;

namespace ExplainingEveryString.Editor
{
    internal interface IEditable
    {
        String GetEditableType();
        PositionOnTileMap PositionTileMap { get; set; }
    }
}
