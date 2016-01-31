namespace YAMP.Sensors
{
    using System;

    abstract class SensorFunction : ArgumentFunction
    {
        EventHandler<Object> readingChanged;
        Int32 readingChangedHandlers;

        /// <summary>
        /// Notification that the reading of the sending SensorFunction had changed
        /// to get the new values, regular calls to the Function with the chosen modifiers have to be used
        /// </summary>
        public event EventHandler<Object> ReadingChanged
        {
            add
            {
                if (readingChangedHandlers == 0)
                {
                    InstallReadingChangedHandler();
                }

                readingChanged += value;
                readingChangedHandlers++;
            }
            remove
            {
                readingChanged -= value;
                readingChangedHandlers--;

                if (readingChangedHandlers == 0)
                {
                    UninstallReadingChangedHandler();
                }
            }
        }

        protected abstract void InstallReadingChangedHandler();

        protected abstract void UninstallReadingChangedHandler();

        protected void RaiseReadingChanged(Object parameter)
        {
            if (readingChanged != null)
            {
                readingChanged(this, parameter);
            }
        }
    }
}
