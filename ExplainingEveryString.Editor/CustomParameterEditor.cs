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
