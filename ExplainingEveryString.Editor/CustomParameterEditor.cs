using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Editor
{

    internal interface ICustomParameterEditor : IEditorMode
    {
        string CurrentParameterValue { get; }
        string ParameterName { get; }
        void ToNextValue();
        void ToPreviousValue();
    }
}
