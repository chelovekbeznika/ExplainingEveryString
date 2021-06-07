using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Data.Notifications
{
    public class NotificationSpecification
    {
        public String Type { get; set; }
        public String SpriteName { get; set; }
        public String SoundName { get; set; }
        public Single Duration { get; set; }
        public Int32 Priority { get; set; }
    }
}
