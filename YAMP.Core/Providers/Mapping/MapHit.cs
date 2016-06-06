namespace YAMP
{
    /// <summary>
    /// Enumeration over possible mapping states.
    /// </summary>
    public enum MapHit
    {
        /// <summary>
        /// There is no mapping.
        /// </summary>
        Miss,
        /// <summary>
        /// The mapping is directly fulfilled.
        /// </summary>
        Direct,
        /// <summary>
        /// The mapping is indirectly fulfilled (e.g., cast required).
        /// </summary>
        Indirect
    }
}
