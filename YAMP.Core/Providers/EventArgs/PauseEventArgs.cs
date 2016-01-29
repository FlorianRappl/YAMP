using System;
using System.Threading;

namespace YAMP
{
    /// <summary>
    /// Data used for giving user prompts.
    /// </summary>
    public class PauseEventArgs : EventArgs
    {
        EventWaitHandle handle;

        /// <summary>
        /// Creates a new user input event argument.
        /// </summary>
        /// <param name="waitHandle">The wait handle where the waiting is based on.</param>
        public PauseEventArgs(EventWaitHandle waitHandle)
        {
            handle = waitHandle;
        }

        /// <summary>
        /// Continues with the given input.
        /// </summary>
        public void Continue()
        {
            handle.Set();
        }
    }
}
