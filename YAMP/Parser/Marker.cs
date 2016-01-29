using System;

namespace YAMP
{
    /// <summary>
    /// This is an enumeration of parse markers.
    /// </summary>
    [Flags]
    enum Marker
    {
        None = 0x0,
        Breakable = 0x1
    }
}
