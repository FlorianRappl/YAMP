using System;
using System.Threading;

namespace YAMP
{
    /// <summary>
    /// Data used for giving user prompts.
    /// </summary>
    public class UserInputEventArgs : EventArgs
    {
        EventWaitHandle handle;

        /// <summary>
        /// Creates a new user input event argument.
        /// </summary>
        /// <param name="waitHandle">The wait handle where the waiting is based on.</param>
        /// <param name="message">The message to show.</param>
        public UserInputEventArgs(EventWaitHandle waitHandle, string message)
        {
            handle = waitHandle;
            Message = message;
        }

        /// <summary>
        /// Gets the associated message.
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the input.
        /// </summary>
        public string Input
        {
            get;
            private set;
        }

        /// <summary>
        /// Continues with the given input.
        /// </summary>
        /// <param name="input">The user's input.</param>
        public void Continue(string input)
        {
            Input = input;
            handle.Set();
        }
    }
}
