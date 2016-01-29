using System;

namespace YAMP
{
    /// <summary>
    /// Classifies the various types of notifications.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Indicates that some (non-exceptional) error occured.
        /// </summary>
        Failure,
        /// <summary>
        /// Indicates that some operation did run successfully.
        /// </summary>
        Success,
        /// <summary>
        /// Just as a pure information point.
        /// </summary>
        Information,
        /// <summary>
        /// Should be displayed like a message (used by printf).
        /// </summary>
        Message
    }
}
