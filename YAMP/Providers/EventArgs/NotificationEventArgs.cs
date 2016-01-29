using System;

namespace YAMP
{
    /// <summary>
    /// This class is used to transmit notifications in interactive mode.
    /// </summary>
    public class NotificationEventArgs : EventArgs
    {
        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="type">The type of notification.</param>
        /// <param name="message">The pure notification message.</param>
        public NotificationEventArgs(NotificationType type, string message)
        {
            Type = type;
            Message = message;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the notification type.
        /// </summary>
        public NotificationType Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the message delivered with the variable.
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets additional details covered by this notification.
        /// </summary>
        public object Tag
        {
            get;
            set;
        }

        #endregion
    }
}
