using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Level
{
    public class DoorStartInfo : ActorStartInfo
    {
        [DefaultValue(null)]
        public Int32? ClosesAt { get; set; }
    }
}
