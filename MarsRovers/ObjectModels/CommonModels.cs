using System.ComponentModel;

namespace MarsRovers.ObjectModels
{
    public enum CompassDirection : byte
    {
        [Description("North")]
        N = 1,

        [Description("South")]
        S = 2,

        [Description("East")]
        E = 3,

        [Description("West")]
        W = 4
    }
    public enum NavigationDirection : byte
    {
        [Description("Turn Left")]
        L = 1,

        [Description("Turn Right")]
        R = 2,

        [Description("Move Forward")]
        M = 3
    }
}
