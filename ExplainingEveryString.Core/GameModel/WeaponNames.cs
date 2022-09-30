using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal static class WeaponNames
    {
        internal const String Default = "Default";
        internal const String Shotgun = "Shotgun";
        internal const String RocketLauncher = "RocketLauncher";
        internal const String Cone = "Cone";
        internal const String Homing = "Homing";
        internal const String FiveShot = "FiveShot";
        internal static readonly String[] AllExisting = new[] { Default, Shotgun, RocketLauncher, Cone, Homing, FiveShot };
    }
}
