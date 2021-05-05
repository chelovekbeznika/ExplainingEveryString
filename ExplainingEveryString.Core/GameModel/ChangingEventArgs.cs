using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ChangingEventArgs : EventArgs
    {
        internal ChangingEventSpecification Specification { get; set; }
    }
}