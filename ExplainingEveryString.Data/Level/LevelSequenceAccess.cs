using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Level
{
    public static class LevelSequenceAccess
    {
        public static String[] LoadLevelSequence()
        {
            return JsonDataAccessor.Instance.Load<String[]>(FileNames.LevelSequence);
        }
    }
}
