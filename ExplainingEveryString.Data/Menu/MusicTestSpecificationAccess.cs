using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Menu
{
    public static class MusicTestSpecificationAccess
    {
        public static MusicTestButtonSpecification[] Load()
        {
            return JsonDataAccessor.Instance.Load<MusicTestButtonSpecification[]>(FileNames.MusicTestMenu);
        }
    }
}
