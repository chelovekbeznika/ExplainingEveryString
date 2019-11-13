using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class BarrelSpecification
    {
        [DefaultValue("0.0, 0.0")]
        public Vector2 Offset { get; set; }
        [DefaultValue(0.0)]
        public Single Length { get; set; }
        [DefaultValue(0.0)]
        public Single AngleCorrection { get; set; }
        [DefaultValue(0.0)]
        public Single Accuracy { get; set; }
        public BulletSpecification Bullet { get; set; }
    }
}
