using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Level
{
    public class CutsceneSpecification
    {
        public Int32 FramesCount { get; set; }
        [DefaultValue(0.5F)]
        public Single MinFrameTime { get; set; }
        [DefaultValue(5F)]
        public Single MaxFrameTime { get; set; }
    }
}
