using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Level
{
    public class DoorStartInfo : ActorStartInfo
    {
        [DefaultValue(null)]
        public Int32? ClosesAt { get; set; }
    }
}
