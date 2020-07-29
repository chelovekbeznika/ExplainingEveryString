using System;

namespace ExplainingEveryString.Editor
{
    internal static class EditingHelper
    {
        public static Int32? SelectedEditableChange(Int32 editablesSwitched, Int32 editablesCount, Int32? currentIndex)
        {
            if (editablesCount == 0)
                return currentIndex;

            if (currentIndex == null)
            {
                if (editablesSwitched > 0)
                    currentIndex = -1;
                else
                    currentIndex = editablesCount;
            }

            currentIndex += editablesSwitched;
            if (currentIndex < 0)
                currentIndex += editablesCount;
            if (currentIndex >= editablesCount)
                currentIndex -= editablesCount;

            return currentIndex;
        }
    }
}
